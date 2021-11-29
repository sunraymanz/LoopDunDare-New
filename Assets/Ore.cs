using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public int amount ;
    SpriteRenderer renderToken;
    Rigidbody2D physicToken;
    public bool onGround = false;
    // Start is called before the first frame update
    void Start()
    {
        renderToken = GetComponentInChildren<SpriteRenderer>();
        physicToken = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSize();
    }

    void CheckSize()
    {
        if (amount <= 0)
        { Destroy(this.gameObject); }
        else
        {
            transform.localScale = new Vector3(1+(0.001f*amount),1+(0.001f*amount),1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        { onGround = true; }
        if (collision.gameObject.tag == "Ore")
        {
            ExchangeCheck(collision.gameObject.GetComponent<Ore>());
        }
    }

    private void ExchangeCheck(Ore target)
    {
        if (onGround)
        {
            if (!target.onGround)
            {
                amount += target.amount;
                Destroy(target.gameObject);
            }
        }
        if(onGround == target.onGround)
        {
            if (amount > target.amount)
            {
                amount += target.amount/4;
                Destroy(target.gameObject);
            }
            else
            {
                target.amount += amount/4;
                Destroy(this.gameObject);
            }

        }
    }
       
}
