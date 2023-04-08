using CS.Core.Utils;

namespace CS.Core.CutVersion.Helpers;

internal class Box3FillState
{
    private readonly Box3 _box;
    private readonly int _minSide;
    private readonly int _maxSide;
    private readonly Stack<(int biggestCubeSize, int volumeLeft)> _states = new();

    public Box3FillState(Box3 box)
    {
        _box = box;
        _minSide = Math.Min(box.LX, Math.Min(box.LY, box.LZ));
        _maxSide = Math.Max(box.LX, Math.Max(box.LY, box.LZ));

        _states.Push((0, box.Volume));
    }

    public bool FilledOut => _states.Peek().volumeLeft == 0;

    public int CanAccommodate(int cubeSize)
    {
        if (_minSide < cubeSize)
            return 0;

        var currentState = _states.Peek();
        if (cubeSize == 1)
            return currentState.volumeLeft;

        if (_maxSide - currentState.biggestCubeSize < cubeSize)
            return 0;

        var canAccomodateByVolume = currentState.volumeLeft / (cubeSize * cubeSize * cubeSize);
        var canAccomodateBySides = (_box.LX / cubeSize) * (_box.LY / cubeSize) * (_box.LZ / cubeSize);

        return Math.Min(canAccomodateByVolume, canAccomodateBySides);
    }

    public IDisposable Put(int cubeSize, int count)
    {
        if (count <= 0)
            return Disposable.Empty;

        var cubesVolume = cubeSize * cubeSize * cubeSize * count;
        var currentState = _states.Peek();
        _states.Push((
            biggestCubeSize: Math.Max(currentState.biggestCubeSize, cubeSize),
            volumeLeft: currentState.volumeLeft - cubesVolume));

        return new Disposable(() => _states.Pop());
    }
}