using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy_melee : Enemy
{
    public int damage = 1;
    public float variationRadius = 5f;

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

                Quaternion ro = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Random.Range(-variationRadius, variationRadius), transform.rotation.eulerAngles.z);

                Instantiate(proyectilePrefab, FirePoint.position, ro);
                timer = 0;
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
