using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy_melee : Enemy
{
    [Header("Base Stats")]
    public int damage = 1;
    public float variationRadius = 5f;
    public float timeBtwAttack;

    [Header("Referencias")]
    public GameObject currentWeapon;
    private MeleeEnemy meleeEnemyWeapon;
    private void FixedUpdate()
    {
        SearchPlayer();
    }

    void Start()
    {
        rb.freezeRotation = true;
        meleeEnemyWeapon = currentWeapon.GetComponent<MeleeEnemy>();
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
        
        if (timer >= timeBtwAttack)
        {
            if (inRange)
            {
                Quaternion ro = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Random.Range(-variationRadius, variationRadius), transform.rotation.eulerAngles.z);

                animator.SetTrigger("Attacking");
                StartCoroutine(meleeEnemyWeapon.Swing(timeBtwAttack));
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
        animator.SetBool("Running", !inRange);
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
