using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverUI : MonoBehaviour
{
    GameManager token;

    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (token.isEnd || token.isWin)
        {
            GetComponent<Animation>().Play();
            enabled = false;
        }
    }
}
