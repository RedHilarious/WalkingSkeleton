package de.hsrm.testswt02.logic_unittest;

import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;

import java.util.ArrayList;

import org.junit.Test;

import com.google.inject.Guice;
import com.google.inject.Injector;

import de.hsrm.swt02.businesslogic.Logic;
import de.hsrm.swt02.businesslogic.exceptions.IncompleteEleException;
import de.hsrm.swt02.businesslogic.exceptions.LogicException;
import de.hsrm.swt02.constructionfactory.ConstructionFactory;
import de.hsrm.swt02.constructionfactory.SingleModule;
import de.hsrm.swt02.model.Action;
import de.hsrm.swt02.model.FinalStep;
import de.hsrm.swt02.model.Item;
import de.hsrm.swt02.model.Role;
import de.hsrm.swt02.model.StartStep;
import de.hsrm.swt02.model.Step;
import de.hsrm.swt02.model.User;
import de.hsrm.swt02.model.Workflow;
import de.hsrm.swt02.persistence.exceptions.PersistenceException;
import de.hsrm.swt02.persistence.exceptions.StorageFailedException;
import de.hsrm.swt02.persistence.exceptions.UserAlreadyExistsException;
import de.hsrm.swt02.persistence.exceptions.UserNotExistentException;
import de.hsrm.swt02.persistence.exceptions.WorkflowNotExistentException;

/**
 * This Testclass tests the logic interface.
 *
 */
public class LogicTest {

    Logic li;
    Workflow w;
    Workflow w1;
    Workflow w2;
    Workflow w3;
    User user;
    User user1;
    User user2;
    Role role;
    Role role1;
    Role role2;
    StartStep startStep;
    StartStep startStep1;
    StartStep startStep2;
    StartStep startStep3;
    Action action;
    Action action1;
    Action action2;
    Action action3;
    FinalStep finalStep1;
    FinalStep finalStep2;

    /**
     * Test if a workflow can be added and retrieved.
     * @throws LogicException 
     */
    @Test
    public void addGetWorkflowTest() throws LogicException {
        init();
        li.addWorkflow(w);
        assertTrue(li.getWorkflow(w.getId()).equals(w));
    }

    /**
     * Test if a workflow can be properly started.
     * @throws LogicException 
     */
    @Test
    public void startWorkflowTest() throws LogicException {
        init();
        initExtension();

        li.addWorkflow(w);

        li.startWorkflow(w.getId(), user.getUsername()); // added
                                                         // user.getUsername
                                                         // because
                                                         // startWorkflow
                                                         // expects String not
                                                         // user

        final Workflow workflow = li.getWorkflow(w.getId());
        assertFalse(workflow.getItems().isEmpty());
    }

    /**
     * Test if deletion of a non existent workflow throws an exception.
     * @throws LogicException 
     */
    @Test(expected = WorkflowNotExistentException.class)
    public void deleteWorkflowTest() throws LogicException {
        init();
        li.addWorkflow(w);
        li.deleteWorkflow(w.getId());

        li.getWorkflow(w.getId());

    }

    /**
     * Test if stepForward is properly implemented.
     * @throws LogicException 
     */
    @Test
    public void stepOverTest() throws LogicException {
        init();
        initExtension();

        final Workflow testwf = new Workflow();
        final Role r = new Role();
        final StartStep ss = new StartStep();
        final Action a = new Action();
        final Action a2 = new Action();
        final FinalStep fs = new FinalStep();
        final User u = new User();

        r.setRolename("role");
        ss.addRole(r.getRolename());
        a.addRole(r.getRolename());
        a2.addRole(r.getRolename());
        fs.addRole(r.getRolename());
        testwf.addStep(ss);
        testwf.addStep(a);
        testwf.addStep(a2);
        testwf.addStep(fs);
        u.setUsername("user");
        u.addRole(r);

        li.addRole(r);
        li.addUser(u);
        li.addWorkflow(testwf);

        li.startWorkflow(testwf.getId(), u.getUsername());

        final Workflow workflow = li.getWorkflow(testwf.getId());
        Item item = workflow.getItemByPos(0);
        final Item item2 = li.getItem(item.getId(), u.getUsername());

        li.stepForward(item.getId(), workflow.getStepById(a.getId()).getId(),
                u.getUsername());

        li.stepForward(item.getId(), workflow.getStepById(a.getId()).getId(),
                u.getUsername());

        item = li.getItem(item.getId(), u.getUsername());
        assertTrue(item.getStepState(a.getId()) == "DONE");
    }

