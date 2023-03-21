using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Team
{
    private List<Player> players;
    private Transform goal;
    public List<Player> Players { get { return players; } set { players = value; } }
    public Transform Goal { get { return goal; } set { goal = value; } }
    public string name;

    public Team(List<Player> _players, Transform _goal, string _name)
    {
        Players = _players;
        Goal = _goal;
        name = _name;
        Players.ForEach(player => player.PlayerTeam = this);
    }

    public List<Player> GetPlayersByDistanceToBall()
    {
        return players.OrderBy(p => p.distanceToBall).ToList(); ;
    }

    public List<Player> GetPlayersByDistanceToPosition()
    {
        return players.OrderBy(p => p.distanceToPosition).ToList(); ;
    }
}
