using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    [Header("Set Dynamically")]
    
    public Vector3 laserScale;
    public GameObject collar;
    public GameObject laser;
    private Renderer rend2;
    private bool isActive = false;

  
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rigid = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        laser = GetComponent<GameObject>();
        
    }
    private void Update()
    {
        
        Destroy(gameObject);

    }



}
