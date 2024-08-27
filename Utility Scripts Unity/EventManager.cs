using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<Type, Action<object>> eventDictionary = new Dictionary<Type, Action<object>>();

    #region Singleton
    public static EventManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public void AddListener<T>(Action<T> listener)
    {
        if (eventDictionary.TryGetValue(typeof(T), out Action<object> action))
        {
            eventDictionary[typeof(T)] = action += (x) => listener((T)x);
        }
        else
        {
            eventDictionary[typeof(T)] = (x) => listener((T)x);
        }
    }

    public void RemoveListener<T>(Action<T> listener)
    {
        if (eventDictionary.TryGetValue(typeof(T), out Action<object> action))
        {
            action -= (x) => listener((T)x);
            if (action == null)
            {
                eventDictionary.Remove(typeof(T));
            }
            else
            {
                eventDictionary[typeof(T)] = action;
            }
        }
    }

    public void FireEvent<T>(T eventInstance)
    {
        if (eventDictionary.TryGetValue(typeof(T), out Action<object> action))
        {
            action.Invoke(eventInstance);
        }
    }
}

