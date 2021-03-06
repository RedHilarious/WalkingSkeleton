package de.hsrm.swt02.businesslogic;

import java.util.List;

import de.hsrm.swt02.businesslogic.exceptions.LogicException;
import de.hsrm.swt02.businesslogic.exceptions.NoPermissionException;
import de.hsrm.swt02.model.Form;
import de.hsrm.swt02.model.Item;
import de.hsrm.swt02.model.Role;
import de.hsrm.swt02.model.Step;
import de.hsrm.swt02.model.User;
import de.hsrm.swt02.model.Workflow;
import de.hsrm.swt02.persistence.Persistence;
import de.hsrm.swt02.persistence.exceptions.PersistenceException;
import de.hsrm.swt02.persistence.exceptions.UserNotExistentException;


/**
 * This interface is used for the business logic.
 */
public interface Logic {

    /**
     * Getter for Persistence.
     * Do not use, only for testing case
     * 
     * @return Persistence
     */
    Persistence getPersistence();
    
    /**
     * This method starts a Workflow.
     * 
     * For starting a workflow, a user has to be in at least one of the roles owned by the workflow's startstep. 
     * 
     * @param workflowID is the workflow, which should be started
     * @param username is the User, who starts the workflow
     * @return logicResponse of starting a workflow
     * @exception LogicException if an error in businesslogic occurs
     */
    LogicResponse startWorkflow(String workflowID, String username) throws LogicException;

    /**
     * This method stores a workflow and distributes a id. This method is used
     * for saving a new workflow and for editting a workflow. It is also used
     * for updating a workflow. If a completely new workflow should be saved it
     * doesn't have an id (means it's null or ""). If a workflow is to be edited
     * it already has an id. If a workflow doesn't have any unfinished items (or
     * none at all) it will be overwritten. Otherwise the "original" workflow
     * will be deactivated and a new workflow (with only steps, other attributes
     * are reseted) will be saved. Should a workflow be de-/activated then its
     * state will be setted on the original one which will be saved.
     * 
     * For adding a workflow, admin rights are required.
     * 
     * @param workflow is the workflow which should be added
     * @return logicResponse of adding a workflow
     * @exception LogicException if an error in businesslogic occurs
     */
    LogicResponse addWorkflow(Workflow workflow) throws LogicException; // later a workflows name will be given a name
                                                  

    /**
     * This method returns all workflows in persistence.
     * 
     * For retrieving all workflows, admin rights are required.
     * 
     * @return list of all workflows.
     * @throws PersistenceException if an error in persistence occurs
     * @throws NoPermissionException 
     */
    List<Workflow> getAllWorkflows() throws PersistenceException, NoPermissionException;


     /**
     * This method returns all workflows in persistence that are not marked inactive.
     * 
     * For retrieving all active workflows, admin rights are required.
     * 
     * @return all active workflows in persistence
     * @throws PersistenceException if an error in persistence occurs
     */
    List<Workflow> getAllActiveWorkflows() throws PersistenceException;
   
    /**
     * This method loads a Workflow.
     * 
     * For retrieving a workflow, admin rights are required.
     * 
     * @param workflowID is the id of the given worklow
     * @return a Workflow, if there is one, who has this workflowID
     * @throws PersistenceException if an error in persistence occurs
     */
    Workflow getWorkflow(String workflowID) throws PersistenceException;

    /**
     * This method delete a Workflow in Persistence.
     * 
     * For deleting a workflow, admin rights are required.
     * 
     * @param workflowID
     *            describe the Workflow
     * @return logicResponse of deleting a workflow
     * @throws PersistenceException if an error in persistence occurs
     */
    LogicResponse deleteWorkflow(String workflowID)
            throws PersistenceException;

    /*
     * item
     */
    /**
     * This method execute a step in an item.
     * 
     * For stepping forward, the user needs to have at least one of the roles owned by the responsible item step.
     * 
     * @param itemId is the itemId of the given item
     * @param stepId the stepId of the responsible item step
     * @param username is the name of the user who executes the step in the Item
     * @exception LogicException if an error in businesslogic occurs 
     * @return the logicResponse of stepForward
     */
    LogicResponse stepForward(String itemId, String stepId, String username) 
            throws LogicException;

    /**
     * This method activates a workflow.
     * 
     * For activating a workflow, admin rights are required.
     * 
     * @return the LogicResponse of an activated workflow
     * @param workflowID the id of the workflow which should be deactivate
     * @throws PersistenceException if an error in persistence occurs
     * @throws PersistenceException 
     **/
    LogicResponse activateWorkflow(String workflowID) throws PersistenceException;
   

