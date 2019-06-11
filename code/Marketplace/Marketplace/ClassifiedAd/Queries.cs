//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Queries;
//using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using static Marketplace.ClassifiedAd.QueryModels;
using static Marketplace.ClassifiedAd.ReadModels;
using static Marketplace.Domain.ClassifiedAd.ClassifiedAd;

namespace Marketplace.ClassifiedAd
{
    public static class Queries
    {
        // RavenDb ==========================================
//        public static Task<List<PublishedClassifiedAdListItem>> Query(
//            this IAsyncDocumentSession session,
//            GetPublishedClassifiedAds query)
//            =>
//                session.Query<Domain.ClassifiedAd.ClassifiedAd>()
//                    .Where(x => x.State == ClassifiedAdState.Active)
//                    .Select(
//                        x =>
//                            new PublishedClassifiedAdListItem
//                            {
//                                ClassifiedAdId = x.Id.Value,
//                                Price = x.Price.Amount,
//                                Title = x.Title.Value,
//                                CurrencyCode = x.Price.Currency.CurrencyCode
//                            }
//                    )
//                    .PagedList(query.Page, query.PageSize);
//
//        public static Task<List<PublishedClassifiedAdListItem>> Query(
//            this IAsyncDocumentSession session,
//            GetOwnersClassifiedAd query)
//            =>
//                session.Query<Domain.ClassifiedAd.ClassifiedAd>()
//                    .Where(x => x.OwnerId.Value == query.OwnerId)
//                    .Select(
//                        x =>
//                            new PublishedClassifiedAdListItem
//                            {
//                                ClassifiedAdId = x.Id.Value,
//                                Price = x.Price.Amount,
//                                Title = x.Title.Value,
//                                CurrencyCode = x.Price.Currency.CurrencyCode
//                            }
//                    )
//                    .PagedList(query.Page, query.PageSize);
//
//        public static Task<ClassifiedAdDetails> Query(
//            this IAsyncDocumentSession session,
//            GetPublishedClassifiedAd query)
//            => (from ad in session.Query<Domain.ClassifiedAd.ClassifiedAd>()
//                where ad.Id.Value == query.ClassifiedAdId
//                let user = RavenQuery
//                    .Load<Domain.UserProfile.UserProfile>(
//                        "UserProfile/" + ad.OwnerId.Value
//                    )
//                select new ClassifiedAdDetails
//                {
//                    ClassifiedAdId = ad.Id.Value,
//                    Title = ad.Title.Value,
//                    Description = ad.Text.Value,
//                    Price = ad.Price.Amount,
//                    CurrencyCode = ad.Price.Currency.CurrencyCode,
//                    SellersDisplayName = user.DisplayName.Value
//                }).SingleAsync();
//
//        /// <summary>
//        /// Paged list of all published ads
//        /// </summary>
//        /// <param name="query"></param>
//        /// <param name="page">zero-based (!) page</param>
//        /// <param name="pageSize">max number of entries to return</param>
//        /// <typeparam name="T"></typeparam>
//        /// <returns></returns>
//        private static Task<List<T>> PagedList<T>(
//            this IRavenQueryable<T> query, int page, int pageSize)
//            =>
//                query
//                    .Skip(page * pageSize)
//                    .Take(pageSize)
//                    .ToListAsync();
        // END RavenDb ==========================================
        
        
        // Dapper / Postgres ==========================================
        public static Task<IEnumerable<PublishedClassifiedAdListItem>> Query(
            this DbConnection connection,
            GetPublishedClassifiedAds query)
            => connection.QueryAsync<PublishedClassifiedAdListItem>(
                "SELECT \"ClassifiedAdId\", \"Price_Amount\" price, \"Title_Value\" title " +
                "FROM \"ClassifiedAds\" WHERE \"State\"=@State LIMIT @PageSize OFFSET @Offset",
                new
                {
                    State = (int)ClassifiedAdState.Active,
                    PageSize = query.PageSize,
                    Offset = Offset(query.Page, query.PageSize)
                });

        public static Task<IEnumerable<PublishedClassifiedAdListItem>> Query(
            this DbConnection connection,
            GetOwnersClassifiedAd query)
            => connection.QueryAsync<PublishedClassifiedAdListItem>(
                "SELECT \"ClassifiedAdId\", \"Price_Amount\" price, \"Title_Value\" title " +
                "FROM \"ClassifiedAds\" WHERE \"OwnerId_Value\"=@OwnerId LIMIT @PageSize OFFSET @Offset",
                new
                {
                    OwnerId = query.OwnerId,
                    PageSize = query.PageSize,
                    Offset = Offset(query.Page, query.PageSize)
                });

        public static Task<ClassifiedAdDetails> Query(
            this DbConnection connection,
            GetPublishedClassifiedAd query)
            => connection.QuerySingleOrDefaultAsync<ClassifiedAdDetails>(
                "SELECT \"ClassifiedAdId\", \"Price_Amount\" price, \"Title_Value\" title, " +
                "\"Text_Value\" description, \"DisplayName_Value\" sellersdisplayname " +
                "FROM \"ClassifiedAds\", \"UserProfiles\" " +
                "WHERE \"ClassifiedAdId\" = @Id AND \"OwnerId_Value\"=\"UserProfileId\"",
                new { Id = query.ClassifiedAdId });

        private static int Offset(int page, int pageSize) => page * pageSize;
        // END Dapper / Postgres ==========================================
    }
}