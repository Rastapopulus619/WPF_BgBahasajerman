using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_DataAccessLibrary.Services.CommunicationServices.EventAggregators
{
    public class WeakEventSubscription<TEventArgs>
    {
        public WeakReference _targetRef;
        private readonly Action<TEventArgs> _handler;

        public WeakEventSubscription(object target, Action<TEventArgs> handler)
        {
            _targetRef = new WeakReference(target);  // Holds a weak reference to the target
            _handler = handler;                      // Keeps a reference to the handler method
        }

        public void Invoke(object sender, TEventArgs args)
        {
            var target = _targetRef.Target;  // Try to get the actual object from the weak reference
            if (target != null)
            {
                _handler(args);  // If the object still exists, invoke the handler
            }
        }

        public bool IsAlive => _targetRef.IsAlive;  // Indicates if the target is still alive

        public bool HandlerMatches<T>(Action<T> handler)
        {
            return _handler == (Delegate)(object)handler;
        }

        public MethodInfo MethodInfo => _handler.Method;  // Expose method info for logging
    }
}
