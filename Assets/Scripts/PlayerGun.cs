using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Vector3 shotPoint;
    public Transform secondaryGun;
    public SpriteRenderer sprToken;
    public Animator anim;
    public WeaponItem weaponToken;
    AudioSource audioPlayer;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioClip soundClip;
    StatCalculator statToken;
    [SerializeField] DefenseSystem defToken;
    ManaSystem manaSys;

    float countTime1st = 0;
    public int lastUseType = 0;
    public int damage;
    public int criChance;
    public int criDamage;
    public float cooldown;
    public bool isAuto = false;
    public Collider2D targetDetect;

    // Start is called before the first frame update
    void Start()
    {
        sprToken = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        statToken = FindObjectOfType<StatCalculator>();
        defToken = GetComponentInParent<DefenseSystem>();
        GetWeaponInfo(isAuto);
        if (!isAuto)
        {
            manaSys = GetComponentInParent<ManaSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!defToken.isDead)
        {
            countTime1st += Time.deltaTime;
        }
        else 
        { 
            anim.SetBool("Dead", defToken.isDead);
            GetComponent<AimingSystem>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            print("KKK");
            sprToken.sprite = weaponToken.spr;
        }
    }

    public void AttackCalculate(bool isAuto)
    {
        if (!isAuto)
        {
            damage = statToken.GetPlayerStat(0);
            criChance = statToken.GetPlayerStat(3);
            criDamage = statToken.GetPlayerStat(4);
            cooldown = statToken.GetPlayerStat();
        }
        else
        {
            if (CompareTag("Enemy"))
            {
                damage = statToken.AtkCal(weaponToken.atk, false);
                criChance = statToken.CriCal(weaponToken.cri, false);
                criDamage = statToken.CriDmgCal(weaponToken.criDmg, false);
                cooldown = statToken.FireRateCal(weaponToken.firerate, false);
            }
            else if (CompareTag("Boss"))
            {
                damage = Mathf.CeilToInt(1.5f * statToken.AtkCal(weaponToken.atk, false));
                criChance = Mathf.CeilToInt(statToken.CriCal(weaponToken.cri, false));
                criDamage = Mathf.CeilToInt(1.5f * statToken.CriDmgCal(weaponToken.criDmg, false));
                cooldown = Mathf.CeilToInt(1.5f * statToken.FireRateCal(weaponToken.firerate, false));
            }
            else
            {
                damage = weaponToken.atk;
                criChance = weaponToken.cri;
                criDamage = weaponToken.criDmg;
                cooldown = weaponToken.firerate;
            }
        }
    }

    public void GetWeaponInfo(bool isAuto)
    {
        if (!isAuto)
        {
            weaponToken = statToken.currentWeapon;
        }
        AttackCalculate(isAuto);
        bulletPrefab = weaponToken.bulletPrefab;
        soundClip = weaponToken.soundClip;
        sprToken.sprite = weaponToken.spr;
    }

    public void Attack()
    {
        if (countTime1st > cooldown && manaSys.mp >= manaSys.skill1Use)
        {
            countTime1st = 0;
            ShootBullet(bulletPrefab, transform, bulletPrefab.GetComponent<Bullet>().bulletSpeed);
            audioPlayer.PlayOneShot(soundClip, 0.4f);
            manaSys.mp -= manaSys.skill1Use;         
        }
    }

    public void AutoAttack(Transform shotPos)
    {
        if (countTime1st > cooldown)
        {
            countTime1st = 0;
            ShootBullet(bulletPrefab, shotPos, bulletPrefab.GetComponent<Bullet>().bulletSpeed);
            audioPlayer.PlayOneShot(soundClip, 0.4f);
            //GetComponentInParent<DeployBox>().energy -= 2;
        }
    }

    void ShootBullet(GameObject prefabs, Transform shotPos, int shotSpeed)
    {
        GameObject newBullet = Instantiate(prefabs, shotPos.position+(statToken.currentWeapon.gunLength* shotPos.up), Quaternion.identity);
        newBullet.GetComponent<Bullet>().damage = this.damage;
        newBullet.GetComponent<Bullet>().criChance = this.criChance;
        newBullet.GetComponent<Bullet>().criDamage = this.criDamage;
        newBullet.GetComponent<Rigidbody2D>().velocity = shotPos.up * shotSpeed;
    }

    public void SaveValue()
    {
       
    }

    
}
