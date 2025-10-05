using InControl;

namespace FastFastTravel.FsmActions;

internal sealed class ListenForSkipBeastlingCall(FsmEvent skipEvent) : FsmStateAction {
	private static ActionSet actionSet = new();

	private static void UpdateActionSet() => actionSet = new();

	static ListenForSkipBeastlingCall() {
		ConfigEntries.SkipBeastlingCall.KeyboardBinding.SettingChanged += (_, _) => UpdateActionSet();
		ConfigEntries.SkipBeastlingCall.ControllerBinding.SettingChanged += (_, _) => UpdateActionSet();
	}

	public FsmEvent skipEvent = skipEvent;

	public override void OnEnter() =>
		BlocksFinish = false;

	public override void OnUpdate() {
		if (ConfigEntries.SkipBeastlingCall.Enabled.Value && actionSet.IsPressed) {
			Plugin.Logger.LogDebug("Beastling call skip triggered");
			Fsm.Event(skipEvent);
			Finish();
		}
	}

	private sealed class ActionSet : PlayerActionSet {
		internal readonly PlayerAction keyboard;
		internal readonly PlayerAction controller;

		internal bool IsPressed => keyboard.IsPressed || controller.IsPressed;

		internal ActionSet() {
			Plugin.Logger.LogDebug("Creating skip beastling call action set");

			if (ConfigEntries.SkipBeastlingCall.KeyboardBinding.Value is Key key && key != Key.None) {
				keyboard = new("Keyboard Skip Beastling Call", this);
				keyboard.AddDefaultBinding(key);
			} else {
				keyboard = InputHandler.Instance.inputActions.Down;
			}

			controller = new("Controller Skip Beastling Call", this);
			controller.AddDefaultBinding(ConfigEntries.SkipBeastlingCall.ControllerBinding.Value);
		}
	}
}
