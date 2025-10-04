namespace FastFastTravel.FsmActions;

internal sealed class InvokeAction(Action action): FsmStateAction {
	public Action action = action;

	public override void OnEnter() {
		action.Invoke();
		Finish();
	}
}
