package com.aptima.ddd.networktools;

import static com.aptima.ddd.networktools.SimulationEventTypes.ACTIVE_REGION_SPEED_MULTIPLIER_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.ACTIVE_REGION_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.ADD_TO_CHAT_ROOM;
import static com.aptima.ddd.networktools.SimulationEventTypes.ADD_TO_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.ATTACK_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.ATTACK_OBJECT_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.ATTACK_SUCCEEDED;
import static com.aptima.ddd.networktools.SimulationEventTypes.AUTHENTICATION_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.AUTHENTICATION_RESPONSE;
import static com.aptima.ddd.networktools.SimulationEventTypes.BASE_EVENT;
import static com.aptima.ddd.networktools.SimulationEventTypes.CHANGE_TAG_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.CLIENT_ATTACK_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.CLIENT_REMOVE_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.CLIENT_SIDE_ASSET_TRANSFER_ALLOWED;
import static com.aptima.ddd.networktools.SimulationEventTypes.CLOSE_CHAT_ROOM;
import static com.aptima.ddd.networktools.SimulationEventTypes.CLOSE_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.CREATE_CHAT_ROOM;
import static com.aptima.ddd.networktools.SimulationEventTypes.CREATE_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.DISCONNECT_DECISION_MAKER;
import static com.aptima.ddd.networktools.SimulationEventTypes.DISCONNECT_TERMINAL;
import static com.aptima.ddd.networktools.SimulationEventTypes.ENGRAM_VALUE;
import static com.aptima.ddd.networktools.SimulationEventTypes.EXTERNAL_APP_LOG;
import static com.aptima.ddd.networktools.SimulationEventTypes.EXTERNAL_APP_MESSAGE;
import static com.aptima.ddd.networktools.SimulationEventTypes.EXTERNAL_APP_SIM_START;
import static com.aptima.ddd.networktools.SimulationEventTypes.EXTERNAL_APP_SIM_STOP;
import static com.aptima.ddd.networktools.SimulationEventTypes.EXTERNAL_EMAIL_RECEIVED;
import static com.aptima.ddd.networktools.SimulationEventTypes.FAILED_TO_CREATE_CHAT_ROOM;
import static com.aptima.ddd.networktools.SimulationEventTypes.GAME_SPEED;
import static com.aptima.ddd.networktools.SimulationEventTypes.GAME_SPEED_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.HANDSHAKE_AVAILABLE_PLAYERS;
import static com.aptima.ddd.networktools.SimulationEventTypes.HANDSHAKE_GUIREGISTER;
import static com.aptima.ddd.networktools.SimulationEventTypes.HANDSHAKE_GUIROLE_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.HANDSHAKE_INITIALIZE_GUI;
import static com.aptima.ddd.networktools.SimulationEventTypes.HANDSHAKE_INITIALIZE_GUIDONE;
import static com.aptima.ddd.networktools.SimulationEventTypes.HISTORY_ATTACKED_OBJECT_REPORT;
import static com.aptima.ddd.networktools.SimulationEventTypes.HISTORY_ATTACKER_OBJECT_REPORT;
import static com.aptima.ddd.networktools.SimulationEventTypes.HISTORY_PURSUE;
import static com.aptima.ddd.networktools.SimulationEventTypes.HISTORY_SUBPLATFORM_LAUNCH;
import static com.aptima.ddd.networktools.SimulationEventTypes.JOIN_CHAT_ROOM;
import static com.aptima.ddd.networktools.SimulationEventTypes.JOIN_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.LEAVE_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.LOAD_SCENARIO_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.MOVE_DONE;
import static com.aptima.ddd.networktools.SimulationEventTypes.MOVE_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.MOVE_OBJECT_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.NEW_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.OBJECT_COLLISION;
import static com.aptima.ddd.networktools.SimulationEventTypes.PAUSE_SCENARIO;
import static com.aptima.ddd.networktools.SimulationEventTypes.PAUSE_SCENARIO_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.PLAYER_CONTROL;
import static com.aptima.ddd.networktools.SimulationEventTypes.PLAYFIELD;
import static com.aptima.ddd.networktools.SimulationEventTypes.PLAY_VOICE_MESSAGE;
import static com.aptima.ddd.networktools.SimulationEventTypes.PUSH_TO_TALK;
import static com.aptima.ddd.networktools.SimulationEventTypes.RANDOM_SEED;
import static com.aptima.ddd.networktools.SimulationEventTypes.REMOVE_FROM_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.REQUEST_CHAT_ROOM_CREATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.REQUEST_CLOSE_CHAT_ROOM;
import static com.aptima.ddd.networktools.SimulationEventTypes.REQUEST_JOIN_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.REQUEST_LEAVE_VOICE_CHANNEL;
import static com.aptima.ddd.networktools.SimulationEventTypes.RESET_SIMULATION;
import static com.aptima.ddd.networktools.SimulationEventTypes.RESUME_SCENARIO;
import static com.aptima.ddd.networktools.SimulationEventTypes.RESUME_SCENARIO_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.REVEAL_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.SCORE_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.SELF_DEFENSE_ATTACK_STARTED;
import static com.aptima.ddd.networktools.SimulationEventTypes.SERVER_STATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.SIMULATION_TIME_EVENT;
import static com.aptima.ddd.networktools.SimulationEventTypes.SIM_CORE_READY;
import static com.aptima.ddd.networktools.SimulationEventTypes.STARTUP_COMPLETE;
import static com.aptima.ddd.networktools.SimulationEventTypes.STATE_CHANGE;
import static com.aptima.ddd.networktools.SimulationEventTypes.STOPPED_TALKING;
import static com.aptima.ddd.networktools.SimulationEventTypes.STOP_REPLAY;
import static com.aptima.ddd.networktools.SimulationEventTypes.STOP_SCENARIO;
import static com.aptima.ddd.networktools.SimulationEventTypes.STOP_SCENARIO_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.SUBPLATFORM_DOCK;
import static com.aptima.ddd.networktools.SimulationEventTypes.SUBPLATFORM_DOCK_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.SUBPLATFORM_LAUNCH;
import static com.aptima.ddd.networktools.SimulationEventTypes.SUBPLATFORM_LAUNCH_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.SYSTEM_MESSAGE;
import static com.aptima.ddd.networktools.SimulationEventTypes.TEXT_CHAT;
import static com.aptima.ddd.networktools.SimulationEventTypes.TEXT_CHAT_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.TIME_TICK;
import static com.aptima.ddd.networktools.SimulationEventTypes.TRANSFER_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.TRANSFER_OBJECT_REQUEST;
import static com.aptima.ddd.networktools.SimulationEventTypes.UPDATE_TAG;
import static com.aptima.ddd.networktools.SimulationEventTypes.VIEW_PRO_ACTIVE_REGION_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.VIEW_PRO_ATTACK_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.VIEW_PRO_ATTRIBUTE_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.VIEW_PRO_INITIALIZE_OBJECT;
import static com.aptima.ddd.networktools.SimulationEventTypes.VIEW_PRO_MOTION_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.VIEW_PRO_STOP_OBJECT_UPDATE;
import static com.aptima.ddd.networktools.SimulationEventTypes.WEAPON_LAUNCH;
import static com.aptima.ddd.networktools.SimulationEventTypes.WEAPON_LAUNCH_FAILURE;
import static com.aptima.ddd.networktools.SimulationEventTypes.WEAPON_LAUNCH_REQUEST;

