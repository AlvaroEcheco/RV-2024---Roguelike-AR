using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuarto : MonoBehaviour
{
    public List<GameObject> Caminos;
    public List<GameObject> Puertas;

    public Transform Center;
    public Transform TopRight;
    public Transform BotomLeft;
    public GameObject Contenido;

    public int EnemyCant = 3;
    public int enemiesKilled = 0;
    public List<Enemy> Enemies;
    public Boss Bosse;

    public bool isBattle = true;
    public bool isComplete = false;
    public bool isActive = false;
    public bool isBoss = false;

    void Start()
    {
        EnemyCant = Mathf.Max(2, dungeonManager.instance.Nivel / 2);
    }

    public void Invocar()
    {
        if (!isActive)
        {
            if (isBoss)
            {
                Boss x = Instantiate(Bosse, Center.position, Quaternion.identity);
                x.transform.parent = Contenido.transform;
                x.cuarto = this;
            }
            else
            {
                for (int i = 0; i < EnemyCant; i++)
                {
                    int r = Random.Range(0, Enemies.Count);
                    Enemy x = Instantiate(Enemies[r], new Vector3(Random.Range(BotomLeft.position.x, TopRight.position.x), 0, Random.Range(BotomLeft.position.z, TopRight.position.z)), Quaternion.identity);
                    x.transform.parent = Contenido.transform;
                    x.cuarto = this;
                }
            }
            isActive = true;
        }
    }

    public void ContarEnemigos(int cant)
    {
        enemiesKilled += cant;
        Debug.Log(enemiesKilled + " de " + EnemyCant);
        if (enemiesKilled >= EnemyCant)
        {
            Completar();
        }
    }

    public void Completar()
    {
        isComplete = true;
        AbrirPuertas();
        if (isBoss)
        {
            Instantiate(dungeonManager.instance.dungeonPoint.Portal, Center);
        }
    }

    private void CerrarPuertas()
    {
        foreach (var item in Puertas)
        {
            item.SetActive(true);
        }
    }
    private void AbrirPuertas()
    {
        foreach (var item in Puertas)
        {
            item.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && isBattle && !isComplete)
        {
            if (other.CompareTag("Player"))
            {

                dungeonManager.instance.dungeonPoint.Cu = this;
                CerrarPuertas();
                Invocar();
            }
        }
        //cuando entra el player
        //invoca los enemigos?

        //cerrar puertas
        //asigna este cuarto a dungeomManager
    }
}