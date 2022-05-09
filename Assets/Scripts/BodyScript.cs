using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyScript : MonoBehaviour
{
    [SerializeField] Animator animController;
    [SerializeField] Rigidbody2D bodyPhysic;
    [SerializeField] DefenseSystem defToken;
    [SerializeField] Player playerToken;
    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        defToken = GetComponentInParent<DefenseSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animController.SetBool("Dead", defToken.isDead);
    }
}
