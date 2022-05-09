using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AutoHideText : MonoBehaviour
{
    public float remainTime;
    public float setTime = 2f;
    public bool useBack;
    public bool useBG;
    public TextMeshProUGUI text;
    public TextMeshProUGUI backText;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowCheck();
        transform.rotation = Quaternion.identity;
    }

    public void AddShowTime()
    {
        remainTime = setTime;
    }
    public void AddShowTime(float time)
    {
        remainTime = time;
    }

    void ShowCheck()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            text.enabled = true;
            if (useBack)
            {
                backText.enabled = true;
            }
            if (useBG)
            {
                image.enabled = true;
            }
        }
        else 
        { 
            text.enabled = false;
            if (useBack)
            {
                backText.enabled = false;
            }
            if (useBG)
            {
                image.enabled = false;
            }
        }
    }

    public void SetWarnText(string str)
    {
        text.text = str;
        if (useBack)
        {
            backText.text = str;    
        }
    }
}
