using HarmonyLib;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace DamageModifiers
{
	public class DamageModifiers : MBSubModuleBase
	{
		public static MCMSettings Settings { get; private set; }

		private bool _isInitialized = false;

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			base.OnBeforeInitialModuleScreenSetAsRoot();
			if (_isInitialized)
				return;

			Settings = GlobalSettings<MCMSettings>.Instance;
			if (Settings == null)
				throw new Exception("Settings is null");

			InformationManager.DisplayMessage(new InformationMessage("Damage Modifiers initialized"));

			_isInitialized = true;
		}

		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			var harmony = new Harmony("sy.damagemodifiers");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}

