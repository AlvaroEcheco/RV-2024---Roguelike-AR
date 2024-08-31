using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    public int MaxHealt = 10;
    public int Healt = 0;

    public int Speed = 5;

    public float AttackSpeed = 2f;
    protected float timer = 0;

    public float MinRange = 7f;
    public float MaxApproach = 2f; // se le resta a minRange
    public bool Approached = false;
    public bool inRange = false;

    protected Player player;
    public Rigidbody rb;
    public GameObject proyectilePrefab;
    public Transform FirePoint;

    protected abstract void Move();
    protected abstract void Attack();

    protected void SearchPlayer()
    {
        GameObject aux = GameObject.FindGameObjectWithTag("Player");
        if (aux != null)
        {
            player = aux.GetComponent<Player>();
        }
    }

    protected void IsPlayerNear()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= MinRange)
            inRange = true;
        else
            inRange = false;
    }


    protected void Acercarce()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
    }
    protected void Acercarse2()
    {
        float angle = 0;
        float Radius = 0;

        // Actualiza el ángulo para que el enemigo se mueva continuamente alrededor del jugador
        angle += Speed * Time.deltaTime / Radius;
        if (angle >= 2 * Mathf.PI) angle -= 2 * Mathf.PI;   

        // Calcula la posición circular alrededor del jugador
        float x = player.transform.position.x + Mathf.Cos(angle) * Radius;
        float z = player.transform.position.z + Mathf.Sin(angle) * Radius;
        Vector3 targetPosition = new Vector3(x, transform.position.y, z);

        // Mueve el enemigo hacia la nueva posición
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
    }

    protected void Alejarse()
    {
        Vector3 direction = (transform.position - player.transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
    }
}