    /**
     * This method add a step into an existing Workflow.
     * 
     * For adding a step, admin rights are required.
     * 
     * @param workflowID the workflow, which shall edited
     * @param stepId is the Id the step, which shall added
     * @return logicResponse of adding a step
     * @throws PersistenceException if an error in persistence occurs
     */
    LogicResponse addStep(String workflowID, Step stepId) throws PersistenceException;

    /**
     * This method deletes a step from an existing Workflow.
     * 
     * For deleting a step, admin rights are required.
     * 
     * @param workflowID is the id of the workflow, which shall edited
     * @param stepID is the id of the step, which shall delete
     * @return logicResponse of deleting a step
     * @throws PersistenceException if an error in persistence occurs
     * @throws PersistenceException   
     */
    LogicResponse deleteStep(String workflowID, String stepID) throws PersistenceException;

    /**
     * This method stores a user and distributes a id.
     * 
     * For storing a user, admin rights are required.
     * 
     * @param user is the given user which should be added
     * @return logicResponse of adding a user
     * @exception LogicException if an error in businesslogic occurs 
     */
    LogicResponse addUser(User user) throws LogicException;

    /**
     * This method loads a User.
     * 
     * For loading a user, admin rights are required.
     * 
     * @param username describe the user
     * @return a User, if there is one, who has this username
     * @throws PersistenceException if an error in persistence occurs 
     * @throws PersistenceException  
     */
    User getUser(String username) throws PersistenceException; // not there yet

    /**
     * This method checks the login data of a user. Username and password will always be checked. When logging in
     * via the adminClient(adminRequired flag), admin rights will be checked in addition.
     * 
     * @param username of the user, to be checked
     * @param password of the user, to be checked
     * @param adminRequired flag whether or not admin check is necessary
     * @return if user correct true, else false
     * @exception LogicException if an error in businesslogic occurs
     */
    boolean checkLogIn(String username, String password, boolean adminRequired) throws LogicException;
    
    /**
     * This method checks if the logged in User is an admin or not.
     * 
     * @param username of the user to be checked
     * @return true or false
     * @throws PersistenceException if there is a problem with the Persistence
     */
    boolean checkUserIsAdmin(String username) throws PersistenceException;

    /**
     * This method deletes a User.
     * 
     * For deleting a user, admin rights are required.
     * 
     * @param username describes the user
     * @exception UserNotExistentException if the given user doesnt exist in the persistence
     * @throws UserNotExistentException
     * @return logicResponse of deleting a user
     */
    LogicResponse deleteUser(String username) throws UserNotExistentException;

    /**
     * This method returns all workflows, in which the user is involved, no
     * matter if there is an open or busy item for him Mind that this method
     * won't return the items only a list of workflows.
     * 
     * For retrieving all workflows for a user, the user has to be logged in.
     * 
     * @param username is the username of the user whose workflows' is looked for
     * @exception LogicException if an error in businesslogic occurs
     * @return a LinkedList of workflows
     */
    List<Workflow> getAllWorkflowsForUser(String username) throws LogicException;

    /**
     * Method for getting a list of the ids of workflows startable by a given user (if the user is responsible for the Startstep of the steplist).
     * 
     * For retrieving all startable workflows for a user, the user has to be logged in.
     * 
     * @param username is the name and describes the given user
     * @exception LogicException if an error in businesslogic occurs
     * @return List<Integer> list of Ids
     */
    List<String> getStartableWorkflowsForUser(String username) throws LogicException;
   
    /**
     * Method for getting a list of ids of the items relevant to an user (if he's responsible for a step in the steplist).
     * 
     * For retrieving all relevant items for a user, the user has to be logged in.
     * 
     * @param workflowId is the id of the given workflow
     * @param username desribes the given user
     * @throws PersistenceException if an error in persistence occurs
     * @throws PersistenceException  
     * @return List<Integer> list of stepIds
     */
    List<Item> getRelevantItemsForUser(String workflowId, String username) throws PersistenceException;

    /**
     * method to load a specific item from persistence.
     * 
     * For retrieving an item, the user needs to have at least one of the roles owned by the requested item.
     * 
     * @param itemID - a items unique string id 
     * @param username - userId
     * @return Item - requested item from persistence
     * @throws LogicException if an error in persistence or a permission error occurs
     */
    Item getItem(String itemID, String username) throws LogicException;
    
    
    // Business Logic Sprint 2
    
