package de.hsrm.swt02.model;

import java.util.Arrays;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import com.fasterxml.jackson.annotation.JsonIgnore;

/**
 * This class represents an User. A User is a manifestation of a RootElement.
 */
public class User extends RootElement {
   
    @JsonIgnore
    private static final long serialVersionUID = 2333696087836440391L;
    
   // Used for (de)serialization. Do not change.
    private Set<Role> roles;
    // Used for (de)serialization. Do not change.
    private List<String> messagingSubs;
    // a users private password
    private String password;
    // Used for (de)serializsation. Do not change.
    private boolean active;

    /**
     * Constructor for User.
     */
    public User() {
        super();
        roles = new HashSet<Role>();
        setActive(true);
        password = "";
    }

    /**
     * Username getter.
     * 
     * @return username is the username
     */
    public String getUsername() {
        return id;
    }

    /**
     * Username setter.
     * 
     * @param username
     *            is the username
     */
    public void setUsername(String username) {
        this.id = username;
    }
    
    /**
     * Password getter.
     * 
     * @return password - a users password
     */
    public String getPassword() {
        return this.password;
    }
    
    /**
     * Password setter.
     * 
     * @param password - a users password
     */
    public void setPassword(String password) {
        this.password = password;
    }
    
    /**
     * Active Getter getter.
     * 
     * @return active if the user is active or not
     */
    public boolean isActive() {
        return active;
    }

    /**
     * Active setter.
     * 
     * 
     * @param active
     *            if the user is active or not
     */
    public void setActive(boolean active) {
        this.active = active;
    }

    /**
     * Roles getter. There is no setter because roles is an HashSet.
     * 
     * @return roles is the list of roles of the user
     */
    public Set<Role> getRoles() {
        return this.roles;
    }
    
    /**
     * adds a new role to this user.
     * 
     * @param role to add
     */
    public void addRole(Role role) {
        this.roles.add(role);
    }
    
    /**
     * removes a role from this user.
     * 
     * @param role to remove
     */
    public void removeRole(Role role) {
        if (this.roles.contains(role)) {
            this.roles.remove(role);            
        }
    }
    
    /**
     * checks if this user has a certain role.
     * 
     * @param role to check
     * @return boolean if this user has role
     */
    public boolean hasRole(Role role) {
        return this.roles.contains(role);
    }
    
    /**
     * Subs getter. There is no setter because roles is an ArrayList.
     * 
     * @return roles is the list of roles of the user
     */
    public List<String> getMessagingSubs() {
        return this.messagingSubs;
    }

    /**
     * Init method.
     * 
     * @param u
     *            is the user we want to init
     */
    public void init(User u) {
        super.init(u);
        setUsername(u.getUsername());
        setActive(u.isActive());
        setPassword(u.getPassword());
    }

    /**
     * Deep Copy - Cloning method for Actions.
     * 
     * @exception CloneNotSupportedException
     *                clone convention
     * @throws CloneNotSupportedException
     * @return clone is the clone of the action
     */
    public Object clone() throws CloneNotSupportedException {
        final User clone = new User();
        clone.init(this);

        for (Role role : this.roles) {
            final Role cloneRole = (Role) role.clone();
            clone.getRoles().add(cloneRole);
        }
        
        clone.setPassword(this.password);

        return clone;
    }
    
    @Override
    public String toString() {
        String ret = "";
        ret += "User: " + this.id + "\n";
        ret += "\tActive: " + this.isActive() + "\n";
        ret += "\tRollen: " + Arrays.toString(this.getRoles().toArray()) + "\n";
        
        return ret;
    }
}