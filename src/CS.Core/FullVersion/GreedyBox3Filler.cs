using CS.Core.FullVersion.Helpers;

namespace CS.Core.FullVersion;

public static class GreedyBox3Filler
{
    public static int Fill(Box3 box, IEnumerable<(int size, int count)> cubes)
    {
        var sortedCubes = cubes.OrderByDescending(v => v.size).ToList();
        var boxFiller = new Box3Filler(box);

        var boxesCount = 0;
        for (var i = 0; i < sortedCubes.Count && !boxFiller.IsFilledOut; i++)
        {
            var count = 0;
            for (var b = 0; b < sortedCubes[i].count; b++)
                if (boxFiller.PutBox(Box3.CreateCube(sortedCubes[i].size)))
                    count++;
                else
                    break;

            boxesCount += count;
        }

        return boxFiller.IsFilledOut ? boxesCount : -1;
    }
}