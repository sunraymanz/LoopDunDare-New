using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBeam : MonoBehaviour
{
    public Vector3 originPos;
    public Vector3 impactPos;
    public LineRenderer lineFX;
    public Transform endFX;
    public LayerMask layerDetect;
    public List<Rigidbody2D> objectsInRange;
    public StatCalculator statToken;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke(nameof(WhiteBeamOn),2f);
        lineFX = GetComponent<LineRenderer>();
        WhiteBeamOn();
        Destroy(gameObject,3f);
        statToken = FindObjectOfType<StatCalculator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WhiteBeamOn()
    {
        originPos = new Vector3(transform.position.x, 20, 0);
        impactPos = new Vector3(originPos.x, -5.5f, 0);
        lineFX.SetPosition(0, originPos);
        lineFX.SetPosition(1, impactPos);
        endFX.gameObject.SetActive(true);
        endFX.position = new Vector3(originPos.x, -5f, 0);
    }
    public void ApplyDamage()
    {      
        Vector2 laserPath = impactPos - originPos;
        RaycastHit2D[] hitList = Physics2D.CircleCastAll(originPos,2f, laserPath, 100f, layerDetect);
        foreach (RaycastHit2D obj in hitList)
        {
            if (!objectsInRange.Contains(obj.collider.attachedRigidbody))
            {
                objectsInRange.Add(obj.collider.attachedRigidbody);
            }
        }
        foreach (Rigidbody2D obj in objectsInRange)
        {
            if (obj.GetComponentInParent<DefenseSystem>() != null)
            {
                print("Beam hit : " + obj.name);
                obj.GetComponentInParent<DefenseSystem>().DamageCalculate(statToken.GetPlayerStat(0), statToken.GetPlayerStat(3), statToken.GetPlayerStat(4));
                obj.GetComponentInParent<DefenseSystem>().GetHit();
            }
        }
    }
}
