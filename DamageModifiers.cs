using HarmonyLib;
using MCM.Abstractions.Base.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
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
			try
			{
				base.OnBeforeInitialModuleScreenSetAsRoot();
				if (_isInitialized)
					return;
				_isInitialized = true;

				Settings = GlobalSettings<MCMSettings>.Instance;
				if (Settings == null)
					throw new Exception("Settings is null");
			}
			catch (Exception exc)
			{
				var text = $"ERROR: Damage Modifiers failed to initialize ({nameof(OnBeforeInitialModuleScreenSetAsRoot)}):";
				InformationManager.DisplayMessage(new InformationMessage(text + exc.GetType().ToString(), new Color(1f, 0f, 0f)));
				FileLog.Log(text + "\n" + exc.ToString());
			}
		}

		protected override void OnSubModuleLoad()
		{
			try
			{
				base.OnSubModuleLoad();
				HarmonyPatches.ApplyPatches();
			}
			catch (Exception exc)
			{
				var text = $"ERROR: Damage Modifiers failed to initialize ({nameof(OnSubModuleLoad)}):";
				InformationManager.DisplayMessage(new InformationMessage(text + exc.GetType().ToString(), new Color(1f, 0f, 0f)));
				FileLog.Log(text + "\n" + exc.ToString());
			}
		}
	}
}

