using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Estadisticas")]
    public float maxHealth;
    public int maxMana;
    public float health;
    public int mana;
    [Header("Referencias")]
    public PlayerInteraction interaction;
    // Start is called before the first frame update
    void Start()
    {
        mana = maxMana;
        health = maxHealth;
        interaction.currentMana = mana;
    }

    private void Update()
    {
        if (interaction != null)
        {
            interaction.currentMana = mana;
        }
    }

    public void SpendMana(int manaCost)
    {
        mana -= manaCost;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            dungeonManager.instance.Restart();
            Debug.Log("morido");
        }
    }
}
