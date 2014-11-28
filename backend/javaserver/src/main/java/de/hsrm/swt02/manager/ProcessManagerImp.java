package de.hsrm.swt02.manager;

import java.util.Observable;
import java.util.Observer;

import de.hsrm.swt02.messaging.ServerPublisher;
import de.hsrm.swt02.messaging.ServerPublisherBrokerException;
import de.hsrm.swt02.model.Action;
import de.hsrm.swt02.model.Item;
import de.hsrm.swt02.model.Step;
import de.hsrm.swt02.model.User;
import de.hsrm.swt02.persistence.Persistence;
import de.hsrm.swt02.processors.ActionProcessor;

import com.google.inject.Inject;
import com.google.inject.Singleton;

/**
 * This class handles the processing of Steps. (For now) it provides methods for
 * checking whether an user can execute a step and selecting the right processor
 * for a step.
 *
 */
@Singleton
public class ProcessManagerImp implements Observer, ProcessManager {

    private ServerPublisher sp;
    private Persistence p;

    /**
     * Constructor of ProcessManager
     */
    @Inject
    public ProcessManagerImp(ServerPublisher sp, Persistence p) {
        this.sp = sp;
        this.p = p;
    }

    /**
     * IMPORTANT: For walking skeleton this method is not used! This method
     * checks if the user who wishes to edit a step is the responsible user who
     * is allowed to execute the step.
     * 
     * @param user who edits the step
     * @param step which user wants to edit
     * @return true if user is "owner" of step and false if not
     */
    public boolean checkUser(User user, Step step) {

        if (step instanceof Action) {
            return user.getUsername().equals(((Action) step).getUsername());
        }
        return false;
    }

    /**
     * This method selects the processor of a step and executes it.
     * 
     * @param step which is to be edited
     * @param item which is currently active
     * @param user who started interaction
     */
    public void selectProcessor(Step step, Item item, User user) {

        if (step instanceof Action) {
            ActionProcessor actionProcessor = new ActionProcessor(p);
            actionProcessor.addObserver(this);
            actionProcessor.handle(item, step, user);
        }
    }

    /**
     * This method starts the messaging broker.
     */
    public void startBroker() {

        try {
            sp.startBroker();
        } catch (ServerPublisherBrokerException e) {
            // TODO Logging
        }

    }

    /**
     * This method stops the messaging broker.
     */
    public void stopBroker() {

        try {
            sp.stopBroker();
        } catch (ServerPublisherBrokerException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

    }

    /**
     * This method is executed if its observables notifies changes.
     */
    public void update(Observable o, Object arg) {

        try {
            sp.publish(
                    "item=" + ((Item) arg).getState() + "="
                            + ((Item) arg).getId(), "ITEMS_FROM_"
                            + ((Item) arg).getWorkflowId());
        } catch (Exception e) {
            // TODO Logging
        }
    }
}
