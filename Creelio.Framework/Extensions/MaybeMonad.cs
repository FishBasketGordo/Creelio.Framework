namespace Creelio.Framework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods used to ensure operations on reference type instances that might be null.
    /// </summary>
    public static class MaybeMonad
    {
        public static TOutput With<TInput, TOutput>(
            this TInput input, 
            Func<TInput, TOutput> evaluator)
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

        public static TOutput Return<TInput, TOutput>(
            this TInput input, 
            Func<TInput, TOutput> evaluator, 
            TOutput failureValue)
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

        public static TInput If<TInput>(
            this TInput input, 
            Predicate<TInput> evaluator)
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

        public static TInput Unless<TInput>(
            this TInput input, 
            Predicate<TInput> evaluator)
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

        public static TInput Do<TInput>(
            this TInput input, 
            Action<TInput> action)
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

        public static TInput DoIf<TInput>(
            this TInput input, 
            Predicate<TInput> evaluator, 
            Action<TInput> action)
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

        public static TInput DoIfNull<TInput>(
            this TInput input, 
            Action<TInput> action)
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

        public static TInput ThrowIfNull<TInput>(
            this TInput input, 
            string errorMessage)
        {
            return ThrowIf(input, i => i == null, errorMessage);
        }

        public static TInput ThrowIfNull<TInput, TException>(
            this TInput input, 
            Func<TException> getException)
            where TException : Exception
        {
            return ThrowIf(input, i => i == null, getException);
        }

        public static TInput ThrowIfNull<TInput, TException>(
            this TInput input, 
            Func<TInput, TException> getException)
            where TException : Exception
        {
            return ThrowIf(input, i => i == null, getException);
        }

        public static IEnumerable<TInput> ThrowIfNullOrEmpty<TInput>(
            this IEnumerable<TInput> input, 
            string errorMessage)
        {
            return ThrowIf<IEnumerable<TInput>>(input, i => i == null || !i.Any(), errorMessage);
        }

        public static IEnumerable<TInput> ThrowIfNullOrEmpty<TInput, TException>(
            this IEnumerable<TInput> input, 
            Func<TException> getException)
            where TException : Exception
        {
            return ThrowIf<IEnumerable<TInput>, TException>(input, i => i == null || !i.Any(), getException);
        }

        public static IEnumerable<TInput> ThrowIfNullOrEmpty<TInput, TException>(
            this IEnumerable<TInput> input,
            Func<IEnumerable<TInput>, TException> getException)
            where TException : Exception
        {
            return ThrowIf<IEnumerable<TInput>, TException>(input, i => i == null || !i.Any(), getException);
        }

        public static string ThrowIfNullOrEmpty(
            this string input, 
            string errorMessage)
        {
            return ThrowIf<string>(input, i => string.IsNullOrEmpty(i), errorMessage);
        }

        public static string ThrowIfNullOrEmpty<TException>(
            this string input, 
            Func<TException> getException)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrEmpty(i), getException);
        }

        public static string ThrowIfNullOrEmpty<TException>(
            this string input, 
            Func<string, TException> getException)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrEmpty(i), getException);
        }

        public static string ThrowIfNullOrWhiteSpace(
            this string input, 
            string errorMessage)
        {
            return ThrowIf<string>(input, i => string.IsNullOrWhiteSpace(i), errorMessage);
        }

        public static string ThrowIfNullOrWhiteSpace<TException>(
            this string input, 
            Func<TException> getException)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrWhiteSpace(i), getException);
        }

        public static string ThrowIfNullOrWhiteSpace<TException>(
            this string input, 
            Func<string, TException> getException)
            where TException : Exception
        {
            return ThrowIf<string, TException>(input, i => string.IsNullOrWhiteSpace(i), getException);
        }

        public static TInput ThrowIf<TInput>(
            this TInput input, 
            Predicate<TInput> evaluator, 
            string errorMessage)
        {
            return ThrowIf(input, evaluator, () => new Exception(errorMessage ?? "An error occurred"));
        }

        public static TInput ThrowIf<TInput, TException>(
            this TInput input, 
            Predicate<TInput> evaluator, 
            Func<TException> getException)
            where TException : Exception
        {
            return ThrowIf(input, evaluator, i => getException());
        }

        public static TInput ThrowIf<TInput, TException>(
            this TInput input, 
            Predicate<TInput> evaluator, 
            Func<TInput, TException> getException)
            where TException : Exception
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            else if (getException == null)
            {
                throw new ArgumentNullException("getException");
            }

            if (evaluator(input))
            {
                throw getException(input);
            }
            else
            {
                return input;
            }
        }
    }
}