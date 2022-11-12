using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace DamageModifiers
{
	[HarmonyPatch(typeof(Mission), "GetDamageMultiplierOfCombatDifficulty")]
	internal static class PatchGetDamageMultiplierOfCombatDifficulty
	{
		[HarmonyPostfix]
		public static void Postfix(Mission __instance, ref float __result, Agent victimAgent, Agent attackerAgent)
		{
			FileLog.Log($"{__instance.GetType()}\t{__instance.CombatType}\t{__instance.Mode}\t{victimAgent.Name}\t{attackerAgent?.Name}\t{__result}");
		}
	}
}
