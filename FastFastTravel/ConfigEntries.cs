using BepInEx.Configuration;

using InControl;

namespace FastFastTravel;

internal static class ConfigEntries {
	internal static class SkipBeastlingCall {
		internal static ConfigEntry<bool> Enabled { get; set; } = null!;
		internal static ConfigEntry<Key> KeyboardBinding { get; set; } = null!;
		internal static ConfigEntry<InputControlType> ControllerBinding { get; set; } = null!;
	}

	internal static void Bind(ConfigFile config) {
		SkipBeastlingCall.Enabled = config.Bind(
			nameof(SkipBeastlingCall),
			nameof(SkipBeastlingCall.Enabled),
			true,
			"Whether to enable skipping Beastling Call performance"
		);
		SkipBeastlingCall.KeyboardBinding = config.Bind(
			nameof(SkipBeastlingCall),
			nameof(SkipBeastlingCall.KeyboardBinding),
			Key.None,
			"Keyboard binding, uses the Down binding when set to \"None\""
		);
		SkipBeastlingCall.ControllerBinding = config.Bind(
			nameof(SkipBeastlingCall),
			nameof(SkipBeastlingCall.ControllerBinding),
			InputControlType.LeftStickButton,
			"Controller binding"
		);
	}
}
