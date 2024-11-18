using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public int ballSpeed = 1;

    public GameObject palaIzquierda;
    public GameObject palaDerecha;

    public int offset = 5;

    public int playerScore = 0;
    public int cpuScore = 0;

    public TextMeshProUGUI playerText;
    public TextMeshProUGUI cpuText;

    void Start()
    {
        Vector2 direction = new Vector2 (CoordAleatoria(), CoordAleatoria());
        GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
    }

    void Update() {
        if (transform.position.x < palaIzquierda.transform.position.x - offset) {
            transform.position = Vector2.zero;
            cpuScore++;
            cpuText.text = cpuScore.ToString();
            Vector2 direction = new Vector2(CoordAleatoria(), CoordAleatoria());
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
        }
        if (transform.position.x > palaDerecha.transform.position.x + offset) {
            transform.position = Vector2.zero;
            playerScore++;
            playerText.text = playerScore.ToString();
            Vector2 direction = new Vector2(CoordAleatoria(), CoordAleatoria());
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
        }
    }

    int CoordAleatoria() 
    {
        int[] posibilidades = { -1, 1 };
        int aleatorio = Random.Range(0, posibilidades.Length);
        return posibilidades[aleatorio];        
    }

    public void ModifySpeed(float modifier, float duration) {
        ballSpeed = Mathf.RoundToInt(ballSpeed * modifier); 
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed;

        Invoke(nameof(ResetBallSpeed), duration);
    }

    public void ModifyPaddleSize(GameObject paddle, float modifier, float duration) {
        paddle.transform.localScale = new Vector3(
            paddle.transform.localScale.x,
            paddle.transform.localScale.y * modifier,
            paddle.transform.localScale.z);

        // Invoke(nameof(() => ResetPaddleSize(paddle, modifier)), duration);
        //Invoke(nameof((ResetPaddleSize(paddle, modifier), duration)));
    }

    private void ResetBallSpeed() {
        ballSpeed = 200; // Cambia este valor si quieres que el valor original sea distinto
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = rb.linearVelocity.normalized * ballSpeed;
    }

    private void ResetPaddleSize(GameObject paddle, float modifier) {
        // Restablece el tamaño original dividiendo por el mismo modificador
        paddle.transform.localScale = new Vector3(
            paddle.transform.localScale.x,
            paddle.transform.localScale.y / modifier,
            paddle.transform.localScale.z);

       
    }
}
