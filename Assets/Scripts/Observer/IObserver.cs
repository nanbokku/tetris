using System;

public interface IObserver
{
    void ValueChanged(object value);
}