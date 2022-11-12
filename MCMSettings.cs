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



		#region SMITHING PART RESEARCH MODIFIERS
		[SettingPropertyFloatingInteger(
			"Part Research Modifier",
			0.01f,
			10.0f,
			"0%",
			RequireRestart = false,
			HintText = "Adjust smithing part research gain rate for smithing and smelting weapons. [Native: 100%]",
			Order = 0)]
		[SettingPropertyGroup(
			"Smithing Research",
			GroupOrder = 1)]
		public float SmithingResearchModifier { get; set; } = 1f;

		[SettingPropertyFloatingInteger(
			"Free Build Part Research Modifier",
			0.01f,
			1.0f,
			"0%",
			RequireRestart = false,
			HintText = "Adjust smithing part research gain rate when in free build mode. With the default setting, unlocking parts is slow in free build mode. [Native: 10%]",
			Order = 1)]
		[SettingPropertyGroup(
			"Smithing Research",
			GroupOrder = 1)]
		public float SmithingFreeBuildResearchModifier { get; set; } = 0.1f;
		#endregion
	}
}