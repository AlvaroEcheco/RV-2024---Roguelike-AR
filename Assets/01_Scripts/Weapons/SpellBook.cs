using Assets._01_Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour, InterfaceWeapons
{
    [Header("Referencias")]
    public GameObject castObjectPrefab;
    public Transform castPoint;

    [Header("Estadísticas")]
    public float damage;
    public float castForce;
    public float timeBtwCast;
    [SerializeField]
    private WeaponType _weaponType = WeaponType.Magic;
    public int _manaCost = 1;

    public int manaCost
    {
        get => _manaCost;
        set => _manaCost = value;
    }

    public WeaponType weaponType
    {
        get => _weaponType;
        set => _weaponType = value;
    }

    private bool canCast = true;
    private Coroutine castingCoroutine;

    public void Attack()
    {
        if (canCast)
        {
            castingCoroutine = StartCoroutine(CastSpell());
        }
    }

    public void NotAttacking()
    {
        // Lógica para cuando el arma no está atacando, si es necesario.
    }

    public void ResetWeapon()
    {
        if (castingCoroutine != null)
        {
            StopCoroutine(castingCoroutine);
        }
        canCast = true;
    }

    private IEnumerator CastSpell()
    {
        canCast = false;
        yield return new WaitForSeconds(0.001f);

        // Crear la instancia del objeto arrojadizo
        GameObject instantiatedObject = Instantiate(castObjectPrefab, castPoint.position, castPoint.rotation);

        GameObject aux = GameObject.FindGameObjectWithTag("Player");
        Player player = aux.GetComponent<Player>();
        if(player != null)
        {
            player.SpendMana(manaCost);
        }

        // Obtener el Rigidbody del objeto recién instanciado
        Rigidbody rb = instantiatedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * castForce, ForceMode.Impulse);
        }

        // Esperar el tiempo entre lanzamientos antes de permitir otro lanzamiento
        yield return new WaitForSeconds(timeBtwCast);

        canCast = true;
    }
}
