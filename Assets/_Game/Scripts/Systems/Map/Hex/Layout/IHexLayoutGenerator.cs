using System.Collections.Generic;
using UnityEngine;

public interface IHexLayoutGenerator
{
    IEnumerable<Vector2Int> GenerateLayout();
}

public class RectangularLayout : IHexLayoutGenerator
{
    private readonly int width;
    private readonly int height;

    public RectangularLayout(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public IEnumerable<Vector2Int> GenerateLayout()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                yield return new Vector2Int(x, y);
    }
}
public class RandomPathLayout : IHexLayoutGenerator
{
    private readonly int length;
    private readonly System.Random rng = new();

    public RandomPathLayout(int length)
    {
        this.length = length;
    }

    public IEnumerable<Vector2Int> GenerateLayout()
    {
        var pos = Vector2Int.zero;
        yield return pos;

        for (int i = 1; i < length; i++)
        {
            var dir = (HexDirection)rng.Next(0, 6);
            pos += dir.GetOffset(pos.y);
            yield return pos;
        }
    }
}

public class CircleLayoutGenerator : IHexLayoutGenerator
{
    private readonly int radius;

    public CircleLayoutGenerator(int radius)
    {
        this.radius = radius;
    }

    public IEnumerable<Vector2Int> GenerateLayout()
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int dz = -dx - dy;
                if (Mathf.Abs(dx) <= radius && Mathf.Abs(dy) <= radius && Mathf.Abs(dz) <= radius)
                {
                    // Cube coords chuyển sang offset coords
                    int col = dx + (dy - (dy & 1)) / 2;
                    int row = dy;
                    yield return new Vector2Int(col, row);
                }
            }
        }
    }
}

public class DiamondLayoutGenerator : IHexLayoutGenerator
{
    private readonly int radius;

    public DiamondLayoutGenerator(int radius)
    {
        this.radius = radius;
    }

    public IEnumerable<Vector2Int> GenerateLayout()
    {
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= radius)
                {
                    yield return new Vector2Int(x, y);
                }
            }
        }
    }
}

public class DataDrivenLayoutGenerator : IHexLayoutGenerator
{
    private readonly IEnumerable<Vector2Int> positions;

    public DataDrivenLayoutGenerator(IEnumerable<Vector2Int> positions)
    {
        this.positions = positions;
    }

    public IEnumerable<Vector2Int> GenerateLayout()
    {
        return positions;
    }
}
