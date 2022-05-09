using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    string levelText;
    public EnemyGun enemyToken;
    public StatCalculator statToken;
    // Start is called before the first frame update
    void Start()
    {
        statToken = FindObjectOfType<StatCalculator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
    }

    public void RefreshLV()
    {
        if (enemyToken != null)
        {
            if (statToken.lvBoss < 10)
            { levelText = "0" + statToken.lvBoss; }
            else levelText = statToken.lvBoss.ToString();
            GetComponent<TextMeshPro>().text = levelText;
        }
        else
        {
            /*if (statToken.lv < 10)
            { levelText = "0" + statToken.lv; }
            else levelText = statToken.lv.ToString();
            GetComponent<TextMeshProUGUI>().text = levelText;*/
        }
    }
}
