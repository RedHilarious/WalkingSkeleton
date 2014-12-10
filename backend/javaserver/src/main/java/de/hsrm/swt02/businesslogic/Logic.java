package de.hsrm.swt02.businesslogic;

import java.util.List;

import de.hsrm.swt02.businesslogic.exceptions.LogicException;
import de.hsrm.swt02.model.Item;
import de.hsrm.swt02.model.Role;
import de.hsrm.swt02.model.Step;
import de.hsrm.swt02.model.User;
import de.hsrm.swt02.model.Workflow;
import de.hsrm.swt02.persistence.exceptions.ItemNotExistentException;
import de.hsrm.swt02.persistence.exceptions.RoleAlreadyExistsException;
import de.hsrm.swt02.persistence.exceptions.RoleNotExistentException;
import de.hsrm.swt02.persistence.exceptions.UserAlreadyExistsException;
import de.hsrm.swt02.persistence.exceptions.UserHasAlreadyRoleException;
import de.hsrm.swt02.persistence.exceptions.UserNotExistentException;
import de.hsrm.swt02.persistence.exceptions.WorkflowNotExistentException;
import de.hsrm.swt02.restserver.LogicResponse;

/**
 * This interface is used for the business logic.
 *
 */
public interface Logic {

    /*
     * workflow functions
     */
    /**
     * This method starts a Workflow.
     * 
     * @param workflowID
     *            the workflow, which should be started
     * @param username
     *            the User, who starts the workflow
     * @return logicResponse of starting a workflow
     * @throws WorkflowNotExistentException
     */
    LogicResponse startWorkflow(int workflowID, String username)
            throws WorkflowNotExistentException;

    /**
     * This method store a workflow and distribute a id.
     * 
     * @param workflow
     *            which should be added
     * @return logicResponse of adding a workflow
     */
    LogicResponse addWorkflow(Workflow workflow); // later a workflows name will
                                                  // be available

    /**
     * This method return all workflows in persistence.
     * 
     * @return list of all workflows.
     * @throws WorkflowNotExistentException .
     */
    List<Workflow> getAllWorkflows() throws WorkflowNotExistentException;

    /**
     * This method loads a Workflow.
     * 
     * @param workflowID
     *            describe the workflow
     * @return a Workflow, if there is one, who has this workflowID
     * @throws WorkflowNotExistentException
     */
    Workflow getWorkflow(int workflowID) throws WorkflowNotExistentException;

    /**
     * This method delete a Workflow in Persistence.
     * 
     * @param workflowID
     *            describe the Workflow
     * @return logicResponse of deleting a workflow
     * @throws WorkflowNotExistentException
     */
    LogicResponse deleteWorkflow(int workflowID)
            throws WorkflowNotExistentException;

    /*
     * item
     */
    /**
     * This method execute a step in an item.
     * 
     * @param itemId
     *            the itemId, which edited
     * @param stepId
     *            the stepId, which execute
     * @param username
     *            who execute the step in the Item
     * @throws ItemNotExistentException
     * @exception ItemNotExistentException
     *                if requested item doesn't exist
     * @throws UserNotExistentException
     * @exception UserNotExistentException
     *                if requested user doesn't exist
     */
    void stepForward(int itemId, int stepId, String username)
            throws ItemNotExistentException, UserNotExistentException;

    // /**
    // * This method finish a step in an item.
    // *
    // * @param itemId the Item, which edited
    // * @param stepId the step, which execute
    // * @param username who execute the step in the Item
    // * @throws ItemNotExistentException
    // * @exception ItemNotExistentException if requested item doesn't exist
    // * @throws UserNotExistentException
    // * @exception UserNotExistentException if requested user doesn't exist
    // */
    // void stepFinished(int itemId, int stepId, String username) throws
    // ItemNotExistentException, UserNotExistentException;

    /*
     * step functions
     */
    /**
     * This method deactivate a workflow.
     * 
     * @param workflowID
     *            the id of the workflow which should be deactivate
     * @throws WorkflowNotExistentException .
     */
    void deactiviateWorkflow(int workflowID)
            throws WorkflowNotExistentException;

    /**
     * This method activate a workflow.
     * 
     * @param workflowID
     *            the id of the workflow which should be deactivate
     * @throws WorkflowNotExistentException .
     */
    void activiateWorkflow(int workflowID) throws WorkflowNotExistentException;

    /**
     * This method add a step into an existing Workflow.
     * 
     * @param workflowID
     *            the workflow, which shall edited
     * @param stepId
     *            the step, which shall added
     * @return logicResponse of adding a step
     * @throws WorkflowNotExistentException
     */
    LogicResponse addStep(int workflowID, Step stepId)
            throws WorkflowNotExistentException;

