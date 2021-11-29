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
    GameManager token;
    bool canRegen = true;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        if (tag == "Base")
        {
            mpBar = GameObject.Find("ManaBar - Base").GetComponent<ManaBar>();
            RefreshBaseEnergy();
        }
        else if (tag == "Player")
        {
            mpBar = GameObject.Find("ManaBar - Player").GetComponent<ManaBar>();
            RefreshEnergy();
        }
        else
        {
            mpBar = GetComponentInChildren<ManaBar>();
            RefreshBaseEnergy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "Player")
        {   RefillCheck();}
        if (canRegen)
        {
            RecoverCheck();
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
            token.modUp += 1;
            RefreshEnergy();
        }
    }
    public bool CheckMana(int dmg)
    {
        if (mp >= dmg*(useRate/10))
        {
            return true;
        }
        return false;
    }

    public void BurnMana(int dmg)
    {
        mp -= dmg*(useRate / 10);
        mpBar.SetMp(mp);
    }

    public void RefreshEnergy()
    {
        modMp = token.modUp;
        maxMp = 100 + (10 * modMp);
        useRate = 100 - (5 * Mathf.FloorToInt(modMp / 10));   
        skill1Use = Mathf.FloorToInt(20 * (useRate * 0.01f));
        skill2Use = Mathf.FloorToInt(100 * (useRate * 0.01f));
        refillTime = 2 - (0.1f * (modMp / 5));
        recovRate = 1 + ((int)Mathf.Floor(modMp / 20));
        mpBar.SetMaxMp(maxMp);
    }

    public void RefreshBaseEnergy()
    {
        modMp = token.baseShiledUp;
        maxMp = (int)(100 + (20 * modMp));
        useRate = 100 - modMp;
        recovRate = 1 + ((int)Mathf.Floor(modMp / 10));
        mpBar.SetMaxMp(maxMp);
    }

}
