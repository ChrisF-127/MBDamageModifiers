using HarmonyLib;
using SandBox.Missions.MissionLogics.Arena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace DamageModifiers
{
	[HarmonyPatch(typeof(Mission), "GetDamageMultiplierOfCombatDifficulty")]
	internal static class Patch_Mission_GetDamageMultiplierOfCombatDifficulty
	{
		[HarmonyPostfix]
		public static void Postfix(Mission __instance, ref float __result, Agent victimAgent, Agent attackerAgent)
		{
			try
			{
				// check if arena fight
				var isArenaFight = __instance.MissionBehaviors.IsArenaFight();

				// get riders and agent types
				var attackerType = attackerAgent.GetAgentType();
				var victimType = victimAgent.GetAgentType();

				__result = DamageModifierHelper.GetDamageMultiplier(attackerType, victimType, isArenaFight);
			}
			catch (Exception exc)
			{
				// catch anything going wrong, just in case - we all know how much Bannerlord loves to crash otherwise!
				FileLog.Log($"{nameof(DamageModifiers)}.{nameof(Patch_Mission_GetDamageMultiplierOfCombatDifficulty)}: [{exc.GetType()}] {exc.Message}\n{exc.StackTrace}");
			}
		}
	}
}
