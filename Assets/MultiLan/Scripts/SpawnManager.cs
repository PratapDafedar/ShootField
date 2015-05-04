using UnityEngine;
using System.Collections;
using uLink;

public class SpawnManager : uLink.MonoBehaviour 
{
    public static SpawnManager Instance;

	public GameObject playerPrefab;
	public Transform [] spawnersBlue;
    public Transform[] spawnersRed;

    private float radius = 5.0f;

    private GameObject currentInstance;

	public void Awake () {
        Instance = this;		
	}

    public void SpawnPlayer (User player)
    {
        DestroySpawnData();

        Vector3 spawnPos;

        if (player.cTeam == User.Team.BLUE)
        {
            int rand = Random.Range(0, spawnersBlue.Length - 1);
            spawnPos = spawnersBlue[rand].position;
        }
        else
        {
            int rand = Random.Range(0, spawnersRed.Length - 1);
            spawnPos = spawnersRed[rand].position;
        }

        spawnPos.x += Random.Range(-radius, radius);
        spawnPos.z += Random.Range(-radius, radius);

        currentInstance = uLink.Network.Instantiate(uLink.Network.player, playerPrefab, playerPrefab, playerPrefab, spawnPos, Quaternion.identity, player.id, player.id, player.id);
        CameraFollow.Instance.target = currentInstance.transform;
        currentInstance.GetComponent<CharacterSynchronizer>().characterID = player.id;
    }

    private void DestroySpawnData()
    {
        if (currentInstance != null)
        {
            DestroyImmediate(currentInstance);
        }

        for (int i = 0; i < GameManager.Instance.proxyPlayerList.Count; i++)
        {
            GameObject tempGO = GameManager.Instance.proxyPlayerList[i];
            DestroyImmediate(tempGO);
        }
    }	
}
