package de.hsrm.swt02.model;

/**
 * This class represents a StartStep. A StartStep is a manifestation of a Step.
 */
public class StartStep extends Step {

    /**
    * Constructor for StartStep without parameters.
    */
    public StartStep() {
        super();
    }

    /**
     * Constructor for StartStep with parameters.
     * @param username is the username of the user responsible for the startstep
     */
    public StartStep(String roleName) {
        super();
        this.roles.add(roleName);
    }
    
    /**
     * Deep Copy - Cloning method for Actions.
     * @exception CloneNotSupportedException convention
     * @throws CloneNotSupportedException
     * @return clone is the 
     */
    public Object clone() throws CloneNotSupportedException {
        final StartStep clone = new StartStep();
        clone.init(this);
        return clone;
    }
}
