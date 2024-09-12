using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> drops;

    // al hacerle click o algo hace algo
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                int random = Random.Range(0, drops.Count);
                GameObject obj = Instantiate(drops[random], transform.position, Quaternion.identity);
                obj.transform.localScale = drops[random].transform.localScale;
                Destroy(gameObject);
            }
        }
    }
}
