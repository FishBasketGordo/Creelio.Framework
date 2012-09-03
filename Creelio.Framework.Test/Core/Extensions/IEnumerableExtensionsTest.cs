namespace Creelio.Framework.Test.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IEnumerableExtensionsTest
    {
        private static readonly IEnumerable<int> TestSequence1 = new List<int> { 1, 2, 3, 4, 5 };

        private static readonly int TestSequence1Count = TestSequence1.Count();

        private static readonly IEnumerable<string> TestSequence2 = new List<string> { "A", "B", "C", "D", "E", "F" };

        private static readonly int TestSequence2Count = TestSequence2.Count();

        private static readonly IEnumerable<char> TestSequence3 = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

        private static readonly int TestSequence3Count = TestSequence3.Count();

        private static readonly IEnumerable<double> TestSequence4 = new List<double> { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0 };

        private static readonly int TestSequence4Count = TestSequence4.Count();

        private static readonly IEnumerable<decimal> TestSequence5 = new List<decimal> { 1M, 2M, 3M, 4M, 5M, 6M, 7M, 8M, 9M };

        private static readonly int TestSequence5Count = TestSequence5.Count();

        [TestMethod]
        public void ToListElementsShouldMatchOriginalElements()
        {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var list = (array as IEnumerable).ToList<int>();

            0.UpTo(array.Length, ii => Assert.AreEqual(array[ii], list[ii]));
        }

        [TestMethod]
        public void TakeLastShouldReturnEmptySequenceIfCountLessThanOne()
        {
            var result = TestSequence1.TakeLast(0);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TakeLastShouldReturnLastIfCountIsOne()
        {
            var result = TestSequence1.TakeLast(1);
            Assert.IsTrue(result.Single() == TestSequence1.Last());
        }

        [TestMethod]
        public void TakeLastShouldReturnLastTwoIfCountIsTwo()
        {
            var result = TestSequence1.TakeLast(2);            
            var count = result.Count();

            Assert.IsTrue(count == 2);
            Assert.IsTrue(result.ElementAt(count - 1) == TestElementAt(-1));
            Assert.IsTrue(result.ElementAt(count - 2) == TestElementAt(-2));
        }

        [TestMethod]
        public void TakeLastShouldReturnWholeSequenceIfCountIsSequenceCount()
        {
            var result = TestSequence1.TakeLast(TestSequence1Count);
            var count = result.Count();

            Assert.IsTrue(count == TestSequence1Count);
            0.UpTo(count, ii => Assert.AreEqual(TestElementAt(ii), result.ElementAt(ii)));
        }

        [TestMethod]
        public void TakeLastShouldReturnWholeSequenceIfCountIsGreaterThanSequenceCount()
        {
            var result = TestSequence1.TakeLast(TestSequence1Count + 1);
            var count = result.Count();

            Assert.IsTrue(count == TestSequence1Count);
            0.UpTo(count, ii => Assert.AreEqual(TestElementAt(ii), result.ElementAt(ii)));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void NullToStringShouldThrow()
        {
            GC.KeepAlive((null as IEnumerable<int>).ToString());
        }

        [TestMethod]
        public void ToStringWithNullSeparatorShouldMatchFormat()
        {
            var result = TestSequence1.ToString(null);
            Assert.AreEqual("12345", result);
        }

        [TestMethod]
        public void ToStringWithEmptySeparatorShouldMatchFormat()
        {
            var result = TestSequence1.ToString(string.Empty);
            Assert.AreEqual("12345", result);
        }

        [TestMethod]
        public void ToStringWithSemicolonSeparatorShouldMatchFormat()
        {
            var result = TestSequence1.ToString(";");
            Assert.AreEqual("1;2;3;4;5", result);
        }

        [TestMethod]
        public void ToStringWithExplicitToStringAndNullSeparatorShouldMatchFormat()
        {
            var result = TestSequence1.ToString(null, ii => (ii + 1).ToString());
            Assert.AreEqual("23456", result);
        }

        [TestMethod]
        public void ToStringWithExplicitToStringAndEmptySeparatorShouldMatchFormat()
        {
            var result = TestSequence1.ToString(string.Empty, ii => (ii + 1).ToString());
            Assert.AreEqual("23456", result);
        }

        [TestMethod]
        public void ToStringWithExplicitToStringAndSemicolonSeparatorShouldMatchFormat()
        {
            var result = TestSequence1.ToString(";", ii => (ii + 1).ToString());
            Assert.AreEqual("2;3;4;5;6", result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void NullToCsvShouldThrow()
        {
            GC.KeepAlive((null as IEnumerable<int>).ToCsv());
        }

        [TestMethod]
        public void ToCsvShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv();
            Assert.AreEqual("1,2,3,4,5", result);
        }

        [TestMethod]
        public void ToCsvWithSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(true);
            Assert.AreEqual("1, 2, 3, 4, 5", result);
        }

        [TestMethod]
        public void ToCsvWithoutSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(false);
            Assert.AreEqual("1,2,3,4,5", result);
        }

        [TestMethod]
        public void ToCsvWithNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv((string)null);
            Assert.AreEqual("1, 2, 3, 4, 5", result);
        }

        [TestMethod]
        public void ToCsvWithEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(string.Empty);
            Assert.AreEqual("1, 2, 3, 4, 5", result);
        }

        [TestMethod]
        public void ToCsvWithAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv("and");
            Assert.AreEqual("1, 2, 3, 4, and 5", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(ii => (ii + 1).ToString());
            Assert.AreEqual("2,3,4,5,6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(true, ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, 6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndNoSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(false, ii => (ii + 1).ToString());
            Assert.AreEqual("2,3,4,5,6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(null, ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, 6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv(string.Empty, ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, 6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToCsv("and", ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, and 6", result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void NullToSingleQuotedCsvShouldThrow()
        {
            GC.KeepAlive((null as IEnumerable<int>).ToSingleQuotedCsv());
        }

        [TestMethod]
        public void ToSingleQuotedCsvShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv();
            Assert.AreEqual("'1','2','3','4','5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(true);
            Assert.AreEqual("'1', '2', '3', '4', '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithoutSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(false);
            Assert.AreEqual("'1','2','3','4','5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv((string)null);
            Assert.AreEqual("'1', '2', '3', '4', '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(string.Empty);
            Assert.AreEqual("'1', '2', '3', '4', '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv("and");
            Assert.AreEqual("'1', '2', '3', '4', and '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(ii => (ii + 1).ToString());
            Assert.AreEqual("'2','3','4','5','6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(true, ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', '6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndNoSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(false, ii => (ii + 1).ToString());
            Assert.AreEqual("'2','3','4','5','6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(null, ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', '6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv(string.Empty, ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', '6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence1.ToSingleQuotedCsv("and", ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', and '6'", result);
        }

        [TestMethod]
        public void FromStringShouldMatchSequence()
        {
            var result = "1;2;3;4;5".FromString(";", s => int.Parse(s));
            var count = result.Count();

            Assert.IsTrue(count == 5);
            0.UpTo(count, ii => Assert.AreEqual(TestElementAt(ii), result.ElementAt(ii)));
        }

        [TestMethod]
        public void FromStringWithEmptyItemsShouldMatchSequence()
        {
            var result = "1;;2;;3;;4;;5".FromString(";", s => int.Parse(s), StringSplitOptions.RemoveEmptyEntries);
            var count = result.Count();

            Assert.IsTrue(count == 5);
            0.UpTo(count, ii => Assert.AreEqual(TestElementAt(ii), result.ElementAt(ii)));
        }

        [TestMethod]
        public void CombineItemsCountWithTwoSequencesShouldEqualMinimum()
        {
            var combinedSequence = TestSequence1.CombineItems(TestSequence2);

            var count = GetMinimum(
                TestSequence1Count,
                TestSequence2Count);

            Assert.AreEqual(count, combinedSequence.Count());
        }

        [TestMethod]
        public void CombineItemsCountWithThreeSequencesShouldEqualMinimum()
        {
            var combinedSequence = TestSequence1.CombineItems(
                TestSequence2,
                TestSequence3);

            var count = GetMinimum(
                TestSequence1Count,
                TestSequence2Count,
                TestSequence3Count);

            Assert.AreEqual(count, combinedSequence.Count());
        }

        [TestMethod]
        public void CombineItemsCountWithFourSequencesShouldEqualMinimum()
        {
            var combinedSequence = TestSequence1.CombineItems(
                TestSequence2,
                TestSequence3,
                TestSequence4);

            var count = GetMinimum(
                TestSequence1Count,
                TestSequence2Count,
                TestSequence3Count,
                TestSequence4Count);

            Assert.AreEqual(count, combinedSequence.Count());
        }

        [TestMethod]
        public void CombineItemsCountWithFiveSequencesShouldEqualMinimum()
        {
            var combinedSequence = TestSequence1.CombineItems(
                TestSequence2,
                TestSequence3,
                TestSequence4,
                TestSequence5);

            var count = GetMinimum(
                TestSequence1Count,
                TestSequence2Count,
                TestSequence3Count,
                TestSequence4Count,
                TestSequence5Count);

            Assert.AreEqual(count, combinedSequence.Count());
        }

        [TestMethod]
        public void CombineItemsWithTwoSequencesShouldMatchItems()
        {
            var combinedSequence = TestSequence1.CombineItems(TestSequence2);

            var count = GetMinimum(
                TestSequence1Count, 
                TestSequence2Count);

            for (int ii = 0; ii < count; ii++)
            {
                Assert.AreEqual(TestElementAt(ii, TestSequence1), combinedSequence.ElementAt(ii).Item1);
                Assert.AreEqual(TestElementAt(ii, TestSequence2), combinedSequence.ElementAt(ii).Item2);
            }
        }

        [TestMethod]
        public void CombineItemsWithThreeSequencesShouldMatchItems()
        {
            var combinedSequence = TestSequence1.CombineItems(
                TestSequence2, 
                TestSequence3);

            var count = GetMinimum(
                TestSequence1Count, 
                TestSequence2Count, 
                TestSequence3Count);

            for (int ii = 0; ii < count; ii++)
            {
                Assert.AreEqual(TestElementAt(ii, TestSequence1), combinedSequence.ElementAt(ii).Item1);
                Assert.AreEqual(TestElementAt(ii, TestSequence2), combinedSequence.ElementAt(ii).Item2);
                Assert.AreEqual(TestElementAt(ii, TestSequence3), combinedSequence.ElementAt(ii).Item3);
            }
        }

        [TestMethod]
        public void CombineItemsWithFourSequencesShouldMatchItems()
        {
            var combinedSequence = TestSequence1.CombineItems(
                TestSequence2, 
                TestSequence3, 
                TestSequence4);

            var count = GetMinimum(
                TestSequence1Count, 
                TestSequence2Count, 
                TestSequence3Count, 
                TestSequence4Count);

            for (int ii = 0; ii < count; ii++)
            {
                Assert.AreEqual(TestElementAt(ii, TestSequence1), combinedSequence.ElementAt(ii).Item1);
                Assert.AreEqual(TestElementAt(ii, TestSequence2), combinedSequence.ElementAt(ii).Item2);
                Assert.AreEqual(TestElementAt(ii, TestSequence3), combinedSequence.ElementAt(ii).Item3);
                Assert.AreEqual(TestElementAt(ii, TestSequence4), combinedSequence.ElementAt(ii).Item4);
            }
        }

        [TestMethod]
        public void CombineItemsWithFiveSequencesShouldMatchItems()
        {
            var combinedSequence = TestSequence1.CombineItems(
                TestSequence2, 
                TestSequence3, 
                TestSequence4,
                TestSequence5);

            var count = GetMinimum(
                TestSequence1Count, 
                TestSequence2Count, 
                TestSequence3Count, 
                TestSequence4Count,
                TestSequence5Count);

            for (int ii = 0; ii < count; ii++)
            {
                Assert.AreEqual(TestElementAt(ii, TestSequence1), combinedSequence.ElementAt(ii).Item1);
                Assert.AreEqual(TestElementAt(ii, TestSequence2), combinedSequence.ElementAt(ii).Item2);
                Assert.AreEqual(TestElementAt(ii, TestSequence3), combinedSequence.ElementAt(ii).Item3);
                Assert.AreEqual(TestElementAt(ii, TestSequence4), combinedSequence.ElementAt(ii).Item4);
                Assert.AreEqual(TestElementAt(ii, TestSequence5), combinedSequence.ElementAt(ii).Item5);
            }
        }
        
        private int TestElementAt(int index)
        {
            return TestElementAt<int>(index, TestSequence1);
        }

        private T TestElementAt<T>(int index, IEnumerable<T> testSequence)
        {
            if (index < 0)
            {
                return testSequence.ElementAt(testSequence.Count() + index);
            }
            else
            {
                return testSequence.ElementAt(index);
            }
        }

        private int GetMinimum(params int[] ints)
        {
            var minimum = int.MaxValue;
            foreach (var ii in ints)
            {
                minimum = Math.Min(minimum, ii);
            }

            return minimum;
        }
    }
}