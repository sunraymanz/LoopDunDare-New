using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // FOR TEST
        {
            BurnMana();
            GetComponent<DefenseSystem>().DieNow();
        }
    }

    void BurnMana()
    {
        GetComponent<ManaSystem>().BurnMana(5);
    }
}
