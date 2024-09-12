using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : Enemy
{
    [Header("Base Stats")]
    public float SpeedJump = 2f; // solo para el slime
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        SearchPlayer();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && canMove)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            Attack();
        }
    }

    protected override void Move()
    {
        return;
        // se mueve saltando
    }

    protected override void Attack()
    {
        if (timer >= AttackSpeed)
        {
            Saltar();

            timer = 0;
        }
        else timer += Time.deltaTime;
    }

    protected void Saltar()
    {
        Vector3 direccionAdelante = transform.forward;
        rb.AddForce(new Vector3(direccionAdelante.x * SpeedJump, Speed, direccionAdelante.z * SpeedJump), ForceMode.VelocityChange);
    }
}
