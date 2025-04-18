using HarmonyLib;
using SandBox.Missions.MissionLogics.Arena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace DamageModifiers
{
	internal enum AgentType
	{
		Player = 0,
		Hero = 1,
		Other = 2,
		Mount = 3,
	}

	internal static class DamageModifierHelper
	{
		internal static void AdjustInflictedDamage(
			Agent attackerAgent,
			Agent victimAgent,
			GameEntity hitObject,
			ref AttackInformation attackInformation,
			ref AttackCollisionData attackCollisionData)
		{
			try
			{
				// apply modifier
				attackCollisionData.InflictedDamage = MathF.Round(attackCollisionData.InflictedDamage * GetDamageMultiplier(attackerAgent, victimAgent, hitObject != null, ref attackInformation, ref attackCollisionData));
			}
			catch (Exception exc)
			{
				// catch anything going wrong, just in case - we all know how much Bannerlord loves to crash otherwise!
				FileLog.Log($"{nameof(DamageModifiers)}.{nameof(AdjustInflictedDamage)}: [{exc.GetType()}] {exc.Message}\n{exc.StackTrace}");
			}
		}

		internal static bool IsArenaFight(this List<MissionBehavior> missionBehaviors)
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

		internal static AgentType GetAgentType(this Agent agent)
		{
			// is mount?
			if (agent?.IsMount == true)
			{
				// get rider
				agent = agent.RiderAgent;
				// riderless mount
				if (agent == null)
					return AgentType.Mount;
			}
			// valid agent?
			if (agent != null)
			{
				// is non-AI-controlled agent aka player?
				if (!agent.IsAIControlled)
					return AgentType.Player;
				// is hero?
				if (agent.Character?.IsHero == true)
					return AgentType.Hero;
			}
			// anyone else
			return AgentType.Other;
		}

		internal static float GetDamageMultiplier(
			Agent attackerAgent,
			Agent victimAgent,
			bool isObjectHit,
			ref AttackInformation attackInformation,
			ref AttackCollisionData attackCollisionData)
		{
			// fall damage multiplier
			if (attackCollisionData.IsFallDamage)
				return DamageModifiers.Settings.Fall;

			// check if arena fight
			var isArenaFight = Mission.Current.MissionBehaviors.IsArenaFight();

			// get riders and agent types
			var attackerType = attackerAgent.GetAgentType();
			var victimType = victimAgent.GetAgentType();

			var output = 1f;
			// modifier depending on attacker and victim
			switch (attackerType)
			{
				// attacker: player
				case AgentType.Player:
					switch (victimType)
					{
						// victim: player
						case AgentType.Player:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaPlayerPlayer :
								DamageModifiers.Settings.BattlePlayerPlayer;
							break;
						// victim: hero
						case AgentType.Hero:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaPlayerHero :
								DamageModifiers.Settings.BattlePlayerHero;
							break;
						// victim: other
						case AgentType.Other:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaPlayerOther :
								DamageModifiers.Settings.BattlePlayerOther;
							break;
						// victim: mount
						case AgentType.Mount:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaAnyMount :
								DamageModifiers.Settings.BattleAnyMount;
							break;
					}
					break;
				// attacker: hero
				case AgentType.Hero:
					switch (victimType)
					{
						// victim: player
						case AgentType.Player:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaHeroPlayer :
								DamageModifiers.Settings.BattleHeroPlayer;
							break;
						// victim: hero
						case AgentType.Hero:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaHeroHero :
								DamageModifiers.Settings.BattleHeroHero;
							break;
						// victim: other
						case AgentType.Other:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaHeroOther :
								DamageModifiers.Settings.BattleHeroOther;
							break;
						// victim: mount
						case AgentType.Mount:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaAnyMount :
								DamageModifiers.Settings.BattleAnyMount;
							break;
					}
					break;
				// attacker: other
				case AgentType.Other:
				// attacker: mount
				case AgentType.Mount:
					switch (victimType)
					{
						// victim: player
						case AgentType.Player:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaOtherPlayer :
								DamageModifiers.Settings.BattleOtherPlayer;
							break;
						// victim: hero
						case AgentType.Hero:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaOtherHero :
								DamageModifiers.Settings.BattleOtherHero;
							break;
						// victim: other
						case AgentType.Other:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaOtherOther :
								DamageModifiers.Settings.BattleOtherOther;
							break;
						// victim: mount
						case AgentType.Mount:
							output *= isArenaFight ?
								DamageModifiers.Settings.ArenaAnyMount :
								DamageModifiers.Settings.BattleAnyMount;
							break;
					}
					break;
			}

			// object damage modifier
			if (isObjectHit)
				output *= DamageModifiers.Settings.Objects;

			// horse charge damage modifier
			if (attackCollisionData.IsHorseCharge)
				output *= DamageModifiers.Settings.HorseCharge;
			// ranged damage modifier
			else if (attackCollisionData.IsMissile)
				output *= DamageModifiers.Settings.Ranged;
			// melee damage modifier
			else
				output *= DamageModifiers.Settings.Melee;

			// shield damage modifier
			if (attackCollisionData.AttackBlockedWithShield)
				output *= DamageModifiers.Settings.Shields;

			// output
			return output;
		}
	}
}
