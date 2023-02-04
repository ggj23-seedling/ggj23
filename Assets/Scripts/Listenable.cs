using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listenable<T> where T : Listenable<T>
{
    readonly List<Action<T>> listeners = new();
    
    public void AddListener(Action<T> listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(Action<T> listener)
    {
        listeners.Remove(listener);
    }

    public void NotifyListeners()
    {
        foreach (var listener in listeners)
        {
            if (this is T)
            {
                listener(this as T);
            } 
            else
            {
                Debug.LogError($"Listenable<{typeof(T)}> is not {typeof(T)}");
            }
        }
    }
}
