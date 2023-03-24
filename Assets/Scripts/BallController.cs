using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    private bool _attachedToPlayer = false;
    private bool _shootMode = false;
    private Player _playerAttachedTo;
    private bool isGrounded = true;
    private List<GameObject> _playersPursuingTheBall = new List<GameObject>();
    public Player PlayerAttachedTo { get { return _playerAttachedTo; } set { _playerAttachedTo = value; } }
    public bool AttachedToPlayer { get { return _attachedToPlayer; } set { _attachedToPlayer = value; } }
    public bool ShootMode { set { _shootMode = value; } }
    public List<GameObject> PlayersPursuingTheBall { get { return _playersPursuingTheBall; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_attachedToPlayer)
        {
            float playerRotationAngle = _playerAttachedTo.transform.eulerAngles.y * Mathf.Deg2Rad;
            Vector3 playerRotation = new Vector3(Mathf.Sin(playerRotationAngle), 0, Mathf.Cos(playerRotationAngle));
            // Debug.Log(playerRotation);
            Vector3 newBallPosition = _playerAttachedTo.transform.position + playerRotation;
            transform.position = newBallPosition + offset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Teammate" || other.tag == "Enemy")
        {
            Player playerController = other.GetComponent<Player>();
            if (_shootMode)
            {
                // Knock the player
                //playerController
                EnemyController enemyController = other.GetComponent<EnemyController>();
                enemyController.isStunned = true;
            }
            else
            {
                _attachedToPlayer = true;
                _playerAttachedTo = playerController;
                playerController.Ball = this;
            }

        }
    }

    public IEnumerator SetShootModeToFalse()
    {
        yield return new WaitForSeconds(2);
        ShootMode = false;
    }

}
