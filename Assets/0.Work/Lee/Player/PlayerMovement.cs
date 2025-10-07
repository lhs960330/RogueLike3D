using System.Collections;
using UnityEngine;

// [추가] Rigidbody 컴포넌트가 필수로 존재하도록 보장합니다.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool isDashing { get; private set; }
    // [추가] 현재 속도를 외부(PlayerAnimator)에서 읽을 수 있도록 프로퍼티를 추가했습니다.
    public float CurrentSpeed { get; private set; }

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
        // [추가] 대시 중에는 일반 이동 및 회전이 불가능하도록 막습니다.
        if (isDashing) return;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Vector3 targetPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
            CurrentSpeed = moveSpeed; // [추가] 현재 이동 속도를 기록합니다.
            Rotate(moveDirection);    // [추가] 이동 방향으로 캐릭터를 회전시킵니다.
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
            // [수정] 대시 방향으로 캐릭터를 즉시 회전시킵니다.
            Rotate(direction.normalized, true);
        }
    }

    // [추가] 캐릭터를 특정 방향으로 회전시키는 함수입니다.
    private void Rotate(Vector3 direction, bool isInstant = false)
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
        float startTime = Time.time;

        // [수정] 대시 이동을 transform.Translate 대신 Rigidbody의 속도(velocity)로 처리합니다.
        // 이렇게 하면 물리 시스템 내에서 움직이므로 벽 충돌 시 뚫고 지나가는 버그 등을 방지할 수 있습니다.
        while ( Time.time < startTime + dashDuration )
        {
            rb.velocity = dashDir * dashSpeed;
            // [수정] 물리 업데이트는 FixedUpdate 주기에 맞춰 기다려야 합니다.
            yield return new WaitForFixedUpdate();
        }

        // [추가] 대시가 끝나면 속도를 0으로 만들어 즉시 멈추게 합니다.
        rb.velocity = Vector3.zero;
        isDashing = false;
    }
}