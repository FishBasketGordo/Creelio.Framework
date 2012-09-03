namespace Creelio.Framework.Test.Core.Extensions
{
    using System;
    using Creelio.Framework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var result = predicate(new object());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AndAlsoShouldReturnFalseIfOneIsFalse()
        {
            Func<object, bool> predicate = o => true;
            predicate = predicate.AndAlso(o => true)
                                 .AndAlso(o => true)
                                 .AndAlso(o => false);

            var result = predicate(new object());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OrElseShouldReturnTrueIfOneIsTrue()
        {
            Func<object, bool> predicate = o => false;
            predicate = predicate.OrElse(o => false)
                                 .OrElse(o => false)
                                 .OrElse(o => true);

            var result = predicate(new object());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OrElseShouldReturnFalseIfAllAreFalse()
        {
            Func<object, bool> predicate = o => false;
            predicate = predicate.OrElse(o => false)
                                 .OrElse(o => false)
                                 .OrElse(o => false);

            var result = predicate(new object());
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ApplyWithOneArgShouldBeEqualToManualFuncCall()
        {
            Func<int, int> func = i => i + 1;

            var arg1 = 0;
            var applied = func.Apply(arg1);

            var expected = func(arg1);
            var actual = applied();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyWithTwoArgsShouldBeEqualToManualFuncCall()
        {
            Func<int, int, int> func = (i1, i2) => i1 + i2;

            var arg1 = 1;
            var arg2 = 2;
            var applied = func.Apply(arg1, arg2);

            var expected = func(arg1, arg2);
            var actual = applied();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyWithThreeArgsShouldBeEqualToManualFuncCall()
        {
            Func<int, int, int, int> func = (i1, i2, i3) => i1 + i2 + i3;

            var arg1 = 1;
            var arg2 = 2;
            var arg3 = 3;
            var applied = func.Apply(arg1, arg2, arg3);

            var expected = func(arg1, arg2, arg3);
            var actual = applied();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BoilerPlateShouldConsumeException()
        {
            Func<int> func = () => { throw new Exception(); };
            func.Boilerplate(TestBoilerplate);            
        }

        private TResult TestBoilerplate<TResult>(Func<TResult> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(TResult);
            }
        }
    }
}