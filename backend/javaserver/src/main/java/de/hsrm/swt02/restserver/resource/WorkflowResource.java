package de.hsrm.swt02.restserver.resource;

import java.util.List;
import java.util.logging.Level;

import javax.ws.rs.Consumes;
import javax.ws.rs.DELETE;
import javax.ws.rs.GET;
import javax.ws.rs.HeaderParam;
import javax.ws.rs.POST;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.MultivaluedMap;
import javax.ws.rs.core.Response;

import de.hsrm.swt02.businesslogic.Logic;
import de.hsrm.swt02.businesslogic.LogicResponse;
import de.hsrm.swt02.businesslogic.exceptions.LogicException;
import de.hsrm.swt02.businesslogic.exceptions.NoPermissionException;
import de.hsrm.swt02.constructionfactory.ConstructionFactory;
import de.hsrm.swt02.logging.UseLogger;
import de.hsrm.swt02.messaging.ServerPublisher;
import de.hsrm.swt02.model.Item;
import de.hsrm.swt02.model.Workflow;
import de.hsrm.swt02.persistence.exceptions.PersistenceException;
import de.hsrm.swt02.restserver.exceptions.JacksonException;

/**
 * This class is called if client operates on workflows.
 *
 */
@Path("resource")
public class WorkflowResource {

    public static final ConstructionFactory FACTORY = ConstructionFactory
            .getInstance();
    public static final Logic LOGIC = FACTORY.getLogic();
    public static final ServerPublisher PUBLISHER = FACTORY.getPublisher();
    public static final UseLogger LOGGER = new UseLogger();
    private static final String PREFIX = "[restserver] ";
    private static final int NOTFOUNDCODE = 404;

    /**
     * This method returns a requested workflow.
     * @param workflowid indicates which workflow is requested
     * @return the requested workflow
     */
    @GET
    @Path("workflows/{workflowid}")
    @Produces(MediaType.TEXT_PLAIN)
    public Response getWorkflow(@PathParam("workflowid") String workflowid) {
        final String loggingBody = PREFIX + "GET /resource/workflows/" + workflowid;
        LOGGER.log(Level.INFO, loggingBody);
        String workflowAsString;
        Workflow workflow = null;

        try {
            workflow = LOGIC.getWorkflow(workflowid);
            workflow.convertReferencesToIdList();
        } catch (PersistenceException e1) {
            LOGGER.log(Level.WARNING, e1);
            return Response.serverError().entity(String.valueOf(e1.getErrorCode())).build();
        }
        
        try {
            workflowAsString = JsonParser.marshall(workflow);
            LOGGER.log(Level.FINE, loggingBody + workflowAsString);
        } catch (JacksonException e) {
            LOGGER.log(Level.INFO, loggingBody);
            LOGGER.log(Level.WARNING, e);
            return Response
                    .serverError()
                    .entity(String.valueOf(e
                            .getErrorCode())).build();
        }
        LOGGER.log(Level.INFO, loggingBody + " Request successful.");
        return Response.ok(workflowAsString).build();
    }

    /**
     * This method returns all startable workflows for a given user.
     * @param username the name of the user for whom the workflows shall be
     *            returned
     * @param workflowid the id of the workflow.
     * @return all startable workflows for user
     */
    @GET
    @Path("workflows/{workflowid}/items")
    @Produces(MediaType.TEXT_PLAIN)
    public Response getRelevantItemsByUser(@PathParam("workflowid") String workflowid,@HeaderParam("username") String username) {
        final String loggingBody = PREFIX + "GET /resource/workflows/" + workflowid + "/items";
        LOGGER.log(Level.INFO, loggingBody);
        List<Item> itemList = null;
        try {
            itemList = LOGIC.getRelevantItemsForUser(workflowid, username);
        } catch (PersistenceException e1) {
            LOGGER.log(Level.WARNING, e1);
            return Response.serverError().entity(String.valueOf(e1.getErrorCode())).build();
        }

        String itemListString;

        try {
            itemListString = JsonParser.marshall(itemList);
        } catch (JacksonException e) {
            LOGGER.log(Level.INFO, loggingBody);
            LOGGER.log(Level.WARNING, e);
            return Response
                    .serverError()
                    .entity(String.valueOf(e
                            .getErrorCode())).build();
        }
        LOGGER.log(Level.INFO, loggingBody + " Request successful.");
        return Response.ok(itemListString).build();
    }

