using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base Stats")]
    public int MaxHealt = 10;
    public float Healt = 0;

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
    public Cuarto cuarto;

    public bool canMove = true;
    public bool dead = false;

    protected abstract void Move();
    protected abstract void Attack();

    protected void SearchPlayer()
    {
        if (canMove)
        {
            GameObject aux = GameObject.FindGameObjectWithTag("Player");
            if (aux != null)
            {
                player = aux.GetComponent<Player>();
            }
        }

    }

    protected void IsPlayerNear()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= MinRange)
            inRange = true;
        else
            inRange = false;
    }

    public void TakeDamage(float damage)
    {
        Healt -= damage;
        if (Healt <= 0 && !dead)
        {
            cuarto.ContarEnemigos(1);
            StartCoroutine(Die());
            dead = true;
        }
    }

    private IEnumerator Die()
    {
        canMove = false;
        float elapsedTime = 0f;
        LayerMask layer = gameObject.layer;
        
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 90));

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        canMove = false;
        Destroy(gameObject, 10f);
    }
}

