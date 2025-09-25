using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isDashing{ get; private set; }
    float dashDuration = 0.2f;
    private void Start()
    {
        isDashing = false;
    }
    public void Move( Vector3 direction, float speed )
    {
        if ( direction.sqrMagnitude > 0.01f )
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
    public void Dash( Vector3 direction, float speed )
    {
        StartCoroutine(DashCoroutine(direction, speed));
    }
    private IEnumerator DashCoroutine( Vector3 moveInput, float dashSpeed )
    {
        isDashing = true;
        float startTime = Time.time;

        Vector3 dashDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        while ( Time.time < startTime + dashDuration )
        {
            transform.Translate(dashDir * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        isDashing = false;
    }
}
