using CS.Core.FullVersion;

namespace CS.Core.Tests.FullVersion;

[TestOf(typeof(GreedyBox3Filler))]
public class GreedyBox3FillerTests
{
    // read cubesInput as: size:num size:num ...
    [TestCase("2 2 2", "1:8 2:1", ExpectedResult = 1)]
    [TestCase("1 2 4", "1:8 2:1", ExpectedResult = 8)]
    [TestCase("10 10 10", "1:2000", ExpectedResult = 1000)]
    [TestCase("10 10 10", "1:900", ExpectedResult = -1)]
    [TestCase("4 4 8", "1:10 2:10 4:1", ExpectedResult = 9)]
    [TestCase("5 5 5", "1:61 2:7 4:1", ExpectedResult = 62)]
    [TestCase("5 5 6", "1:61 2:4 4:1", ExpectedResult = 59)]
    [TestCase("1 1 9", "1:9 2:1", ExpectedResult = 9)]
    public int Fill_ValidData_ResolvesCorrectCount(string boxInput, string cubesInput)
    {
        var data = PrepareCase(boxInput, cubesInput);

        return GreedyBox3Filler.Fill(data.box, data.cubes);
    }

    // Following case will work incorrectly with greedy algorithm
    [TestCase("4 4 4", "3:99 2:99 1:99", ExpectedResult = 38)] // real minimum is 8
    [TestCase("4 4 4", "3:99 2:99", ExpectedResult = -1)] // real minimum is still 8
    public int Fill_ValidData_IncorrectResult(string boxInput, string cubesInput)
    {
        var data = PrepareCase(boxInput, cubesInput);

        return GreedyBox3Filler.Fill(data.box, data.cubes);
    }

    private static (Box3 box, (int, int)[] cubes) PrepareCase(string boxInput, string cubesInput)
    {
        var boxNums = boxInput.Split(' ').Select(int.Parse).ToList();
        var box = new Box3(boxNums[0], boxNums[1], boxNums[2]);
        var cubes = cubesInput.Split(' ')
            .Select(c => c.Split(':'))
            .Select(cc => (int.Parse(cc[0]), int.Parse(cc[1])))
            .ToArray();
        return (box, cubes);
    }
}