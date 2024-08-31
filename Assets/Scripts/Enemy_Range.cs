using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy
{
    public int proyectileAmount = 5;
    public int damage = 1;
    public Vector2 variationRadius = new Vector2(5f, 15f);

    void Start()
    {
        SearchPlayer();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (player != null)
        {
            Move();
            IsPlayerNear();
            Attack();
        }
    }

    protected override void Attack()
    {
        if (timer >= AttackSpeed)
        {
            if (inRange)
            {
                for (int i = 0; i < proyectileAmount; i++)
                {
                    float x = Random.Range(0f, variationRadius.x);
                    float y = Random.Range(0f, variationRadius.y);
                    
                    Quaternion ro = Quaternion.Euler(transform.rotation.eulerAngles.x + x, transform.rotation.eulerAngles.y + y, transform.rotation.eulerAngles.z);

                    Instantiate(proyectilePrefab, FirePoint.position, ro);
                    timer = 0;
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform.position);

        if (distance < MinRange)
        {
            inRange = true;
            if (distance < MinRange - MaxApproach)
            {
                Approached = true;
            }
        }
        else
        {
            inRange = false;
            Approached = false;
        }



        if (!inRange)
        {
            Acercarce();
            Debug.Log("acercado 1");
        }
        else
        {
            if (Approached)
            {
                Alejarse();
            Debug.Log("alejado");
            }
            else
            {
                Acercarce();
            Debug.Log("acercado 2");
            }
        }
    }
}
