using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_FX : MonoBehaviour
{
    [SerializeField] bool isEnemy;
    [SerializeField] GameObject prefab;
    [SerializeField] GameManager token;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        if (tag == "Enemy")
        {
            isEnemy = true;
            prefab = token.enemyPrefab;
        }
        else
        { 
            isEnemy = false;
            prefab = token.playerPrefab;
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
                //DestroyNow();
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
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
