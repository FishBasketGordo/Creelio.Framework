namespace Creelio.Framework.Test.Core
{
    using System;
    using Creelio.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuardTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullPredicateShouldThrow()
        {
            var guard = new Guard<object>();
            guard.AddPredicate(null, o => new Exception());

            Assert.Fail("Should have thrown ArgumentNullException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCreateExceptionShouldThrow()
        {
            var guard = new Guard<object>();
            guard.AddPredicate(o => o != null, null);

            Assert.Fail("Should have thrown ArgumentNullException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTargetShouldThrow()
        {
            var guard = new Guard<object>();
            guard.AddPredicate(o => o != null, o => new ArgumentNullException());

            guard.Evaluate(null);
        }

        [TestMethod]
        public void NoPredicatesShouldReturnTarget()
        {
            var target = new object();
            var guard = new Guard<object>();

            Assert.AreEqual(target, guard.Evaluate(target));
        }

        [TestMethod]
        public void SuccessfulEvaluationShouldReturnTarget()
        {
            var target = new object();
            var guard = new Guard<object>();
            guard.AddPredicate(o => o != null, o => new ArgumentNullException());

            Assert.AreEqual(target, guard.Evaluate(target));
        }
    }
}