using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    Rigidbody2D physicToken;
    StatCalculator statToken;
    public int type;
    [SerializeField] Sprite silverSpr;
    [SerializeField] Sprite goldSpr;
    [SerializeField] bool isTaken = false;
    // Start is called before the first frame update
    void Start()
    {
        statToken = FindObjectOfType<StatCalculator>();
        physicToken = GetComponent<Rigidbody2D>();
        physicToken.velocity = new Vector2(Mathf.Sign(transform.position.x)*Random.Range(-2,0), Random.Range(-1, 3));
        if (type == 0)
        {
            GetComponent<SpriteRenderer>().sprite = silverSpr;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = goldSpr;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTaken)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Miner"))
            {
                isTaken = true;
                GetComponent<Collider2D>().enabled = false;
                //print("Get point");
                statToken.AddPoint(type, 1);
                Destroy(gameObject);
            }
        }
    }
}
