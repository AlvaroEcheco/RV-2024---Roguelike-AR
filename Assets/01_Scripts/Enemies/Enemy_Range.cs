using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Range : Enemy
{
    [Header("Base Stats")]
    public float MaxApproach = 2f;
    public bool Approached = false;

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
        if (player != null && canMove)
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
                for (int i = 0; i < proyectileAmount; i++)
                {
                    float x = Random.Range(-variationRadius.x, variationRadius.x);
                    float y = Random.Range(-variationRadius.y, variationRadius.y);

                    Quaternion ro = Quaternion.Euler(transform.rotation.eulerAngles.x + x, transform.rotation.eulerAngles.y + y, transform.rotation.eulerAngles.z);

                    Instantiate(proyectilePrefab, FirePoint.position, ro);
                    timer = 0;
                }
        }
        else timer += Time.deltaTime;
    }

    protected override void Move()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        transform.LookAt(player.transform.position);

        if (distance < MinRange)
        {
            inRange = true;
            if (distance < MinRange - MaxApproach) Approached = true;
        }
        else
        {
            inRange = false;
            Approached = false;
        }



        if (!inRange) Acercarce();
        else
        {
            if (Approached) Alejarse();
            else Acercarce();
        }
    }



    protected void Acercarce()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
    }

    protected void Alejarse()
    {
        Vector3 direction = (transform.position - player.transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
    }
}