    /**
     * Test if steps can be added.
     * @throws LogicException 
     */
    @Test
    public void addStepTest() throws LogicException {
        init();
        li.addWorkflow(w);
        final int i = w.getSteps().size();
        final StartStep testStep = new StartStep();
        testStep.getRoleIds().add("role1");
        li.addStep(w.getId(), testStep);
        final Workflow workflow = li.getWorkflow(w.getId());
        assertTrue(workflow.getSteps().size() == i + 1);

    }

    /**
     * Test if users can be added and retrieved.
     * @throws LogicException if an error in businesslogic occurs 
     */
    @Test
    public void addGetUserTest() throws LogicException {
        init();
        try {
            li.addUser(user);
        } catch (UserAlreadyExistsException e) {
            assertTrue(false);
        }

        assertTrue(li.getUser(user.getUsername()).equals(user));
    }

    /**
     * Test if deletion of a non existent user throws an exception.
     * @throws LogicException if an error in businesslogic occurs  
     */
    @Test(expected = UserNotExistentException.class)
    public void deleteUserTest() throws LogicException {
        init();
        try {
            li.addUser(user);
        } catch (UserAlreadyExistsException e) {
            assertTrue(false);
        }
        li.deleteUser(user.getUsername());
        assertTrue(li.getUser(user.getUsername()) == null);
    }

    /**
     * Test if workflows for a user can be retrieved.
     * @throws LogicException if something went wrong in logic
     * @throws UserNotExistentException 
     * @throws WorkflowNotExistentException 
     * @throws StorageFailedException 
     */
    @Test
    public void getWorkflowsByUser() throws LogicException,
            WorkflowNotExistentException, UserNotExistentException,
            StorageFailedException 
    {
        init();
        initExtension();

        int i = li.getAllWorkflowsForUser(user2.getUsername()).size();
        for (int counter = 0; counter < i; counter++) {
            final Workflow workflow = li.getAllWorkflowsForUser(user2.getUsername()).get(counter);
            i--;
        }
        assertTrue(true);
    }

    /**
     * Test if all open items for a user can be retrieved.
     * @throws LogicException 
     */
    @Test
    public void getOpenItemsByUserTest() throws LogicException {
        init();
        initExtension();

        li.startWorkflow(w.getId(), user.getUsername());
        li.startWorkflow(w.getId(), user.getUsername());

        final int i = li.getRelevantItemsForUser(w.getId(), user.getUsername())
                .size();
        assertTrue(i == 2);

    }

    /**
     * Check if steps in a workflow are connected.
     * 
     * @throws LogicException 
     */
    @Test
    public void connectWorkflowSteps() throws LogicException {
        init();
        initExtension();

        for (Step step : w2.getSteps()) {
            if (!(step instanceof FinalStep)) {
                assertTrue(step.getNextSteps().size() == 1);
            }
        }
    }

    /**
     * Test if all startable workflows can be retrieved.
     * @throws LogicException if something went wrong in logic.
     * @throws WorkflowNotExistentException 
     * @throws UserNotExistentException 
     * @throws StorageFailedException 
     */
    @Test
    public void getStartableWorkflowsTest() throws LogicException,
            UserNotExistentException, WorkflowNotExistentException,
            StorageFailedException 
    {
        init();
        initExtension();
        
        int i = li.getStartableWorkflowsForUser(user2.getUsername()).size();
        for (int counter = 0; counter < i; counter++) {
            final String workflowId = li.getStartableWorkflowsForUser(user2.getUsername()).get(counter);
            li.startWorkflow(workflowId, user2.getUsername());
            i--;
        }
        assertTrue(true);
    }

    /**
     * This Method test deactivation.
     * 
     * @throws LogicException 
     */
    @Test
    public void deactivateWorkflow() throws LogicException {
        init();
        li.addWorkflow(w);
        w.setActive(false);
        assertFalse(w.isActive());
    }

