using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace FLEETMOD
{
    [HarmonyPatch(typeof(PLPawn),"Update")]
    internal class UpdatePLPawn
    {

		// This fixes the health issue with death of a player that is the same class
		// Although rejoining resets max health to base health
        static void Postfix(PLPawn __instance)
		{ 
				if (__instance.GetPlayer() != null && __instance.GetPlayer().GetPlayerID() != -1)
                {
					MyVariables.MySurvivalBonus = MyVariables.survivalBonusDict[__instance.GetPlayer().GetPlayerID()];
						float BaseHealth = 100f;
						if (__instance.GetPlayer().RaceID == 2)
						{
							BaseHealth = 60f;
						}
						float ModifiedHealth = BaseHealth + (float)__instance.GetPlayer().Talents[0] * 20f;
						ModifiedHealth += (float)__instance.GetPlayer().Talents[57] * 20f;
						foreach (PawnStatusEffect pawnStatusEffect5 in __instance.MyStatusEffects)
						{
							if (pawnStatusEffect5 != null && pawnStatusEffect5.Type == EPawnStatusEffectType.HEALTH_REGEN)
							{
								ModifiedHealth += 20f;
							}
						}
						float value2 = ModifiedHealth;
						if (__instance.GetPlayer().GetClassID() != -1 && __instance.GetPlayer().GetClassID() < 5)
						{						
							ModifiedHealth +=  MyVariables.MySurvivalBonus * 5f;
						}
						if (__instance.MaxHealth != ModifiedHealth)
						{
							__instance.Health = __instance.Health / __instance.MaxHealth * ModifiedHealth;
							__instance.MaxHealth = ModifiedHealth;
							__instance.MaxHealth_Normal = value2;
						}
				}
			
			

		}
    }
}
