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
        animController = this.GetComponent<Animator>();
        bodyPhysic = GetComponentInParent<Rigidbody2D>();
        defToken = GetComponentInParent<DefenseSystem>();
        playerToken = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animController.SetFloat("Xspeed", bodyPhysic.velocity.x);
        animController.SetFloat("Yspeed", bodyPhysic.velocity.y);
        animController.SetBool("Dead", defToken.isDead);
        animController.SetBool("IsJump", playerToken.jump);
        animController.SetBool("IsWalk", playerToken.walk);
    }
}
