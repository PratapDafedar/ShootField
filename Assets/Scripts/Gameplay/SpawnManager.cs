using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour 
{
	public static SpawnManager Instance;

	public Transform[] spawnPointTeamA;
	public Transform[] spawnPointTeamB;

	void Awake ()
	{
		Instance = this;
	}
}
