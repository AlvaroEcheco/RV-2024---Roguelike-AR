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

    public Cuarto cuarto;
    public bool isdead = false;

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
        bool isMoving = rb.velocity.magnitude > 0; 
        animator.SetBool("Running", isMoving);
        float veloci = rb.velocity.magnitude;
        Debug.Log(veloci);
        if (inRange == false)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * Speed * Time.deltaTime);
            
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
        animator.SetTrigger("Cross");
    }

    void Arriba()
    {
        animator.SetTrigger("Upper");
    }

    void Invocar()
    {
        cuarto.EnemyCant = EnemyCount;
        cuarto.Enemies.Clear();
        cuarto.Enemies.Add(EnemyPrefab);
        cuarto.Invocar();
        cuarto.EnemyCant++;
    }
    #endregion

    private bool hasInvoqued = false;
    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (!isdead)
        {
            
            
            if (Health <= MaxHealth * 0.3 && !hasInvoqued)
            {
                animator.SetTrigger("Scream");
                Invocar();
                hasInvoqued = true;
            }
            if (Health <= 0)
            {
                isdead = true;
                animator.SetTrigger("Death");
                Destroy(gameObject, 10);
                cuarto.ContarEnemigos(1);
                
            }
        }
    }
}