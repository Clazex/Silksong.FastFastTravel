using System.Collections.Generic;

using BepInEx.Configuration;

using InControl;

namespace FastFastTravel;

internal static class ConfigEntries {
	internal static class SkipBeastlingCall {
		internal static ConfigEntry<bool> Enabled { get; set; } = null!;
		internal static ConfigEntry<KeyCode> KeyboardBinding { get; set; } = null!;
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
			KeyCode.None,
			new ConfigDescription(
				"Keyboard binding, uses the Down binding when set to \"None\"",
				new AcceptableKeyCodes()
			)
		);
		SkipBeastlingCall.ControllerBinding = config.Bind(
			nameof(SkipBeastlingCall),
			nameof(SkipBeastlingCall.ControllerBinding),
			InputControlType.LeftStickButton,
			"Controller binding"
		);
	}

	internal sealed class AcceptableKeyCodes() : AcceptableValueBase(typeof(KeyCode)) {
		private static readonly Dictionary<KeyCode, Key> mappings = [];
		private static readonly HashSet<KeyCode> validKeyCodes;

		static AcceptableKeyCodes() {
			foreach (UnityKeyboardProvider.KeyMapping mapping in UnityKeyboardProvider.KeyMappings) {
				mappings[mapping.target0] = mapping.source;
				mappings[mapping.target1] = mapping.source; // target1 can be None which we'll handle later
			}

			mappings[KeyCode.None] = Key.None;
			validKeyCodes = [.. mappings.Keys];
		}

		internal static Key ToKey(KeyCode keyCode) => mappings[keyCode];


		public override bool IsValid(object value) =>
			value is KeyCode keyCode && validKeyCodes.Contains(keyCode);
		public override object Clamp(object value) =>
			IsValid(value) ? value : KeyCode.None;
		public override string ToDescriptionString() =>
			"# Acceptable keys: " + string.Join(", ", validKeyCodes);
	}
}
