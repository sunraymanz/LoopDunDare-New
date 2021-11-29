using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class LockZRotation : MonoBehaviour
{
    public Vector3 initRotation;
    public Vector3 oppositeRotation;
    public MinerAI minerToken;
    public float xDirection;
    public float preDirection;
    public bool enableFaceTurn = false;
    float tempRotation;
    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.rotation.eulerAngles;
        minerToken = GetComponentInParent<MinerAI>();
        if (enableFaceTurn)
        {
            oppositeRotation = new Vector3(initRotation.x, initRotation.y, 360 - initRotation.z);     
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (preDirection == 0)
        { preDirection = xDirection; }
        xDirection = minerToken.xAxisDirection;
        if (enableFaceTurn)
        {
            FaceTurn();
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(initRotation.z, Vector3.forward);
        }
    }

    void FaceTurn()
    {
        if (xDirection != preDirection)
        {
            if (xDirection < 0)
            {
                tempRotation = initRotation.z;
            }
            else if (xDirection > 0)
            {
                tempRotation = oppositeRotation.z;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(tempRotation, Vector3.forward), Time.deltaTime * 330);
            Invoke("SyncDirection",0.25f);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(tempRotation, Vector3.forward);
        }
    }

    void SyncDirection()
    {
        preDirection = xDirection; ;
    }
}
