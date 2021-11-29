using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AutoHide : MonoBehaviour
{
    public float remainTime;
    public TextMeshProUGUI text;
    public TextMeshProUGUI backText;
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
        remainTime = 2f;
    }

    void ShowCheck()
    {
        remainTime -= Time.deltaTime;
        if (remainTime > 0)
        { text.enabled = true; backText.enabled = true; }
        else { text.enabled = false; backText.enabled = false; }
    }

    public void WarnText(string str)
    {
        text.text = str;
        backText.text = str;
    }
}
