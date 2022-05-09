using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bodyPhysic;
    public Collider2D collider;
    Vector3 hitPos;

    //stat
    public int damage;
    public int criChance;
    public int criDamage;
    public int hp;
    public bool isProjectile;
    public bool isPenetrate;
    public float destroyTime;
    public int bulletSpeed;
    //prefab
    public Transform sparkFX;

    // Start is called before the first frame update
    void Start()
    {
        bodyPhysic = this.GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        Destroy(this.gameObject, destroyTime);
        float angle = Mathf.Atan2(bodyPhysic.velocity.y, bodyPhysic.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        isPenetrate = collider.isTrigger;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("impact");
        GetComponent<Collider2D>().enabled = false;
        hitPos = collision.GetContact(0).point;
        if (hp>0)
        {
            CollisionResult(0, collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("through");
        GetComponent<Collider2D>().enabled = false;
        //if (isPenetrate)
        {
            hitPos = collision.ClosestPoint(transform.position);
            CollisionResult(1, collision.gameObject);
        }
    }

    void CollisionResult(int type ,GameObject obj) 
    {
        //print("Bullet hit : "+obj.name);
        hp -= 1;
        if (obj.CompareTag("Collider"))
        {
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            { Destroy(gameObject); }
            return;
        }
        else
        {
            CreateSpark(type);
            if (obj.CompareTag("Ground"))
            { Destroy(gameObject); }
            else if (obj.GetComponent<DefenseSystem>() != null )
            {          
                obj.GetComponent<DefenseSystem>().DamageCalculate(damage, criChance, criDamage);
                obj.GetComponent<DefenseSystem>().GetHit();
            }
        }
        if (hp <= 0)
        { Destroy(gameObject); }
        else { GetComponent<Collider2D>().enabled = true; }
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

    public int[] GetBulletStat() 
    {
        return new int[] { damage,criChance,criDamage};
    }
}
