using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFlask : MonoBehaviour
{
    [Header("Referencias")]
    public Rigidbody rb;
    public ParticleSystem particleEffect;
    [Header("Estadisticas")]
    public float damage;
    public float timeToDestroy;
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            //Instantiate(particleEffect);
            Destroy(gameObject);
        }
    }

}
