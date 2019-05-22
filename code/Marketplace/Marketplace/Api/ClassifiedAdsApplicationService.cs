using System;
using System.Threading.Tasks;
using Marketplace.Domain;
using Marketplace.Framework;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IClassifiedAdRepository _repository;
        private readonly ICurrencyLookup _currencyLookup;

        public ClassifiedAdsApplicationService(
            IClassifiedAdRepository repository,
            ICurrencyLookup currencyLookup)
        {
            _repository = repository;
            _currencyLookup = currencyLookup;
        }

        public Task Handle(object command) =>
            command switch
                {
                V1.Create cmd => HandleCreate(cmd),
                _ => Task.CompletedTask
                };

        private async Task HandleCreate(V1.Create cmd)
        {
            if (await _repository.Exists(cmd.Id.ToString()))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(cmd.Id),
                new UserId(cmd.OwnerId)
            );

            await _repository.Save(classifiedAd);
        }
    }
}