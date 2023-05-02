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
    [SerializeField] private Button decideButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private TextMeshProUGUI brightnessOutput;
    [SerializeField] private TextMeshProUGUI finishText;

    private List<int> brightnessValues = new List<int> { 0, 30, 60, 90, 120, 150, 180, 210, 240, 256 };
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

        changeButton.onClick.AddListener(ChangeBrightness);
        decideButton.onClick.AddListener(DecideBrightness);
        resetButton.onClick.AddListener(ResetBrightness);
        finishText.gameObject.SetActive(false);
    }

    void ChangeBrightness()
    {
        if (currentIndex >= shuffledBrightnessValues.Count)
        {
            currentIndex = 0;
            loopCount++;

            if (loopCount >= maxLoops)
            {
                loopCount = 0;
                ShuffleBrightnessValues();
            }
        }

        float brightness = shuffledBrightnessValues[currentIndex] / 256f;
        redSquare.color = AdjustColor(redSquareInitialColor, brightness);
        currentIndex++;
    }

    void DecideBrightness()
    {
        brightnessOutput.text += redSquare.color.ToString() + "\n";
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

    void ResetBrightness()
    {
        brightnessOutput.text = "";
        loopCount = 0;
        currentIndex = 0;
        ShuffleBrightnessValues();
        finishText.gameObject.SetActive(false);
        decideButtonClickCount = 0;
    }

    void ShuffleBrightnessValues()
    {
        for (int i = 0; i < shuffledBrightnessValues.Count; i++)
        {
            int temp = shuffledBrightnessValues[i];
            int randomIndex = Random.Range(i, shuffledBrightnessValues.Count);
            shuffledBrightnessValues[i] = shuffledBrightnessValues[randomIndex];
            shuffledBrightnessValues[randomIndex] = temp;
        }
    }

    Color AdjustColor(Color original, float brightness)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        v = brightness;
        return Color.HSVToRGB(h, s, Mathf.Clamp01(v));
    }

    void SaveDataToCSV() // ’Ç‰Á
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "brightness_data.csv");

        string[] lines = brightnessOutput.text.Split('\n');

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine("R,G,B");

            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i].Replace("RGBA(", "").Replace(", 1.000)", "").Replace(" ", "");
                sw.WriteLine(line);
            }
        }
    }

}

