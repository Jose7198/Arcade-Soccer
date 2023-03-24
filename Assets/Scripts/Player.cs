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
                Vector3 direction = teammate.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                float angleWithPlayer = Quaternion.Angle(transform.rotation, rotation);
                if (angleWithPlayer < closestAngle)
                {
                    closestAngle = angleWithPlayer;
                    nearestPlayer = teammate;
                }
            }
        });
        float distanceToTeammate = Vector3.Distance(transform.position, nearestPlayer.transform.position) / 2;
        float launchAngle = Mathf.Atan(2 * 5.0f / distanceToTeammate);
        float speed = Mathf.Sqrt((Physics.gravity.y * Mathf.Pow(distanceToTeammate, 2)) / (2 * (5.0f - distanceToTeammate * Mathf.Tan(launchAngle)) * Mathf.Pow(Mathf.Cos(launchAngle), 2)));
        Vector3 directionToTeammate = (nearestPlayer.transform.position - transform.position).normalized;
        Vector3 velocityDirection = new Vector3(directionToTeammate.x * Mathf.Cos(launchAngle), Mathf.Sin(launchAngle), directionToTeammate.z * Mathf.Cos(launchAngle));
        Vector3 velocity = velocityDirection * speed;
        Ball.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
    }

}
