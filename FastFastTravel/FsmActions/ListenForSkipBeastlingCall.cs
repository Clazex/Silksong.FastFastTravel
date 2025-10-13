using InControl;

namespace FastFastTravel.FsmActions;

internal sealed class ListenForSkipBeastlingCall : FsmStateAction {
	public FsmEvent skipEvent;

	public ListenForSkipBeastlingCall(FsmEvent skipEvent) {
		BlocksFinish = false;
		this.skipEvent = skipEvent;
	}

	public override void OnUpdate() {
		if (ConfigEntries.SkipBeastlingCall.Enabled.Value && ActionSet.Instance.IsPressed) {
			Plugin.Logger.LogDebug("Beastling call skip triggered");
			Fsm.Event(skipEvent);
			Finish();
		}
	}

	private sealed class ActionSet : PlayerActionSet {
		internal static ActionSet Instance { get; private set; } = new();

		static ActionSet() => Plugin.ConfigChanged += () => Instance = new();


		internal readonly PlayerAction keyboard;
		internal readonly PlayerAction controller;

		internal bool IsPressed => keyboard.IsPressed || controller.IsPressed;

		private ActionSet() {
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
