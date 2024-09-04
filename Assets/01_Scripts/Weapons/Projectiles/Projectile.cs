using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Referencias")]
    public float speed;
    public float timeToDestroy = 1f;
    public bool playerProjectile = true;
    public float damage = 1f;
    public float knockBack = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, 0);
    }
    void OnTriggerEnter(Collider collision)
    {
        if (playerProjectile && collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * knockBack, ForceMode.Impulse);
            }
            
            Debug.Log("Enemigo daniado");
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        if(playerProjectile && collision.gameObject.CompareTag("Boss"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Boss boss = collision.gameObject.GetComponent<Boss>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * knockBack, ForceMode.Impulse);
            }

            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        if(!playerProjectile && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Player player = collision.gameObject.GetComponent<Player>();
            if (rb != null)
            {
                rb.AddForce(transform.forward * knockBack, ForceMode.Impulse);
            }

            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        if(collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
