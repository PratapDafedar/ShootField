using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[RequireComponent(typeof(PlayerMoveController))]
//[RequireComponent(typeof(Health))]
public class SoldierController : NetworkBehaviour 
{
	[SyncVar]
	private int currentHealth;
	
	[SyncVar]
	public bool isDead;
	
	[SerializeField]
	public Player player;
	
	[SerializeField]
	private int maxHealth = 100;

	void Start ()
	{
		if (!isLocalPlayer) 
		{
			DisableComponents ();
		} 
		else 
		{
			SetupLocalPlayer ();
		}
	}

	void DisableComponents ()
	{

	}

	void SetupLocalPlayer ()
	{

	}

	void OnLocalPlayerDead ()
	{

	}

	private void Die()
	{
		isDead = true;

		//Switch cameras
		if (isLocalPlayer)
		{			
			OnLocalPlayerDead ();
		}

		Debug.Log(transform.name + " is DEAD!");
	}

	public void SetDefaults ()
	{
		isDead = false;
		currentHealth = maxHealth;
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		GameManager.RegisterPlayer(_netID, this);
	}

	// When we are destroyed
//	void OnDisable ()
//	{
//		GameManager.UnRegisterPlayer(transform.name);
//	}
}