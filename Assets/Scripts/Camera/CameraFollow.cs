using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public static CameraFollow Instance;
	Vector3 OFFSET_TEMP = new Vector3 (0.8f, - 3.9f, -2.4f);

	const float SQR_MAG = 350.0f;

	const float DIV_OFFSET = 5.0f;

	public struct TargetPosition
	{
		public Vector3 initialPos;
		public Vector3 finalPos;
	}

	public Vector3 initPos;
	public Vector3 finPos;

    public Transform target;            // The position that that camera will be following.

    public float smoothing = 0.2f;      // The speed with which the camera will be following.

	public float zoomDamping = 0.6f;

	public float fovAcceleration = 3;
        
	private Camera cCamera;

    public Vector3 offset;                     // The initial offset from the target.

	private Transform cursor;

	private TargetPosition tarPosition;
	private Vector3 cursorOffset;

	Vector3 camRefVel;

	float intiailFOV;
    float initRadius;

    float angle;

    bool initDone = false;

	void Awake ()
	{
		Instance = this;

		Application.targetFrameRate = 50;
		if (GameManager.Instance != null && 
			GameManager.Instance.localPlayer != null) {
			target = GameManager.Instance.localPlayer;
		}
	}

    private void Init ()
    {
        if (target == null)
            return;

		Vector3 temp = OFFSET_TEMP;

        offset = transform.position - target.position - temp; 
        //"(0.0, 13.7, -12.5)"
        //transform.position = target.position;

//        offset.x = 0;
//        offset.y = 13.7f;
//        offset.z = - 12.5f;

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
		cursor = target.GetComponent<PlayerMoveController> ().cursorObject;
		tarPosition.initialPos = target.position;
    }

    void LateUpdate ()
    {
        if (! initDone)
        {
            Init ();
            return;
        }

        if (target == null)
            return;
		initPos = cursor.position;
		finPos = tarPosition.finalPos; 
		if (Vector3.SqrMagnitude (cursor.position - tarPosition.initialPos) > SQR_MAG) 
		{
			if (Vector3.Distance(tarPosition.finalPos,cursor.position) > 4)
			{
				cursorOffset = (cursor.position - tarPosition.initialPos) / DIV_OFFSET;
				tarPosition.initialPos = target.position;
				tarPosition.finalPos = cursor.position;
			}
		} 
		else 
		{
			cursorOffset = new Vector3(0, 0, 0);
		}

		Vector3 targetCamPos = target.position + offset + cursorOffset;
	
		float acceleration = Vector3.Distance (transform.position, targetCamPos);
		
		targetCamPos = Vector3.SmoothDamp (transform.position, 
												targetCamPos,
												ref camRefVel,
												Time.deltaTime,
				                                10,
				                                smoothing * acceleration * 2 * Time.deltaTime);

		transform.position = targetCamPos;
                                       
		Quaternion rotation = Quaternion.LookRotation((target.position + cursorOffset) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothing * 10);        
    }
}
    