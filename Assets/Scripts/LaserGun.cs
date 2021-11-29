using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    Animator animToken;
    // Start is called before the first frame update
    void Start()
    {
        animToken = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animToken.SetBool("Dead", GetComponentInParent<DefenseSystem>().isDead);
    }
}
