namespace Creelio.Framework.Core.Extensions.FuncExtensions
{
    using System;
    using System.Linq;

    public static class FuncExtensions
    {
        public static Func<T1, bool> AndAlso<T1>(this Func<T1, bool> predicate1, Func<T1, bool> predicate2)
        {
            return arg => predicate1(arg) && predicate2(arg);
        }

        public static Func<T1, T2, bool> AndAlso<T1, T2>(this Func<T1, T2, bool> predicate1, Func<T1, T2, bool> predicate2)
        {
            return (arg1, arg2) => predicate1(arg1, arg2) && predicate2(arg1, arg2);
        }

        public static Func<T1, T2, T3, bool> AndAlso<T1, T2, T3>(this Func<T1, T2, T3, bool> predicate1, Func<T1, T2, T3, bool> predicate2)
        {
            return (arg1, arg2, arg3) => predicate1(arg1, arg2, arg3) && predicate2(arg1, arg2, arg3);
        }

        public static Func<T1, bool> OrElse<T1>(this Func<T1, bool> predicate1, Func<T1, bool> predicate2)
        {
            return arg => predicate1(arg) || predicate2(arg);
        }

        public static Func<T1, T2, bool> OrElse<T1, T2>(this Func<T1, T2, bool> predicate1, Func<T1, T2, bool> predicate2)
        {
            return (arg1, arg2) => predicate1(arg1, arg2) || predicate2(arg1, arg2);
        }

        public static Func<T1, T2, T3, bool> OrElse<T1, T2, T3>(this Func<T1, T2, T3, bool> predicate1, Func<T1, T2, T3, bool> predicate2)
        {
            return (arg1, arg2, arg3) => predicate1(arg1, arg2, arg3) || predicate2(arg1, arg2, arg3);
        }
    }
}