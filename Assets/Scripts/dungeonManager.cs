using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class dungeonManager : MonoBehaviour
{
    public int Nivel = 3;
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
        UnirListas(aux.Caminos);
        CantCuartos++;

        while (proximosCuartos.Count > 0)
        {
            GameObject spawnPoint = proximosCuartos.Dequeue();

            if (!Utilidades.BuscarArea(spawnPoint.transform.position, Vector3.one * 5))
            {
                Vector3 p = new Vector3(spawnPoint.transform.position.x, 0, spawnPoint.transform.position.z);
                if (CantCuartos <= Nivel)
                {
                    CantCuartos++;
                    int a = Random.Range(0, Cuartos.Count);
                    aux = Instantiate(Cuartos[a], p, Quaternion.identity);
                    aux.transform.LookAt(spawnPoint.transform.root.transform.position);// MIRAR AL SPAWNPOINT PARA ALINEAR LA PUERTA
                    UnirListas(aux.Caminos);
                }
                else
                {
                    aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 1), p, Quaternion.identity);
                    aux.transform.LookAt(spawnPoint.transform.root.transform.position);// MIRAR AL SPAWNPOINT PARA ALINEAR LA PUERTA
                }
            }
        }
    }

    void UnirListas(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            proximosCuartos.Enqueue(item);
        }
    }
}
