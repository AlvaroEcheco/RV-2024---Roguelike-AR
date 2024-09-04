using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public bool isPickUp = false;
    private GameObject player;
    private Collider weaponCollider;

    private void Start()
    {
        weaponCollider = GetComponent<Collider>();
    }

    void Update()
    {
        //if (player != null && isPickUp == false && Input.GetKeyDown(KeyCode.F))
        //{
        //    PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
        //    interaction.PickUpWeapon(gameObject);
        //    isPickUp = true;
        //}
        if(!isPickUp)
        {
            weaponCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            player = null;
        }
    }
}
