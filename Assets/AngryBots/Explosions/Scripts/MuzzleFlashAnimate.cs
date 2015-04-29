using UnityEngine;
using System.Collections;

public class MuzzleFlashAnimate : MonoBehaviour {

    void Update()
    {
        transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

        Vector3 tAngle = transform.localEulerAngles;
        tAngle.z = Random.Range(0, 90.0f);
        transform.localEulerAngles = tAngle;
    }
}
