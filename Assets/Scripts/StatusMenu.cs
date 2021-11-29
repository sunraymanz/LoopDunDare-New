using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusMenu : MonoBehaviour
{
    public GameManager token;
    public Animator animToken;
    bool isShow = false;
    public TextMeshProUGUI lvPoint;
    public TextMeshProUGUI dmgUp ;
    public TextMeshProUGUI criUp;
    public TextMeshProUGUI criDamageUp ;
    public TextMeshProUGUI modUp;
    public TextMeshProUGUI speedUp ;
    public TextMeshProUGUI hpUp ;
    public TextMeshProUGUI defUp ;
    //Base Status
    public TextMeshProUGUI basehpUp ;
    public TextMeshProUGUI basedefUp ;
    public TextMeshProUGUI baseRegenUp;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
        animToken = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleShow()
    {
        isShow = !isShow;
        animToken.SetBool("IsShow", isShow);
    }

    public void RefreshStat()
    {
        lvPoint.text= "Point="+ token.lvPoint;
        dmgUp.text = "Dmg=" + token.dmgUp;
        criUp.text = "Cri=" + token.criUp;
        criDamageUp.text = "C.Dm=" + token.criDamageUp;
        modUp.text = "Mod=" + token.modUp;
        speedUp.text = "Spd=" + token.speedUp;
        hpUp.text = "Hp=" + token.hpUp;
        defUp.text = "Def=" + token.defUp;
        //Base Status
        basehpUp.text = "B.Hp=" + token.basehpUp;
        basedefUp.text = "B.Df=" + token.basedefUp;
        baseRegenUp.text = "B.Sh=" + token.baseShiledUp;
    }

    private void OnMouseOver()
    {
        FindObjectOfType<MyCursor>().onMenu = true;
    }
}
