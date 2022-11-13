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

		[SettingPropertyFloatingInteger(
			"Player",
			0f,
			1f,
			"0%",
			RequireRestart = false,
			HintText = "Damage modifier for the player controlled character in any non-arena/tournament fight, relative to difficulty setting. [Native: 100%]",
			Order = 0)]
		[SettingPropertyGroup(
			"Battles etc.",
			GroupOrder = 0)]
		public float BattlePlayerModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Hero",
			0f,
			1f,
			"0%",
			RequireRestart = false,
			HintText = "Damage modifier for heroes in any non-arena/tournament fight, relative to difficulty setting. [Native: 100%]",
			Order = 1)]
		[SettingPropertyGroup(
			"Battles etc.",
			GroupOrder = 0)]
		public float BattleHeroModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Player",
			0f,
			1f,
			"0%",
			RequireRestart = false,
			HintText = "Damage modifier for the player controlled character in arena and tournament fights, relative to difficulty setting. [Native: 100%]",
			Order = 0)]
		[SettingPropertyGroup(
			"Arena & Tournament",
			GroupOrder = 1)]
		public float ArenaPlayerModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Hero",
			0f,
			1f,
			"0%",
			RequireRestart = false,
			HintText = "Damage modifier for heroes in arena and tournament fights, relative to difficulty setting. [Native: 100%]",
			Order = 1)]
		[SettingPropertyGroup(
			"Arena & Tournament",
			GroupOrder = 1)]
		public float ArenaHeroModifier { get; set; } = 1f;
	}
}