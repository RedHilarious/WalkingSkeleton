package de.hsrm.swt02.restserver;

import java.io.BufferedInputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.net.URI;
import java.util.Properties;
import java.util.logging.Level;

import org.glassfish.jersey.jdkhttp.JdkHttpServerFactory;
import org.glassfish.jersey.server.ResourceConfig;

import com.sun.net.httpserver.HttpServer;

import de.hsrm.swt02.logging.UseLogger;

/**
 * This class defines the REST-Server.
 * 
 */
public class RestServer {

    private static final int WAITING_TIME = 5;
    private UseLogger logger = new UseLogger();
    private HttpServer server;
    private String baseURI;

    /** 
     * Constructor.
     */
    public RestServer() {
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
        baseURI = properties.getProperty("RestServerURI");
    }

    /**
     * Starts HTTP-server to run REST on.
     */
    public void startHTTPServer() {
        final ResourceConfig rc = new ResourceConfig();
        rc.packages("de.hsrm.swt02.restserver.resource");
        server = JdkHttpServerFactory.createHttpServer(URI.create(baseURI), rc);
        logger.log(Level.INFO, "HTTP-Server started...");
    }

    /**
     * Stops HTTP-server. The server waits a few seconds before it stops. Giving
     * all open requests enough time to run through.
     * 
     * @param forceServerStop forces the HTTP-Server to stop without waiting
     */
    public void stopHTTPServer(boolean forceServerStop) {
        if (server == null) {
            return;
        }

        if (forceServerStop) {
            server.stop(0);
        } else {
            server.stop(WAITING_TIME);
        }
        logger.log(Level.INFO, "HTTP-Server stopped...");
    }

    /**
     * This method returns the URI for the HTTP-server.
     * 
     * @return the URI for the HTTP-server
     */
    public String getBaseURI() {
        return baseURI;
    }
}