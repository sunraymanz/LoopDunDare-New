using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Upgradable : MonoBehaviour
{
    public int tier = 0;
    public int maxTier;
    public int[] oreCost;
    public int[] coreCost;
    public int[] rareCost;
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
        maxTier = oreCost.Length;
    }

    // Update is called once per frame
    void Update()
    {

    }
     
    bool IsAffordable() 
    {
        if (token.oreAmount >= oreCost[tier] && statToken.corePoint >= coreCost[tier] && statToken.rarePoint >= rareCost[tier])
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
        else { token.ShowWarning("!!! Cannot Afford !!!"); }
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
            token.ShowWarning("!!! Cannot Be Recycle !!!");
            FindObjectOfType<Inspector_Info>().SetDefault();
        }       
        
    }

    void CostCalculation() 
    {
        token.oreAmount -= oreCost[tier];
        statToken.corePoint -= coreCost[tier];
        statToken.rarePoint -= rareCost[tier];
        oreRefund += Mathf.FloorToInt(oreCost[tier]/2);
        coreRefund += Mathf.FloorToInt(coreCost[tier] / 2);
        rareRefund += Mathf.FloorToInt(rareCost[tier] / 2);
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
            hpAdd = 20;
            defAdd = 15;
        }
        else if (GetComponent<DeployBox>())
        {
            hpAdd = 10;
            defAdd = 25;
        }
        else if (GetComponent<PowerPole>())
        {
            hpAdd = 15;
            defAdd = 20;
            GetComponent<PowerPole>().healAmount *= 2;
        }
        GetComponent<DefenseSystem>().PercentAdd(hpAdd,defAdd);
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
            return oreCost[tier];
        }
        else if (type == 2)
        {
            return coreCost[tier];
        }
        else
        {
            return rareCost[tier];
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
