namespace Creelio.Framework.Test.Core.Extensions
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Creelio.Framework.Core.Extensions.FuncExtensions;

    [TestClass]
    public class FuncExtensionsTest
    {
        [TestMethod]
        public void AndAlsoShouldReturnTrueIfAllAreTrue()
        {
            Func<object, bool> predicate = o => true;
            predicate = predicate.AndAlso(o => true)
                                 .AndAlso(o => true)
                                 .AndAlso(o => true);

            Assert.IsTrue(predicate(new object()));
        }

        [TestMethod]
        public void AndAlsoShouldReturnFalseIfOneIsFalse()
        {
            Func<object, bool> predicate = o => true;
            predicate = predicate.AndAlso(o => true)
                                 .AndAlso(o => true)
                                 .AndAlso(o => false);

            Assert.IsFalse(predicate(new object()));
        }

        [TestMethod]
        public void OrElseShouldReturnTrueIfOneIsTrue()
        {
            Func<object, bool> predicate = o => false;
            predicate = predicate.OrElse(o => false)
                                 .OrElse(o => false)
                                 .OrElse(o => true);

            Assert.IsTrue(predicate(new object()));
        }

        [TestMethod]
        public void OrElseShouldReturnFalseIfAllAreFalse()
        {
            Func<object, bool> predicate = o => false;
            predicate = predicate.OrElse(o => false)
                                 .OrElse(o => false)
                                 .OrElse(o => false);

            Assert.IsFalse(predicate(new object()));
        }
    }
}