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
    
    // Start is called before the first frame update
    void Start()
    {      
        playerToken = FindObjectOfType<Player>();
        HQToken = FindObjectOfType<HQBase>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!token.isOnMenu)
        {
            token.isOnMenu = true;
        }       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();            
        }
    }   
    public void Close() 
    {
        gameObject.SetActive(false);
    }
    public void ToggleShow()
    {
        if (!isBusy)
        {
            isShow = !isShow;
            animToken.SetBool("IsShow", isShow);
        }
    }

    private void OnEnable()
    {
        token = FindObjectOfType<GameManager>();
        token.Pause();
        token.isOnMenu = true;
    }
    private void OnDisable()
    {
        token.Unpause();
        token.isOnMenu = false;
    }
    /*
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
    public void PlayerStat()
    {
        if (playerToken != null)
        {
            PlayerGun temp = playerToken.GetComponentInChildren<PlayerGun>();
            dmgUp.text = "Damage=" + temp.damage;
            criUp.text = "CriRate=" + temp.criChance + "%";
            criDamageUp.text = "CriDmg=+" + temp.criDamage + "%";
            defUp.text = "Defense=" + playerToken.GetComponent<DefenseSystem>().def;
            fireRate.text = "F.Rate=" + temp.cooldown.ToString("F2") + "s/1T";
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
        //worldLV.text = "Enemy.LV=" + token.lvBoss;
    }

    void NullData()
    {
        dmgUp.text = "EnemyDmg="+" ---";
        criUp.text = "E.CriRate=" + " ---";
        criDamageUp.text = "E.CriDmg=+" + " ---";
        defUp.text = "E.Defense="+ " ---"; 
        fireRate.text = "F.Rate=" + " ---";
    }*/
}
