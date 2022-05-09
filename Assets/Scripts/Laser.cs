using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Laser : MonoBehaviour
{
    public Camera cam;
    public LineRenderer line;
    public Transform originPos;
    public Transform guntipPos;
    public Transform originFX;
    public Transform endFX;
    public Transform targetToken;
    Vector2 mousePos;
    public LayerMask layerDetect;
    public ManaSystem manaSys;
    public int defaultRegen;
    public int miningRate;
    public bool isAI = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        line = GetComponent<LineRenderer>();
        if (!isAI)
        {
            manaSys = GetComponentInParent<ManaSystem>();
            defaultRegen = manaSys.recovRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAI)
        { CheckKeyPress(); }
    }

    void CheckKeyPress()
    {
        if (!manaSys.GetComponent<DefenseSystem>().isDead)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                LaserOn();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                LaserUpdate();
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                LaserOff();              
            }
        }
        
    }
    public void LaserOn()
    {
        line.enabled = true;
        originFX.gameObject.SetActive(true);
    }
    public void LaserUpdate()
    {
        //Vector3 temp = transform.position + (0.5f * transform.up);
        originFX.position = guntipPos.position;
        line.SetPosition(0, guntipPos.position);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 laserPath = mousePos - (Vector2)originPos.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(guntipPos.position, laserPath, 5f, layerDetect);
        Vector2 point = (Vector2)guntipPos.position+ laserPath.normalized*5f;
        line.SetPosition(1, point);
        manaSys.recovRate = 0;
        if (hit.Length > 0)
        {
            line.SetPosition(1, hit[0].point);
            endFX.gameObject.SetActive(true);
            endFX.position = hit[0].point;
            if (hit[0].rigidbody != null)
            {
                if (hit[0].transform.GetComponent<DefenseSystem>())
                {
                    hit[0].transform.GetComponent<DefenseSystem>().ChargeRepair(5,true);
                    //if (hit[0].transform.GetComponent<DeployBox>())
                    //{ hit[0].transform.GetComponent<DeployBox>().energy += 1; }
                }
                else
                {
                    if (hit[0].transform.GetComponent<Ore>())
                    {
                        hit[0].transform.GetComponent<Ore>().amount -= 1;
                        FindObjectOfType<GameManager>().oreAmount += 1;
                    }
                }
                manaSys.recovRate = 0;
            }
        }
        else
        {   endFX.gameObject.SetActive(false); }
    }
    public void MinerLaserUpdate()
    {
        originFX.position = guntipPos.position;
        line.SetPosition(0, guntipPos.position);
        if (targetToken == null)
        { return; }
        Vector2 laserPath = (Vector2)targetToken.position - (Vector2)originPos.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(guntipPos.position, laserPath, 5f, layerDetect);
        Vector2 point = (Vector2)guntipPos.position + laserPath.normalized * 5f;
        line.SetPosition(1, point);
        if (hit.Length > 0)
        {
            line.SetPosition(1, hit[0].point);
            endFX.gameObject.SetActive(true);
            endFX.position = hit[0].point;
            if (hit[0].rigidbody != null)
            {
                //hit[0].transform.GetComponent<DefenseSystem>().GetRepair(10);
                //if (hit[0].transform.GetComponent<DeployBox>())
                //{ hit[0].transform.GetComponent<DeployBox>().energy += 1; }
                if (hit[0].transform.GetComponent<Ore>())
                { 
                    hit[0].transform.GetComponent<Ore>().amount -= 1;
                    GetComponentInParent<MinerAI>().oreAmount += 1;
                }
            }
        }
        else
        { endFX.gameObject.SetActive(false); }
    }
    public void LaserOff()
    {
        originFX.gameObject.SetActive(false);
        endFX.gameObject.SetActive(false);
        line.enabled = false;
        if (!isAI)
        {
            manaSys.recovRate = defaultRegen;            
        }              
    }

    
}
