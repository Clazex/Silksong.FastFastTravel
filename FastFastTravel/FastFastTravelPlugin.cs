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

	private static Harmony Harmony { get; } = new(Id);

	private void Awake() {
		Instance = this;
		Logger = base.Logger;

		ConfigEntries.Bind(Config);
		Config.ConfigReloaded += (_, _) => ConfigEntries.Bind(Config);
		Harmony.PatchAll(typeof(Patches));

		Logger.LogInfo($"Plugin {Name} ({Id}) v{Version} has loaded!");
	}

	private void OnDestroy() {
#if !DEBUG
		Logger.LogWarning("Unload called in release build");
#endif
		Harmony.UnpatchSelf();
		Logger.LogInfo($"Plugin {Name} has unloaded!");
	}
}
