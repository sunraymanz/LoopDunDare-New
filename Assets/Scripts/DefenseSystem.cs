using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenseSystem : MonoBehaviour
{
    CommonAsset assetToken;
    GameManager token;
    StatCalculator statToken;
    ArmorItem armorToken;
    //InfoMenu infoToken;
    [SerializeField] Animator coreController;
    [SerializeField] Animator bodyController;
    public HealthBar hpBar;
    //Stat
    public int baseHP;
    public int baseDef;
    public int maxHp = 100;
    public int hp = 100;
    public int def = 0;
    public bool isDead = false;
    int dmgFinal = 0;
    //int seedCal;
    float repairTimeUsed = 0f;
    float repairTimeNeed = 1f;
    public bool isAuto;
    public bool needHeal;

    void Start()
    {
        token = FindObjectOfType<GameManager>();
        statToken = FindObjectOfType<StatCalculator>();
        assetToken = gameObject.GetComponentInParent<CommonAsset>();
        // Enemy & Boss
        if (tag == "Enemy" || tag == "Boss" || tag == "Drone")
        {
            armorToken = GetComponentInChildren<ArmorItem>();
            hpBar = this.GetComponentInChildren<HealthBar>();
        }
        else if (CompareTag("EnemyGate")) 
        {
            baseHP = 1000;
            hpBar = this.GetComponentInChildren<HealthBar>();
        }
        else if (tag == "Base")
        {
            baseHP = 500;
            hpBar = GameObject.Find("HealthBar - Base").GetComponent<HealthBar>();
        }
        else if (tag == "MinerBase")
        {
            baseHP = 300;
            hpBar = GetComponentInChildren<HealthBar>();
        }
        else if (tag == "PowerPole")
        {
            baseHP = 100;
            hpBar = GetComponentInChildren<HealthBar>();
        }
        // Player 
        else if (!isAuto)
        {
            hpBar = GameObject.Find("HealthBar - Player").GetComponent<HealthBar>();
        }
        else // Miner & Box 
        {
            baseHP = 20;
            if (tag == "Miner")
            {
                armorToken = GetComponentInChildren<ArmorItem>();
                maxHp = (int)Mathf.Round(baseHP * (1 + (0.15f * statToken.hpUp)));
                def = statToken.defUp;
                token.minerAmount += 1;
            }
            else
            {
                maxHp = (int)Mathf.Round(baseHP * (1 + (0.15f * statToken.hpUp))) + 2 * statToken.hpUp;
                def = 2 + statToken.defUp;
                token.boxAmount += 1;
            }
            hpBar = GetComponentInChildren<HealthBar>();
        }
        RefreshMaxHp();
        RefreshDef();
        hp = maxHp;
        hpBar.SetFullHp(hp);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (token.isEnd && (gameObject.layer != LayerMask.NameToLayer("Enemy")))
        {
            hp = -1;
            CheckDead();
        }
        else if (token.isWin && (CompareTag("Enemy") || CompareTag("Boss") || CompareTag("Drone")))
        {
            hp = -1;
            CheckDead();
        }
        if (hp < maxHp)
        { needHeal = true; }
        else
        { needHeal = false; }
    }

    private void CheckDead()
    {
        if (hp <= 0 && !isDead) //check HP < 0
        {
            isDead = true;
            if ( coreController != null)
            {
                coreController.SetBool("Dead", true);
            }
            bodyController.SetBool("Dead", true);
            if (tag == "Drone")
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                GetComponentInChildren<PolygonCollider2D>().gameObject.layer = LayerMask.NameToLayer("OnlyGround");
            }
            else GetComponent<Rigidbody2D>().simulated = false;       
            if (tag == "Base")
            {
                statToken.SaveStat();
                token.isEnd = true;
                hpBar.Reset();
            }
            else if (tag == "EnemyGate")
            {
                statToken.SaveStat();
                token.isWin = true;               
            }
            //enemy die & boost player
            else if (tag == "Enemy" || tag == "Boss" || tag == "Drone")
            {
                token.noLvUpCountBoss += 1;
                //check if enemy too easy
                if (token.noLvUpCountBoss / 5 == 1)
                {
                    //boost enemy
                    Debug.Log("World Get Harder");
                    statToken.lvBoss += 1;
                    statToken.AtkUpgrade(false);
                    statToken.DefUpgrade(false);
                    token.noLvUpCountBoss = 0;
                }
                //refresh stat
                token.currentEnemy -= 1;
                //token.DelayRespawnEnemy();
                if (tag == "Boss")
                {
                    token.StartCoroutine(token.DelaySpawnDrop(1.5f, transform.position, 1));
                }
                else
                {
                    if (tag == "Drone") token.StartCoroutine(token.DelaySpawnDrop(1.5f, transform.position-(Vector3.up*5), 0));
                    else token.StartCoroutine(token.DelaySpawnDrop(1.5f, transform.position, 0));
                }
            }
            //player die & boost enemy
            else if (tag == "Player")
            {
                //Boost enemy
                statToken.lvBoss += 1;
                statToken.RandomUpgrade(false);
                GameObject.Find("ManaBar - Player").GetComponent<ManaBar>().Reset();
                //check if it's Player or Turret
                if (!isAuto)
                {
                    hpBar.Reset();
                    token.noLvUpCount++;
                    //check if Player Can't Fight Back
                    if (token.noLvUpCount % 4 == 0)
                    {
                        Debug.Log("Your Engineer Improve You Something!");
                        statToken.RandomUpgrade(true);
                        token.noLvUpCount = 0;
                    }
                    print("call spawn hero");
                    token.DelayRespawnPlayer(1.5f);
                }
                else
                { token.boxAmount -= 1; }
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
        assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 12;
        if (temp < cri)
        {
            dmgFinal = (int)Mathf.Round(dmg * (criDmg * 0.01f));
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(1, 0, 0, 1);
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 16;
        }
        else dmgFinal = dmg;
        dmgFinal = dmgFinal - def;
        if (dmgFinal < 1)
        { dmgFinal = 1; }
    }

    public void GetHit()
    {      
        if (tag == "Miner")
        { GetComponent<MinerAI>().SetRetreat(true); }
        if (tag == "Base" || tag == "EnemyGate")
        {
            if (tag == "EnemyGate")
            {
                if (token.ready == false && token.waveEnd)
                {
                    token.ready = true;
                }
            }
            if (GetComponent<ManaSystem>().CheckMana(dmgFinal,false))
            {
                GetComponent<ManaSystem>().BurnMana(dmgFinal);
                assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "-" + dmgFinal;
                assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = Color.blue;
                Instantiate(assetToken.textPrefabs, (Vector3.up * (0.2f * Random.Range(1, 7))) + assetToken.textSpawnPoint.position, Quaternion.identity);
                return; 
            }
        }
        assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "-"+dmgFinal;
        hp -= dmgFinal;
        if (tag == "Enemy" || tag == "EnemyGate" || tag == "Boss" || tag == "Drone")
        { 
            hpBar.SetHp(hp);           
            GetComponentInChildren<CanvasScript>().AddShowTime();
        }
        else
        {
            hpBar.SetHp(hp);
            if (isAuto)
            { GetComponentInChildren<CanvasScript>().AddShowTime();  }
        }

        Instantiate(assetToken.textPrefabs, (Vector3.up * (0.25f * Random.Range(1, 8))) +assetToken.textSpawnPoint.position, Quaternion.identity);
        CheckDead();
    }
    public void DieNow()
    {
        hp = -1;
        CheckDead();
    }

    public void RefreshMaxHp()
    {
        if (tag == "EnemyGate")
        {
            maxHp = statToken.BaseHpCal(0, false);
        }
        else if (tag == "Enemy" || tag == "Drone")
        {
            maxHp = statToken.HpCal(armorToken.hp, false);
        }
        else if (tag == "Base")
        {
            maxHp = statToken.BaseHpCal(0,true);
        }
        else if (tag == "MinerBase")
        {
            maxHp = statToken.BaseHpCal(1, true);
        }
        else if (tag == "PowerPole")
        {
            maxHp = statToken.BaseHpCal(2, true);
        }
        else if (tag == "Player")
        {
            maxHp = statToken.GetPlayerStat(2);
        }
        else { maxHp = statToken.HpCal(armorToken.hp, false); }
        hpBar.SetMaxHp(maxHp);
    }
    public void RefreshDef()
    {
        if (tag == "EnemyGate")
        {
            def = statToken.BaseDefCal(0, false);
        }
        else if (tag == "Enemy" || tag == "Drone")
        {
            def = statToken.DefCal(armorToken.def, false);
        }
        else if (tag == "Base")
        {
            def = statToken.BaseDefCal(0, true);
        }
        else if (tag == "MinerBase")
        {
            def = statToken.BaseDefCal(1, true);
        }
        else if (tag == "PowerPole")
        {
            def = statToken.BaseDefCal(2, true);
        }
        else if (tag == "Player")
        {
            def = statToken.GetPlayerStat(1);
        }
        else { def = statToken.DefCal(armorToken.def,false); }
    }
    public void ChargeRepair(int amount,bool isPercentage)
    {
        if (repairTimeUsed < repairTimeNeed)
        {
            repairTimeUsed += Time.deltaTime;
            return;
        }
        else
        {
            repairTimeUsed = 0f;
            Repair(amount, isPercentage);
        }
    }

    public void Repair(int amount, bool isPercentage)
    {
        if (isDead || !needHeal)
        { return; }
        if (isPercentage)
        { amount = Mathf.CeilToInt(amount * maxHp * 0.01f); }
        if (hp < maxHp)
        {
            hp += amount;
            if (isAuto)
            {
                GetComponentInChildren<CanvasScript>().AddShowTime();
            }
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().color = new Color(0, 1, 0, 1);
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().fontSize = 6;
            assetToken.textPrefabs.GetComponentInChildren<TextMeshPro>().text = "+" + amount;
            Instantiate(assetToken.textPrefabs, (Vector3.up * (0.3f* Random.Range(1, 6))) + assetToken.textSpawnPoint.position, Quaternion.identity);
        }
        if(hp>maxHp)
        {
            hp = maxHp;
        }
        hpBar.SetHp(hp);
    }

    public void PercentAdd(int hpAdd,int defAdd) 
    {
        float temp = (float)hp/maxHp;
        maxHp += Mathf.RoundToInt(hpAdd*0.01f*maxHp);
        hp = Mathf.RoundToInt(temp * maxHp);
        def += Mathf.RoundToInt(defAdd * 0.01f * def);
        print("Temp : "+temp + " Max HP : "+maxHp +" Def : "+def);
        hpBar.SetMaxHp(maxHp);
        hpBar.SetHp(hp);
    }

}
