using UnityEngine;

public class CPUController : MonoBehaviour
{
    public Transform ballPosition;
    public int cpuSpedd = 1;

    private void FixedUpdate() {
        if (ballPosition.position.y > transform.position.y) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * cpuSpedd, ForceMode2D.Force);
        }
        if (ballPosition.position.y < transform.position.y) {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * cpuSpedd, ForceMode2D.Force);
        }
    }
}
