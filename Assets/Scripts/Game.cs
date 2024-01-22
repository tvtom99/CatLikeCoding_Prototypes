using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle topPaddle, bottomPaddle;

    private void Awake()
    {
        ball.StartNewGame();
    }

    private void Update()
    {
        ball.Move();
        ball.UpdateVisualisation();
    }
}
