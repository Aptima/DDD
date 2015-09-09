package com.aptima.ddd.networktools;

// Auto Generated from DDD schema file using python script enum.py
public enum SimulationEventTypes {
	ACTIVE_REGION_SPEED_MULTIPLIER_UPDATE(
	"ActiveRegionSpeedMultiplierUpdate"), ACTIVE_REGION_UPDATE("ActiveRegionUpdate"), ADD_TO_CHAT_ROOM("AddToChatRoom"), ADD_TO_VOICE_CHANNEL(
			"AddToVoiceChannel"), ATTACK_OBJECT("AttackObject"), ATTACK_OBJECT_REQUEST("AttackObjectRequest"), ATTACK_SUCCEEDED(
			"AttackSucceeded"), AUTHENTICATION_REQUEST(
			"AuthenticationRequest"), AUTHENTICATION_RESPONSE(
			"AuthenticationResponse"), BASE_EVENT("BaseEvent"), CHANGE_TAG_REQUEST(
			"ChangeTagRequest"), CLIENT_ATTACK_REQUEST(
			"ClientAttackRequest"), CLIENT_REMOVE_OBJECT(
			"ClientRemoveObject"), CLIENT_SIDE_ASSET_TRANSFER_ALLOWED(
			"ClientSideAssetTransferAllowed"), CLOSE_CHAT_ROOM("CloseChatRoom"), CLOSE_VOICE_CHANNEL("CloseVoiceChannel"), CREATE_CHAT_ROOM(
			"CreateChatRoom"), CREATE_VOICE_CHANNEL(
			"CreateVoiceChannel"), DISCONNECT_DECISION_MAKER(
			"DisconnectDecisionMaker"), DISCONNECT_TERMINAL(
			"DisconnectTerminal"), ENGRAM_VALUE(
			"EngramValue"), EXTERNAL_APP_LOG(
			"ExternalApp_Log"), EXTERNAL_APP_MESSAGE("ExternalApp_Message"), EXTERNAL_APP_SIM_START(
			"ExternalApp_SimStart"), EXTERNAL_APP_SIM_STOP(
			"ExternalApp_SimStop"), EXTERNAL_EMAIL_RECEIVED(
			"ExternalEmailReceived"), FAILED_TO_CREATE_CHAT_ROOM(
					"FailedToCreateChatRoom"), GAME_SPEED("GameSpeed"), GAME_SPEED_REQUEST(
			"GameSpeedRequest"), HANDSHAKE_AVAILABLE_PLAYERS(
			"HandshakeAvailablePlayers"), HANDSHAKE_GUIREGISTER(
			"HandshakeGUIRegister"), HANDSHAKE_GUIROLE_REQUEST(
			"HandshakeGUIRoleRequest"), HANDSHAKE_INITIALIZE_GUI(
			"HandshakeInitializeGUI"), HANDSHAKE_INITIALIZE_GUIDONE(
			"HandshakeInitializeGUIDone"), HISTORY_ATTACKED_OBJECT_REPORT(
			"History_AttackedObjectReport"), HISTORY_ATTACKER_OBJECT_REPORT(
			"History_AttackerObjectReport"), HISTORY_PURSUE("History_Pursue"), HISTORY_SUBPLATFORM_LAUNCH(
			"History_SubplatformLaunch"), JOIN_CHAT_ROOM("JoinChatRoom"), JOIN_VOICE_CHANNEL("JoinVoiceChannel"), LEAVE_VOICE_CHANNEL("LeaveVoiceChannel"), LOAD_SCENARIO_REQUEST("LoadScenarioRequest"), MOVE_DONE("MoveDone"), MOVE_OBJECT("MoveObject"), MOVE_OBJECT_REQUEST(
			"MoveObjectRequest"), NEW_OBJECT(
			"NewObject"), NONE("None"), OBJECT_COLLISION(
			"ObjectCollision"), PAUSE_SCENARIO("PauseScenario"), PAUSE_SCENARIO_REQUEST("PauseScenarioRequest"), PLAY_VOICE_MESSAGE("PlayVoiceMessage"), PLAYER_CONTROL(
			"PlayerControl"), PLAYFIELD("Playfield"), PUSH_TO_TALK(
			"PushToTalk"), RANDOM_SEED("RandomSeed"), REMOVE_FROM_VOICE_CHANNEL(
			"RemoveFromVoiceChannel"), REQUEST_CHAT_ROOM_CREATE("RequestChatRoomCreate"), REQUEST_CLOSE_CHAT_ROOM(
			"RequestCloseChatRoom"), REQUEST_JOIN_VOICE_CHANNEL(
			"RequestJoinVoiceChannel"), REQUEST_LEAVE_VOICE_CHANNEL(
			"RequestLeaveVoiceChannel"), RESET_SIMULATION(
			"ResetSimulation"), RESUME_SCENARIO(
			"ResumeScenario"), RESUME_SCENARIO_REQUEST(
			"ResumeScenarioRequest"), REVEAL_OBJECT("RevealObject"), SCORE_UPDATE("ScoreUpdate"), SELF_DEFENSE_ATTACK_STARTED(
			"SelfDefenseAttackStarted"), SERVER_STATE(
			"ServerState"), SIM_CORE_READY("SimCoreReady"), SIMULATION_TIME_EVENT("SimulationTimeEvent"), STARTUP_COMPLETE(
			"StartupComplete"), STATE_CHANGE("StateChange"), STOP_REPLAY("StopReplay"), STOP_SCENARIO("StopScenario"), STOP_SCENARIO_REQUEST(
			"StopScenarioRequest"), STOPPED_TALKING(
			"StoppedTalking"), SUBPLATFORM_DOCK("SubplatformDock"), SUBPLATFORM_DOCK_REQUEST(
			"SubplatformDockRequest"), SUBPLATFORM_LAUNCH(
			"SubplatformLaunch"), SUBPLATFORM_LAUNCH_REQUEST(
			"SubplatformLaunchRequest"), SYSTEM_MESSAGE("SystemMessage"), TEXT_CHAT(
			"TextChat"), TEXT_CHAT_REQUEST(
			"TextChatRequest"), TIME_TICK(
			"TimeTick"), TRANSFER_OBJECT(
			"TransferObject"), TRANSFER_OBJECT_REQUEST("TransferObjectRequest"), UPDATE_TAG("UpdateTag"), VIEW_PRO_ACTIVE_REGION_UPDATE(
			"ViewProActiveRegionUpdate"), VIEW_PRO_ATTACK_UPDATE(
			"ViewProAttackUpdate"), VIEW_PRO_ATTRIBUTE_UPDATE(
			"ViewProAttributeUpdate"), VIEW_PRO_INITIALIZE_OBJECT(
			"ViewProInitializeObject"), VIEW_PRO_MOTION_UPDATE(
			"ViewProMotionUpdate"), VIEW_PRO_STOP_OBJECT_UPDATE(
			"ViewProStopObjectUpdate"), WEAPON_LAUNCH(
			"WeaponLaunch"), WEAPON_LAUNCH_FAILURE("WeaponLaunchFailure"), WEAPON_LAUNCH_REQUEST(
			"WeaponLaunchRequest");

	public static SimulationEventTypes getType(String name) {
		return valueOf(name.replaceAll("([a-z])([A-Z])", "$1_$2").toUpperCase());
	}

	private final String type;

	SimulationEventTypes(String value) {
		this.type = value;
	}

	/**
	 * @return the type
	 */
	public String getType() {
		return type;
	}

	@Override
	public String toString() {
		return type;
	}
}