namespace Creelio.Framework.Test.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionExtensionsTest
    {
        [TestMethod]
        public void ApplyWithOneArgShouldBeEqualToManualActionCall()
        {
            var list = new List<int>();
            Action<int> action = i => list.Add(i);

            var arg1 = 0;
            var applied = action.Apply(arg1);

            action(arg1);
            applied();

            Assert.IsTrue(list.Count == 2);
            Assert.AreEqual(list[0], list[1]);
        }

        [TestMethod]
        public void ApplyWithTwoArgsShouldBeEqualToManualActionCall()
        {
            var list = new List<int>();
            Action<int, int> action = (i1, i2) => list.Add(i1 + i2);

            var arg1 = 1;
            var arg2 = 2;
            var applied = action.Apply(arg1, arg2);

            action(arg1, arg2);
            applied();

            Assert.IsTrue(list.Count == 2);
            Assert.AreEqual(list[0], list[1]);
        }

        [TestMethod]
        public void ApplyWithThreeArgsShouldBeEqualToManualActionCall()
        {
            var list = new List<int>();
            Action<int, int, int> action = (i1, i2, i3) => list.Add(i1 + i2 + i3);

            var arg1 = 1;
            var arg2 = 2;
            var arg3 = 3;
            var applied = action.Apply(arg1, arg2, arg3);

            action(arg1, arg2, arg3);
            applied();

            Assert.IsTrue(list.Count == 2);
            Assert.AreEqual(list[0], list[1]);
        }

        [TestMethod]
        public void BoilerPlateShouldConsumeException()
        {
            Action action = () => { throw new Exception(); };
            action.Boilerplate(TestBoilerplate);
        }

        private void TestBoilerplate(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }
    }
}