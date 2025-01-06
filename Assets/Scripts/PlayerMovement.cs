using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSource gameMusic;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip floatySound;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip collectedPU;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator animator;
    
    public Floaty targetFloaty;
    private FloatySpawning spawner;
    private PlayerHealth healthUI;

    //[SerializeField][Range(0, 10)] int playerSpeed;
    [SerializeField][Range(0, 10)] float jumpDistance;
    [SerializeField][Range(0, 10)] float jumpApexHeight;
    [SerializeField][Range(0, 10)] float jumpApexLength;
    [SerializeField][Range(0, 10)] float movementSpeed;

    [SerializeField] Transform jumpApexMarker;
    [SerializeField] GameObject jumpMarker;
    [SerializeField] GameObject sealSpriteCenter;
    private Vector3 spriteOffset;

    public bool isJumping = false;
    private bool controllEnabled = true;
    public bool jumpEnabled = true;

    private bool jumpingCoroutineActive = false;
    public bool powerUpCoroutineActice = false;

    private float jumpingParameter = 0f;
    [SerializeField][Range(0, 2)] float jumpDuration;

    Vector3 _jumpStart;
    Vector3 _jumpApex;
    Vector3 _jumpEnd;

    public SpriteRenderer jumpMarkerSpriteRenderer;
    public SpriteRenderer sR;

    public Vector3 movement;
    public Vector3 playerDirection;
    public int playerHealth = 3;

    private bool isLosingHealth = false;
    public bool isSpeedy = false;

    List<GameObject> playerSpawnPoints;
    List<GameObject> validSpawnPoints;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameActiveScreen;
    // --
    [SerializeField] private ProgressBar progressBar;
    public SpriteRenderer arrowSpriteRenderer;
    [SerializeField] private float arrowDisappearTime = 2f;

    void Start()
    {
        spriteOffset = sealSpriteCenter.transform.position - transform.position;

        gameMusic = GameObject.FindGameObjectWithTag("Game Music").GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        jumpMarkerSpriteRenderer = jumpMarker.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        sR = GetComponent<SpriteRenderer>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<FloatySpawning>();
        healthUI = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<PlayerHealth>();
        

        MakePlayerSpawnPointsList();
        transform.position = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)].transform.position;

        StartCoroutine(HandleArrowVisibility());
    }

    void Update()
    {
        if (controllEnabled)
        {
            movement = Vector3.zero;
            //movement.x = Input.GetAxis("Horizontal");
            //movement.y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                movement.x = -movementSpeed;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                movement.x = movementSpeed;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                movement.y = movementSpeed;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                movement.y = -movementSpeed;
            }


            //if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) { movement.y = 0; }
            //else { movement.x = 0; }

            movement = movement.normalized;
            rb.velocity = movement * movementSpeed;

            //if (movement == Vector3.zero) { rb.velocity = new Vector3(0,0,0); }
            //movement = new Vector3(0f, 0f, 0f);  //this vector holds the player movement at any time
            //movement.x = Input.GetAxisRaw("Horizontal"); //gets the horizontal and verticle inputs
            //movement.y = Input.GetAxisRaw("Vertical");
            //movement = movement.normalized;                          //makes sure the x and y axis speed doesn't sum
            //transform.position += movement * playerSpeed * Time.deltaTime; //regulates framerate and incorporates playerSpeed 


            if (movement.x > 0)
            {
                jumpMarker.transform.position = transform.position + new Vector3(jumpDistance, 0, 0);
                jumpMarker.transform.rotation = Quaternion.identity;

                jumpApexMarker.position = transform.position + new Vector3(jumpApexLength, jumpApexHeight, 0);
                playerDirection = Vector3.right;

                animator.SetTrigger("facingHor");
                sR.flipX = false;
            }
            else if (movement.x < 0)
            {
                jumpMarker.transform.position = transform.position + new Vector3(-jumpDistance, 0, 0);
                jumpMarker.transform.rotation = Quaternion.identity;

                jumpApexMarker.position = transform.position + new Vector3(-jumpApexLength, jumpApexHeight, 0);
                playerDirection = Vector3.left;

                animator.SetTrigger("facingHor");
                sR.flipX = true;
            }
            else if (movement.y > 0)
            {
                jumpMarker.transform.position = transform.position + new Vector3(spriteOffset.y, 1f, 0); //magic number here
                jumpMarker.transform.rotation = Quaternion.Euler(0, 0, 90);

                jumpApexMarker.position = transform.position + new Vector3(0, jumpApexLength, 0);
                playerDirection = Vector3.up;
                animator.SetTrigger("facingBackward");
            }
            else if (movement.y < 0)
            {
                jumpMarker.transform.position = transform.position + new Vector3(spriteOffset.y, -0.9f, 0); //and here
                jumpMarker.transform.rotation = Quaternion.Euler(0, 0, 90);

                jumpApexMarker.position = transform.position + new Vector3(0, -jumpApexLength, 0);
                playerDirection = Vector3.down;
                animator.SetTrigger("facingForward");
            }
            if (Input.GetKeyDown(KeyCode.Space) && controllEnabled && jumpEnabled)
            {
                controllEnabled = false;
                isJumping = true;
                jumpingParameter = 0;
                audioSource.PlayOneShot(jumpSound);
            }
            if (Input.GetKeyDown(KeyCode.Space) && controllEnabled && !jumpEnabled)
            {
                if (playerDirection == Vector3.down)
                {
                    animator.SetTrigger("frontJumpDeclined");
                }
                else if (playerDirection == Vector3.up)
                {
                    animator.SetTrigger("backJumpDeclined");
                }
                else
                {
                    animator.SetTrigger("sideJumpDeclined");
                }

            }

        }

        if (isJumping)
        {
            if (jumpingParameter == 0)
            {
                if (jumpingCoroutineActive == false)
                {
                    jumpingCoroutineActive = true;
                    StartCoroutine(PlayingJumpingAnimationOnce());
                }

                _jumpStart = transform.position;
                _jumpApex = jumpApexMarker.position;

                if (targetFloaty == null)
                {
                    if (playerDirection == Vector3.left || playerDirection == Vector3.right)
                    {
                        _jumpEnd = jumpMarker.transform.position;
                    }
                    else
                    {
                        Bounds bounds = jumpMarkerSpriteRenderer.bounds;
                        _jumpEnd = bounds.center;
                    }

                }
                else
                {
                    _jumpEnd = targetFloaty.transform.position;
                    audioSource.PlayOneShot(floatySound);
                }
            }

            jumpingParameter += Time.deltaTime / jumpDuration;
            transform.position = CalculateBezierPoint(jumpingParameter, _jumpStart, _jumpApex, _jumpEnd);

            if (jumpingParameter >= 1)
            {
                isJumping = false;
                controllEnabled = true;

                if (targetFloaty != null)
                {
                    targetFloaty.Destroy();
                }
            }
        }
        if (powerUpCoroutineActice)
        {
            StartCoroutine(SpeedPowerUp());
        }
    }

    private void OnValidate()
    {
        jumpMarker.transform.position = new Vector3(transform.position.x + jumpDistance, transform.position.y, 0);
        jumpApexMarker.position = new Vector3(transform.position.x + jumpApexLength, transform.position.y + jumpApexHeight, 0);
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            //controllEnabled = false;
        }
    }

    private IEnumerator PlayingJumpingAnimationOnce()
    {
        if (playerDirection == Vector3.right || playerDirection == Vector3.left)
        {
            AnimationClip clip = GetAnimationClipByName("HorJumping");
            animator.speed = clip.length / jumpDuration;

            if (playerDirection == Vector3.left)
            {
                sR.flipX = true;
            }

            animator.SetBool("isHorJumping", true);
            yield return new WaitForSeconds(jumpDuration);
            animator.SetBool("isHorJumping", false);
        }
        else if (playerDirection == Vector3.up)
        {
            AnimationClip clip = GetAnimationClipByName("BackJumping");
            animator.speed = clip.length / jumpDuration;
            animator.SetBool("isBackJumping", true);
            yield return new WaitForSeconds(jumpDuration);
            animator.SetBool("isBackJumping", false);
        }
        else if (playerDirection == Vector3.down)
        {
            AnimationClip clip = GetAnimationClipByName("FacingJumping");
            animator.speed = clip.length / jumpDuration;
            animator.SetBool("isFrontJumping", true);
            yield return new WaitForSeconds(jumpDuration);
            animator.SetBool("isFrontJumping", false);
        }

        animator.speed = 1f;
        jumpingCoroutineActive = false;
    }

    public AnimationClip GetAnimationClipByName(string clipName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }

    public void UpdateHealth(int modifier)
    {
        if (!isLosingHealth)
        {
            playerHealth += modifier;
            if (modifier > 0)
            {
                audioSource.PlayOneShot(collectedPU);
            }
            if (playerHealth > 4)
            {
                playerHealth = 4; 
            }

            Debug.Log("Player Health: " + playerHealth);
            healthUI.SetHealthIcons(playerHealth);  

            if (playerHealth <= 0)
            {
                PlayerDeath();
            }
            else if (modifier <= 0)
            {
                audioSource.PlayOneShot(damageSound);
                StartCoroutine(PlayerHealthDown());
            }

            //list<gameobject> playerspawnpoints = new list<gameobject>(gameobject.findgameobjectswithtag("node"));
            //list<gameobject> validspawnpoints = new list<gameobject>();
            //foreach (var node in playerspawnpoints)
            //{
            //    node nodescript = node.getcomponent<node>();
            //    if (nodescript.playerspawn)
            //    {
            //        validspawnpoints.add(node);
            //    }
            //}
            //transform.position = validspawnpoints[random.range(0, validspawnpoints.count)].transform.position;
        }

    }

    private void PlayerDeath()
    {
        audioSource.PlayOneShot(gameOverSound);
        boxCollider.enabled = false;
        controllEnabled = false;
        StopAllCoroutines();
        rb.velocity = Vector3.zero;
        animator.SetTrigger("isDead");
        gameActiveScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        //AnimationClip deathClip = GetAnimationClipByName("Bouy");
        progressBar.OnPlayerDeath();
        arrowSpriteRenderer.enabled = false;
    }

    private IEnumerator HandleArrowVisibility()
    {
        arrowSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(arrowDisappearTime);
        arrowSpriteRenderer.enabled = false;
    }

    private IEnumerator PlayerHealthDown()
    {
        isLosingHealth = true;
        controllEnabled = false;

        rb.velocity = Vector3.zero;


        AnimationClip healthDownClip = GetAnimationClipByName("Seal Hit");
        animator.SetTrigger("takeDamage");
        yield return new WaitForSeconds(healthDownClip.length);
        controllEnabled = true;
        arrowDisappearTime = 4f;
        arrowSpriteRenderer.enabled = true;
        StartCoroutine(HandleArrowVisibility());

        transform.position = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)].transform.position;

        isLosingHealth = false;
    }
    private void MakePlayerSpawnPointsList()
    {
        playerSpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Node"));
        validSpawnPoints = new List<GameObject>();
        foreach (var node in playerSpawnPoints)
        {
            Node nodeScript = node.GetComponent<Node>();
            if (nodeScript.playerSpawn)
            {
                validSpawnPoints.Add(node);
            }
        }
        Debug.Log("list made");
    }
    public IEnumerator SpeedPowerUp()
    {
        isSpeedy = true;
        gameMusic.pitch = 2f;

        audioSource.PlayOneShot(collectedPU);
        powerUpCoroutineActice = false;
        float initialSpeed = movementSpeed;
        float initialJumptDuration = jumpDuration;
        movementSpeed *= 2f;
        jumpDuration *= 0.5f;
        
        yield return new WaitForSeconds(5);

        gameMusic.pitch = 1f;
        movementSpeed = initialSpeed;
        jumpDuration = initialJumptDuration;
        Debug.Log("Speed Reset");
        isSpeedy = false;
    }
    public void Kill()
    {
        //gameover screen
        animator.enabled = false;
        sR.enabled = false;
    }
    
}
