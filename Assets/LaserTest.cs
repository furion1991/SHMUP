using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest
{
    [Header("Set in inspector")]
    public GameObject laser;

    [Header("Set dynamicaly")]
    public BoundsCheck bndCheck;
    public GameObject collar;
    public float laserLength;
    public bool isActive = false;

    void MakeBeam()
    {
        
        collar = GameObject.Find("Collar");
        
    }
}
