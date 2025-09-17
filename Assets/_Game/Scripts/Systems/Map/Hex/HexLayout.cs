using System.Collections.Generic;
using UnityEngine;

public static class HexLayout
{
    public static CircleLayoutGenerator Circle(int radius) => new CircleLayoutGenerator(radius);
    public static DiamondLayoutGenerator Diamond(int radius) => new DiamondLayoutGenerator(radius);
    public static RandomPathLayout RandomPath(int length) => new RandomPathLayout(length);
    public static DataDrivenLayoutGenerator DataDriven(IEnumerable<Vector2Int> layout) => new DataDrivenLayoutGenerator(layout);
}


