using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LaserMiner : MonoBehaviour
{
    public Transform targetToken;
    public MinerAI minerToken;
    Vector3 gunPos;
    public bool isMining = false;
    // Start is called before the first frame update
    void Start()
    {
        minerToken = GetComponentInParent<MinerAI>();
        gunPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMining)
        {
            targetToken.position = new Vector3(minerToken.transform.position.x + (2 * minerToken.xAxisDirection), 1, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-90 * minerToken.xAxisDirection, Vector3.forward), Time.deltaTime * 300);
        }
        else 
        {
            gunPos = transform.position;
            float angle = Mathf.Atan2(targetToken.position.x - gunPos.x, targetToken.position.y - gunPos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-angle, Vector3.forward), Time.deltaTime * 300);
            GetComponentInChildren<Laser>().LaserOn();
            GetComponentInChildren<Laser>().MinerLaserUpdate();
        }
    }
    public void StartMining(Transform target)
    {
        isMining = true;
        targetToken.position = target.position;
    }

    public void StopMining(float time)
    {
        Invoke("StopMining", time);
    }
    public void StopMining()
    {
        isMining = false;
        if(GetComponentInChildren<Laser>())
        GetComponentInChildren<Laser>().LaserOff();
    }

    public void ToggleMining(float time)
    {
        Invoke("ToggleMining", time);
    }
    public void ToggleMining()
    {
        isMining = !isMining;
    }
}
