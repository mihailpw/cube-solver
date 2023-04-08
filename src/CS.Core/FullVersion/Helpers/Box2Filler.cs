namespace CS.Core.FullVersion.Helpers;

public class Box2Filler
{
    private readonly Box2?[,] _boxesGrid;
    // optimization: caching stored box positions for quick remove
    private readonly Dictionary<Box2, (int ix, int iy)> _boxesInside = new();
    private int _squareLeft;

    public Box2Filler(Box2 box)
    {
        Box = box;
        _boxesGrid = new Box2[box.LX, box.LY];
        _squareLeft = box.Square;
    }

    public Box2 Box { get; }
    public bool IsFilledOut => _squareLeft == 0;
    public bool IsEmpty => _squareLeft == Box.Square;

    public Box2? this[int x, int y] => _boxesGrid[x, y];

    public bool PutBox(Box2 box)
    {
        if (_boxesInside.ContainsKey(box))
            throw new ArgumentException("The box is already inside");

        // optimization: skip adding box is we don't have required volume for that
        if (_squareLeft < box.Square)
            return false;

        // optimization: skip adding box if any of sides is more than current box size
        if (Box.LX < box.LX || Box.LY < box.LY)
            return false;

        for (var ix = 0; ix < Box.LX; ix++)
        for (var iy = 0; iy < Box.LY; iy++)
            if (TryReplaceBatch(
                    ix, iy,
                    box.LX, box.LY,
                    null, box))
            {
                _boxesInside.Add(box, (ix, iy));
                _squareLeft -= box.Square;
                return true;
            }
            else
            {
                // optimization: skip next N iterations because it is already busy
                var boxHere = _boxesGrid[ix, iy];
                if (boxHere != null)
                    iy += boxHere.LY - 1;
            }

        return false;
    }

    public bool TakeBox(Box2 box)
    {
        if (!_boxesInside.ContainsKey(box))
            return false;

        var (ix, iy) = _boxesInside[box];
        if (!TryReplaceBatch(
                ix, iy,
                box.LX, box.LY,
                box, null))
            throw new InvalidOperationException("Something went wrong");
        
        _boxesInside.Remove(box);
        _squareLeft += box.Square;
        return true;
    }

    private bool TryReplaceBatch(
        int ix, int iy,
        int sx, int sy,
        Box2? verifyCurrentBox,
        Box2? setNewBox)
    {
        var lx = ix + sx;
        if (ix < 0 || lx > Box.LX)
            return false;
        var ly = iy + sy;
        if (iy < 0 || ly > Box.LY)
            return false;

        for (var wix = ix; wix < lx; wix++)
        for (var wiy = iy; wiy < ly; wiy++)
            if (_boxesGrid[wix, wiy] != verifyCurrentBox)
                return false;

        for (var wix = ix; wix < lx; wix++)
        for (var wiy = iy; wiy < ly; wiy++)
            _boxesGrid[wix, wiy] = setNewBox;

        return true;
    }
}