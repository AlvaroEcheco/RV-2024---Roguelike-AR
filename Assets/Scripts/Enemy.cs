using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    public int MaxHealt = 10;
    public int Healt = 0;

    public float Speed = 5f;

    public float AttackSpeed = 2f;
    protected float timer = 0;

    public float MinRange = 7f;
    public bool inRange = false;

    [Header("References")]
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
                dungeonManager.instance.RevisarEnemigos();
            }
        }
    }
}

