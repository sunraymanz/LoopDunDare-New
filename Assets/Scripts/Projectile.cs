using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D bodyPhysic;
    bool isHitTarget = false;
    public float selfDestroyTime;
    // Start is called before the first frame update
    void Start()
    {
        bodyPhysic = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isHitTarget)
        {
            float angle = Mathf.Atan2(bodyPhysic.velocity.y, bodyPhysic.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            if (collision.gameObject.GetComponent<DefenseSystem>() != null)
            { this.transform.SetParent(collision.transform); }
            isHitTarget = true;
            bodyPhysic.velocity = Vector2.zero;
            bodyPhysic.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            Destroy(this.gameObject, selfDestroyTime);
        }
    }
        
}
