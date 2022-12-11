using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace DamageModifiers
{
	public class MCMSettings : AttributeGlobalSettings<MCMSettings>
	{
		public override string Id => "DamageModifiers";
		public override string DisplayName => "Damage Modifiers";
		public override string FolderName => "DamageModifiers";
		public override string FormatType => "json";

		#region GENERAL
		const string GroupNameGeneral = "General Settings";

		[SettingPropertyBool(
			"Apply Damage Dealt against Player",
			RequireRestart = false,
			HintText = "Also apply damage dealt modifiers against player. [Native: false]",
			Order = 0)]
		[SettingPropertyGroup(
			GroupNameGeneral,
			GroupOrder = 0)]
		public bool ApplyAttackerModifierAgainstPlayer { get; set; } = false;

		[SettingPropertyBool(
			"Apply Damage Dealt against Heroes",
			RequireRestart = false,
			HintText = "Also apply damage dealt modifiers against heroes. [Native: false]",
			Order = 1)]
		[SettingPropertyGroup(
			GroupNameGeneral,
			GroupOrder = 0)]
		public bool ApplyAttackerModifierAgainstHeroes { get; set; } = false;
		#endregion

		#region BATTLE MODIFIERS
		const string GroupNameBattle = "Battles etc.";

		[SettingPropertyFloatingInteger(
			"Player Damage Received",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage received modifier for the player controlled character in any non-arena/tournament fight (further modified by difficulty setting). [Native: 100%]",
			Order = 0)]
		[SettingPropertyGroup(
			GroupNameBattle,
			GroupOrder = 1)]
		public float BattlePlayerModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Hero Damage Received",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage received modifier for heroes in any non-arena/tournament fight (further modified by difficulty setting). [Native: 100%]",
			Order = 1)]
		[SettingPropertyGroup(
			GroupNameBattle,
			GroupOrder = 1)]
		public float BattleHeroModifier { get; set; } = 1f;


		[SettingPropertyFloatingInteger(
			"Player Damage Dealt",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage dealt modifier for the player controlled character in any non-arena/tournament fight (further modified by difficulty setting). [Native: 100%]",
			Order = 2)]
		[SettingPropertyGroup(
			GroupNameBattle,
			GroupOrder = 1)]
		public float BattlePlayerAttackerModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Hero Damage Dealt",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage dealt modifier for heroes in any non-arena/tournament fight (further modified by difficulty setting). [Native: 100%]",
			Order = 3)]
		[SettingPropertyGroup(
			GroupNameBattle,
			GroupOrder = 1)]
		public float BattleHeroAttackerModifier { get; set; } = 1f;
		#endregion

		#region ARENA MODIFIERS
		const string GroupNameArena = "Arena & Tournament";

		[SettingPropertyFloatingInteger(
			"Player Damage Received",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage received modifier for the player controlled character in arena and tournament fights (further modified by difficulty setting). [Native: 100%]",
			Order = 0)]
		[SettingPropertyGroup(
			GroupNameArena,
			GroupOrder = 2)]
		public float ArenaPlayerModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Hero Damage Received",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage received modifier for heroes in arena and tournament fights (further modified by difficulty setting). [Native: 100%]",
			Order = 1)]
		[SettingPropertyGroup(
			GroupNameArena,
			GroupOrder = 2)]
		public float ArenaHeroModifier { get; set; } = 1f;


		[SettingPropertyFloatingInteger(
			"Player Damage Dealt",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage dealt modifier for the player controlled character in arena and tournament fights (further modified by difficulty setting). [Native: 100%]",
			Order = 2)]
		[SettingPropertyGroup(
			GroupNameArena,
			GroupOrder = 2)]
		public float ArenaPlayerAttackerModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Hero Damage Dealt",
			0f,
			10f,
			"0%",
			RequireRestart = false,
			HintText = "Damage dealt modifier for heroes in arena and tournament fights (further modified by difficulty setting). [Native: 100%]",
			Order = 3)]
		[SettingPropertyGroup(
			GroupNameArena,
			GroupOrder = 2)]
		public float ArenaHeroAttackerModifier { get; set; } = 1f;
		#endregion
	}
}