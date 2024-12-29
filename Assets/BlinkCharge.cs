using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkCharge : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] List<GameObject> list;
    // Start is called before the first frame update
    void LateUpdate()
    {
        
    }

    // Update is called once per frame
    public void spawnCharge()
    {
        list.Add(Instantiate(prefab,this.gameObject.transform));
    }

    public void removeCharge()
    {
        GameObject temp = list[0];
        list.RemoveAt(0);
        Destroy(temp);
    }
}
