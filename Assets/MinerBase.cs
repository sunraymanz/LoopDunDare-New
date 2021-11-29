using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerBase : MonoBehaviour
{
    GameManager token;
    public Transform minerPrefab;
    public int oreStored = 0;
    bool onGround;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //maxMiner += 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Miner")
        {
            oreStored += collision.GetComponent<MinerAI>().oreAmount;
            token.oreAmount += collision.GetComponent<MinerAI>().oreAmount;
            collision.GetComponent<MinerAI>().oreAmount = 0;
        }
    }

    public void SpawnMiner()
    {
        Instantiate(minerPrefab, transform.position+Vector3.down, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        { onGround = true; SpawnMiner(); }
    }
}
