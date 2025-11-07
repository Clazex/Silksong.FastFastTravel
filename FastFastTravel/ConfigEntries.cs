using System.Collections.Generic;

using BepInEx.Configuration;

using InControl;

using UnityKey = UnityEngine.InputSystem.Key;

namespace FastFastTravel;

internal static class ConfigEntries {
	internal static class SkipBeastlingCall {
		internal static ConfigEntry<bool> Enabled { get; set; } = null!;
		internal static ConfigEntry<UnityKey> KeyboardBinding { get; set; } = null!;
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
			UnityKey.None,
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
		private static readonly Dictionary<UnityKey, Key> mappings = [];
		private static readonly HashSet<UnityKey> validKeyCodes;

		static AcceptableKeyCodes() {
			foreach (UnityKeyboardProvider.KeyMapping mapping in UnityKeyboardProvider.KeyMappings) {
				mappings[mapping.target0] = mapping.source;
				mappings[mapping.target1] = mapping.source; // target1 can be None which we'll handle later
			}

			mappings[UnityKey.None] = Key.None;
			validKeyCodes = [.. mappings.Keys];
		}

		internal static Key ToKey(UnityKey keyCode) => mappings[keyCode];


		public override bool IsValid(object value) =>
			value is UnityKey keyCode && validKeyCodes.Contains(keyCode);
		public override object Clamp(object value) =>
			IsValid(value) ? value : UnityKey.None;
		public override string ToDescriptionString() =>
			"# Acceptable keys: " + string.Join(", ", validKeyCodes);
	}
}
