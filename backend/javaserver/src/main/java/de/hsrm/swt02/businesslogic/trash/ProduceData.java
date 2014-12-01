package de.hsrm.swt02.businesslogic.trash;

import de.hsrm.swt02.model.Action;
import de.hsrm.swt02.model.FinalStep;
import de.hsrm.swt02.model.StartStep;
import de.hsrm.swt02.model.User;
import de.hsrm.swt02.model.Workflow;
import de.hsrm.swt02.persistence.Persistence;
import de.hsrm.swt02.persistence.exceptions.UserAlreadyExistsException;

public class ProduceData {

    private int countWorkflow = 0;
    private int countUser = 0;

    /**
     * This class create sample data and store it in persistence 0-4 workflows,
     * 0-4 User
     */

    public ProduceData(Persistence p) {
        for (int i = 0; i < 5; i++) {
            createUser(p);
            createWorkflow(p);
        }

    }

    /**
     * Create a sample User with userId = countUser, Username = "countUser"
     */
    public void createUser(Persistence p) {
        User user = new User();
        user.setId(countUser);
        user.setUsername(countUser + "");
        try {
            p.addUser(user);
        } catch (UserAlreadyExistsException e) {
            // Loggin
        }
        countUser++;
    }

    /**
     * Create a sample Workflow with Startstep, 3 Actions, FinalStep. Each step
     * is a user assigned
     * 
     * @param p persistence
     */
    public void createWorkflow(Persistence p) {

        Workflow w = new Workflow(countWorkflow);
        w.addStep(new StartStep((int) (Math.random() * 4) + ""));
        w.addStep(new Action(countWorkflow * 100 + 1, (int) (Math.random() * 4)
                + "", "Eins"));
        w.addStep(new Action(countWorkflow * 100 + 2, (int) (Math.random() * 4)
                + "", "Zwei"));
        w.addStep(new Action(countWorkflow * 100 + 3, (int) (Math.random() * 4)
                + "", "Drei"));
        w.addStep(new FinalStep());

        p.storeWorkflow(w);

        countWorkflow++;
    }

}
