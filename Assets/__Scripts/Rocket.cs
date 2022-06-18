using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rocket : Projectile
{

    
    [Header("Set in ispector")]
    private float rocketSpeed;
    public float curve = 0.06f;
    public float u = 0.1f;
    public float birthTime;
    float rotSpeed = 1f;


    [Header("Set dynamically")]
    public GameObject enemyInLock;

    private void Awake()
    {
        
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponentInChildren<Renderer>();
        rigid = GetComponent<Rigidbody>();
        birthTime = Time.time;

    }

    private void Update()
    {
        enemyInLock = LockOnEnemy();
        if (enemyInLock == null)
        {
            transform.position += Vector3.up;
        }
        MoveToEnemy();
        if (bndCheck.offDown||bndCheck.offLeft||bndCheck.offRight||bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }


    public void MoveToEnemy()
    {
        if (enemyInLock == null)
        {
            return;
        }

        Vector3 enemyPos = enemyInLock.transform.position;
        Vector3 myPos = transform.position;
        Quaternion tempRot = transform.GetChild(0).rotation;

        Quaternion rotation = Quaternion.LookRotation(myPos, enemyPos);

        tempRot = Quaternion.Euler(0, 0, rotation.z);

        transform.position = Vector3.MoveTowards(myPos, enemyPos, 0.3f);
        transform.GetChild(0).rotation = rotation;


        //transform.position = (1 - u) * myPos + u * enemyPos;
        //transform.rotation = Quaternion.LookRotation(enemyPos, Vector3.down);
        //if (Vector3.Distance(enemyPos, myPos) <= 5f)
        //{
        //    transform.position = enemyPos;
        //}
    }


    public GameObject LockOnEnemy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyList.Length == 0)
        {
            return null;
        }
        Dictionary<float, GameObject> distancesToEnemies = new Dictionary<float, GameObject>();

        foreach (GameObject enemy in enemyList)
        {
            distancesToEnemies.Add(Vector3.Distance(transform.position, enemy.transform.position), enemy);
        }
        
        float minDistance = distancesToEnemies.Keys.Min();

        return distancesToEnemies[minDistance];
        
       
    }
    
}
