using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    [SerializeField] LayerMask layerDetect;
    float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] int detectRange;
    [SerializeField] GameObject target;
    [SerializeField] Vector3 targetPos;
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().radius = detectRange;
        speed = GetComponent<Bullet>().bulletSpeed;
    }

    void FixedUpdate()
    {
        rb.angularVelocity = 0;
        if (target != null)
        {
            Vector2 direction = (Vector2)target.transform.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = -rotateAmount * turnSpeed;
        }
        rb.velocity = transform.right * speed;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("Homing Detected");
        if (1 << collision.gameObject.layer == layerDetect.value && target == null && !collision.gameObject.CompareTag("Bullet"))
        {
            target = collision.gameObject;
        }  
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        target = null;
    }
}
