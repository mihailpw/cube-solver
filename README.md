# Cube Solver

This repository contains solution for box filling with cubes.

## What is the problem?

The cube-solving problem is a type of mathematical puzzle that involves
arranging small cubes into a larger cube or rectangular box. The objective
of the puzzle is to determine how many small cubes are needed to fill the
larger cube or box.

### Inputs and expected outputs

You are given a box with integer dimensions length * width * height. You
are also given a set of cubes whose sides are powers of 2, e.g. 1x1x1,
2x2x2, 4x4x4 etc. You need to fill the box with cubes from the set.

- box: `2x2x2`          cubes: `8 1`                             > output: `1`
- box: `1x2x4`          cubes: `8 1`                             > output: `8`
- box: `10x10x10`       cubes: `2000`                            > output: `1000`
- box: `10x10x10`       cubes: `900`                             > output: `-1`
- box: `4x4x8`          cubes: `10 10 1`                         > output: `9`
- box: `5x5x5`          cubes: `61 7 1`                          > output: `62`
- box: `5x5x6`          cubes: `61 4 1`                          > output: `59`
- box: `1000x1000x1000` cubes: `0 0 0 46501 0 2791 631 127 19 1` > output: `50070`
- box: `1x1x9`          cubes: `9 1`                             > output: `9`

## Solution

### Repository structure

This repository contains the following directories:

- `src/`: Contains the source code of the problem.
    - `CS.ConsoleApp/`: Contains runner-code for demo purposes.
    - `CS.Core/`: Contains the algorithms code.
    - `CS.Core.Benchmarks/`: Contains benchmark tests for algorithms.
    - `CS.Core.Tests/`: Contains unit tests for algorithms.

### Getting Started

To see the code, you can:

- Clone the repository to your local machine.
- Open src/CubeSolver.sln with Visual Studio or JetBrains Rider

To run the demo you can use:

- Set working directory `src/`
- To run the algorithm demo, use
```
dotnet run --project CS.ConsoleApp/CS.ConsoleApp.csproj
```
- To run the cube filling demo, use
```
dotnet run --project CS.ConsoleApp/CS.ConsoleApp.csproj -- --demo
```
- To run unit tests, use
```
dotnet test
```
- To run benchmarks, use
```
dotnet run --project CS.Core.Benchmarks/CS.Core.Benchmarks.csproj -c Release
```

### Algorithms

The code show us how to solve this problem in 2 ways: in greedy way,
and non-greedy (thrifty) way.

The greedy solution puts cubes to the box from biggest to smallest, if fit. 

The thrifty solution puts cubes from biggest to smallest like in greedy one,
and after iterating over smaller cubes it takes one back, and then iterates
over smaller cubes again.

### Cut version of filling

**NOTE: This version works fine for requested inputs but won't work well for all cases.**

[
  [Greedy](https://github.com/mihailpw/cube-solver/blob/master/src/CS.Core/CutVersion/GreedyBox3Filler.cs) |
  [Thrifty](https://github.com/mihailpw/cube-solver/blob/master/src/CS.Core/CutVersion/ThriftyBox3Filler.cs)
]

The main idea is to try calculate how many cubes fit into the box, including
case that the box can contain already added cubes (and this is the main pain).

All the rules how to calculate the number of cubes in the box are determined in the
[following method](https://github.com/mihailpw/cube-solver/blob/master/src/CS.Core/CutVersion/Helpers/Box3FillState.cs#L23):

``` C#
// checking that cube side is more than min box side
if (_minSide < cubeSize)
    return 0;

var currentState = _states.Peek();
// if the cube side is 1 we can fill the rest of the box
if (cubeSize == 1)
    return currentState.volumeLeft;

// this is the main trick: just check if we have enough space including the biggest cube inside
if (_maxSide - currentState.biggestCubeSize < cubeSize)
    return 0;

// calculate the amount of cubes by volume
var canAccomodateByVolume = currentState.volumeLeft / (cubeSize * cubeSize * cubeSize);
// calculate the amount of cubes by each sides
var canAccomodateBySides = (_box.LX / cubeSize) * (_box.LY / cubeSize) * (_box.LZ / cubeSize);

// ... and take the minimum of the calculated amounts above
return Math.Min(canAccomodateByVolume, canAccomodateBySides);
```

### Full version of the filling

[
  [Greedy](https://github.com/mihailpw/cube-solver/blob/master/src/CS.Core/FullVersion/GreedyBox3Filler.cs) |
  [Thrifty](https://github.com/mihailpw/cube-solver/blob/master/src/CS.Core/FullVersion/ThriftyBox3Filler.cs)
]

The main idea is to store grid of box's cells. When the cube is putting into the box,
it iterates over all the cells and validates that the space for incoming cube is enough.

### Performance

The performance was tested with BenchmarkDotNet (v0.13.5):

```
BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.2.1 (22D68) [Darwin 22.3.0]
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 7.0.0 (7.0.22.61201), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.0 (7.0.22.61201), X64 RyuJIT AVX2

|              Method |                Input |           Mean |        Error |       StdDev |         Median | Allocated |
|-------------------- |--------------------- |---------------:|-------------:|-------------:|---------------:|----------:|
|  FullVersion_Greedy |    10 10 10 > 1:2000 | 3,887,561.1 ns | 73,919.30 ns | 82,161.11 ns | 3,890,086.3 ns |  171738 B |
| FullVersion_Thrifty |    10 10 10 > 1:2000 | 4,007,302.1 ns | 75,165.10 ns | 83,545.81 ns | 3,975,109.3 ns |  188333 B |
|   CutVersion_Greedy |    10 10 10 > 1:2000 |       180.8 ns |      3.18 ns |      4.24 ns |       180.3 ns |     552 B |
|  CutVersion_Thrifty |    10 10 10 > 1:2000 |    29,988.8 ns |    518.13 ns |    459.30 ns |    30,000.5 ns |   88464 B |
|  FullVersion_Greedy |      2 2 2 > 1:8 2:1 |       421.0 ns |     14.03 ns |     40.26 ns |       413.2 ns |     784 B |
| FullVersion_Thrifty |      2 2 2 > 1:8 2:1 |     2,290.7 ns |     48.48 ns |    140.65 ns |     2,244.7 ns |    2272 B |
|   CutVersion_Greedy |      2 2 2 > 1:8 2:1 |       214.5 ns |      4.27 ns |      5.70 ns |       213.1 ns |     568 B |
|  CutVersion_Thrifty |      2 2 2 > 1:8 2:1 |       240.3 ns |      4.07 ns |      3.80 ns |       240.4 ns |     568 B |
|  FullVersion_Greedy | 30 30(...)16:99 [22] |   950,647.9 ns | 18,319.25 ns | 18,812.52 ns |   957,363.6 ns |  219912 B |
| FullVersion_Thrifty | 30 30(...)16:99 [22] | 1,788,372.4 ns | 22,471.05 ns | 19,920.00 ns | 1,792,078.9 ns |  222146 B |
|   CutVersion_Greedy | 30 30(...)16:99 [22] |       235.3 ns |      4.67 ns |      7.12 ns |       235.1 ns |     656 B |
|  CutVersion_Thrifty | 30 30(...)16:99 [22] |     1,954.2 ns |     37.87 ns |     47.89 ns |     1,952.3 ns |    5320 B |
```

As you can see the FullVersion takes much more time to solve problem and a lot of memory allocated.