import java.io.StringReader;
import java.io.StringWriter;
import java.math.BigInteger;

import javax.xml.bind.JAXB;
import javax.xml.bind.JAXBElement;
import javax.xml.namespace.QName;

import com.aptima.ddd.networktools.jxab.AttributeCollectionType;
import com.aptima.ddd.networktools.jxab.LocationType;
import com.aptima.ddd.networktools.jxab.PolygonType;
import com.aptima.ddd.networktools.jxab.StateTableType;
import com.aptima.ddd.networktools.jxab.StringListType;
import com.aptima.ddd.networktools.jxab.Value;

//create*(..) methods auto generated from python script createEvents.py
public class SimulationEventFactory {

	public static SimulationEvent createActiveRegionSpeedMultiplierUpdate(
			String ObjectID) {
		SimulationEvent e = new SimulationEvent(
				ACTIVE_REGION_SPEED_MULTIPLIER_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		return e;
	}

	public static SimulationEvent createActiveRegionUpdate(String ObjectID,
			Boolean IsVisible, Boolean IsActive) {
		SimulationEvent e = new SimulationEvent(ACTIVE_REGION_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setBooleanType(IsVisible);
		e.addParameter("IsVisible", v);
		v = new Value();
		v.setBooleanType(IsActive);
		e.addParameter("IsActive", v);
		return e;
	}

	public static SimulationEvent createAddToChatRoom(String RoomName,
			String TargetPlayerID) {
		SimulationEvent e = new SimulationEvent(ADD_TO_CHAT_ROOM);
		Value v = new Value();
		v = new Value();
		v.setStringType(RoomName);
		e.addParameter("RoomName", v);
		v = new Value();
		v.setStringType(TargetPlayerID);
		e.addParameter("TargetPlayerID", v);
		return e;
	}

	public static SimulationEvent createAddToVoiceChannel(String ChannelName,
			String NewAccessor) {
		SimulationEvent e = new SimulationEvent(ADD_TO_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(NewAccessor);
		e.addParameter("NewAccessor", v);
		return e;
	}

	public static SimulationEvent createAttackObject(String ObjectID,
			String TargetObjectID, String CapabilityName) {
		SimulationEvent e = new SimulationEvent(ATTACK_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		v = new Value();
		v.setStringType(CapabilityName);
		e.addParameter("CapabilityName", v);
		return e;
	}

	public static SimulationEvent createAttackObjectRequest(String UserID,
			String ObjectID, String TargetObjectID, String CapabilityName) {
		SimulationEvent e = new SimulationEvent(ATTACK_OBJECT_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		v = new Value();
		v.setStringType(CapabilityName);
		e.addParameter("CapabilityName", v);
		return e;
	}

	public static SimulationEvent createAttackSucceeded(String ObjectID,
			String TargetID, String NewState, StringListType Capabilities) {
		SimulationEvent e = new SimulationEvent(ATTACK_SUCCEEDED);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(TargetID);
		e.addParameter("TargetID", v);
		v = new Value();
		v.setStringType(NewState);
		e.addParameter("NewState", v);
		v = new Value();
		v.setStringListType(Capabilities);
		e.addParameter("Capabilities", v);
		return e;
	}

	public static SimulationEvent createAuthenticationRequest(String Username,
			String Password, String TerminalID) {
		SimulationEvent e = new SimulationEvent(AUTHENTICATION_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(Username);
		e.addParameter("Username", v);
		v = new Value();
		v.setStringType(Password);
		e.addParameter("Password", v);
		v = new Value();
		v.setStringType(TerminalID);
		e.addParameter("TerminalID", v);
		return e;
	}

	public static SimulationEvent createAuthenticationResponse(Boolean Success,
			String Message, String TerminalID) {
		SimulationEvent e = new SimulationEvent(AUTHENTICATION_RESPONSE);
		Value v = new Value();
		v = new Value();
		v.setBooleanType(Success);
		e.addParameter("Success", v);
		v = new Value();
		v.setStringType(Message);
		e.addParameter("Message", v);
		v = new Value();
		v.setStringType(TerminalID);
		e.addParameter("TerminalID", v);
		return e;
	}

	// The following were automatically generated by a python script
	// createEvents.py as convenience methods
	public static SimulationEvent createBaseEvent(BigInteger Time) {
		SimulationEvent e = new SimulationEvent(BASE_EVENT);
		Value v = new Value();
		v = new Value();
		v.setIntegerType(Time);
		e.addParameter("Time", v);
		return e;
	}

	public static SimulationEvent createChangeTagRequest(String UnitID,
			String DecisionMakerID, String Tag) {
		SimulationEvent e = new SimulationEvent(CHANGE_TAG_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UnitID);
		e.addParameter("UnitID", v);
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		v = new Value();
		v.setStringType(Tag);
		e.addParameter("Tag", v);
		return e;
	}

	public static SimulationEvent createClientAttackRequest(String PlayerID,
			String AttackingObjectID, String TargetObjectID,
			String WeaponOrCapabilityName) {
		SimulationEvent e = new SimulationEvent(CLIENT_ATTACK_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(PlayerID);
		e.addParameter("PlayerID", v);
		v = new Value();
		v.setStringType(AttackingObjectID);
		e.addParameter("AttackingObjectID", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		v = new Value();
		v.setStringType(WeaponOrCapabilityName);
		e.addParameter("WeaponOrCapabilityName", v);
		return e;
	}

	public static SimulationEvent createClientRemoveObject(String TargetPlayer,
			String ObjectID) {
		SimulationEvent e = new SimulationEvent(CLIENT_REMOVE_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(TargetPlayer);
		e.addParameter("TargetPlayer", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		return e;
	}

	public static SimulationEvent createClientSideAssetTransferAllowed(
			Boolean EnableAssetTransfer) {
		SimulationEvent e = new SimulationEvent(
				CLIENT_SIDE_ASSET_TRANSFER_ALLOWED);
		Value v = new Value();
		v = new Value();
		v.setBooleanType(EnableAssetTransfer);
		e.addParameter("EnableAssetTransfer", v);
		return e;
	}

	public static SimulationEvent createCloseChatRoom(String RoomName) {
		SimulationEvent e = new SimulationEvent(CLOSE_CHAT_ROOM);
		Value v = new Value();
		v = new Value();
		v.setStringType(RoomName);
		e.addParameter("RoomName", v);
		return e;
	}

	public static SimulationEvent createCloseVoiceChannel(String ChannelName) {
		SimulationEvent e = new SimulationEvent(CLOSE_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		return e;
	}

	public static SimulationEvent createCreateChatRoom(String RoomName,
			StringListType MembershipList) {
		SimulationEvent e = new SimulationEvent(CREATE_CHAT_ROOM);
		Value v = new Value();
		v = new Value();
		v.setStringType(RoomName);
		e.addParameter("RoomName", v);
		v = new Value();
		v.setStringListType(MembershipList);
		e.addParameter("MembershipList", v);
		return e;
	}

	public static SimulationEvent createCreateVoiceChannel(String ChannelName,
			StringListType MembershipList) {
		SimulationEvent e = new SimulationEvent(CREATE_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringListType(MembershipList);
		e.addParameter("MembershipList", v);
		return e;
	}

	public static SimulationEvent createDisconnectDecisionMaker(
			String DecisionMakerID) {
		SimulationEvent e = new SimulationEvent(DISCONNECT_DECISION_MAKER);
		Value v = new Value();
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		return e;
	}

	public static SimulationEvent createDisconnectTerminal(String TerminalID) {
		SimulationEvent e = new SimulationEvent(DISCONNECT_TERMINAL);
		Value v = new Value();
		v = new Value();
		v.setStringType(TerminalID);
		e.addParameter("TerminalID", v);
		return e;
	}

	public static SimulationEvent createEngramValue(String EngramName,
			String SpecificUnit, String EngramValue, String EngramDataType) {
		SimulationEvent e = new SimulationEvent(ENGRAM_VALUE);
		Value v = new Value();
		v = new Value();
		v.setStringType(EngramName);
		e.addParameter("EngramName", v);
		v = new Value();
		v.setStringType(SpecificUnit);
		e.addParameter("SpecificUnit", v);
		v = new Value();
		v.setStringType(EngramValue);
		e.addParameter("EngramValue", v);
		v = new Value();
		v.setStringType(EngramDataType);
		e.addParameter("EngramDataType", v);
		return e;
	}

	public static SimulationEvent createExternalApp_Log(String AppName,
			String LogEntry) {
		SimulationEvent e = new SimulationEvent(EXTERNAL_APP_LOG);
		Value v = new Value();
		v = new Value();
		v.setStringType(AppName);
		e.addParameter("AppName", v);
		v = new Value();
		v.setStringType(LogEntry);
		e.addParameter("LogEntry", v);
		return e;
	}

	public static SimulationEvent createExternalApp_Message(String AppName,
			String Message) {
		SimulationEvent e = new SimulationEvent(EXTERNAL_APP_MESSAGE);
		Value v = new Value();
		v = new Value();
		v.setStringType(AppName);
		e.addParameter("AppName", v);
		v = new Value();
		v.setStringType(Message);
		e.addParameter("Message", v);
		return e;
	}

	public static SimulationEvent createExternalApp_SimStart() {
		SimulationEvent e = new SimulationEvent(EXTERNAL_APP_SIM_START);
		return e;
	}

	public static SimulationEvent createExternalApp_SimStop() {
		SimulationEvent e = new SimulationEvent(EXTERNAL_APP_SIM_STOP);
		return e;
	}

	public static SimulationEvent createExternalEmailReceived(
			String FromAddress, StringListType ToAddresses,
			String WallClockTime, String Subject, String Body,
			StringListType Attachments) {
		SimulationEvent e = new SimulationEvent(EXTERNAL_EMAIL_RECEIVED);
		Value v = new Value();
		v = new Value();
		v.setStringType(FromAddress);
		e.addParameter("FromAddress", v);
		v = new Value();
		v.setStringListType(ToAddresses);
		e.addParameter("ToAddresses", v);
		v = new Value();
		v.setStringType(WallClockTime);
		e.addParameter("WallClockTime", v);
		v = new Value();
		v.setStringType(Subject);
		e.addParameter("Subject", v);
		v = new Value();
		v.setStringType(Body);
		e.addParameter("Body", v);
		v = new Value();
		v.setStringListType(Attachments);
		e.addParameter("Attachments", v);
		return e;
	}

	public static SimulationEvent createFailedToCreateChatRoom(String Message,
			String SenderDM_ID) {
		SimulationEvent e = new SimulationEvent(FAILED_TO_CREATE_CHAT_ROOM);
		Value v = new Value();
		v = new Value();
		v.setStringType(Message);
		e.addParameter("Message", v);
		v = new Value();
		v.setStringType(SenderDM_ID);
		e.addParameter("SenderDM_ID", v);
		return e;
	}

	public static SimulationEvent createGameSpeed(Double SpeedFactor) {
		SimulationEvent e = new SimulationEvent(GAME_SPEED);
		Value v = new Value();
		v = new Value();
		v.setDoubleType(SpeedFactor);
		e.addParameter("SpeedFactor", v);
		return e;
	}

	public static SimulationEvent createGameSpeedRequest(Double SpeedFactor) {
		SimulationEvent e = new SimulationEvent(GAME_SPEED_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setDoubleType(SpeedFactor);
		e.addParameter("SpeedFactor", v);
		return e;
	}

	public static SimulationEvent createHandshakeAvailablePlayers(
			String TargetTerminalID, StringListType AvailablePlayers,
			StringListType TakenPlayers, StringListType Players) {
		SimulationEvent e = new SimulationEvent(HANDSHAKE_AVAILABLE_PLAYERS);
		Value v = new Value();
		v = new Value();
		v.setStringType(TargetTerminalID);
		e.addParameter("TargetTerminalID", v);
		v = new Value();
		v.setStringListType(AvailablePlayers);
		e.addParameter("AvailablePlayers", v);
		v = new Value();
		v.setStringListType(TakenPlayers);
		e.addParameter("TakenPlayers", v);
		v = new Value();
		v.setStringListType(Players);
		e.addParameter("Players", v);
		return e;
	}

	public static SimulationEvent createHandshakeGUIRegister(String TerminalID) {
		SimulationEvent e = new SimulationEvent(HANDSHAKE_GUIREGISTER);
		Value v = new Value();
		v = new Value();
		v.setStringType(TerminalID);
		e.addParameter("TerminalID", v);
		return e;
	}

	public static SimulationEvent createHandshakeGUIRoleRequest(
			String PlayerID, String TerminalID) {
		SimulationEvent e = new SimulationEvent(HANDSHAKE_GUIROLE_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(PlayerID);
		e.addParameter("PlayerID", v);
		v = new Value();
		v.setStringType(TerminalID);
		e.addParameter("TerminalID", v);
		return e;
	}

	public static SimulationEvent createHandshakeInitializeGUI(String PlayerID,
			String TerminalID, String ScenarioInfo, String ScenarioName,
			String ScenarioDescription, String MapName, Double UTMNorthing,
			Double UTMEasting, Double HorizontalPixelsPerMeter,
			Double VerticalPixelsPerMeter, String PlayerBrief,
			String IconLibrary, Boolean VoiceChatEnabled,
			String VoiceChatServerName, BigInteger VoiceChatServerPort,
			String VoiceChatUserPassword) {
		SimulationEvent e = new SimulationEvent(HANDSHAKE_INITIALIZE_GUI);
		Value v = new Value();
		v = new Value();
		v.setStringType(PlayerID);
		e.addParameter("PlayerID", v);
		v = new Value();
		v.setStringType(TerminalID);
		e.addParameter("TerminalID", v);
		v = new Value();
		v.setStringType(ScenarioInfo);
		e.addParameter("ScenarioInfo", v);
		v = new Value();
		v.setStringType(ScenarioName);
		e.addParameter("ScenarioName", v);
		v = new Value();
		v.setStringType(ScenarioDescription);
		e.addParameter("ScenarioDescription", v);
		v = new Value();
		v.setStringType(MapName);
		e.addParameter("MapName", v);
		v = new Value();
		v.setDoubleType(UTMNorthing);
		e.addParameter("UTMNorthing", v);
		v = new Value();
		v.setDoubleType(UTMEasting);
		e.addParameter("UTMEasting", v);
		v = new Value();
		v.setDoubleType(HorizontalPixelsPerMeter);
		e.addParameter("HorizontalPixelsPerMeter", v);
		v = new Value();
		v.setDoubleType(VerticalPixelsPerMeter);
		e.addParameter("VerticalPixelsPerMeter", v);
		v = new Value();
		v.setStringType(PlayerBrief);
		e.addParameter("PlayerBrief", v);
		v = new Value();
		v.setStringType(IconLibrary);
		e.addParameter("IconLibrary", v);
		v = new Value();
		v.setBooleanType(VoiceChatEnabled);
		e.addParameter("VoiceChatEnabled", v);
		v = new Value();
		v.setStringType(VoiceChatServerName);
		e.addParameter("VoiceChatServerName", v);
		v = new Value();
		v.setIntegerType(VoiceChatServerPort);
		e.addParameter("VoiceChatServerPort", v);
		v = new Value();
		v.setStringType(VoiceChatUserPassword);
		e.addParameter("VoiceChatUserPassword", v);
		return e;
	}

	public static SimulationEvent createHandshakeInitializeGUIDone(
			String PlayerID) {
		SimulationEvent e = new SimulationEvent(HANDSHAKE_INITIALIZE_GUIDONE);
		Value v = new Value();
		v = new Value();
		v.setStringType(PlayerID);
		e.addParameter("PlayerID", v);
		return e;
	}

	public static SimulationEvent createHistory_AttackedObjectReport(
			String ObjectID, LocationType ObjectLocation,
			Boolean AttackSuccess, String NewState) {
		SimulationEvent e = new SimulationEvent(HISTORY_ATTACKED_OBJECT_REPORT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setLocationType(ObjectLocation);
		e.addParameter("ObjectLocation", v);
		v = new Value();
		v.setBooleanType(AttackSuccess);
		e.addParameter("AttackSuccess", v);
		v = new Value();
		v.setStringType(NewState);
		e.addParameter("NewState", v);
		return e;
	}

	public static SimulationEvent createHistory_AttackerObjectReport(
			String ObjectID, LocationType ObjectLocation,
			String TargetObjectID, LocationType TargetObjectLocation,
			String CapabilityName) {
		SimulationEvent e = new SimulationEvent(HISTORY_ATTACKER_OBJECT_REPORT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setLocationType(ObjectLocation);
		e.addParameter("ObjectLocation", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		v = new Value();
		v.setLocationType(TargetObjectLocation);
		e.addParameter("TargetObjectLocation", v);
		v = new Value();
		v.setStringType(CapabilityName);
		e.addParameter("CapabilityName", v);
		return e;
	}

	public static SimulationEvent createHistory_Pursue(String ObjectID,
			LocationType ObjectLocation, String TargetObjectID,
			LocationType TargetObjectLocation) {
		SimulationEvent e = new SimulationEvent(HISTORY_PURSUE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setLocationType(ObjectLocation);
		e.addParameter("ObjectLocation", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		v = new Value();
		v.setLocationType(TargetObjectLocation);
		e.addParameter("TargetObjectLocation", v);
		return e;
	}

	public static SimulationEvent createHistory_SubplatformLaunch(
			String ObjectID, String ParentObjectID,
			LocationType ParentObjectLocation,
			LocationType LaunchDestinationLocation, Boolean IsWeaponLaunch,
			String TargetObjectID) {
		SimulationEvent e = new SimulationEvent(HISTORY_SUBPLATFORM_LAUNCH);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		v = new Value();
		v.setLocationType(ParentObjectLocation);
		e.addParameter("ParentObjectLocation", v);
		v = new Value();
		v.setLocationType(LaunchDestinationLocation);
		e.addParameter("LaunchDestinationLocation", v);
		v = new Value();
		v.setBooleanType(IsWeaponLaunch);
		e.addParameter("IsWeaponLaunch", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		return e;
	}

	public static SimulationEvent createJoinChatRoom(String RoomName,
			String TargetPlayerID) {
		SimulationEvent e = new SimulationEvent(JOIN_CHAT_ROOM);
		Value v = new Value();
		v = new Value();
		v.setStringType(RoomName);
		e.addParameter("RoomName", v);
		v = new Value();
		v.setStringType(TargetPlayerID);
		e.addParameter("TargetPlayerID", v);
		return e;
	}

	public static SimulationEvent createJoinVoiceChannel(String ChannelName,
			String DecisionMakerID) {
		SimulationEvent e = new SimulationEvent(JOIN_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		return e;
	}

	public static SimulationEvent createLeaveVoiceChannel(String ChannelName,
			String DecisionMakerID) {
		SimulationEvent e = new SimulationEvent(LEAVE_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		return e;
	}

	public static SimulationEvent createLoadScenarioRequest(
			String ScenarioPath, String GroupName, String OutputLogDir) {
		SimulationEvent e = new SimulationEvent(LOAD_SCENARIO_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(ScenarioPath);
		e.addParameter("ScenarioPath", v);
		v = new Value();
		v.setStringType(GroupName);
		e.addParameter("GroupName", v);
		v = new Value();
		v.setStringType(OutputLogDir);
		e.addParameter("OutputLogDir", v);
		return e;
	}

	public static SimulationEvent createMoveDone(String ObjectID, String Reason) {
		SimulationEvent e = new SimulationEvent(MOVE_DONE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(Reason);
		e.addParameter("Reason", v);
		return e;
	}

	public static SimulationEvent createMoveObject(String ObjectID,
			LocationType DestinationLocation, Double Throttle) {
		SimulationEvent e = new SimulationEvent(MOVE_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setLocationType(DestinationLocation);
		e.addParameter("DestinationLocation", v);
		v = new Value();
		v.setDoubleType(Throttle);
		e.addParameter("Throttle", v);
		return e;
	}

	public static SimulationEvent createMoveObjectRequest(String UserID,
			String ObjectID, LocationType DestinationLocation, Double Throttle) {
		SimulationEvent e = new SimulationEvent(MOVE_OBJECT_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setLocationType(DestinationLocation);
		e.addParameter("DestinationLocation", v);
		v = new Value();
		v.setDoubleType(Throttle);
		e.addParameter("Throttle", v);
		return e;
	}

	public static SimulationEvent createNewObject(String ID, String ObjectType,
			StateTableType StateTable, AttributeCollectionType Attributes) {
		SimulationEvent e = new SimulationEvent(NEW_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ID);
		e.addParameter("ID", v);
		v = new Value();
		v.setStringType(ObjectType);
		e.addParameter("ObjectType", v);
		v = new Value();
		v.setStateTableType(StateTable);
		e.addParameter("StateTable", v);
		v = new Value();
		v.setAttributeCollectionType(Attributes);
		e.addParameter("Attributes", v);
		return e;
	}

	public static SimulationEvent createObjectCollision(String ObjectID1,
			String ObjectID2) {
		SimulationEvent e = new SimulationEvent(OBJECT_COLLISION);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID1);
		e.addParameter("ObjectID1", v);
		v = new Value();
		v.setStringType(ObjectID2);
		e.addParameter("ObjectID2", v);
		return e;
	}

	public static SimulationEvent createPauseScenario() {
		SimulationEvent e = new SimulationEvent(PAUSE_SCENARIO);
		return e;
	}

	public static SimulationEvent createPauseScenarioRequest() {
		SimulationEvent e = new SimulationEvent(PAUSE_SCENARIO_REQUEST);
		return e;
	}

	public static SimulationEvent createPlayerControl(String DecisionMakerID,
			String ControlledBy) {
		SimulationEvent e = new SimulationEvent(PLAYER_CONTROL);
		Value v = new Value();
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		v = new Value();
		v.setStringType(ControlledBy);
		e.addParameter("ControlledBy", v);
		return e;
	}

	public static SimulationEvent createPlayfield(String MapDataFile,
			String IconLibrary, String UTMZone, Double UTMNorthing,
			Double UTMEasting, Double HorizontalScale, Double VerticalScale,
			String Name, String Description) {
		SimulationEvent e = new SimulationEvent(PLAYFIELD);
		Value v = new Value();
		v = new Value();
		v.setStringType(MapDataFile);
		e.addParameter("MapDataFile", v);
		v = new Value();
		v.setStringType(IconLibrary);
		e.addParameter("IconLibrary", v);
		v = new Value();
		v.setStringType(UTMZone);
		e.addParameter("UTMZone", v);
		v = new Value();
		v.setDoubleType(UTMNorthing);
		e.addParameter("UTMNorthing", v);
		v = new Value();
		v.setDoubleType(UTMEasting);
		e.addParameter("UTMEasting", v);
		v = new Value();
		v.setDoubleType(HorizontalScale);
		e.addParameter("HorizontalScale", v);
		v = new Value();
		v.setDoubleType(VerticalScale);
		e.addParameter("VerticalScale", v);
		v = new Value();
		v.setStringType(Name);
		e.addParameter("Name", v);
		v = new Value();
		v.setStringType(Description);
		e.addParameter("Description", v);
		return e;
	}

	public static SimulationEvent createPlayVoiceMessage(String Channel,
			String File) {
		SimulationEvent e = new SimulationEvent(PLAY_VOICE_MESSAGE);
		Value v = new Value();
		v = new Value();
		v.setStringType(Channel);
		e.addParameter("Channel", v);
		v = new Value();
		v.setStringType(File);
		e.addParameter("File", v);
		return e;
	}

	public static SimulationEvent createPushToTalk(String ChannelName,
			String Speaker) {
		SimulationEvent e = new SimulationEvent(PUSH_TO_TALK);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(Speaker);
		e.addParameter("Speaker", v);
		return e;
	}

	public static SimulationEvent createRandomSeed(BigInteger SeedValue) {
		SimulationEvent e = new SimulationEvent(RANDOM_SEED);
		Value v = new Value();
		v = new Value();
		v.setIntegerType(SeedValue);
		e.addParameter("SeedValue", v);
		return e;
	}

	public static SimulationEvent createRemoveFromVoiceChannel(
			String ChannelName, String DeletedPlayer) {
		SimulationEvent e = new SimulationEvent(REMOVE_FROM_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(DeletedPlayer);
		e.addParameter("DeletedPlayer", v);
		return e;
	}

	public static SimulationEvent createRequestChatRoomCreate(String RoomName,
			StringListType MembershipList, String SenderDM_ID) {
		SimulationEvent e = new SimulationEvent(REQUEST_CHAT_ROOM_CREATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(RoomName);
		e.addParameter("RoomName", v);
		v = new Value();
		v.setStringListType(MembershipList);
		e.addParameter("MembershipList", v);
		v = new Value();
		v.setStringType(SenderDM_ID);
		e.addParameter("SenderDM_ID", v);
		return e;
	}

	public static SimulationEvent createRequestCloseChatRoom(String RoomName,
			String SenderDM_ID) {
		SimulationEvent e = new SimulationEvent(REQUEST_CLOSE_CHAT_ROOM);
		Value v = new Value();
		v = new Value();
		v.setStringType(RoomName);
		e.addParameter("RoomName", v);
		v = new Value();
		v.setStringType(SenderDM_ID);
		e.addParameter("SenderDM_ID", v);
		return e;
	}

	public static SimulationEvent createRequestJoinVoiceChannel(
			String ChannelName, String DecisionMakerID) {
		SimulationEvent e = new SimulationEvent(REQUEST_JOIN_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		return e;
	}

	public static SimulationEvent createRequestLeaveVoiceChannel(
			String ChannelName, String DecisionMakerID) {
		SimulationEvent e = new SimulationEvent(REQUEST_LEAVE_VOICE_CHANNEL);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		return e;
	}

	public static SimulationEvent createResetSimulation() {
		SimulationEvent e = new SimulationEvent(RESET_SIMULATION);
		return e;
	}

	public static SimulationEvent createResumeScenario() {
		SimulationEvent e = new SimulationEvent(RESUME_SCENARIO);
		return e;
	}

	public static SimulationEvent createResumeScenarioRequest() {
		SimulationEvent e = new SimulationEvent(RESUME_SCENARIO_REQUEST);
		return e;
	}

	public static SimulationEvent createRevealObject(String ObjectID,
			AttributeCollectionType Attributes) {
		SimulationEvent e = new SimulationEvent(REVEAL_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setAttributeCollectionType(Attributes);
		e.addParameter("Attributes", v);
		return e;
	}

	public static SimulationEvent createScoreUpdate(String DecisionMakerID,
			String ScoreName, Double ScoreValue) {
		SimulationEvent e = new SimulationEvent(SCORE_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(DecisionMakerID);
		e.addParameter("DecisionMakerID", v);
		v = new Value();
		v.setStringType(ScoreName);
		e.addParameter("ScoreName", v);
		v = new Value();
		v.setDoubleType(ScoreValue);
		e.addParameter("ScoreValue", v);
		return e;
	}

	public static SimulationEvent createSelfDefenseAttackStarted(
			String AttackerObjectID, String TargetObjectID) {
		SimulationEvent e = new SimulationEvent(SELF_DEFENSE_ATTACK_STARTED);
		Value v = new Value();
		v = new Value();
		v.setStringType(AttackerObjectID);
		e.addParameter("AttackerObjectID", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		return e;
	}

	public static SimulationEvent createServerState(String MessageType,
			String MessageText) {
		SimulationEvent e = new SimulationEvent(SERVER_STATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(MessageType);
		e.addParameter("MessageType", v);
		v = new Value();
		v.setStringType(MessageText);
		e.addParameter("MessageText", v);
		return e;
	}

	public static SimulationEvent createSimCoreReady() {
		SimulationEvent e = new SimulationEvent(SIM_CORE_READY);
		return e;
	}

	public static SimulationEvent createSimulationTimeEvent(
			String SimulationTime) {
		SimulationEvent e = new SimulationEvent(SIMULATION_TIME_EVENT);
		Value v = new Value();
		v = new Value();
		v.setStringType(SimulationTime);
		e.addParameter("SimulationTime", v);
		return e;
	}

	public static SimulationEvent createStartupComplete() {
		SimulationEvent e = new SimulationEvent(STARTUP_COMPLETE);
		return e;
	}

	public static SimulationEvent createStateChange(String ObjectID,
			String NewState) {
		SimulationEvent e = new SimulationEvent(STATE_CHANGE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(NewState);
		e.addParameter("NewState", v);
		return e;
	}

	public static SimulationEvent createStoppedTalking(String ChannelName,
			String Speaker) {
		SimulationEvent e = new SimulationEvent(STOPPED_TALKING);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChannelName);
		e.addParameter("ChannelName", v);
		v = new Value();
		v.setStringType(Speaker);
		e.addParameter("Speaker", v);
		return e;
	}

	public static SimulationEvent createStopReplay() {
		SimulationEvent e = new SimulationEvent(STOP_REPLAY);
		return e;
	}

	public static SimulationEvent createStopScenario() {
		SimulationEvent e = new SimulationEvent(STOP_SCENARIO);
		return e;
	}

	public static SimulationEvent createStopScenarioRequest() {
		SimulationEvent e = new SimulationEvent(STOP_SCENARIO_REQUEST);
		return e;
	}

	public static SimulationEvent createSubplatformDock(String ObjectID,
			String ParentObjectID) {
		SimulationEvent e = new SimulationEvent(SUBPLATFORM_DOCK);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		return e;
	}

	public static SimulationEvent createSubplatformDockRequest(String UserID,
			String ObjectID, String ParentObjectID) {
		SimulationEvent e = new SimulationEvent(SUBPLATFORM_DOCK_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		return e;
	}

	public static SimulationEvent createSubplatformLaunch(String ObjectID,
			String ParentObjectID, LocationType LaunchDestinationLocation) {
		SimulationEvent e = new SimulationEvent(SUBPLATFORM_LAUNCH);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		v = new Value();
		v.setLocationType(LaunchDestinationLocation);
		e.addParameter("LaunchDestinationLocation", v);
		return e;
	}

	public static SimulationEvent createSubplatformLaunchRequest(String UserID,
			String ObjectID, String ParentObjectID,
			LocationType LaunchDestinationLocation) {
		SimulationEvent e = new SimulationEvent(SUBPLATFORM_LAUNCH_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		v = new Value();
		v.setLocationType(LaunchDestinationLocation);
		e.addParameter("LaunchDestinationLocation", v);
		return e;
	}

	public static SimulationEvent createSystemMessage(String PlayerID,
			String Message, BigInteger TextColor, String DisplayStyle) {
		SimulationEvent e = new SimulationEvent(SYSTEM_MESSAGE);
		Value v = new Value();
		v = new Value();
		v.setStringType(PlayerID);
		e.addParameter("PlayerID", v);
		v = new Value();
		v.setStringType(Message);
		e.addParameter("Message", v);
		v = new Value();
		v.setIntegerType(TextColor);
		e.addParameter("TextColor", v);
		v = new Value();
		v.setStringType(DisplayStyle);
		e.addParameter("DisplayStyle", v);
		return e;
	}

	public static SimulationEvent createTextChat(String ChatBody,
			String UserID, String TargetUserID) {
		SimulationEvent e = new SimulationEvent(TEXT_CHAT);
		Value v = new Value();
		v = new Value();
		v.setStringType(ChatBody);
		e.addParameter("ChatBody", v);
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(TargetUserID);
		e.addParameter("TargetUserID", v);
		return e;
	}

	public static SimulationEvent createTextChatRequest(String UserID,
			String ChatBody, String ChatType, String TargetUserID) {
		SimulationEvent e = new SimulationEvent(TEXT_CHAT_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ChatBody);
		e.addParameter("ChatBody", v);
		v = new Value();
		v.setStringType(ChatType);
		e.addParameter("ChatType", v);
		v = new Value();
		v.setStringType(TargetUserID);
		e.addParameter("TargetUserID", v);
		return e;
	}

	public static SimulationEvent createTimeTick(String SimulationTime) {
		SimulationEvent e = new SimulationEvent(TIME_TICK);
		Value v = new Value();
		v = new Value();
		v.setStringType(SimulationTime);
		e.addParameter("SimulationTime", v);
		return e;
	}

	public static SimulationEvent createTransferObject(String UserID,
			String ObjectID, String DonorUserID) {
		SimulationEvent e = new SimulationEvent(TRANSFER_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(DonorUserID);
		e.addParameter("DonorUserID", v);
		return e;
	}

	public static SimulationEvent createTransferObjectRequest(String UserID,
			String ObjectID, String RecipientID, String State) {
		SimulationEvent e = new SimulationEvent(TRANSFER_OBJECT_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(RecipientID);
		e.addParameter("RecipientID", v);
		v = new Value();
		v.setStringType(State);
		e.addParameter("State", v);
		return e;
	}

	public static SimulationEvent createUpdateTag(String UnitID, String Tag,
			StringListType TeamMembers) {
		SimulationEvent e = new SimulationEvent(UPDATE_TAG);
		Value v = new Value();
		v = new Value();
		v.setStringType(UnitID);
		e.addParameter("UnitID", v);
		v = new Value();
		v.setStringType(Tag);
		e.addParameter("Tag", v);
		v = new Value();
		v.setStringListType(TeamMembers);
		e.addParameter("TeamMembers", v);
		return e;
	}

	public static SimulationEvent createViewProActiveRegionUpdate(
			String ObjectID, Boolean IsVisible, BigInteger DisplayColor,
			PolygonType Shape) {
		SimulationEvent e = new SimulationEvent(VIEW_PRO_ACTIVE_REGION_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setBooleanType(IsVisible);
		e.addParameter("IsVisible", v);
		v = new Value();
		v.setIntegerType(DisplayColor);
		e.addParameter("DisplayColor", v);
		v = new Value();
		v.setPolygonType(Shape);
		e.addParameter("Shape", v);
		return e;
	}

	public static SimulationEvent createViewProAttackUpdate(String AttackerID,
			String TargetID, BigInteger AttackEndTime) {
		SimulationEvent e = new SimulationEvent(VIEW_PRO_ATTACK_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(AttackerID);
		e.addParameter("AttackerID", v);
		v = new Value();
		v.setStringType(TargetID);
		e.addParameter("TargetID", v);
		v = new Value();
		v.setIntegerType(AttackEndTime);
		e.addParameter("AttackEndTime", v);
		return e;
	}

	public static SimulationEvent createViewProAttributeUpdate(
			String TargetPlayer, String ObjectID, String OwnerID,
			AttributeCollectionType Attributes) {
		SimulationEvent e = new SimulationEvent(VIEW_PRO_ATTRIBUTE_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(TargetPlayer);
		e.addParameter("TargetPlayer", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(OwnerID);
		e.addParameter("OwnerID", v);
		v = new Value();
		v.setAttributeCollectionType(Attributes);
		e.addParameter("Attributes", v);
		return e;
	}

	public static SimulationEvent createViewProInitializeObject(
			String TargetPlayer, String ObjectID, LocationType Location,
			String IconName, String OwnerID, Boolean IsWeapon,
			BigInteger LabelColor) {
		SimulationEvent e = new SimulationEvent(VIEW_PRO_INITIALIZE_OBJECT);
		Value v = new Value();
		v = new Value();
		v.setStringType(TargetPlayer);
		e.addParameter("TargetPlayer", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setLocationType(Location);
		e.addParameter("Location", v);
		v = new Value();
		v.setStringType(IconName);
		e.addParameter("IconName", v);
		v = new Value();
		v.setStringType(OwnerID);
		e.addParameter("OwnerID", v);
		v = new Value();
		v.setBooleanType(IsWeapon);
		e.addParameter("IsWeapon", v);
		v = new Value();
		v.setIntegerType(LabelColor);
		e.addParameter("LabelColor", v);
		return e;
	}

	public static SimulationEvent createViewProMotionUpdate(
			String TargetPlayer, String ObjectID, String OwnerID,
			LocationType Location, LocationType DestinationLocation,
			Double MaximumSpeed, Double Throttle, String IconName,
			Boolean IsWeapon) {
		SimulationEvent e = new SimulationEvent(VIEW_PRO_MOTION_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(TargetPlayer);
		e.addParameter("TargetPlayer", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(OwnerID);
		e.addParameter("OwnerID", v);
		v = new Value();
		v.setLocationType(Location);
		e.addParameter("Location", v);
		v = new Value();
		v.setLocationType(DestinationLocation);
		e.addParameter("DestinationLocation", v);
		v = new Value();
		v.setDoubleType(MaximumSpeed);
		e.addParameter("MaximumSpeed", v);
		v = new Value();
		v.setDoubleType(Throttle);
		e.addParameter("Throttle", v);
		v = new Value();
		v.setStringType(IconName);
		e.addParameter("IconName", v);
		v = new Value();
		v.setBooleanType(IsWeapon);
		e.addParameter("IsWeapon", v);
		return e;
	}

	public static SimulationEvent createViewProStopObjectUpdate(String ObjectID) {
		SimulationEvent e = new SimulationEvent(VIEW_PRO_STOP_OBJECT_UPDATE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		return e;
	}

	public static SimulationEvent createWeaponLaunch(String ObjectID,
			String ParentObjectID, String TargetObjectID) {
		SimulationEvent e = new SimulationEvent(WEAPON_LAUNCH);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		return e;
	}

	public static SimulationEvent createWeaponLaunchFailure(String ObjectID,
			String WeaponObjectID, String Reason) {
		SimulationEvent e = new SimulationEvent(WEAPON_LAUNCH_FAILURE);
		Value v = new Value();
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(WeaponObjectID);
		e.addParameter("WeaponObjectID", v);
		v = new Value();
		v.setStringType(Reason);
		e.addParameter("Reason", v);
		return e;
	}

	public static SimulationEvent createWeaponLaunchRequest(String UserID,
			String ObjectID, String ParentObjectID, String TargetObjectID) {
		SimulationEvent e = new SimulationEvent(WEAPON_LAUNCH_REQUEST);
		Value v = new Value();
		v = new Value();
		v.setStringType(UserID);
		e.addParameter("UserID", v);
		v = new Value();
		v.setStringType(ObjectID);
		e.addParameter("ObjectID", v);
		v = new Value();
		v.setStringType(ParentObjectID);
		e.addParameter("ParentObjectID", v);
		v = new Value();
		v.setStringType(TargetObjectID);
		e.addParameter("TargetObjectID", v);
		return e;
	}

	static public SimulationEvent XMLDeserialize(String xml) throws Exception {
		try {

			StringReader in = new StringReader(xml);
			String type = xml.substring(xml.indexOf('<') + 1, xml.indexOf('>'));
			SimulationEvent e = JAXB.unmarshal(in, SimulationEvent.class);
			e._setEventType(type);
			e.msg = xml;
			return e;
		} catch (Exception e) {
			e.printStackTrace();
			throw new Exception(String.format(
					"Invalid XML string : SimulationEvent: %s", xml));
		}

	}

	static public String XMLSerialize(SimulationEvent e) {
		return XMLSerialize(e, false);
	}

	static public String XMLSerialize(SimulationEvent e, boolean prettyPrint) {
		StringWriter s = new StringWriter();
		JAXBElement<SimulationEvent> e2 = new JAXBElement<SimulationEvent>(
				new QName(e._getEventType()), SimulationEvent.class, e);
		JAXB.marshal(e2, s);
		// Transform it to one line
		String str = s.toString().replaceFirst("(<\\?.*\\?>)?", "");
		if (!prettyPrint) {
			str = str.replaceAll("(\\s\\s+)|(\\n)", "");
		}
		return str;
	}

	public static SimulationEvent createExternalApp_Message(String AppName,
			AttributeCollectionType attrs) {
		SimulationEvent e = new SimulationEvent(EXTERNAL_APP_MESSAGE);
		Value v = new Value();
		v = new Value();
		v.setStringType(AppName);
		e.addParameter("AppName", v);
		v = new Value();
		v.setAttributeCollectionType(attrs);
		e.addParameter("Message", v);
		return e;
	}

}
