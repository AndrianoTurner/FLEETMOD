﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FLEETMOD
{
    internal class MyVariables
    {
        public static bool isrunningmod = true;
        public static bool shipfriendlyfire = false;
        public static bool shipgodmode = false;
        //public static float warprange = -1;
        public static int shipcount = 7;
        // variable for storing current survival bonus. Used in UpdatePLPawn
        public static bool recentfriendlyfire = false;
        public static bool DialogGenerated = false;
        public static bool CargoMenu = false;
        public static Dictionary<PLShipInfo, PLPlayer> ShipCrews;
        // ShipID, PLPlayer // List of crews in ship
        public static List<PLShipInfo> Fleet;
        // ShipIDs of the ships in the Fleet
        public static List<PhotonPlayer> FleetmodPhoton;
        public static List<PLPlayer> FleetmodPlayer;
        // PLPlayer of the Players who have Fleetmod active and running
        public static Dictionary<int /*PlayerID*/ , /*Bonus*/ int> survivalBonusDict; 
        // Dictionary that stores <playerID,healthBonus> on hostside, then it's being sent to clients
        public static int MySurvivalBonus; 
        // variable for storing localplayer's healthBonus


       public static PLPlayer GetFleetPlayerFromClassID(int inClassID)
       {
            foreach (PLPlayer player in FleetmodPlayer)
            {
                if (player.GetClassID() == inClassID)
                {
                    return player;
                }
            }
            return null;
       }

        public static int GetShipCaptain (int inShipID)
        {
            foreach (KeyValuePair<PLShipInfo, PLPlayer> pair in ShipCrews)
            {
                if (pair.Value != null && pair.Key == PLEncounterManager.Instance.GetShipFromID(inShipID) && pair.Value.GetClassID() == 0 && pair.Value.TeamID == 0)
                {
                    //if (pLPlayer.GetPhotonPlayer().GetScore() == inShipID)
                    //{
                        return pair.Value.GetPlayerID();
                    //}
                }
            }
            return -1;
        }

        public static bool IsInFleet(int inShipID)
        {
            foreach (KeyValuePair<PLShipInfo, PLPlayer> shipInfo in ShipCrews)
            {
                if (shipInfo.Key == PLEncounterManager.Instance.GetShipFromID(inShipID))
                {

                    return true;

                }          
            }
            return false;
        }
        public static List<PLPlayer> GetShipCrew (int inShipID)
        {
            List<PLPlayer> Crew = null;
            foreach (KeyValuePair<PLShipInfo, PLPlayer> pair in ShipCrews)
            {
                if (pair.Value != null && pair.Key == PLEncounterManager.Instance.GetShipFromID(inShipID) && pair.Value.TeamID == 0)
                {
                    Crew.Add(pair.Value);
                }
            }
            return Crew;
        }
        public static bool ShipHasCaptain (int inShipID)
        {
            if (PLServer.Instance != null && PLEncounterManager.Instance.GetShipFromID(inShipID) != null)
            {
                if (GetShipCaptain(inShipID) != -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
