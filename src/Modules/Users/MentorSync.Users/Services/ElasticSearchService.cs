using Elastic.Clients.Elasticsearch;
using MentorSync.Users.Domain;

namespace MentorSync.Users.Services;

public interface IElasticSearchService
{
    Task IndexUserAsync(AppUser user);
    Task<IEnumerable<AppUser>> SearchUsersByBioAsync(string query);
    Task<IEnumerable<AppUser>> SearchUsersByComplexQueryAsync(string query);
}
public sealed class ElasticSearchService(ElasticsearchClient elasticClient) : IElasticSearchService
{
    private readonly string _indexName = "users";

    public async Task IndexUserAsync(AppUser user)
    {
        await elasticClient.IndexAsync(user, idx => idx.Index(_indexName));
    }

    public async Task<IEnumerable<AppUser>> SearchUsersByBioAsync(string query)
    {
        var response = await elasticClient.SearchAsync<AppUser>(s => s
            .Index(_indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Bio)
                    .Query(query)
                )
            )
        );

        return response.Documents;
    }

    public async Task<IEnumerable<AppUser>> SearchUsersByComplexQueryAsync(string query)
    {
        var response = await elasticClient.SearchAsync<AppUser>(s => s
            .Index(_indexName)
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Match(mm => mm
                            .Field(f => f.Bio)
                            .Query(query)
                        )
                    )
                    .Filter(f => f
                        .Term(t => t
                            .Field(ff => ff.IsActive)
                            .Value(true)
                        )
                    )
                )
            )
        );

        return response.Documents;
    }
}
