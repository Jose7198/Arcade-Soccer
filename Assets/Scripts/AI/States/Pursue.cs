using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : State
{
    float speed = 3.0f;

    public Pursue(Player npc, GameObject ball, BallController ballController, AI aiController) : base(npc, ball, ballController, aiController)
    {
        currentState = STATE.PURSUE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (ShouldStopPursuingTheBall())
        {
            nextState = new Idle(npc, ball, ballController, aiController);
            ballController.PlayersPursuingTheBall.Remove(npc.gameObject);
            stage = EVENT.EXIT;
        }
        else if (MyTeamHasTheBall())
        {
            nextState = new Attack(npc, ball, ballController, aiController);
            stage = EVENT.EXIT;
        }
        else
        {
            npc.MovePlayerToPosition(ball.transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