    /**
     * This method returns workflows where a user is involved.
     * @param username indicates which user's workflows are requested
     * @param clientID signals which of the clients sent the request
     * @param state optional selector to reduce the results
     * @return the requested workflow
     * @throws NoPermissionException 
     */
    @GET
    @Path("workflows")
    @Produces(MediaType.TEXT_PLAIN)
    public Response getWorkflows(@HeaderParam("username") String username,@QueryParam("state") String state,
            @HeaderParam("client_id") String clientID) throws NoPermissionException 
    {
        if (state != null) {
            final String loggingBody = PREFIX + "GET /resource/workflows?state=" + state;
            switch(state) {
                case "startable":
                    LOGGER.log(Level.INFO, loggingBody);
                    List<String> wIdList = null;
        
                    try {
                        wIdList = LOGIC.getStartableWorkflowsForUser(username);
                    } catch (LogicException e1) {
                        LOGGER.log(Level.WARNING, e1);
                        return Response.serverError().entity(String.valueOf(e1.getErrorCode())).build();
                    }
        
                    String wIdListString;
        
                    try {
                        wIdListString = JsonParser.marshall(wIdList);
                        LOGGER.log(Level.FINE, loggingBody + wIdListString);
                    } catch (JacksonException e) {
                        LOGGER.log(Level.INFO, loggingBody);
                        LOGGER.log(Level.WARNING, e);
                        return Response
                                .serverError()
                                .entity(String.valueOf(e
                                        .getErrorCode())).build();
                    }
                    LOGGER.log(Level.INFO, loggingBody + " Request successful.");
                    return Response.ok(wIdListString).build();
                default:
                    return Response.status(NOTFOUNDCODE).build();
            }
        } else {
            final String loggingBody = PREFIX + "GET /resource/workflows";
            LOGGER.log(Level.INFO, loggingBody);
            List<Workflow> wflowList = null;
            
            if (clientID.equals("admin")) {
                try {
                    wflowList = LOGIC.getAllWorkflows();
                } catch (PersistenceException e) {
                    LOGGER.log(Level.WARNING, e);
                    return Response.serverError()
                            .entity(String.valueOf(e.getErrorCode())).build();
                }
            } else {
                try {
                    wflowList = LOGIC.getAllWorkflowsForUser(username);
                    for (Workflow w : wflowList) {
                        w.convertReferencesToIdList();
                    }
                } catch (LogicException e) {
                    LOGGER.log(Level.WARNING, e);
                    return Response.serverError().entity(String.valueOf(e.getErrorCode())).build();
                }
            }
            
            String wListString;
    
            try {
                wListString = JsonParser.marshall(wflowList);
                LOGGER.log(Level.FINE, loggingBody + wListString);
            } catch (JacksonException e) {
                LOGGER.log(Level.INFO, loggingBody);
                LOGGER.log(Level.WARNING, e);
                return Response
                        .serverError()
                        .entity(String.valueOf(e
                                .getErrorCode())).build();
            }
            LOGGER.log(Level.INFO, loggingBody + " Request successful.");
            return Response.ok(wListString).build();
        }
    }

    /**
     * Receives a workflow and stores it into the database. This operation will
     * be published on the message broker.
     * @param username the user requesting the service
     * @param formParams wrapper for an sent workflow
     * @return "true" if everything was successful OR "jackson exception" if
     *         serialization crashed
     */
    @POST
    @Path("workflows")
    @Produces(MediaType.TEXT_PLAIN)
    @Consumes("application/x-www-form-urlencoded")
    public Response saveWorkflow(MultivaluedMap<String, String> formParams, @HeaderParam("username") String username) {
        
        try {
            LOGIC.checkUserIsAdmin(username);
        } catch (LogicException e2) {
            LOGGER.log(Level.INFO, e2);
            return Response.serverError().entity(String.valueOf(e2.getErrorCode())).build();
        }
        
        LogicResponse logicResponse;
        final String loggingBody = PREFIX + "POST /resource/workflows";
        LOGGER.log(Level.INFO, loggingBody);
        final String workflowAsString = formParams.get("data").get(0);
        Workflow workflow = new Workflow();

        try {
            workflow = (Workflow)JsonParser.unmarshall(workflowAsString, workflow);
        } catch (JacksonException e) {
            LOGGER.log(Level.INFO, loggingBody);
            LOGGER.log(Level.WARNING, e);
            return Response
                    .serverError()
                    .entity(String.valueOf(e
                            .getErrorCode())).build();
        }
        workflow.convertIdListToReferences();
        try {
            
            logicResponse = LOGIC.addWorkflow(workflow);
        } catch (LogicException e1) {
            LOGGER.log(Level.WARNING, e1);
            return Response.serverError()
                    .entity(String.valueOf(e1.getErrorCode())).build();
        }
        
        PUBLISHER.publishEvent(logicResponse);
        
        LOGGER.log(Level.INFO, loggingBody + " Workflow successfully saved.");
        return Response.ok().build();
    }

