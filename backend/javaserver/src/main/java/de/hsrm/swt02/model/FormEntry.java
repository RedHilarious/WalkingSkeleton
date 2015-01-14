package de.hsrm.swt02.model;

/**
 * This class represents an entry of a form definition.
 *
 */
public class FormEntry extends RootElement {
    
    
    private static final long serialVersionUID = -5022106078170690185L;
    private String value;
    
    /**
     * Default-Constructor.
     */
    public FormEntry() {
        
    }

    /**
     * Getter for key (== id).
     * @return id
     */
    public String getKey() {
        return id;
    }

    /**
     * Setter for key (== id).
     * @param key indicates value to be setted
     */
    public void setKey(String key) {
        this.id = key;
    }

    /**
     * Getter for value.
     * @return value
     */
    public String getValue() {
        return value;
    }

    /**
     * Setter for value.
     * @param value indicates value to be setted
     */
    public void setValue(String value) {
        this.value = value;
    }
    
    /**
     * Clone-Method for object.
     * @return clone of this object
     */
    public Object clone() {
        final FormEntry clone = new FormEntry();
        
        clone.setKey(id);
        clone.setValue(value);
        return clone;
    }
    
    /**
     * String view of object.
     * @return string view of object
     */
    public String toString() {
        String str = "";
        str += "\t\tKey: " + this.id + "\n";
        str += "\t\tValue: " + this.value + "\n";
        return str;
    }

}
