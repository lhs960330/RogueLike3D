using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    public bool IsDashing { get; private set; }
    public Vector3 MoveDirection { get; private set; }

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
