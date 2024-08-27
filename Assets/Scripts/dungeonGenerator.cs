using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class dungeonGenerator : MonoBehaviour
{
    public int CantMaxCuartos = 3;
    private int CantCuartos = 0;
    public List<Cuarto> Cuartos;
    
    private Queue<GameObject> proximosCuartos = new Queue<GameObject>();

    void Start()
    {
        Generar();
    }

    void Generar()
    {
        Cuarto aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 4), Vector3.zero, Quaternion.identity);
        unirListas( aux.Caminos );
        Debug.Log("faltan " + proximosCuartos.Count + " cuartos");
        CantCuartos++;

        while (proximosCuartos.Count > 0)
        {
            GameObject spawnPoint = proximosCuartos.Dequeue();
            Debug.Log("faltan " + proximosCuartos.Count + " cuartos");

            if (!BuscarArea(spawnPoint.transform.position, Vector3.one))
            {
                if (CantCuartos <= CantMaxCuartos)
                {
                    CantCuartos++;
                    int a = Random.Range(0, Cuartos.Count);
                    aux = Instantiate(Cuartos[a], spawnPoint.transform.position, Quaternion.identity);
                    aux.transform.LookAt(spawnPoint.transform.root.transform.position);
                    unirListas(aux.Caminos);
                }
                else
                {
                    aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 0 || x.Caminos.Count == 1), spawnPoint.transform.position, Quaternion.identity);

                    aux.transform.LookAt(spawnPoint.transform.root.transform.position);// MIRAR AL SPAWNPOINT PARA ALINEAR LA PUERTA
                }
            }
        }
    }

    bool BuscarArea(Vector3 position, Vector3 size)
    {
        Collider[] colliders = Physics.OverlapBox(position, size / 2);
        if (colliders.Length > 0)
        {
            Debug.Log("Hay un objeto en la posición");
            return true;
        }
        else
        {
            Debug.Log("No hay objetos en la posición");
            return false;
        }
    }

    void unirListas(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            proximosCuartos.Enqueue(item);
        }
    }
}
