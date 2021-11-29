using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLegScript : MonoBehaviour
{
    [SerializeField] Animator animController;
    [SerializeField] Enemy enemyToken;
    [SerializeField] Rigidbody2D bodyPhysic;
    [SerializeField] DefenseSystem defToken;
    // Start is called before the first frame update
    void Start()
    {
        animController = this.GetComponent<Animator>();
        enemyToken = GetComponentInParent<Enemy>();
        bodyPhysic = GetComponentInParent<Rigidbody2D>();
        defToken = GetComponentInParent<DefenseSystem>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animController.SetFloat("Xspeed", bodyPhysic.velocity.x);
        animController.SetFloat("Yspeed", bodyPhysic.velocity.y);
        animController.SetBool("IsJump", enemyToken.jump);
        animController.SetBool("IsWalk", enemyToken.walk);
        animController.SetBool("Dead", defToken.isDead);
    }
}
