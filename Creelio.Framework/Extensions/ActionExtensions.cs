namespace Creelio.Framework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ActionExtensions
    {
        public static Action Boilerplate(this Action action, Action<Action> boilerplate)
        {
            boilerplate(action);
            return action;
        }

        public static Action Apply<T1>(this Action<T1> action, T1 arg1)
        {
            return () => action(arg1);
        }

        public static Action Apply<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            return () => action(arg1, arg2);
        }

        public static Action Apply<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return () => action(arg1, arg2, arg3);
        }
    }
}