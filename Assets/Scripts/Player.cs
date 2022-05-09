using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D bodyPhysic;
    public PlayerGun[] gunTokenList;
    public DefenseSystem defToken;
    public ManaSystem manaSys;
    GameManager token;
    public Transform blinkFrom;
    public Transform blinkTo;
    AudioSource audioPlayer;
    [SerializeField] Animator animController;
    [SerializeField] AudioClip[] soundClip;
    [SerializeField] Transform blinkPoint;
    [SerializeField] LayerMask collideLayer;
    public float xSpeed = 50f;
    public float jumpForce = 50f;
    float xAxisMove = 0f;
    float xDirection = 1f;
    float blinkDistance = 5f;
    [SerializeField] float blinkRadius = 0.6f;
    public bool jump = false;
    public bool walk = false;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        defToken = GetComponent<DefenseSystem>();
        gunTokenList = GetComponentsInChildren<PlayerGun>();
        manaSys = GetComponent<ManaSystem>();
        token = FindObjectOfType<GameManager>();
        blinkPoint.localPosition = new Vector3(xDirection * blinkDistance, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!defToken.isDead && !token.isOnMenu)
        {
            CheckKeyPress();
            FrameCheck();
        }
        else 
        {
            jump = false;
            walk = false;
        }
    }


    void CheckKeyPress()
    {
        xAxisMove = Input.GetAxisRaw("Horizontal") * xSpeed;
        bodyPhysic.velocity = new Vector2(xAxisMove, bodyPhysic.velocity.y);
        animController.SetBool("IsJump", false);
        if (Input.GetButton("Horizontal"))
        {
            xDirection = Input.GetAxisRaw("Horizontal");
            blinkPoint.localPosition = new Vector3(xDirection * blinkDistance, 0, 0);
            walk = true;
            animController.SetBool("IsWalk", walk);
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            walk = false;
            animController.SetBool("IsWalk", walk);
        }
        if (Input.GetButtonDown("Jump"))
        {           
            Jump();
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (true)
            {
            Blink();
            }
        }   
        if (Input.GetMouseButton(0))
        {
            foreach (var gun in gunTokenList)
            {
                gun.Attack();
            }
            //gunToken.Attack();
        }
        animController.SetFloat("Xspeed", bodyPhysic.velocity.x);
        animController.SetFloat("Yspeed", bodyPhysic.velocity.y);      

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            audioPlayer.PlayOneShot(soundClip[0], 0.5f);
        }
        if (collision.gameObject.tag == "Ground")
        {
            //bodyPhysic.velocity = Vector2.zero;
            jump = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            audioPlayer.PlayOneShot(soundClip[0], 0.5f);
        }
    }

    public void UpdateGunStat()
    {
        foreach (var item in gunTokenList)
        {
            item.AttackCalculate(false);
        }
    }

    void Jump()
    {
        if (manaSys.CheckMana(50, true))
        {
            manaSys.BurnPercentMana(50);
            jump = true;
            animController.SetBool("IsJump", jump);
            bodyPhysic.velocity = new Vector2(bodyPhysic.velocity.x, 0f);
            bodyPhysic.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void Blink()
    {
        if (manaSys.CheckMana(50, true))
        {
            manaSys.BurnPercentMana(50);
            bodyPhysic.velocity = new Vector2(bodyPhysic.velocity.x, 0f);
            Instantiate(blinkFrom, transform.position, Quaternion.identity);
            BlinkCheck();
            Instantiate(blinkTo, transform.position, Quaternion.identity);
        }
    }

    void BlinkCheck()
    {
        Collider2D wallcheck = Physics2D.OverlapCircle(blinkPoint.position,blinkRadius, LayerMask.GetMask("Stage"));
        if (wallcheck == null)
        {
            //Debug.Log("Nothing");
            this.transform.position += new Vector3(xDirection * blinkDistance, 0f, 0f);
            return;
        }

        RaycastHit2D temp = Physics2D.CircleCast(transform.position, blinkRadius,xDirection*Vector2.right,blinkDistance,LayerMask.GetMask("Stage"));
        if (temp.collider != null)
        {
            //Debug.Log("Hit Obj : " + temp.collider.name);
            //Debug.Log("Hit x : " + temp.point.x);
            //Debug.Log("Hit y : " + temp.point.y);
            this.transform.position = new Vector2 (temp.point.x-(0.9f*xDirection), temp.point.y);
        }
    }

    private void OnDrawGizmos()
    {
        if (blinkPoint == null)
        { return; }
        Gizmos.DrawWireSphere(blinkPoint.position, blinkRadius);
    }

    void FrameCheck()
    {
        if (this.transform.position.y > 10f)
        {
            bodyPhysic.velocity = new Vector2(bodyPhysic.velocity.x, 0f);
            Instantiate(blinkFrom, transform.position, Quaternion.identity);
            transform.position = new Vector3(transform.position.x, -4, transform.position.z);
            Instantiate(blinkTo, transform.position, Quaternion.identity);
        }
    }
}
