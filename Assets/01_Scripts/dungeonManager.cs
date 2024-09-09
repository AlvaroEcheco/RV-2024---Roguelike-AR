using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dungeonManager : MonoBehaviour
{
    public int Nivel = 1;
    public string escena = "Prueba Ar";

    public Dungeon dungeonPoint;

    public Player playerPrefab;
    public Player player;
    public Sword InitialSword;

    public static dungeonManager instance;
    private bool nuevo = true;

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
        PlaceObject placeObject = FindObjectOfType<PlaceObject>();
        placeObject.gameObject.SetActive(true);
    }

    public void NuevoNivel()
    {
        Debug.Log("Nuevo nivel");
        Nivel++;

        SceneManager.LoadScene(escena);
        PlaceObject placeObject = FindObjectOfType<PlaceObject>();
        placeObject.gameObject.SetActive(true);

    }
    public void Restart()
    {
        Debug.Log("restart");
        Nivel = 0;
        nuevo = true;

        SceneManager.LoadScene(escena);
        PlaceObject placeObject = FindObjectOfType<PlaceObject>();
        placeObject.gameObject.SetActive(true);
    }

    public void GenerarPlayer()
    {
        Vector3 pos = GetComponent<Dungeon>().position;

        if (nuevo)
        {
            player = Instantiate(playerPrefab, pos + (Vector3.up * 5), Quaternion.identity);
            DontDestroyOnLoad(player);

            Sword s = Instantiate(InitialSword, pos + new Vector3(0.035f, 0.65f, 2.2f), new Quaternion(-0.75f, -0.036f, 0.64f, -0.11f));
            s.transform.localScale = new Vector3(0.35f, 0.11f, 1f);
            nuevo = false;
        }
        else
        {
            player.transform.position = pos + (Vector3.up * 5);
        }
    }
}
