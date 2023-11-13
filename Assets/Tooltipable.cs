using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltipable : MonoBehaviour
{
    public string text ;
    private void OnMouseOver()
    {
        FindObjectOfType<Tooltips>().ShowToolTip(text);
        print("tooltip show");
    }

    private void OnMouseExit()
    {
        FindObjectOfType<Tooltips>().HideToolTip();
        print("tooltip hide");
    }


}
