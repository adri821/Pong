using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Velocidad de la rotación
    [SerializeField] float rotationSpeed = 15f;

    // Rotación objetivo
    private Quaternion targetRotation = Quaternion.Euler(0, 0, 180);

    // Variable para controlar si el objeto está rotando
    private bool rotando = false;


    public int speed = 1;

    void Update()
    {
        /*
        GetComponent<Rigidbody2D>().linearVelocityY = Input.GetAxisRaw("Vertical") * speed;

        // Detectamos la pulsación de la tecla
        if (Input.GetButtonDown("Fire1")) {
            rotando = true;
            GetComponent<Rigidbody2D>().freezeRotation = false; // Descongelamos las rotaciones
        }

        // Si se ha pulsado la tecla aplicamos la rotación suave desde el estado de rotación actual hasta el deseado
        if (rotando) transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Si el objeto se aproxima a la posición final, se recoloca para que quede bien ajustado
        if (transform.rotation.eulerAngles.z < 185) {
            rotando = false;
            transform.rotation = Quaternion.Euler(0, 0, 0); // Recolocamos la rotación
            GetComponent<Rigidbody2D>().freezeRotation = true; // Volvemos a congelar las rotaciones
        }
        */
        // Desactivar movimiento mientras rota
        if (!rotando) {
            // Solo permitir movimiento cuando no está rotando
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, Input.GetAxisRaw("Vertical") * speed);
        }
        else {
            // Congelar el movimiento mientras rota
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        // Detectar cuando se pulsa la tecla para iniciar la rotación
        if (Input.GetButtonDown("Fire1")) {
            rotando = true;
            GetComponent<Rigidbody2D>().freezeRotation = false; // Permitir rotación
        }

        // Aplicar rotación suave mientras rotando esté activo
        if (rotando) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Si se ha alcanzado la rotación objetivo, detener la rotación
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f) {
                rotando = false;
                transform.rotation = Quaternion.Euler(0, 0, 0); // Ajustar a la rotación final
                GetComponent<Rigidbody2D>().freezeRotation = true; // Volver a congelar rotación
            }
        }
    }
}
