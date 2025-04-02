//using TMPro;
//using UnityEngine;

//public class BallController : MonoBehaviour
//{
//    public int ballSpeed = 1;

//   // public CPUController aiAgent;

//    public GameObject palaIzquierda;
//    public GameObject palaDerecha;

//    public int offset = 5;

//    public int playerScore = 0;
//    public int cpuScore = 0;

//    public TextMeshProUGUI playerText;
//    public TextMeshProUGUI cpuText;

//    void Start()
//    {
//        Vector2 direction = new Vector2 (CoordAleatoria(), CoordAleatoria());
//        GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
//    }

//    void Update() {
//        if (transform.position.x < palaIzquierda.transform.position.x - offset) {
//            transform.position = Vector2.zero;
//            cpuScore++;
//            cpuText.text = cpuScore.ToString();
//            //aiAgent.GiveReward(+10f);
//            Vector2 direction = new Vector2(CoordAleatoria(), CoordAleatoria());
//            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
//            GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
//        }
//        if (transform.position.x > palaDerecha.transform.position.x + offset) {
//            transform.position = Vector2.zero;
//            playerScore++;
//            playerText.text = playerScore.ToString();
//            //aiAgent.GiveReward(-10f);
//            Vector2 direction = new Vector2(CoordAleatoria(), CoordAleatoria());
//            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
//            GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
//        }
//    }

//    int CoordAleatoria() 
//    {
//        int[] posibilidades = { -1, 1 };
//        int aleatorio = Random.Range(0, posibilidades.Length);
//        return posibilidades[aleatorio];        
//    }

//    void OnCollisionEnter2D(Collision2D collision) {
//        if (collision.gameObject.CompareTag("CPU")) {
//           // aiAgent.GiveReward(+10f); // Recompensa positiva
//        }
//    }
//}
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour {
    public float ballSpeed = 5f;
    public int offset = 5;

    [HideInInspector]
    public int playerScore = 0;
    [HideInInspector]
    public int cpuScore = 0;

    public TextMeshProUGUI PlayerText;
    public TextMeshProUGUI cpuText;

    public GameObject palaIzquierda;
    public GameObject palaDerecha;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ResetBall();
    }

    void Update() {
        if (transform.position.x < palaIzquierda.transform.position.x - offset) {
            cpuScore++;
            cpuText.text = cpuScore.ToString();
            ResetBall();
        }
        else if (transform.position.x > palaDerecha.transform.position.x + offset) {
            playerScore++;
            PlayerText.text = playerScore.ToString();
            ResetBall();
        }

        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed;
    }

    void ResetBall() {
        transform.position = Vector2.zero;

        // Genera un ángulo aleatorio entre -45° y 45° o entre 135° y 225°
        float angle = Random.Range(0, 2) == 0 ? Random.Range(-45f, 45f) : Random.Range(135f, 225f);

        // Convierte el ángulo en un vector de dirección
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        rb.linearVelocity = direction.normalized * ballSpeed;
    }

    int CordAleatoria() {
        return Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed;
    }
}
