using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * From https://unity3d.com/fr/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
 * Changes : remove the unused Singleton pattern, use static instead (75 to 46 lines)
 */
public static class EventManager<T> {

    [System.Serializable]
    public class EventWithArgs : UnityEvent<T> { }

    private static Dictionary<string, EventWithArgs> eventDictionary = new Dictionary<string, EventWithArgs> ();

    public static void StartListening (string eventName, UnityAction<T> listener) {
        EventWithArgs thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent)) {
            thisEvent.AddListener (listener);
        } else {
            thisEvent = new EventWithArgs ();
            thisEvent.AddListener (listener);
            eventDictionary.Add (eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction<T> listener) {
        EventWithArgs thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent)) {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent (string eventName, T arg = default(T)) {
        EventWithArgs thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent)) {
            thisEvent.Invoke (arg);
        }
    }
}