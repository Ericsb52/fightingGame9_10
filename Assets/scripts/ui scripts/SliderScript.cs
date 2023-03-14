using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderValue;
    // Start is called before the first frame update
    void Start()
    {
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat("roundTimer", 100));
        sliderValue.text = slider.value.ToString();

        slider.onValueChanged.AddListener((value) => { 
            sliderValue.text = value.ToString();
            PlayerPrefs.SetFloat("roundTimer", value);

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
