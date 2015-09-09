package com.aptima.ddd.networktools;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class NetworkClient {

	private Socket client;

	private int clientID;
	private Thread clientThread;
	private List<SimulationEvent> eventQueue;
	private java.lang.Object eventQueueLock;
	private InputStream in;
	private boolean isConnected;
	private OutputStream out;

	// / <summary>
	// / The NetworkClient should be instantiated once per client application.
	// / It is used to subscribe to and receive events from the DDD Server.
	// / It is also used to send events to the DDD Server.
	// / </summary>
	public NetworkClient() {
		eventQueueLock = new java.lang.Object();
		isConnected = false;
		eventQueue = Collections.synchronizedList( new ArrayList<SimulationEvent>() );
		client = null;
		clientThread = null;
		in = null;
		out = null;
		clientID = -1;
	}

	// / <summary>
	// / This method is used to connect to the DDD Server.
	// / </summary>
	// / <param name="servername">The hostname of the machine the DDD Server is
	// running on</param>
	// / <param name="port">The port number that the DDD Server is listening
	// on</param>
	// / <returns>boolean connect success</returns>
	public boolean Connect(String servername, int port) {
		try {
			if (!isConnected) {
				client = new Socket(servername, port);
				out = client.getOutputStream();
				in = client.getInputStream();
				NetMessage m = new NetMessage();
				m.type = NetMessage.NetMessageType.REGISTER;
				m.send(out);
				m.receive(in);
				clientID = m.clientID;
				clientThread = new Thread() {
					public void run() {
						try {
							SocketHandler();
						} catch (Exception e) {
							e.printStackTrace();
						}
					};
				};
				clientThread.start();
				isConnected = true;
				return true;
			} else {
				throw new Exception("NetworkClient: already connected");
			}
		} catch (Exception e) {
			System.out.println(e.toString());
			System.out.println(e.getMessage());
			return false;

		}

	}

	// / <summary>
	// / Disconnect from the DDD Server.
	// / This should always be called before a client application closes
	// / : order to gracefully disconnect from the server.
	// / </summary>
	public void Disconnect() throws IOException {
		if (isConnected) {

			NetMessage m = new NetMessage();
			m.type = NetMessage.NetMessageType.DISCONNECT;
			m.send(out);

			client.close();
			isConnected = false;
			clientThread = null;
		}

	}

	public int getClientID() {
		return clientID;
	}

	// / <summary>
	// / The method is used to get all waiting events from the DDD Server.
	// / This should be called periodically to see if any events are waiting.
	// / </summary>
	// / <returns>A list of events</returns>
	public List<SimulationEvent> GetEvents() {
		List<SimulationEvent> result = null;
		synchronized (eventQueueLock) {
			result = eventQueue;
			eventQueue = Collections.synchronizedList( new ArrayList<SimulationEvent>() );
		}
		return result;

	}

	// / <summary>
	// / This method should be called to determine if the NetworkClient object
	// is connected
	// / to the DDD Server. If for some reason the connection is lost, this will
	// return false.
	// / </summary>
	// / <returns></returns>
	public boolean IsConnected() {
		return isConnected;
	}

	// / <summary>
	// / This method is used to send an event to the DDD Server.
	// / </summary>
	// / <param name="e">The event</param>
	public void PutEvent(SimulationEvent e) throws IOException {
		if (isConnected) {
			NetMessage m = new NetMessage();
			m.type = NetMessage.NetMessageType.EVENT;
			m.msg = SimulationEventFactory.XMLSerialize(e);
			m.clientID = clientID;
			m.send(out);
		}
	}

	private void SocketHandler() throws Exception {

		NetMessage m = new NetMessage();
		SimulationEvent e = null;
		while (true) {
			try {
				m.receive(in);
				switch (m.type) {
				case EVENT:
					e = SimulationEventFactory.XMLDeserialize(m.msg);
					synchronized(eventQueueLock){
						eventQueue.add(e);
					}
					break;
				case DISCONNECT:
					System.out
							.println("NetworkClient: recieved Shutdown message from server.  Disconnecting...");
					Disconnect();
					return;
				case PING:
					break;
				case NONE:

					System.out
							.println("NetworkClient: Message with no type received.  Disconnecting.");
					Disconnect();
					return;
				default:
					throw new Exception(
							"NetworkClient: recieved unhandled message");
				}
			} catch (IOException ioe) {
				System.out
						.println("NetworkClient: Lost connection with remote server!");
				client.close();
				isConnected = false;
				clientThread = null;
				return;
			}
		}
	}

	// / <summary>
	// / This method is used to subscribe to event types.
	// / GetEvents will only return events that have been subscribed to with
	// this method.
	// / </summary>
	// / <param name="eventName">The name of an event type that is defined in
	// the simulation model</param>
	public void Subscribe(String eventName) throws Exception {
		if (isConnected) {
			NetMessage m = new NetMessage();
			m.type = NetMessage.NetMessageType.SUBSCRIBE;
			m.msg = eventName;
			m.clientID = clientID;
			m.send(out);
		} else {
			throw new Exception("NetworkClient: Must be connected first");
		}
	}
}
