using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReminderText : MonoBehaviour
{
    [SerializeField] GameObject startReminder;
    [SerializeField] GameObject enemyCountReminder;
    [SerializeField] Transform enemylist;
    [SerializeField] int prevCount = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void LateUpdate()
    {
        if (enemyCountReminder.activeSelf)
        {
            if (prevCount != enemylist.childCount)
            {
                enemyCountReminder.GetComponent<Animation>().Play();
                enemyCountReminder.GetComponent<TextMeshProUGUI>().text = enemylist.childCount + " enemy left";
                prevCount = enemylist.childCount;
            }
        }
    }
    public void InitiateState()
    {
        StartReminderActive(true);
        enemyCountReminder.GetComponent<TextMeshProUGUI>().text = 0 + " enemy left";
        EnemyCountActive(false); 
    }
    public void StartWaveState()
    {
        StartReminderActive(false) ;
        EnemyCountActive(true); 
    }
    public void EndState()
    {
        StartReminderActive(false); 
        EnemyCountActive(false); 
    }
    void EnemyCountActive(bool b)
    {
        enemyCountReminder.SetActive(b);
    }
    void StartReminderActive(bool b)
    {
        startReminder.SetActive(b);
    }
}
