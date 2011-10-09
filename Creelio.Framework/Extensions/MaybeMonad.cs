using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Creelio.Framework.Extensions
{
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

        public static bool Exists<TInput>(this TInput input)
            where TInput : class
        {
            return input != null;
        }

        public static bool NotExists<TInput>(this TInput input)
            where TInput : class
        {
            return input == null;
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

        public static TInput DoIfExists<TInput>(this TInput input, Action<TInput> action)
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

        public static TInput DoIfNotExists<TInput>(this TInput input, Action<TInput> action)
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

        public static TInput ThrowIfNotExists<TInput>(this TInput input, string errorMsg)
            where TInput : class
        {
            return ThrowIf(input, x => false, errorMsg);
        }

        public static TInput ThrowIfNotExists<TInput>(this TInput input, Exception ex)
            where TInput : class
        {
            return ThrowIf(input, x => false, ex);
        }

        public static TInput ThrowIf<TInput>(this TInput input, Predicate<TInput> evaluator, string errorMsg)
        {
            return ThrowIf(input, evaluator, new Exception(errorMsg ?? "An error occurred"));
        }

        public static TInput ThrowIf<TInput>(this TInput input, Predicate<TInput> evaluator, Exception ex)
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException("evaluator");
            }
            else if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            if (input == null || evaluator(input))
            {
                throw ex;
            }
            else
            {
                return input;
            }
        }
    }
}
