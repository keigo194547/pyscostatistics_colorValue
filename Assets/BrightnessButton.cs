using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BrightnessButton : MonoBehaviour
{
    private Button button;
    private RandomColorController randomColorController;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        randomColorController = FindObjectOfType<RandomColorController>();
        button.onClick.AddListener(() => randomColorController.DecideBrightness(gameObject.tag));
    }
}
