namespace Creelio.Framework.Test.Templating.Sql
{
    using Creelio.Framework.Templating.Sql;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class ValueConstraintTest : ConstraintTest
    {
        public abstract void CannotCreateWithNullColumn();

        public abstract void IntConstraintShouldMatchFormat();

        public abstract void FullyQualifiedIntConstraintShouldMatchFormat();

        public abstract void StringConstraintShouldMatchFormat();

        public abstract void FullyQualifiedStringConstraintShouldMatchFormat();

        public virtual void CannotCreateWithColumnConstraintTypeMismatch()
        {
        }
        
        public virtual void NullIntConstraintShouldMatchFormat() 
        {
        }

        public virtual void FullyQualifiedNullIntConstraintShouldMatchFormat() 
        {
        }

        public virtual void NullStringConstraintShouldMatchFormat() 
        {
        }

        public virtual void FullyQualifiedNullStringConstraintShouldMatchFormat() 
        { 
        }
        
        public virtual void ShouldEscapeSingleQuotesInSqlStrings() 
        {
        }

        public virtual void CreatingWithNullValueShouldBeEqualToCreatingWithDBNull()
        {
        }

        public virtual void NullValueFormatShouldMatchDBNullFormat()
        {
        }

        internal void TestFormat(TestConstraints._TestConstraint test)
        {
            var constraint = test.Constraint;
            var result = constraint.ToString();

            Assert.AreEqual(test.Expected, result);
        }
    }
}