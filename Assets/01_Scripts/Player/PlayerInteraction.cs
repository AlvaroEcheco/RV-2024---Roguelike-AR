using Assets._01_Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referencias")]
    public Transform hand;
    public GameObject objectInFloor;
    public Animator animator;
    public NewInputSystem inputSystem;
    public float currentMana;
    [Header("Armas")]
    public GameObject currentWeapon;
    public List<GameObject> weapons;
    private Coroutine shootingCoroutine; // Referencia a la corrutina de disparo

    private void Awake()
    {
        inputSystem = new NewInputSystem();

        // Cambiar a corrutina para disparar constantemente
        inputSystem.Player.Shoot.performed += ctx => StartAttacking();
        inputSystem.Player.Shoot.canceled += ctx => StopAttacking();
        inputSystem.Player.ChangeWeapon.started += ctx => ChangeWeapon();
        inputSystem.Player.PickUpWeapon.started += ctx => PickUpWeapon(objectInFloor);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    public void StartAttacking()
    {
        // Iniciar corrutina de disparo continuo
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootContinuously());
        }
    }

    public void StopAttacking()
    {
        // Detener corrutina de disparo
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }

        // Detener animaciones cuando se suelta el botón
        StopAnimations();
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            UseWeapon();  // Ejecutar el ataque
            yield return new WaitForSeconds(0.1f);  // Intervalo entre disparos, ajustar según sea necesario
        }
    }

    public void UseWeapon()
    {
        
        if (currentWeapon != null)
        {
            Debug.Log("Atacando");
            InterfaceWeapons weaponUse = currentWeapon.gameObject.GetComponent<InterfaceWeapons>();

            if (weaponUse != null)
            {
                if(currentMana < weaponUse.manaCost)
                {
                    Debug.Log("Te has quedado sin mana");
                    return;
                }
                if (weaponUse.weaponType == WeaponType.Melee)
                {
                    animator.SetBool("MeleeAttack1",true);
                }
                else if (weaponUse.weaponType == WeaponType.Magic)
                {
                    animator.SetBool("CastingSpell", true);
                }
                else if (weaponUse.weaponType == WeaponType.Range)
                {
                    animator.SetBool("Shooting", true);
                }
                weaponUse.Attack();
            }
        }
        
    }

    public void StopAnimations()
    {
        animator.SetBool("Shooting", false);
        animator.SetBool("MeleeAttack1", false);
        animator.SetBool("CastingSpell", false);
    }

    public void ChangeWeapon()
    {
        if (weapons.Count > 1 && currentWeapon != null)
        {
            InterfaceWeapons weaponUse;

            if (weapons[0].activeSelf)
            {
                weaponUse = weapons[0].gameObject.GetComponent<InterfaceWeapons>();
                weaponUse.NotAttacking();
                weapons[0].SetActive(false);
                weaponUse.ResetWeapon(); // Resetea el estado del arma

                weapons[1].SetActive(true);
                currentWeapon = weapons[1];
            }
            else if (weapons[1].activeSelf)
            {
                weaponUse = weapons[1].gameObject.GetComponent<InterfaceWeapons>();
                weaponUse.NotAttacking();
                weapons[1].SetActive(false);
                weaponUse.ResetWeapon(); // Resetea el estado del arma

                weapons[0].SetActive(true);
                currentWeapon = weapons[0];
            }
        }
    }


    public void PickUpWeapon(GameObject newWeapon)
    {
        InterfaceWeapons pickedWeapon = newWeapon.GetComponent<InterfaceWeapons>();
        if (pickedWeapon != null)
        {
            WeaponPickUp pick = newWeapon.GetComponent<WeaponPickUp>();
            pick.isPickUp = true;
            if (weapons.Count < 2)
            {
                // Añadir el arma a la lista si hay espacio
                weapons.Add(newWeapon);
                currentWeapon = newWeapon;
            }
            else if (weapons.Count == 2)
            {
                // Reemplazar el arma que está en la mano
                GameObject weaponToDrop = currentWeapon;
                DropWeapon(newWeapon, weaponToDrop);

                if (weapons[0] == weaponToDrop)
                {
                    weapons[0] = newWeapon;
                }
                else if (weapons[1] == weaponToDrop)
                {
                    weapons[1] = newWeapon;
                }

                currentWeapon = newWeapon;

                pick = weaponToDrop.GetComponent<WeaponPickUp>();
                if (pick != null)
                {
                    pick.isPickUp = false;
                }
            }

            // Asegurar que el nuevo arma se asigne correctamente como hijo del 'hand'
            pickedWeapon.NotAttacking();
            newWeapon.transform.SetParent(hand);
            newWeapon.transform.localPosition = Vector3.zero; // Asegura que se coloque en la misma posición local que la mano
            newWeapon.transform.localRotation = Quaternion.identity; // Asegura que se alinee con la rotación de la mano
            newWeapon.SetActive(true); // Asegura que el arma esté activa

            // Desactiva la otra arma si es necesario
            if (weapons.Count == 2)
            {
                if (weapons[0] == currentWeapon)
                {
                    weapons[1].SetActive(false);
                }
                else if (weapons[1] == currentWeapon)
                {
                    weapons[0].SetActive(false);
                }
            }

            objectInFloor = null;
        }
    }

    private void DropWeapon(GameObject weaponPicked, GameObject weaponDropped)
    {
        Vector3 dropPosition = weaponPicked.transform.position;

        weaponDropped.transform.SetParent(null);
        weaponDropped.transform.position = dropPosition;
        weaponDropped.transform.gameObject.SetActive(true);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        InterfaceWeapons interfaceWeapons = other.GetComponent<InterfaceWeapons>();
        if(interfaceWeapons != null)
        {
            WeaponPickUp weaponPickUp = other.GetComponent<WeaponPickUp>();
            if(!weaponPickUp.isPickUp)
            {
                Debug.Log("Encuentra un arma");
                objectInFloor = other.gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        InterfaceWeapons interfaceWeapons = other.GetComponent<InterfaceWeapons>();
        if (interfaceWeapons != null)
        {
            WeaponPickUp weaponPickUp = other.GetComponent<WeaponPickUp>();
            if (!weaponPickUp.isPickUp)
            {
                Debug.Log("No encuentra arma");
                objectInFloor = null;
            }
        }
    }


}
