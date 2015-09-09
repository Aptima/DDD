package com.aptima.ddd.networktools.testing;

import java.math.BigInteger;
import java.util.Random;

import com.aptima.ddd.networktools.NetworkClient;
import com.aptima.ddd.networktools.SimulationEvent;
import com.aptima.ddd.networktools.SimulationEventFactory;
import com.aptima.ddd.networktools.jxab.LocationType;
import com.aptima.ddd.networktools.jxab.Value;

public class NetClientMoveStub {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		NetworkClient nc = new NetworkClient();

		try {
			nc.Connect(args[0], Integer.parseInt(args[1]));

			while (nc.IsConnected()) {
				/*
				 * MoveObjectRequest ( UserID, LocalController ) ( ObjectID,
				 * MQ3611 ) ( DestinationLocation, ( 1977.81066894531,
				 * 3290.5263671875, 0.0 ) ) ( Throttle, 1.0 ) ( Time, 993000 )
				 */
				Random r = new Random();
				String UserID = "LocalController";
				String ObjectID = "AA880";
				Double Throttle = 1.0;
				LocationType l = new LocationType((double) r.nextInt(6000),
						(double) r.nextInt(6000), 0.0);

				SimulationEvent e = SimulationEventFactory
						.createMoveObjectRequest(UserID, ObjectID, l, Throttle);
				Value time = new Value();
				time.setIntegerType(BigInteger.valueOf(1009000));
				e.addParameter("Time", time);
				System.out.println(l);
				nc.PutEvent(e);

				Thread.sleep(r.nextInt(10) * 1000);
			}

			return;

		} catch (Exception e) {

			e.printStackTrace();
			return;
		}

	}
}
