using System;
using UnityEngine;
using System.Collections;

public class EventCallbackTimer 
{
    public float spentTime;
    public float remainingTime;

    private Action          callback;
    private MonoBehaviour   root;
    private float           callBackTime;

    public EventCallbackTimer(MonoBehaviour root, Action callback, float callBackTime)
    {
        this.root = root;
        this.callback = callback;
        this.callBackTime = callBackTime;
    }

    public void Start()
    {
        root.StartCoroutine(TimerRoutine());
    }

    public void Stop()
    {
        root.StopAllCoroutines();
    }

    IEnumerator TimerRoutine()
    {
        while (spentTime <= callBackTime)
        {
            spentTime += Time.deltaTime;
            remainingTime = callBackTime - spentTime;

            yield return null;
        }

        if (callback != null)
        {
            callback();
        }
    }
}
