using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Base Stats")]
    public int MaxHealt = 50;
    public int Healt = 0;

    public float Speed = 3f;

    public float AttackSpeed = 5f;
    protected float AttackTimer = 0;

    public float MinRange = 3f;
    public bool inRange = false;
    
    public float StunedTime = 3f;
    public float StuTimer = 0;
    public bool Stuned = false;

    public int EnemyCant = 5;
    public Enemy EnemyPrefab;

    Player player;
    public Rigidbody rb;
    public GameObject Barridorefab;
    public GameObject ArribaPrefab;

    public Transform AttackPoint;

    void Start()
    {
        SearchPlayer();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
            if (Stuned)
            {
                Recuperarse();
            }
            else
            {
                IsPlayerNear();

                Attack();
                if (!inRange)
                {
                    Move();
                }
            }
        }
    }

    void SearchPlayer()
    {
        GameObject aux = GameObject.FindGameObjectWithTag("Player");
        if (aux != null)
        {
            player = aux.GetComponent<Player>();
        }
    }

    void IsPlayerNear()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= MinRange)
            inRange = true;
        else
            inRange = false;
    }

    void Recuperarse()
    {
        if (StuTimer >= StunedTime)
        {
            Stuned = false;
            StuTimer = 0;
        }
        else
        {
            StuTimer += Time.deltaTime;
        }
    }

    void Move()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < MinRange) inRange = true;
        else inRange = false;

        if (!inRange)
        {
            Acercarce();
        }
    }
    void Acercarce()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
    }

    #region Attacks
    void Attack()
    {
        if (AttackTimer >= AttackSpeed)
        {
            if (inRange)
            {
                if (Random.Range(0, 2) == 0) Barrido();
                else Arriba();

                AttackTimer = 0;
            }
        }
        else
        {
            AttackTimer += Time.deltaTime;
        }
    }

    // tres ataques
    void Barrido()
    {
        GameObject obj = Instantiate(Barridorefab, AttackPoint);
        Destroy(obj,1f);

        StunedTime = 3f;
        Stuned = true;
    }

    void Arriba()
    {
        GameObject obj = Instantiate(ArribaPrefab, AttackPoint);
        Destroy(obj, 1f);

        StunedTime = 4f;
        Stuned = true;
    }

    void Invocar()
    {
        Cuarto cuarto = transform.parent.parent.GetComponent<Cuarto>();
        cuarto.EnemyCant = EnemyCant;
        cuarto.Invocar();
    }
    #endregion

    public void TakeDamage(int cant)
    {
        Healt -= cant;

        if (Healt <= 0)
        {
            Destroy(gameObject);
            dungeonManager.instance.RevisarEnemigos();
        }
        else if (Healt <= MaxHealt * 0.3)
        {
            Invocar();
            StunedTime = 5f;
            Stuned = true;
        }
        else if (Healt <= MaxHealt * 0.6)
        {
            Speed *= 1.2f;
            AttackSpeed *= 0.8f;
        }
        else if (Healt <= MaxHealt * 0.8)
        {
            Speed *= 1.2f;
        }
    }
}
