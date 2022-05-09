using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerDetector : MonoBehaviour
{
    public LayerMask layerDetect;
    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(1,2) * 8, 0f, layerDetect))
        {
            GetComponentInParent<MinerAI>().isRetreat = true;
        } 
    }

}
