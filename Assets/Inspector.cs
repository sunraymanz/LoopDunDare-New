using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    public GameObject target;
    [SerializeField] GameObject idlePage;
    [SerializeField] GameObject infoPage;
    // Start is called before the first frame update
    void Start()
    {
        target = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            target = gameObject;
        }
        if (target == gameObject || target == null)
        {
             ShowIdlePage(); 
        }      
    }

    void ShowIdlePage() 
    {
        idlePage.SetActive(true);
        infoPage.SetActive(false);
    }
    public void ShowInfoPage(GameObject obj)
    {
        target = obj;
        infoPage.SetActive(true);
        idlePage.SetActive(false);
        GetComponentInChildren<Inspector_Info>().SetDefault();
    }
}
