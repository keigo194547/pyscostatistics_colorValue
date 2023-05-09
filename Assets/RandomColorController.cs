using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class RandomColorController : MonoBehaviour
{
    [SerializeField] private Image redSquare;
    [SerializeField] private Button changeButton;
    [SerializeField] private TextMeshProUGUI brightnessOutput;
    [SerializeField] private TextMeshProUGUI finishText;
    [SerializeField] private int setCount = 10;

    private List<int> brightnessValues = new List<int> {51, 76, 102, 128, 153, 179, 204 };
    private List<int> shuffledBrightnessValues;
    private Color redSquareInitialColor;
    private int currentIndex = 0;
    private int loopCount = 0;
    private int maxLoops = 4;
    private int decideButtonClickCount = 0;


    void Start()
    {
        redSquareInitialColor = redSquare.color;
        List<int> repeatedValues = new List<int> { };
        for (int i = 0; i < setCount; i++)
        {
            repeatedValues.AddRange(brightnessValues);
        }


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

        Debug.Log("Shuffled Brightness Values Count: " + shuffledBrightnessValues.Count);

        Debug.Log(string.Join(",", shuffledBrightnessValues.Select(n => n.ToString())));

        finishText.gameObject.SetActive(false);
    }


    public void ChangeBrightness()
    {
        int randomIndex = 0;
        float brightness = shuffledBrightnessValues[randomIndex] / 256f;
        redSquare.color = AdjustColor(redSquareInitialColor, brightness);
        randomIndex++;
        print(randomIndex);
    }

    public void DecideBrightness(string tag)
    {
        int buttonType = tag == "up" ? 0 : 1;
        brightnessOutput.text += $"{buttonType},{redSquare.color.ToString()}\n";
        loopCount++;
        currentIndex = 0;

        decideButtonClickCount++;

        if (decideButtonClickCount >= shuffledBrightnessValues.Count)
        {
            finishText.gameObject.SetActive(true);
            SaveDataToCSV();
        }

        if (loopCount >= shuffledBrightnessValues.Count)
        {
            loopCount = 0;
        }
    }

   

    Color AdjustColor(Color original, float brightness)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        v = brightness;
        return Color.HSVToRGB(h, s, Mathf.Clamp01(v));
    }

    void SaveDataToCSV()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "brightness_data.csv");

        string[] lines = brightnessOutput.text.Split('\n');

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("Button_Type,R,G,B");

            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i].Replace("RGBA(", "").Replace(", 1.000)", "").Replace(" ", "");
                sw.WriteLine(line);
            }
        }
    }
}

