using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingSystem : MonoBehaviour
{
    Vector2 gunPos;
    Vector2 mousePos;
    Vector2 targetPos;
    public float detectRange;
    public LayerMask layerDetect;
    [SerializeField] Collider2D targetDetect;
    [SerializeField] PlayerGun gunToken;
    [SerializeField] bool isAuto;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<Player>())
        {
            isAuto = false;
        }
        else
        {
            gunToken = GetComponent<PlayerGun>();
            isAuto = true; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        gunPos = transform.position;
        if (!isAuto && Time.timeScale == 1)
        {
            MouseAiming();
        }
        else
        {
            AutoAiming();
        }
    }
    public void MouseAiming()
    {      
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePos - gunPos;
    }
    public void AutoShoot()
    {
        
    }
    public void AutoAiming()
    {
        //targetDetect = Physics2D.OverlapCircle(this.transform.position, detectRange, layerDetect);
        targetDetect = Physics2D.OverlapBox(transform.position, new Vector2(detectRange, 20f), 0f,layerDetect); 
        if (targetDetect != null)
        {
            if (GetComponentInParent<Enemy>())
            {
                GetComponentInParent<Enemy>().busy = true;
            }
            targetPos = targetDetect.transform.position;
            float angle = Mathf.Atan2(targetPos.x - gunPos.x, targetPos.y - gunPos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-angle, Vector3.forward), Time.deltaTime * 1000);
            if (Physics2D.Raycast(gunPos,transform.up, detectRange, layerDetect))
            {         
                gunToken.AutoAttack(transform);
            }
            else { targetDetect = null; }
        }
        else
        {
            if (GetComponentInParent<Enemy>())
            {
                GetComponentInParent<Enemy>().busy = false;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * 500);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(detectRange, 20f));
    }
}