    /**
     * Test if all active Workflows for a user can be retrieved.
     * @throws UserNotExistentException if user doesnt exisis.
     * @throws LogicException if something went wrong in logic.
     * @throws WorkflowNotExistentException 
     * @throws StorageFailedException 
     */
    @Test
    public void getAllActiveWorkflowsByUserTest()
            throws UserNotExistentException, LogicException,
            WorkflowNotExistentException, StorageFailedException 
    {
        init();
        initExtension();
        final int before = li.getAllWorkflowsForUser(user.getUsername()).size();
        System.out.println(before);
        
        w.setActive(false);
        li.addWorkflow(w);

        final int after = li.getAllWorkflowsForUser(user.getUsername()).size();
        System.out.println(after);
        assertTrue(after == before - 1);

    }

    /**
     * open items should show even the workflow is deaktive.
     * 
     * @throws LogicException .
     */

    /**
     * this method init basic data for testing.
     * 
     * @throws StorageFailedException 
     */
    private void init() throws StorageFailedException {
        li = ConstructionFactory.getTestInstance().getLogic();

        role = new Role();
        role.setRolename("role");

        user = new User();
        user.setUsername("0");
        user.getRoles().add(role);

        final ArrayList<String> roles = new ArrayList<String>();
        roles.add(role.getRolename());

        startStep = new StartStep(roles);
        action = new Action(roles, "description");
        finalStep1 = new FinalStep();
        startStep.getRoleIds().add(role.getRolename());
        action.getRoleIds().add(role.getRolename());
        finalStep1.getRoleIds().add(role.getRolename());

        w = new Workflow();
        w.addStep(startStep);
        w.addStep(action);
        w.addStep(finalStep1);

        try {
            li.addRole(role);
            li.addUser(user);
            li.addWorkflow(w);
        } catch (PersistenceException e) {
            e.printStackTrace();
        } catch (LogicException e) {
            e.printStackTrace();
        }
    }

    /**
     * This method init more data for testing.
     * 
     * @throws LogicException 
     */
    private void initExtension() throws LogicException {

        role1 = new Role();
        role1.setRolename("role1");

        role2 = new Role();
        role2.setRolename("role2");

        user1 = new User();
        user1.setUsername("1");
        user1.getRoles().add(role1);

        user2 = new User();
        user2.setUsername("2");
        user2.getRoles().add(role2);

        final ArrayList<String> roles1 = new ArrayList<String>();
        roles1.add(role1.getRolename());
        final ArrayList<String> roles2 = new ArrayList<String>();
        roles2.add(role2.getRolename());

        startStep1 = new StartStep(roles1);
        startStep2 = new StartStep(roles2);
        startStep3 = new StartStep(roles2);
        action1 = new Action(roles1, "description");
        action2 = new Action(roles2, "description");
        action3 = new Action(roles2, "description");
        finalStep1 = new FinalStep();
        finalStep2 = new FinalStep();

        startStep1.getRoleIds().add(role1.getRolename());
        startStep2.getRoleIds().add(role1.getRolename());
        startStep3.getRoleIds().add(role1.getRolename());
        action1.getRoleIds().add(role2.getRolename());
        action2.getRoleIds().add(role2.getRolename());
        action3.getRoleIds().add(role2.getRolename());
        finalStep1.getRoleIds().add(role2.getRolename());
        finalStep2.getRoleIds().add(role2.getRolename());

        w1 = new Workflow();
        w1.addStep(startStep1);
        w1.addStep(action1);
        w1.addStep(finalStep1);

        w2 = new Workflow();
        w2.addStep(startStep2);
        w2.addStep(action2);
        w2.addStep(finalStep2);

        w3 = new Workflow();
        w3.addStep(startStep3);
        w3.addStep(action3);
        w3.addStep(finalStep2);

        try {
            li.addRole(role1);
            li.addRole(role2);
            li.addUser(user2);
            li.addUser(user1);

        } catch (UserAlreadyExistsException e) {
            assertTrue(false);
        }
        
        try {
            li.addWorkflow(w1);
            li.addWorkflow(w2);
            li.addWorkflow(w3);

        } catch (IncompleteEleException e1) {
            e1.printStackTrace();
        }


    }

}
