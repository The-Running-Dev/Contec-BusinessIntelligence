using System;
using System.Threading;

namespace Contec.Framework.Extensions
{
    public static class ActionExtensions
    {
        #region InvokeWithTimeout(Action action, int msTimeout)

        public static void InvokeWithTimeout(this Action action, int msTimeout)
        {
            //-------------------------------------------------------------------------------------
            // Setup the wrapped action that we will us to run the given action in asynchronously.
            Thread actionThread = null;

            Action wrappedAction = () =>
            {
                actionThread = Thread.CurrentThread;
                action();
            };

            //-------------------------------------------------------------------------------------
            // Invoke the wrapped action asynchronously and wait for the given amount of time.
            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (((msTimeout != -1) && !result.IsCompleted) &&
                (!result.AsyncWaitHandle.WaitOne(msTimeout, false) || !result.IsCompleted))
            {
                //---------------------------------------------------------------------------------
                // Stop the action's thread since it has exceed the given timeout.
                if (actionThread != null)
                    actionThread.Abort();

                //---------------------------------------------------------------------------------
                // Throw a TimeoutException so the caller knows that the action timeout.
                throw new TimeoutException();
            }
            else
            {
                //---------------------------------------------------------------------------------
                // If we get here, the action either completed before the given timeout or we
                // weren't given a timeout value.
                action.EndInvoke(result);
            }
        }

        #endregion
    }
}