using UnityEngine;
using System.Collections;

public class Player 
{
	public string name;

	public enum Team
	{
		Blue,
		Red,
	}
	public Team team;

	public int id;

	public Player (string name, Team team)
	{
		this.name = name;
		this.team = team;
	}
}
