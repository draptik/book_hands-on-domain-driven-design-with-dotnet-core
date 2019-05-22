using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Framework
{
    public abstract class Entity<TId>
        where TId : Value<TId>
    {
        private readonly List<object> _changes;

        protected Entity() => _changes = new List<object>();

        protected abstract void When(object @event);

        protected abstract void EnsureValidState();
        
        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            _changes.Add(@event);
        }

        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

        public void ClearChanges() => _changes.Clear();
    }
}