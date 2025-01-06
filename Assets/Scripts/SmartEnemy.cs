using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField] public Node currentNode;
    [SerializeField] Node targetNode;
    [SerializeField] Node playerNode;
    [SerializeField] [Range(0, 5)] float initialSpeed;
    [SerializeField] float speed;
    public float chargeSpeed;
    public Vector3 direction;
    public bool isCharging = false;
    public float nodeJumpApproximation;

    public List<Node> pathInstructions;

    private Pathfinding pathfinder;
    private Animator animator;
    public SpriteRenderer sR;

    public float speedMultiplier;
    public float speedIncreaseDuration;
    private float initialSpeedAtStart;
    void Start()
    {
        pathfinder = GameObject.FindGameObjectWithTag("Pathfinder").GetComponent<Pathfinding>();
        animator = GetComponent<Animator>();
        sR = GetComponent<SpriteRenderer>();

        pathInstructions = pathfinder.FindPath(currentNode);
        playerNode = pathfinder.FindPlayer();

        speed = initialSpeed;
        initialSpeedAtStart = initialSpeed; // Store the initial speed
    }

    void Update()
    {
        if (targetNode == null && pathInstructions.Count >= 1)
        {
            targetNode = pathInstructions[0];
            pathInstructions.RemoveAt(0);
        }
        if (targetNode == null && pathInstructions.Count == 0)
        {
            pathInstructions = pathfinder.FindPath(currentNode);
        }

        if (targetNode != null)
        {
            direction = new Vector3(targetNode.transform.position.x, targetNode.transform.position.y, 0) - transform.position;
            transform.position += direction.normalized * speed * Time.deltaTime;
        }

        if (direction.magnitude <= nodeJumpApproximation && targetNode != null)
        {
            transform.position = targetNode.transform.position;
            currentNode = targetNode;
            targetNode = null;

            Node checkPlayerNode = pathfinder.FindPlayer();
            if (checkPlayerNode != playerNode)
            {
                playerNode = checkPlayerNode;
                pathInstructions = pathfinder.FindPath(currentNode);
            }
            isCharging = false;
            speed = initialSpeedAtStart; // Reset the speed to the initial speed at the start of the next movement
        }

        if (currentNode == playerNode && !isCharging)
        {
            ChargeAtPlayer();
        }

        // Add this block to progressively increase the speed
        if (speed <= initialSpeedAtStart * speedMultiplier)
        {
            speed += Time.deltaTime / speedIncreaseDuration;
        }

        SetAnim();
    }

    private void ChargeAtPlayer()
    {
        Debug.Log("charging now");
        isCharging = true;
        bool foundPlayer = false;

        foreach (Node neighbourNode in currentNode.neighbourNodes)
        {
            int layerMask = LayerMask.GetMask("Nodes");
            Vector3 direction = neighbourNode.transform.position - currentNode.transform.position;

            RaycastHit2D ray = Physics2D.Raycast(currentNode.transform.position, direction, direction.magnitude, ~layerMask);
            //Debug.DrawRay(currentNode.transform.position, neighbourNode.transform.position - currentNode.transform.position, Color.red, 0.1f);

            if (ray.collider != null && ray.collider.CompareTag("Player"))
            {
                targetNode = neighbourNode;
                foundPlayer = true;
            }
        }
        if (!foundPlayer)
        {
            Debug.Log("couldn't find player");
            targetNode = currentNode.neighbourNodes[Random.Range(0, currentNode.neighbourNodes.Count)];
        }
        speed *= chargeSpeed;
    }

    private void SetAnim()
    {
        if (direction.x > 0 && direction.x > Mathf.Abs(direction.y))
        {
            animator.SetTrigger("idleSide");
            sR.flipX = false;
        }
        else if (direction.x < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetTrigger("idleSide");
            sR.flipX = true;
        }
        else if (direction.y > 0 && direction.y > Mathf.Abs(direction.x))
        {
            animator.SetTrigger("idleBack");
        }
        else if (direction.y < 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            animator.SetTrigger("idleForward");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            player.UpdateHealth(-1);
        }

        //if (col.gameObject.CompareTag("IceBlock"))
        //{
        //    IceBlock iceBlock = col.gameObject.GetComponent<IceBlock>();
        //    iceBlock.Smash();
        //}
    }
}
