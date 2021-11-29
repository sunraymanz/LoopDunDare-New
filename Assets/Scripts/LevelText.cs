using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    string levelText;
    public EnemyGun enemyToken;
    public GameManager token;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
    }

    public void RefreshLV()
    {
        if (enemyToken != null)
        {
            if (token.lvBoss < 10)
            { levelText = "0" + token.lvBoss; }
            else levelText = token.lvBoss.ToString();
            GetComponent<TextMeshPro>().text = levelText;
        }
        else
        {
            if (token.lv < 10)
            { levelText = "0" + token.lv; }
            else levelText = token.lv.ToString();
            GetComponent<TextMeshProUGUI>().text = levelText;
        }
    }
}
