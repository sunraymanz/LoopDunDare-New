using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    float remainTime = 0f;
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
        remainTime = 3f;
    }

    void ShowCheck()
    {
        remainTime -= Time.deltaTime;
        if (remainTime > 0)
        { this.GetComponent<Canvas>().enabled = true; }
        else this.GetComponent<Canvas>().enabled = false;
    }
}
