package de.hsrm.swt02.properties;

import java.io.BufferedInputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Properties;
import java.util.logging.Level;

import de.hsrm.swt02.logging.UseLogger;

/**
 * 
 * Class responsible for loading the properties initially. can be globally accessed to get Properties. Implements
 * singleton pattern.
 * 
 * @author akoen001
 *
 */
public class ConfigProperties {
    
    private static ConfigProperties theInstance;
    private Properties properties;
    private static UseLogger logger;
    
    /**
     * 
     * constructor.
     * 
     */
    private ConfigProperties() {
        logger = new UseLogger();
        properties = new Properties();
        BufferedInputStream stream;
        // read configuration file for rest properties
        try {
            stream = new BufferedInputStream(new FileInputStream(
                        "server.config"));
            properties.load(stream);
            stream.close();
        } catch (FileNotFoundException e) {
            logger.log(Level.SEVERE, "Configuration file not found!");
        } catch (IOException e) {
            logger.log(Level.SEVERE, "Can't read file!");
        } catch (SecurityException e) {
            logger.log(Level.SEVERE, "Read Access not granted!");
        }
    }
    
    /**
     * 
     * takes care of the fact that only one instance of this class exists.
     * 
     * @return theInstance instance of Properties
     */
    public synchronized static ConfigProperties getInstance() {
        if (theInstance == null) {
            theInstance = new ConfigProperties();
        }
        return theInstance;
    }
   
    /**
     * 
     * getter for the properties.
     * 
     * @return properties of the configFile
     */
    public Properties getProperties() {
        return this.properties;
    }
}