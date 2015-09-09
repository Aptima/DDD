package com.aptima.ddd.networktools.testing;

import java.util.List;

import com.aptima.ddd.networktools.NetworkClient;
import com.aptima.ddd.networktools.SimulationEvent;
import com.aptima.ddd.networktools.jxab.Parameter;

public class NetworkClientStub {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		NetworkClient nc = new NetworkClient();

		try {
			nc.Connect(args[0], Integer.parseInt(args[1]));

			if (args.length > 2) {
				for (int i = 2; i < args.length; i++) {
					nc.Subscribe(args[i]);
				}
			} else {
				nc.Subscribe("ALL");
			}

			List<SimulationEvent> events;

			while (nc.IsConnected()) {

				events = nc.GetEvents();
				for (SimulationEvent e : events) {
					System.out.println(e._getEventType());
					for (Parameter dv : e.getParameters()) {
						System.out.println(dv);
					}
				}
				Thread.sleep(50);
			}

			return;

		} catch (Exception e) {

			e.printStackTrace();
			return;
		}

	}

}
