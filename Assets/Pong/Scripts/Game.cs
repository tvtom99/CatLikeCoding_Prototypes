using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle topPaddle, bottomPaddle;

    [SerializeField, Min(0f)]
    Vector2 arenaExtents = new Vector2(10f, 20f);   //Size of the arena (it is not set by the physical bound objects!)

    [SerializeField]
    int pointsToWin = 3;

    [SerializeField]
    TextMeshPro countdownText;

    [SerializeField, Min(1f)]
    float newGameDelay = 3f;

    [SerializeField]
    LiveleyCamera liveleyCamera;

    float countdownUntilNextGame;

    private void Awake()
    {
        countdownUntilNextGame = newGameDelay;
        StartNewGame();
    }

    void StartNewGame()
    {
        ball.StartNewGame();
        bottomPaddle.StartNewGame();
        topPaddle.StartNewGame();
    }

    private void Update()
    {
        bottomPaddle.Move(ball.Position.x, arenaExtents.x);
        topPaddle.Move(ball.Position.x, arenaExtents.x);
        
        //Check if the game should start yet
        if(countdownUntilNextGame <= 0f)
        {
            UpdateGame();
        }
        else
        {
            UpdateCountdown();
        }
    }

    void UpdateGame()
    {
        ball.Move();
        BounceXIfNeeded(ball.Position.x);
        BounceYIfNeeded();
        ball.UpdateVisualisation();
    }

    void UpdateCountdown()
    {
        countdownUntilNextGame -= Time.deltaTime;
        if(countdownUntilNextGame <= 0f)
        {
            countdownText.gameObject.SetActive(false);
            StartNewGame();
        }
        else
        {
            float displayValue = Mathf.Ceil(countdownUntilNextGame);
            if (displayValue < newGameDelay)
            {
                countdownText.SetText("{0}", displayValue);
            }
        }
    }

    void EndGame()
    {
        countdownUntilNextGame = newGameDelay;
        countdownText.SetText("GAME OVER");
        countdownText.gameObject.SetActive(true);
        ball.EndGame();
    }

    void BounceYIfNeeded()
    {
        float yExtents = arenaExtents.y - ball.Extents;
        if(ball.Position.y > yExtents)
        {
            BounceY(yExtents, topPaddle, bottomPaddle);
        }
        else if(ball.Position.y < -yExtents) 
        {
            BounceY(-yExtents, bottomPaddle, topPaddle);
        }
    }

    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x - ball.Extents;
        if (x > xExtents)
        {
            liveleyCamera.PushXZ(ball.Velocity);
            ball.BounceX(xExtents);
        }
        else if (x < -xExtents)
        {
            liveleyCamera.PushXZ(ball.Velocity);
            ball.BounceX(-xExtents);
        }
    }

    void BounceY(float boundary, Paddle defender, Paddle attacker)
    {
        float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
        float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

        BounceXIfNeeded(bounceX);
        bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
        liveleyCamera.PushXZ(ball.Velocity);
        ball.BounceY(boundary);

        if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
        {
            ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
        else 
        {
            liveleyCamera.JostleY();
            if (attacker.ScorePoint(pointsToWin))
            {
                EndGame();
            }
        }
    }
}
