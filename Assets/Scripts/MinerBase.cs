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
        Invoke(nameof(SpawnMiner),1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Miner")
        {           
            if (collision.GetComponent<MinerAI>().oreAmount > 0)
            {
                oreStored += collision.GetComponent<MinerAI>().oreAmount;
                token.oreAmount += collision.GetComponent<MinerAI>().oreAmount;
                collision.GetComponent<MinerAI>().oreAmount = 0;               
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Miner")
        {
            collision.GetComponent<MinerAI>().isRetreat = false;
        }
    }

    public void SpawnMiner()
    {
        Instantiate(minerPrefab, transform.position+Vector3.down, Quaternion.identity, token.playerList);
    }

}
