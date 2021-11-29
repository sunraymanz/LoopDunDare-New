using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject deployPrefab;
    public GameObject orePrefab;
    public GameObject minerBasePrefab;
    public GameObject polePrefab;
    public Transform spawnPoint;
    public Transform enemySpawnPoint;
    public Transform spawnFX_Player;
    public Transform spawnFX_Enemy;
    public Transform warnText;
    public HQBase hqToken;
    public List<Transform> gateList;
    public bool ready = false;
    public int waveNum = 1;
    AudioSource audioPlayer;
    int seedCal;
    public CinemachineVirtualCamera vcam;
    public AudioClip[] soundClip;

    //Game Status
    public int gold = 0;
    public int lv = 10;
    public bool isEnd = false;
    //Player Status
    public int lvPoint = 8;
    public int dmgUp = 0;
    public int criUp = 0;
    public int criDamageUp = 0;
    public int modUp = 0;
    public int speedUp = 0;
    public int hpUp = 0;
    public int defUp = 0;
    //Base Status
    public int basehpUp = 0;
    public int basedefUp = 0;
    public int baseShiledUp = 0;
    //Enemy Status
    public int lvBoss = 10;
    public int dmgUpBoss = 0;
    public int criUpBoss = 0;
    public int criDamageUpBoss = 0;
    public int speedUpBoss = 0;
    public int hpUpBoss = 0;
    public int defUpBoss = 0;
    //World Status
    public int noLvUpCount = 0;
    public int noLvUpCountBoss = 5;
    public int maxEnemy = 10;
    public int currentEnemy = 0;
    //Right Bar Detail
    public int oreAmount = 0;
    public int boxAmount = 0;
    public int minerAmount = 0;
    //Left Bar Detail
    public int price1 = 800;
    public int price2 = 1500;
    public int price3 = 2000;
    public int price4 = 500;
    //Debug

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = this.GetComponent<AudioSource>();
        hqToken = FindObjectOfType<HQBase>();
        spawnPoint = hqToken.transform;
        seedCal = Random.Range(0, 63);
        //Debug.Log("seed is " + seedCal);
        Random.InitState(seedCal);
        DelayRespawnHero(2f);
        Invoke("ToggleReady",6f);
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ready)
        {
            warnText.GetComponent<AutoHide>().WarnText("!!! Incoming !!!");
            warnText.GetComponent<AutoHide>().AddShowTime();
            SpawnWave(waveNum);
        }
        if (!vcam.Follow || FindObjectOfType<Player>())
        {
            vcam.Follow = spawnPoint;
            if (FindObjectOfType<Player>())
            { vcam.Follow = FindObjectOfType<Player>().transform; }
        }
        if (isEnd)
        {
            vcam.Follow = spawnPoint;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.zero;
            DelayEndGame(4f);
            while (vcam.m_Lens.OrthographicSize > 5)
            {
                vcam.m_Lens.OrthographicSize -= 0.05f;              
            }
        }
    }

    public void BuyItem(int type, Vector2 pos)
    {
        if (CheckPrice(type))
        {
            /*if (pos.y < -2)
            {
                warnText.GetComponent<AutoHide>().WarnText("!!! Please Deploy Higher !!!");
                warnText.GetComponent<AutoHide>().AddShowTime();
                return;
            }*/
            if (type == 1)
            { Instantiate(deployPrefab, pos, Quaternion.identity); }
            else if (type == 2)
            { Instantiate(polePrefab, pos, Quaternion.identity); }
            else
            { Instantiate(minerBasePrefab, pos, Quaternion.identity); }
        }
        else
        {
            warnText.GetComponent<AutoHide>().WarnText("!!! Need More Ore !!!");
            warnText.GetComponent<AutoHide>().AddShowTime();
        }
    }
    bool CheckPrice(int type)
    {
        int price;
        if (type == 1)
        { price = price1; }
        else if (type == 2)
        { price = price2; }
        else
        { price = price3; }

        if (oreAmount >= price)
        { 
            oreAmount -= price;
            return true;
        }
        else return false;
    }
    public void SpawnMiner()
    {
        if (FindObjectOfType<MinerBase>())
        {
            if (oreAmount >= price4 )
            { 
                FindObjectOfType<MinerBase>().SpawnMiner();
                oreAmount -= price4;
            }
            else
            {
                warnText.GetComponent<AutoHide>().WarnText("!!! Need More Ore !!!");
                warnText.GetComponent<AutoHide>().AddShowTime();
            }
            
        }
        else
        {
            warnText.GetComponent<AutoHide>().WarnText("!!! No Miner Base !!!");
            warnText.GetComponent<AutoHide>().AddShowTime();
        }
    }

    void SpawnWave(int waveNum)
    {
        if (waveNum == 1)
        { Invoke("StartSpawnEnemy", 2f); }
        else 
        { }
        waveNum += 1;
        ready = false;
    }

    public void StartSpawnEnemy()
    {
        RespawnEnemy();
        if (currentEnemy < maxEnemy)
        {
            Invoke("StartSpawnEnemy",Random.Range(1f,3f));
        }
    }

    public void RespawnEnemy()
    {
        if (currentEnemy < maxEnemy)
        {
            //GeneratePosition(enemySpawnPoint);
            if (gateList.Count > 0)
            { 
                enemySpawnPoint = gateList[RandomGate()];
                currentEnemy += 1;
                if(enemySpawnPoint)
                { Instantiate(spawnFX_Enemy, new Vector3(enemySpawnPoint.position.x, enemySpawnPoint.position.y, Random.Range(0,128)) , Quaternion.identity, enemySpawnPoint);}
                //StartCoroutine(RespawnAdv(enemyPrefab, enemySpawnPoint.position, 1f, spawnFX_Enemy));
            }
        }
    }
    public void RespawnHero()
    {
        //GeneratePosition(spawnPoint);
        if (!isEnd)
        { 
            Instantiate(spawnFX_Player, spawnPoint.position, Quaternion.identity,hqToken.transform);
            Debug.Log("spawn hero");
        }
        //StartCoroutine(RespawnAdv(playerPrefab, spawnPoint.position, 1f, spawnFX_Player));
    }
    public void DelayRespawnEnemy()
    {
        Invoke("RespawnEnemy", Random.Range(1f, 3f));
    }
    public void DelayRespawnHero(float time)
    {
        Debug.Log("delay spawn hero");
        Invoke("RespawnHero", time);
    }

    public void GeneratePosition(Transform pos)
    {
        pos.position = new Vector3(Random.Range(-8,9),1, Random.Range(-10, 10));
    }

    public int RandomGate()
    {
        int temp = Random.Range(0, gateList.Count);
        return temp;
    }

    public IEnumerator RespawnAdv(GameObject prefab, Vector3 pos, float time, Transform prefabFX)
    {
        yield return new WaitForSeconds(time);
        Instantiate(prefabFX, pos, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Instantiate(prefab, pos, Quaternion.identity);
    }

    public void DelayPlaySound(float time)
    {
        Invoke("PlayBlastSound",time);
    }

    public void DelaySpawnOre(float time, Vector3 pos)
    {
        StartCoroutine(SpawnOre(time, pos));
    }

    public IEnumerator SpawnOre(float time, Vector3 pos)
    {
        yield return new WaitForSeconds(time);
        Instantiate(orePrefab.transform, pos, Quaternion.identity);
    }
    
    public void PlayBlastSound()
    {
        audioPlayer.PlayOneShot(soundClip[0],0.5f);
    }

    public void EnemyUpgrade()
    {
        int temp = Random.Range(0, 2);
        Debug.Log("E.Upgrade is : " + temp);
        if (temp == 0)
        { AtkUpgrade(); }
        else
        { DefUpgrade(); }
        FindObjectOfType<InfoMenu>().RefreshStat();
    }

    public void PlayerUpgrade()
    {
        int temp = Random.Range(1, 8);
        SelectUpgrade(temp);
    }

    public void AtkUpgrade()
    {
        int addPoint = Random.Range(0, 4);
        Debug.Log("Enemy Upgrade Atk : " + addPoint);
        if (addPoint == 0)
        { dmgUpBoss += 1; }
        else if (addPoint == 1)
        { criUpBoss += 1; }
        else if (addPoint == 2)
        { criDamageUpBoss += 1; }
        else
        { speedUpBoss += 1;}
        if (FindObjectOfType<EnemyGun>())
        {
            FindObjectOfType<EnemyGun>().RefreshValue();
        }
        
    }
    public void DefUpgrade()
    {
        int addPoint = Random.Range(0, 2);
        Debug.Log("Enemy Upgrade Def : " + addPoint);
        if (addPoint == 0)
        { hpUpBoss += 1; }
        else 
        { defUpBoss +=1; }
        if (FindObjectOfType<Enemy>())
        {
            FindObjectOfType<Enemy>().GetComponent<DefenseSystem>().RefreshDef();
            FindObjectOfType<Enemy>().GetComponent<DefenseSystem>().RefreshMaxHp();
        }
        
    }

    public void SelectUpgrade(int type)
    {
        //point
        if (lvPoint < 1)
        { return; }
        else { lvPoint -= 1; }
        //Atk
        if (type <= 4)
        {
            if (type == 1)
            { dmgUp += 1; }
            else if(type == 2)
            { criUp += 1; }
            else if(type == 3)
            { criDamageUp += 1; }
            else if(type == 4)
            { speedUp += 1; }
            if (FindObjectOfType<Player>())
            {
                Debug.Log("Upgrade Atk");
                FindObjectOfType<Player>().GetComponentInChildren<TurrentGun>().RefreshValue(); 
            }
        }
        //mana
        if (type == 5)
        { 
            modUp += 1;
            if (FindObjectOfType<Player>())
            {
                Debug.Log("Upgrade Energy");
                FindObjectOfType<Player>().GetComponent<ManaSystem>().RefreshEnergy();
            }    
        }
        //def
        if (type >= 6  && type < 8)
        {
            if (type == 6)
            { hpUp += 1; }
            else if (type == 7)
            { defUp += 1; }
            if (FindObjectOfType<Player>())
            {
                Debug.Log("Upgrade Def");
                FindObjectOfType<Player>().GetComponent<DefenseSystem>().RefreshMaxHp();
                FindObjectOfType<Player>().GetComponent<DefenseSystem>().RefreshDef();
            }
        }
        //Base
        if (type >= 8 && type < 10)
        {
            if (type == 8)
            { basehpUp += 1; }
            else if (type == 9)
            { basedefUp += 1; }
            if (FindObjectOfType<HQBase>())
            {
                Debug.Log("Upgrade B.Def");
                FindObjectOfType<HQBase>().GetComponent<DefenseSystem>().RefreshMaxHp();
                FindObjectOfType<HQBase>().GetComponent<DefenseSystem>().RefreshDef();
            }
        }
        //shield
        if (type == 10)
        { 
            baseShiledUp += 1;
            if (FindObjectOfType<HQBase>())
            {
                Debug.Log("Upgrade Shield");
                FindObjectOfType<HQBase>().GetComponent<ManaSystem>().RefreshBaseEnergy();
            }
        }
        FindObjectOfType<InfoMenu>().RefreshStat();
    }

    public void StatusReport(int faction)
    {
        if (faction == 0)
        {
            Debug.Log("-----Player-----");
            Debug.Log("lv : " + lv);
            Debug.Log("dmgUp :" + dmgUp);
            Debug.Log("criUp : " + criUp);
            Debug.Log("criDamageUp : " + criDamageUp);
            Debug.Log("modUp : " + modUp);
            Debug.Log("speedUp : " + speedUp);
            Debug.Log("hpUp : " + hpUp);
            Debug.Log("defUp : " + defUp);
        }
        if (faction == 0)
        {
            Debug.Log("-----Enemy-----");
            Debug.Log("lvBoss : " + lvBoss);
            Debug.Log("dmgUpBoss : " + dmgUpBoss);
            Debug.Log("criUpBoss : " + criUpBoss);
            Debug.Log("criDamageUpBoss : " + criDamageUpBoss);
            Debug.Log("speedUpBoss : " + speedUpBoss);
            Debug.Log("hpUpBoss : " + hpUpBoss);
            Debug.Log("defUpBoss : " + defUpBoss);
        }
    }

    void ToggleReady()
    { ready = !ready; }
    public void DelayEndGame(float time)
    {
        Invoke("EndGame", time);
        Debug.Log("End Game!");
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }
}
