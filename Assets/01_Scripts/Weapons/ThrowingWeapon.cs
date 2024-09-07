using Assets._01_Scripts.Weapons;
using System.Collections;
using UnityEngine;

public class ThrowingWeapon : MonoBehaviour, InterfaceWeapons
{
    [Header("Referencias")]
    public GameObject throwingObjectPrefab;

    [Header("Estadísticas")]
    public float damage;
    public float throwForce;
    public float timeBtwThrow;

    private bool canThrow = true;
    private Coroutine throwingCoroutine;

    [SerializeField]
    private WeaponType _weaponType = WeaponType.Magic;

    public WeaponType weaponType
    {
        get => _weaponType;
        set => _weaponType = value;
    }

    public int _manaCost = 1;

    public int manaCost
    {
        get => _manaCost;
        set => _manaCost = value;
    }

    public void Attack()
    {
        if (canThrow)
        {
            throwingCoroutine = StartCoroutine(Throw());
        }
    }

    public void NotAttacking()
    {
        // Lógica para cuando el arma no está atacando, si es necesario.
    }

    public void ResetWeapon()
    {
        if (throwingCoroutine != null)
        {
            StopCoroutine(throwingCoroutine);
        }
        canThrow = true;
    }

    private IEnumerator Throw()
    {
        canThrow = false;

        // Crear la instancia del objeto arrojadizo
        GameObject instantiatedObject = Instantiate(throwingObjectPrefab, transform.position, transform.rotation);

        // Obtener el Rigidbody del objeto recién instanciado
        Rigidbody rb = instantiatedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        // Esperar el tiempo entre lanzamientos antes de permitir otro lanzamiento
        yield return new WaitForSeconds(timeBtwThrow);

        canThrow = true;
    }
}
