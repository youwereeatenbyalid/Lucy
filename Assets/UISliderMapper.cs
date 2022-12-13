using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISliderMapper : MonoBehaviour
{
    public LinearSlider spawnrateslider;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        spawnrateslider.value = PlayerPrefs.GetFloat("difficulty", 0.5f);
        slider.value = PlayerPrefs.GetFloat("difficulty", 0.5f);
    }

    private void OnEnable()
    {
        spawnrateslider.OnHandleUpdate += UpdateSlider;
    }

    private void OnDisable()
    {
        spawnrateslider.OnHandleUpdate -= UpdateSlider;
    }

    public void UpdateSlider(float value) {
        slider.value = value;
        PlayerPrefs.SetFloat("difficulty", value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
