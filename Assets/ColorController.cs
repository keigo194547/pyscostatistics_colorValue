using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 追加

public class ColorController : MonoBehaviour
{
    [SerializeField] private Image square1;
    [SerializeField] private Image square2;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider saturationSlider;
    [SerializeField] private TextMeshProUGUI brightnessText; // 変更
    [SerializeField] private TextMeshProUGUI saturationText; // 変更

    private Color square1InitialColor;
    private Color square2InitialColor;

    void Start()
    {
        square1InitialColor = square1.color;
        square2InitialColor = square2.color;

        brightnessSlider.onValueChanged.AddListener(UpdateColors);
        saturationSlider.onValueChanged.AddListener(UpdateColors);
    }

    void UpdateColors(float value)
    {
        float brightness = brightnessSlider.value;
        float saturation = saturationSlider.value;

        square1.color = AdjustColor(square1InitialColor, brightness, saturation);
        // square2.color = AdjustColor(square2InitialColor, brightness, saturation); // この行をコメントアウト

        brightnessText.text = "Brightness: " + brightness.ToString("0.00");
        saturationText.text = "Saturation: " + saturation.ToString("0.00");
    }


    Color AdjustColor(Color original, float brightness, float saturation)
    {
        Color.RGBToHSV(original, out float h, out float s, out float v);
        s *= saturation;
        v *= brightness;
        return Color.HSVToRGB(h, Mathf.Clamp01(s), Mathf.Clamp01(v));
    }
}
