using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerCellView : MonoBehaviour 
{
	public Button switchButton;
	
	public Text serialNumber;
	public Text name;

	public Action SwitchTeam;

	public void AssignPlayerInfo(string name, string serialNumber, string team)
	{
		this.name.text = name;
		this.serialNumber.text = serialNumber;
		if (team.Equals ("Red")) 
		{
			switchButton.transform.rotation = Quaternion.Euler (0, 0, 180);
		}
	}

	public void OnClickSwitchTeam()
	{
		if (SwitchTeam != null) 
		{
			SwitchTeam ();
		}
	}

}
