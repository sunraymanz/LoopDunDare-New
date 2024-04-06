using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D bodyPhysic;
    AudioSource audioToken;
    [SerializeField] AudioClip[] soundClip;
    public DefenseSystem defSys;
    public Transform playerToken;
    public EnemyGun gunToken;
    public CanvasScript canvasToken;

    public bool isFight = false;
    public float xSpeed = 1f;
    public float jumpForce = 50f;
    [SerializeField] float xAxisDirection = 0f;
    public bool jump = false;
    public bool walk = false;
    public bool busy = false;


    // Start is called before the first frame update
    void Start()
    {
        audioToken = this.GetComponent<AudioSource>();
        defSys = this.GetComponent<DefenseSystem>();
        gunToken = this.GetComponentInChildren<EnemyGun>();
        canvasToken = this.GetComponentInChildren<CanvasScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (defSys.isDead)
        {
            bodyPhysic.constraints = RigidbodyConstraints2D.None;
        }
        if (tag == "Drone" )
        {
            if (transform.position.y > 1) transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, 1), Time.deltaTime * 1);
            else if (transform.position.y < 1) transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, 1), Time.deltaTime * 1);
        }
        float distance;
        if (FindObjectOfType<Player>())
        {
            playerToken = FindObjectOfType<Player>().transform;
            distance = playerToken.position.x - transform.position.x;
        }
        else distance = 0 - transform.position.x;
        xAxisDirection = Mathf.Sign(distance);
        if (!defSys.isDead && !busy)
        {
            if (Mathf.Abs(distance) > 4)
            {           
                bodyPhysic.velocity = new Vector2(xAxisDirection * xSpeed, bodyPhysic.velocity.y);
                walk = true;
            }
        }
        else
        {
            walk = false;
            bodyPhysic.velocity = new Vector2(0, bodyPhysic.velocity.y);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            audioToken.PlayOneShot(soundClip[0], 0.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            audioToken.PlayOneShot(soundClip[0], 0.5f);
        }
    }

    public void DestroySelf(float time)
    {
        Destroy(this.gameObject, time);
    }
}
