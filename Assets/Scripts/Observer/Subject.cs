using System.Collections;
using System.Collections.Generic;

public abstract class Subject
{
    private List<IObserver> observers = new List<IObserver>();


    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(object value)
    {
        foreach (var observer in observers)
        {
            observer.Update(value);
        }
    }
}