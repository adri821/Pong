using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Globalization;

public class QLearningCPUController : MonoBehaviour
{
    public Transform ballPosition;
    private Rigidbody2D rb;
    private Rigidbody2D ballRb;
    public int cpuSpeed = 5;

    public float learningRate = 0.1f;
    public float discountFactor = 0.9f;
    private float explorationRate = 1.0f;
    private float minExplorationRate = 0.01f;
    private float decayRate = 0.995f;
    private float forgetRate = 0.0001f;

    private Dictionary<string, float[]> QTable;
    private const int numActions = 3;

    private int saveCounter = 0;
    private const int saveInterval = 500;

    private List<(string, int, float, string)> memory = new List<(string, int, float, string)>();
    private int memorySize = 1000;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ballRb = ballPosition.GetComponent<Rigidbody2D>();
        QTable = new Dictionary<string, float[]>();
        LoadQTable();
        Time.timeScale = 15f;
    }

    void FixedUpdate() {
        string currentState = GetState();
        int action = EpsilonGreedyPolicy(currentState);
        PerformAction(action);
        float reward = CalculateReward();
        string nextState = GetState();

        UpdateQTable(currentState, action, reward, nextState);
        memory.Add((currentState, action, reward, nextState));
        if (memory.Count > memorySize) memory.RemoveAt(0);

        if (memory.Count > 10) {
            for (int i = 0; i < 10; i++) {
                var experience = memory[Random.Range(0, memory.Count)];
                UpdateQTable(experience.Item1, experience.Item2, experience.Item3, experience.Item4);
            }
        }

        explorationRate = Mathf.Max(minExplorationRate, explorationRate * decayRate);
        ApplyForgetRate();

        saveCounter++;
        if (saveCounter >= saveInterval) {
            SaveQTable();
            saveCounter = 0;
        }
    }

    string GetState() {
        int positionDifference = Mathf.Clamp(Mathf.FloorToInt(ballPosition.position.y - transform.position.y), -10, 10);
        int ballVelocityY = Mathf.Clamp(Mathf.FloorToInt(ballRb.linearVelocity.y), -5, 5);
        int ballVelocityX = Mathf.Clamp(Mathf.FloorToInt(ballRb.linearVelocity.x), -5, 5);
        int paddleVelocityY = Mathf.Clamp(Mathf.FloorToInt(rb.linearVelocity.y), -5, 5);
        int ballDirection = ballVelocityY > 0 ? 1 : -1;

        return $"{positionDifference}_{ballVelocityY}_{ballVelocityX}_{paddleVelocityY}_{ballDirection}";
    }

    int EpsilonGreedyPolicy(string state) {
        if (Random.value < explorationRate || !QTable.ContainsKey(state)) {
            return Random.Range(0, numActions);
        }
        else {
            return MaxIndex(QTable[state]);
        }
    }

    void PerformAction(int action) {
        if (action == 0) {
            rb.linearVelocity = Vector2.up * cpuSpeed;
        }
        else if (action == 2) {
            rb.linearVelocity = Vector2.down * cpuSpeed;
        }
        else {
            rb.linearVelocity = Vector2.zero;
        }
    }

    float CalculateReward() {
        float distance = Mathf.Abs(ballPosition.position.y - transform.position.y);
        if (distance < 0.5f) return 1.5f;
        if (distance < 1.5f) return 0.5f;
        if (ballPosition.position.x > transform.position.x) return -2.0f;
        return -0.05f;
    }

    void UpdateQTable(string state, int action, float reward, string nextState) {
        if (!QTable.ContainsKey(state)) {
            QTable[state] = new float[] { 0.1f, 0.1f, 0.1f };
        }

        float[] actions = QTable[state];
        float maxNextQ = QTable.ContainsKey(nextState) ? Mathf.Max(QTable[nextState]) : 0f;
        actions[action] = actions[action] * (1 - forgetRate) + learningRate * (reward + discountFactor * maxNextQ - actions[action]);
    }

    void ApplyForgetRate() {
        foreach (var state in QTable.Keys.ToList()) {
            QTable[state] = QTable[state].Select(q => q * (1 - forgetRate)).ToArray();
        }
    }

    void LoadQTable() {
        string path = Path.Combine(Application.streamingAssetsPath, "q_table.csv");
        if (File.Exists(path)) {
            foreach (var line in File.ReadAllLines(path)) {
                string[] parts = line.Split(';');
                if (parts.Length > 1) {
                    string state = parts[0];
                    QTable[state] = parts.Skip(1).Select(p => float.Parse(p, CultureInfo.InvariantCulture)).ToArray();
                }
            }
            Debug.Log("Tabla Q cargada correctamente.");
        }
        else {
            Debug.Log("No se encontró la tabla Q. Se creará una nueva.");
        }
    }

    void SaveQTable() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "q_table.csv");
        File.WriteAllLines(filePath, QTable.Select(e => e.Key + ";" + string.Join(";", e.Value.Select(v => v.ToString(CultureInfo.InvariantCulture)))));
        Debug.Log("Tabla Q guardada en: " + filePath);
    }

    int MaxIndex(float[] actions) {
        return actions.ToList().IndexOf(actions.Max());
    }
}
