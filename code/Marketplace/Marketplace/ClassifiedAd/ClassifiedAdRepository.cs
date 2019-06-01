using System;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAd;
using Raven.Client.Documents.Session;

namespace Marketplace.ClassifiedAd
{
    // RavenDb Repository
    public class ClassifiedAdRepository : IClassifiedAdRepository, IDisposable
    {
        private readonly IAsyncDocumentSession _session;

        public ClassifiedAdRepository(IAsyncDocumentSession session) 
            => _session = session;

        public Task Add(Domain.ClassifiedAd.ClassifiedAd entity)
            => _session.StoreAsync(entity, EntityId(entity.Id));

        public Task<bool> Exists(ClassifiedAdId id)
            => _session.Advanced.ExistsAsync(EntityId(id));

        public Task<Domain.ClassifiedAd.ClassifiedAd> Load(ClassifiedAdId id) 
            => _session.LoadAsync<Domain.ClassifiedAd.ClassifiedAd>(EntityId(id));

        public void Dispose() => _session.Dispose();
        
        private static string EntityId(ClassifiedAdId id) 
            => $"ClassifiedAd/{id}";
    }

    // EF-Core Repository
//    public class ClassifiedAdRepository : IClassifiedAdRepository
//    {
//        private readonly ClassifiedAdDbContext _dbContext;
//
//        public ClassifiedAdRepository(ClassifiedAdDbContext dbContext) 
//            => _dbContext = dbContext;
//
//        public async Task<bool> Exists(ClassifiedAdId id) 
//            => await _dbContext.ClassifiedAds.FindAsync(id.Value) != null;
//
//        public Task<Domain.ClassifiedAd.ClassifiedAd> Load(ClassifiedAdId id) 
//            => _dbContext.ClassifiedAds.FindAsync(id.Value);
//
//        public Task Add(Domain.ClassifiedAd.ClassifiedAd entity) 
//            => _dbContext.ClassifiedAds.AddAsync(entity);
//    }
}