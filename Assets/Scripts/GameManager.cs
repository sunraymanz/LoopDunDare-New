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
    public GameObject enemydronePrefab;
    public GameObject bossPrefab;
    public GameObject boxPrefab;
    public GameObject orePrefab;
    public GameObject minerBasePrefab;
    public GameObject polePrefab;
    public GameObject supplyPrefab;
    public GameObject allyDronePrefab;
    public GameObject endgame_UI;

    public DropItem corePrefab;
    public Transform spawnPoint;
    public Transform enemySpawnPoint;
    public Transform spawnFX_Player;
    public Transform spawnFX_Enemy;
    public Transform warnText;
    public ReminderText reminderText;
    public Transform enemyList;
    public Transform playerList;
    public HQBase hqToken;
    public List<Transform> gateList;
    public bool isOnMenu = false;
    AudioSource audioPlayer;
    int seedCal;
    public CinemachineVirtualCamera vcam;
    public AudioClip[] soundClip;

    //Game Status
    [Header("Game Section")]
    //public int gold = 0;
    public bool isEnd = false;
    public bool isWin = false;
    public int tier = 0;
    public int waveNum = 0;
    public bool waveEnd = true;
    public bool ready = false;
    //World Status
    [Header("World Section")]
    public int noLvUpCount = 0;
    public int noLvUpCountBoss = 5;
    public int maxEnemy = 5;
    public int SpawnedEnemy = 0;
    public int currentEnemy = 0;
    //Right Bar Detail
    [Header("Counting Section")]
    public int oreAmount = 0;
    public int boxAmount = 0;
    public int minerAmount = 0;
    //Left Bar Detail
    [Header("Price Section")]
    string[] itemName = { "Power Pole", "Miner Base", "Box Turret", "Miner" };
    public int price_PP;
    public int price_MB;
    public int price_BT;
    public int price_M;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        seedCal = Random.Range(0, 63);
        Random.InitState(seedCal);
        waveNum = (PlayerPrefs.GetInt("lastWave", 0) / 5) * 5;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = this.GetComponent<AudioSource>();
        hqToken = FindObjectOfType<HQBase>();
        spawnPoint = hqToken.transform;
        DelayRespawnPlayer(2f);
        reminderText.InitiateState();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckEndGame();
        if ((!vcam.Follow || FindObjectOfType<Player>()) && !isWin && !isEnd)
        {
            vcam.Follow = spawnPoint;
            if (FindObjectOfType<Player>())
            { vcam.Follow = FindObjectOfType<Player>().transform; }
        }
        if (waveEnd)
        {
            if (ready)//check when press Y ready
            {
                WaveInitiate();
                reminderText.StartWaveState();
                SpawnWave(waveNum);
            }
        }
        else
        {
            if (SpawnedEnemy == maxEnemy && currentEnemy == 0)//check if enemy all spawned and dead
            {
                maxEnemy += (maxEnemy / 5);
                SpawnedEnemy = 0;
                reminderText.InitiateState();
                waveEnd = true;
                SpawnSupplyDrop();
            }
        }
    }

    public void SpawnSupplyDrop() 
    {
        Instantiate(supplyPrefab, new Vector2( hqToken.transform.position.x+Random.Range(-25,25),30), Quaternion.identity);
        ShowWarning("!!! Supply Drop Incomming !!!");
    }

    public void CheckEndGame() 
    {
        if (isEnd == !isWin)
        {           
            if (isEnd)
            {
                vcam.Follow = spawnPoint;
            }
            if (isWin)
            {
                if (FindObjectOfType<EnemyGate>())
                {
                    vcam.Follow = FindObjectOfType<EnemyGate>().transform;
                }
            }
            reminderText.EndState();
            warnText.gameObject.SetActive(false);
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.zero;
            DelayEndGame(4f);
            while (vcam.m_Lens.OrthographicSize > 5)
            {
                vcam.m_Lens.OrthographicSize -= 0.05f;
            }            
        }       
    }
    public void WaveInitiate() 
    {      
        waveNum += 1;
        tier =  (waveNum-1) / 10;
        if (waveNum % 10 == 1)
        {
            maxEnemy = 5+(tier/2);
        }
    }

    public void BuyItem(int type, Vector2 pos)
    {
        if (type == 4)
        {
            if (!FindObjectOfType<MinerBase>())
            {
                ShowWarning("!!! No Miner Base !!!",true);
                return;
            }
        }
        if (CheckPrice(type))
        {
            if (type == 1)
            { Instantiate(polePrefab, pos, Quaternion.identity,playerList); }
            else if (type == 2)
            { Instantiate(minerBasePrefab, pos, Quaternion.identity, playerList); }
            else if (type == 3)
            { Instantiate(boxPrefab, pos, Quaternion.identity, playerList); }
            else
            {
                FindObjectOfType<MinerBase>().SpawnMiner(); 
            }
        }
        else
        {
            ShowWarning("!!! Need More Ore For : "+ itemName[type-1] + " !!!",true);
        }
    }
    bool CheckPrice(int type)
    {
        int price;
        if (type == 1)
        { price = price_PP; }
        else if (type == 2)
        { price = price_MB; }
        else if(type == 3)
        { price = price_BT; }
        else
        { price = price_M; }
        if (oreAmount >= price)
        { 
            oreAmount -= price;
            return true;
        }
        else return false;
    }

    void SpawnWave(int num)
    {
        Invoke(nameof(StartSpawnEnemy), 2f);
        ShowWarning("!!! WAVE : " + waveNum + " Incoming !!!",true);
        ready = false;
        waveEnd = false;
    }

    public void StartSpawnEnemy()
    {    
        RespawnEnemy();
        if (SpawnedEnemy < maxEnemy) // check if enemy max yet
        {
            Invoke(nameof(StartSpawnEnemy), 1f);
        }
        else // check if boss is coming
        {
            if (waveNum % (5-tier) == 0)
            {
                print("Boss In Wave : " + waveNum );
                Invoke(nameof(RespawnBoss), 1f);
            }
        }
    }
    public void RespawnBoss()
    {
        maxEnemy += 1;       
        spawnFX_Enemy.GetComponent<Spawner_FX>().isBoss = true;
        spawnEnemy(spawnFX_Enemy, enemySpawnPoint.position);
    }
    public void RespawnEnemy()
    {
        spawnFX_Enemy.GetComponent<Spawner_FX>().isBoss = false;
        //GeneratePosition(enemySpawnPoint);
        if (gateList.Count > 0)
        {
            enemySpawnPoint = gateList[RandomGate()];
            if (enemySpawnPoint)
            {
                spawnEnemy(spawnFX_Enemy, enemySpawnPoint.position);
            }
            //StartCoroutine(RespawnAdv(enemyPrefab, enemySpawnPoint.position, 1f, spawnFX_Enemy));
        }
    }

    public void spawnEnemy(Transform prefab, Vector2 target)
    {
        Instantiate(prefab, new Vector3(target.x, target.y, Random.Range(0, 128)), Quaternion.identity, enemySpawnPoint);
        currentEnemy += 1;
        SpawnedEnemy += 1;
    }

    public void RespawnPlayer()
    {
        //GeneratePosition(spawnPoint);
        if (!isEnd)
        { 
            Instantiate(spawnFX_Player, spawnPoint.position, Quaternion.identity,playerList);
            //Debug.Log("spawn hero");
        }       
    }
    public void DelayRespawnEnemy()
    {
        Invoke("RespawnEnemy", Random.Range(1f, 3f));
    }
    public void DelayRespawnPlayer(float time)
    {
        //Debug.Log("delay spawn hero");
        Invoke("RespawnPlayer", time);
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

    public IEnumerator DelaySpawnDrop(float time, Vector3 pos,int type)
    {
        yield return new WaitForSeconds(time);
        SpawnOre(pos);
        SpawnCore(pos,type);
    }
    public IEnumerator DelaySpawnCore(float time, Vector3 pos, int type)
    {
        yield return new WaitForSeconds(time);
        SpawnCore(pos, type);
    }

    public void SpawnOre( Vector3 pos)
    {
        Instantiate(orePrefab.transform, pos, Quaternion.identity);
    }

    public void SpawnCore( Vector3 pos, int type)
    {
        corePrefab.type = type;
        Instantiate(corePrefab.transform, pos, Quaternion.identity);
    }
    public void ShowWarning(string text, bool isUrgent)
    {
        if (isUrgent)
        {
            warnText.GetComponent<Animator>().SetBool("isUrgent",true);
        }
        ShowWarning(text);
    }
    public void ShowWarning(string text) 
    {
        warnText.GetComponent<AutoHideText>().SetWarnText(text);
        warnText.GetComponent<AutoHideText>().AddShowTime();
    }
    
    public void PlayBlastSound()
    {
        audioPlayer.PlayOneShot(soundClip[0],0.5f);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Unpause()
    {
        Time.timeScale = 1;
    }

    void ToggleReady()
    { ready = !ready; }
    public void DelayEndGame(float time)
    {
        if (endgame_UI.active == false)
        {
            Invoke("EndGame", time);
            Debug.Log("End Game!");
        }
    }

    public void EndGame()
    {       
        endgame_UI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
}
