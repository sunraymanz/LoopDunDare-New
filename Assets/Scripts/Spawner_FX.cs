using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_FX : MonoBehaviour
{
    [SerializeField] bool isEnemy;
    public bool isBoss;
    [SerializeField] GameObject prefab;
    [SerializeField] GameManager token;
    [SerializeField] Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        if (tag == "Enemy")
        {
            isEnemy = true;
            if (Random.Range(0,2) == 0)prefab = token.enemyPrefab;
            else prefab = token.enemydronePrefab;
                parent = token.enemyList;
            if (isBoss)
            {
                prefab = token.bossPrefab;
            }
        }
        else
        { 
            isEnemy = false;
            prefab = token.playerPrefab;
            parent = token.playerList;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isEnemy)
        {
            if (GetComponentInParent<EnemyGate>().GetComponent<DefenseSystem>().hp <= 0)
            {
                Debug.Log(" Enemy FX Boom ");
                DestroyNow();
            }
        }
        else
        {
            if (token.isEnd)
            {
                DestroyNow();
            }
        }
        
    }

    void DestroyNow()
    {
        Destroy(this.gameObject);
    }

    void Spawn()
    {
        //Debug.Log("Spawn!");
        Instantiate(prefab, transform.position, Quaternion.identity, parent);
    }
}
