// using AdventOfCode2024.Day09;
// using Xunit.Abstractions;
//
// namespace AdventOfCode2024.Tests;
//
// public class Day09Tests(ITestOutputHelper output)
// {
//     [Fact]
//     public void Part01_Sample_Test()
//     {
//         // Arrange
//         var result = (long)0;
//         const string input = """
//                              2333133121414131402
//                              """;
//
//         // Act
//         var consoleOutput = ConsoleOutputHelper.CaptureOutput(() =>
//         {
//             result = Puzzle.Part01(input.Split(Environment.NewLine));
//         });
//
//         output.WriteLine(consoleOutput);
//         
//         // Assert
//         result.Should().Be(1928);
//     }
//     
//     [Fact(Skip = "WIP")]
//     public void Part02_Sample_Test()
//     {
//         const string input = """
//                              ............
//                              ........0...
//                              .....0......
//                              .......0....
//                              ....0.......
//                              ......A.....
//                              ............
//                              ............
//                              ........A...
//                              .........A..
//                              ............
//                              ............
//                              """;
//         
//         var result = Puzzle.Part02(input.Split(Environment.NewLine));
//     
//         result.Should().Be(11387);
//     }
// }