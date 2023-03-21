using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public float distanceToSteal = 5.0f;
    private GameObject ball;
    private Player playerController;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        playerController = transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float shootingInput = Input.GetAxis("Fire1");
        float stealingInput = Input.GetAxis("Fire2");
        Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, verticalInput);
        if (movementDirection != Vector3.zero)
        {
            transform.position += movementDirection.normalized * movementSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        }
        BallController ballController = this.ball.GetComponent<BallController>();
        GameObject playerWithTheBall = ballController.PlayerAttachedTo?.gameObject;
        if (shootingInput != 0 && ballController.AttachedToPlayer && playerWithTheBall == this.gameObject)
        {
            playerController.ShootTheBall(movementDirection + new Vector3(0, 0.45f, 0));
        }
        if (stealingInput != 0 && playerWithTheBall != this.gameObject)
        {
            Vector3 directionToBall = ball.transform.position - transform.position;
            float distanceToBall = directionToBall.magnitude;
            if (distanceToBall <= distanceToSteal)
            {
                playerWithTheBall.GetComponent<EnemyController>().isStunned = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerController.PassTheBall();
        }
    }

    public void SetBall(GameObject ball)
    {
        this.ball = ball;
    }
}
