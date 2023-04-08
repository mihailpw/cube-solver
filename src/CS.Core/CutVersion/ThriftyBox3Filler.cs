using CS.Core.CutVersion.Helpers;

namespace CS.Core.CutVersion;

public static class ThriftyBox3Filler
{
    public static int Fill(Box3 box, IEnumerable<(int size, int count)> cubes)
    {
        var sortedCubes = cubes.OrderByDescending(v => v.size).ToList();
        var boxFillState = new Box3FillState(box);

        var minCubes = FillInternal(sortedCubes, boxFillState, 0, 0, int.MaxValue);

        return minCubes == int.MaxValue ? -1 : minCubes;
    }

    private static int FillInternal(IList<(int size, int count)> cubes, Box3FillState state, int index, int count, int minCubes)
    {
        if (state.FilledOut)
            return count;

        if (index >= cubes.Count)
            return minCubes;

        var maxCount = Math.Min(cubes[index].count, state.CanAccommodate(cubes[index].size));
        for (var c = maxCount; c >= 0 && count + c < minCubes; c--)
        {
            using var _ = state.Put(cubes[index].size, c);
            var newMinCubes = FillInternal(cubes, state, index + 1, count + c, minCubes);
            if (newMinCubes < minCubes)
                minCubes = newMinCubes;
        }

        return minCubes;
    }
}