using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    public List<Sprite> sprList;
    public Sprite sprOff;
    public Sprite sprOn;
    public Color colorOn;
    Color colorOff;
    public Image imgToken;
    public TextMeshProUGUI textToken;
    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        imgToken = GetComponent<Image>();
        if(textToken)
        colorOff = textToken.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleSprite()
    {
        if (!isActive)
        { imgToken.sprite = sprOn; }
        else { imgToken.sprite = sprOff; }
        isActive = !isActive;
    }

    public void ToggleColor()
    {
        if (!isActive)
        { textToken.color = colorOn; }
        else { textToken.color = colorOff; }
        isActive = !isActive;
    }

    public void ToggleAll()
    {
        if (!isActive)
        { 
            imgToken.sprite = sprOn;
            textToken.color = colorOn;
        }
        else 
        { 
            imgToken.sprite = sprOff;
            textToken.color = colorOff;
        }
        isActive = !isActive;
    }
}
