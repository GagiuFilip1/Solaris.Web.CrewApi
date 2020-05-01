using System;
using System.ComponentModel.DataAnnotations;
using GraphQL;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.InputObjects.ExplorersTeam;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Mutations
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class ExplorersTeamMutations : ObjectGraphType
    {
        private const string CREATE_REQUEST_ENDPOINT = "create";
        private const string DELETE_REQUEST_ENDPOINT = "update";
        private const string UPDATE_REQUEST_ENDPOINT = "delete";
        private const string UPDATE_CREATE_ARGUMENT_NAME = "explorersteam";
        private const string DELETE_ARGUMENT_NAME = "id";

        public ExplorersTeamMutations(IExplorersTeamService service)
        {
            FieldAsync<ActionResponseType>(
                CREATE_REQUEST_ENDPOINT,
                "Creates a new ExplorersTeam",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<ExplorersTeamCreateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "ExplorersTeam Entity to be Created"}),
                async context =>
                {
                    var explorersTeam = context.GetArgument<ExplorersTeam>(UPDATE_CREATE_ARGUMENT_NAME);

                    try
                    {
                        await service.CreateExplorersTeamAsync(explorersTeam);
                    }
                    catch (ValidationException e)
                    {
                        context.Errors.Add(new ExecutionError(e.Message));
                        return new ActionResponse(false);
                    }
                    catch (Exception)
                    {
                        context.Errors.Add(new ExecutionError("Server Error"));
                        return new ActionResponse(false);
                    }

                    return new ActionResponse(true, explorersTeam.Id);
                });

            FieldAsync<ActionResponseType>(
                UPDATE_REQUEST_ENDPOINT,
                "Updates an existing ExplorersTeam",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<ExplorersTeamUpdateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "ExplorersTeam to be Updated"}),
                async context =>
                {
                    var explorersTeam = context.GetArgument<ExplorersTeam>(UPDATE_CREATE_ARGUMENT_NAME);
                    try
                    {
                        await service.UpdateExplorersTeamAsync(explorersTeam);
                    }
                    catch (ValidationException e)
                    {
                        context.Errors.Add(new ExecutionError(e.Message));
                        return new ActionResponse(false);
                    }
                    catch (Exception)
                    {
                        context.Errors.Add(new ExecutionError("Server Error"));
                        return new ActionResponse(false);
                    }

                    return new ActionResponse(true, explorersTeam.Id);
                });

            FieldAsync<ActionResponseType>(
                DELETE_REQUEST_ENDPOINT,
                "Removes an existing ExplorersTeam",
                new QueryArguments(
                    new QueryArgument<GuidGraphType>
                        {Name = DELETE_ARGUMENT_NAME, Description = "ExplorersTeam Id used to identify which ExplorersTeam will be deleted"}),
                async context =>
                {
                    var id = context.GetArgument<Guid>(DELETE_ARGUMENT_NAME);
                    try
                    {
                        await service.DeleteExplorersTeamAsync(id);
                    }
                    catch (ValidationException e)
                    {
                        context.Errors.Add(new ExecutionError(e.Message));
                        return new ActionResponse(false);
                    }
                    catch (Exception)
                    {
                        context.Errors.Add(new ExecutionError("Server Error"));
                        return new ActionResponse(false);
                    }

                    return new ActionResponse(true);
                });
        }
    }
}