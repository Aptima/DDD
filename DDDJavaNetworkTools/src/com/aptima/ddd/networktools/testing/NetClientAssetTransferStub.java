package com.aptima.ddd.networktools.testing;

import java.math.BigInteger;
import java.util.Random;

import com.aptima.ddd.networktools.NetworkClient;
import com.aptima.ddd.networktools.SimulationEvent;
import com.aptima.ddd.networktools.SimulationEventFactory;
import com.aptima.ddd.networktools.jxab.LocationType;
import com.aptima.ddd.networktools.jxab.Value;

public class NetClientAssetTransferStub {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		NetworkClient nc = new NetworkClient();

		try {
			nc.Connect(args[0], Integer.parseInt(args[1]));

			/*
			 * AuthenticationRequest ( Username, mtherrien ) ( Password,
			 * h/jtkVcSX/xNqeBqe4ARrYClP+E= ) ( TerminalID, MTHERRIEN ) ( Time,
			 * 284000 ) AuthenticationResponse ( Success, false ) ( Message,
			 * Authentication successful! ) ( TerminalID, MTHERRIEN ) ( Time,
			 * 284000 ) HandshakeGUIRegister ( TerminalID, MTHERRIEN ) ( Time,
			 * 284000 ) HandshakeAvailablePlayers ( TargetTerminalID, MTHERRIEN
			 * ) ( AvailablePlayers,
			 * [(GroundController)(Airport)(LocalController)] ) ( TakenPlayers,
			 * [] ) ( Players, [(GroundController)(Airport)(LocalController)] )
			 * ( Time, 284000 ) HandshakeGUIRoleRequest ( PlayerID, Airport ) (
			 * TerminalID, MTHERRIEN ) ( Time, 284000 ) HandshakeInitializeGUI (
			 * PlayerID, Airport ) ( TerminalID, MTHERRIEN ) ( ScenarioInfo,
			 * BASIC SCENARIO INFO **PLACEHOLDER** ) ( ScenarioName, airport ) (
			 * ScenarioDescription, ) ( MapName, DFW.jpg ) ( UTMNorthing, 0.0 )
			 * ( UTMEasting, 0.0 ) ( HorizontalPixelsPerMeter, 6.66 ) (
			 * VerticalPixelsPerMeter, 6.66 ) ( PlayerBrief, (No briefing
			 * supplied.) ) ( IconLibrary, airport4 ) ( Time, 284000 )
			 * HandshakeInitializeGUIDone ( PlayerID, Airport ) ( Time, 284000 )
			 */
			
			Random r = new Random();
			String UserID = "Airport";

			Value time = new Value();
			time.setIntegerType(BigInteger.valueOf(1009000));
			
			SimulationEvent done = SimulationEventFactory
					.createHandshakeInitializeGUIDone("Airport");// (UserID,
																	// ObjectID,
																	// l,
																	// Throttle);
			
			done.addParameter("Time", time);
			System.out.println(done);
			nc.PutEvent(done);

		} catch (Exception e) {

			e.printStackTrace();
			return;
		}

	}
}
