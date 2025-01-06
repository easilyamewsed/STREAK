using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbEnemy : MonoBehaviour
{
    [SerializeField] public Node node;
    [SerializeField] public Node previousNode;

    private Animator animator;
    public SpriteRenderer sR;

    private bool travellingToNewNode = false;
    private Vector3 direction;
    [SerializeField][Range(0, 10)] float speedMax;
    public float speed;
    public float initialSpeed;
    public float speedMultiplier;
    public float speedIncreaseDuration;
    [SerializeField][Range(0, 0.1f)] float nodeJumpApproximation;

    PlayerMovement player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        speed = Random.Range(1, speedMax);
        initialSpeed = speed;
        animator = GetComponent<Animator>();
        sR = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if (!travellingToNewNode)
        {
            previousNode = node;
            node = node.neighbourNodes[Random.Range(0, node.neighbourNodes.Count)];
            travellingToNewNode = true;
        }
        else
        {
            direction = new Vector3(node.transform.position.x, node.transform.position.y, 0) - transform.position;
            transform.position += direction.normalized * speed * Time.deltaTime;

            if (direction.magnitude <= nodeJumpApproximation)
            {
                transform.position = node.transform.position;
                travellingToNewNode = false;
            }

            if(direction.x > 0 && direction.x > Mathf.Abs(direction.y))
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

        if (speed <= initialSpeed * speedMultiplier)
        {
            speed += Time.deltaTime / speedIncreaseDuration;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Floaty") || collision.gameObject.CompareTag("IceBlock"))
        {
            
            Node placeHolderNode;

            placeHolderNode = previousNode;
            previousNode = node;
            node = placeHolderNode;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            player.UpdateHealth(-1);
        }

    }
}