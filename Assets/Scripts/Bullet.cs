using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bodyPhysic;
    public float destroyTime;
    public int bulletSpeed;
    public int damage;
    public int criChance;
    public int criDamage;
    public bool canPenetrate;
    public bool isProjectile;
    public Transform sparkFX;
    Vector3 hitPos;
    // Start is called before the first frame update
    void Start()
    {
        bodyPhysic = this.GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, destroyTime);
        float angle = Mathf.Atan2(bodyPhysic.velocity.y, bodyPhysic.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitPos = collision.GetContact(0).point;
        CollisionResult(0,collision.gameObject);
        /*if (collision.gameObject.tag == "Collider")
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            CreateSpark(0);
        }
        if (collision.gameObject.GetComponent<DefenseSystem>() != null && this.gameObject.layer != collision.gameObject.layer)
        {
            collision.gameObject.GetComponent<DefenseSystem>().DamageCalculate(damage, criChance, criDamage);
        }
        if (!canPenetrate)
        { Destroy(this.gameObject); }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hitPos = new Vector3(this.GetComponent<BoxCollider2D>().size.x, 0, 0);
        CollisionResult(1,collision.gameObject);
        /*if (collision.gameObject.tag == "Collider")
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            CreateSpark(1);
            if (collision.gameObject.tag == "Ground")
            { Destroy(this.gameObject); }
        }
        if (collision.gameObject.GetComponent<DefenseSystem>() != null && this.gameObject.layer != collision.gameObject.layer)
        {
            collision.gameObject.GetComponent<DefenseSystem>().DamageCalculate(damage, criChance, criDamage);
        }
        if (!canPenetrate)
        { Destroy(this.gameObject); }*/
    }

    void CollisionResult(int i ,GameObject obj) 
    {
        if (obj.tag == "Collider")
        {
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            { Destroy(this.gameObject); }
            return;
        }
        else
        {
            CreateSpark(i);
            if (obj.tag == "Ground")
            { Destroy(this.gameObject); }
        }
        if (obj.GetComponent<DefenseSystem>() != null && this.gameObject.layer != obj.layer)
        {
            obj.GetComponent<DefenseSystem>().DamageCalculate(damage, criChance, criDamage);
        }
        if (!canPenetrate)
        { Destroy(this.gameObject); }
    }

    void CreateSpark(int type)
    {
        if (type == 0)
        {
            Instantiate(sparkFX, hitPos, Quaternion.LookRotation(transform.right, transform.up));
        }
        if (type == 1)
        {
            Instantiate(sparkFX, transform.TransformPoint(hitPos), Quaternion.LookRotation(transform.right, transform.up));
        }
        
    }
}
