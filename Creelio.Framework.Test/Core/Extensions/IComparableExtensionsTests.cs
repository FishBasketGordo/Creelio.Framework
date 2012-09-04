namespace Creelio.Framework.Test.Core.Extensions
{
    using System;
    using Creelio.Framework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IComparableExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CompareTo_NullExtensionObject_Throws()
        {
            object comparable = null;
            object other = new object();

            comparable.CompareToProperties(other, "Property1", "Property2", "Property3");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CompareTo_NullOther_Throws()
        {
            object comparable = new object();
            object other = null;

            comparable.CompareToProperties(other, "Property1", "Property2", "Property3");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CompareTo_NullPropertyNames_Throws()
        {
            object comparable = new object();
            object other = new object();

            comparable.CompareToProperties(other);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CompareTo_EmptyPropertyNames_Throws()
        {
            object comparable = new object();
            object other = new object();
            string[] propertyNames = new string[] { };

            comparable.CompareToProperties(other, propertyNames);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_PropertyNameDoesNotExist_Throws()
        {
            object comparable = new object();
            object other = new object();

            comparable.CompareToProperties(other, "InvalidProperty");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_PropertyNameIsNotComparable_Throws()
        {
            var comparable = new MockClass();
            var other = new MockClass();

            comparable.CompareToProperties(other, "NonComparable");
        }

        [TestMethod]
        public void CompareTo_SinglePropertyName_Equal()
        {
            var comparable = new MockClass { Comparable1 = new Comparable { IntProperty = 0 } };
            var other = new MockClass { Comparable1 = new Comparable { IntProperty = 0 } };

            var result = comparable.CompareToProperties(other, "Comparable1");
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void CompareTo_SinglePropertyName_LessThan()
        {
            var comparable = new MockClass { Comparable1 = new Comparable { IntProperty = 1 } };
            var other = new MockClass { Comparable1 = new Comparable { IntProperty = 2 } };

            var result = comparable.CompareToProperties(other, "Comparable1");
            Assert.IsTrue(result == -1);
        }

        [TestMethod]
        public void CompareTo_SinglePropertyName_GreaterThan()
        {
            var comparable = new MockClass { Comparable1 = new Comparable { IntProperty = 2 } };
            var other = new MockClass { Comparable1 = new Comparable { IntProperty = 1 } };

            var result = comparable.CompareToProperties(other, "Comparable1");
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void CompareTo_SinglePropertyNameGeneric_Equal()
        {
            var comparable = new MockClass { GenericComparable = new GenericComparable { IntProperty = 0 } };
            var other = new MockClass { GenericComparable = new GenericComparable { IntProperty = 0 } };

            var result = comparable.CompareToProperties(other, "GenericComparable");
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void CompareTo_SinglePropertyNameGeneric_LessThan()
        {
            var comparable = new MockClass { GenericComparable = new GenericComparable { IntProperty = 1 } };
            var other = new MockClass { GenericComparable = new GenericComparable { IntProperty = 2 } };

            var result = comparable.CompareToProperties(other, "GenericComparable");
            Assert.IsTrue(result == -1);
        }

        [TestMethod]
        public void CompareTo_SinglePropertyNameGeneric_GreaterThan()
        {
            var comparable = new MockClass { GenericComparable = new GenericComparable { IntProperty = 2 } };
            var other = new MockClass { GenericComparable = new GenericComparable { IntProperty = 1 } };

            var result = comparable.CompareToProperties(other, "GenericComparable");
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNames_Equal()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNamesNonMatchFirst_LessThan()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 1 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 2 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == -1);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNamesNonMatchMiddle_LessThan()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 1 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 2 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == -1);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNamesNonMatchLast_LessThan()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 1 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 2 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == -1);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNamesNonMatchFirst_GreaterThan()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 2 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 1 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNamesNonMatchMiddle_GreaterThan()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 2 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 1 },
                GenericComparable = new GenericComparable { IntProperty = 0 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void CompareTo_MultiplePropertyNamesNonMatchLast_GreaterThan()
        {
            var comparable = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 2 }
            };

            var other = new MockClass
            {
                Comparable1 = new Comparable { IntProperty = 0 },
                Comparable2 = new Comparable { IntProperty = 0 },
                GenericComparable = new GenericComparable { IntProperty = 1 }
            };

            var result = comparable.CompareToProperties(other, "Comparable1", "Comparable2", "GenericComparable");
            Assert.IsTrue(result == 1);
        }

        [TestMethod]
        public void CompareTo_Subproperty_Equal()
        {
            var comparable = new MockClass
            {
                NonComparable = new NonComparable
                {
                    Comparable = new Comparable { IntProperty = 0 },
                    GenericComparable = new GenericComparable { IntProperty = 0 }
                }
            };

            var other = new MockClass
            {
                NonComparable = new NonComparable
                {
                    Comparable = new Comparable { IntProperty = 0 },
                    GenericComparable = new GenericComparable { IntProperty = 0 }
                }
            };

            var result = comparable.CompareToProperties(other, "NonComparable.Comparable", "NonComparable.GenericComparable");
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_SubpropertyNameDoesNotExist_Throws()
        {
            var comparable = new MockClass();
            var other = new MockClass();

            comparable.CompareToProperties(other, "NonComparable.InvalidProperty");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompareTo_SubpropertyNameIsNotComparable_Throws()
        {
            var comparable = new MockClass();
            var other = new MockClass();

            comparable.CompareToProperties(other, "NonComparable.InnerNonComparable");
        }

        [TestMethod]
        public void GreaterThan_Ints_True()
        {
            var comparable = 2;
            var other = 1;

            var result = comparable.IsGreaterThan(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThan_Ints_False()
        {
            var comparable = 1;
            var other = 2;

            var result = comparable.IsGreaterThan(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GreaterThan_Strings_True()
        {
            var comparable = "z";
            var other = "a";

            var result = comparable.IsGreaterThan(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThan_Strings_False()
        {
            var comparable = "a";
            var other = "z";

            var result = comparable.IsGreaterThan(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GreaterThan_GenericComparables_True()
        {
            var comparable = new GenericComparable { IntProperty = 2 };
            var other = new GenericComparable { IntProperty = 1 };

            var result = comparable.IsGreaterThan(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThan_GenericComparables_False()
        {
            var comparable = new GenericComparable { IntProperty = 1 };
            var other = new GenericComparable { IntProperty = 2 };

            var result = comparable.IsGreaterThan(other);
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void GreaterThanOrEqualTo_Ints_True()
        {
            var comparable = 2;
            var other = 1;

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_IntsEqual_True()
        {
            var comparable = 0;
            var other = 0;

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_Ints_False()
        {
            var comparable = 1;
            var other = 2;

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_Strings_True()
        {
            var comparable = "z";
            var other = "a";

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_StringsEqual_True()
        {
            var comparable = "a";
            var other = "a";

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_Strings_False()
        {
            var comparable = "a";
            var other = "z";

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_GenericComparables_True()
        {
            var comparable = new GenericComparable { IntProperty = 2 };
            var other = new GenericComparable { IntProperty = 1 };

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_GenericComparablesEqual_True()
        {
            var comparable = new GenericComparable { IntProperty = 0 };
            var other = new GenericComparable { IntProperty = 0 };

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_GenericComparables_False()
        {
            var comparable = new GenericComparable { IntProperty = 1 };
            var other = new GenericComparable { IntProperty = 2 };

            var result = comparable.IsGreaterThanOrEqualTo(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThan_Ints_True()
        {
            var comparable = 1;
            var other = 2;

            var result = comparable.IsLessThan(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_Ints_False()
        {
            var comparable = 2;
            var other = 1;

            var result = comparable.IsLessThan(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThan_Strings_True()
        {
            var comparable = "a";
            var other = "z";

            var result = comparable.IsLessThan(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_Strings_False()
        {
            var comparable = "z";
            var other = "a";

            var result = comparable.IsLessThan(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThan_GenericComparables_True()
        {
            var comparable = new GenericComparable { IntProperty = 1 };
            var other = new GenericComparable { IntProperty = 2 };

            var result = comparable.IsLessThan(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_GenericComparables_False()
        {
            var comparable = new GenericComparable { IntProperty = 2 };
            var other = new GenericComparable { IntProperty = 1 };

            var result = comparable.IsLessThan(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_Ints_True()
        {
            var comparable = 1;
            var other = 2;

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_IntsEqual_True()
        {
            var comparable = 0;
            var other = 0;

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_Ints_False()
        {
            var comparable = 2;
            var other = 1;

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_Strings_True()
        {
            var comparable = "a";
            var other = "z";

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_StringsEqual_True()
        {
            var comparable = "a";
            var other = "a";

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_Strings_False()
        {
            var comparable = "z";
            var other = "a";

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_GenericComparables_True()
        {
            var comparable = new GenericComparable { IntProperty = 1 };
            var other = new GenericComparable { IntProperty = 2 };

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_GenericComparablesEqual_True()
        {
            var comparable = new GenericComparable { IntProperty = 0 };
            var other = new GenericComparable { IntProperty = 0 };

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThanOrEqualTo_GenericComparables_False()
        {
            var comparable = new GenericComparable { IntProperty = 2 };
            var other = new GenericComparable { IntProperty = 1 };

            var result = comparable.IsLessThanOrEqualTo(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueEquals_Ints_True()
        {
            var comparable = 0;
            var other = 0;

            var result = comparable.ValueEquals(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueEquals_Ints_False()
        {
            var comparable = 1;
            var other = 2;

            var result = comparable.ValueEquals(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueEquals_Strings_True()
        {
            var comparable = "a";
            var other = "a";

            var result = comparable.ValueEquals(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueEquals_Strings_False()
        {
            var comparable = "a";
            var other = "z";

            var result = comparable.ValueEquals(other);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueEquals_GenericComparables_True()
        {
            var comparable = new GenericComparable { IntProperty = 0 };
            var other = new GenericComparable { IntProperty = 0 };

            var result = comparable.ValueEquals(other);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueEquals_GenericComparables_False()
        {
            var comparable = new GenericComparable { IntProperty = 1 };
            var other = new GenericComparable { IntProperty = 2 };

            var result = comparable.ValueEquals(other);
            Assert.IsFalse(result);
        }

        private class Comparable : IComparable
        {
            public int IntProperty { get; set; }

            public int CompareTo(object obj)
            {
                var other = obj as Comparable;
                if (other == null)
                {
                    return -1;
                }
                else
                {
                    return IntProperty.CompareTo(other.IntProperty);
                }
            }
        }

        private class GenericComparable : Comparable, IComparable<Comparable>
        {
            public int CompareTo(Comparable other)
            {
                return IntProperty.CompareTo(other.IntProperty);
            }
        }

        private class NonComparable
        {
            public Comparable Comparable { get; set; }

            public GenericComparable GenericComparable { get; set; }

            public NonComparable InnerNonComparable { get; set; }
        }

        private class MockClass
        {
            public Comparable Comparable1 { get; set; }

            public Comparable Comparable2 { get; set; }

            public GenericComparable GenericComparable { get; set; }

            public NonComparable NonComparable { get; set; }
        }
    }
}