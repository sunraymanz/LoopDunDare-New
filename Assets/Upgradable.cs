using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Upgradable : MonoBehaviour
{
    public int tier = 0;
    public int maxTier;
    public int oreCost;
    public int coreCost;
    public int rareCost;
    [SerializeField] int oreRefund;
    [SerializeField] int coreRefund;
    [SerializeField] int rareRefund;
    [SerializeField] GameManager token;
    [SerializeField] StatCalculator statToken;
    public bool recyclity;
    public bool upgradability;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        statToken = FindObjectOfType<StatCalculator>();
        //maxTier = 99;
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool IsAffordable() 
    {
        if (token.oreAmount >= oreCost && statToken.corePoint >= coreCost && statToken.rarePoint >= rareCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void UpgradeProcess() 
    {
        if (IsAffordable())
        {
            CostCalculation();
            DoUpgrade();
            token.ShowWarning("!!! Upgrade Complete !!!");
        }
        else { token.ShowWarning("!!! Cannot Afford !!!",true); }
        FindObjectOfType<Inspector_Info>().SetDefault();
        CheckUpgradable();
    }
    public void RecycleProcess()
    {
        if (recyclity)
        {
            RefundCalculation();
            token.ShowWarning("!!! Recycle Complete !!!");
            FindObjectOfType<Inspector_Info>().SetDefault();
            Destroy(gameObject);
        }
        else 
        { 
            token.ShowWarning("!!! Cannot Be Recycle !!!",true);
            FindObjectOfType<Inspector_Info>().SetDefault();
        }       
        
    }

    void CostCalculation() 
    {
        token.oreAmount -= oreCost;
        statToken.corePoint -= coreCost;
        statToken.rarePoint -= rareCost;
        statToken.RefreshCoreAmount();
        oreRefund += Mathf.FloorToInt(oreCost/2);
        coreRefund += Mathf.FloorToInt(coreCost / 2);
        rareRefund += Mathf.FloorToInt(rareCost / 2);
    }
    void DoUpgrade() 
    {
        tier += 1;
        int hpAdd = 0;
        int defAdd = 0;
        if (GetComponent<HQBase>())
        {
            hpAdd = 20;
            defAdd = 20;
            GetComponent<ManaSystem>().PercentAdd(10);
            print("this is base " + hpAdd+ ":"+defAdd);
        }
        else if (GetComponent<MinerBase>())
        {
            hpAdd = 25;
            defAdd = 15;
        }
        else if (GetComponent<DeployBox>())
        {
            hpAdd = 15;
            defAdd = 25;
        }
        else if (GetComponent<PowerPole>())
        {
            hpAdd = 15;
            defAdd = 20;
            GetComponent<PowerPole>().healAmount *= 2;
        }
        GetComponent<DefenseSystem>().PercentAdd(hpAdd,defAdd*tier);
        oreCost = Mathf.FloorToInt(oreCost* 1.25f);
        coreCost += tier;
        rareCost = tier / 5;     
    }
    void RefundCalculation() 
    {
        token.oreAmount += oreRefund;
        statToken.corePoint += coreRefund;
        statToken.rarePoint += rareRefund;
    }

    public int GetCostValue(int type) 
    {
        if (type == 1)
        {
            return oreCost;
        }
        else if (type == 2)
        {
            return coreCost;
        }
        else
        {
            return rareCost;
        }
    }
    public int GetRefundValue(int type)
    {
        if (type == 1)
        {
            return oreRefund;
        }
        else if (type == 2)
        {
            return coreRefund;
        }
        else
        {
            return rareRefund;
        }
    }

    void CheckUpgradable() 
    {
        if (tier == maxTier)
        {
            upgradability = false;
        }
        else
        {
            upgradability = true;
        }
    }
}
