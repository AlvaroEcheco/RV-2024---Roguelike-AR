using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Estadisticas")]
    public float maxHealth;
    public float maxMana;
    public float health;
    public float mana;
    [Header("Referencias")]
    public PlayerInteraction interaction;
    public Rigidbody rb;
    public Image healthBar;
    public Image manaBar;
    // Start is called before the first frame update
    void Start()
    {
        mana = maxMana;
        health = maxHealth;
        interaction.currentMana = mana;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        manaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<Image>();
    }


    private void Update()
    {
        if (interaction != null)
        {
            interaction.currentMana = mana;
        }
    }

    public void SpendMana(float manaCost)
    {
        mana -= manaCost;
        UpdateStatBars();
    }

    public void TakeDamage(float damage)
    {

        health -= damage;
        UpdateStatBars();
        if (health <= 0)
        {
            dungeonManager.instance.Restart();
            Debug.Log("morido");
            Destroy(gameObject);
        }
    }

    void UpdateStatBars()
    {
        healthBar.fillAmount = health / maxHealth;
        manaBar.fillAmount = mana / maxMana;
    }
}
