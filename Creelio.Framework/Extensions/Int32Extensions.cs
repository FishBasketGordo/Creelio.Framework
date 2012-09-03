namespace Creelio.Framework.Extensions
{
    using System;

    public static class Int32Extensions
    {
        public static void UpTo(this int ii, int max, Action<int> action)
        {
            if (ii < max)
            {
                for (int index = ii; index < max; index++)
                {
                    action(index);
                }
            }
        }

        public static bool UpTo(this int ii, int max, Func<int, bool> shortCircuitAction)
        {
            bool fullIterations = true;

            if (ii < max)
            {
                for (int index = 0; index < max; index++)
                {
                    fullIterations = shortCircuitAction(index);

                    if (!fullIterations)
                    {
                        break;
                    }
                }
            }

            return fullIterations;
        }

        public static void DownTo(this int ii, int min, Action<int> action)
        {
            if (ii >= min)
            {
                for (int index = ii - 1; index >= min; index--)
                {
                    action(index);
                }
            }
        }

        public static bool DownTo(this int ii, int min, Func<int, bool> shortCircuitAction)
        {
            bool fullIterations = true;

            if (ii > min)
            {
                for (int index = ii - 1; index >= min; index--)
                {
                    fullIterations = shortCircuitAction(index);

                    if (!fullIterations)
                    {
                        break;
                    }
                }
            }

            return fullIterations;
        }

        public static void StepTo(this int ii, int limit, Action<int> action)
        {
            if (ii < limit)
            {
                UpTo(ii, limit, action);
            }
            else
            {
                DownTo(ii, limit, action);
            }
        }

        public static bool StepTo(this int ii, int limit, Func<int, bool> shortCircuitAction)
        {
            if (ii < limit)
            {
                return UpTo(ii, limit, shortCircuitAction);
            }
            else
            {
                return DownTo(ii, limit, shortCircuitAction);
            }
        }

        public static bool IsValueEqual(this int ii)
        {
            return ii == 0;
        }

        public static bool IsLessThan(this int ii)
        {
            return ii < 0;
        }

        public static bool IsGreaterThan(this int ii)
        {
            return ii > 0;
        }

        public static bool IsLessThanOrEqualTo(this int ii)
        {
            return IsLessThan(ii) || IsValueEqual(ii);
        }

        public static bool IsGreaterThanOrEqualTo(this int ii)
        {
            return IsGreaterThan(ii) || IsValueEqual(ii);
        }
    }
}