using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    float speed = 3.0f;
    public Idle(Player npc, GameObject ball, BallController ballController, AI aiController) : base(npc, ball, ballController, aiController)
    {
        currentState = STATE.IDLE;
    }

    public override void Enter()
    {
        npc.CurrentPosition = Player.FIELD_POSITION.INITIAL;
        base.Enter();
    }

    public override void Update()
    {
        if (ShouldPursueTheBall())
        {
            nextState = new Pursue(npc, ball, ballController, aiController);
            ballController.PlayersPursuingTheBall.Add(npc.gameObject);
            stage = EVENT.EXIT;
        }
        else if (!MyTeamHasTheBall())
        {
            nextState = new Defend(npc, ball, ballController, aiController);
            stage = EVENT.EXIT;
        }
        else if (MyTeamHasTheBall())
        {
            nextState = new Attack(npc, ball, ballController, aiController);
            stage = EVENT.EXIT;
        }
        else if (ShouldReturnToInitialPosition())
        {
            Vector3 direction = aiController.initialPosition - npc.transform.position;
            npc.transform.position += (direction.normalized * Time.deltaTime * speed);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
