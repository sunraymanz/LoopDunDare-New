using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectUI : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Upgradable upgradableToken;
    [SerializeField] GameObject upgradeButton;
    [SerializeField] GameObject recycleButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || target == null)
        {
            // Do click stuff here
            upgradeButton.SetActive(false);
            recycleButton.SetActive(false);
        }
        if (upgradeButton.active || recycleButton.active)
        {
            UpdateButton();
            print("Object Ui active");
        }
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
        upgradableToken = target.GetComponent<Upgradable>();
        upgradeButton.SetActive(true);
        recycleButton.SetActive(true);
        transform.position = new Vector2(target.transform.position.x,-5.75f);
        FindObjectOfType<Inspector>().ShowInfoPage(target) ;
    }

    void UpdateButton() 
    {
        upgradeButton.GetComponent<Button>().interactable = upgradableToken.upgradability;
        recycleButton.GetComponent<Button>().interactable = upgradableToken.recyclity;  
    }




}
