using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployBox : MonoBehaviour
{
    int goldReturn;
    public int energy;
    public GameObject gunToken;
    public bool isActive = false;
    public Animator bodyController;
    public Animator gunController;
    // Start is called before the first frame update
    void Start()
    {
        SetActive();
        energy = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void SetActive()
    {
        gunToken.SetActive(true);
        isActive = true;
    }

    void SetInActive()
    {
        gunToken.SetActive(false);
        isActive = false;
    }

    void CheckEnergy()
    {
        if (energy > 100)
        {
            if (!isActive)
            { SetActive(); }
            energy = 100;
        }
        if (energy < 0)
        {
            SetInActive();
            energy = 0;
        }
        bodyController.SetInteger("Energy", energy);
        if (isActive)
        { gunController.SetInteger("Energy", energy); }
    }

}
