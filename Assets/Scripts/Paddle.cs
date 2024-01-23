using TMPro;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float
        minExtents = 4f,
        maxExtents = 4f,
        speed = 10f,
        maxTargetingBias = 0.75f;

    [SerializeField]
    bool isAI;

    [SerializeField]
    TextMeshPro scoreText;

    int score;

    float extents, targetingBias;

    private void Awake()
    {
        SetScore(0);    
    }

    public void StartNewGame()
    {
        SetScore(0);
        ChangeTargetingBias();
    }

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
        target += targetingBias * extents;

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

    void ChangeTargetingBias()
    {
        targetingBias = Random.Range(-maxTargetingBias, maxTargetingBias);
    }

    void SetExtents(float newExtents)
    {
        extents = newExtents;
        Vector3 s = transform.localScale;
        s.x = 2f * newExtents;
        transform.localScale = s;
    }

    public bool HitBall(float ballX, float ballExtents, out float hitFactor)
    {
        ChangeTargetingBias();

        hitFactor =
            (ballX - transform.localPosition.x) /
            (extents + ballExtents);
        return -1f <= hitFactor && hitFactor <= 1f;
    }

    void SetScore(int newScore, float pointsToWin = 1000f)
    {
        score = newScore;
        scoreText.SetText("{0}", newScore);
        SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (pointsToWin - 1f)));
    }

    public bool ScorePoint(int pointsToWin)
    {
        SetScore(score + 1, pointsToWin);
        return score >= pointsToWin;        //Okay this is awful code, why is the function doing BOTH score incrementation AND checking if the player has enough points to win? This tutorial smh
    }
}
