using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;

    [Header("Ground Raycast")]
    public float groundRaycastDist;
    public LayerMask groundLayerMask;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;

    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    private Rigidbody2D charaRB;
    private Animator charaAnim;
    private CharacterSoundController sound;

    private bool isJumping;
    private bool isOnGround;
    private float lastPositionX;

    private void Start()
    {
        charaRB = GetComponent<Rigidbody2D>();
        charaAnim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();

        lastPositionX = transform.position.x;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDist, groundLayerMask);
        if(hit)
        {
            if (!isOnGround && charaRB.velocity.y <= 0) isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        Vector2 velocityVector = charaRB.velocity;
        if(isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        charaRB.velocity = velocityVector;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                isJumping = true;
                sound.PlayJump();
            }
        }
        charaAnim.SetBool("isOnGround", isOnGround);
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        score.FinishScoring();
        gameCamera.enabled = false;
        gameOverScreen.SetActive(true);
        this.enabled = false;
    }


    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDist), Color.white);
    }
}
