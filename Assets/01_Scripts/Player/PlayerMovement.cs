using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    public Rigidbody rb;
    public Animator animator;
    public GameObject bodyRolling;

    [Header("Estadísticas de Movimiento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Estadísticas de Rodar")]
    public float rollForce = 8f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 1f;

    private Vector3 movementInput;
    public bool isRolling = false;
    private bool canRoll = true;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!isRolling)
        {
            ProcessInputs();
            HandleRotation();
            UpdateAnimations();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttemptRoll();
        }
    }

    private void FixedUpdate()
    {
        if (!isRolling)
        {
            Move();
        }
    }

    /// <summary>
    /// Procesa la entrada del jugador.
    /// </summary>
    private void ProcessInputs()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movementInput = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
    }

    /// <summary>
    /// Maneja el movimiento del jugador utilizando el Rigidbody.
    /// </summary>
    private void Move()
    {
        Vector3 moveVelocity = movementInput * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    /// <summary>
    /// Maneja la rotación suave del jugador hacia la dirección de movimiento.
    /// </summary>
    private void HandleRotation()
    {
        if (movementInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementInput);
            Quaternion smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(smoothRotation);
        }
    }

    /// <summary>
    /// Intenta iniciar la acción de rodar si es posible.
    /// </summary>
    private void AttemptRoll()
    {
        if (canRoll && movementInput != Vector3.zero)
        {
            StartCoroutine(Roll());
        }
    }

    /// <summary>
    /// Corrutina que maneja la acción de rodar.
    /// </summary>
    private IEnumerator Roll()
    {
        canRoll = false;
        isRolling = true;

        // Opcional: Activar animación de rodar
        animator.SetTrigger("Roll");

        rb.velocity = Vector3.zero; // Reiniciar velocidad antes de rodar
        rb.AddForce(movementInput * rollForce, ForceMode.Impulse);
        bodyRolling.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(rollDuration);

        isRolling = false;

        yield return new WaitForSeconds(rollCooldown);

        canRoll = true;
    }

    /// <summary>
    /// Actualiza los parámetros de animación basados en el estado actual.
    /// </summary>
    private void UpdateAnimations()
    {
        bool isMoving = movementInput.magnitude > 0f;
        animator.SetBool("Walking", isMoving);
    }
}
