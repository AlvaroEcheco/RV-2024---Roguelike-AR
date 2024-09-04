using Assets._01_Scripts.Weapons;
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour, InterfaceWeapons
{
    [Header("Referencias")]
    BoxCollider swordCollider;
    WeaponPickUp pickedUp;
    public bool canDealDamage = false;
    public Animator animator;
    [Header("Estadísticas")]
    public float damage;
    public float knockBack;
    public float timeBtwAttack;
    private bool isAttacking;
    private bool canAttack = true;
    private Coroutine attackCoroutine;

    [SerializeField]
    private WeaponType _weaponType = WeaponType.Melee;

    public WeaponType weaponType
    {
        get => _weaponType;
        set => _weaponType = value;
    }

    public int _manaCost = 0;

    public int manaCost
    {
        get => _manaCost;
        set => _manaCost = value;
    }

    void Start()
    {
        swordCollider = GetComponent<BoxCollider>();
        pickedUp = GetComponent<WeaponPickUp>();
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            canAttack = true;
        }
        if (pickedUp.isPickUp) canDealDamage = true;
        else canDealDamage = false;


    }

    public void Attack()
    {
        if (canAttack)
        {
            attackCoroutine = StartCoroutine(Swing());
        }
    }

    public void NotAttacking()
    {
        swordCollider.enabled = false;
    }

    public void ResetWeapon()
    {
        // Detener la corrutina de ataque si está en curso
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }

        // Resetear el estado del arma
        NotAttacking();
        canAttack = true;
    }

    IEnumerator Swing()
    {
        canAttack = false;
        swordCollider.enabled = true;
        yield return new WaitForSeconds(timeBtwAttack);
        NotAttacking();
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(canDealDamage)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            Boss boss = other.gameObject.GetComponent<Boss>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * knockBack, ForceMode.Impulse);
                }
            }
            if(boss != null)
            {
                boss.TakeDamage(damage);
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * knockBack, ForceMode.Impulse);
                }
            }
        }  
    }
}
