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
    public MinerControl controlToken;
    //Miner Stat
    Vector2 selfPos;
    public float xSpeed = 20f;
    public float jumpForce = 50f;
    public float xAxisDirection = 1f;
    float speedLimit = 6f;
    float radarRadius = 7f;
    public int oreAmount = 0;
    public bool findingOre = true;
    public bool ready = true;
    public bool isWorking = true;
    public bool isMining;
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
        controlToken = FindObjectOfType<MinerControl>();
        xAxisDirection = 1f;
    }

    // Update is called once per frame
    
    void Update()
    {
        isWorking = controlToken.isWorking;
        isMining = GetComponentInChildren<LaserMiner>().isMining;
        if (FindObjectOfType<Ore>() && FindObjectOfType<MinerBase>() && !isRetreat && isWorking)
        {
            if (ready)
            {
                if (oreAmount >= 99)
                { 
                    findingOre = false;
                }
                else
                { 
                    findingOre = true;
                }
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
                if (!isMining)
                {
                    GoBack();
                }
                bodyPhysic.velocity = new Vector2(0, bodyPhysic.velocity.y);
            }
        }
        else 
        {
            if (FindObjectOfType<MinerBase>())
            { minerBase = FindObjectOfType<MinerBase>().transform; }
            else
            { minerBase = FindObjectOfType<HQBase>().transform; }
            GoBack();
        }
    }

    void Follow(Transform target)
    {
        if (target != null)
        {
            float distance = target.position.x - this.transform.position.x;
            xAxisDirection = Mathf.Sign(distance);
            if (Mathf.Abs(distance) > 6 && Mathf.Abs(bodyPhysic.velocity.x) < speedLimit)//Object is Far away
            {
                bodyPhysic.AddForce(new Vector2(xAxisDirection * xSpeed, 0), 0);
                walk = true;
            }
            else //Object is Near
            {
                if (Mathf.Abs(distance) > 1.5f && Mathf.Abs(bodyPhysic.velocity.x) <= 3f) //Object is Near and already slow down
                {
                    bodyPhysic.AddForce(new Vector2(xAxisDirection * (xSpeed), 0), 0);
                    walk = true;
                }
                else if (Mathf.Abs(distance) <= 1.5f)//Object is close
                {
                    bodyPhysic.velocity = Vector2.zero;
                    bodyPhysic.angularVelocity = bodyPhysic.angularVelocity * 0.9f;
                    walk = false;
                    if (findingOre && ready)//Start Mining
                    {
                        MiningState();
                        //Invoke("MiningState", 0.4f);
                    }
                }
                else //Slow Down
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
        GameObject[] temp = GameObject.FindGameObjectsWithTag(tag);
        temp.OrderBy(o => (o.transform.position.x - position.x));
        if (temp.First())
        {
            return temp.First();
        }
        else
        {
            return null;              
        }
    }

    public GameObject FindClosest(string tag)
    {
        GameObject[] targetList;
        targetList = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject target in targetList)
        {
            Vector2 diff = target.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = target;
                distance = curDistance;
            }
        }
        return closest;
    }
    void MiningState()
    {
        if (targetToken == null)
        {
            GetComponentInChildren<LaserMiner>().StopMining();
            return;
        }
        GetComponentInChildren<LaserMiner>().StartMining(targetToken);
        GetComponentInChildren<Laser>().targetToken = targetToken;
        ready = false;
        //GetComponentInChildren<LaserMiner>().StopMining(2f);
    }

    void GoBack()
    {
        GetComponentInChildren<LaserMiner>().StopMining();
        findingOre = false;
        targetToken = null;
        Follow(minerBase);
    }
    public void ToggleWorking()
    {
        isWorking = !isWorking;
    }
    public void SetRetreat(bool input)
    {
        isRetreat = input;
    }

    private void OnDrawGizmos2D()
    {
        if (radarRadius == 0)
        { return; }
        Gizmos.DrawWireSphere(this.transform.position, radarRadius);
    }


}
