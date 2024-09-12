using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [Header("Referencias")]
    public float rotationSpeed = 5f;
    public float detectionRadius = 10f; 
    public LayerMask enemyLayer;
    public PlayerMovement playerMovement;

    private Transform closestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        if(!playerMovement.isRolling)
        {
            if(closestEnemy != null)
            {
                RotateTowardsEnemy();
            }
            else
            {
                NormalRotation();
            }
        }
    }

    void FindClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        float closestDistance = Mathf.Infinity;
        closestEnemy = null;

        foreach (Collider enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;

                
            }
        }
    }

    void RotateTowardsEnemy()
    {
        Vector3 directionToEnemy = (closestEnemy.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToEnemy.x, 0, directionToEnemy.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void NormalRotation()
    {
        transform.localRotation = Quaternion.identity;
    }
}
