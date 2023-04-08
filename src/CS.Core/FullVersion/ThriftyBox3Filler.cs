using CS.Core.FullVersion.Helpers;

namespace CS.Core.FullVersion;

public static class ThriftyBox3Filler
{
    public static int Fill(Box3 box, IEnumerable<(int size, int count)> availableCubes)
    {
        var sortedCubes = availableCubes.OrderByDescending(v => v.size).ToList();
        var boxFiller = new Box3Filler(box);

        var minCubes = FillInternal(boxFiller, sortedCubes, 0, 0, int.MaxValue);

        return minCubes == int.MaxValue ? -1 : minCubes;
    }

    private static int FillInternal(Box3Filler boxFiller, IList<(int size, int count)> cubes,
        int index, int count, int minCubes)
    {
        if (boxFiller.IsFilledOut)
            return count;

        if (index >= cubes.Count)
            return minCubes;

        var puttedBoxes = new Stack<Box3>();
        try
        {
            for (var b = 0; b < cubes[index].count; b++)
            {
                var puttedBox = Box3.CreateCube(cubes[index].size);
                if (boxFiller.PutBox(puttedBox))
                {
                    puttedBoxes.Push(puttedBox);
                }
                else
                    break;
            }
            while (puttedBoxes.Count >= 0 && count + puttedBoxes.Count < minCubes)
            {
                var newMinCubes = FillInternal(boxFiller, cubes, index + 1, count + puttedBoxes.Count, minCubes);
                if (newMinCubes < minCubes)
                    minCubes = newMinCubes;
                if (puttedBoxes.Count > 0)
                    boxFiller.TakeBox(puttedBoxes.Pop());
                else
                    break;
            }
        }
        finally
        {
            while (puttedBoxes.Count > 0)
                boxFiller.TakeBox(puttedBoxes.Pop());
        }

        return minCubes;
    }
}