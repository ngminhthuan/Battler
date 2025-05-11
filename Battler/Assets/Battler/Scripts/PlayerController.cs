using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;

    [Header("Movement Settings")]
    [SerializeField] float acceleration = 5f;
    [SerializeField] float deceleration = 5f;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float rotationSpeed = 10f;
    Vector2 InputVector = Vector2.zero;
    float velocity = 0f;
    static readonly int VelocityHash = Animator.StringToHash("Velocity");

    ComboAttack comboAttack;

    void Start()
    {
        comboAttack = new ComboAttack(playerAnimator);
    }

    void Update()
    {
        HandleMovementInput();
        HandleRotation();

        if (Input.GetMouseButtonDown(0))
        {
            comboAttack.OnAttackInput();
        }

        comboAttack.ComboUpdate();
    }

    void HandleMovementInput()
    {
        // Get input from both horizontal and vertical axes
        InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMoving = InputVector.magnitude >= 0.1f; // Check if there's any movement input
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = runPressed ? runSpeed : walkSpeed;

        if (isMoving)
        {
            // Accelerate towards the target speed
            if (velocity < targetSpeed)
            {
                velocity += Time.deltaTime * acceleration;
                velocity = Mathf.Min(velocity, targetSpeed);
            }
            else if (velocity > targetSpeed)
            {
                velocity -= Time.deltaTime * deceleration;
                velocity = Mathf.Max(velocity, targetSpeed);
            }
        }
        else
        {
            // Decelerate to zero when no input
            if (velocity > 0f)
            {
                velocity -= Time.deltaTime * deceleration;
                velocity = Mathf.Max(velocity, 0f);
            }
            else if (velocity < 0f)
            {
                velocity += Time.deltaTime * deceleration;
                velocity = Mathf.Min(velocity, 0f);
            }
        }

        playerAnimator.SetFloat(VelocityHash, velocity); // Use the actual velocity for animation
    }
    void HandleRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}