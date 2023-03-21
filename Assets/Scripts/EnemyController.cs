using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    private bool _isStunned;
    public bool isStunned
    {
        get { return _isStunned; }
        set
        {
            if (value)
            {
                Invoke("StopStun", 3);
            }
            _isStunned = value;
        }
    }
    private GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        isStunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = ball.transform.position - transform.position;
        if (!isStunned)
        {
            transform.position += movementDirection.normalized * movementSpeed * Time.deltaTime;
        }
    }

    void StopStun()
    {
        isStunned = false;
    }
}
