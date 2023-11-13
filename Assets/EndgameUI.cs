using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndgameUI : MonoBehaviour
{
    bool isShow;
    public StatCalculator statToken;
    //Remain Core
    public TextMeshProUGUI remainCore;
    public TextMeshProUGUI remainRareCore;
    //Previous
    public TextMeshProUGUI atkPre;
    public TextMeshProUGUI defPre;
    public TextMeshProUGUI hpPre;
    public TextMeshProUGUI criPre;
    public TextMeshProUGUI fireRatePre;
    public TextMeshProUGUI energyPre;
    //Current
    public TextMeshProUGUI atkPoint;
    public TextMeshProUGUI defPoint;
    public TextMeshProUGUI hpPoint;
    public TextMeshProUGUI criPoint;
    public TextMeshProUGUI fireRatePoint;
    public TextMeshProUGUI energyPoint;
    //Result
    public TextMeshProUGUI atkResult;
    public TextMeshProUGUI defResult;
    public TextMeshProUGUI hpResult;
    public TextMeshProUGUI criResult;
    public TextMeshProUGUI fireRateResult;
    public TextMeshProUGUI energyResult;
    // Start is called before the first frame update
    void Start()
    {
        statToken = FindObjectOfType<StatCalculator>();
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Refresh()
    {
        //Core Count
        remainCore.text = statToken.corePoint.ToString();
        remainRareCore.text = statToken.rarePoint.ToString();
        //Pre Bonus
        atkPre.text = "+"+statToken.atkPre.ToString();
        defPre.text = "+" + statToken.defPre.ToString();
        hpPre.text = "+" + statToken.hpPre.ToString();
        criPre.text = "+" + statToken.criPre.ToString();
        fireRatePre.text = "+" + statToken.fireratePre.ToString();
        energyPre.text = "+" + statToken.energyPre.ToString();
        //Current
        atkPoint.text = statToken.atkUp + " / 10";
        defPoint.text = statToken.defUp + " / 10";
        hpPoint.text = statToken.hpUp + " / 10";
        criPoint.text = statToken.criUp + " / 10";
        fireRatePoint.text = statToken.firerateUp + " / 10";
        energyPoint.text = statToken.energyUp + " / 10";
        //Last result
        atkResult.text = "+" + PlayerPrefs.GetInt("atkUp", 0).ToString(); 
        defResult.text = "+" + PlayerPrefs.GetInt("defUp", 0).ToString();
        hpResult.text = "+" + PlayerPrefs.GetInt("hpUp", 0).ToString();
        criResult.text = "+" + PlayerPrefs.GetInt("criUp", 0).ToString();
        fireRateResult.text = "+" + PlayerPrefs.GetInt("firerateUp", 0).ToString();
        energyResult.text = "+" + PlayerPrefs.GetInt("energyUp", 0).ToString();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

}
