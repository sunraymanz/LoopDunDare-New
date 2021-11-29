using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public Text text;
    public Image fill;
    public Gradient fillGradient;
    public Gradient textGradient;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
    }
    public void SetFullMp(int mp,bool showMpOnly)
    {
        slider.maxValue= mp;
        slider.value = mp;
        if (showMpOnly)
        { RefreshMp(mp.ToString()); }
        else
        { RefreshMp(); }
    }
    public void SetMp(int mp)
    {
        slider.value = mp;
        RefreshMp();

    }
    public void SetMpString(int mp)
    {
        slider.value = mp;
        RefreshMp(slider.value.ToString());
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
            text.text = slider.value + "/" + slider.maxValue; 
            text.color = textGradient.Evaluate(slider.normalizedValue); 
        }
        fill.color = fillGradient.Evaluate(slider.normalizedValue);
    }

    public void RefreshMp(string str)
    {
        if (text != null)
        { 
            text.text = str; 
            text.color = textGradient.Evaluate(slider.normalizedValue); 
        }
        fill.color = fillGradient.Evaluate(slider.normalizedValue);
    }



}
