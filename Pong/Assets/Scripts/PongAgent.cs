using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PongAgent {
    private Dictionary<string, float[]> qTable = new Dictionary<string, float[]>();
    private string filePath = "C:/Users/Adri/JuegoPong/Pong/qtable.csv";

    public PongAgent() {
        LoadQTable();
    }

    private void LoadQTable() {
        if (!File.Exists(filePath)) return;
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines) {
            string[] parts = line.Split(',');
            if (parts.Length < 4) continue;

            string state = parts[0];
            float[] qValues = new float[3];
            qValues[0] = float.Parse(parts[1]);
            qValues[1] = float.Parse(parts[2]);
            qValues[2] = float.Parse(parts[3]);

            qTable[state] = qValues;
        }
    }

    public void SaveQTable() {
        List<string> lines = new List<string>();
        foreach (var entry in qTable) {
            string state = entry.Key;
            float[] qValues = entry.Value;
            lines.Add($"{state},{qValues[0]},{qValues[1]},{qValues[2]}");
        }

        File.WriteAllLines(filePath, lines);
    }

    public float[] GetQValues(string state) {
        if (!qTable.ContainsKey(state))
            qTable[state] = new float[3]; // Inicializa en 0

        return qTable[state];
    }

    public void UpdateQValue(string state, int action, float value) {
        qTable[state][action] = value;
    }
}