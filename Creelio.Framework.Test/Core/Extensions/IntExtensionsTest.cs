namespace Creelio.Framework.Test.Core.Extensions
{
    using System.Collections.Generic;
    using Creelio.Framework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IntExtensionsTest
    {
        [TestMethod]
        public void UpToShouldIterateFourTimesFromOneToFive()
        {
            var list = new List<int>();
            1.UpTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 4);
        }

        [TestMethod]
        public void UpToShouldIterateFiveTimesFromZeroToFive()
        {
            var list = new List<int>();
            0.UpTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 5);
        }        

        [TestMethod]
        public void DownToShouldIterateFourTimesFromFiveToOne()
        {
            var list = new List<int>();
            5.DownTo(1, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 4);
        }

        [TestMethod]
        public void DownToShouldIterateFiveTimesFromFiveToZero()
        {
            var list = new List<int>();
            5.DownTo(0, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 5);
        }

        [TestMethod]
        public void StepToShouldIterateFourTimesFromOneToFive()
        {
            var list = new List<int>();
            1.StepTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 4);
        }

        [TestMethod]
        public void StepToShouldIterateFiveTimesFromZeroToFive()
        {
            var list = new List<int>();
            0.StepTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 5);
        }

        [TestMethod]
        public void StepToShouldIterateFourTimesFromFiveToOne()
        {
            var list = new List<int>();
            5.StepTo(1, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 4);
        }

        [TestMethod]
        public void StepToShouldIterateFiveTimesFromFiveToZero()
        {
            var list = new List<int>();
            5.StepTo(0, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 5);
        }

        [TestMethod]
        public void ShortCircuitUpToShouldIterateOnlyOnce()
        {
            var list = new List<int>();
            0.UpTo(5, ii => { list.Add(ii); return false; });

            Assert.IsTrue(list.Count == 1);
        }

        [TestMethod]
        public void ShortCircuitDownToShouldIterateOnlyOnce()
        {
            var list = new List<int>();
            5.DownTo(0, ii => { list.Add(ii); return false; });

            Assert.IsTrue(list.Count == 1);
        }

        [TestMethod]
        public void ShortCircuitStepToFromZeroToFiveShouldIterateOnlyOnce()
        {
            var list = new List<int>();
            0.StepTo(5, ii => { list.Add(ii); return false; });

            Assert.IsTrue(list.Count == 1);
        }

        [TestMethod]
        public void ShortCircuitStepToFromFiveToZeroShouldIterateOnlyOnce()
        {
            var list = new List<int>();
            5.StepTo(0, ii => { list.Add(ii); return false; });

            Assert.IsTrue(list.Count == 1);
        }

        [TestMethod]
        public void UpToShouldNotIterateIfMaxIsLessThanBase()
        {
            var list = new List<int>();
            5.UpTo(1, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void UpToShouldNotIterateIfMaxIsEqualToBase()
        {
            var list = new List<int>();
            5.UpTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void DownToShouldNotIterateIfMinIsGreaterThanBase()
        {
            var list = new List<int>();
            1.DownTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void DownToShouldNotIterateIfMinIsEqualToBase()
        {
            var list = new List<int>();
            5.DownTo(5, ii => list.Add(ii));

            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void IsValueEqualShouldMatchIComparableExtension()
        {
            var comparable = "abc123";
            var other = "abc123";
            var compareResult = comparable.CompareTo(other).IsValueEqual();

            Assert.AreEqual(comparable.ValueEquals(other), compareResult);
        }

        [TestMethod]
        public void IsLessThanShouldMatchIComparableExtension()
        {
            var comparable = "abc123";
            var other = "abc124";
            var compareResult = comparable.CompareTo(other).IsLessThan();

            Assert.AreEqual(comparable.IsLessThan(other), compareResult);
        }

        [TestMethod]
        public void IsGreaterThanShouldMatchIComparableExtension()
        {
            var comparable = "abc123";
            var other = "abc122";
            var compareResult = comparable.CompareTo(other).IsGreaterThan();

            Assert.AreEqual(comparable.IsGreaterThan(other), compareResult);
        }

        [TestMethod]
        public void IsLessThanOrEqualToShouldMatchIComparableExtension()
        {
            var comparable = "abc123";
            var other = "abc124";
            var compareResult = comparable.CompareTo(other).IsLessThanOrEqualTo();

            Assert.AreEqual(comparable.IsLessThanOrEqualTo(other), compareResult);
        }

        [TestMethod]
        public void IsGreaterThanOrEqualToShouldMatchIComparableExtension()
        {
            var comparable = "abc123";
            var other = "abc122";
            var compareResult = comparable.CompareTo(other).IsGreaterThanOrEqualTo();

            Assert.AreEqual(comparable.IsGreaterThanOrEqualTo(other), compareResult);
        }
    }
}