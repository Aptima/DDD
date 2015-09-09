using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using Aptima.Asim.DDD.CommonComponents.DataTypeTools;
namespace Aptima.Asim.DDD.ScenarioParser
{
    /// 
    public abstract class AbstractParser
    {
        public  abstract string[][] StageMembers();

        public void NotInLanguage(string itemName)
        {
            throw new ApplicationException("Statement " + itemName + " is not in this version of DDD.");
        }
 
        public delegate string StringFromInput();
        public abstract void SetReader(XmlReader r);
  
        public abstract List<string> pGetStringList(Regex separator);
        public abstract List<string> pRetrieveStringList(StringFromInput getString,Regex separator);
        public abstract List<double> pGetDoublesList(int numberExpected);
        public abstract List<double> pGetDoublesList(int lowNumberExpected, int highNumberExpected);
        public abstract List<int> pGetIntList(int lowNumberExpected, int highNumberExpected);

/// <summary>
        /// Get a boolean either as "true"/"false" or as "1"/"0"
        /// </summary>
        /// <returns>Boolean</returns>
        public abstract Boolean pGetBoolean();
        /// <summary>
        /// Reads/returns a string from the input
        /// </summary>
        public abstract string pGetString();
        public abstract string pGetNakedString();
        /// <summary>
        /// Reads/returns a double from the input
        /// </summary>
        public abstract double pGetDouble();
        /// <summary>
        /// Reads/returns an integer from the input
        /// </summary>
        public abstract int pGetInt();
        public abstract pNoteType pGetNote();
        /// <summary>
        /// Retrieves the integer value of a time
        /// </summary>
        public abstract int pGetTime();
   
        public abstract pPointType pGetPoint();
        public abstract string pGetColor();
        /// <summary>
        /// Reads/returns a Location Type from the input
        /// </summary>
        public abstract pLocationType pGetLocation();
        public abstract pTeamType pGetTeam();
        public abstract pNetworkType pGetNetwork();
        /// <summary>
        /// Reads/returns a Velocity Type from the input
        /// </summary>
        public abstract pVelocityType pGetVelocity();
        public abstract pCone pGetCone();
        public abstract pSensor pGetSensor();
        
        /// <summary>
        /// Retrieves a ist of Capabilities
        /// </summary>
        public abstract Dictionary<string, pCapabilityType> pGetCapabilities();
        public abstract pTransitionType pGetTransition();
        public abstract pSingletonVulnerabilityType pGetSingletonVulnerability();
        public abstract Dictionary<string, pSingletonVulnerabilityType> pGetVulnerabilities();
        public abstract pContributionType pGetContribution();
        public abstract pComboVulnerabilityType pGetComboVulnerability();
        public abstract List<pComboVulnerabilityType> pGetCombinations();
  
        public abstract pEmitterType pGetEmitter();
        /// <summary>
        /// Retrieves a set of parameters
        /// </summary>
        public abstract Dictionary<string, object> pGetParameters();
        public abstract pArmamentType pGetArmament();
        public abstract pDockedPlatformType pGetDockedPlatform();
        public abstract pAdoptType pGetAdoptType();
        public abstract pLaunchedPlatformType pGetLaunchedPlatform();
        public abstract pSubplatformType pGetSubplatform();
        public abstract pStateBody pGetStateBody(Boolean requireIcon);
        /// <summary>
        /// Retrieves the information about a single state
        /// </summary>
        public abstract pStateType pGetState();
        /// <summary>
        /// Reads/returns a Playfield directive from the input
        /// </summary>
        public abstract pPlayfieldType pGetPlayfield();
        public abstract pLandRegionType pGetLandRegion();

        public abstract pActiveRegionType pGetActiveRegion();
        /// <summary>
        /// Retrieves the definition of a genus
        /// </summary>
        public abstract pGenusType pGetGenus();
        /// <summary>
        /// Retrieves the definition of a species
        /// </summary>
        /// <returns></returns>
        public abstract pSpeciesType pGetSpecies();
        /// <summary>
        /// Reads/returns a Decision maker directive from the input
        /// </summary>
        public abstract pDecisionMakerType pGetDecisionMaker();
        /// <summary>
        /// Retrieves a create unit command from the scenario 
        /// </summary>
        public abstract pCreateType pGetCreate();
        /// <summary>
        /// Retrieves a reveal command from the scenario 
        /// </summary>
        public abstract pRevealType pGetReveal();
        /// <summary>
        /// Reads/returns a Move Event directive from the input
        /// </summary>
        public abstract pMoveType pGetMove();
        public abstract pHappeningCompletionType pGetHappeningCompletion();
        public abstract pSpeciesCompletionType pGetSpeciesCompletion();
        public abstract pReiterateType pGetReiterate();
        public abstract pStateChangeType pGetStateChange();
        public abstract pTransferType pGetTransfer();
        public abstract pLaunchType pGetLaunch();
        public abstract pWeaponLaunchType pGetWeaponLaunch();
        public abstract pDefineEngramType pGetDefineEngram();
        public abstract pChangeEngramType pGetChangeEngram();
        public abstract pRemoveEngramType pGetRemoveEngram();
        public abstract pEngramRange pGetEngramRange();
        public abstract pOpenChatRoomType pGetOpenChatRoom();
        public abstract pCloseChatRoomType pGetCloseChatRoom();
        public abstract pOpenWhiteboardRoomType pGetOpenWhiteboardRoom();
        public abstract pOpenVoiceChannelType pGetOpenVoiceChannel();
        public abstract pCloseVoiceChannelType pGetCloseVoiceChannel();
        public abstract pGrantVoiceChannelAccessType pGetGrantVoiceChannelAccess();
        public abstract pRemoveVoiceChannelAccessType pGetRemoveVoiceChannelAccess();

        public abstract pSetRegionVisibilityType pGetSetRegionVisibility();
        public abstract pSetRegionActivityType pGetSetRegionActivity();
        public abstract pScoringLocationType pGetScoringLocation();
        public abstract pActorType pGetActor();
        public abstract pScoringRuleType pGetScoringRule();
        public abstract pScoreType pGetScore();
        public abstract pFlushEventsType pGetFlushEventsType();
        public abstract pAttack_Successful_Completion_Type pGetAttackSuccessfulCompletion();
        public abstract pRandomIntervalType pGetRandomInterval();
        public abstract pSendChatMessageType pGetSendChatMessage();
        
        public abstract pApplyType pGetApply();
        public abstract pSendVoiceMessageType pGetSendVoiceMessage();
        public abstract pSendVoiceMessageToUserType pGetSendVoiceMessageToUser();


  
        /// <summary>
        /// The conditions under which an attack request will be approved
        /// </summary>
        /// <returns></returns>
        public abstract pAttack_Request_Approval_Type pGetAttackRequestApproval();
        public abstract List<String> pGetClassifications();
        public abstract ClassificationDisplayRulesValue pGetClassificationDisplayRules();
    }
}
