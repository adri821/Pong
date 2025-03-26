using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public int ballSpeed = 1;

    public CPUController aiAgent;

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
            aiAgent.GiveReward(+1f);
            Vector2 direction = new Vector2(CoordAleatoria(), CoordAleatoria());
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(direction * ballSpeed, ForceMode2D.Force);
        }
        if (transform.position.x > palaDerecha.transform.position.x + offset) {
            transform.position = Vector2.zero;
            playerScore++;
            playerText.text = playerScore.ToString();
            aiAgent.GiveReward(-1f);
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("CPU")) {
            aiAgent.GiveReward(+0.5f); // Recompensa positiva
        }
    }
}