    /**
     * This method delete a step from an existing Workflow.
     * 
     * @param workflowID
     *            the workflow, which shall edited
     * @param stepID
     *            the step, which shall delete
     * @return logicResponse of deleting a step
     * @throws WorkflowNotExistentException
     */
    LogicResponse deleteStep(int workflowID, int stepID)
            throws WorkflowNotExistentException;

    /*
     * user functions
     */
    /**
     * This method store a workflow and distribute a id.
     * 
     * @param user
     *            which should be added
     * @return logicResponse of adding a user
     * @throws UserAlreadyExistsException
     */
    LogicResponse addUser(User user) throws UserAlreadyExistsException;

    /**
     * This method loads a User.
     * 
     * @param username
     *            describe the user
     * @return a User, if there is one, who has this username
     * @throws UserNotExistentException
     */
    User getUser(String username) throws UserNotExistentException; // not
                                                                   // attached
                                                                   // yet

    /**
     * This method check a User.
     * 
     * @param username
     *            the user, to be checked
     * @return if user correct true, else false
     * @throws UserNotExistentException
     *             will be thrown if user doesn't exist
     */
    boolean checkLogIn(String username) throws UserNotExistentException; // later
                                                                         // with
                                                                         // password

    /**
     * This method delete a User.
     * 
     * @param username
     *            describe the user
     * @throws UserNotExistentException
     * @return logicResponse of deleting a user
     */
    LogicResponse deleteUser(String username) throws UserNotExistentException;

    /**
     * This method returns all workflows, in which the user is involved, no
     * matter if there is an open or busy item for him Mind that this method
     * won't return the items only a list of workflows
     * 
     * @param username
     *            whose workflows' is looked for
     * @return a LinkedList of workflows
     */
    List<Workflow> getAllWorkflowsByUser(String username)
            throws WorkflowNotExistentException, UserNotExistentException,
            LogicException;

    /**
     * 
     * @param username
     * @return
     * @throws UserNotExistentException
     */
    List<Integer> getStartableWorkflowsByUser(String username)
            throws UserNotExistentException, WorkflowNotExistentException,
            LogicException;

    /**
     * 
     * @param workflowId
     * @param username
     * @return
     * @throws WorkflowNotExistentException
     * @throws UserNotExistentException
     */
    List<Item> getRelevantItemsByUser(int workflowId, String username)
            throws WorkflowNotExistentException, UserNotExistentException;

    /**
     * This method gets a LogicResponse object.
     * 
     * @return LogicResponse object
     */
    LogicResponse getProcessLogicResponse();

    /**
     * Setter for logicResponse.
     * 
     * @param lr
     *            is new value for logicResponse
     */
    void setProcessLogicResponse(LogicResponse lr);

    // Business Logik Sprint 2

    /**
     * Method for getting a list of all the existing roles in the persistance.
     * 
     * @return list of all roles
     * @exception RoleNotExistentException
     *                if the requested Role is not there
     * @throws RoleNotExistentException
     */
    List<Role> getAllRoles() throws RoleNotExistentException;

    /**
     * Method for returning a list of all the existing users in the persistance.
     * 
     * @return list of all users
     * @exception UserNotExistentException
     *                if the requested user is not there
     * @throws UserNotExistentException
     */
    List<User> getAllUsers() throws UserNotExistentException;

    /**
     * Method for adding a new role in the persistance.
     * 
     * @param role
     *            is the role we want to add
     * @return LogicResponse object
     * @exception RoleAlreadyExistsException
     *                if the role we want to add is already there
     * @throws RoleAlreadyExistsException
     */
    LogicResponse addRole(Role role) throws RoleAlreadyExistsException;

    /**
     * Method for adding an existing role to the rolelist of an existing user.
     * 
     * @param username
     *            is the username of the user
     * @param role
     *            is the role we want to add
     * @return LogicResponse object
     * @exception UserNotExistentException
     *                if the user is not in the persistance
     * @exception RoleNotExistentException
     *                if the role is not in the persistance
     * @exception UserHasAlreadyRoleException
     *                if the user has already the role in his rolelist
     * @throws UserNotExistentException
     * @throws RoleNotExistentException
     * @throws UserHasAlreadyRoleException
     */
    LogicResponse addRoleToUser(String username, Role role)
            throws UserNotExistentException, RoleNotExistentException,
            UserHasAlreadyRoleException;

    /**
     * Method for deleting an existing role from the persistance. The users who
     * have this role will lose it too.
     * 
     * @param rolename
     *            of the role
     * @return LogicResponce object
     * @exception RoleNotExistentException
     *                if the requested role is not there
     * @throws RoleNotExistentException
     */
    LogicResponse deleteRole(String rolename) throws RoleNotExistentException;
}
