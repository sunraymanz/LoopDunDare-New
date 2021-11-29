using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;
    public Image fill;
    public Image border;
    public Gradient fillGradient;
    public Gradient textGradient;
    public GameManager token;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
        token = FindObjectOfType<GameManager>();
    }
    public void SetFullHp(int hp,bool showHpOnly)
    {
        slider.maxValue= hp;
        slider.value = hp;
        if (showHpOnly)
        { RefreshHp(hp.ToString()); }
        else
        { RefreshHp(); }
    }
    public void SetHp(int hp)
    {
        slider.value = hp;
        RefreshHp();

    }
    public void SetHpString(int hp)
    {
        slider.value = hp;
        RefreshHp(slider.value.ToString());
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
            text.text = slider.value + "/" + slider.maxValue;
            text.color = textGradient.Evaluate(slider.normalizedValue);
        }
        fill.color = fillGradient.Evaluate(slider.normalizedValue);
    }

    public void RefreshHp(string str)
    {
        if (text != null)
        {
            text.text = str;
            text.color = textGradient.Evaluate(slider.normalizedValue);
        }
        fill.color = fillGradient.Evaluate(slider.normalizedValue);
    }



}
