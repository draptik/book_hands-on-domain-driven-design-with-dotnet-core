using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(ClassifiedAdId id);

        Task<ClassifiedAd> Load(ClassifiedAdId id);
        
        Task Add(ClassifiedAd entity);
    }
}