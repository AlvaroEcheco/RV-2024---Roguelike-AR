using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumible : MonoBehaviour
{
    public ConsumibleType type;
    public int recuperation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                if(type == ConsumibleType.Health)
                {
                    player.health += recuperation;
                    if (player.health > player.maxHealth)
                    {
                        player.health = player.maxHealth;
                    }
                }
                else if(type == ConsumibleType.Mana)
                {
                    player.mana += recuperation;
                    if (player.mana > player.maxMana)
                    {
                        player.mana = player.maxMana;
                    }
                }
                
            }
            Destroy(gameObject);
        }
    }
}
public enum ConsumibleType
{
    Health,
    Mana
}
