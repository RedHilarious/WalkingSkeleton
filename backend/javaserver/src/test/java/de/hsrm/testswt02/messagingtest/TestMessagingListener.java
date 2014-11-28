package de.hsrm.testswt02.messagingtest;

import javax.jms.Connection;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageConsumer;
import javax.jms.MessageListener;
import javax.jms.Session;
import javax.jms.TextMessage;
import javax.jms.Topic;

import org.apache.activemq.ActiveMQConnectionFactory;

/**
 * This class is for testing. It simulates a message listener listening to a
 * Test_Topic.
 */
public class TestMessagingListener implements MessageListener {
    private Connection connection;
    private Session session;
    private MessageConsumer consumer;
    private String receivedMsg = "fail";

    private String BROKERURL = "tcp://localhost:61616";
    final private String TOPIC_NAME = "TEST_TOPIC";

    public TestMessagingListener() {

    }

    public void onMessage(Message message) {
        TextMessage tm = (TextMessage) message;
        try {
            receivedMsg = tm.getText();
        } catch (JMSException e) {

        }
    }

    public void start() {
        ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory(
                BROKERURL);
        try {
            connection = factory.createConnection();
            session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);
            Topic topic = session.createTopic(TOPIC_NAME);
            consumer = session.createConsumer(topic);
            consumer.setMessageListener(this);
            connection.start();
        } catch (JMSException e) {

        }
    }

    public void stop() {
        try {
            connection.close();
        } catch (JMSException e) {

        }
    }

    public String getReceivedMsg() {
        return receivedMsg;
    }
}
