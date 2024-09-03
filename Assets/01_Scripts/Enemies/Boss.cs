using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Estadísticas")]
    public float MaxHealth = 50;
    public float Health = 0;

    public float Speed = 3f;

    public float TimeBtwAttack = 2f;
    private float AttackTimer = 0;

    public float MinRange = 3f;
    public bool inRange = false;

    public int EnemyCount = 5;
    public Enemy EnemyPrefab;

    [Header("Referencias")]
    private Player player;
    public Rigidbody rb;
    public Animator animator;
    public GameObject currentWeapon;
    private MeleeEnemy meleeEnemyWeapon;

    public Transform AttackPoint;

    void Start()
    {
        Health = MaxHealth;
        rb = GetComponent<Rigidbody>();
        SearchPlayer();
        rb.freezeRotation = true;
        meleeEnemyWeapon = currentWeapon.GetComponent<MeleeEnemy>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            IsPlayerNear();

            Attack();
            Move();

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

    void Move()
    {
        if (inRange == false)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
            animator.SetBool("Walking", true);
        }
    }

    #region Attacks
    void Attack()
    {
        if(AttackTimer <= TimeBtwAttack)
        {
            AttackTimer += Time.deltaTime;
        }
        else
        {
            if (inRange == true)
            {
                if (Random.Range(0, 2) == 0) Barrido();
                else Arriba();
                StartCoroutine(meleeEnemyWeapon.Swing(TimeBtwAttack));  // Ejecuta el ataque

                AttackTimer = 0;
            }
        }
    }

    void Barrido()
    {
        animator.SetTrigger("Barrido");
    }

    void Arriba()
    {
        animator.SetTrigger("Arriba");
    }

    void Invocar()
    {
        Cuarto cuarto = transform.parent.parent.GetComponent<Cuarto>();
        cuarto.EnemyCant = EnemyCount;
        cuarto.Invocar();
    }
    #endregion

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            Destroy(gameObject);
            dungeonManager.instance.RevisarEnemigos();
        }
        else if (Health <= MaxHealth * 0.3)
        {
            Invocar();
        }
        //else if (Health <= MaxHealth * 0.6)
        //{
        //    Speed *= 1.2f;
        //    TimeBtwAttack *= 0.8f;
        //}
        //else if (Health <= MaxHealth * 0.8)
        //{
        //    Speed *= 1.2f;
        //}
    }
}
