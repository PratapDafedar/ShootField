using UnityEngine;
using System.Collections;

public class User {

    public int      id;
    public string name;

    public enum Team
    {
        RED = 0,
        BLUE,
    };
    public Team cTeam;

    public User()
    {

    }

    public User (int _id, string _name, Team _team)
    {
        this.id = _id;
        this.name = _name;
        this.cTeam = _team;
    }
}
