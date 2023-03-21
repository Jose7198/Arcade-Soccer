using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defend : State
{
    public Defend(Player npc, GameObject ball, BallController ballController, AI aiController) : base(npc, ball, ballController, aiController)
    {
        currentState = STATE.DEFEND;
    }

    public override void Enter()
    {
        npc.CurrentPosition = Player.FIELD_POSITION.DEFENSE;
        base.Enter();
    }

    public override void Update()
    {
        if (MyTeamHasTheBall())
        {
            nextState = new Attack(npc, ball, ballController, aiController);
            stage = EVENT.EXIT;
        }
        if (Vector3.Distance(npc.transform.position, npc.DefensePosition) > 1)
        {
            npc.MovePlayerToPosition(npc.DefensePosition);
        }
        else
        {
            nextState = new Idle(npc, ball, ballController, aiController);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
