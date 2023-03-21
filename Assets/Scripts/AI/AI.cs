using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject ball;
    State currentState;
    public Vector3 initialPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Manager manager = Manager.Singleton;
        BallController ballController = ball.GetComponent<BallController>();
        Player playerScript = gameObject.GetComponent<Player>();
        currentState = new Idle(playerScript, ball, ballController, this);
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();
    }
}
