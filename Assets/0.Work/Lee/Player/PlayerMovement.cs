using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    public bool IsDashing { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public bool IsGrounded { get; private set; }

    private float groundCheckDistance = 0.1f;
    [SerializeField]private LayerMask groundMask =(int) Define.Layer.Ground;
    private Rigidbody rb;
    private PlayerStats stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<PlayerStats>();

        rb.linearDamping = stats.drag;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;
    }

    void Update()
    {
        CheckGrounded();

    }
    private void CheckGrounded()
    {
        float checkDistance = stats.height / 2f + groundCheckDistance;
        Vector3 rayStart = transform.position + new Vector3(0,2,0);
        Vector3 rayDirection = Vector3.down;
        if (Physics.Raycast(rayStart, rayDirection, checkDistance, groundMask))
        {
            IsGrounded = true;

            Debug.DrawRay(rayStart, rayDirection * checkDistance, Color.green);
        }
        else
        {
            IsGrounded = false;
            Debug.DrawRay(rayStart, rayDirection * checkDistance, Color.red);
             
        }
    }
    public void SetMoveDirection(Vector3 dir)
    {
        MoveDirection = dir.normalized;
    }

    public void Move()
    {
        if (IsDashing) return;

        rb.AddForce(MoveDirection * stats.moveForce, ForceMode.Force);

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > stats.maxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * stats.maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    public void Dash()
    {
        if (MoveDirection.sqrMagnitude > 0.01f && !IsDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Jump()
    {
        if (!IsGrounded) return;

        rb.AddForce(Vector3.up * stats.jumpForce * 5, ForceMode.Impulse);
    }

    public void Rotate(Vector3 direction, bool isInstant = false)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        if (isInstant)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, stats.rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator DashCoroutine()
    {
        IsDashing = true;
        float startTime = Time.time;
        
        rb.linearVelocity = new Vector3(MoveDirection.x * stats.dashSpeed, rb.linearVelocity.y, MoveDirection.z * stats.dashSpeed);

        while (Time.time < startTime + stats.dashDuration)
        {
            Rotate(MoveDirection, true);
            yield return null;
        }

        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        IsDashing = false;
    }
}
