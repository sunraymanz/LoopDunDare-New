using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    public List<Sprite> sprList;
    public Sprite sprOff;
    public Sprite sprOn;
    public Color colorOn;
    public Color colorOff;
    public Image imgToken;
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Vector2.Distance(transform.position, FindObjectOfType<MyCursor>().transform.position) < 0.8f)
        {
            imgToken.sprite = sprOn;
        }
        else if (Input.GetMouseButtonUp(0) && Vector2.Distance(transform.position, FindObjectOfType<MyCursor>().transform.position) < 0.8f)
        {
            imgToken.sprite = sprOff;
        }
        else if(Vector2.Distance(transform.position, FindObjectOfType<MyCursor>().transform.position) >= 0.8f)
        {
            imgToken.sprite = sprOff;
        }
        
    }


}
