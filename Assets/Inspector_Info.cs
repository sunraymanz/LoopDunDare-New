using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inspector_Info : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject infoPage;
    [SerializeField] GameObject upgradePage;
    [SerializeField] GameObject recyclePage;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI tierValue;
    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] TextMeshProUGUI maxHpValue;
    [SerializeField] TextMeshProUGUI shieldValue;
    [SerializeField] TextMeshProUGUI maxShieldValue;
    [SerializeField] TextMeshProUGUI atkValue;
    [SerializeField] TextMeshProUGUI defValue;
    [SerializeField] TextMeshProUGUI criValue;
    [SerializeField] TextMeshProUGUI criDmgValue;
    [SerializeField] TextMeshProUGUI oreCost;
    [SerializeField] TextMeshProUGUI coreCost;
    [SerializeField] TextMeshProUGUI rareCost;
    [SerializeField] TextMeshProUGUI oreRefund;
    [SerializeField] TextMeshProUGUI coreRefund;
    [SerializeField] TextMeshProUGUI rareRefund;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (infoPage.active)
        {
            RefreshValue();
        }
    }

    public void ShowRecyclePage()
    {
        if (recyclePage.active)
        {
            target.GetComponent<Upgradable>().RecycleProcess();
            return;
        }
        infoPage.SetActive(false);
        upgradePage.SetActive(false);
        recyclePage.SetActive(true);
        SetRefundValue();
    }

    public void ShowUpgradePage()
    {
        if (upgradePage.active)
        {
            target.GetComponent<Upgradable>().UpgradeProcess();
            return;
        }
        infoPage.SetActive(false);
        upgradePage.SetActive(true);
        recyclePage.SetActive(false);
        SetCostValue();
    }
    public void ShowInfoPage()
    {
        infoPage.SetActive(true);
        upgradePage.SetActive(false);
        recyclePage.SetActive(false);
    }

    void SetCostValue() 
    {
        string ore = "-"+target.GetComponent<Upgradable>().GetCostValue(1);
        string core= "-"+target.GetComponent<Upgradable>().GetCostValue(2);
        string rare= "-"+target.GetComponent<Upgradable>().GetCostValue(3);
        oreCost.text = ore;
        coreCost.text = core;
        rareCost.text = rare;
    }

    void SetRefundValue()
    {
        string ore = "+" + target.GetComponent<Upgradable>().GetRefundValue(1);
        string core = "+" + target.GetComponent<Upgradable>().GetRefundValue(2);
        string rare = "+" + target.GetComponent<Upgradable>().GetRefundValue(3);
        oreRefund.text = ore;
        coreRefund.text = core;
        rareRefund.text = rare;
    }

    public void SetDefault()
    {
        tierValue.text = "0";
        atkValue.text = "0";
        criValue.text = "0";
        criDmgValue.text = "0";
        defValue.text = "0";
        hpValue.text = "0";
        maxHpValue.text = "0";
        shieldValue.text = "0";
        maxShieldValue.text = "0";
        ShowInfoPage();
        print("Inspector Reset");
    }

    void RefreshValue() 
    {
        target = GetComponentInParent<Inspector>().target;
        if (target.GetComponent<Upgradable>())
        {
            tierValue.text = target.GetComponent<Upgradable>().tier.ToString();
        }
        if (target.GetComponent<CommonAsset>())
        {
            icon.sprite = target.GetComponent<CommonAsset>().icon;
        }
        if (target.GetComponentInChildren<PlayerGun>())
        {
            atkValue.text = target.GetComponentInChildren<PlayerGun>().damage.ToString();
            criValue.text = target.GetComponentInChildren<PlayerGun>().criChance.ToString();
            criDmgValue.text = target.GetComponentInChildren<PlayerGun>().criDamage.ToString();
        }
        if (target.GetComponentInChildren<EnemyGun>())
        {          
            atkValue.text = target.GetComponentInChildren<EnemyGun>().damage.ToString();
            criValue.text = target.GetComponentInChildren<EnemyGun>().criChance.ToString();
            criDmgValue.text = target.GetComponentInChildren<EnemyGun>().criDamage.ToString();
        }
        if (target.GetComponent<DefenseSystem>())
        {
            defValue.text = target.GetComponent<DefenseSystem>().def.ToString();
            hpValue.text = target.GetComponent<DefenseSystem>().hp.ToString();
            maxHpValue.text = target.GetComponent<DefenseSystem>().maxHp.ToString();
        }
        if (target.GetComponent<ManaSystem>())
        {
            shieldValue.text = target.GetComponent<ManaSystem>().mp.ToString();
            maxShieldValue.text = target.GetComponent<ManaSystem>().maxMp.ToString(); 
        }
    }
}
