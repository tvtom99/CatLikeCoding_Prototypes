using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    Ball ball;

    [SerializeField]
    Paddle topPaddle, bottomPaddle;

    [SerializeField, Min(0f)]
    Vector2 arenaExtents = new Vector2(10f, 10f);   //Size of the arena (it is not set by the physical bound objects!)

    private void Awake()
    {
        ball.StartNewGame();
    }

    private void Update()
    {
        bottomPaddle.Move(ball.Position.x, arenaExtents.x);
        topPaddle.Move(ball.Position.x, arenaExtents.x);
        ball.Move();
        BounceXIfNeeded(ball.Position.x);
        BounceYIfNeeded();
        ball.UpdateVisualisation();
    }

    void BounceYIfNeeded()
    {
        float yExtents = arenaExtents.y - ball.Extents;
        if(ball.Position.y > yExtents)
        {
            BounceY(yExtents, topPaddle);
        }
        else if(ball.Position.y < -yExtents) 
        {
            BounceY(-yExtents, bottomPaddle);
        }
    }

    void BounceXIfNeeded(float x)
    {
        float xExtents = arenaExtents.x - ball.Extents;
        if (x > xExtents)
        {
            ball.BounceX(xExtents);
        }
        else if (x < -xExtents)
        {
            ball.BounceX(-xExtents);
        }
    }

    void BounceY(float boundary, Paddle defender)
    {
        float durationAfterBounce = (ball.Position.y - boundary) / ball.Velocity.y;
        float bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;

        BounceXIfNeeded(bounceX);
        bounceX = ball.Position.x - ball.Velocity.x * durationAfterBounce;
        ball.BounceY(boundary);

        if (defender.HitBall(bounceX, ball.Extents, out float hitFactor))
        {
            ball.SetXPositionAndSpeed(bounceX, hitFactor, durationAfterBounce);
        }
    }
}
