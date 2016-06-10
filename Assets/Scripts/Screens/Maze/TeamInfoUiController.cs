using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TeamInfoUiController : MonoBehaviour 
{
	public GameObject teamInfo;
	public GameObject playerData;
	public GameObject playerBlueTeamParent;
	public GameObject playerRedTeamParent;

	public Text redTeamWinCount;
	public Text blueTeamWinCount;

	private GameObject obj;

	private List<GameObject> teamInfoList;

	private bool canTabPressed;

	void Start()
	{
		Init ();
	}

	void Init()
	{
		teamInfoList = new List<GameObject> ();
		canTabPressed = true;
	}

	void Update()
	{
		if (Input.GetKeyDown ((KeyCode.Tab))) 
		{
			if (canTabPressed) 
			{
				TabPressed ();
				canTabPressed = false;
			}
		}
	
		if (Input.GetKeyUp ((KeyCode.Tab))) 
		{
			TabReleased ();
		}
	}

	void TabPressed()
	{
		teamInfo.SetActive (true);
	}

	void TabReleased()
	{
		teamInfo.SetActive (false);
		canTabPressed = true;
	}


	void ClearTeamInfoList()
	{
		for (int i = 0; i < teamInfoList.Count; i++) 
		{
			DestroyImmediate (teamInfoList[i]);
		}
		teamInfoList.Clear ();
	}

}
