package de.hsrm.swt02.businesslogic.exceptions;

import de.hsrm.swt02.restserver.exceptions.BasicException;

public class LogicException extends BasicException {
    
    private static final long serialVersionUID = 3428135084952400723L;
    private int errorcode = 11000;
    
    public LogicException() {
        super();
    }

    public LogicException(String msg) {
        super(msg);
    }
    
    public int getErrorCode() {
        return this.errorcode;
    }

}