using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefreshAmount : MonoBehaviour
{
    GameManager token;
    GameObject refToken;
    public int refAmount;
    public int refIndex;
    int amount = 0;
    public TextMeshProUGUI refTarget;
    // Start is called before the first frame update
    void Start()
    {
        token = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetRefAmount(refIndex);
        if (amount < refAmount)
        {
            if (refAmount - amount > 10000)
            {
                amount += (int)((refAmount - amount) / 100);
            }
            else if (refAmount - amount > 100)
            { 
                amount += (int)((refAmount - amount)/10); 
            }
            else
            { amount++; }
            refTarget.text = amount.ToString();
        }
        else 
        {
            amount = refAmount;
            refTarget.text = amount.ToString();
        }
    }

    void GetRefAmount(int index)
    {
        if (index < 5)
        {
            if (index == 1)
            { refAmount = token.price_PP; }
            else if (index == 2)
            { refAmount = token.price_MB; }
            else if (index == 3)
            { refAmount = token.price_BT; }
            else
            { refAmount = token.price_M; }
            amount = refAmount;
        }
        else if (index == 5)
        { refAmount = token.oreAmount; }
        else if (index == 6)
        { refAmount = token.minerAmount; }
        else 
        { refAmount = token.boxAmount; }
    }
}
