using System;
using System.ComponentModel.DataAnnotations;
using GraphQL;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Captain;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Mutations
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class CaptainMutations : ObjectGraphType
    {
        private const string CREATE_REQUEST_ENDPOINT = "create";
        private const string DELETE_REQUEST_ENDPOINT = "update";
        private const string UPDATE_REQUEST_ENDPOINT = "delete";
        private const string UPDATE_CREATE_ARGUMENT_NAME = "captain";
        private const string DELETE_ARGUMENT_NAME = "id";

        public CaptainMutations(ICaptainService service)
        {
            FieldAsync<ActionResponseType>(
                CREATE_REQUEST_ENDPOINT,
                "Creates a new Captain",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<CaptainCreateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "Captain Entity to be Created"}),
                async context =>
                {
                    var captain = context.GetArgument<Captain>(UPDATE_CREATE_ARGUMENT_NAME);

                    try
                    {
                        await service.CreateCaptainAsync(captain);
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

                    return new ActionResponse(true, captain.Id);
                });

            FieldAsync<ActionResponseType>(
                UPDATE_REQUEST_ENDPOINT,
                "Updates an existing Captain",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<CaptainUpdateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "Captain to be Updated"}),
                async context =>
                {
                    var captain = context.GetArgument<Captain>(UPDATE_CREATE_ARGUMENT_NAME);
                    try
                    {
                        await service.UpdateCaptainAsync(captain);
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

                    return new ActionResponse(true, captain.Id);
                });

            FieldAsync<ActionResponseType>(
                DELETE_REQUEST_ENDPOINT,
                "Removes an existing Captain",
                new QueryArguments(
                    new QueryArgument<GuidGraphType>
                        {Name = DELETE_ARGUMENT_NAME, Description = "Captain Id used to identify which Captain will be deleted"}),
                async context =>
                {
                    var id = context.GetArgument<Guid>(DELETE_ARGUMENT_NAME);
                    try
                    {
                        await service.DeleteCaptainAsync(id);
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