using System.Collections;
using UnityEngine;

// [추가] Rigidbody 컴포넌트가 필수로 존재하도록 보장합니다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool isDashing { get; private set; }
    // [추가] 현재 속도를 외부(PlayerAnimator)에서 읽을 수 있도록 프로퍼티를 추가했습니다.
    public float CurrentSpeed { get; private set; }
    // [추가] 현재 대시 방향을 외부(PlayerController)에서 읽을 수 있도록 프로퍼티를 추가합니다.
    public Vector3 DashDirection { get; private set; }

    // [추가] 대시 지속시간을 이 스크립트에서 직접 관리합니다.
    [SerializeField] private float dashDuration = 0.2f;
    // [추가] 캐릭터의 회전 속도를 Inspector에서 조절할 수 있도록 변수를 추가했습니다.
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Rigidbody의 움직임을 부드럽게 보간합니다.
        // [추가] 물리 현상(충돌 등)으로 인해 캐릭터가 원치 않게 회전하는 것을 방지합니다.
        rb.freezeRotation = true;
    }

    public void SetMoveDirection( Vector3 dir )
    {
        // [수정] 어떤 크기의 벡터가 들어와도 일관된 방향을 유지하기 위해 항상 정규화(normalized)합니다.
        moveDirection = dir.normalized;
    }

    public void Move( float moveSpeed )
    {
        // [추가] 대시 중에는 일반 이동이 불가능하도록 막습니다.
        if (isDashing) return;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Vector3 targetPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
            CurrentSpeed = moveSpeed; // [추가] 현재 이동 속도를 기록합니다.
        }
        else
        {
            CurrentSpeed = 0f; // [추가] 멈췄을 때는 속도를 0으로 설정합니다.
        }
    }

    public void Dash( Vector3 direction, float speed )
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            StartCoroutine(DashCoroutine(direction.normalized, speed));
        }
    }

    // [수정] PlayerController에서 호출할 수 있도록 접근 제어자를 public으로 변경합니다.
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
        DashDirection = dashDir; // [추가] 대시 방향을 기록합니다.
        float startTime = Time.time;

        while ( Time.time < startTime + dashDuration )
        {
            Rotate(dashDir, true);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector3.zero;
        isDashing = false;
        DashDirection = Vector3.zero; // [추가] 대시가 끝나면 방향을 초기화합니다.
    }
}
