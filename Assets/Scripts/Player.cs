using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum FIELD_POSITION { INITIAL, ATTACK, DEFENSE }
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 attackPosition;
    [SerializeField] private Vector3 defensePosition;
    private BallController _ball;
    private FIELD_POSITION currentPosition;
    private Team playerTeam;
    public Vector3 InitialPosition { get { return initialPosition; } set { initialPosition = value; } }
    public Vector3 AttackPosition { get { return attackPosition; } set { attackPosition = value; } }
    public Vector3 DefensePosition { get { return defensePosition; } set { defensePosition = value; } }
    public FIELD_POSITION CurrentPosition { get { return currentPosition; } set { currentPosition = value; } }
    public BallController Ball { get { return _ball; } set { _ball = value; } }
    public Team PlayerTeam { get { return playerTeam; } set { playerTeam = value; } }
    public float movementSpeed = 4.0f;
    public float distanceToBall;
    public float distanceToPosition;
    public float shootForce = 20.0f;
    public float speedDuringPass = 10.0f;
    public int number;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPosition;
        CurrentPosition = FIELD_POSITION.INITIAL;
        Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToBall = Vector3.Distance(transform.position, Ball.transform.position);
        switch (CurrentPosition)
        {
            case FIELD_POSITION.INITIAL:
                {
                    distanceToPosition = Vector3.Distance(transform.position, initialPosition);
                    break;
                }
            case FIELD_POSITION.ATTACK:
                {
                    distanceToPosition = Vector3.Distance(transform.position, attackPosition);
                    break;
                }
            case FIELD_POSITION.DEFENSE:
                {
                    distanceToPosition = Vector3.Distance(transform.position, defensePosition);
                    break;
                }
        }

    }

    public void MovePlayerToPosition(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        transform.position += (direction.normalized * Time.deltaTime * movementSpeed);
        transform.LookAt(direction);
    }

    public void ShootTheBall(Vector3 direction)
    {
        Ball.AttachedToPlayer = false;
        Ball.PlayerAttachedTo = null;
        Ball.ShootMode = true;
        Ball.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);
        StartCoroutine(Ball.SetShootModeToFalse());
    }

    public void PassTheBall()
    {
        Ball.AttachedToPlayer = false;
        Ball.PlayerAttachedTo = null;
        Player nearestPlayer = this;
        float closestAngle = Mathf.Infinity;
        PlayerTeam.Players.ForEach(teammate =>
        {
            if (teammate != this)
            {
                float angleWithPlayer = Mathf.Abs(Vector3.Distance(transform.forward, teammate.transform.position));
                if (angleWithPlayer < closestAngle)
                {
                    closestAngle = angleWithPlayer;
                    nearestPlayer = teammate;
                }
            }
        });
        Debug.Log("Pass to " + nearestPlayer.number);
        Vector3 velocity = (transform.forward + Vector3.up) * speedDuringPass;
        StartCoroutine(Ball.MoveTheBallDuringPass(nearestPlayer.transform.position, velocity, true));
    }

}
