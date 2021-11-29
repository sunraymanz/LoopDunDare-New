using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegScript : MonoBehaviour
{
    [SerializeField] Animator animController;
    [SerializeField] Player playerToken;
    [SerializeField] Enemy enemyToken;
    [SerializeField] Rigidbody2D bodyPhysic;
    [SerializeField] DefenseSystem defToken;
    bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        animController = this.GetComponent<Animator>();
        if (GetComponentInParent<Player>() != null)
        { 
            playerToken = GetComponentInParent<Player>();
            isPlayer = true;
        }
        else
        {
            enemyToken = GetComponentInParent<Enemy>();
            isPlayer = false;
        }
        bodyPhysic = GetComponentInParent<Rigidbody2D>();
        defToken = GetComponentInParent<DefenseSystem>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animController.SetFloat("Xspeed", bodyPhysic.velocity.x);
        if (isPlayer)
        {
            animController.SetFloat("Yspeed", bodyPhysic.velocity.y);
            animController.SetBool("IsJump", playerToken.jump);
            animController.SetBool("IsWalk", playerToken.walk);
        }
        else 
        {
            animController.SetBool("IsWalk", enemyToken.walk);
        }
        animController.SetBool("Dead", defToken.isDead);
    }
}
