package de.hsrm.swt02;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Properties;
import java.util.logging.Handler;
import java.util.logging.Level;
import java.util.logging.Logger;

import de.hsrm.swt02.constructionfactory.ConstructionFactory;
import de.hsrm.swt02.logging.LogConfigurator;
import de.hsrm.swt02.logging.UseLogger;
import de.hsrm.swt02.messaging.ServerPublisher;
import de.hsrm.swt02.messaging.ServerPublisherBrokerException;
import de.hsrm.swt02.restserver.RestServer;

/**
 * Class for application start. Contains the main-method for the server.
 * Loads the logging configuration. Initiates business logic.
 * Starts the Rest-server.
 */
public class App {
    
    private static boolean isShuttingDown;
    private static Object synchronizer = new Object();
    private static UseLogger logger = new UseLogger();
    
    /**
     * Application startup method.
     * Configures logging.
     * Starts the Rest-Server.
     * 
     * @param args are the program start parameters
     */
    public static void main(String[] args) {
        
        final RestServer server;
        final BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));
        String userInput = "";

        final Properties properties = new Properties();
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
        
        // setup log configuration
        LogConfigurator.setup(properties);
        
        // performance optimization
        new Thread(new Runnable() {
            public void run() {
                ConstructionFactory.getInstance(properties);
            }
        }).start();
        
        // start rest-server instance
        server = new RestServer(properties);
        
        // ShutDownHook
        Runtime.getRuntime().addShutdownHook(new Thread() {
            public void run() {
                shutdown(server);             
            }
        });
        
        server.startHTTPServer();
            
        /*
         * STOP MECHANISM
         */  
        while (!userInput.equals("stop")) {
            try {
                userInput = reader.readLine();
            } catch (IOException e) {
                System.err.println(e.getMessage());
            }
        }
        
        shutdown(server);
    }
    
    /** Stops the HTTP-server.
     *  Stops the message broker.
     *  Closes all logging file handlers. 
     * 
     * @param server the server to shutdown
     */
    public static void shutdown(RestServer server) {
        /* Prevents that a thread sets the shutdown-variable
         * while another thread refers to it.
         * Otherwise it could cause a double shutdown 
         * which would not work at all.
         */
        synchronized (synchronizer) {
            if (isShuttingDown) {
                return;
            }
            isShuttingDown = true;
        }
        
        final Logger rootLogger = Logger.getLogger("");
        final Handler[] handlers = rootLogger.getHandlers();
        final ConstructionFactory factory = ConstructionFactory.getInstance(new Properties());
        final ServerPublisher publisher;
        
        // stop the server    
        if (server != null) {
            server.stopHTTPServer(true);
        }
        
        // stop the Message broker
        try {
            publisher = factory.getPublisher();
            if (publisher != null) {
                publisher.stopBroker();
            }
        } catch (ServerPublisherBrokerException e) {
            System.err.println(e.getMessage());
        }
        
        // close logging handler
        for (Handler handler : handlers) {
            handler.close();
        }
    }
}
