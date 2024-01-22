using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float
        constantXSpeed = 8f,    //The initial speed of the ball when the game starts
        constantYSpeed = 10f;

    //Tracks the balls position and velocity in [x,z] 3D space, as the y of the Vector2 is used to position in z.
    Vector2 position, velocity;

    public void UpdateVisualisation() => transform.localPosition = new Vector3(position.x, 0f, position.y); //Use position vector to move the ball to its new position

    public void Move() => position += velocity * Time.deltaTime;    //Apply velocity to position vector

    public void StartNewGame()
    {
        position = Vector2.zero;
        UpdateVisualisation();
        velocity = new Vector2(constantXSpeed, -constantYSpeed);
    }
}