using UnityEngine;

public interface ICardLayoutBase
{
    Vector3[] CalculatePositions();
    void UpdateLayout();
    void ApplyLayout(Vector3[] positions);
}