using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class Manager
{
    private static Manager instance;
    private Team playerTeam;
    private Team enemyTeam;
    public Team PlayerTeam { get { return playerTeam; } set { playerTeam = value; } }
    public Team EnemyTeam { get { return enemyTeam; } set { enemyTeam = value; } }

    public static Manager Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new Manager();
                List<Player> enemyTeamPlayers = GameObject.FindGameObjectsWithTag("Enemy").ToList().Select(enemy => enemy.GetComponent<Player>()).ToList();
                Transform enemyTeamGoal = GameObject.FindGameObjectWithTag("EnemyGoal").transform;
                instance.EnemyTeam = new Team(enemyTeamPlayers, enemyTeamGoal, "Idolosh");

                List<Player> playerTeamPlayers = GameObject.FindGameObjectsWithTag("Teammate").ToList().Select(teammate => teammate.GetComponent<Player>()).ToList();
                Transform playerTeamGoal = GameObject.FindGameObjectWithTag("TeamGoal").transform;
                instance.PlayerTeam = new Team(playerTeamPlayers, playerTeamGoal, "Bombisho");
            }
            return instance;
        }
    }
}
