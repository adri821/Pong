using UnityEngine;

public class PowerUp : MonoBehaviour {
    // Enum para los tipos de power-up
    public enum PowerUpType {
        IncreaseBallSpeed,
        IncreasePaddleSize,
        DecreasePaddleSize
    }

    public PowerUpType powerUpType;  // Tipo de power-up, configurable desde el Inspector
    public float speedModifier = 1.5f; // Factor de modificación de la velocidad
    public float sizeModifier = 1.5f;
    public float duration = 5;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            BallController ballController = collision.gameObject.GetComponent<BallController>();
            if (ballController != null) {
                ApplyPowerUp(ballController);
            }
            Destroy(gameObject);
        }
    }

    void ApplyPowerUp(BallController ballController) {
        switch (powerUpType) {
            case PowerUpType.IncreaseBallSpeed:
                ballController.ModifySpeed(speedModifier, duration);
                break;
            case PowerUpType.IncreasePaddleSize:
                ballController.ModifyPaddleSize(ballController.palaIzquierda, sizeModifier);
                ballController.ModifyPaddleSize(ballController.palaDerecha, sizeModifier);
                break;
            case PowerUpType.DecreasePaddleSize:
                ballController.ModifyPaddleSize(ballController.palaIzquierda, 1 / sizeModifier);
                ballController.ModifyPaddleSize(ballController.palaDerecha, 1 / sizeModifier);
                break;
        }
    }
}

