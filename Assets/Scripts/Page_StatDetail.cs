using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Page_StatDetail : MonoBehaviour
{
    bool isShow;
    public TextMeshProUGUI spendPoint;
    public TextMeshProUGUI spendRarePoint;
    //Status
    public TextMeshProUGUI atkPoint;
    public TextMeshProUGUI defPoint;
    public TextMeshProUGUI hpPoint;
    public TextMeshProUGUI criPoint;
    public TextMeshProUGUI criDmgPoint;
    public TextMeshProUGUI fireRatePoint;
    public TextMeshProUGUI energyPoint;
    //result
    public TextMeshProUGUI atkResult;
    public TextMeshProUGUI defResult;
    public TextMeshProUGUI hpResult;
    public TextMeshProUGUI criResult;
    public TextMeshProUGUI criDmgResult;
    public TextMeshProUGUI fireRateResult;
    public TextMeshProUGUI energyResult;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        FindObjectOfType<MyCursor>().onMenu = true;
    }

    private void OnEnable()
    {
        //FindObjectOfType<StatCalculator>().RefreshPageStat();
    }

}
