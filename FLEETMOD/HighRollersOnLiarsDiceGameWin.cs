using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace FLEETMOD
{
    /*[HarmonyPatch(typeof(PLHighRollersShipInfo), "OnLiarsDiceGameWin")]
    internal class HighRollersOnLiarsDiceGameWin
    {
        public static void Postfix(PLHighRollersShipInfo __instance,PLPlayer player, PLLiarsDiceGame game)
        {
			if (player != null)
			{
				if (MyVariables.IsInFleet(player.StartingShip.ShipID) && Array.IndexOf<PLLiarsDiceGame>(__instance.SmallGames, game) != -1)
				{
					__instance.CrewChips++;
				}
				
			}
		}
    }
	*/
}