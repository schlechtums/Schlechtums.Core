using System;
using System.Runtime.CompilerServices;

namespace Schlechtums.Core.Common.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void Raise<T>(this EventHandler<T> eventHandler, Object sender, T e)
   where T : EventArgs
        {
            eventHandler.RaisePrivate(sender, e);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RaisePrivate<T>(this EventHandler<T> eventHandler, Object sender, T e)
            where T : EventArgs
        {
            if (eventHandler != null)
                eventHandler(sender, e);
        }

        public static void RaiseSafe<T>(this Action<T> a, T e)
        {
            if (a != null)
                a(e);
        }
    }
}