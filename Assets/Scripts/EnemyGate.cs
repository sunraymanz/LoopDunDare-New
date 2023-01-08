using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGate : MonoBehaviour
{
    GameManager token;
    bool onGround = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {       
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;   
        }    
    }
}
