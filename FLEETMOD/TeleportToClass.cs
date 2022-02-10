using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Pathfinding;
using UnityEngine;

namespace FLEETMOD
{
	[HarmonyPatch(typeof(PLPlayer),"TeleportToClass")]
    internal class TeleportToClass
    {
		public static void Postifix(PLPlayer __instance ,int classID)
		{
			
			// This code doesn't do anything
            
				if (PLServer.Instance == null)
				{
					return;
				}
				PLPlayer cachedFriendlyPlayerOfClass = MyVariables.GetFleetPlayerFromClassID(classID);
				if (cachedFriendlyPlayerOfClass == null || cachedFriendlyPlayerOfClass.GetPawn() == null)
				{
					return;
				}
				PLPathfinderGraphEntity pgeforPlayer = PLPathfinder.GetInstance().GetPGEforPlayer(cachedFriendlyPlayerOfClass, true);  
				NNConstraint nnconstraint = new NNConstraint();
				nnconstraint.area = (int)cachedFriendlyPlayerOfClass.GetPawn().AreaIndex;
				nnconstraint.constrainArea = true;
				nnconstraint.constrainWalkability = true;
				if (pgeforPlayer != null && __instance != cachedFriendlyPlayerOfClass && __instance.StartingShip.ShipID == cachedFriendlyPlayerOfClass.StartingShip.ShipID && (!cachedFriendlyPlayerOfClass.GetPawn().SpawnedInArena || !cachedFriendlyPlayerOfClass.GetPawn().IsDead))
				{
					__instance.GetPawn().SpawnedInArena = cachedFriendlyPlayerOfClass.GetPawn().SpawnedInArena;
					__instance.GetPawn().CurrentShip = cachedFriendlyPlayerOfClass.GetPawn().CurrentShip;
					Vector3 vector = cachedFriendlyPlayerOfClass.GetPawn().transform.position + Vector3.Scale(UnityEngine.Random.onUnitSphere, new Vector3(1f, 0.2f, 1f)).normalized * 2f;
					vector = pgeforPlayer.Graph.GetNearest(vector, nnconstraint).clampedPosition + Vector3.up * 0.6f;
					__instance.photonView.RPC("NetworkTeleportToSubHub", PhotonTargets.All, new object[]
					{
						cachedFriendlyPlayerOfClass.MyCurrentTLI.SubHubID,
						0
					});
					__instance.photonView.RPC("RecallPawnToPos", PhotonTargets.All, new object[]
					{
						vector
					});
				
			}
			
		}
	}
}
