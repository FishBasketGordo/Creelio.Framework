namespace Creelio.Framework.Core.Extensions.MaybeMonad
{
    using System;

    /// <summary>
    /// Extension methods used to ensure operations on reference type instances that might be null.
    /// </summary>
    public static class MaybeMonad
    {
        public static TOutput With<TInput, TOutput>(this TInput input, Func<TInput, TOutput> evaluator)
            where TOutput : class
            where TInput : class
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                return evaluator(input);
            }
        }

        public static TOutput Return<TInput, TOutput>(this TInput input, Func<TInput, TOutput> evaluator, TOutput failureValue)
            where TInput : class
        {
            if (input == null)
            {
                return failureValue;
            }
            else
            {
                return evaluator(input);
            }
        }

        public static TInput If<TInput>(this TInput input, Predicate<TInput> evaluator)
            where TInput : class
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                return evaluator(input) ? input : null;
            }
        }

        public static TInput Unless<TInput>(this TInput input, Predicate<TInput> evaluator)
            where TInput : class
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                return evaluator(input) ? null : input;
            }
        }

        public static TInput Do<TInput>(this TInput input, Action<TInput> action)
            where TInput : class
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                action(input);
                return input;
            }
        }

        public static TInput DoIf<TInput>(this TInput input, Predicate<TInput> evaluator, Action<TInput> action)
            where TInput : class
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                if (evaluator(input))
                {
                    action(input);
                }

                return input;
            }
        }

        public static TInput DoIfNull<TInput>(this TInput input, Action<TInput> action)
            where TInput : class
        {
            if (input == null)
            {
                action(input);
                return null;
            }
            else
            {
                return input;
            }
        }

        public static TInput ThrowIfNull<TInput>(this TInput input, string errorMsg)
        {
            return ThrowIf(input, i => i == null, errorMsg);
        }

        public static TInput ThrowIfNull<TInput, TException>(this TInput input, Func<TException> getEx)
            where TException : Exception
        {
            return ThrowIf(input, i => i == null, getEx);
        }

        public static TInput ThrowIfNull<TInput, TException>(this TInput input, Func<TInput, TException> getEx)
            where TException : Exception
        {
            return ThrowIf(input, i => i == null, getEx);
        }

        public static string ThrowIfNullOrEmpty(this string input, string errorMsg)
        {
            return ThrowIf<string>(input, i => string.IsNullOrEmpty(i), errorMsg);
        }

        public static string ThrowIfNullOrEmpty<TException>(this string input, Func<TException> getEx)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrEmpty(i), getEx);
        }

        public static string ThrowIfNullOrEmpty<TException>(this string input, Func<string, TException> getEx)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrEmpty(i), getEx);
        }

        public static string ThrowIfNullOrWhiteSpace(this string input, string errorMsg)
        {
            return ThrowIf<string>(input, i => string.IsNullOrWhiteSpace(i), errorMsg);
        }

        public static string ThrowIfNullOrWhiteSpace<TException>(this string input, Func<TException> getEx)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrWhiteSpace(i), getEx);
        }

        public static string ThrowIfNullOrWhiteSpace<TException>(this string input, Func<string, TException> getEx)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrWhiteSpace(i), getEx);
        }

        public static TInput ThrowIf<TInput>(this TInput input, Predicate<TInput> evaluator, string errorMsg)
        {
            return ThrowIf(input, evaluator, () => new Exception(errorMsg ?? "An error occurred"));
        }

        public static TInput ThrowIf<TInput, TException>(this TInput input, Predicate<TInput> evaluator, Func<TException> getEx)
            where TException : Exception
        {
            return ThrowIf(input, evaluator, i => getEx());
        }

        public static TInput ThrowIf<TInput, TException>(this TInput input, Predicate<TInput> evaluator, Func<TInput, TException> getEx)
            where TException : Exception
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            else if (getEx == null)
            {
                throw new ArgumentNullException("getEx");
            }

            if (evaluator(input))
            {
                throw getEx(input);
            }
            else
            {
                return input;
            }
        }
    }
}