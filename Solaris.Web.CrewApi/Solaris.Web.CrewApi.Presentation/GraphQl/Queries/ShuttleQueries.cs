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
    public class ShuttleQueries : ObjectGraphType
    {
        private const string SEARCH_REQUEST_ENDPOINT = "search";
        private const string PAGINATION_ARGUMENT_NAME = "pagination";
        private const string ORDERING_ARGUMENT_NAME = "ordering";
        private const string FILTERING_ARGUMENT_NAME = "filtering";

        public ShuttleQueries(IShuttleService shuttleService)
        {
            FieldAsync<ListShuttlesQueryModelType>(
                SEARCH_REQUEST_ENDPOINT,
                "Returns a paginated list of Shuttles",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<PagedRequestType>> {Name = PAGINATION_ARGUMENT_NAME, Description = PagedRequestType.Description},
                    new QueryArgument<NonNullGraphType<OrderedRequestType>> {Name = ORDERING_ARGUMENT_NAME, Description = OrderedRequestType.Description},
                    new QueryArgument<NonNullGraphType<FilteredRequestType<Shuttle>>> {Name = FILTERING_ARGUMENT_NAME, Description = FilteredRequestType<Shuttle>.Description}
                ),
                async context =>
                {
                    var pagination = context.GetArgument<Pagination>(PAGINATION_ARGUMENT_NAME);
                    var ordering = context.GetArgument<Ordering>(ORDERING_ARGUMENT_NAME);
                    var filtering = context.GetArgument<ShuttleFilter>(FILTERING_ARGUMENT_NAME);

                    var (totalCount, items) = await shuttleService.SearchShuttleAsync(pagination, ordering, filtering);
                    try
                    {
                        return new ListResponse<Shuttle>
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