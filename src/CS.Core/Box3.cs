using System.Diagnostics;

namespace CS.Core;

[DebuggerDisplay("Box3({LX}x{LY}x{LZ})")]
public class Box3
{
    public Box3(int lx, int ly, int lz)
    {
        LX = lx;
        LY = ly;
        LZ = lz;
        Volume = lx * ly * lz;
    }

    public int LX { get; }
    public int LY { get; }
    public int LZ { get; }
    public int Volume { get; }

    public static Box3 CreateCube(int size) => new(size, size, size);
}