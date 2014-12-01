package de.hsrm.swt02.model;

import java.util.ArrayList;
import java.util.List;

import com.fasterxml.jackson.annotation.JsonIgnore;

/**
 * This class represents an Item. It extends the class RootElement, so it can have an Id.
 */
public class Item extends RootElement {
    // Used for (de)serialization. Do not change.
    private int workflowId;

    // Used for (de)serialization. Do not change.
    private List<MetaEntry> metadata;

    // Used for (de)serialization. Do not change.
    private boolean finished;

    // When an Item object is (de)serializated: state will be ignored because in the client item, there is no state.
    @JsonIgnore
    private String state;

    /**
     * Constructor for Item
     */
    public Item() {
        super();
        metadata = new ArrayList<MetaEntry>();
    }

    /**
     * WorkflowId getter
     * @return workflowId
     */
    public int getWorkflowId() {
        return workflowId;
    }

    /**
     * WorkflowId setter
     * @param workflowId
     */
    public void setWorkflowId(int workflowId) {
        this.workflowId = workflowId;
    }

    /**
     * Metadata (list of MetaEntries) getter
     * @return metadata
     */
    public List<MetaEntry> getMetadata() {
        return this.metadata;
    }

    /**
     * Finished getter
     * @return finished
     */
    public boolean isFinished() {
        return finished;
    }

    /**
     * Finished setter
     * @param finished
     */
    public void setFinished(boolean finished) {
        this.finished = finished;
    }
    
    /**
     * State getter
     * @return state
     */
    public String getState() {
        return state;
    }

    /**
     * State setter
     * @param state
     */
    public void setState(String state) {
        this.state = state;
    }

    /**
     * This method gets an entry of the metadata list which suits the group and
     * key parameters.
     * 
     * @param key is the id of an entry
     * @param group is the type of an entry
     * @return the suitable entry, else return NULL
     */
    public MetaEntry getEntry(String key, String group) {

        for (MetaEntry a : metadata) {
            if (a.getGroup().equals(group) && a.getKey().equals(key)) {
                return a;
            }
        }
        return null;
    }

    /**
     * This method gets the value of an entry.
     * 
     * @param key is the id of an entry
     * @param group is the type of an entry
     * @return the suitable entry value, if the entry was not found return NULL
     */
    public String getEntryValue(String key, String group) {

        MetaEntry ame = getEntry(key, group);

        if (ame != null) {
            return ame.getValue();
        }
        return null;
    }

    /**
     * This method returns the Metastate of an entry.
     * 
     * @param key is the id of an entry
     * @return the Metastate of the searched entry
     */
    public String getStepState(int key) {

        return MetaState.fromValue(getEntryValue(key + "", "step")).toString();
    }

    /**
     * This method gets a list which contains looked for entries.
     * 
     * @param group is the type of the entries which are looked for
     * @return a list of suitable entries
     */
    public List<MetaEntry> getForGroup(String group) {

        List<MetaEntry> list = new ArrayList<MetaEntry>();

        for (MetaEntry a : metadata) {
            if (a.getGroup().equals(group)) {
                list.add(a);
            }
        }
        return list;
    }

    public MetaEntry getActStep() {
        for (MetaEntry me : getForGroup("step")) {
            if (me.getValue() == MetaState.OPEN.toString()) {
                return me;
            }
        }
        return null;
    }

    /**
     * This method sets the value of an entry or adds a new entry.
     * 
     * @param key is the id of an entry
     * @param group is the type of an entry
     * @param value represents an entrie's content
     */
    public void set(String key, String group, String value) {

        MetaEntry ame = getEntry(key, group);

        if (ame != null) {
            ame.setValue(value);
        } else {
            MetaEntry entry = new MetaEntry();
            entry.setGroup(group);
            entry.setKey(key);
            entry.setValue(value);
            metadata.add(entry);
        }
    }

    /**
     * This method sets specifically the metastate of an step entry.
     * 
     * @param key is the id of an entry
     * @param value is the new state of an entry
     */
    public void setStepState(int key, String value) {

        set(Integer.toString(key), "step", value);
    }

    /**
     * This methods is just for the initial state setting of an step.
     * 
     * @param value has to be OPEN, for enabling very first Step
     */
    public void setFirstStepState(String value) {

        getForGroup("step").get(0).setValue(value);
    }
}
