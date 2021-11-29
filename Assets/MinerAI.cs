using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class MinerAI : MonoBehaviour
{
    //Ref
    public Rigidbody2D bodyPhysic;
    AudioSource audioPlayer;
    [SerializeField] AudioClip[] soundClip;
    public DefenseSystem defSys;
    public Transform playerToken;
    public EnemyGun gunToken;
    public CanvasScript canvasToken;
    public Transform minerBase;
    public Transform targetToken;
    public Transform detector;
    //Miner Stat
    Vector2 selfPos;
    public float xSpeed = 20f;
    public float jumpForce = 50f;
    public float xAxisDirection = 1f;
    float speedLimit = 6f;
    float radarRadius = 7f;
    public int oreAmount = 0;
    public bool findingOre = true;
    public bool ready = false;
    public bool atBase = true;
    public bool isMining;
    public bool isWorking;
    public bool isRetreat = false;
    public bool jump = false;
    public bool walk = false;
    [SerializeField]
    Collider2D[] enemyDetect;
    public LayerMask layerDetect;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = this.GetComponent<AudioSource>();
        if (FindObjectOfType<Player>() != null)
        { playerToken = FindObjectOfType<Player>().transform; }
        selfPos = this.transform.position;
        defSys = this.GetComponent<DefenseSystem>();
        bodyPhysic = GetComponent<Rigidbody2D>();
        minerBase = FindObjectOfType<MinerBase>().transform;
        xAxisDirection = 1f;
    }

    // Update is called once per frame
    
    void Update()
    {
        isMining = GetComponentInChildren<LaserMiner>().isMining;
        if (FindObjectOfType<Ore>() && FindObjectOfType<MinerBase>() && !isRetreat)
        {
            if (ready)
            {
                if (findingOre)
                {
                    //targetToken = FindClosestTarget("Ore").transform;
                    targetToken = FindClosest("Ore").transform;
                    Follow(targetToken);
                }
                else
                {
                    //minerBase = FindClosestTarget("MinerBase").transform;
                    minerBase = FindClosest("MinerBase").transform;
                    GoBack();
                }
            }
            else
            {
                if (targetToken == null)
                {
                    GoBack();
                }
                if (!isMining && findingOre)
                {
                    GoBack();
                }
                bodyPhysic.velocity = new Vector2(0, bodyPhysic.velocity.y);
            }
        }
        else 
        {
            if (minerBase == null) 
            {
                if (FindObjectOfType<MinerBase>())
                { minerBase = FindObjectOfType<MinerBase>().transform; }
                else
                { minerBase = FindObjectOfType<HQBase>().transform; }
            }
            GoBack();
        }
    }

    void Follow(Transform target)
    {
        if (target != null)
        {
            float distance = target.position.x - this.transform.position.x;
            xAxisDirection = Mathf.Sign(distance);
            if (Mathf.Abs(distance) > 8)
            {
                bodyPhysic.AddForce(new Vector2(xAxisDirection * xSpeed, 0), 0);
                if (Mathf.Abs(bodyPhysic.velocity.x) > speedLimit)
                { 
                    bodyPhysic.velocity = new Vector2(xAxisDirection * speedLimit, 0); 
                }
                walk = true;
            }
            else
            {
                if (Mathf.Abs(distance) > 1.5f && Mathf.Abs(bodyPhysic.velocity.x) <= 3f)
                {
                    bodyPhysic.AddForce(new Vector2(xAxisDirection * (xSpeed), 0), 0);
                    walk = true;
                }
                else if (Mathf.Abs(distance) <= 1.5f)
                {
                    bodyPhysic.velocity = Vector2.zero;
                    bodyPhysic.angularVelocity = bodyPhysic.angularVelocity * 0.9f;
                    walk = false;
                    if (findingOre && ready && atBase)
                    {
                        //MiningState();
                        Invoke("MiningState", 0.6f);
                        atBase = false;
                    }
                    else if (!findingOre && ready && !atBase)
                    {
                        Invoke("ProvidingState", 3f);
                        ready = false;
                        atBase = true;
                        isRetreat = false;
                        if (FindObjectOfType<Ore>())
                        { targetToken = FindClosestTarget("Ore").transform; }
                    }
                }
                else 
                { 
                    bodyPhysic.velocity = bodyPhysic.velocity * 0.95f;
                }
            }
        }
        else { walk = false; }
    }
    GameObject FindClosestTarget(string tag)
    {
        Vector3 position = FindObjectOfType<HQBase>().transform.position;
        return GameObject.FindGameObjectsWithTag(tag)
        .OrderBy(o => (o.transform.position - position).sqrMagnitude)
        .First();
    }

    public GameObject FindClosest(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    void MiningState()
    {
        if (targetToken == null)
        {
            targetToken = FindClosestTarget("Ore").transform;
        }
        GetComponentInChildren<LaserMiner>().StartMining(targetToken);
        GetComponentInChildren<Laser>().targetToken = targetToken;
        ready = false;
        GetComponentInChildren<LaserMiner>().StopMining(2f);
    }
    void ProvidingState()
    {
        if (FindObjectOfType<Ore>())
        { findingOre = true; }
        ready = true;
    }

    void GoBack()
    {
        GetComponentInChildren<LaserMiner>().StopMining();
        findingOre = false;
        ready = true;
        atBase = false;
        targetToken = null;
        Follow(minerBase);
    }

    public void Retreat()
    {
        isRetreat = true;
    }
    private void OnDrawGizmos()
    {
        if (radarRadius == 0)
        { return; }
        Gizmos.DrawWireSphere(this.transform.position, radarRadius);
    }

}
