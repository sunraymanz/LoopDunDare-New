using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatCalculator : MonoBehaviour
{
    Equipment eqToken;
    [SerializeField] Page_Upgrade page_upToken;
    [SerializeField] Page_StatDetail page_statToken;
    [SerializeField] EndgameUI page_endToken;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] TextMeshProUGUI rarepointText;

    [Header("Core Section")]
    public int spendPoint = 0;
    public int spendRarePoint = 0;
    public int corePoint = 10;
    public int rarePoint = 0;

    [Header("Previous Section")]
    //Old Upgrade
    public int atkPre = 0;
    public int defPre = 0;
    public int hpPre = 0;
    public int criPre = 0;
    public int fireratePre = 0;
    public int energyPre = 0;

    [Header("Player Section")]
    //Player Upgrade
    public int atkUp = 0;
    public int defUp = 0;
    public int hpUp = 0;
    public int criUp = 0;
    public int firerateUp = 0;
    public int energyUp = 0;
    //Base Upgrade
    public int b_atkUp = 0;
    public int b_defUp = 0;
    public int b_hpUp = 0;
    public int b_criUp = 0;
    public int b_firerateUp = 0;
    public int b_energyUp = 0;

    [Header("Enemy Section")]
    //Enemy Upgrade
    public int lvBoss = 1;
    public int atkUpBoss = 0;
    public int defUpBoss = 0;
    public int hpUpBoss = 0;
    public int criUpBoss = 0;
    public int firerateUpBoss = 0;
    public int energyUpBoss = 0;

    [Header("Equipment Section")]
    //Equipment
    int weaponIndex = 0;
    int armorIndex = 0;
    public WeaponItem currentWeapon;
    public ArmorItem currentArmor;

    // Start is called before the first frame update
    void Start()
    {
        LoadStat();
        eqToken = FindObjectOfType<Equipment>();
        currentWeapon = eqToken.weaponList[weaponIndex];
        currentArmor = eqToken.armorList[armorIndex];
        pointText.text = corePoint.ToString();
        rarepointText.text = rarePoint.ToString();
        InitEnemy();
    }

    void InitEnemy() 
    {
        int upgradeAmount = FindObjectOfType<GameManager>().waveNum;
        int defenseAmount = upgradeAmount - Random.Range(upgradeAmount / 3, upgradeAmount * 2 / 3);
        for (int i = 0; i < upgradeAmount - defenseAmount; i++)
        {
            AtkUpgrade(false);
        }
        for (int i = 0; i < defenseAmount; i++)
        {
            DefUpgrade(false);
        }
    }

    /// <summary>(0)atk (1)def (2)hp (3)cri (4)cridmg (6)energy (7)e.regen</summary>
    public float GetPlayerStat()
    { 
        return FireRateCal(currentWeapon.firerate, true);
    }

    /// <summary>(0)atk (1)def (2)hp (3)cri (4)cridmg (6)energy (7)e.regen</summary>
    public int GetPlayerStat(int type) 
    {
        if (type == 0)
        {
            return AtkCal(currentWeapon.atk, true);
        }
        else if (type == 1)
        {
            return DefCal(currentArmor.def, true);
        }
        else if (type == 2)
        {
            return HpCal(currentArmor.hp, true);
        }
        else if (type == 3)
        {
            return CriCal(currentWeapon.cri, true);
        }
        else if (type == 4)
        {
            return CriDmgCal(currentWeapon.criDmg, true);
        }
        else if (type == 6)
        {
            return EnergyCal(currentArmor.energy, true);
        }
        else if (type == 7)
        {
            return EnergyRegenCal(currentArmor.energyRegen, true);
        }
        else return 0;
    }

    public void ChangeArmor(int index)
    {
        if (index != armorIndex)
        {
            print("Change Armor");
            armorIndex = index;
            currentArmor = eqToken.armorList[index];
        }
        else { Debug.Log("Same Armor"); }
    }
    public void ChangeWeapon(int index)
    {
        if (index != weaponIndex)
        {
            print("Change Weapon");
            weaponIndex = index;
            currentWeapon = eqToken.weaponList[index];
        }
        else { Debug.Log("Same Weapon"); }
    }

    public int AtkCal(int baseStat, bool isPlayer)
    {
        if (isPlayer)
        {
            return (baseStat + atkUp) * (1+Mathf.FloorToInt(criUp / 10));
        }
        else
        {
            return (baseStat + atkUpBoss) * (1 + Mathf.FloorToInt(criUpBoss / 10));
        }
    }
    public int DefCal(int baseStat, bool isPlayer)
    {
        if (isPlayer)
        {
            return (baseStat + defUp);
        }
        else
        {
            return (baseStat + defUpBoss);
        }
    }
    public int HpCal(int baseStat, bool isPlayer)
    {
        if (isPlayer)
        {
            return Mathf.CeilToInt((baseStat + (2 * hpUp)) * (1 + (0.04f * hpUp)));
        }
        else
        {
            return Mathf.CeilToInt((baseStat + (3 * hpUpBoss)) * (1 + (0.03f * hpUpBoss)));
        }     
    }
    public int CriCal(int baseStat,bool isPlayer)
    {
        if (isPlayer)
        {
            return baseStat+(10 * (criUp % 10));
        }
        else
        {
            return baseStat+(10 * (criUpBoss % 10));
        }
    }
    public int CriDmgCal(int baseStat,bool isPlayer)
    {
        if (isPlayer)
        {
            return baseStat+(100 + (10 * (criUp % 10)));
        }
        else
        {
            return baseStat+(100 + (10 * (criUpBoss % 10)));
        }
    }
    public float FireRateCal(float baseStat, bool isPlayer)
    {
        if (isPlayer)
        {
            return baseStat * (Mathf.Pow(0.95f, firerateUp)); //- (0.01f*Mathf.FloorToInt(firerateUp / 10));
        }
        else
        {
            return baseStat * (Mathf.Pow(0.98f, firerateUpBoss)); //- (0.01f * Mathf.FloorToInt(firerateUpBoss / 10));
        }
        
    }
    public int EnergyCal(int baseStat, bool isPlayer)
    {
        return baseStat + (90*energyUp);
    }
    public int EnergyRegenCal(int baseStat, bool isPlayer)
    {
        return baseStat + Mathf.FloorToInt(energyUp/10);
    }
    public int BaseEnergyCal(bool isPlayer)
    {
        if (isPlayer)
        {
            return 100 + (20 * b_energyUp);
        }
        else
        {
            return 100 + (20 * (lvBoss+hpUpBoss));
        }
    }
    public int BaseEnergyRegenCal(bool isPlayer)
    {
        if (isPlayer)
        {
            return 1 + Mathf.FloorToInt(b_energyUp / 10);
        }
        else
        {
            return 1 + Mathf.FloorToInt((lvBoss + hpUpBoss) / 10);
        }
    }

    /// <summary>(0)MainBase (1)MinerBase (2)PowerPole (3)Box</summary>
    public int BaseHpCal(int type , bool isPlayer)
    {
        int baseStat;
        if (isPlayer)
        {
            if (type == 0) //Player-Base
            {
                baseStat = 500;
            }
            else if (type == 1) //Player-Mine       
            {
                baseStat = 300;
            }
            else if (type == 2) //Player-Pole
            {
                baseStat = 100;
            }
            else //Player-Turret
            {
                baseStat = 20;
            }
            return Mathf.RoundToInt((baseStat+(baseStat*0.1f* b_hpUp)) * (1 + (0.1f * b_hpUp)));
        }
        else
        {
            if (type == 0) //Enemy-Base
            {
                baseStat = 1000;
            }
            else //Enemy-Turret
            {
                baseStat = 20;
            }
            return Mathf.RoundToInt((baseStat + (baseStat * 0.1f * (lvBoss + hpUpBoss))) * (1 + (0.1f * (lvBoss + hpUpBoss))));
        }
    }

    /// <summary>(0)MainBase (1)MinerBase (2)PowerPole (3)Box</summary>
    public int BaseDefCal(int type, bool isPlayer)
    {
        int baseStat;
        if (isPlayer)
        {
            if (type == 0)
            {
                baseStat = 2;
            }
            else if (type == 1)
            {
                baseStat = 2;
            }
            else if (type == 2)
            {
                baseStat = 1;
            }
            else
            {
                baseStat = 1;
            }
            return baseStat + b_defUp * 2;
        }
        else
        {
            if (type == 0)
            {
                baseStat = 5;
            }
            else
            {
                baseStat = 2;
            }
            return baseStat + defUpBoss + lvBoss;
        }
    }

    public void RandomUpgrade(bool isPlayer)
    {
        int temp = Random.Range(0, 2);
        if (temp == 0)
        { 
            AtkUpgrade(isPlayer);
        }
        else
        { 
            DefUpgrade(isPlayer);
        }
    }
    /// <summary>For Player or Not</summary>
    public void AtkUpgrade(bool isPlayer)
    {
        int addPoint = Random.Range(0, 3);
        string type;
        if (isPlayer)
        {
            if (addPoint == 0)
            {
                atkUp += 1;
                type = "ATK";
            }
            else if (addPoint == 1)
            {
                criUp += 1;
                type = "CRI";
            }
            else
            {
                firerateUp += 1;
                type = "FIRERATE";
            }
            Debug.Log("Player +1 -> " + type);
        }
        else
        {
            if (addPoint == 0)
            {
                atkUpBoss += 1;
                type = "ATK";
            }
            else if (addPoint == 1)
            {
                criUpBoss += 1;
                type = "CRI";
            }
            else
            {
                //atkUpBoss += 1;
                //criUpBoss += 1;
                //type = "ATK + CRI";
                firerateUpBoss += 1;
                type = "FIRERATE";
            }
            Debug.Log("Enemy +1 -> " + type);
        }
    }
    public void DefUpgrade(bool isPlayer)
    {
        int addPoint = Random.Range(0, 3);
        string type;
        if (isPlayer)
        {
            if (addPoint == 0)
            {
                hpUp += 1;
                type = "HP";
            }
            else if (addPoint == 0)
            {
                defUp += 1;
                type = "DEF";
            }
            else
            {
                energyUp += 1;
                type = "ENERGY";
            }
            Debug.Log("Player +1 -> " + type);
        }
        else
        {
            if (addPoint == 0)
            {
                hpUpBoss += 1;
                type = "HP";
            }
            else if (addPoint == 0)
            {
                defUpBoss += 1;
                type = "DEF";
            }
            else
            {
                defUpBoss += 1;
                hpUpBoss += 1;
                type = "HP & DEF";
            }
            Debug.Log("Enemy +1 -> " + type);
        }
    }

    public int IsEnoughPoint(bool useRarePoint)
    {
        int usePoint = 1;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            usePoint = 10;
        }
        if (!useRarePoint) //use point
        {
            if (corePoint < usePoint)
            {
                print("Not Enough Point : " + (usePoint - corePoint) + "More Point(s) Needed");
                return 0;
            }
            else
            {
                corePoint -= usePoint;
                return usePoint;
            }
        }
        else //use rare point
        {
            if (rarePoint < usePoint)
            {
                print("Not Enough Point : " + (usePoint - rarePoint) + "More Point(s) Needed");
                return 0;
            }
            else
            {
                rarePoint -= usePoint;
                return usePoint;
            }
        }
    }
    public void SelectUpgrade(int type)
    {
        int usePoint;
        //Use Point Stat
        if (type < 4)
        {
            //Check Use Point
            usePoint = IsEnoughPoint(false);
            if (usePoint == 0)
            {
                return;
            }
            else if (type == 1)
            {
                atkUp += usePoint;
                //Debug.Log("Upgrade Atk");
            }
            else if (type == 2)
            {
                defUp += usePoint;
                Debug.Log("Upgrade Def");
            }
            else
            {
                hpUp += usePoint;
                Debug.Log("Upgrade HP");
            }
            spendPoint += usePoint;
        }
        //Use Rare Point Stat
        else
        {
            //Check Use Point
            usePoint = IsEnoughPoint(true);
            if (usePoint == 0)
            {
                return;
            }
            else if (type == 4)
            {
                criUp += usePoint;
                Debug.Log("Upgrade Critical");
            }
            else if (type == 5)
            {
                firerateUp += usePoint;
                Debug.Log("Upgrade Fire Rate");
                if (FindObjectOfType<Player>())
                {
                    FindObjectOfType<Player>().GetComponentInChildren<PlayerGun>().cooldown = GetPlayerStat() ;
                }
            }
            else
            {
                energyUp += usePoint;
                Debug.Log("Upgrade Energy");
                if (FindObjectOfType<Player>())
                {
                    //refresh mana
                }
            }
            spendRarePoint += usePoint;
        }
        UpdatePlayerStat(type);
        RefreshPageStat();
        RefreshPageUpgrade();
        RefreshCoreAmount();
    }

    void BaseUpgrade(int type)
    {
        int usePoint = IsEnoughPoint(false);
        if (usePoint == 0)
        {
            return;
        }
        //Base
        if (type >= 8 && type < 10)
        {
            if (type == 8)
            { b_hpUp += usePoint; }
            else if (type == 9)
            { b_defUp += usePoint; }
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
            b_energyUp += usePoint;
            if (FindObjectOfType<HQBase>())
            {
                Debug.Log("Upgrade Shield");
                FindObjectOfType<HQBase>().GetComponent<ManaSystem>().RefreshEnergy();
            }
        }
    }

    /// <summary>(1)atk (2)def (3)hp (4)cri (5)firerate (6)energy</summary>
    public void UpdatePlayerStat(int type) 
    {
        if (FindObjectOfType<Player>())
        {
            Player temp = FindObjectOfType<Player>();
            if (type == 1 || type == 4 || type == 5)
            {
                temp.UpdateGunStat();
            }
            else if (type == 2)
            {
                temp.defToken.RefreshDef();
            }
            else if ( type == 3)
            {
                temp.defToken.RefreshMaxHp();
            }
            else
            {
                temp.manaSys.RefreshEnergy();
            }
        }

    }
    public void AddPoint(int type,int amount)
    {
        if (type == 0)
        {
            corePoint += amount;
            pointText.text = corePoint.ToString();
        }
        else
        {
            rarePoint += amount;
            rarepointText.text = rarePoint.ToString();
        }
    }

    public void RefreshCoreAmount() 
    {
        pointText.text = corePoint.ToString();
        rarepointText.text = rarePoint.ToString();
    }

    public void RefreshPageUpgrade()
    {
        RefreshCoreAmount();
        /*pointText.text = corePoint.ToString();
        rarepointText.text = rarePoint.ToString();*/
        if (!FindObjectOfType<Page_Upgrade>())
        {
            return;
        }
        page_upToken = FindObjectOfType<Page_Upgrade>();
        page_upToken.atkPoint.text = atkUp.ToString();
        page_upToken.defPoint.text = defUp.ToString();
        page_upToken.hpPoint.text = hpUp.ToString();
        page_upToken.criPoint.text = criUp.ToString();
        page_upToken.fireRatePoint.text = firerateUp.ToString();
        page_upToken.energyPoint.text = energyUp.ToString();
    }

    public void RefreshPageStat()
    {
        RefreshCoreAmount();
        /*pointText.text = corePoint.ToString();
        rarepointText.text = rarePoint.ToString();*/
        if (!FindObjectOfType<Page_StatDetail>())
        {
            return;
        }
        page_statToken = FindObjectOfType<Page_StatDetail>();
        page_statToken.spendPoint.text = spendPoint.ToString();
        page_statToken.spendRarePoint.text = spendRarePoint.ToString();
        //Spend Point
        page_statToken.atkPoint.text = atkUp.ToString();
        page_statToken.defPoint.text = defUp.ToString();
        page_statToken.hpPoint.text = hpUp.ToString();
        page_statToken.criPoint.text = criUp.ToString();
        page_statToken.criDmgPoint.text = criUp.ToString();
        page_statToken.fireRatePoint.text = firerateUp.ToString();
        page_statToken.energyPoint.text = energyUp.ToString();
        //Result
        page_statToken.atkResult.text = AtkCal(currentWeapon.atk, true).ToString();
        page_statToken.defResult.text = DefCal(currentArmor.def, true).ToString();
        page_statToken.hpResult.text = HpCal(currentArmor.hp, true).ToString();
        page_statToken.criResult.text = CriCal(currentWeapon.cri, true).ToString();
        page_statToken.criDmgResult.text = CriDmgCal(currentWeapon.criDmg, true).ToString();
        page_statToken.fireRateResult.text = (1 / FireRateCal(currentWeapon.firerate, true)).ToString("F2");
        page_statToken.energyResult.text = EnergyCal(currentArmor.energy, true).ToString();
    }
    public void StatusReport(int faction)
    {
        if (faction == 0)
        {
            Debug.Log("-----Player-----");
            Debug.Log("dmgUp :" + atkUp);
            Debug.Log("defUp : " + defUp);
            Debug.Log("hpUp : " + hpUp);
            Debug.Log("criUp : " + criUp);
            Debug.Log("speedUp : " + firerateUp);
            Debug.Log("energyUp : " + energyUp);
        }
        if (faction == 1)
        {
            Debug.Log("-----Enemy-----");
            Debug.Log("lvBoss : " + lvBoss);
            Debug.Log("dmgUpBoss : " + atkUpBoss);
            Debug.Log("criUpBoss : " + criUpBoss);
            Debug.Log("speedUpBoss : " + firerateUpBoss);
            Debug.Log("hpUpBoss : " + hpUpBoss);
            Debug.Log("defUpBoss : " + defUpBoss);
        }
    }
    public void SaveStat()
    {
        int t_atkUp = Mathf.CeilToInt(atkPre + (atkUp / 10f));
        int t_defUp = Mathf.CeilToInt(defPre + (defUp / 10f));
        int t_hpUp = Mathf.CeilToInt(hpPre + (hpUp / 10f));
        int t_criUp = Mathf.CeilToInt(criPre + (criUp / 10f));
        int t_firerateUp = Mathf.CeilToInt(fireratePre + (firerateUp / 10f));
        int t_energyUp = Mathf.CeilToInt(energyPre + (energyUp / 10f));
        PlayerPrefs.SetInt("atkUp", t_atkUp);
        PlayerPrefs.SetInt("defUp", t_defUp);
        PlayerPrefs.SetInt("hpUp", t_hpUp);
        PlayerPrefs.SetInt("criUp", t_criUp);
        PlayerPrefs.SetInt("firerateUp", t_firerateUp);
        PlayerPrefs.SetInt("energyUp", t_energyUp);
        PlayerPrefs.SetInt("lastWave", FindObjectOfType<GameManager>().waveNum);
    }

    public void LoadStat()
    {
        atkPre = PlayerPrefs.GetInt("atkUp", 0);
        defPre = PlayerPrefs.GetInt("defUp", 0);
        hpPre = PlayerPrefs.GetInt("hpUp", 0);
        criPre = PlayerPrefs.GetInt("criUp", 0);
        fireratePre = PlayerPrefs.GetInt("firerateUp", 0);
        energyPre = PlayerPrefs.GetInt("energyUp", 0);
        atkUp = atkPre;
        defUp = defPre;
        hpUp = hpPre;
        criUp = criPre;
        firerateUp = fireratePre;
        energyUp = energyPre;
        //base
        b_atkUp = Mathf.CeilToInt( atkPre /2);
        b_defUp = Mathf.CeilToInt(defPre/2);
        b_hpUp = Mathf.CeilToInt(hpPre/2);
        b_criUp = Mathf.CeilToInt(criPre/2);
        b_firerateUp = Mathf.CeilToInt(fireratePre/2);
        b_energyUp = Mathf.CeilToInt(energyPre/2);
    }
}
