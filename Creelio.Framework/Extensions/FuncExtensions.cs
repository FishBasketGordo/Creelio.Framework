namespace Creelio.Framework.Extensions
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

        public static TResult InvokeOrReturnDefault<TResult>(this Func<TResult> func)
        {
            return func == null ? default(TResult) : func();
        }

        public static TResult InvokeOrReturnDefault<TResult>(this Func<TResult> func, TResult defaultValue)
        {
            return func == null ? defaultValue : func();
        }

        public static TResult InvokeOrReturnDefault<TResult>(this Func<TResult> func, Func<TResult> getDefaultValue)
        {
            return func == null ? getDefaultValue() : func();
        }

        public static TResult Boilerplate<TResult>(this Func<TResult> func, Func<Func<TResult>, TResult> boilerplate)
        {
            return boilerplate(func);
        }

        public static Func<TResult> Apply<T1, TResult>(this Func<T1, TResult> func, T1 arg1)
        {
            return () => func(arg1);
        }

        public static Func<TResult> Apply<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1, T2 arg2)
        {
            return () => func(arg1, arg2);
        }

        public static Func<TResult> Apply<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3)
        {
            return () => func(arg1, arg2, arg3);
        }
    }
}