package moduleDI;

import persistence.Persistence;
import persistence.PersistenceImp;
import manager.ProcessManager;
import manager.ProcessManagerImp;
import messaging.ServerPublisher;
import messaging.ServerPublisherImp;

import com.google.inject.AbstractModule;

public class SingleModule extends AbstractModule {

	
	protected void configure() {
		bind(ServerPublisher.class).to(ServerPublisherImp.class);
		bind(ProcessManager.class).to(ProcessManagerImp.class);
		bind(Persistence.class).to(PersistenceImp.class);
	}
}
