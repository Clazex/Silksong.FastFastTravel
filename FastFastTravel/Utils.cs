namespace FastFastTravel;

internal static class Utils {
	public static string GetBaseSceneName<T>(this T component) where T : Component =>
		GameManager.InternalBaseSceneName(component.gameObject.scene.name);

	public static FsmState AddState(this Fsm fsm, string stateName, params FsmStateAction[] actions) {
		FsmState state = new(fsm) {
			Name = stateName,
			Actions = actions
		};
		fsm.States = [..fsm.States, state];
		return state;
	}

	public static T GetAction<T>(this Fsm fsm, string stateName, int index) where T : FsmStateAction =>
		(T) fsm.GetState(stateName).Actions[index];

	public static void DisableAction(this Fsm fsm, string stateName, int index) =>
		fsm.GetState(stateName).Actions[index].Enabled = false;

	public static void AddAction(this Fsm fsm, string stateName, FsmStateAction action) {
		FsmState state = fsm.GetState(stateName);
		state.Actions = [..state.Actions, action];
	}

	public static void ReplaceAction(this Fsm fsm, string stateName, int index, FsmStateAction action) =>
		fsm.GetState(stateName).Actions[index] = action;

	public static void InsertAction(this Fsm fsm, string stateName, int index, FsmStateAction action) {
		FsmState state = fsm.GetState(stateName);
		state.Actions = [..state.Actions[0..index], action, ..state.Actions[index..]];
	}

	public static void ChangeTransition(this Fsm fsm, string stateName, string eventName, string toStateName) {
		FsmTransition transition = fsm.GetState(stateName)
			.Transitions
			.First(i => i.EventName == eventName);
		transition.ToFsmState = fsm.GetState(toStateName);
		transition.ToState = toStateName;
	}

	public static void AddTransition(this Fsm fsm, string stateName, FsmEvent fsmEvent, string toStateName) {
		FsmState state = fsm.GetState(stateName);
		state.Transitions = [..state.Transitions, new() {
			FsmEvent = fsmEvent,
			ToFsmState = fsm.GetState(toStateName),
			ToState = toStateName
		}];
	}
}
