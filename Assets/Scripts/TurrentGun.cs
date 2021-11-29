using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurrentGun : MonoBehaviour
{
    public Transform shotPoint;
    public Transform shotPoint2;
    public Transform secondaryGun;
    public Animator anim;
    AudioSource audioPlayer;
    [SerializeField] GameObject[] bulletPrefab;
    [SerializeField] AudioClip[] soundClip;
    GameManager token;
    ManaSystem manaSys;
    DefenseSystem defSys;
    public float shootCooldown;
    float countTime1st = 0;
    float countTime2nd = 0;
    public int lastUseType = 0;
    public int damage;
    public int criChance;
    public int criDamage;
    public int dmgUp = 0;
    public int criUp = 0;
    public int criDamageUp = 0;
    public bool isAuto = false;
    public float radarRadius = 5f;
    public Collider2D targetDetect;
    Vector2 gunPos;
    Vector2 secondgunPos;
    Vector2 mousePos;
    Vector2 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        audioPlayer = this.GetComponent<AudioSource>();
        token = FindObjectOfType<GameManager>();
        defSys = this.GetComponentInParent<DefenseSystem>();
        RefreshValue();
        if (!isAuto)
        {
            manaSys = this.GetComponentInParent<ManaSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!defSys.isDead)
        { 
            gunPos = this.transform.position;
            countTime1st += Time.deltaTime;
            if (!isAuto)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.up = mousePos - gunPos;
                secondgunPos = secondaryGun.position;
                secondaryGun.right = mousePos - secondgunPos;
                countTime2nd += Time.deltaTime;
            }
            else
            {
                if (targetDetect != null)
                { 
                    targetPos = targetDetect.transform.position; 
                    float angle = Mathf.Atan2(targetPos.x - gunPos.x, targetPos.y - gunPos.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-angle, Vector3.forward), Time.deltaTime * 500);
                    if (targetDetect.GetComponentInParent<Rigidbody2D>().simulated)
                    {
                        if (countTime1st > shootCooldown)
                        {
                            AutoAttack(0);
                        }
                    }
                    else{ targetDetect = null; }    
                }
                else
                {
                    targetDetect = Physics2D.OverlapCircle(this.transform.position, radarRadius, LayerMask.GetMask("Enemy"));
                    if (targetDetect != null) 
                    {
                        if (targetDetect.tag != "Bullet")
                        { transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), Time.deltaTime * 500); }
                        else
                        { targetDetect = null; }
                    }    
                }
            }
        }
        else 
        { anim.SetBool("Dead", defSys.isDead); }
    }

    public void Attack(int type)
    {
        if (lastUseType != type)
        { AttackCalculate(type); }
        lastUseType = type;
        if (type == 0 && countTime1st > shootCooldown && manaSys.mp >= manaSys.skill1Use)
        {
            //RefreshValue();
            countTime1st = 0;
            ShootBulletUpAxis(bulletPrefab[0], shotPoint, bulletPrefab[0].GetComponent<Bullet>().bulletSpeed);
            audioPlayer.PlayOneShot(soundClip[0], 0.4f);
            manaSys.mp -= manaSys.skill1Use;
            FindObjectOfType<InfoMenu>().RefreshStat();
        }
        if (type == 1 && countTime2nd > shootCooldown*3f && manaSys.mp >= manaSys.skill2Use)
        {
            //RefreshValue();
            countTime2nd = 0;
            ShootBulletRightAxis(bulletPrefab[1], shotPoint2, bulletPrefab[1].GetComponent<Bullet>().bulletSpeed);
            audioPlayer.PlayOneShot(soundClip[1], 0.4f);
            manaSys.mp -= manaSys.skill2Use;
            FindObjectOfType<InfoMenu>().RefreshStat();
        }      
    }

    public void AttackCalculate(int type)
    {
        if (type == 0)
        {
            damage = 2 + dmgUp;
            criChance = 20 + criUp;
            criDamage = 50 + criDamageUp;
        }
        else
        {
            damage = 6 + (dmgUp * 2);
            criChance = 50 + criUp;
            criDamage = 50 + (criDamageUp * 2);
        }
    }

    public void AutoAttack(int type)
    {
        
        if (type == 0 && countTime1st > shootCooldown )
        {
            RefreshValue();
            damage = 2 + dmgUp;
            criChance = 20 + criUp;
            criDamage = 50 + criDamageUp;
            countTime1st = 0;
            ShootBulletUpAxis(bulletPrefab[0], shotPoint, bulletPrefab[0].GetComponent<Bullet>().bulletSpeed);
            audioPlayer.PlayOneShot(soundClip[0], 0.4f);
            //GetComponentInParent<DeployBox>().energy -= 2;
        }
        if (type == 1 && countTime2nd > shootCooldown * 3f)
        {
            RefreshValue();
            damage = 6 + dmgUp * 2;
            criChance = 50 + criUp;
            criDamage = 50 + criDamageUp * 2;
            countTime2nd = 0;
            ShootBulletRightAxis(bulletPrefab[1], shotPoint2, bulletPrefab[1].GetComponent<Bullet>().bulletSpeed);
            audioPlayer.PlayOneShot(soundClip[1], 0.4f);
            //GetComponentInParent<DeployBox>().energy -= 5;
        }
    }

    void ShootBulletRightAxis(GameObject prefabs,Transform shotPos,int shotSpeed)
    {
        GameObject newBullet = Instantiate(prefabs, shotPos.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().damage = this.damage;
        newBullet.GetComponent<Bullet>().criChance = this.criChance;
        newBullet.GetComponent<Bullet>().criDamage = this.criDamage;
        newBullet.GetComponent<Rigidbody2D>().velocity = shotPos.right * shotSpeed;
    }

    void ShootBulletUpAxis(GameObject prefabs, Transform shotPos, int shotSpeed)
    {
        GameObject newBullet = Instantiate(prefabs, shotPos.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().damage = this.damage;
        newBullet.GetComponent<Bullet>().criChance = this.criChance;
        newBullet.GetComponent<Bullet>().criDamage = this.criDamage;
        newBullet.GetComponent<Rigidbody2D>().velocity = shotPos.up * shotSpeed;
    }
    public void GotUpgrade()
    {
        int addPoint = Random.Range(0,4);
        if (addPoint == 0)
        { token.dmgUp += 1; }
        else if (addPoint == 1)
        { token.criUp += 1; }
        else if (addPoint == 2)
        { token.criDamageUp += 1; }
        else  
        {
            token.speedUp += 1;
            shootCooldown = shootCooldown * (1 - (0.05f * token.speedUp));
            if (shootCooldown < 0.06f)
            { shootCooldown = 0.06f; }
        }
        SaveValue();
    }

    public void SaveValue()
    {
       
    }

    public void RefreshValue()
    {
        dmgUp = token.dmgUp;
        criUp = token.criUp;
        criDamageUp = token.criDamageUp;
        AttackCalculate(lastUseType);
        shootCooldown = 1.5f*(Mathf.Pow(0.97f, token.speedUp));
    }
}
