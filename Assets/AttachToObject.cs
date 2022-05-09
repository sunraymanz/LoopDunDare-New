using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToObject : MonoBehaviour
{
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = FindObjectOfType<GameManager>().transform;
        }
        else
        {
            if (FindObjectOfType<Player>())
            {
                target = FindObjectOfType<Player>().transform;
            }
            transform.position = target.position;
        }
        
    }
}
