using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class RandomColorController : MonoBehaviour
{

    // UI elements and variables
    [SerializeField] private Image redSquare;
    [SerializeField] private Button changeButton;
    [SerializeField] private TextMeshProUGUI brightnessOutput;
    [SerializeField] private TextMeshProUGUI finishText;
    [SerializeField] private int setCount = 10;

    // Brightness values and shuffled brightness values
    private List<int> brightnessValues = new List<int> {51, 76, 102, 128, 153, 179, 204 };
    private List<int> shuffledBrightnessValues;
    private Color redSquareInitialColor;

    // loop control variables
    private int loopCount = 0;
    private int decideButtonClickCount = 0;


    void Start()
    {

        redSquareInitialColor = redSquare.color;

        // Create a list of repeated brightness values
        List<int> repeatedValues = new List<int> { };
        for (int i = 0; i < setCount; i++)
        {
            repeatedValues.AddRange(brightnessValues);
        }

        // Shuffle the brightness values
        shuffledBrightnessValues = new List<int>(repeatedValues);
        System.Random random = new System.Random();
        int n = shuffledBrightnessValues.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            int value = shuffledBrightnessValues[k];
            shuffledBrightnessValues[k] = shuffledBrightnessValues[n];
            shuffledBrightnessValues[n] = value;
        }


        // Debug output
        Debug.Log("Shuffled Brightness Values Count: " + shuffledBrightnessValues.Count);
        Debug.Log(string.Join(",", shuffledBrightnessValues.Select(n => n.ToString())));

        finishText.gameObject.SetActive(false);
    }


    // Change the brightness of the red square and when click system
    public void ChangeBrightness()
    {
        float brightness = shuffledBrightnessValues[loopCount] / 256f;
        redSquare.color = AdjustColor(redSquareInitialColor, brightness);

        loopCount++;
        print(loopCount);
    }



    // Record the selected brightness and update loop variables
    public void DecideBrightness(string tag)
    {
        int buttonType = tag == "up" ? 0 : 1;
        brightnessOutput.text += $"{buttonType},{redSquare.color.ToString()}\n";
        


        decideButtonClickCount++;

        // Check if all brightness values have been decided and save data
        if (decideButtonClickCount >= shuffledBrightnessValues.Count)
        {
            finishText.gameObject.SetActive(true);
            SaveDataToCSV();
        }

        Debug.Log(loopCount);
        if (loopCount >= shuffledBrightnessValues.Count)
        {
            loopCount = 0;
        }
    }


    // Adjust the color based on the given brightness
    Color AdjustColor(Color original, float brightness)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        v = brightness;
        return Color.HSVToRGB(h, s, Mathf.Clamp01(v));
    }


    // Save brightness data to a CSV file and output
    void SaveDataToCSV()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "brightness_data.csv");
        string[] lines = brightnessOutput.text.Split('\n');

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("Button_Type,Brightness");

            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i].Replace("RGBA(", "").Replace(", 1.000)", "").Replace(" ", "");
                sw.WriteLine(line);
            }
        }
    }
}

