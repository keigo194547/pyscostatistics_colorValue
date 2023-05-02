using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // �ǉ�

public class ColorController : MonoBehaviour
{
    [SerializeField] private Image square1;
    [SerializeField] private Image square2;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider saturationSlider;
    [SerializeField] private TextMeshProUGUI brightnessText; // �ύX
    [SerializeField] private TextMeshProUGUI saturationText; // �ύX

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
        // square2.color = AdjustColor(square2InitialColor, brightness, saturation); // ���̍s���R�����g�A�E�g

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