    /**
     * Method for getting a list of all the existing roles in the persistance.
     * 
     * For retrieving all roles, admin rights are required.
     * 
     * @return list of all roles
     * @throws PersistenceException if an error in persistence occurs
     */
    List<Role> getAllRoles() throws PersistenceException;

    /**
     * Method for returning a list of all the existing users in the persistence.
     * 
     * for retrieving all users, admin rights are required.
     * 
     * @return list of all users
     * @throws PersistenceException if an error in persistence occurs
     * @throws PersistenceException  
     */
    List<User> getAllUsers() throws PersistenceException;
    
    /**
    * This method return all users in persistence that are not marked inactive.
    * 
    * for retrieving all active users, admin rights are required.
    * 
    * @return all active users in persistence
    * @throws PersistenceException if an error in persistence occurs
    */
    List<User> getAllActiveUsers() throws PersistenceException;
    
    /**
     * Method for adding a new role in the persistence.
     * 
     * for storing a role, admin rights are required.
     * 
     * @param role is the role we want to add
     * @return LogicResponse object
     * @throws PersistenceException if persistence errors occur
     */
    LogicResponse addRole(Role role) throws PersistenceException;

    /**
     * This method adds a role to a user.
     * 
     * For adding a role to a user, admin rights are required.
     * 
     * @param user - the user to which the role shall be added
     * @param role - role to be added
     * @return LogicResponse object
     * @throws PersistenceException if persistence errors occur
     */
    LogicResponse addRoleToUser(User user, Role role) throws PersistenceException;
    
    /**
     * Method for deleting an existing role from the persistence. The users who
     * have this role will lose it too.
     * 
     * for deleting a role, admin rights are required.
     * 
     * @param rolename of the role
     * @return LogicResponce object
     * @throws LogicException 
     */
    LogicResponse deleteRole(String rolename) throws LogicException;
    
    /**
     * Method for giving a List of items of a user which are all open.
     * 
     * For retrieving all open items for a user, the user must be logged in.
     * 
     * @param username describes the given user
     * @exception LogicException if an error in businesslogic occurs
     * @throws PersistenceException  
     * @return List<Item> is the list of items we want to get
     */
    List<Item> getOpenItemsByUser(String username) throws LogicException;

    /**
     * method to load a specific role from persistence.
     * 
     * For retrieving a role, admin rights are required.
     * 
     * @param rolename a roles unique string id
     * @return Role - requested Role from persistence
     * @throws PersistenceException if an error in persistence occurs
     */
    Role getRole(String rolename) throws PersistenceException;
    
    /**
     * Method for adding a form to persistence.
     * 
     * For storing a form, admin rights are required.
     * 
     * @param form which should be added
     * @return logicResponse which contains definition notification
     * @throws PersistenceException if an error in persistence occurs
     */
    LogicResponse addForm(Form form) throws PersistenceException;
    
    /**
     * Method for deleting a form in persistence. Only possible if form isn't currently used.
     * 
     * For deleting a form, admin rights are required.
     * 
     * @param formId indicates which form should be deleted
     * @return logicResponse which contains deletion notification
     * @throws PersistenceException if an error in persistence occurs
     */
    LogicResponse deleteForm(String formId) throws PersistenceException;
    
    /**
     * method to load a specific form from persistence.
     * 
     * For loading a form, admin rights are required.
     * 
     * @param formId a forms unique string id
     * @return Form - requested Form from persistence
     * @throws PersistenceException if an error in persistence occurs
     */
    Form getForm(String formId) throws PersistenceException;
    
    /**
     * Method for updating an item. Suitable item from a workflow in persistence will be overwritten.
     * 
     * For updating an item, the user needs to have a role owned by the item and needs to be correctly logged in.
     * 
     * @param item contains changes, will be used to overwrite item in workflow
     * @param username indicates user who wants to update item
     * @return logicResponse which contains update notification
     * @throws LogicException if an error in persistence occurs
     */
    LogicResponse updateItem(Item item, String username) throws LogicException;
    
    /**
     * Method for getting all available forms in persistence.
     * 
     * For retrieving all forms, admin rights are required.
     * 
     * @return list of of all forms in persistence
     * @throws PersistenceException 
     */
    List<Form> getAllForms() throws PersistenceException;

    /**
     * Method for saving all relevant data from server. Calls the method persistence.save().
     */
    void saveData();
    
    /**
     * Method to load the data model from filesystem. 
     * File is read, in case of reading errors or missing file some default test data 
     * is load. Method calls load() in persistence.
     */
    void loadData();

    /**
     * Sets the storagePath from config file to logic.
     * @param storagePath the path for persist the data model
     */
    void setStoragePath(String storagePath);
}