    /**
     * This method updates a workflow. This operation will be published on the
     * message broker.
     * @param formParams wrapper for an sent workflow
     * @param workflowid - the workflow to update
     * @param username the user requesting the service
     * @return String true or false
     */
    @PUT
    @Path("workflows/{workflowid}")
    @Produces(MediaType.TEXT_PLAIN)
    @Consumes("application/x-www-form-urlencoded")
    public Response updateWorkflow(MultivaluedMap<String, String> formParams, @PathParam("workflowid") String workflowid, @HeaderParam("username") String username) {
        
        try {
            LOGIC.checkUserIsAdmin(username);
        } catch (LogicException e2) {
            LOGGER.log(Level.INFO, e2);
            return Response.serverError().entity(String.valueOf(e2.getErrorCode())).build();
        }        
        
        LogicResponse logicResponse;
        final String loggingBody = PREFIX + "PUT /resource/workflows/" + workflowid;
        LOGGER.log(Level.INFO, loggingBody);
        final String workflowAsString = formParams.get("data").get(0);
        Workflow workflow = new Workflow();

        try {
            workflow = (Workflow)JsonParser.unmarshall(workflowAsString, workflow);
        } catch (JacksonException e) {
            LOGGER.log(Level.INFO, loggingBody);
            LOGGER.log(Level.WARNING, e);
            return Response
                    .serverError()
                    .entity(String.valueOf(e
                            .getErrorCode())).build();
        }
        workflow.convertIdListToReferences();
        try {
            logicResponse = LOGIC.addWorkflow(workflow);
            PUBLISHER.publishEvent(logicResponse);
        } catch (LogicException e1) {
            LOGGER.log(Level.WARNING, e1);
            return Response.serverError()
                    .entity(String.valueOf(e1.getErrorCode())).build();
        }

        LOGGER.log(Level.INFO, loggingBody + " Workflow successfully updated.");
        return Response.ok().build();
    }

    /**
     * This method deletes a workflow. This operation will be published on the
     * message broker.
     * @param workflowid which indicates which workflow should be deleted
     * @param username the user requesting the service
     * @return deleted workflow, if successful
     */
    @DELETE
    @Path("workflows/{workflowid}")
    @Produces(MediaType.TEXT_PLAIN)
    public Response deleteWorkflow(@PathParam("workflowid") String workflowid, @HeaderParam("username") String username) {
        
        try {
            LOGIC.checkUserIsAdmin(username);
        } catch (LogicException e2) {
            LOGGER.log(Level.INFO, e2);
            return Response.serverError().entity(String.valueOf(e2.getErrorCode())).build();
        } 
        
        LogicResponse logicResponse;
        final String loggingBody = PREFIX + "DELETE /resource/workflows/" + workflowid;
        LOGGER.log(Level.INFO, loggingBody);
        Workflow workflow = null;
        String workflowAsString;

        try {
            workflow = LOGIC.getWorkflow(workflowid);
            logicResponse = LOGIC.deleteWorkflow(workflowid);
        } catch (PersistenceException e1) {
            LOGGER.log(Level.WARNING, e1);
            return Response.serverError()
                    .entity(String.valueOf(e1.getErrorCode())).build();
        }
            
        try {
            workflowAsString = JsonParser.marshall(workflow);
        } catch (JacksonException e) {
            LOGGER.log(Level.WARNING, e);
            return Response
                    .serverError()
                    .entity(String.valueOf(e
                            .getErrorCode())).build();
        }
        
        PUBLISHER.publishEvent(logicResponse);
        
        LOGGER.log(Level.INFO, loggingBody + " Workflow succesfully deleted.");
        return Response.ok(workflowAsString).build();
    }
}
