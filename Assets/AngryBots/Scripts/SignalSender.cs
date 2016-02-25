﻿
using UnityEngine;
using System.Collections;

[System.Serializable]
public class ReceiverItem
{
    public GameObject receiver;
    public string action = "OnSignal";
    public float delay;

    public IEnumerator SendWithDelay(MonoBehaviour sender)
    {
		yield return new WaitForSeconds (delay);
		if (receiver)
		    receiver.SendMessage (action, SendMessageOptions.DontRequireReceiver);
		else
			Debug.LogWarning ("No receiver of signal \""+action+"\" on object "+sender.name+" ("+sender.GetType().Name+")", sender);
	}
}

[System.Serializable]
public class SignalSender
{
    public bool onlyOnce;
    public ReceiverItem[] receivers;

    private bool hasFired = false;

    public void SendSignals(MonoBehaviour sender)
    {
        if (hasFired == false || onlyOnce == false)
        {
            for (int i = 0; i < receivers.Length; i++)
            {
                sender.StartCoroutine(receivers[i].SendWithDelay(sender));
            }
            hasFired = true;
        }
    }
}
