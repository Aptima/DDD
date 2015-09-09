package com.aptima.ddd.networktools.testing;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.FileInputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;

import com.aptima.ddd.networktools.SimulationEvent;
import com.aptima.ddd.networktools.SimulationEventFactory;

class JAXBTestStub {

	/**
	 * @param args
	 * @throws Exception
	 */
	// public static final String PATH =
	// "src/com/aptima/ddd/networktools/testing/sampleEvents.xml";
	public static final String PATH = "src/com/aptima/ddd/networktools/testing/BigSampleEvents.xml";

	public static void main(String[] args) throws Exception {

		long t0 = System.currentTimeMillis();
		FileInputStream fstream = new FileInputStream(PATH);
		DataInputStream in = new DataInputStream(fstream);
		BufferedReader br = new BufferedReader(new InputStreamReader(in));
		ArrayList<SimulationEvent> events = new ArrayList<SimulationEvent>();
		try {
			String s = "";
			while ((s = br.readLine()) != null) {
				SimulationEvent e = SimulationEventFactory.XMLDeserialize(s);
			}
		} catch (Exception e) {
		}
		long t1 = System.currentTimeMillis();
		br.close();

		fstream = new FileInputStream(PATH);
		in = new DataInputStream(fstream);
		br = new BufferedReader(new InputStreamReader(in));
		try {
			String s = "";
			while ((s = br.readLine()) != null) {
				SimulationEvent e = SimulationEventFactory.XMLDeserialize(s);
				// adds lots of overhead
				events.add(e);
			}
			;
		} catch (Exception e) {
		}
		br.close();

		long t0_2 = System.currentTimeMillis();
		for (SimulationEvent event : events) {
			String s = SimulationEventFactory.XMLSerialize(event);
			// System.out.println(s);

		}
		long t2 = System.currentTimeMillis();

		double elapsed1 = (t1 - t0) / 1000.0;
		double elapsed2 = (t2 - t0_2) / 1000.0;
		double eventCount = events.size();
		System.out.println("Events: " + eventCount);
		System.out.println("t0: " + t0);
		System.out.println("t1: " + t1);
		System.out.println("t2_0: " + t0_2);
		System.out.println("t2: " + t2);
		System.out.println("Deserialize: t1 - t0: " + elapsed1 + "s ");
		System.out.println("Serialize:   t2 - t1: " + elapsed2 + "s ");
		System.out.println("Deserialized per second: " + eventCount / elapsed1
				+ " messages ");
		System.out.println("Serialized   per second: " + eventCount / elapsed2
				+ " messages ");

		// / Print out stuff
		// for(SimulationEvent e : events) {
		// List<Parameter> ps = e.getParameters();
		// System.out.println(e._getEventType());
		// for (Parameter p : ps) {
		// System.out.println("> " + p.getName() + " , "
		// + p.getValue());
		// }
		// }
		//		

	}
}
