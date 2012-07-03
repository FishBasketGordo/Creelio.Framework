namespace Creelio.Framework.Test.Templating.Sql
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Creelio.Framework.Templating.Sql;

    public abstract class ConstraintTest
    {
        public abstract void ShouldEqualSelf();
        
        public abstract void ShouldEqualClone();

        public abstract void ShouldMatchCloneFormat();

        internal void TestEqualityToSelf(Constraint constraint)
        {
            Assert.IsTrue(constraint.Equals(constraint));
        }

        internal void TestEqualityToClone(Constraint constraint)
        {
            Assert.IsTrue(constraint.Equals(constraint.Clone()));
        }

        internal void TestEqualityToCloneFormat(Constraint constraint)            
        {
            var clone = constraint.Clone();
            Assert.AreEqual(constraint.ToString(), clone.ToString());
        }
    }
}