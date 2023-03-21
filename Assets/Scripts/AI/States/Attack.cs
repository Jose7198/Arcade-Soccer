using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public Attack(Player npc, GameObject ball, BallController ballController, AI aiController) : base(npc, ball, ballController, aiController)
    {
        currentState = STATE.ATTACK;
    }

    public override void Enter()
    {
        npc.CurrentPosition = Player.FIELD_POSITION.ATTACK;
        base.Enter();
    }

    public override void Update()
    {
        if (MyTeamHasTheBall())
        {
            if (ballController.PlayerAttachedTo == npc)
            {
                npc.MovePlayerToPosition(Manager.Singleton.PlayerTeam.Goal.transform.position);
            }
            else if (Vector3.Distance(npc.transform.position, npc.AttackPosition) > 1)
            {
                npc.MovePlayerToPosition(npc.AttackPosition);
            }
            else
            {
                nextState = new Idle(npc, ball, ballController, aiController);
                stage = EVENT.EXIT;
            }
        }
        else
        {
            nextState = new Defend(npc, ball, ballController, aiController);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
