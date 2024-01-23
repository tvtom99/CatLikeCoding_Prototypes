using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float
        extents = 4f,
        speed = 10f;

    [SerializeField]
    bool isAI;

    public void Move(float target, float arenaExtents)
    {
        Vector3 p = transform.localPosition;
        p.x = isAI ? AdjustByAI(p.x, target) : AdjustByPlayer(p.x);
        float limit = arenaExtents - extents;
        p.x = Mathf.Clamp(p.x, -limit, limit);
        transform.localPosition = p;
    }

    float AdjustByAI(float x, float target)
    {
        if (x < target)
        {
            return Mathf.Min(x + speed * Time.deltaTime, target);   //Honeslty not sure how this works specifically, perhaps rewrite your own implementation?
        }
        return Mathf.Max(x - speed * Time.deltaTime, target);       //I DO NOT like multiple return statements!
    }   

    float AdjustByPlayer(float x)
    {
        bool goRight = Input.GetKey(KeyCode.RightArrow);
        bool goLeft = Input.GetKey(KeyCode.LeftArrow);
        if(goRight && !goLeft)
        {
            return x + speed * Time.deltaTime;  //Multiple returns? Again?
        }
        else if (!goRight && goLeft)
        {
            return x - speed * Time.deltaTime;
        }
        return x;   //Surely just hold this float variable somewhere and return it at the end once? I guess it's marginally faster to instant return but i dont like it
    }

    public bool HitBall(float ballX, float ballExtents, out float hitFactor)
    {
        hitFactor =
            (ballX - transform.localPosition.x) /
            (extents + ballExtents);
        return -1f <= hitFactor && hitFactor <= 1f;
    }
}
