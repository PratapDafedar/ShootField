using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerCellView : MonoBehaviour 
{
	public Button switchButton;
	
	public Text serialNumber;
	public Text playerName;
	public Player player;

	public void AssignPlayerInfo(string name, string serialNumber, Player player)
	{
		this.playerName.text = name;
		this.serialNumber.text = serialNumber;
		this.player = player;
		if (player.team == Player.Team.Red) {
			switchButton.transform.rotation = Quaternion.Euler (0, 0, 0);
		} else {
			switchButton.transform.rotation = Quaternion.Euler (0, 0, 180);
		}

		Invoke ("RefreshDataWithDelay", 0.1f);
	}

	void RefreshDataWithDelay ()
	{
		switchButton.gameObject.SetActive (player.isLocalPlayer);
		if (player.isLocalPlayer) {
			playerName.color = serialNumber.color = Color.cyan;
		}
	}

	public void OnClickSwitchTeam()
	{
		player.team = (player.team == Player.Team.Blue) ? Player.Team.Red : Player.Team.Blue;
		RoomUIController.Instance.SwitchTeam ();
	}
}
