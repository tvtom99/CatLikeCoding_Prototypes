using UnityEngine;
using UnityEngine.Rendering;

public class Ball : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float
        maxXSpeed = 20f,
        maxStartXSpeed = 8f,    //The initial speed of the ball when the game starts
        constantYSpeed = 10f,
        extents = 0.5f; //This value tracks how wide the ball is from its center point

    [SerializeField]
    ParticleSystem bounceParticleSystem;

    [SerializeField]
    int bounceParticleEmission = 20;

    //Tracks the balls position and velocity in [x,z] 3D space, as the y of the Vector2 is used to position in z.
    Vector2 position, velocity;

    /*Getters*/

    public float Extents => extents;

    public Vector2 Position => position;

    public Vector2 Velocity => velocity;

    /*Other*/

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void UpdateVisualisation() => transform.localPosition = new Vector3(position.x, 0f, position.y); //Use position vector to move the ball to its new position

    public void Move() => position += velocity * Time.deltaTime;    //Apply velocity to position vector

    public void StartNewGame()
    {
        position = Vector2.zero;
        UpdateVisualisation();
        velocity.x = Random.Range(-maxStartXSpeed, maxStartXSpeed);
        velocity.y = -constantYSpeed;
        gameObject.SetActive(true);
    }

    public void EndGame()
    {
        position.x = 0f;
        gameObject.SetActive(false);
    }

    public void BounceX(float boundary) //Reflect on the X axis
    {
        float durationAfterBounce = (position.x - boundary) / velocity.x;
        position.x = 2f * boundary - position.x;
        velocity.x = -velocity.x;
        EmitBounceParticles(
            boundary, 
            position.y - velocity.y * durationAfterBounce,
            boundary < 0f ? 90f : 270f
            );
    }

    public void BounceY(float boundary) //Reflect on the 'y' axis (actually z in 3D space)
    {
        float durationAfterBounce = (position.y - boundary) / velocity.y;

        position.y = 2f * boundary - position.y;
        Debug.Log("y" + position.y);
        velocity.y = -velocity.y;

        EmitBounceParticles(
            position.x - velocity.x * durationAfterBounce,
            boundary,
            boundary < 0f ? 0f : 180f
        );
        Debug.Log("emitted y at: " + (position.x - velocity.x * durationAfterBounce) + " " + boundary);
    }

    public void SetXPositionAndSpeed(float start, float speedFactor, float deltaTime)
    {
        velocity.x = maxXSpeed * speedFactor;
        position.x = start + velocity.x * deltaTime;
    }

    void EmitBounceParticles(float x, float z, float rotation)
    {
        ParticleSystem.ShapeModule shape = bounceParticleSystem.shape;
        shape.position = new Vector3(x, 0f, z);
        shape.rotation = new Vector3(0f, rotation, 0f);
        bounceParticleSystem.Emit(bounceParticleEmission);
    }
}