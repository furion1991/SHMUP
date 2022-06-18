using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hero : MonoBehaviour
{
    static public Hero S;
    [Header("Set in inspector")]
    public float gameRestartDelay = 2f;
    public float speed = 30f;
    public float rollMult = -45f;
    public float pitchMult = 30;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public Weapon[] weapons;

    [Header("Set dynamically")]
    public BoundsCheck boundsCheck;
    [SerializeField]
    private float _shieldLevel = 1;
    

    private GameObject lastTriggerGo = null;                                    //a
    public delegate void WeaponFireDelegate();
    public delegate void RocketLaunchDelegate();
    public WeaponFireDelegate fireDelegate;
    public RocketLaunchDelegate launchDelegate;

    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");    
        }
        // fireDelegate += TempFire;
        ClearWeapons();
        weapons[0].SetType(WeaponType.spread);
    }

    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }

        if (Input.GetKeyDown(KeyCode.Q) && launchDelegate != null)
        {
            launchDelegate();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
                                                                                //b

        if (go == lastTriggerGo)                                                //c
        {
            return;
        }

        lastTriggerGo = go;                                                     //d

        if (go.tag == "Enemy")
        {
            shieldLevel--;
            Destroy(go);                                                        //e
        }
        else if (go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print($"Triggered by non-Enemy: {go.name}");                        //f
        }
    }

    public float shieldLevel
    {
        get { return _shieldLevel; }

        set 
        {
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(gameObject);
                ScoreCounter.SetScoreTable();
                ScoreCounter.score = 0;
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return weapons[i];
            }
        }
        return weapons[weapons.Length - 1];
    }

    void ClearWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetType(WeaponType.none);
        }
    }

    void TempFire()
    {
        GameObject projGO = Instantiate(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //rigidB.velocity = Vector3.up * projectileSpeed;
        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }

    void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                if (shieldLevel >= 4)
                {
                    ScoreCounter.score += 50;
                }
                break;
            case WeaponType.blaster:
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                    else
                    {
                        ClearWeapons();
                        weapons[0].SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;
            case WeaponType.laser:
                if (pu.type == weapons[0].type)
                {
                    break;
                }
                ClearWeapons();
                weapons[0].SetType(pu.type);
                break;
            case WeaponType.phaser:
                if (pu.type == weapons[0].type)
                {
                    break;
                }
                ClearWeapons();
                weapons[0].SetType(pu.type);
                break;
            case WeaponType.spread:
                if (pu.type == weapons[0].type)
                {
                    if (weapons[0].def.delayBetweenShots >= 0.1f)
                    {
                        weapons[0].def.delayBetweenShots -= 0.05f;
                    }
                    break;
                }
                ClearWeapons();
                weapons[0].SetType(pu.type);
                break;
            default:
                break;
               
        }
        pu.AbsorbedBy(gameObject);
        
    }
}
