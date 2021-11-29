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
    public TurrentGun gunToken;
    public DefenseSystem defSys;
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
        audioPlayer = this.GetComponent<AudioSource>();
        defSys = this.GetComponent<DefenseSystem>();
        gunToken = this.GetComponentInChildren<TurrentGun>();
        manaSys = this.GetComponent<ManaSystem>();
        token = FindObjectOfType<GameManager>();
        blinkPoint.localPosition = new Vector3(xDirection * blinkDistance, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<DefenseSystem>().isDead)
        {          
            CheckKeyPress();
            FrameCheck();
        }
    }

    private void FixedUpdate()
    {

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
            if (manaSys.turbo > 0)
            {
                jump = true;
                animController.SetBool("IsJump", jump);
                Jump();
            }   
        }

        if (Input.GetButtonDown("Dash"))
        {
            if (manaSys.turbo > 0)
            {
                Blink();
            }
        }
        if (Input.GetKeyDown(KeyCode.E)) // FOR TEST
        {
            Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(token.deployPrefab, temp, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.R)) // FOR TEST
        {
            token.ready = true;
            //token.RespawnEnemy();
        }
        if (Input.GetKeyDown(KeyCode.Y)) // FOR TEST
        {
            Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(token.orePrefab, temp, Quaternion.identity);
        }

        if (Input.GetMouseButton(0))
        {
            gunToken.Attack(0);
        }
        if (Input.GetMouseButton(1))
        {
            gunToken.Attack(1);
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

    public void Upgrade()
    {
        defSys.RefreshMaxHp();
        //token.StatusReport(0);
    }

    public void GotUpgrade()
    {
        
        //token.StatusReport(0);
    }

    void Jump()
    {
        bodyPhysic.velocity = new Vector2(bodyPhysic.velocity.x, 0f);
        bodyPhysic.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        manaSys.turbo -= 1;
    }

    void Blink()
    {
        bodyPhysic.velocity = new Vector2(bodyPhysic.velocity.x, 0f);
        Instantiate(blinkFrom, transform.position, Quaternion.identity);
        BlinkCheck();
        //FrameCheck();
        Instantiate(blinkTo, transform.position, Quaternion.identity);
        manaSys.turbo -= 1;
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
            this.transform.position = new Vector3(transform.position.x, -4, transform.position.z);
            Instantiate(blinkTo, transform.position, Quaternion.identity);
            manaSys.turbo -= 1;
        }

        /*if (this.transform.position.x > 11f)
        {
            if (manaSys.turbo == 0)
            {
                defSys.hp -= 10;
            }
            else
            {
                manaSys.turbo -= 1;
                this.transform.position = new Vector3(-10f, transform.position.y, transform.position.z);
            }
        }
        if (this.transform.position.x < -11f)
        {
            if (manaSys.turbo == 0)
            {
                defSys.hp -= 10;
            }
            else
            {
                manaSys.turbo -= 1;
                this.transform.position = new Vector3(10f, transform.position.y, transform.position.z);
            }
        }*/
    }
}
