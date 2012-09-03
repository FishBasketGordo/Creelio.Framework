using System;
using System.Collections.Generic;

namespace Creelio.Framework.Extensions
{
    public static class Tree
    {
        public static void Traverse<T>(this T root, Func<T, List<T>> getChildren, Action<T> visitNode)
        {
            Stack<T> stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                T node = stack.Pop();
                visitNode(node);

                List<T> children = getChildren(node);
                if (children != null && children.Count > 0)
                {
                    for (int ii = children.Count - 1; ii >= 0; ii--)
                    {
                        stack.Push(children[ii]);
                    }
                }
            }
        }
    }
}
