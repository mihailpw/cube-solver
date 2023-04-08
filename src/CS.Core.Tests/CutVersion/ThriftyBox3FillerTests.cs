using CS.Core.CutVersion;

namespace CS.Core.Tests.CutVersion;

[TestOf(typeof(ThriftyBox3Filler))]
public class ThriftyBox3FillerTests
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
    [TestCase("4 4 4", "3:99 2:99 1:99", ExpectedResult = 8)]
    public int Fill_ValidData_ResolvesCorrectCount(string boxInput, string cubesInput)
    {
        var data = PrepareCase(boxInput, cubesInput);

        return ThriftyBox3Filler.Fill(data.box, data.cubes);
    }

    [Test]
    // Following case will took a long time because of a lot of calculations
    public async Task Fill_ValidData_TakesMore3Sec()
    {
        var data = PrepareCase("1000 1000 1000", "1:0 2:0 4:0 8:46501 16:0 32:2791 64:631 128:127 256:19 512:1");
        var timeoutTask = Task.Delay(-1, new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token);

        var firstExecutedTask = await Task.WhenAny(
            Task.Run(() => ThriftyBox3Filler.Fill(data.box, data.cubes)),
            timeoutTask);

        Assert.That(firstExecutedTask, Is.EqualTo(timeoutTask));
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