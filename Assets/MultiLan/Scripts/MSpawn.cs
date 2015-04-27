using UnityEngine;
using System.Collections;


public class MSpawn : MonoBehaviour {
	public GameObject playerPrefab;
	private Transform [] spawners;
	
	public void Start () {
		// Search all spawn points
		spawners = gameObject.GetComponentsInChildren<Transform>();	    
		// Start the player loading
		StartCoroutine(loadPlayer());		  
	}
	
	// Load the player
	private IEnumerator loadPlayer(){
		// Search  a spawn for the player (randomly)
		int rand = Random.Range(0, spawners.Length-1);
	    Transform spawn = spawners[rand];
		
		// Wait one second (the waiting time allows a better synchronisation on the players loading)
		yield return new WaitForSeconds(1);
		
		// Instantiate our player
		Network.Instantiate(playerPrefab, spawn.transform.position, Quaternion.identity, 0);
	}
	
}
