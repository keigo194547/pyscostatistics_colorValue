using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RandomColorController : MonoBehaviour
{
    [SerializeField] private Image redSquare;
    [SerializeField] private Button changeButton;
    [SerializeField] private TextMeshProUGUI brightnessOutput;
    [SerializeField] private TextMeshProUGUI finishText;

    private List<int> brightnessValues = new List<int> { 25, 51, 76, 102, 128, 153, 179, 204 };
    private List<int> shuffledBrightnessValues;
    private Color redSquareInitialColor;
    private int currentIndex = 0;
    private int loopCount = 0;
    private int maxLoops = 4;
    private int decideButtonClickCount = 0;

    void Start()
    {
        redSquareInitialColor = redSquare.color;
        shuffledBrightnessValues = new List<int>(brightnessValues);
        ShuffleBrightnessValues();
        finishText.gameObject.SetActive(false);
    }

    public void ChangeBrightness()
    {
        int randomIndex = Random.Range(0, brightnessValues.Count);
        float brightness = brightnessValues[randomIndex] / 256f;
        redSquare.color = AdjustColor(redSquareInitialColor, brightness);
    }

    public void DecideBrightness(string tag)
    {
        int buttonType = tag == "up" ? 0 : 1;
        brightnessOutput.text += $"{buttonType},{redSquare.color.ToString()}\n";
        loopCount++;
        currentIndex = 0;

        decideButtonClickCount++;

        if (decideButtonClickCount >= 4)
        {
            finishText.gameObject.SetActive(true);
            SaveDataToCSV();
        }

        if (loopCount >= maxLoops)
        {
            loopCount = 0;
            ShuffleBrightnessValues();
        }
    }

    void ShuffleBrightnessValues()
    {
        // Implement your logic to shuffle brightness values
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

