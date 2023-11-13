using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PowerPole : MonoBehaviour
{
    [SerializeField] float currentTime;
    [SerializeField] float cooldownTime = 1f;
    public int healAmount = 1;
    [SerializeField] float radarRadius = 5f;
    public LayerMask layerDetect;
    public Collider2D[] allyDetect;
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
            currentTime = 0;
        }
    }

    void Heal()
    {      
        allyDetect = Physics2D.OverlapCircleAll(this.transform.position, radarRadius, layerDetect);
        if (allyDetect.Length > 0)
        {
            foreach (Collider2D ally in allyDetect)
            {
                if (ally.GetComponent<DefenseSystem>())
                {
                    ally.GetComponent<DefenseSystem>().Repair(healAmount, true); 
                }
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
