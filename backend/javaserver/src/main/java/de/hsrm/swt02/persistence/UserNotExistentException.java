package de.hsrm.swt02.persistence;

public class UserNotExistentException extends Exception {

    private static final long serialVersionUID = 4657473446452493460L;

    public UserNotExistentException() {
        super();
    }

    public UserNotExistentException(String msg) {
        super(msg);
    }
}
