using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool Activate = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && !Activate)
        {
            if (other.CompareTag("Player") || other.CompareTag("PlayerBody"))
            {
                dungeonManager.instance.NuevoNivel();
                Activate = true;
            }
        }
    }
}
