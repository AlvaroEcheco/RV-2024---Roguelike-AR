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
                Instantiate(drops[Random.Range(0,drops.Count)], other.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
