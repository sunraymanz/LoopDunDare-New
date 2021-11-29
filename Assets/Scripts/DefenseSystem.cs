using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenseSystem : MonoBehaviour
{
    CommonAsset assetToken;
    GameManager token;
    StatusMenu statToken;
    //InfoMenu infoToken;
    [SerializeField] Animator coreController;
    [SerializeField] Animator bodyController;
    public HealthBar hpBar;
    
    public int hp = 100;
    public int maxHp = 100;
    public int initHp = 100;
    public int def = 0;
    public bool isDead = false;
    int dmgFinal = 0;
    int seedCal;
    float repairTimeUsed = 0f;
    float repairTimeNeed = 1f;
    public bool isAuto;
    public bool needHeal;
    public float isRepair = 0f;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        assetToken = gameObject.GetComponent<CommonAsset>();
        seedCal = Random.Range(0, 63);
        //Debug.Log("seed is " + seedCal);
        Random.InitState(seedCal);
        if (tag == "Enemy")
        {
            if (GetComponent<EnemyGate>()) // Gate def
            {
                initHp = 1000;
                maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.hpUpBoss))) + 50 * token.lvBoss;
                def = 1 + token.defUpBoss;
            }
            else // enemy robot def
            {
                initHp = 60;
                maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.hpUpBoss))) + 3 * token.lvBoss;
                def = 1 + token.defUpBoss;  
            }
            statToken = FindObjectOfType<StatusMenu>();
            hpBar = this.GetComponentInChildren<HealthBar>();
            hpBar.SetFullHp(maxHp, true);
        }
        else if (tag == "Base")
        {
            initHp = 500;
            maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.basehpUp))) + 10 * token.lv;
            hpBar.SetFullHp(maxHp, false);
            def = (token.basedefUp * 2);
        }
        else if (tag == "MinerBase")
        {
            initHp = 300;
            maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.basehpUp))) + 5 * token.lv;
            hpBar = this.GetComponentInChildren<HealthBar>();
            hpBar.SetFullHp(maxHp, false);
            def = (token.basedefUp * 2);
        }
        else if (tag == "PowerPole")
        {
            initHp = 100;
            maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.basehpUp))) + 10 * token.lv;
            hpBar = this.GetComponentInChildren<HealthBar>();
            hpBar.SetFullHp(maxHp, false);
            def = 2+(token.basedefUp * 1);
        }
        else if (!isAuto) // player def
        {
            initHp = 40;
            maxHp = (int)Mathf.Round(initHp * (1 + (0.15f * token.hpUp))) + 2 * token.lv;
            def = 2 + token.defUp;
            hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<HealthBar>();
            hpBar.SetFullHp(maxHp, false);
        }
        else // miner & box def
        {
            initHp = 20;
            if (tag == "Miner")
            {
                maxHp = (int)Mathf.Round(initHp * (1 + (0.15f * token.hpUp)));
                def = token.defUp;
                token.minerAmount += 1;
            }
            else 
            {
                maxHp = (int)Mathf.Round(initHp * (1 + (0.15f * token.hpUp))) + 2 * token.lv;
                def = 2 + token.defUp;
                token.boxAmount += 1;
            }    
            hpBar = this.GetComponentInChildren<HealthBar>();
            hpBar.SetFullHp(maxHp, false);
        }
        hp = maxHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (token.isEnd && tag != "Enemy")
        {
            hp = -1;
            CheckDead();
        }
        else
        {
            if (hp < maxHp)
            { needHeal = true; }
            else
            { needHeal = false; }
        }
        if (isRepair > 0)
        { 
            GetRepair((int)(maxHp*0.01f));
        }
    }

    private void CheckDead()
    {
        if (hp <= 0 && !isDead)
        {
            isDead = true;
            if ( coreController != null)
            {
                coreController.SetBool("Dead", true);
            }
            bodyController.SetBool("Dead", true);
            GetComponent<Rigidbody2D>().simulated = false;
            if (tag == "Base")
            {
                Debug.Log("TimeScale" + Time.timeScale);
                token.isEnd = true;
                Destroy(this.gameObject, 1.5f);
                return;
            }
            //enemy die & boost player
            else if (tag == "Enemy")
            {
                //Add point & lv
                token.lvPoint += 1;
                token.lv += 1;
                token.noLvUpCountBoss += 1;
                if (token.noLvUpCountBoss % 10 == 0)
                {
                    Debug.Log("World Get Harder");
                    token.lvBoss += 1;
                    token.AtkUpgrade();
                    GotUpgrade();
                    token.noLvUpCountBoss = 0;
                    token.maxEnemy += 1;
                    token.DelayRespawnEnemy();
                }
                //refresh stat
                GameObject.Find("Rank - Number").GetComponent<LevelText>().RefreshLV();
                statToken.RefreshStat();
                token.currentEnemy -= 1;
                token.DelayRespawnEnemy();
                token.DelaySpawnOre(1.5f, transform.position);
            }
            //player die & boost enemy
            else if (tag == "Player")
            {
                token.lvBoss += 1;
                token.EnemyUpgrade();
                if (!isAuto)
                {
                    token.noLvUpCount++;
                    if (token.noLvUpCount % 4 == 0)
                    {
                        Debug.Log("Your Engineer Improve You Something!");
                        token.lvPoint += 1;
                        token.noLvUpCount = 0;
                    }
                    print("call spawn hero");
                    token.DelayRespawnHero(1.5f);
                }
                else
                { token.boxAmount-=1; }
            }
            else if (tag == "Miner")
            { token.minerAmount -= 1; }
            token.DelayPlaySound(1.5f);
            Destroy(this.gameObject, 1.5f);
        }
    }

    public void DamageCalculate(int dmg, int cri, int criDmg)
    {
        int temp = Random.Range(0, 100);
        assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(1, 1, 1, 1);
        assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 4;
        if (temp < cri)
        {
            dmgFinal = (int)Mathf.Round(dmg * (1 + (criDmg * 0.01f)));
            //Debug.Log("!!!CRI!!!");
            //Debug.Log("damage : " + dmgFinal);
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(1, 0, 0, 1);
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 8;
        }
        else dmgFinal = dmg;
        dmgFinal = dmgFinal - def;
        if (dmgFinal < 1)
        { dmgFinal = 1; }
        //Debug.Log(this.tag+" take damage : " + dmgFinal);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GetHit();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GetHit();
        }
    }

    void GetHit()
    {
        if (tag == "Miner")
        { GetComponent<MinerAI>().Retreat(); }
        if (tag == "Base")
        {
            if (GetComponent<ManaSystem>().CheckMana(dmgFinal))
            {
                GetComponent<ManaSystem>().BurnMana(dmgFinal);
                assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "-" + dmgFinal;
                assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(0, 0, 1, 1);
                Instantiate(assetToken.textPrefabs, (Vector3.up / 2) + assetToken.textSpawnPoint.position, Quaternion.identity);
                return; 
            }
        }
        assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "-"+dmgFinal;
        hp -= dmgFinal;
        if (tag == "Enemy")
        { 
            hpBar.SetHpString(hp);
            GetComponentInChildren<CanvasScript>().AddShowTime();
        }
        else
        {
            hpBar.SetHp(hp);
            if (isAuto)
            { GetComponentInChildren<CanvasScript>().AddShowTime();  }
        }
        Instantiate(assetToken.textPrefabs, (Vector3.up/2)+assetToken.textSpawnPoint.position, Quaternion.identity);
        CheckDead();
    }
    public void DieNow()
    {
        hp -= 9999;
        CheckDead();
    }
    public void GotUpgrade()
    {
        int addPoint = Random.Range(0, 3);
        if (addPoint == 0)
        { 
            token.hpUpBoss += 1;
        }
        else if (addPoint == 1)
        {
            token.defUpBoss += 1;
        }
        else 
        { 
            token.defUpBoss += 1;
            token.hpUp += 1;
        }
        RefreshMaxHp();
    }

    public void RefreshMaxHp()
    {
        if (tag == "Enemy")
        { 
            maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.hpUpBoss))) + 3 * token.lvBoss;
        }
        else if (tag == "Base")
        {
            maxHp = (int)Mathf.Round(initHp * (1 + (0.1f * token.basehpUp))) + 10 * token.lv;
            hpBar.SetMaxHp(maxHp);
        }
        else
        {
            maxHp = (int)Mathf.Round(initHp * (1 + (0.15f * token.hpUp))) + 2 * token.lv;
            hpBar.SetMaxHp(maxHp);
        }
    }
    public void RefreshDef()
    {
        if (tag == "Enemy")
        {
            def = 1 + token.defUpBoss;
        }
        else if (tag == "Base")
        {
            def = (token.basedefUp * 2);
        }
        else
        {
            def = 2+token.defUp;
        }
    }
    public void GetRepair(int amount)
    {
        if (isDead || !needHeal)
        { return; }
        if (repairTimeUsed < repairTimeNeed)
        {
            repairTimeUsed += Time.deltaTime;
            return;
        }
        if (hp < maxHp)
        {
            if (amount <= 0)
            { amount = 1; }
            hp += amount;
            isRepair -= 1f;
            hpBar.SetHp(hp);
            if (isAuto)
            {
                GetComponentInChildren<CanvasScript>().AddShowTime();
            }
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(0, 1, 0, 1);
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 2;
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "+" + amount;
            Instantiate(assetToken.textPrefabs, (Vector3.up / 2) + assetToken.textSpawnPoint.position, Quaternion.identity);
            repairTimeUsed = 0f;
        }
        else
        {
            hp = maxHp;
            hpBar.SetHp(hp);
        }
    }

    public void Repair(int amount)
    {
        if (isDead || !needHeal)
        { return; }
        if (amount <= 0)
        { amount = 1; }
        if (hp < maxHp)
        {
            hp += amount;
            hpBar.SetHp(hp);
            if (isAuto)
            {
                GetComponentInChildren<CanvasScript>().AddShowTime();
            }
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(0, 1, 0, 1);
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 2;
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "+" + amount;
            Instantiate(assetToken.textPrefabs, (Vector3.up / 2) + assetToken.textSpawnPoint.position, Quaternion.identity);
            repairTimeUsed = 0f;
        }
        else
        {
            hp = maxHp;
            hpBar.SetHp(hp);
        }
    }



}
