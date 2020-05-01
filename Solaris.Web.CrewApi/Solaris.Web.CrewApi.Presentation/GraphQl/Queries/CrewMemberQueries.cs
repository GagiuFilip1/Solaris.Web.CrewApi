using System;
using GraphQL;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Models.Helpers;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Queries
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class CrewMemberQueries : ObjectGraphType
    {
        private const string SEARCH_REQUEST_ENDPOINT = "search";
        private const string COUNT_REQUEST_ENDPOINT = "count";
        private const string PAGINATION_ARGUMENT_NAME = "pagination";
        private const string ORDERING_ARGUMENT_NAME = "ordering";
        private const string FILTERING_ARGUMENT_NAME = "filtering";

        public CrewMemberQueries(ICrewMemberService crewMemberService)
        {
            FieldAsync<ListCrewMembersQueryModelType>(
                SEARCH_REQUEST_ENDPOINT,
                "Returns a paginated list of CrewMembers",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<PagedRequestType>> {Name = PAGINATION_ARGUMENT_NAME, Description = PagedRequestType.Description},
                    new QueryArgument<NonNullGraphType<OrderedRequestType>> {Name = ORDERING_ARGUMENT_NAME, Description = OrderedRequestType.Description},
                    new QueryArgument<NonNullGraphType<FilteredRequestType<CrewMember>>> {Name = FILTERING_ARGUMENT_NAME, Description = FilteredRequestType<CrewMember>.Description}
                ),
                async context =>
                {
                    var pagination = context.GetArgument<Pagination>(PAGINATION_ARGUMENT_NAME);
                    var ordering = context.GetArgument<Ordering>(ORDERING_ARGUMENT_NAME);
                    var filtering = context.GetArgument<CrewMemberFilter>(FILTERING_ARGUMENT_NAME);

                    var (totalCount, items) = await crewMemberService.SearchCrewMemberAsync(pagination, ordering, filtering);
                    try
                    {
                        return new ListResponse<CrewMember>
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
            
            FieldAsync<IntGraphType>(
                COUNT_REQUEST_ENDPOINT,
                "Returns a count of the filtered search",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<FilteredRequestType<CrewMember>>> {Name = FILTERING_ARGUMENT_NAME, Description = FilteredRequestType<CrewMember>.Description}
                ),
                async context =>
                {
                    var filtering = context.GetArgument<CrewMemberFilter>(FILTERING_ARGUMENT_NAME);

                    var count = await crewMemberService.SimpleCountAsync(filtering);
                    try
                    {
                        return count;
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