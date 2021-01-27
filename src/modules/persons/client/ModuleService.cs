using Delights.Modules.Persons.GraphQL;
using Modulight.Modules.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delights.Modules.Persons
{
    public class ModuleService : IModuleService
    {
        public IPersonsGraphQLClient GraphQLClient { get; }

        public UrlGenerator UrlGenerator { get; }

        public ModuleService(IPersonsGraphQLClient graphQLClient)
        {
            GraphQLClient = graphQLClient;
            UrlGenerator = new UrlGenerator();
        }

        public async Task<IData> GetPerson(string id)
        {
            var result = await GraphQLClient.GetDataByIdAsync(id);
            result.EnsureNoErrors();
            return result.Data!.DataById;
        }

        public async IAsyncEnumerable<IData> GetAllPersons()
        {
            var result = await GraphQLClient.GetMetadataIdsAsync();
            result.EnsureNoErrors();
            foreach(var item in result.Data!.Metadata.Nodes)
            {
                var pr = await GraphQLClient.GetDataByMetadataIdAsync(item.Id);
                pr.EnsureNoErrors();
                yield return pr.Data!.DataByMetadataId;
            }
        }
    }
}
