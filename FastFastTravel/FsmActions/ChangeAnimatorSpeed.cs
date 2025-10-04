namespace FastFastTravel.FsmActions;

internal sealed class ChangeAnimatorSpeed(float speed) : FsmStateAction {
	public float speed = speed;

	public override void OnEnter() {
		fsmComponent.GetComponent<Animator>().speed = speed;
		Finish();
	}
}
