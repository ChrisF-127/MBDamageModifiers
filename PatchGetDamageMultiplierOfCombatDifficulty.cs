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
using static TaleWorlds.MountAndBlade.Mission;

namespace DamageModifiers
{
	[HarmonyPatch(typeof(Mission), "GetDamageMultiplierOfCombatDifficulty")]
	internal static class PatchGetDamageMultiplierOfCombatDifficulty
	{
		[HarmonyPostfix]
		public static void Postfix(Mission __instance, ref float __result, Agent victimAgent, Agent attackerAgent)
		{
			try
			{
				// check for horse
				victimAgent = victimAgent?.IsMount == true ? victimAgent.RiderAgent : victimAgent;
				if (victimAgent == null)
					return;

				// check for arena/tournament fights
				var isArena = IsArenaFight(__instance.MissionBehaviors);

				// use adjusted damage
				if (!victimAgent.IsAIControlled)
					__result *= isArena ? DamageModifiers.Settings.ArenaPlayerModifier : DamageModifiers.Settings.BattlePlayerModifier;
				else if (victimAgent.IsHero)
					__result *= isArena ? DamageModifiers.Settings.ArenaHeroModifier : DamageModifiers.Settings.BattleHeroModifier;

				//FileLog.Log(
				//	$"IsArena {isArena}\t" +
				//	$"Hero {victimAgent.Character?.IsHero}\t" +
				//	$"AIControlled {victimAgent.IsAIControlled}\t" +
				//	$"Mount {victimAgent.IsMount}\t" +
				//	$"Result {__result}");
			}
			catch (Exception e)
			{
				FileLog.Log(e.ToString());
			}
		}

		private static bool IsArenaFight(List<MissionBehavior> missionBehaviors)
		{
			if (missionBehaviors != null)
			{
				for (int i = 0; i < missionBehaviors.Count; i++)
				{
					// Arena fights and Tournaments
					if (missionBehaviors[i] is ArenaAgentStateDeciderLogic)
						return true;
				}
			}
			return false;
		}
	}
}
