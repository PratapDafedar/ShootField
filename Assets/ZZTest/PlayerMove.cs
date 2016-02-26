using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public float speed = 15f;

    void Update()
    {
        if (!isLocalPlayer)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            return;
        }

        var x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(x, 0, z, Space.World);
    }
}