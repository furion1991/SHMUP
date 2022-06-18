using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Part
{
    [Header("Set in Ispector")]
    public string name;
    public float health;
    public string[] protectedBy;

    [HideInInspector]
    public GameObject go;
    [HideInInspector]
    public Material mat;

}

/// <summary>
/// Enemy_4 создается за верхней границей, выбирает случайную точку на экране
/// и перемещается к ней. Добравшись до места, выбирает другую случайную точку
/// и продолжает двидаться, пока игрок не уничтожит его
/// </summary>
public class Enemy_4 : Enemy
{
    [Header("Set in ispector: Enemy_4")]
    public Part[] parts;

    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4;

    private void Start()
    {
        p0 = p1 = pos;
        InitMovement();

        Transform t;
        foreach (Part part in parts)
        {
            t = transform.Find(part.name);
            if (t != null)
            {
                part.go = t.gameObject;
                part.mat = part.go.GetComponent<Renderer>().material;
            }
        }
    }

    void InitMovement()
    {
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        timeStart = Time.time;
    }

    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }

    Part FindPart(string n)
    {
        foreach (var prt in parts)
        {
            if (prt.name == n)
            {
                return prt;
            }
        }
        return null;
    }                                                      //a

    Part FindPart(GameObject go)
    {
        foreach (var prt in parts)
        {
            if (prt.go == go)
            {
                return prt;
            }
        }
        return null;
    }                                                 //b

    bool Destroyed(GameObject go)
    {
        return Destroyed(FindPart(go));
    }
    bool Destroyed (string n)
    {
        return Destroyed(FindPart(n));
    }
    bool Destroyed (Part prt)
    {
        if (prt == null)
        {
            return true;
        }
        return prt.health <= 0;
    }

    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }                                         //d

    private void OnCollisionEnter(Collision collision)                              //e
    {
       GameObject other = collision.gameObject;

        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                GameObject goHit = collision.GetContact(0).thisCollider.gameObject;   //f
                Part prtHit = FindPart(goHit);
                if (prtHit == null)                                                 //g
                {
                    goHit = collision.GetContact(0).otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                if (prtHit.protectedBy != null)                                     //h
                {
                    foreach (var s in prtHit.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;

                ShowLocalizedDamage(prtHit.mat);

                if (prtHit.health <= 0)                                             //i
                {
                    prtHit.go.SetActive(false);
                }

                bool allDestroyed = true;

                foreach (var prt in parts)
                {
                    if(!Destroyed(prt))
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed)                                               
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(gameObject);
                }
                Destroy(other);
                break;
            case "LaserHero":
                Laser l = other.GetComponent<Laser>();
                
                GameObject goHit1 = collision.GetContact(0).thisCollider.gameObject;   //f
                Part prtHit1 = FindPart(goHit1);
                if (prtHit1 == null)                                                 //g
                {
                    goHit1 = collision.GetContact(0).otherCollider.gameObject;
                    prtHit1 = FindPart(goHit1);
                }

                if (prtHit1.protectedBy != null)                                     //h
                {
                    foreach (var s in prtHit1.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            Destroy(other);
                            return;
                        }
                    }
                }
                prtHit1.health -= Main.GetWeaponDefinition(l.type).continiousDamage;

                ShowLocalizedDamage(prtHit1.mat);

                if (prtHit1.health <= 0)                                             //i
                {
                    prtHit1.go.SetActive(false);
                }

                bool allDestroyed1 = true;

                foreach (var prt in parts)
                {
                    if (!Destroyed(prt))
                    {
                        allDestroyed1 = false;
                        break;
                    }
                }
                if (allDestroyed1)
                {
                    Main.S.ShipDestroyed(this);
                    Destroy(gameObject);
                }
                Destroy(other);
                break;

                
        }
    }
}
