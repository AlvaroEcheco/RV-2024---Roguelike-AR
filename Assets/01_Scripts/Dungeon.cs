using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public int nivel;
    public int CantCuartos = 0;
    public int CantEventos = 1;

    public List<Cuarto> Cuartos;
    public Portal Portal;
    public GameObject Event;

    public Cuarto Cu;
    public Vector3 position;

    Queue<GameObject> proximosCuartos = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        nivel = dungeonManager.instance.Nivel;
        dungeonManager.instance.dungeonPoint = this;
    }

    public void GenerarSalas()
    {
        Cuarto aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 4), position, Quaternion.identity);
        aux.isBattle = false;
        UnirListas(aux.Caminos);
        CantCuartos++;
        aux.transform.parent = transform;

        Debug.Log("faltan generar " + proximosCuartos.Count);
        while (proximosCuartos.Count > 0)
        {
            GameObject spawnPoint = proximosCuartos.Dequeue();
            if (!BuscarArea(spawnPoint.transform.position, 4))
            {
                Vector3 p = new Vector3(spawnPoint.transform.position.x, 0, spawnPoint.transform.position.z);
                if (CantCuartos <= nivel)
                {
                    aux = Instantiate(Cuartos[Random.Range(0, Cuartos.Count)], p, Quaternion.identity);
                    UnirListas(aux.Caminos);
                }
                else
                {
                    aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 1), p, Quaternion.identity);
                }
                aux.transform.LookAt(PadrePadre(spawnPoint.transform));// MIRAR AL SPAWNPOINT PARA ALINEAR LA PUERTA
                aux.transform.parent = transform;
                CantCuartos++;
            }
        }

        GenerarEventos(aux);
    }
    void GenerarEventos(Cuarto aux)
    {
        //genera el portal o jefe en el ultimo cuarto creado
        if (nivel % 5 == 0)
        {
            aux.isBoss = true;
        }
        else
        {
            Instantiate(Portal, aux.Center);
            aux.isBattle = false;
        }

        //genera los cuartos especiales
        List<Cuarto> cuartos = transform.GetComponentsInChildren<Cuarto>()
            .Where(x => x.Caminos.Count == 1)
            .ToList();


        cuartos.Remove(aux);
        List<Cuarto> Elegidos = cuartos
            .OrderBy(x => Random.value)
            .Take(CantEventos)
            .ToList();

        int i = 0;
        foreach (Cuarto cuarto in Elegidos)
        {
            cuarto.isBattle = false;
            if (i < CantEventos)
            {
                Instantiate(Event, cuarto.Center);
                i++;
            }
            else break;
        }
    }

    Vector3 PadrePadre(Transform t)
    {
        return t.parent.parent.parent.transform.position;
    }
    void UnirListas(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            proximosCuartos.Enqueue(item);
        }
    }
    bool BuscarArea(Vector3 position, float radio)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radio);
        return colliders.Any(x => x.CompareTag("Sala"));
    }
}
