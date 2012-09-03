namespace Creelio.Framework
{
    using System;
    using System.Collections.Generic;
    using Creelio.Framework.Extensions;

    public class Guard<T>
    {
        private Lazy<List<GuardPredicate<T>>> _predicates = 
            new Lazy<List<GuardPredicate<T>>>(() => new List<GuardPredicate<T>>());

        public Guard()
        {
        }

        private List<GuardPredicate<T>> Predicates
        {
            get
            {
                return _predicates.Value;
            }
        }

        public Guard<T> AddPredicate(Predicate<T> predicate, Func<T, Exception> createException)
        {
            Predicates.Add(new GuardPredicate<T>(predicate, createException));
            return this;
        }

        public Guard<T> ClearPredicates()
        {
            Predicates.Clear();
            return this;
        }

        public T Evaluate(T target)
        {
            foreach (var predicate in Predicates)
            {
                if (!predicate.Predicate(target))
                {
                    throw predicate.CreateException(target);
                }
            }

            return target;
        }

        private class GuardPredicate<TGuard>
        {
            public GuardPredicate(Predicate<TGuard> predicate, Func<TGuard, Exception> createException)
            {
                predicate.ThrowIfNull(
                    _ => new ArgumentNullException("predicate"));

                createException.ThrowIfNull(
                    _ => new ArgumentNullException("createException"));

                Predicate = predicate;
                CreateException = createException;
            }

            public Predicate<TGuard> Predicate { get; set; }

            public Func<TGuard, Exception> CreateException { get; set; }
        }
    }
}