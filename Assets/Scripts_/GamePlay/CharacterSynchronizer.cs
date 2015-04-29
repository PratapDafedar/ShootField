using UnityEngine;
using System.Collections;

public class CharacterSynchronizer : MonoBehaviour {

    private NetworkView netView;

    float positionX = 0;
    float positionZ = 0;
    float rotationY = 0;

    float netDeltaTime = 0;

	// Use this for initialization
	void Start () 
    {
        netView = this.gameObject.GetComponent<NetworkView>();
        if (!netView.isMine)
        {
            //this.GetComponent <Playermo>
        }
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            positionX = transform.position.x;
            positionZ = transform.position.z;

            rotationY = transform.eulerAngles.y;

            stream.Serialize(ref positionX);
            stream.Serialize(ref positionZ);
            stream.Serialize(ref rotationY);
        }
        else
        {
            stream.Serialize(ref positionX);
            stream.Serialize(ref positionZ);
            stream.Serialize(ref rotationY);
        }

        netDeltaTime = (float) info.timestamp;
    }

	void LateUpdate () 
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
        }
	}
}
