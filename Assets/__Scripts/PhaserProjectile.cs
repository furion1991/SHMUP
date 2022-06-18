using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaserProjectile : Projectile
{
    public float birthTime;
    public float waveFrequency = 2;
    public float pitch = 40;
    public bool firstInPair = true;

    public Vector3 pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private void Start()
    {
        birthTime = Time.time;
    }

    void Update()
    {
        if (firstInPair)
        {
            Vector3 tempPos = pos;
            float age = Time.time - birthTime;
            float theta = Mathf.PI * 2 * age / waveFrequency;

            float sin = Mathf.Sin(theta);

            tempPos.x = transform.position.x + pitch * sin;
            pos = tempPos;
        }
        else
        {
            Vector3 tempPos = pos;
            float age = Time.time - birthTime;
            float theta = Mathf.PI * 2 * age / waveFrequency;

            float sin = - Mathf.Sin(theta);

            tempPos.x = transform.position.x + pitch * sin;
            pos = tempPos;
        }
        

    }
}
