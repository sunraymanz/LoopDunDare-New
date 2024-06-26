﻿using System.Collections.Generic;
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
    public bool isExplosive;
    public bool isHoming;
    public bool isPenetrate;
    public bool isPointer;
    public float destroyTime;
    public int bulletSpeed;
    public int radius;
    public LayerMask layerDetect;
    //prefab
    public Transform sparkFX;
    public List<Rigidbody2D> objectsInRange;

    // Start is called before the first frame update
    void Start()
    {
        bodyPhysic = this.GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        if (!isPointer)
        {
            Destroy(this.gameObject, destroyTime);
        }
        float angle = Mathf.Atan2(bodyPhysic.velocity.y, bodyPhysic.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (GetComponent<Homing>())
        {
            isHoming = true;
        }       
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ScreenCollider"))
        {
            Destroy(gameObject);
            return;
        }
        //print("impact");
        //GetComponent<Collider2D>().enabled = false;
        if (isExplosive)
        {
            if (objectsInRange.Count == 0)
            {          
                bodyPhysic.simulated = false;
                collider.enabled = false;
                ExplosionResult();
            }
        }
        else if (isPointer)
        {
            //Call the Beam
            bodyPhysic.simulated = false;
            Destroy(this.gameObject, destroyTime);
        }
        else
        {
            hitPos = collision.GetContact(0).point;
            if (hp > 0) CollisionResult(0, collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ScreenCollider") && !isHoming)
        {
            Destroy(gameObject);
        }
        print("trigger enter");
        if (isPenetrate)
        {
            GetComponent<Collider2D>().enabled = false;
            hitPos = collision.ClosestPoint(transform.position);
            CollisionResult(1, collision.gameObject);
        }
    }

    void ExplosionResult()
    {
        print("Bomb Activated");
        Collider2D[] temp = Physics2D.OverlapCircleAll(transform.position, radius, layerDetect);
        foreach (Collider2D obj in temp)
        {
            if (!objectsInRange.Contains(obj.attachedRigidbody))
            {
                objectsInRange.Add(obj.attachedRigidbody);
            }
        }
        foreach (Rigidbody2D obj in objectsInRange)
        {
            if (obj.GetComponent<DefenseSystem>() != null)
            {
                print("Bomb explode hit : " + obj.name);
                obj.GetComponent<DefenseSystem>().DamageCalculate(damage, criChance, criDamage);
                obj.GetComponent<DefenseSystem>().GetHit();
            }
        }
        sparkFX.localScale = Vector3.one * radius;
        CreateSpark(1);
        Destroy(gameObject);
    }
    void CollisionResult(int type ,GameObject obj) 
    {
        hp -= 1;
        print("Bullet hit : "+obj.name);
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
            else if (obj.GetComponent<DefenseSystem>() != null)
            {    
                Vector3 dir = hitPos - transform.position;
                dir.Normalize();
                obj.GetComponent<Rigidbody2D>().AddForce(20 * dir, ForceMode2D.Force);
                obj.GetComponent<DefenseSystem>().DamageCalculate(damage, criChance, criDamage);
                obj.GetComponent<DefenseSystem>().GetHit();
            }
        }
        if (hp <= 0)
        { Destroy(gameObject); }
        //else { GetComponent<Collider2D>().enabled = true; }
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
    private void OnDestroy()
    {
        if (isPointer)
        {
            Instantiate(sparkFX, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }
    public int[] GetBulletStat() 
    {
        return new int[] { damage,criChance,criDamage};
    }
}
