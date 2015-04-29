using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public static CameraFollow Instance;

	public enum CameraType {
		Binocular,
		FIFA_Style,
		DSLR,
	}
	public CameraType currentType;

    public Transform target;            // The position that that camera will be following.

    public float smoothing = 0.2f;      // The speed with which the camera will be following.

	public float zoomDamping = 0.6f;

	public float fovAcceleration = 3;
        
	private Camera cCamera;

    public Vector3 offset;                     // The initial offset from the target.

	Vector3 camRefVel;

	float intiailFOV;
    float initRadius;

    float angle;

    bool initDone = false;

	void Awake ()
	{
		Instance = this;

		Application.targetFrameRate = 50;
	}

    private void Init ()
    {
        if (target == null)
            return;

        //offset = transform.position - target.position;
        //"(0.0, 13.7, -12.5)"
        //transform.position = target.position;

        offset.x = 0;
        offset.y = 13.7f;
        offset.z = - 12.5f;

        initRadius = offset.sqrMagnitude;

        //set it's intial position w.r.t. target pos.
        transform.position = target.position + offset;

        cCamera = this.GetComponent<Camera>();
        if (cCamera == null)
        {
            Debug.LogError("Attach this script to Camera component.");
            return;
        }
        intiailFOV = cCamera.fieldOfView;

        initDone = true;
    }

    void LateUpdate ()
    {
        if (! initDone)
        {
            Init ();
            return;
        }

		
		Vector3 targetCamPos = target.position + offset;
					
		float acceleration = Vector3.Distance (transform.position, targetCamPos);
		
		targetCamPos = Vector3.SmoothDamp (transform.position, 
												targetCamPos,
												ref camRefVel,
												Time.deltaTime,
				                                10,
				                                smoothing * acceleration * 2 * Time.deltaTime);					
					
		transform.position = targetCamPos;
                                       
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothing * 10);        
    }
}
    