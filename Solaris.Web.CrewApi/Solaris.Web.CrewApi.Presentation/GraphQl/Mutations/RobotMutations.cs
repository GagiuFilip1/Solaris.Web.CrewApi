using System;
using System.ComponentModel.DataAnnotations;
using GraphQL;
using GraphQL.Types;
using Solaris.Web.CrewApi.Core.GraphQl.Helpers;
using Solaris.Web.CrewApi.Core.GraphQl.InputObjects.Robot;
using Solaris.Web.CrewApi.Core.Models.Entities;
using Solaris.Web.CrewApi.Core.Services.Interfaces;
using Solaris.Web.CrewApi.Infrastructure.Filters;
using Solaris.Web.CrewApi.Infrastructure.Ioc;

namespace Solaris.Web.CrewApi.Presentation.GraphQl.Mutations
{
    [RegistrationKind(Type = RegistrationType.Scoped, AsSelf = true)]
    public class RobotMutations : ObjectGraphType
    {
        private const string CREATE_REQUEST_ENDPOINT = "create";
        private const string DELETE_REQUEST_ENDPOINT = "update";
        private const string UPDATE_REQUEST_ENDPOINT = "delete";
        private const string SEND_ROBOTS_REQUEST_ENDPOINT = "send";
        private const string UPDATE_CREATE_ARGUMENT_NAME = "robot";
        private const string DELETE_ARGUMENT_NAME = "id";
        private const string PLANET_ARGUMENT_NAME = "planetId";
        private const string ROBOTS_FILTER_ARGUMENT_NAME = "filter";

        public RobotMutations(IRobotService service)
        {
            FieldAsync<ActionResponseType>(
                CREATE_REQUEST_ENDPOINT,
                "Creates a new Robot",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<RobotCreateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "Robot Entity to be Created"}),
                async context =>
                {
                    var robot = context.GetArgument<Robot>(UPDATE_CREATE_ARGUMENT_NAME);

                    try
                    {
                        await service.CreateRobotAsync(robot);
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

                    return new ActionResponse(true, robot.Id);
                });

            FieldAsync<ActionResponseType>(
                UPDATE_REQUEST_ENDPOINT,
                "Updates an existing Robot",
                new QueryArguments(
                    new QueryArgument<NonNullGraphType<RobotUpdateViewModel>>
                        {Name = UPDATE_CREATE_ARGUMENT_NAME, Description = "Robot to be Updated"}),
                async context =>
                {
                    var robot = context.GetArgument<Robot>(UPDATE_CREATE_ARGUMENT_NAME);
                    try
                    {
                        await service.UpdateRobotAsync(robot);
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

                    return new ActionResponse(true, robot.Id);
                });

            FieldAsync<ActionResponseType>(
                DELETE_REQUEST_ENDPOINT,
                "Removes an existing Robot",
                new QueryArguments(
                    new QueryArgument<GuidGraphType>
                        {Name = DELETE_ARGUMENT_NAME, Description = "Robot Id used to identify which Robot will be deleted"}),
                async context =>
                {
                    var id = context.GetArgument<Guid>(DELETE_ARGUMENT_NAME);
                    try
                    {
                        await service.DeleteRobotAsync(id);
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

            FieldAsync<ActionResponseType>(
                SEND_ROBOTS_REQUEST_ENDPOINT,
                "Send robots to explore the planet",
                new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = PLANET_ARGUMENT_NAME, Description = "Robot Id used to identify which Robot will be deleted"},
                    new QueryArgument<FilteredRequestType<Robot>> {Name = ROBOTS_FILTER_ARGUMENT_NAME, Description = "The filter which will retrieve the robots to be sent to the planet"}
                ),
                async context =>
                {
                    var planetId = context.GetArgument<Guid>(PLANET_ARGUMENT_NAME);
                    var fiter = context.GetArgument<RobotFilter>(ROBOTS_FILTER_ARGUMENT_NAME);
                    
                    try
                    {
                        await service.SendRobotsToPlanetAsync(fiter, planetId);
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