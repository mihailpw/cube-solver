using CS.Core.CutVersion.Helpers;

namespace CS.Core.CutVersion;

public static class GreedyBox3Filler
{
    public static int Fill(Box3 box, IEnumerable<(int size, int count)> cubes)
    {
        var sortedCubes = cubes.OrderByDescending(v => v.size).ToList();
        var boxFillState = new Box3FillState(box);

        var minCubes = 0;
        for (var i = 0; i < sortedCubes.Count && !boxFillState.FilledOut; i++)
        {
            var count = Math.Min(sortedCubes[i].count, boxFillState.CanAccommodate(sortedCubes[i].size));
            minCubes += count;
            boxFillState.Put(sortedCubes[i].size, count);
        }

        return boxFillState.FilledOut ? minCubes : -1;
    }
}