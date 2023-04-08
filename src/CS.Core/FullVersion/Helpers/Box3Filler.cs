namespace CS.Core.FullVersion.Helpers;

public class Box3Filler
{
    private readonly Box3?[,,] _boxesGrid;
    // optimization: caching stored box positions for quick remove
    private readonly Dictionary<Box3, (int ix, int iy, int iz)> _boxesInside = new();
    private int _volumeLeft;

    public Box3Filler(Box3 box)
    {
        Box = box;
        _boxesGrid = new Box3[box.LX, box.LY, box.LZ];
        _volumeLeft = box.Volume;
    }

    public Box3 Box { get; }
    public bool IsFilledOut => _volumeLeft == 0;
    public bool IsEmpty => _volumeLeft == Box.Volume;

    public Box3? this[int x, int y, int z] => _boxesGrid[x, y, z];

    public bool PutBox(Box3 box)
    {
        if (_boxesInside.ContainsKey(box))
            throw new ArgumentException("The box is already inside");

        // optimization: skip adding box is we don't have required volume for that
        if (_volumeLeft < box.Volume)
            return false;

        // optimization: skip adding box if any of sides is more than current box size
        if (Box.LX < box.LX || Box.LY < box.LY || Box.LZ < box.LZ)
            return false;

        for (var ix = 0; ix < Box.LX; ix++)
        for (var iy = 0; iy < Box.LY; iy++)
        for (var iz = 0; iz < Box.LZ; iz++)
            if (TryReplaceBatch(
                    ix, iy, iz,
                    box.LX, box.LY, box.LZ,
                    null, box))
            {
                _boxesInside.Add(box, (ix, iy, iz));
                _volumeLeft -= box.Volume;
                return true;
            }
            else
            {
                // optimization: skip next N iterations because it is already busy
                var boxHere = _boxesGrid[ix, iy, iz];
                if (boxHere != null)
                    iz += boxHere.LZ - 1;
            }

        return false;
    }

    public bool TakeBox(Box3 box)
    {
        if (!_boxesInside.ContainsKey(box))
            return false;

        var (ix, iy, iz) = _boxesInside[box];
        if (!TryReplaceBatch(
                ix, iy, iz,
                box.LX, box.LY, box.LZ,
                box, null))
            throw new InvalidOperationException("Something went wrong");
        
        _boxesInside.Remove(box);
        _volumeLeft += box.Volume;
        return true;
    }

    private bool TryReplaceBatch(
        int ix, int iy, int iz,
        int sx, int sy, int sz,
        Box3? verifyCurrentBox,
        Box3? setNewBox)
    {
        var lx = ix + sx;
        if (ix < 0 || lx > Box.LX)
            return false;
        var ly = iy + sy;
        if (iy < 0 || ly > Box.LY)
            return false;
        var lz = iz + sz;
        if (iz < 0 || lz > Box.LZ)
            return false;

        for (var wix = ix; wix < lx; wix++)
        for (var wiy = iy; wiy < ly; wiy++)
        for (var wiz = iz; wiz < lz; wiz++)
            if (_boxesGrid[wix, wiy, wiz] != verifyCurrentBox)
                return false;

        for (var wix = ix; wix < lx; wix++)
        for (var wiy = iy; wiy < ly; wiy++)
        for (var wiz = iz; wiz < lz; wiz++)
            _boxesGrid[wix, wiy, wiz] = setNewBox;

        return true;
    }
}