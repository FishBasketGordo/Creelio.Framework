namespace Creelio.Framework.Core.Extensions
{
    using System;
    using System.Linq;

    public static class FuncExtensions
    {
        public static Func<bool> AndAlso(this Func<bool> predicate1, Func<bool> predicate2)
        {
            return () => predicate1() && predicate2();
        }

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

        public static Func<bool> OrElse(this Func<bool> predicate1, Func<bool> predicate2)
        {
            return () => predicate1() || predicate2();
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

        public static TOutput InvokeOrReturnDefault<TOutput>(this Func<TOutput> func)
        {
            return func == null ? default(TOutput) : func();
        }

        public static TOutput InvokeOrReturnDefault<TOutput>(this Func<TOutput> func, TOutput defaultValue)
        {
            return func == null ? defaultValue : func();
        }

        public static TOutput InvokeOrReturnDefault<TOutput>(this Func<TOutput> func, Func<TOutput> getDefaultValue)
        {
            return func == null ? getDefaultValue() : func();
        }
    }
}