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
    public int currentMana;
    [Header("Armas")]
    public GameObject currentWeapon;
    public List<GameObject> weapons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UseWeapon();
        if(Input.GetKeyDown(KeyCode.Q) && !Input.GetMouseButton(0))
        {
            ChangeWeapon();
        }
        if(Input.GetKeyDown(KeyCode.F) && objectInFloor != null)
        {
            PickUpWeapon(objectInFloor);
        }
    }

    public void UseWeapon()
    {
        if (Input.GetMouseButton(0) && currentWeapon != null)
        {
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
                    animator.SetBool("CastingSpell", Input.GetMouseButton(0));
                }
                else if (weaponUse.weaponType == WeaponType.Range)
                {
                    animator.SetBool("Shooting", Input.GetMouseButton(0));
                }
                weaponUse.Attack();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Detener la animación cuando se suelta el botón para las armas mágicas
            animator.SetBool("MeleeAttack1", false);
            animator.SetBool("CastingSpell", false);
            animator.SetBool("Shooting", false) ;
        }
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
