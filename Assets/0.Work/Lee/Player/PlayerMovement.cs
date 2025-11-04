using System.Collections;
using UnityEngine;

// Rigidbody 컴포넌트가 필수로 존재하도록 보장합니다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool isDashing { get; private set; }
  
    public float CurrentSpeed { get; private set; }

    public Vector3 DashDirection { get; private set; }

  
    [SerializeField] private float dashDuration = 0.2f;

    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float drag = 10; // 저항 값 (관성 제어)              

    private Vector3 moveDirection;
    private Rigidbody rb ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = drag; // Rigidbody의 drag 값 설정    
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Rigidbody의 움직임을 부드럽게 보간합니다.
        rb.freezeRotation = true;
    }

    public void SetMoveDirection( Vector3 dir )
    {
        // 어떤 크기의 벡터가 들어와도 일관된 방향을 유지하기 위해 항상 정규화(normalized)합니다.
        moveDirection = dir.normalized;
    }

    public void Move( float maxSpeed, float moveForce  )
    {
        // 대시 중에는 일반 이동이 불가능하도록 막습니다.
        if (isDashing) return;

        rb.AddForce(moveDirection * moveForce, ForceMode.Force);

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if(horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeed;                                           
              rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);   
        }
    }

    public void Dash( Vector3 direction, float speed )
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            StartCoroutine(DashCoroutine(direction.normalized, speed));
        }
    }

    public void Rotate(Vector3 direction, bool isInstant = false)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        if (isInstant) // 즉시 회전이 필요할 경우 (예: 대시)
        {
            transform.rotation = targetRotation;
        }
        else // 일반 이동 시에는 부드럽게 회전
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private IEnumerator DashCoroutine( Vector3 dashDir, float dashSpeed )
    {
        isDashing = true;
        DashDirection = dashDir; 
        float startTime = Time.time;

        while ( Time.time < startTime + dashDuration )
        {
            Rotate(dashDir, true);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector3.zero;
        isDashing = false;
        DashDirection = Vector3.zero; 
    }
}
