namespace VolleyManagement.UnitTests
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class provides way to test StyleCop support for C#7
    /// </summary>
    [TestClass]
    [Ignore]
    public class TestCSharp7FeaturesSupport
    {
        private interface IFigure
        {
        }

        [TestMethod]
        public void CoolNewFeatures()
        {
            // Out variables
            var p = new Point { X = 2, Y = 3 };
            p.GetCoordinates(out int x, out int y);
            Debug.WriteLine($"({x}, {y})");

            // Same with var
            p.GetCoordinates(out var x1, out var y1);
            Debug.WriteLine($"({x1}, {y1})");

            // Pattern matching
            IFigure shape = new Rectangle { Height = 10, Length = 13 };
            switch (shape)
            {
                case Circle c:
                    Debug.WriteLine($"circle with radius {c.Radius}");
                    break;
                case Rectangle s when s.Length == s.Height:
                    Debug.WriteLine($"{s.Length} x {s.Height} square");
                    break;
                case Rectangle r:
                    Debug.WriteLine($"{r.Length} x {r.Height} rectangle");
                    break;
                default:
                    Debug.WriteLine("<unknown shape>");
                    break;
                case null:
                    throw new ArgumentNullException(nameof(shape));
            }

            // Tuples
            var names = LookupName(10);
            Debug.WriteLine($"found {names.Item1} {names.Item3}.");

            var names1 = LookupName1(10);
            Debug.WriteLine($"found {names1.first} {names1.last}.");

            // Deconstruction
            (var pX, var pY) = p;

            Debug.WriteLine($"({pX}, {pY})");

            // Local functions
            Debug.WriteLine(Fibonacci(6));

            // Literals
            var d = 123_456;
            var h = 0xAB_CD_EF;
            var b = 0b1010_1011_1100_1101_1110_1111;
            Debug.WriteLine($"d={d}, h={h}, b={b}");

            // Ref returns and locals
            int[] array = { 1, 15, -39, 0, 7, 14, -12 };
            ref int place = ref Find(7, array); // aliases 7's place in the array
            place = 9; // replaces 7 with 9 in the array
            Debug.WriteLine(array[4]); // prints 9
        }

        public int Fibonacci(int x)
        {
            if (x < 0)
            {
                throw new ArgumentException("Less negativity please!", nameof(x));
            }

            return Fib(x).current;

            (int current, int previous) Fib(int i)
            {
                if (i == 0)
                {
                    return (1, 0);
                }

                var(p, pp) = Fib(i - 1);
                return (p + pp, p);
            }
        }

        public ref int Find(int number, int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == number)
                {
                    return ref numbers[i]; // return the storage location, not the value
                }
            }

            throw new IndexOutOfRangeException($"{nameof(number)} not found");
        }

        private(string, string, string) LookupName(long id) // tuple return type
        {
            var first = "First";
            var middle = "Middle";
            var last = "Last";
            return (first, middle, last); // tuple literal
        }

        private(string first, string middle, string last)LookupName1(long id) // tuple return type
        {
            var first = "First";
            var middle = "Middle";
            var last = "Last";
            return (first, middle, last) ; // tuple literal
        }

        private class Point
        {
            public int X { get; set; }

            public int Y { get; set; }

            public void GetCoordinates(out int x, out int y)
            {
                x = X;
                y = Y;
            }

            public void Deconstruct(out int x, out int y)
            {
                x = X;
                y = Y;
            }

            public string GetLastName() => throw new NotImplementedException();
        }

        private class Circle : IFigure
        {
            public int Radius { get; set; }
        }

        private class Rectangle : IFigure
        {
            public int Height { get; set; }

            public int Length { get; set; }
        }
    }
}