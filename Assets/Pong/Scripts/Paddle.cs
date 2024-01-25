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

    [SerializeField]
    MeshRenderer goalRenderer;

    [SerializeField, ColorUsage(true, true)]
    Color goalColour = Color.white;

    int score;

    float extents, targetingBias;

    static readonly int
        emissionsColourId = Shader.PropertyToID("_EmissionColour"),
        faceColourId = Shader.PropertyToID("_FaceColor"),
        timeOfLastHitId = Shader.PropertyToID("_TimeOfLastHit");

    Material paddleMaterial, goalMaterial, scoreMaterial;

    private void Awake()
    {
        goalMaterial = goalRenderer.material;
        goalMaterial.SetColor(emissionsColourId, goalColour);
        paddleMaterial = GetComponent<MeshRenderer>().material;
        scoreMaterial = scoreText.fontMaterial;
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
        bool success =  -1f <= hitFactor && hitFactor <= 1f;
        if (success)
        {
            paddleMaterial.SetFloat(timeOfLastHitId, Time.time);
        }
        return success;
    }

    void SetScore(int newScore, float pointsToWin = 1000f)
    {
        score = newScore;
        scoreText.SetText("{0}", newScore);
        if(scoreMaterial != null)   //Checking this seemed to fix the error and now the game works fine?
        {
            scoreMaterial.SetColor(faceColourId, goalColour * (newScore / pointsToWin));
        }
        else
        {
            Debug.Log("scoreMaterial is not set");
        }
        SetExtents(Mathf.Lerp(maxExtents, minExtents, newScore / (pointsToWin - 1f)));
    }

    public bool ScorePoint(int pointsToWin)
    {
        goalMaterial.SetFloat(timeOfLastHitId, Time.time);
        SetScore(score + 1, pointsToWin);
        return score >= pointsToWin;        //Okay this is awful code, why is the function doing BOTH score incrementation AND checking if the player has enough points to win? This tutorial smh
    }
}
