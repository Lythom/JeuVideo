using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/**
 * From https://unity3d.com/fr/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
 * Changes : remove the unused Singleton pattern, use static instead (75 to 46 lines)
 */
public static class EventManager {

    private static Dictionary <string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();

    public static void StartListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.AddListener (listener);
        } 
        else
        {
            thisEvent = new UnityEvent ();
            thisEvent.AddListener (listener);
            eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent))
        {
            thisEvent.Invoke ();
        }
    }
}