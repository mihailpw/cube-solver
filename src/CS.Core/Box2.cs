using System.Diagnostics;

namespace CS.Core;

[DebuggerDisplay("Box2({LX}x{LY})")]
public class Box2
{
    public Box2(int lx, int ly)
    {
        LX = lx;
        LY = ly;
        Square = lx * ly;
    }

    public int LX { get; }
    public int LY { get; }
    public int Square { get; }

    public static Box2 CreateSquare(int size) => new(size, size);
}