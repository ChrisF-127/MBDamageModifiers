using HarmonyLib;
using MonoMod.Utils;
using SandBox.Missions.MissionLogics.Arena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace DamageModifiers
{
	internal static class HarmonyPatches
	{
		public static void ApplyPatches()
		{
			var harmony = new Harmony("sy.damagemodifiers");

			harmony.Patch(AccessTools.Method(typeof(Mission), "GetAttackCollisionResults"), 
				transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
		}

		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			var applied = false;
			var list = instructions.ToList();
			for (int i = 0; i < list.Count; i++)
			{
				//    ldloca.s 2 (System.Int32)
				// >> call static System.Void TaleWorlds.MountAndBlade.MissionCombatMechanicsHelper::GetAttackCollisionResults(TaleWorlds.MountAndBlade.AttackInformation& attackInformation, System.Boolean crushedThrough, System.Single momentumRemaining, TaleWorlds.MountAndBlade.MissionWeapon& attackerWeapon, System.Boolean cancelDamage, TaleWorlds.MountAndBlade.AttackCollisionData& attackCollisionData, TaleWorlds.MountAndBlade.CombatLogData& combatLog, System.Int32& speedBonus)
				// ++ ldarg.1 NULL
				// ++ ldarg.2 NULL
				// ++ ldarg.3 NULL
				// ++ ldloca.s 0 (TaleWorlds.MountAndBlade.AttackInformation)
				// ++ ldarg.s 9
				// ++ call static System.Void DamageModifiers.DamageModifierHelper::Magic(TaleWorlds.MountAndBlade.Agent attackerAgent, TaleWorlds.MountAndBlade.Agent victimAgent, TaleWorlds.Engine.GameEntity hitObject, TaleWorlds.MountAndBlade.AttackInformation& attackInformation, TaleWorlds.MountAndBlade.AttackCollisionData& attackCollisionData)
				//    ldarg.s 9
				//    ldfld System.Int32 TaleWorlds.MountAndBlade.AttackCollisionData::InflictedDamage
				if (list[i].opcode == OpCodes.Call
					&& list[i].operand is MethodInfo mi
					&& mi.DeclaringType == typeof(MissionCombatMechanicsHelper)
					&& mi.Name == "GetAttackCollisionResults")
				{
					list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_1));
					list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_2));
					list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_3));
					list.Insert(++i, new CodeInstruction(OpCodes.Ldloca_S, 0));
					list.Insert(++i, new CodeInstruction(OpCodes.Ldarg_S, 9));
					list.Insert(++i, new CodeInstruction(OpCodes.Call, typeof(DamageModifierHelper).GetMethod(nameof(DamageModifierHelper.AdjustInflictedDamage), BindingFlags.Static | BindingFlags.NonPublic)));
					applied = true;
				}
			}
			if (!applied)
				throw new Exception($"{nameof(DamageModifiers)}: patch not applied!");
			return list;
		}
	}
}
