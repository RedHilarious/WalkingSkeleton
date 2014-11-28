package de.hsrm.swt02.messaging;

import java.io.BufferedInputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Properties;
import java.util.logging.Level;

import javax.jms.Connection;
import javax.jms.DeliveryMode;
import javax.jms.JMSException;
import javax.jms.MessageProducer;
import javax.jms.Session;
import javax.jms.TextMessage;
import javax.jms.Topic;

import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.broker.BrokerService;

import com.google.inject.Singleton;

import de.hsrm.swt02.logging.UseLogger;

/**
 * Class for message publishing on server-side. Uses ActiveMQ as message broker.
 * Enables string publishing on a specified message topic.
 */
@Singleton
public class ServerPublisherImp implements ServerPublisher {

    private ActiveMQConnectionFactory factory;
    private UseLogger logger;
    private Connection connection;
    private Session session;
    private MessageProducer publisher;
    private BrokerService broker;
    private String brokerURL;
    private String connectionURL;

    /**
     * Constructor.
     */
    public ServerPublisherImp() {
        logger = new UseLogger();
        final Properties properties = new Properties();
        BufferedInputStream stream;
        // read configuration file for broker properties
        try {
            stream = new BufferedInputStream(new FileInputStream(
                    "server.config"));
            properties.load(stream);
            stream.close();
        } catch (FileNotFoundException e) {
            logger.log(Level.WARNING, e);
        } catch (IOException e) {
            logger.log(Level.WARNING, e);
        } catch (SecurityException e) {
            logger.log(Level.WARNING, e);
        }
        brokerURL = properties.getProperty("BrokerURL");
        connectionURL = properties.getProperty("BrokerConnectionURL");
        factory = new ActiveMQConnectionFactory(brokerURL);
    }

    /**
     * Publishes a String-content on a specified topic.
     * 
     * @param content content to publish
     * @param topicName topic where the content will be published
     * @throws ServerPublisherBrokerException if publishing goes wrong
     */
    public void publish(String content, String topicName)
            throws ServerPublisherBrokerException {
        try {
            connection = factory.createConnection();
            session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);

            // create a message-producer for the topic
            final Topic topic = session.createTopic(topicName);
            publisher = session.createProducer(topic);
            publisher.setDeliveryMode(DeliveryMode.NON_PERSISTENT);

            // define messaging-content (TextMessage or MapMessage)
            final TextMessage msg = session.createTextMessage(content);

            // send and close
            publisher.send(msg);
            session.close();
            connection.close();
        } catch (JMSException e) {
            throw new ServerPublisherBrokerException("Publishing-Error: +"
                    + e.getMessage());
        }
    }

    /**
     * Starts the messaging broker.
     * 
     * @throws ServerPublisherBrokerException if broker start does not work
     */
    public void startBroker() throws ServerPublisherBrokerException {
        if (broker == null) {
            broker = new BrokerService();
            try {
                broker.addConnector(connectionURL);
            } catch (Exception e) {
                throw new ServerPublisherBrokerException(
                        "Message broker could not add connector (URL: "
                                + connectionURL + ")");
            }
        }
        if (!broker.isStarted()) {
            try {
                broker.start();
            } catch (Exception ex) {
                throw new ServerPublisherBrokerException(
                        "Could not START message broker");
            }
        }
    }

    /**
     * Stops the messaging broker.
     * 
     * @throws ServerPublisherBrokerException if broker sto does not work
     */
    public void stopBroker() throws ServerPublisherBrokerException {
        if (broker == null) {
            throw new ServerPublisherBrokerException(
                    "Message broker was never started");
        } else if (broker.isStarted()) {
            try {
                broker.stop();
            } catch (Exception ex) {
                throw new ServerPublisherBrokerException(
                        "Could not STOP message broker");
            }
        }
    }

    /**
     * Check weather the message broker is running.
     * 
     * @return true if the embedded message-broker is running
     */
    public boolean brokerStarted() {
        if (broker == null) {
            return false;
        } else {
            return broker.isStarted();
        }
    }
}