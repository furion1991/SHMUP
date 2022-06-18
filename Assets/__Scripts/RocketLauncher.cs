using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public int rocketCounter = 0;
    bool isActive = false;

    private void Start()
    {
        collar = transform.Find("CollarRocket").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        SetType(WeaponType.rocket);

        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().launchDelegate += LaunchRocket;
        }
    }

    public void LaunchRocket()
    {
        if (rocketCounter <= 0)
        {
            return;
        }
        

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
        rocketCounter--;
        
    }
}
