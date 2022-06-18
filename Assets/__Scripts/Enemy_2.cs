using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in inspector: Enemy_2")]
    //ќпредел€ет насколько €рко будет выражен синусоидальный характер движени€
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")]
    //Ёнеми 2 использует линейную интерпол€цию между двум€ точками
    // измен€€ результат по синусоиде
    public Vector3 p0;
    public Vector3 p1;
    public float birthTime;

    private void Start()
    {
        //—лучайна€ точка на левой границе экрана
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //—лучайна€ точка на правой границе экрана
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);

        //—лучайно помен€ть начальную и конечную точку местами
        if (Random.value > 0.5f)
        {
            p0.x *= -1;
            p1.x *= -1;
        }

        //«аписать текущее врем€ в birthTime
        birthTime = Time.time;
    }

    public override void Move()
    {
        //  ривые безье вычисл€ютс€ на основе значени€ u между 0 и 1
        float u = (Time.time - birthTime) / lifeTime;

        //≈сли u>1, значит корабль существует дольше чем lifeTime
        if (u > 1)
        {
            Destroy(gameObject);
            return;
        }

        // —корректировать u добавлением значени€ кривой, измен€ющейс€ по синусоиде
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

        //»нтерполировать местоположение между двум€ точками 
        pos = (1 - u) * p0 + u * p1;
        
    }
}
