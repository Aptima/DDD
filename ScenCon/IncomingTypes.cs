using System;
using System.Collections.Generic;

namespace Aptima.Asim.DDD.ScenarioController
{



    /// <summary>
    /// An event providing notification that a previous move command has been executed and completed
    /// </summary>
    public class MoveComplete_Event : ScenarioEventType
    {
        /// <summary>
        /// Constructs a notification that a move was completed
        /// </summary>
        /// <param name="unit">Identifier of the unit that was moved</param>
        public MoveComplete_Event(string unit)
            : base(unit)
        { }
    }

    public class WeaponLaunchRequestType : ScenarioEventType
    {
        private string decisionMaker;
        public string DecisionMaker
        {
            get { return decisionMaker; }
            set { decisionMaker = value; }
        }
        private string targetObjectID;
        public string TargetObjectID
        {
            get { return targetObjectID; }
            set { targetObjectID = value; }
        }
        private string weaponID;
        public string WeaponID
        {
            get { return weaponID; }
        }
    
        /// <summary>
        /// Constructs the weaponattack request
        /// </summary>
        /// <param name="unit">Identifier of the unit doing the attacking</param>
        /// <param name="decisionMaker">The DM id number</param>
        /// <param name="targetObjectID">Identifier of the unit that is to be attacked</param>
        /// <param name="weaponID">Identifier of the individual weapon</param>
        public WeaponLaunchRequestType(string unitID, string decisionMaker, string targetObjectID, string weaponID
            )
            : base(unitID)
        {
            this.decisionMaker = decisionMaker;
            this.TargetObjectID = targetObjectID;
            this.weaponID = weaponID;

        }
    }


    /// <summary>
    /// An event providing notification that a DM wants one unit to attack another using a capability of the unit
    /// </summary>   

    public class AttackObjectRequestType : ScenarioEventType
    {

        private string decisionMaker;
        public string DecisionMaker
        {
            get { return decisionMaker; }
            set { decisionMaker = value; }
        }
        private string targetObjectID;
        public string TargetObjectID
        {
            get { return targetObjectID; }
            set { targetObjectID = value; }
        }
        private string capabilityName;
        public string CapabilityName
        {
            get { return capabilityName; }
        }
        /// <summary>
        /// Constructs the attack request
        /// </summary>
        /// <param name="unit">Identifier of the unit doing the attacking</param>
        /// <param name="decisionMaker">The DM id number</param>
        /// <param name="targetObjectID">Identifier of the unit that is to be attacked</param>
        public AttackObjectRequestType(string unit, string decisionMaker, string targetObjectID,
            string capabilityName)
            : base(unit)
        {
            this.decisionMaker = decisionMaker;
            this.TargetObjectID = targetObjectID;
            this.capabilityName = capabilityName;

        }
    }



    /// <summary>
    /// Notification that a DM wants to cause a unit to move in a particular way
    /// </summary>
    public class MoveObjectRequestType : ScenarioEventType
    {

        private string decisionMaker;
        public string DecisionMaker
        {
            get { return decisionMaker; }
            set { decisionMaker = value; }
        }
        private LocationType destination;
        public LocationType Destination
        {
            get { return destination; }
            set { destination = value; }

        }
        private double throttle;
        public double Throttle
        {
            get { return throttle; }
            set { throttle = value; }
        }
        /// <summary>
        /// Constructs the notification
        /// </summary>
        /// <param name="unitID">Ientifier of the unit being mover.</param>
        /// <param name="decisionMaker">DM making the request</param>
        /// <param name="destination">Destination location</param>
        /// <param name="throttle">Fraction of full throttle to use</param>
        public MoveObjectRequestType(string unitID, string decisionMaker, LocationType destination, double throttle)
            : base(unitID)
        {

            this.DecisionMaker = decisionMaker;
            this.Destination = destination;
            this.throttle = throttle;
        }
    }


    public class SubplatformLaunchRequestType : ScenarioEventType
    {
        private string requestor;
        public string Requestor
        {
            get { return requestor; }
        }

        private string parentUnit;
        public string ParentUnit
        {
            get { return parentUnit; }
              }
        private LocationType destination;
        public LocationType Destination
        {
            get { return destination; }
        }

        /// <summary>
        /// Constructs the  request
        /// </summary>
        /// <param name="unit">Identifier of the unit doing the attacking</param>
        /// <param name="decisionMaker">The DM id number</param>
        /// <param name="targetObjectID">Identifier of the unit that is to be attacked</param>
        /// <param name="weaponID">Identifier of the individual weapon</param>
        public SubplatformLaunchRequestType(string requestor, string childUnit, string parentUnit, LocationType destination
            )
            : base(childUnit)
        {
            this.requestor = requestor;
            this.parentUnit = parentUnit;
            this.destination = destination;

        }
    }


    public class SubplatformDockRequestType : ScenarioEventType
    {
        private string requestor;
        public string Requestor
        {
            get { return requestor; }
        }

        private string parentUnit;
        public string ParentUnit
        {
            get { return parentUnit; }
        }
  

        /// <summary>
        /// Constructs the  request
        /// </summary>
         public SubplatformDockRequestType(string requestor, string childUnit, string parentUnit
            )
            : base(childUnit)
        {
            this.requestor = requestor;
            this.parentUnit = parentUnit;
    
        }
    }

    public class TransferObjectRequest : ScenarioEventType
    {
        private string requestor;
        public string Requestor
        {
            get { return requestor; }
        }

        private string recipient;
        public string Recipient
        {
            get { return recipient; }
        }
        private string state;
        public string State
        {
            get {return state;}
            set { state = value; }
        }

        /// <summary>
        /// Constructs the  request
        /// </summary>
        public TransferObjectRequest(string requestor, string recipient,string asset,string state)
           
            : base(asset)
        {
            this.requestor = requestor;
            this.recipient = recipient;
            this.State=state;

        }
    }


    public class StateChangeNotice : ScenarioEventType
    {
        private string newState;
        public string NewState
        {
            get { return newState; }
        }
        public StateChangeNotice(string unitID, string newState)
            : base(unitID)
        {
            this.newState = newState;
        }
    }

     public class SelfDefenseAttackNotice : ScenarioEventType
    {
        private string target;
        public string Target
        {
            get { return target; }
        }
        public SelfDefenseAttackNotice(string unitID, string target)
            : base(unitID)
        {
            this.target = target;
        }
    }

    public class AttackSuccessfulNotice : ScenarioEventType
    {
        private string target;
        public string Target
        {
            get { return target; }
        }
        private List<string> capabilities;
        public List<string> Capabilities
        {
            get { return capabilities; }
        }
        private string newState;
        public string NewState
        {
            get { return newState; }
        }
        public AttackSuccessfulNotice(string unitID, string target, List<string> capabilities, string newState)
            : base(unitID)
        {
            this.target = target;
            this.capabilities = capabilities;
            this.newState = newState;
        }

    }

}