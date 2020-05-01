using System;
using System.ComponentModel.DataAnnotations;
using GraphQL;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Shuttle;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Mutations
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class ShuttleMutations : ObjectGraphType
    {
        private const string CREATE_REQUEST_ENDPOINT = "create";
        private const string DELETE_REQUEST_ENDPOINT = "update";
        private const string UPDATE_REQUEST_ENDPOINT = "delete";
        private const string UPDATE_CREATE_ARGUMENT_NAME = "Shuttle";
        private const string DELETE_ARGUMENT_NAME = "id";

        public ShuttleMutations(IShuttleService service)
        {
            FieldAsync<ActionResponseType>(
                CREATE_REQUEST_ENDPOINT,
                "Creates a new Shuttle",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<ShuttleCreateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "Shuttle Entity to be Created"}),
                async context =>
                {
                    var shuttle = context.GetArgument<Shuttle>(UPDATE_CREATE_ARGUMENT_NAME);

                    try
                    {
                        await service.CreateShuttleAsync(shuttle);
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

                    return new ActionResponse(true, shuttle.Id);
                });

            FieldAsync<ActionResponseType>(
                UPDATE_REQUEST_ENDPOINT,
                "Updates an existing Shuttle",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<ShuttleUpdateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "Shuttle to be Updated"}),
                async context =>
                {
                    var shuttle = context.GetArgument<Shuttle>(UPDATE_CREATE_ARGUMENT_NAME);
                    try
                    {
                        await service.UpdateShuttleAsync(shuttle);
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

                    return new ActionResponse(true, shuttle.Id);
                });

            FieldAsync<ActionResponseType>(
                DELETE_REQUEST_ENDPOINT,
                "Removes an existing Shuttle",
                new QueryArguments(
                    new QueryArgument<GuidGraphType>
                        {Name = DELETE_ARGUMENT_NAME, Description = "Shuttle Id used to identify which Shuttle will be deleted"}),
                async context =>
                {
                    var id = context.GetArgument<Guid>(DELETE_ARGUMENT_NAME);
                    try
                    {
                        await service.DeleteShuttleAsync(id);
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