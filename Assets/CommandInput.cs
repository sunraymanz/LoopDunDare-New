using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandInput : MonoBehaviour
{
    GameManager token;
    public bool isDebugMode = false;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        token = GetComponent<GameManager>();
        print("Debug Mode : " + isDebugMode);
        text.SetActive(isDebugMode);
    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
        if (isDebugMode)
        {
            CheatKey();
        }
    }

    public void CheckKey()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (token.waveEnd)
            {
                token.ready = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            isDebugMode = !isDebugMode;
            text.SetActive(isDebugMode);
            print("Debug Mode : "+isDebugMode);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void CheatKey()
    {
        Vector2 temp;
        int rng = Random.Range(0, 127);
        if (Input.GetKeyDown(KeyCode.T)) // FOR TEST
        {
            //turret
            temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(token.boxPrefab, new Vector3(temp.x, temp.y, rng), Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.E)) // FOR TEST
        {
            //enemy
            temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            token.spawnEnemy(token.enemyPrefab.transform, new Vector3(temp.x, temp.y, rng));
        }
        else if (Input.GetKeyDown(KeyCode.R)) // FOR TEST
        {
            //ore
            temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(token.orePrefab, new Vector3(temp.x, temp.y, rng), Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.B)) // FOR TEST
        {
            //ore
            temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(token.bossPrefab, new Vector3(temp.x, temp.y, rng), Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.O)) // FOR TEST
        {
            FindObjectOfType<HQBase>().GetComponent<DefenseSystem>().DieNow();
        }
        else if (Input.GetKeyDown(KeyCode.P)) // FOR TEST
        {
            FindObjectOfType<EnemyGate>().GetComponent<DefenseSystem>().DieNow();
        }
    }
}
