using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfoCellView : MonoBehaviour 
{
	public Text name;
	public Text serialNumber;
	public Text killCount;

	public void AssignInfo(string name, string serialNumber, string killCount)
	{
		this.name.text = name;
		this.serialNumber.text = serialNumber;
		this.killCount.text = killCount;
	}
}
