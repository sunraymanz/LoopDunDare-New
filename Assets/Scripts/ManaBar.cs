using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public StatCalculator statToken;
    public Slider slider;
    public TextMeshProUGUI text;
    public TextMeshProUGUI textMax;
    public Image fill;
    public Gradient fillGradient;
    public Gradient textGradient;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
        statToken = FindObjectOfType<StatCalculator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        //slider.value = 0;
    }
    public void SetFullMp(int mp)
    {
        slider.maxValue = mp;
        slider.value = mp;
        RefreshMp();
    }
    public void SetMp(int mp)
    {
        slider.value = mp;
        RefreshMp();
    }
    public void SetMaxMp(int mp)
    {
        slider.maxValue = mp;
        RefreshMp();
    }

    public void RefreshMp()
    {
        if (text != null)
        { 
            text.text = slider.value.ToString(); 
            text.color = textGradient.Evaluate(slider.normalizedValue); 
        }
        if (textMax != null)
        {
            textMax.text = slider.maxValue.ToString();
        }
        fill.color = fillGradient.Evaluate(slider.normalizedValue);
    }

    public void Reset()
    {
        text.text = "error";
        text.color = Color.white;
        if (textMax != null)
        {
            textMax.text = "error";
        }
        slider.value = 0;
    }

}
