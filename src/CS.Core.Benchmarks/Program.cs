using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CS.Core;

BenchmarkRunner.Run<Benchmark>();

[MemoryDiagnoser]
public class Benchmark
{
    private (Box3 box, IEnumerable<(int size, int count)> cubes) _input;

    [Params(
        "2 2 2 > 1:8 2:1",
        "10 10 10 > 1:2000",
        "30 30 30 > 8:999 16:99")]
    public string Input { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _input = PrepareArguments(Input);
    }

    [Benchmark]
    public void FullVersion_Greedy()
    {
        CS.Core.FullVersion.GreedyBox3Filler.Fill(_input.box, _input.cubes);
    }

    [Benchmark]
    public void FullVersion_Thrifty()
    {
        CS.Core.FullVersion.ThriftyBox3Filler.Fill(_input.box, _input.cubes);
    }

    [Benchmark]
    public void CutVersion_Greedy()
    {
        CS.Core.CutVersion.GreedyBox3Filler.Fill(_input.box, _input.cubes);
    }

    [Benchmark]
    public void CutVersion_Thrifty()
    {
        CS.Core.CutVersion.ThriftyBox3Filler.Fill(_input.box, _input.cubes);
    }

    private static (Box3 box, (int, int)[] cubes) PrepareArguments(string input)
    {
        var inputSplit = input.Split(" > ");
        var boxNums = inputSplit[0].Split(' ').Select(int.Parse).ToList();
        var box = new Box3(boxNums[0], boxNums[1], boxNums[2]);
        var cubes = inputSplit[1].Split(' ')
            .Select(c => c.Split(':'))
            .Select(cc => (int.Parse(cc[0]), int.Parse(cc[1])))
            .ToArray();
        return (box, cubes);
    }
}
