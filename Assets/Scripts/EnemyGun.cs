using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform shotPoint;
    public Animator anim;
    AudioSource audioPlayer;
    DefenseSystem defSys;
    [SerializeField] GameObject[] bulletPrefab;
    [SerializeField] AudioClip[] soundClip;
    GameManager token;
    public LayerMask layerDetect;


    public float shootCooldown;
    float countTime = 0;
    public int level = 1;
    public int damage;
    public int criChance;
    public int criDamage;
    public int dmgUp = 0;
    public int criUp = 0;
    public int criDamageUp = 0;
    public int radarRadius = 0;
    Vector2 gunPos;
    [SerializeField] Vector2 targetPos;
    [SerializeField] Collider2D playerDetect;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        audioPlayer = this.GetComponent<AudioSource>();
        token = FindObjectOfType<GameManager>();
        defSys = this.GetComponentInParent<DefenseSystem>();
        RefreshValue();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gunPos = this.transform.position;
        if (!defSys.isDead)
        {
            playerDetect = Physics2D.OverlapCircle(this.transform.position, radarRadius, layerDetect);
            if (playerDetect != null)
            {
                targetPos = playerDetect.transform.position;
            }
            else 
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * 500);
                GetComponentInParent<Enemy>().busy = false;
                return;
                /*playerDetect = Physics2D.OverlapCircle(this.transform.position, radarRadius, layerDetect);
                if (playerDetect != null)
                { targetPos = playerDetect.transform.position; }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * 300);
                    GetComponentInParent<Enemy>().busy = false;
                    return;               
                }*/
            }
            GetComponentInParent<Enemy>().busy = true;
            countTime += Time.deltaTime;
            float angle = Mathf.Atan2(targetPos.x - gunPos.x, targetPos.y - gunPos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-angle, Vector3.forward), Time.deltaTime * 500);
            if (playerDetect.GetComponentInParent<Rigidbody2D>().simulated)
            {
                if (countTime > shootCooldown)
                {
                    Shoot();
                }
            }
            else { playerDetect = null; }
            
        }
        else
        { anim.SetBool("Dead", defSys.isDead); }
    }

    void Shoot()
    {
        damage = 2+ dmgUp*2;
        criChance = 30+criUp;
        criDamage = 50+criDamageUp;
        countTime = 0;
        ShootBullet(bulletPrefab[0], bulletPrefab[0].GetComponent<Bullet>().bulletSpeed);
        audioPlayer.PlayOneShot(soundClip[0], 0.2f);
    }

    void ShootBullet(GameObject prefabs,int shotSpeed)
    {
        GameObject newBullet = Instantiate(prefabs, shotPoint.position,shotPoint.rotation);
        newBullet.GetComponent<Bullet>().damage = this.damage;
        newBullet.GetComponent<Bullet>().criChance = this.criChance;
        newBullet.GetComponent<Bullet>().criDamage = this.criDamage;
        newBullet.GetComponent<Rigidbody2D>().velocity = transform.up * shotSpeed;
    }
    public void GotUpgrade()
    {
        token.lvBoss += 1;
        int addPoint = Random.Range(0, 4);
        if (addPoint == 0)
        { token.dmgUpBoss += 1; }
        else if (addPoint == 1)
        { token.criUpBoss += 1; }
        else if (addPoint == 2)
        { token.criDamageUpBoss += 1; }
        else 
        { 
            token.speedUpBoss += 1;
            shootCooldown = shootCooldown * (1 - (0.05f * token.speedUpBoss));
            if (shootCooldown < 0.4f)
            { shootCooldown = 0.4f; }
        }
    }

    private void OnDrawGizmos()
    {
        if (radarRadius == 0)
        { return; }
        Gizmos.DrawWireSphere(this.transform.position, radarRadius);
    }
    public void AttackCalculate(int type)
    {
        if (type == 0)
        {
            damage = 2 + dmgUp * 2;
            criChance = 30 + criUp;
            criDamage = 50 + criDamageUp;
        }
        else
        {
            damage = 6 + (dmgUp * 2);
            criChance = 50 + criUp;
            criDamage = 50 + (criDamageUp * 2);
        }
    }
    public void RefreshValue()
    {
        level = token.lvBoss;
        dmgUp = token.dmgUpBoss;
        criUp = token.criUpBoss;
        criDamageUp = token.criDamageUpBoss;
        AttackCalculate(0);
        shootCooldown = 1f * (Mathf.Pow(0.95f, token.speedUpBoss));
        if (shootCooldown < 0.4f)
        { shootCooldown = 0.4f; }
        FindObjectOfType<InfoMenu>().RefreshStat();
    }


}
