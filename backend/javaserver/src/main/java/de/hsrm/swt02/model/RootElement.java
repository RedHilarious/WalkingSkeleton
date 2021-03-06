package de.hsrm.swt02.model;

import java.io.Serializable;

import com.fasterxml.jackson.annotation.JsonIgnore;


/**
 * This class represents a RootElement.
 */
public class RootElement implements Cloneable, Serializable {
    
    @JsonIgnore
    private static final long serialVersionUID = -5434330163538855058L;
    
    // Used for (de)serialization. Do not change.
    protected String id;

    /**
     * Constructor for RootElement.
     */
    public RootElement() {

    }

    /**
     * Init method for cloning.
     * @param re is the rootelement we want to clone
     */
    public void init(RootElement re) {
        setId(re.getId());
    }

    /**
     * clone method for RootElement.
     * @exception CloneNotSupportedException cobnvention
     * @throws CloneNotSupportedException
     * @return clone is the requested clone
     */
    public Object clone() throws CloneNotSupportedException {
        final RootElement clone = new RootElement();
        clone.init(this);
        super.clone();

        return clone;
    }

    /**
     * Id getter.
     * 
     * @return id of the RootElement
     */
    public String getId() {
        return id;
    }

    /**
     * Id setter.
     * 
     * @param id
     *            of the RootElement
     */
    public void setId(String id) {
        this.id = id;
    }
    
    @Override
    public int hashCode() {
        return getId().hashCode();
    }
    
    @Override
    public boolean equals(Object object) {
        if (object instanceof RootElement) {
            if (((RootElement) object).getId() == null || this.getId() == null) {
                return super.equals(object);
            } else if (this.getId().equals(((RootElement)object).getId())) {
                return true;
            }
        }
        return false;
    }
    
    @Override
    public String toString() {
        String ret = "";
        ret += this.getClass().getSimpleName() + " ";
        ret += this.id;
        
        return ret;
    }
}
