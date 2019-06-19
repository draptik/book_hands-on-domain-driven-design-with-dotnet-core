using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Marketplace.Framework;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Marketplace.Infrastructure
{
    public class ProjectionManager
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<ProjectionManager>();
        
        private readonly IEventStoreConnection _connection;
        private readonly ICheckpointStore _checkpointStore;
        private readonly IProjection[] _projections;
        private EventStoreAllCatchUpSubscription _subscription;

        public ProjectionManager(
            IEventStoreConnection connection,
            ICheckpointStore checkpointStore,
            params IProjection[] projections)
        {
            _connection = connection;
            _checkpointStore = checkpointStore;
            _projections = projections;
        }

        public async Task Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500,
                Log.IsEnabled(LogEventLevel.Verbose),
                false, "try-out-subscription");

            var position = await _checkpointStore.GetCheckpoint();
            _subscription = _connection.SubscribeToAllFrom(position, settings, EventAppeared);
        }

        private async Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (resolvedEvent.Event.EventType.StartsWith("$"))
                return;

            var @event = resolvedEvent.Deserialize();
            Log.Debug("Projecting event {type}", @event.GetType().Name);

            await Task.WhenAll(_projections.Select(x => x.Project(@event)));

            await _checkpointStore.StoreCheckpoint(resolvedEvent.OriginalPosition.Value);
        }

        public void Stop() => _subscription.Stop();
    }
}