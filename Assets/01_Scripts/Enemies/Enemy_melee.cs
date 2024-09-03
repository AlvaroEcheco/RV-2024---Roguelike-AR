using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy_melee : Enemy
{
    [Header("Base Stats")]
    public int damage = 1;
    public float variationRadius = 5f;
    public Animator animator;

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
        animator.SetBool("Attack", inRange);
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

        if (distance < MinRange) inRange = true;
        else inRange = false;



        if (!inRange)
        {
            Acercarce();
        }
    }


    protected void Acercarce()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);

    }
}
