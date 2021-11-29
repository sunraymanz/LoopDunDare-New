using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PowerPole : MonoBehaviour
{
    float currentTime;
    float cooldownTime = 1f;
    public float radarRadius = 5f;
    public LayerMask layerDetect;
    public List<Collider2D> allyDetect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > cooldownTime)
        { 
            Heal();
        }
    }

    void Heal()
    {      
        allyDetect = Physics2D.OverlapCircleAll(this.transform.position, radarRadius, layerDetect).ToList();
        if (allyDetect.Count > 0)
        {
            foreach (Collider2D ally in allyDetect)
            {
                if (ally.GetComponent<DefenseSystem>())
                { ally.GetComponent<DefenseSystem>().isRepair = 1f; }
                else if (ally.GetComponentInParent<DefenseSystem>())
                { ally.GetComponentInParent<DefenseSystem>().isRepair = 1f; }

            }
        }
        
    }


private void OnDrawGizmos()
    {
        if (radarRadius == 0)
        { return; }
        Gizmos.DrawWireSphere(this.transform.position, radarRadius);
    }
}
