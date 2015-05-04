using UnityEngine;
using System.Collections;
using uLink;

public class CharacterSynchronizer : uLink.MonoBehaviour
{
    public TriggerOnMouseOrJoystick fireInputHandler;
    public PerFrameRaycast          perFrameRaycast;
    public LaserScope               laserScope;

    public SignalSender             mouseDownSignals;
    public SignalSender             mouseUpSignals;

    [HideInInspector]
    public int characterID;

    public User cUser
    {
        get
        {
            return User.SearchPlayerId (characterID, NetworkManager.Instance.playerList);
        }
    }

    [HideInInspector]
    public float cachedHealth = 100;
    [HideInInspector]
    private float oldHealth = 100;

    private PlayerMoveController playerMoveController;
    private uLinkNetworkView netView;

    float positionX = 0;
    float positionZ = 0;
    float rotationY = 0;
    bool isFiring = false;

    private bool fireStateSync = false; 

    float netDeltaTime = 0;
    

	// Use this for initialization
	void Start () 
    {
        oldHealth = cachedHealth = 100;

        netView = this.gameObject.GetComponent<uLinkNetworkView>();

        if (!netView.isMine)
        {
            this.GetComponent<PlayerMoveController>().isControlLocked = true;
            
            //Disable some input input handlers on clientside.
            fireInputHandler.enabled = false;

            GameManager.Instance.proxyPlayerList.Add (this.gameObject);
        }
	}

    void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            positionX = transform.position.x;
            positionZ = transform.position.z;

            rotationY = transform.eulerAngles.y;

            stream.Serialize(ref isFiring);
            stream.Serialize(ref positionX);
            stream.Serialize(ref positionZ);
            stream.Serialize(ref rotationY);
        }
        else
        {
            stream.Serialize(ref isFiring);
            stream.Serialize(ref positionX);
            stream.Serialize(ref positionZ);
            stream.Serialize(ref rotationY);
        }

        netDeltaTime = (float) info.timestamp;
    }

	void Update ()
    {
        StateSynch();

        if (GameManager.Instance.cPlayer.isGameHost)
        {
            if (cachedHealth != oldHealth)
            {
                SynchData ((int)cachedHealth);
                oldHealth = cachedHealth;
            }

            if (cachedHealth <= 0)
            {
                    GameManager.Instance.CheckRoundFinish();
            }
        }
    }

    private void StateSynch()
    {
        if (!netView.isMine)
        {
            Vector3 cPos = transform.position;
            cPos.x = positionX;
            cPos.z = positionZ;
            transform.position = Vector3.Lerp(transform.position, cPos, netDeltaTime);

            Vector3 cRot = transform.eulerAngles;
            cRot.y = rotationY;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(cRot), netDeltaTime);

            if (isFiring != fireStateSync)
            {
                if (isFiring)
                {
                    mouseDownSignals.SendSignals(this);
                }
                else
                {
                    mouseUpSignals.SendSignals(this);
                }
                fireStateSync = isFiring;
            }
        }
    }

    void OnStartFire()
    {
        isFiring = true;
    }

    void OnStopFire()
    {
        isFiring = false;
    }

    void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
    {
        int id = info.networkView.initialData.Read<int>();        
        characterID = id;
    }

    /////////////////////  [RPC]  /////////////////////

    void SynchData (int _health)
    {
        networkView.RPC("ReceiveData", uLink.RPCMode.Others, _health);	
    }

    [RPC]
    void ReceiveData (int _health)
    {
        cUser.health = _health;
    }
}
