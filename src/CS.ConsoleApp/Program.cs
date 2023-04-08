using CS.Core;
using CS.Core.CutVersion;
using CS.Core.FullVersion.Helpers;

if (!Environment.GetCommandLineArgs().Contains("--demo"))
{
    ExecuteMainFlow();
}
else
{
    ExecuteDemo();
}

void ExecuteMainFlow()
{
    const string input = @"2 2 2 8 1
1 2 4 8 1
10 10 10 2000
10 10 10 900
4 4 8 10 10 1
5 5 5 61 7 1
5 5 6 61 4 1
1000 1000 1000 0 0 0 46501 0 2791 631 127 19 1
1 1 9 9 1";

    var lines = input.Split(Environment.NewLine);
    foreach (var line in lines)
    {
        var nums = line.Split(' ').Select(int.Parse).ToList();
        var box = new Box3(nums[0], nums[1], nums[2]);
        var cubes = nums.Skip(3).Select((c, i) => (1 << i, c));
        var cubesCount = GreedyBox3Filler.Fill(box, cubes);
        Console.WriteLine(cubesCount);
    }
}

void ExecuteDemo()
{
    var filler = new Box2Filler(new Box2(4, 3));

    Box2 AddSquare(int size)
    {
        var box = Box2.CreateSquare(size);
        Console.WriteLine($"Adding {size}x{size}... {(filler.PutBox(box) ? "done" : "no space")}");
        return box;
    }

    AddSquare(2);
    var second2x2Box = AddSquare(2);
    AddSquare(2);
    Printers.PrintBox(filler, "result adding 2x2 square");
    AddSquare(1);
    AddSquare(1);
    Printers.PrintBox(filler, "result adding two 1x1 squares");
    AddSquare(1);
    AddSquare(1);
    AddSquare(1);
    Printers.PrintBox(filler, "filled result");
    filler.TakeBox(second2x2Box);
    Printers.PrintBox(filler, "result after taking second 2x2 square");
}