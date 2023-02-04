using System;
using System.Collections;
using System.Collections.Generic;

public class Listenable
{
    readonly List<Action<Listenable>> listeners = new();
    
    public void AddListener(Action<Listenable> listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(Action<Listenable> listener)
    {
        listeners.Remove(listener);
    }

    public void NotifyListeners()
    {
        foreach (var listener in listeners)
        {
            listener(this);
        }
    }
}
