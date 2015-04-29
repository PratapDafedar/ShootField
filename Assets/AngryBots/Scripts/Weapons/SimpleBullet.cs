
using UnityEngine;
using System.Collections;

public class SimpleBullet : MonoBehaviour
{
    float speed = 10;
    float lifeTime = 0.5f;
    float dist = 10000;

    private float spawnTime = 0.0f;
    private Transform tr;

    void OnEnable()
    {
        tr = transform;
        spawnTime = Time.time;
    }

    void Update()
    {
        tr.position += tr.forward * speed * Time.deltaTime;
        dist -= speed * Time.deltaTime;
        if (Time.time > spawnTime + lifeTime || dist < 0)
        {
            Spawner.Destroy(gameObject);
        }
    }

}