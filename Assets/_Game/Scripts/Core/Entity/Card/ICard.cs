using UnityEngine;

public interface ICard  : IEntity
{
    void Select();
    void Unselect();
    void MoveTo(Transform target);
    CardAnimationManager cardAnimation { get; }
    }
