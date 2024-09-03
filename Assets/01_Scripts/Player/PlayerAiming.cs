using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LayerMask groundMask; // Asegúrate de que este LayerMask solo incluya el suelo
    private Camera mainCamera;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerMovement.isRolling)
        {
            Aim();
        }
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;
            transform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Solo detecta colisiones con el suelo usando el groundMask
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
}
