using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dungeonManager : MonoBehaviour
{
    public int Nivel = 3;
    private int CantCuartos = 0;
    public int CantEventos = 1;
    public List<Cuarto> Cuartos;
    public GameObject dungeonPoint;
    public Portal Portal;
    public GameObject Event;

    public Cuarto Cu;

    private Queue<GameObject> proximosCuartos = new Queue<GameObject>();

    public string NombreScene = "TestsGeneracionNivel";

    public static dungeonManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Generar();
    }

    void Generar()
    {

        Cuarto aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 4), dungeonPoint.transform.position, Quaternion.identity);
        aux.isBattle = false;
        UnirListas(aux.Caminos);
        CantCuartos++;
        aux.transform.parent = dungeonPoint.transform;

        while (proximosCuartos.Count > 0)
        {
            GameObject spawnPoint = proximosCuartos.Dequeue();
            if (!BuscarArea(spawnPoint.transform.position, Vector3.one * 3))
            {
                Vector3 p = new Vector3(spawnPoint.transform.position.x, 0, spawnPoint.transform.position.z);
                if (CantCuartos <= Nivel)
                {
                    CantCuartos++;
                    int a = Random.Range(0, Cuartos.Count);
                    aux = Instantiate(Cuartos[a], p, Quaternion.identity);
                    aux.transform.LookAt(PadrePadre(spawnPoint.transform));// MIRAR AL SPAWNPOINT PARA ALINEAR LA PUERTA
                    UnirListas(aux.Caminos);
                }
                else
                {
                    aux = Instantiate(Cuartos.First(x => x.Caminos.Count == 1), p, Quaternion.identity);
                    aux.transform.LookAt(PadrePadre(spawnPoint.transform));// MIRAR AL SPAWNPOINT PARA ALINEAR LA PUERTA
                }
                aux.transform.parent = dungeonPoint.transform;
            }
        }

        //genera el portal en el ultimo cuarto creado
        Instantiate(Portal, aux.Center);
        aux.isBattle = false;

        //genera los cuartos especiales
        List<Cuarto> cuartos = dungeonPoint.transform.GetComponentsInChildren<Cuarto>()
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


        //tp al player a 0 0 
        FindAnyObjectByType<Player>().transform.position = new Vector3(0, 2, 0);
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

    public void NuevoNivel()
    {
        CantCuartos = 0;
        proximosCuartos.Clear();
        Nivel++; 
        SceneManager.LoadScene(NombreScene);
        foreach (Transform child in dungeonPoint.transform)
        {
            Destroy(child.gameObject);
        }
        Generar();

    }
    public void Restart()
    {
        CantCuartos = 0;
        proximosCuartos.Clear();
        Nivel = 1; 
        SceneManager.LoadScene(NombreScene);
        foreach (Transform child in dungeonPoint.transform)
        {
            Destroy(child.gameObject);
        }
        Generar();
    }
    bool BuscarArea(Vector3 position, Vector3 size)
    {
        Collider[] colliders = Physics.OverlapBox(position, size / 2);
        if (colliders.Length > 0)
        {
            // hay objetos
            return true;
        }
        else
        {
            // no hay objetos
            return false;
        }
    }

    public void RevisarEnemigos()
    {
        if (Cu != null)
        {
            if (Cu.Contenido.transform.childCount <= 1)
            {
                Cu.Completar();
                Instantiate(Event,Cu.transform);
            }
        }
    }
}
