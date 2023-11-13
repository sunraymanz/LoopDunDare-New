using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandInput : MonoBehaviour
{
    [SerializeField] GameManager token;
    [SerializeField] StatCalculator statToken;
    public bool isDebugMode = false;
    public GameObject text;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        token = GetComponent<GameManager>();
        statToken = GetComponent<StatCalculator>();
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
            token.oreAmount = 100000;
            statToken.corePoint = 1000;
            statToken.rarePoint = 100;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !token.isEnd)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (!token.isOnMenu)
            {
                pauseMenu.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FindObjectOfType<StatCalculator>().ChangeWeapon(0);
            FindObjectOfType<Player>().GetComponentInChildren<PlayerGun>().GetWeaponInfo(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            FindObjectOfType<StatCalculator>().ChangeWeapon(1);
            FindObjectOfType<Player>().GetComponentInChildren<PlayerGun>().GetWeaponInfo(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            FindObjectOfType<StatCalculator>().ChangeWeapon(2);
            FindObjectOfType<Player>().GetComponentInChildren<PlayerGun>().GetWeaponInfo(false);
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
        else if (Input.GetKeyDown(KeyCode.F3)) // FOR TEST
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
