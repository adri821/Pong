using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array de prefabs de power-ups
    public float spawnInterval = 10f; // Intervalo de generaci�n en segundos
    public Vector2 spawnRangeX; // Rango de posici�n X en el campo
    public Vector2 spawnRangeY; // Rango de posici�n Y en el campo

    private void Start() {
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);
    }

    void SpawnPowerUp() {
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnRangeX.x, spawnRangeX.y),
            Random.Range(spawnRangeY.x, spawnRangeY.y)
        );

        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }
}
