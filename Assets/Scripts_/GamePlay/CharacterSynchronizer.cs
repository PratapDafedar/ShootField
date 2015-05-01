using UnityEngine;
using System.Collections;

public class CharacterSynchronizer : MonoBehaviour 
{
    public TriggerOnMouseOrJoystick fireInputHandler;
    public PerFrameRaycast          perFrameRaycast;
    public LaserScope               laserScope;

    public SignalSender mouseDownSignals;
    public SignalSender mouseUpSignals;

    private PlayerMoveController playerMoveController;
    private NetworkView netView;

    float positionX = 0;
    float positionZ = 0;
    float rotationY = 0;
    bool isFiring = false;

    private bool fireStateSync = false; 

    float netDeltaTime = 0;

	// Use this for initialization
	void Start () 
    {
        netView = this.gameObject.GetComponent<NetworkView>();
        if (!netView.isMine)
        {
            this.GetComponent<PlayerMoveController>().isControlLocked = true;
            
            //Disable some input input handlers on clientside.
            fireInputHandler.enabled = false;
            //perFrameRaycast.enabled = false;
            //laserScope.enabled = false;
        }
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
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
}
