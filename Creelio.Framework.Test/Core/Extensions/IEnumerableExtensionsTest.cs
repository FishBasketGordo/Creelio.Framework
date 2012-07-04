namespace Creelio.Framework.Test.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Core.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IEnumerableExtensionsTest
    {
        private static readonly IEnumerable<int> TestSequence = new List<int> { 1, 2, 3, 4, 5 };

        private static readonly int TestSequenceCount = TestSequence.Count();

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
            var result = TestSequence.TakeLast(0);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TakeLastShouldReturnLastIfCountIsOne()
        {
            var result = TestSequence.TakeLast(1);
            Assert.IsTrue(result.Single() == TestSequence.Last());
        }

        [TestMethod]
        public void TakeLastShouldReturnLastTwoIfCountIsTwo()
        {
            var result = TestSequence.TakeLast(2);            
            var count = result.Count();

            Assert.IsTrue(count == 2);
            Assert.IsTrue(result.ElementAt(count - 1) == TestElementAt(-1));
            Assert.IsTrue(result.ElementAt(count - 2) == TestElementAt(-2));
        }

        [TestMethod]
        public void TakeLastShouldReturnWholeSequenceIfCountIsSequenceCount()
        {
            var result = TestSequence.TakeLast(TestSequenceCount);
            var count = result.Count();

            Assert.IsTrue(count == TestSequenceCount);
            0.UpTo(count, ii => Assert.AreEqual(TestElementAt(ii), result.ElementAt(ii)));
        }

        [TestMethod]
        public void TakeLastShouldReturnWholeSequenceIfCountIsGreaterThanSequenceCount()
        {
            var result = TestSequence.TakeLast(TestSequenceCount + 1);
            var count = result.Count();

            Assert.IsTrue(count == TestSequenceCount);
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
            var result = TestSequence.ToString(null);
            Assert.AreEqual("12345", result);
        }

        [TestMethod]
        public void ToStringWithEmptySeparatorShouldMatchFormat()
        {
            var result = TestSequence.ToString(string.Empty);
            Assert.AreEqual("12345", result);
        }

        [TestMethod]
        public void ToStringWithSemicolonSeparatorShouldMatchFormat()
        {
            var result = TestSequence.ToString(";");
            Assert.AreEqual("1;2;3;4;5", result);
        }

        [TestMethod]
        public void ToStringWithExplicitToStringAndNullSeparatorShouldMatchFormat()
        {
            var result = TestSequence.ToString(null, ii => (ii + 1).ToString());
            Assert.AreEqual("23456", result);
        }

        [TestMethod]
        public void ToStringWithExplicitToStringAndEmptySeparatorShouldMatchFormat()
        {
            var result = TestSequence.ToString(string.Empty, ii => (ii + 1).ToString());
            Assert.AreEqual("23456", result);
        }

        [TestMethod]
        public void ToStringWithExplicitToStringAndSemicolonSeparatorShouldMatchFormat()
        {
            var result = TestSequence.ToString(";", ii => (ii + 1).ToString());
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
            var result = TestSequence.ToCsv();
            Assert.AreEqual("1,2,3,4,5", result);
        }

        [TestMethod]
        public void ToCsvWithSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(true);
            Assert.AreEqual("1, 2, 3, 4, 5", result);
        }

        [TestMethod]
        public void ToCsvWithoutSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(false);
            Assert.AreEqual("1,2,3,4,5", result);
        }

        [TestMethod]
        public void ToCsvWithNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToCsv((string)null);
            Assert.AreEqual("1, 2, 3, 4, 5", result);
        }

        [TestMethod]
        public void ToCsvWithEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(string.Empty);
            Assert.AreEqual("1, 2, 3, 4, 5", result);
        }

        [TestMethod]
        public void ToCsvWithAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToCsv("and");
            Assert.AreEqual("1, 2, 3, 4, and 5", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(ii => (ii + 1).ToString());
            Assert.AreEqual("2,3,4,5,6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(true, ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, 6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndNoSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(false, ii => (ii + 1).ToString());
            Assert.AreEqual("2,3,4,5,6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(null, ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, 6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToCsv(string.Empty, ii => (ii + 1).ToString());
            Assert.AreEqual("2, 3, 4, 5, 6", result);
        }

        [TestMethod]
        public void ToCsvWithExplicitToStringAndAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToCsv("and", ii => (ii + 1).ToString());
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
            var result = TestSequence.ToSingleQuotedCsv();
            Assert.AreEqual("'1','2','3','4','5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(true);
            Assert.AreEqual("'1', '2', '3', '4', '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithoutSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(false);
            Assert.AreEqual("'1','2','3','4','5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv((string)null);
            Assert.AreEqual("'1', '2', '3', '4', '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(string.Empty);
            Assert.AreEqual("'1', '2', '3', '4', '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv("and");
            Assert.AreEqual("'1', '2', '3', '4', and '5'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(ii => (ii + 1).ToString());
            Assert.AreEqual("'2','3','4','5','6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(true, ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', '6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndNoSpaceAfterCommaShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(false, ii => (ii + 1).ToString());
            Assert.AreEqual("'2','3','4','5','6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndNullConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(null, ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', '6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndEmptyConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv(string.Empty, ii => (ii + 1).ToString());
            Assert.AreEqual("'2', '3', '4', '5', '6'", result);
        }

        [TestMethod]
        public void ToSingleQuotedCsvWithExplicitToStringAndAndConjunctionShouldMatchFormat()
        {
            var result = TestSequence.ToSingleQuotedCsv("and", ii => (ii + 1).ToString());
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
        
        private int TestElementAt(int index)
        {
            if (index < 0)
            {
                return TestSequence.ElementAt(TestSequenceCount + index);
            }
            else
            {
                return TestSequence.ElementAt(index);
            }
        }
    }
}