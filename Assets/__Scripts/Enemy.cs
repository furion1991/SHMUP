using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f;
    public int scoreValue = 100;

    [Header("Set dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;
    public bool notifiedDestruction = false;

    protected BoundsCheck bndCheck;


    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
    private void Update()
    {
        Move();
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGo = collision.gameObject;
        switch (otherGo.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGo.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGo);
                    break;
                }
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                ShowDamage();
                if (health <= 0)
                {
                    if (!notifiedDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                        
                    }
                    notifiedDestruction = true;
                    Destroy(gameObject);
                }
                Destroy(otherGo);
                break;

            case "LaserHero":
                Laser l = otherGo.GetComponent<Laser>();
                Vector3 initialScale = otherGo.transform.localScale;
                Vector3 pointHit = collision.GetContact(0).thisCollider.transform.position;
                Vector3 pointFiredFrom = l.collar.transform.position;
                Vector3 changedLaserScale = pointHit - pointFiredFrom;
                otherGo.transform.localScale = changedLaserScale;

                health -= Main.GetWeaponDefinition(l.type).continiousDamage;
                ShowDamage();
                
                if (health <= 0)
                {
                    if (!notifiedDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedDestruction = true;
                    Destroy(gameObject);
                }
                break;
                
            default:
                print($"Enemy hit by non-ProjectileHero: {otherGo.name}");
                break;
        }
    }

    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
