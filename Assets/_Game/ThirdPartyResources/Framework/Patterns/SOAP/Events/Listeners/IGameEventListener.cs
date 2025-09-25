using Google.Impl;
using UnityEngine;

public interface IGameEventListener<T>
{
    void OnEventRaised(T sender);

}
public interface IGameEventListener
{
    void OnEventRaised();
}
