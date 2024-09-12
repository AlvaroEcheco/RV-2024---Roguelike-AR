using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Estadisticas")]
    public float damage;
    public float knockback;
    [Header("Referencias")]
    public Collider weaponCollider;

    private void Start()
    {
        weaponCollider.enabled = false;
    }

    public IEnumerator Swing(float swingDuration)
    {
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(swingDuration);
        weaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponCollider.enabled)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(transform.forward * knockback, ForceMode.Impulse);
                }
            }
        }
    }
    
}
