using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public enum STATE { IDLE, ATTACK, DEFEND, PURSUE, STEAL, SHOOT };
    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE currentState;
    protected EVENT stage;
    protected Player npc;
    protected AI aiController;
    protected GameObject ball;
    protected BallController ballController;
    protected State nextState;
    protected State previousState;

    float distanceToPursue = 15.0f;
    float distanceToStopPursue = 10.0f;
    float distanceToShoot = 10.0f;
    float angleToShoot = 45.0f;
    float distanceToSteal = 2.0f;
    float angleToSteal = 45.0f;
    Manager manager = new Manager();

    public State(Player _npc, GameObject _ball, BallController _ballController, AI _aiController)
    {
        npc = _npc;
        ball = _ball;
        ballController = _ballController;
        aiController = _aiController;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool ShouldPursueTheBall()
    {
        if (ballController.PlayersPursuingTheBall.Count < 2)
        {
            float distanceToBall = Vector3.Distance(npc.transform.position, ball.transform.position);
            if (distanceToBall < distanceToPursue)
            {
                return true;
            }
        }
        return false;
    }

    public bool ShouldStopPursuingTheBall()
    {
        float distanceToBall = Vector3.Distance(npc.transform.position, ball.transform.position);
        List<Player> teammates = npc.PlayerTeam.GetPlayersByDistanceToBall();
        //Debug.Log("Distance to Ball position:" + teammates.IndexOf(npc));
        if (distanceToBall > distanceToStopPursue && ballController.AttachedToPlayer && ballController.PlayersPursuingTheBall.Count > 1)
        {
            return true;
        }
        return false;
    }

    public bool ShouldReturnToInitialPosition()
    {
        float distanceToInitialPosition = Vector3.Distance(npc.transform.position, aiController.initialPosition);
        if (distanceToInitialPosition > 1.0f)
        {
            return true;
        }
        return false;
    }

    public bool MyTeamHasTheBall()
    {
        if (ballController.AttachedToPlayer)
        {
            Player playerWithTheBall = ballController.PlayerAttachedTo.GetComponent<Player>();
            if (playerWithTheBall.PlayerTeam == npc.PlayerTeam)
            {
                return true;
            }
        }
        return false;
    }
}
