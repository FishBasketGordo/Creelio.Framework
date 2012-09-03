namespace Creelio.Framework.WebServices.Extensions
{
    using System;
    using System.ServiceModel;
    using Creelio.Framework.Extensions;

    public static class WcfExtensions
    {
        /// <summary>
        /// Ensures that a client object is used correctly, i.e. that it is closed, etc.
        /// </summary>
        public static void Using<TClient>(this TClient client, Action<TClient> callClientMethod)
            where TClient : ICommunicationObject
        {
            Using<TClient, object>(client, c => { callClientMethod(c); return (object)null; });
        }

        /// <summary>
        /// Ensures that a client object is used correctly, i.e. that it is closed, etc.
        /// </summary>
        public static TResponse Using<TClient, TResponse>(this TClient client, Func<TClient, TResponse> callClientMethod)
            where TClient : ICommunicationObject
        {
            client.ThrowIfNull(() => new NullReferenceException("The client object is null."));

            bool success = false;

            try
            {
                var response = callClientMethod(client);
                client.Close();

                success = true;
                return response;
            }
            finally
            {
                if (!success)
                {
                    client.Abort();
                }
            }
        }

        /// <summary>
        /// Guards against exceptions likely to be thrown from a WCF call.
        /// </summary>
        public static TReturn ServiceGuard<T, TReturn>(
            this T extensionObj,
            Func<TReturn> execute,
            Func<TimeoutException, TReturn> timeoutExHandler,
            Func<CommunicationException, TReturn> commExHandler)
        {
            return ServiceGuard<T, TReturn, Exception>(extensionObj, execute, timeoutExHandler, commExHandler, null);
        }

        /// <summary>
        /// Guards against exceptions likely to be thrown from a WCF call, as well as a user-specified exception.
        /// </summary>
        public static TReturn ServiceGuard<T, TReturn, TException1>(
            this T extensionObj,
            Func<TReturn> execute,
            Func<TimeoutException, TReturn> timeoutExHandler,
            Func<CommunicationException, TReturn> commExHandler,
            Func<TException1, TReturn> exceptionHandler1)
            where TException1 : Exception
        {
            return ServiceGuard<T, TReturn, TException1, Exception>(extensionObj, execute, timeoutExHandler, commExHandler, exceptionHandler1, null);
        }

        /// <summary>
        /// Guards against exceptions likely to be thrown from a WCF call, as well as multiple user-specified exceptions.
        /// </summary>
        public static TReturn ServiceGuard<T, TReturn, TException1, TException2>(
            this T extensionObj,
            Func<TReturn> execute,
            Func<TimeoutException, TReturn> timeoutExHandler,
            Func<CommunicationException, TReturn> commExHandler,
            Func<TException1, TReturn> exceptionHandler1,
            Func<TException2, TReturn> exceptionHandler2)
            where TException1 : Exception
            where TException2 : Exception
        {
            return ServiceGuard<T, TReturn, TException1, TException2, Exception>(extensionObj, execute, timeoutExHandler, commExHandler, exceptionHandler1, exceptionHandler2, null);
        }

        /// <summary>
        /// Guards against exceptions likely to be thrown from a WCF call, as well as multiple user-specified exceptions.
        /// </summary>
        public static TReturn ServiceGuard<T, TReturn, TException1, TException2, TException3>(
            this T extensionObj,
            Func<TReturn> execute,
            Func<TimeoutException, TReturn> timeoutExHandler,
            Func<CommunicationException, TReturn> commExHandler,
            Func<TException1, TReturn> exceptionHandler1,
            Func<TException2, TReturn> exceptionHandler2,
            Func<TException3, TReturn> exceptionHandler3)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            try
            {
                return execute();
            }
            catch (TimeoutException ex)
            {
                return timeoutExHandler(ex);
            }
            catch (CommunicationException ex)
            {
                return commExHandler(ex);
            }
            catch (TException1 ex)
            {
                if (exceptionHandler1 == null)
                {
                    throw;
                }
                else
                {
                    return exceptionHandler1(ex);
                }
            }
            catch (TException2 ex)
            {
                if (exceptionHandler2 == null)
                {
                    throw;
                }
                else
                {
                    return exceptionHandler2(ex);
                }
            }
            catch (TException3 ex)
            {
                if (exceptionHandler3 == null)
                {
                    throw;
                }
                else
                {
                    return exceptionHandler3(ex);
                }
            }
        }
    }
}