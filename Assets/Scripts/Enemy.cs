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
    float xAxisDirection = 0f;
    public bool jump = false;
    public bool walk = false;
    public bool busy = false;
    Vector2 selfPos;

    // Start is called before the first frame update
    void Start()
    {
        audioToken = this.GetComponent<AudioSource>();
        selfPos = this.transform.position;
        defSys = this.GetComponent<DefenseSystem>();
        gunToken = this.GetComponentInChildren<EnemyGun>();
        canvasToken = this.GetComponentInChildren<CanvasScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToken == null)
        {
            if (FindObjectOfType<Player>() != null)
            {
                playerToken = FindObjectOfType<Player>().transform;
            }
        }
        if (!defSys.isDead && !busy)
        {
            float distance = 0-selfPos.x;
            if (playerToken != null)
            {
                distance = playerToken.position.x - selfPos.x;
            }
            if (Mathf.Abs(distance) > 4)
            {
                xAxisDirection = Mathf.Sign(distance);
                bodyPhysic.velocity = new Vector2(xAxisDirection * xSpeed, bodyPhysic.velocity.y);
                walk = true;
            }
            else 
            { walk = false; }
        }
        else { walk = false; }


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
