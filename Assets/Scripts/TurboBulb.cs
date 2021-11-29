using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboBulb : MonoBehaviour
{
    public int id;
    public Sprite[] sprite;
    ManaSystem manaToken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (manaToken != null)
        {
            ActiveCheck();
        }
        else
        {
            this.GetComponent<Image>().sprite = sprite[0];
            if (FindObjectOfType<Player>() != null)
            {
                manaToken = FindObjectOfType<Player>().GetComponent<ManaSystem>();
            }
        }
    }

    void ActiveCheck()
    {
        if (manaToken.turbo < id)
        { this.GetComponent<Image>().sprite = sprite[0]; }
        else 
        { this.GetComponent<Image>().sprite = sprite[1]; }
    }
}
