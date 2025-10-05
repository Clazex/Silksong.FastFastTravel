using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace FastFastTravel;

[BepInAutoPlugin(id: "dev.clazex.fastfasttravel")]
public partial class FastFastTravelPlugin : BaseUnityPlugin {
	public static FastFastTravelPlugin Instance {
		get => field != null
			? field
			: throw new InvalidOperationException("instance not present");
		private set;
	}

	internal static new ManualLogSource Logger {
		get => field ?? throw new InvalidOperationException("instance not present");
		private set;
	}

	private void Awake() {
		Instance = this;
		Logger = base.Logger;
		Harmony.CreateAndPatchAll(typeof(FastFastTravelPlugin));
		Logger.LogInfo($"Plugin {Name} ({Id}) v{Version} has loaded!");
	}
}
