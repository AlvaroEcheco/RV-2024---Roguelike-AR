using Assets._01_Scripts.Weapons;
using System.Collections;
using UnityEngine;

public class FireGun : MonoBehaviour, InterfaceWeapons
{
    [Header("Referencias")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public FireGunType firegunType;
    public int pellets = 5; // Número de perdigones
    public float spreadAngle = 7f; // Ángulo de dispersión
    public float fireRate = 1f; // Tiempo entre disparos en segundos

    private bool canShoot = true;
    private Coroutine firingCoroutine;

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
        if (canShoot)
        {
            firingCoroutine = StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        

        if (firegunType == FireGunType.Shotgun)
        {
            // Dispara el perdigón principal
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Dispara los perdigones adicionales
            for (int i = 0; i < pellets; i++)
            {
                float randomX = Random.Range(-spreadAngle, spreadAngle);
                float randomY = Random.Range(-spreadAngle, spreadAngle);

                Quaternion rotationVariation = Quaternion.Euler(firePoint.rotation.eulerAngles + new Vector3(randomX, randomY, 0));
                Instantiate(bulletPrefab, firePoint.position, rotationVariation);
            }
        }
        canShoot = false;

        yield return new WaitForSeconds(fireRate);

        canShoot = true;
        yield return new WaitForSeconds(fireRate);
    }

    public void NotAttacking()
    {
        // Código para detener el ataque si es necesario
    }

    public void ResetWeapon()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }

        canShoot = true;
    }
}

public enum FireGunType
{
    Rifle,
    Shotgun
}
