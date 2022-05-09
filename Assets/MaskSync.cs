using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskSync : MonoBehaviour
{
    public SpriteRenderer[] targetList;
    public SpriteRenderer target;
    public SpriteMask mask;
    // Start is called before the first frame update
    void Start()
    {
        targetList = GetComponentsInParent<SpriteRenderer>();
        target = targetList[1];
        mask = GetComponent<SpriteMask>();
        mask.sprite = target.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
