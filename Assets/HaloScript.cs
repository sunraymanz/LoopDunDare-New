using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloScript : MonoBehaviour
{
    SpriteRenderer sprToken;
    MyCursor cursorToken;
    // Start is called before the first frame update
    void Start()
    {
        sprToken = GetComponent<SpriteRenderer>();
        cursorToken = GetComponentInParent<MyCursor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorToken.isOverlap)
        {
            sprToken.color = Color.red;
        }
        else { sprToken.color = Color.white; }
        
    }
}
