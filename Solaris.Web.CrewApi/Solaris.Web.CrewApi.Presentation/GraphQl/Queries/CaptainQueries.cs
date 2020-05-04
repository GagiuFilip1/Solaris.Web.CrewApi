using System;
using GraphQL;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Filters.Implementation;
using Solaris.Web.CrewApi.Core.Models.Helpers.Commons;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Queries
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class CaptainQueries : ObjectGraphType
    {
        private const string SEARCH_REQUEST_ENDPOINT = "search";
        private const string PAGINATION_ARGUMENT_NAME = "pagination";
        private const string ORDERING_ARGUMENT_NAME = "ordering";
        private const string FILTERING_ARGUMENT_NAME = "filtering";

        public CaptainQueries(ICaptainService captainService)
        {
            FieldAsync<ListCaptainsQueryModelType>(
                SEARCH_REQUEST_ENDPOINT,
                "Returns a paginated list of Captains",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<PagedRequestType>> {Name = PAGINATION_ARGUMENT_NAME, Description = PagedRequestType.Description},
                    new QueryArgument<NonNullGraphType<OrderedRequestType>> {Name = ORDERING_ARGUMENT_NAME, Description = OrderedRequestType.Description},
                    new QueryArgument<NonNullGraphType<FilteredRequestType<Captain>>> {Name = FILTERING_ARGUMENT_NAME, Description = FilteredRequestType<Captain>.Description}
                ),
                async context =>
                {
                    var pagination = context.GetArgument<Pagination>(PAGINATION_ARGUMENT_NAME);
                    var ordering = context.GetArgument<Ordering>(ORDERING_ARGUMENT_NAME);
                    var filtering = context.GetArgument<CaptainFilter>(FILTERING_ARGUMENT_NAME);

                    var (totalCount, items) = await captainService.SearchCaptainAsync(pagination, ordering, filtering);
                    try
                    {
                        return new ListResponse<Captain>
                        {
                            TotalCount = totalCount,
                            Items = items
                        };
                    }
                    catch (Exception)
                    {
                        context.Errors.Add(new ExecutionError("Server Error"));
                        return null;
                    }
                }
            );
        }
    }
}