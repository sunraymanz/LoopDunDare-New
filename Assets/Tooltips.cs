using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltips : MonoBehaviour
{
    [SerializeField] Image bgToken;
    [SerializeField] TextMeshProUGUI textToken;
    float textPaddingsize = 0.25f;
    [SerializeField] Vector3 mousePos;
    // Start is called before the first frame update
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bgToken.rectTransform.sizeDelta = new Vector2(textToken.renderedWidth + (textPaddingsize * 2f), textToken.renderedHeight + (textPaddingsize * 2f)); ;
    }

    void CalculateBG() 
    {
        bgToken.rectTransform.localPosition = new Vector2(0,0);
        if (mousePos.x < Camera.main.transform.position.x)
        {
            //mouse on left side
            if (mousePos.y < Camera.main.transform.position.y)
            {
                //mouse on left-lower side
                bgToken.rectTransform.anchorMin = new Vector2(0, 0);
                bgToken.rectTransform.anchorMax = new Vector2(0, 0);
                bgToken.rectTransform.pivot = new Vector2(0, 0);
            }
            else
            {
                //mouse on left-upper side
                bgToken.rectTransform.anchorMin = new Vector2(0, 1);
                bgToken.rectTransform.anchorMax = new Vector2(0, 1);
                bgToken.rectTransform.pivot = new Vector2(0, 1);
            }
        }
        else
        {
            //mouse on right side
            if (mousePos.y < Camera.main.transform.position.y)
            {
                //mouse on right-lower side
                bgToken.rectTransform.anchorMin = new Vector2(1, 0);
                bgToken.rectTransform.anchorMax = new Vector2(1, 0);
                bgToken.rectTransform.pivot = new Vector2(1, 0);
            }
            else
            {
                //mouse on right-upper side
                bgToken.rectTransform.anchorMin = new Vector2(1, 1);
                bgToken.rectTransform.anchorMax = new Vector2(1, 1);
                bgToken.rectTransform.pivot = new Vector2(1, 1);
                bgToken.rectTransform.localPosition = new Vector2(-0.5f,0);
            }
        }
        
    }

    public void ShowToolTip(string str)
    {
        bgToken.gameObject.SetActive(true);
        textToken.text = str;
        CalculateBG();
    }


    public void HideToolTip()
    {
        bgToken.gameObject.SetActive(false);
    }
}
