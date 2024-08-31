using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    public float Speed = 8f;
    public float LifeTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}
