using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !FindObjectOfType<MyCursor>().onMenu)
        {
            // Do click stuff here
            FindObjectOfType<ObjectUI>().SetTarget(gameObject);
        }
    }
    private void OnMouseExit()
    {

    }
}
