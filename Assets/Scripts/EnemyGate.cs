using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGate : MonoBehaviour
{
    GameManager token;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        token.gateList.Add(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
