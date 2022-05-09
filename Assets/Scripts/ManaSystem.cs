using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Serialization;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public int maxTurbo = 4;
    public int turbo = 0;
    public float refillCount = 0f;
    public float refillTime = 2f;
    public int mp = 0;
    public int maxMp = 100;
    public int skill1Use = 20;
    public int skill2Use = 100;
    public int modMp = 1;
    public int recovRate = 1;
    public int useRate = 100;
    public ManaBar mpBar;
    StatCalculator statToken;
    bool canRegen;
    float delayRegen = 0;
    // Start is called before the first frame update
    void Start()
    {
        canRegen = true;
        statToken = FindObjectOfType<StatCalculator>();
        if (tag == "Base")
        {
            mpBar = GameObject.Find("ManaBar - Base").GetComponent<ManaBar>();
            RefreshEnergy();
        }
        else if (tag == "Player")
        {
            mpBar = GameObject.Find("ManaBar - Player").GetComponent<ManaBar>();
            RefreshEnergy();
        }
        else if(CompareTag("EnemyGate"))
        {
            mpBar = GetComponentInChildren<ManaBar>();
            RefreshEnergy();
        }
        mpBar.SetMaxMp(maxMp);
    }

    // Update is called once per frame
    void Update()
    {
        if (delayRegen > 0 && CompareTag("Base"))
        {
            delayRegen -= Time.deltaTime;
            return;
        }
        if (!GetComponent<DefenseSystem>().isDead)
        {
            if (tag == "Player")
            {
                RefillCheck();
            }
            if (canRegen)
            {
                RecoverCheck();
            }
        }
    }
    void RefillCheck()
    {
        refillCount += Time.deltaTime;
        if (refillCount > refillTime)
        {
            if (turbo < maxTurbo)
            {
                turbo += 1;
            }
            refillCount = 0f;
        }
    }

    public void RecoverCheck()
    {
        if (mp < maxMp)
        {
            mp += recovRate;
        }
        mpBar.SetMp(mp);
    }

    public void GotUpgrade()
    {
        int addPoint = Random.Range(0, 3);
        if (addPoint == 0)
        {
            
        }
        else if (addPoint == 1)
        {
            RefreshEnergy();
        }
        else
        {
            statToken.energyUp += 1;
            RefreshEnergy();
        }
    }
    public bool CheckMana(int amount, bool isPercent)
    {
        if (isPercent)
        {
            if (mp >= Mathf.FloorToInt(amount * 0.01f * maxMp))
            {
                return true;
            }
            return false;
        }
        else
        {
            if (mp >= amount * (useRate / 10))
            {
                return true;
            }
            return false;
        }
    }

    public void BurnMana(int dmg)
    {
        mp -= dmg*(useRate / 10);
        mpBar.SetMp(mp);
    }
    public void BurnPercentMana(int amount)
    {
        mp -= Mathf.FloorToInt(amount*0.01f*maxMp);
        mpBar.SetMp(mp);
    }

    public void RefreshEnergy()
    {
        /*useRate = 100 - (5 * Mathf.FloorToInt(modMp / 10));   
        skill1Use = Mathf.FloorToInt(20 * (useRate * 0.01f));
        skill2Use = Mathf.FloorToInt(100 * (useRate * 0.01f));
        refillTime = 2 - (0.1f * (modMp / 5));*/
        useRate = 100;
        if (CompareTag("EnemyGate"))
        {
            maxMp = statToken.BaseEnergyCal(false);
            recovRate = statToken.BaseEnergyRegenCal(false);
        }
        else if (tag == "Base")
        {
            maxMp = statToken.BaseEnergyCal(true);
            recovRate = statToken.BaseEnergyRegenCal(true);
        }
        else
        {
            maxMp = statToken.GetPlayerStat(6);
            recovRate = statToken.GetPlayerStat(7);
            mpBar.SetMaxMp(maxMp);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            delayRegen = 3f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            delayRegen = 3f;
        }
    }

}
