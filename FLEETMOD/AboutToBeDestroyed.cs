﻿using System;
using HarmonyLib;
using UnityEngine;

namespace FLEETMOD
{
	[HarmonyPatch(typeof(PLShipInfoBase), "AboutToBeDestroyed")]
	internal class AboutToBeDestroyed
	{
		public static bool Prefix(PLShipInfoBase __instance)
		{
			if (!MyVariables.isrunningmod)
			{
				return true;
			}
			else
			{
				/*continue if following evaluates true
				 *&&
				 *Ship has NOT been destroyed
				 *Server Instance != null
				 *Local Player != null
				 *Server GameHasStarted
				 *Local Player Has Started
				*/
				if (!__instance.HasBeenDestroyed && PLServer.Instance != null && PLNetworkManager.Instance.LocalPlayer != null && PLServer.Instance.GameHasStarted && PLNetworkManager.Instance.LocalPlayer.GetHasStarted())
				{
					/*continue if following evaluates true
					 *&&
					 *this Ship is part of the Fleet
					 *Local Player is MasterClient
					*/
					if (__instance.TagID == -23 && PhotonNetwork.isMasterClient)
					{
						PLServer.Instance.photonView.RPC("AddCrewWarning", PhotonTargets.All, new object[]
						{
							"The " + __instance.ShipNameValue + " Destroyed!",
							Color.green,
							0,
							"SHIP"
						});
					}
					/*continue if following evaluates true
					 *&&
					 *Local Player is Master Client
					 *Local Player Ship != this Ship
					*/
					if (PhotonNetwork.isMasterClient && PLNetworkManager.Instance.LocalPlayer.StartingShip != __instance as PLShipInfo)
					{
						/*foreach player on the server
						 *continue if following evaluates true
						 *&&
						 *player != null
						 *player photonplayer != null
						 *player != masterclient
						 *player != bot
						 *player ship != this ship
						 *<summary>
						 *Moves this ship crew to Host ship
						 *</summary>
						*/
						foreach (PLPlayer plplayer in PLServer.Instance.AllPlayers)
						{
							if (plplayer != null && plplayer.GetPhotonPlayer() != null && !plplayer.GetPhotonPlayer().IsMasterClient && !plplayer.IsBot && plplayer.StartingShip == __instance)
							{
                                				plplayer.SetClassID(1);
								plplayer.GetPhotonPlayer().SetScore(PhotonNetwork.player.GetScore());
								plplayer.StartingShip = PLNetworkManager.Instance.LocalPlayer.StartingShip;
							}
						}
					}
					/*continue if following evaluates true
					 *&&
					 *Local Player is Master Client
					 *Local Player Ship == this Ship
					*/
                    			if (PhotonNetwork.isMasterClient && PLNetworkManager.Instance.LocalPlayer.StartingShip == __instance as PLShipInfo)
			                {
						/*foreach player on the server
						 *continue if following evaluates true
						 *&&
						 *player != null
						 *player photonplayer != null
						 *player != masterclient
						 *player != bot
						 *<summary>
						 *Moves this Host crew to other ship
						 *##BROKEN
						 *</summary>
						*/
				                foreach (PLPlayer plplayer in PLServer.Instance.AllPlayers)
                        			{
				                        if (plplayer != null && plplayer.GetPhotonPlayer() != null && !plplayer.GetPhotonPlayer().IsMasterClient && !plplayer.IsBot)
				                        {
				                                plplayer.GetPhotonPlayer().SetScore(PhotonNetwork.player.GetScore());
      					                        plplayer.StartingShip = PLNetworkManager.Instance.LocalPlayer.StartingShip;
				                        }
			                        }
			                }
					/*continue if following evaluates true
					 *Local Player is Master Client
					 *Local Player != null
					 *Local Player Current Ship == Fleet Ship
					 *Local Player Current Ship == this ship
					 *Local Player Main Ship != this ship
					*/
			                if (PhotonNetwork.isMasterClient && PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip.TagID == -23 && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip == __instance && PLNetworkManager.Instance.LocalPlayer.StartingShip != __instance as PLShipInfo)
					{
						PLNetworkManager.Instance.LocalPlayer.photonView.RPC("NetworkTeleportToSubHub", PhotonTargets.All, new object[]
						{
							PLNetworkManager.Instance.LocalPlayer.StartingShip.MyTLI.SubHubID,
							0
						});
					}
             				__instance.TagID = -1;
				}
				return true;
			}
		}
	}
}
 
