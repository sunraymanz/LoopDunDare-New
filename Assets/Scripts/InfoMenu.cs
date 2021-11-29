using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoMenu : MonoBehaviour
{
    public GameManager token;
    public Player playerToken;
    public HQBase HQToken;
    public Animator animToken;
    bool isShow = false;
    public bool isBusy = false;
    int page = 1;
    //Atk
    public TextMeshProUGUI dmgUp;
    public TextMeshProUGUI criUp;
    public TextMeshProUGUI criDamageUp;
    public TextMeshProUGUI fireRate;
    public TextMeshProUGUI defUp;
    //Mana
    public TextMeshProUGUI energy;
    public TextMeshProUGUI eRegen;
    public TextMeshProUGUI eUsage;
    public TextMeshProUGUI tRegen;
    //Enemy Status
    public TextMeshProUGUI worldLV;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        playerToken = FindObjectOfType<Player>();
        HQToken = FindObjectOfType<HQBase>();
        animToken = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animToken.SetBool("Busy", isBusy);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleShow();
            RefreshStat();
        }
        if (playerToken != null)
        {
            return;
        }
        else
        {
            if (FindObjectOfType<Player>())
            {
                playerToken = FindObjectOfType<Player>();
            }
        }
    }

    public void ToggleShow()
    {
        if (!isBusy)
        {
            isShow = !isShow;
            animToken.SetBool("IsShow", isShow);
        }
        
    }
    public void CheckPage()
    {
        if (page == 1)
        {
            Debug.Log("Button Pressed -> World");
            page = 2;
        }
        else
        {
            Debug.Log("Button Pressed -> Player");
            page = 1;
        }
    }

    public void RefreshStat()
    {
        if (page == 1)
        {
            PlayerStat();
        }
        else 
        {   WorldStat(); }
    }
    public void PlayerStat()
    {
        if (playerToken != null)
        {
            TurrentGun temp = playerToken.GetComponentInChildren<TurrentGun>();
            dmgUp.text = "Damage=" + temp.damage;
            criUp.text = "CriRate=" + temp.criChance + "%";
            criDamageUp.text = "CriDmg=+" + temp.criDamage + "%";
            defUp.text = "Defense=" + playerToken.GetComponent<DefenseSystem>().def;
            fireRate.text = "F.Rate=" + temp.shootCooldown.ToString("F2") + "s/1T";
            //Mana
            ManaSystem temp2 = playerToken.GetComponentInChildren<ManaSystem>();
            energy.text = "Energy=" + temp2.maxMp;
            eRegen.text = "E.Regen=" + temp2.recovRate + "p";
            eUsage.text = "E.Usage=" + temp2.useRate + "%";
            tRegen.text = "Move.Re=" + temp2.refillTime.ToString("F2") + "s/1T";
            //Enemy Status
            if (temp.lastUseType == 0)
            { worldLV.text = "Machine Gun"; }
            else { worldLV.text = "Laser Gun"; }
            
        }
    }
    public void WorldStat()
    {
        if (FindObjectOfType<Enemy>())
        {
            EnemyGun temp = FindObjectOfType<Enemy>().GetComponentInChildren<EnemyGun>();
            dmgUp.text = "EnemyDmg=" + temp.damage;
            criUp.text = "E.CriRate=" + temp.criChance + "%";
            criDamageUp.text = "E.CriDmg=+" + temp.criDamage + "%";
            defUp.text = "E.Defense=" + FindObjectOfType<Enemy>().GetComponent<DefenseSystem>().def;
            fireRate.text = "F.Rate=" + temp.shootCooldown.ToString("F2") + "s/1T";
            //Mana
        }
        else { NullData(); }
        ManaSystem temp2 = HQToken.GetComponentInChildren<ManaSystem>();
        energy.text = "Shield=" + temp2.maxMp;
        eRegen.text = "S.Regen=" + temp2.recovRate + "p";
        eUsage.text = "S.Usage=" + temp2.useRate + "%";
        tRegen.text = "B.Defense=" + HQToken.GetComponent<DefenseSystem>().def;
        //Enemy Status
        worldLV.text = "Enemy.LV=" + token.lvBoss;
    }

    void NullData()
    {
        dmgUp.text = "EnemyDmg="+" ---";
        criUp.text = "E.CriRate=" + " ---";
        criDamageUp.text = "E.CriDmg=+" + " ---";
        defUp.text = "E.Defense="+ " ---"; 
        fireRate.text = "F.Rate=" + " ---";
    }
}
