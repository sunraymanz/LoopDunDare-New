using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
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
        if (GetComponentInChildren<TextMeshProUGUI>())
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }      
    }
    public void SetFullHp(int hp)
    {
        slider.maxValue= hp;
        slider.value = hp;
        RefreshHp(); 
    }
    public void SetHp(int hp)
    {
        slider.value = hp;
        RefreshHp();
    }
    public void SetMaxHp(int hp)
    {
        slider.maxValue = hp;
        RefreshHp();
    }

    public void RefreshHp()
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
