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
				// try to get rider if victim is a mount
				victimAgent = victimAgent?.IsMount == true ? victimAgent.RiderAgent : victimAgent;

				// no modifiers if victim is null (for example riderless horse)
				if (victimAgent == null)
					return;

				// check if victim is player controlled
				if (!victimAgent.IsAIControlled)
				{
					// apply arena or battle modifier for player
					__result *= IsArenaFight(__instance.MissionBehaviors) ? 
						DamageModifiers.Settings.ArenaPlayerModifier : 
						DamageModifiers.Settings.BattlePlayerModifier;
				}
				// check if victim is hero
				else if (victimAgent.Character?.IsHero == true)
				{
					// apply arena or battle modifier for hero
					__result *= IsArenaFight(__instance.MissionBehaviors) ? 
						DamageModifiers.Settings.ArenaHeroModifier : 
						DamageModifiers.Settings.BattleHeroModifier;
				}
			}
			catch (Exception exc)
			{
				// catch anything going wrong, just in case - we all know how much Bannerlord loves to crash otherwise!
				FileLog.Log($"{nameof(DamageModifiers)}.{nameof(Patch_Mission_GetDamageMultiplierOfCombatDifficulty)}: [{exc.GetType()}] {exc.Message}\n{exc.StackTrace}");
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
