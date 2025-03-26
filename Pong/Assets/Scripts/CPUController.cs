using UnityEngine;

public class CPUController : MonoBehaviour {
    private PongAgent qLearning;
    private Rigidbody2D rb;
    public float learningRate = 0.1f;
    public float discountFactor = 0.9f;
    public float explorationRate = 0.2f;
    public float speed = 5f;

    void Start() {
        qLearning = new PongAgent();
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 15f;
    }

    void Update() {
        string state = GetState();
        float[] qValues = qLearning.GetQValues(state);

        int action = ChooseAction(qValues);
        ApplyAction(action);

        // Guardar la tabla peri�dicamente
        if (Random.Range(0f, 1f) < 0.01f) qLearning.SaveQTable();
    }

    private string GetState() {
        Vector2 ballPos = GameObject.FindGameObjectWithTag("Ball").transform.position;
        Vector2 ballVelocity = GetComponent<Rigidbody2D>().linearVelocity;
        float paddleY = transform.position.y;

        return $"{Mathf.Round(ballPos.x)},{Mathf.Round(ballPos.y)},{Mathf.Round(ballVelocity.x)},{Mathf.Round(ballVelocity.y)},{Mathf.Round(paddleY)}";
    }

    private int ChooseAction(float[] qValues) {
        if (Random.Range(0f, 1f) < explorationRate) {
            return Random.Range(0, 3); // Acci�n aleatoria
        }

        return System.Array.IndexOf(qValues, Mathf.Max(qValues));
    }

    private void ApplyAction(int action) {
        if (action == 0) rb.linearVelocity = Vector2.up * speed;
        else if (action == 1) rb.linearVelocity = Vector2.down * speed;
        else rb.linearVelocity = Vector2.zero;
    }

    public void GiveReward(float reward) {
        string state = GetState();
        float[] qValues = qLearning.GetQValues(state);
        int action = ChooseAction(qValues);

        float oldQ = qValues[action];
        float maxFutureQ = Mathf.Max(qValues);
        float newQ = oldQ + learningRate * (reward + discountFactor * maxFutureQ - oldQ);

        qLearning.UpdateQValue(state, action, newQ);
    }

    void OnApplicationQuit() {
        qLearning.SaveQTable(); // Guardar al salir
    }
}