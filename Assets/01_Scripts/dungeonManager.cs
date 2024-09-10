using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dungeonManager : MonoBehaviour
{
    public int Nivel = 1;
    public string escena = "TestsGeneracionNivel";

    public Dungeon dungeonPoint;

    public Player playerPrefab;
    public Player player;
    public Sword InitialSword;

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
        dungeonPoint = FindObjectOfType<Dungeon>();
        dungeonPoint.GenerarSalas();

        GenerarPlayer();
    }

    public void NuevoNivel()
    {
        Debug.Log("Nuevo nivel");
        Nivel++;

        SceneManager.LoadScene(escena);

        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        player.transform.position = new Vector3(0, 5, 0);

    }
    public void Restart()
    {
        Debug.Log("restart");
        Nivel = 1;

        Destroy(player);
        SceneManager.LoadScene(escena);

        dungeonPoint = FindObjectOfType<Dungeon>();
        dungeonPoint.GenerarSalas();

        GenerarPlayer();
    }

    private void GenerarPlayer()
    {
        if (player == null)
        {
			player = Instantiate(playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
			DontDestroyOnLoad(player);
		}
    }
    public void GenerarEspada()
    {
		Sword s = Instantiate(InitialSword, new Vector3(0.035f, 0.65f, 2.2f), new Quaternion(-0.75f, -0.036f, 0.64f, -0.11f));
		s.transform.localScale = new Vector3(0.35f, 0.11f, 1f);
	}
}
