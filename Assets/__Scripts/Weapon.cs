using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Это перечисление всех возможных типов оружия.
/// Также включает тип "Shield" чтобы дать возможность совершенствовать защиту
/// Аббривеатурой [HP] ниже отмечены элементы не реализованные в книге (видимо самостоятельно можно попробовать)
/// </summary>
///
public enum WeaponType
{
    none,
    blaster,
    spread,
    phaser,
    rocket,
    laser,
    shield
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continiousDamage = 0;
    public float delayBetweenShots = 0;
    public float velocity = 20;
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;
    static public Weapon W;

    [Header("Set Dynamically")]
    [SerializeField]
    protected WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    protected Renderer collarRend;
    public GameObject laser;

    private void Start()
    {
        
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        

        SetType(_type);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }

    }
    

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }    

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;

        lastShotTime = 0;
    }

    public void Fire()
    {
        if(!gameObject.activeInHierarchy && type != WeaponType.laser)
        {
            return;
        }

        if (Time.time - lastShotTime < def.delayBetweenShots && type != WeaponType.laser)
        {
            return;
        }

        Projectile p = null;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        
        switch (type)
        {
            
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            case WeaponType.laser:
                FireLaser();
                break;
            case WeaponType.phaser:
                
                MakeTwoPhaserProjectile();
                break;
           
        }

        
    }


    

    public void FireLaser()
    {
        BoundsCheck bndCheck = GetComponent<BoundsCheck>();
        GameObject go = Instantiate(def.projectilePrefab);
        //Collision collision = go.GetComponent<Collision>();

        Vector3 laserScale = go.transform.localScale;

        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "LaserHero";
            go.layer = LayerMask.NameToLayer("LaserHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        Laser p = go.GetComponent<Laser>();
        p.type = type;
        p.collar = collar;

        float addLength = bndCheck.camHeight - collar.transform.position.y - 15;
        laserScale.y += addLength;
        go.transform.localScale = laserScale;

        Vector3 tPos = collar.transform.position;
        tPos.y += go.transform.localScale.y / 2;
        go.transform.position = tPos;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
    }

    
   
    public Projectile MakeProjectile()
    {
        
        GameObject go = Instantiate(def.projectilePrefab);
        
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return p;
    }
    
    public void MakeTwoPhaserProjectile()
    {
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        GameObject phaser1 = Instantiate(def.projectilePrefab);
        GameObject phaser2 = Instantiate(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            phaser1.tag = "ProjectileHero";
            phaser2.tag = "ProjectileHero";
            phaser1.layer = LayerMask.NameToLayer("ProjectileHero");
            phaser2.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            phaser1.tag = "ProjectileEnemy";
            phaser2.tag = "ProjectileEnemy";
            phaser1.layer = LayerMask.NameToLayer("ProjectileEnemy");
            phaser2.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        
        phaser1.transform.position = collar.transform.position;
        phaser1.transform.SetParent(PROJECTILE_ANCHOR, true);
        phaser2.transform.position = collar.transform.position;
        phaser2.transform.SetParent(PROJECTILE_ANCHOR, true);

        PhaserProjectile p1 = phaser1.GetComponent<PhaserProjectile>();
        p1.firstInPair = true;
        PhaserProjectile p2 = phaser2.GetComponent<PhaserProjectile>();
        p2.firstInPair = false;

        p1.rigid.velocity = p2.rigid.velocity = vel;

        p = phaser1.GetComponent<Projectile>();
        p.type = type;
        p = phaser2.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
    }
    
}


