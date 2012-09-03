namespace Creelio.Framework.Test.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.UnitTesting;
    using Creelio.Framework.UnitTesting.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NavigationItemTests : ISerializationTests, IDataContractSerializationTests
    {
        [TestMethod]
        public void DeserializedObjectShouldBeEqualToOriginal()
        {
            var navItem = CreateNavItemWithParentAndChild();
            var comparer = new NavigationItemEqualityComparer();
            DtoAssert.DeserializedObjectIsEqualToOriginal(navItem, comparer);
        }

        [TestMethod]
        public void DataContractDeserializedObjectShouldEqualOriginal()
        {
            var navItem = CreateNavItemWithParentAndChild();
            var comparer = new NavigationItemEqualityComparer();
            DtoAssert.DataContractDeserializedObjectIsEqualToOriginal(navItem, comparer);
        }

        [TestMethod]
        public void CountPropertyShouldEqualChildrenCount()
        {
            var navItem = CreateNavItemWithParentAndChild();
            Assert.IsTrue(navItem.Count == navItem.Children.Count(), "The count property does not equal the actual number of objects in Children.");
        }

        [TestMethod]
        public void SettingChildrenPropertyToNullShouldEmptyIt()
        {
            var navItem = CreateNavItemWithParentAndChild();
            navItem.Children = null;
            Assert.IsTrue(navItem.Children != null && navItem.Count == 0, "The Children property should be empty after setting it to null, but instead it is null or non-empty.");
        }

        [TestMethod]
        public void ParentObjectCountShouldBeAtLeastOne()
        {
            var navItem = CreateNavItemWithParentAndChild();
            Assert.IsTrue(navItem.Parent.Count > 0, "The Parent object's Count is less than one.");
        }

        [TestMethod]
        public void ChildrenObjectsParentPropertyShouldNotBeNull()
        {
            var navItem = CreateNavItemWithParentAndChild();
            var result = true;

            foreach (var child in navItem)
            {
                result &= child.Parent != null;

                if (!result)
                {
                    break;
                }
            }

            Assert.IsTrue(result, "At least one of the Children objects does not have its Parent property set.");
        }

        [TestMethod]
        public void CompareToWithNullShouldReturnGreaterThan()
        {
            var navItem = new NavigationItem { Text = "Test" };
            var result = navItem.CompareTo(null);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareToWithOtherHavingNullTextShouldReturnGreaterThan()
        {
            var navItem = new NavigationItem { Text = "Test" };
            var otherItem = new NavigationItem { Text = null };
            var result = navItem.CompareTo(otherItem);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareToWithOtherHavingLesserTextShouldReturnGreaterThan()
        {
            var navItem = new NavigationItem { Text = "TestB" };
            var otherItem = new NavigationItem { Text = "TestA" };
            var result = navItem.CompareTo(otherItem);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareToWithOtherHavingGreaterTextShouldReturnLessThan()
        {
            var navItem = new NavigationItem { Text = "TestA" };
            var otherItem = new NavigationItem { Text = "TestB" };
            var result = navItem.CompareTo(otherItem);
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareToWithOtherHavingEqualTextShouldReturnEqual()
        {
            var navItem = new NavigationItem { Text = "Test" };
            var otherItem = new NavigationItem { Text = "Test" };
            var result = navItem.CompareTo(otherItem);
            Assert.IsTrue(result == 0);
        }

        private static NavigationItem CreateNavItemWithParentAndChild()
        {
            var navItem = new NavigationItem
            {
                Text = "Test",
                Url = "http://www.dummysite.com/Test.html",
                IconUrl = "http://www.dummysite.com/Images/Test.gif",
            };

            navItem.Parent = new NavigationItem
            {
                Text = "Parent",
                Url = "http://www.dummysite.com/Parent.html",
                IconUrl = "http://www.dummysite.com/Images/Parent.gif",
                Children = new List<NavigationItem>
                {
                    navItem,
                }
            };

            navItem.Add(
                new NavigationItem
                {
                    Text = "Child1",
                    Url = "http://www.dummysite.com/Child1.html",
                    IconUrl = "http://www.dummysite.com/Images/Child1.gif",
                    Parent = navItem
                });

            navItem.Add(
                new NavigationItem
                {
                    Text = "Child2",
                    Url = "http://www.dummysite.com/Child2.html",
                    IconUrl = "http://www.dummysite.com/Images/Child2.gif",
                    Parent = navItem
                });

            return navItem;
        }

        private class NavigationItemEqualityComparer : IEqualityComparer<NavigationItem>
        {
            public bool Equals(NavigationItem navItem, NavigationItem otherNavItem)
            {
                return Equals(navItem, otherNavItem, true, true);
            }

            public int GetHashCode(NavigationItem obj)
            {
                return 0;
            }

            private static bool Equals(NavigationItem navItem, NavigationItem otherNavItem, bool compareParentsProperties, bool compareChildrensProperties)
            {
                if (navItem == null || otherNavItem == null)
                {
                    return navItem == null && otherNavItem == null;
                }

                var result = true;
                result &= navItem.Text.ValueEquals(otherNavItem.Text);
                result &= navItem.Url.ValueEquals(otherNavItem.Url);
                result &= navItem.IconUrl.ValueEquals(otherNavItem.IconUrl);
                result &= CompareParents(navItem, otherNavItem, compareParentsProperties, result);
                result &= CompareChildren(navItem, otherNavItem, compareChildrensProperties, result);

                return result;
            }

            private static bool CompareParents(NavigationItem navItem, NavigationItem otherNavItem, bool compareParentsProperties, bool result)
            {
                var bothHaveParents = navItem.Parent != null && otherNavItem.Parent != null;
                var neitherHaveParents = navItem.Parent == null && otherNavItem.Parent == null;

                result &= bothHaveParents || neitherHaveParents;

                if (result && compareParentsProperties)
                {
                    result &= Equals(navItem.Parent, otherNavItem.Parent, false, false);
                }

                return result;
            }

            private static bool CompareChildren(NavigationItem navItem, NavigationItem otherNavItem, bool compareChildrensProperties, bool result)
            {
                result &= navItem.Count.ValueEquals(otherNavItem.Count);

                if (result && compareChildrensProperties)
                {
                    for (int ii = 0; ii < navItem.Count; ii++)
                    {
                        result &= Equals(navItem.Children.ElementAt(ii), otherNavItem.Children.ElementAt(ii), false, false);

                        if (!result)
                        {
                            break;
                        }
                    }
                }

                return result;
            }
        }
    }
}