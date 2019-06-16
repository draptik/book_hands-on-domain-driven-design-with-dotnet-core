using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.ClassifiedAd;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Marketplace.Infrastructure
{
    public class ProjectionsManager
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ProjectionsManager>();
        
        private readonly IEventStoreConnection _connection;
        private readonly IList<ReadModels.ClassifiedAdDetails> _items;
        private EventStoreAllCatchUpSubscription _subscription;

        public ProjectionsManager(IEventStoreConnection connection,
            IList<ReadModels.ClassifiedAdDetails> items)
        {
            _connection = connection;
            _items = items;
        }

        public void Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500,
                Log.IsEnabled(LogEventLevel.Verbose),
                true, "try-out-subscription");

            _subscription = _connection.SubscribeToAllFrom(
                Position.Start, settings, EventAppeared);
        }

        private Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$"))
                return Task.CompletedTask;

            var @event = resolvedEvent.Deserialize();
            Log.Debug("Projecting event {type}", @event.GetType().Name);

            // TODO
            return Task.CompletedTask;
        }

        public void Stop() => _subscription.Stop();
    }
}