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

    }

    public void NuevoNivel()
    {
        Debug.Log("Nuevo nivel");
        Nivel++;

        SceneManager.LoadScene(escena);
    }
    public void Restart()
    {
        Debug.Log("restart");
        Nivel = 1;

        Destroy(player.gameObject);
        SceneManager.LoadScene(escena);
    }

    public void GenerarPlayer()
    {
        if (player == null)
        {
			player = Instantiate(playerPrefab, Vector3.up * 3, Quaternion.identity);
			DontDestroyOnLoad(player);
		}
        else
        {
            player.transform.position = Vector3.up * 3;

        }
    }
    public void GenerarEspada()
    {
        Vector3 pos = new Vector3(0.035f, 0.65f, 2.2f) / 4;
        pos.y -= 0.6f;

        Sword s = Instantiate(InitialSword, pos, new Quaternion(-0.75f, -0.036f, 0.64f, -0.11f));
		s.transform.localScale = new Vector3(0.35f, 0.11f, 1f) / 5;
	}
}
