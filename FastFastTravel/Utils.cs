namespace FastFastTravel;

internal static class Utils {
	internal static string GetBaseSceneName<T>(this T component) where T : Component =>
		GameManager.InternalBaseSceneName(component.gameObject.scene.name);

	internal static T GetAction<T>(this Fsm fsm, string stateName, int index) where T : FsmStateAction =>
		(T) fsm.GetState(stateName).Actions[index];

	internal static void DisableAction(this Fsm fsm, string stateName, int index) =>
		fsm.GetState(stateName).Actions[index].Enabled = false;

	internal static void DisableActions(this Fsm fsm, string stateName, params int[] indices) {
		FsmStateAction[] actions = fsm.GetState(stateName).Actions;
		foreach (int index in indices) {
			actions[index].Enabled = false;
		}
	}

	internal static void AddAction(this Fsm fsm, string stateName, FsmStateAction action) {
		FsmState state = fsm.GetState(stateName);
		state.Actions = [..state.Actions, action];
	}

	internal static void ReplaceAction(this Fsm fsm, string stateName, int index, FsmStateAction action) =>
		fsm.GetState(stateName).Actions[index] = action;

	internal static void InsertAction(this Fsm fsm, string stateName, int index, FsmStateAction action) {
		FsmState state = fsm.GetState(stateName);
		state.Actions = [..state.Actions[0..index], action, ..state.Actions[index..]];
	}

	internal static void ChangeTransition(this Fsm fsm, string stateName, string eventName, string toStateName) {
		FsmTransition transition = fsm.GetState(stateName)
			.Transitions
			.First(i => i.EventName == eventName);
		transition.ToFsmState = fsm.GetState(toStateName);
		transition.ToState = toStateName;
	}

	internal static void AddTransition(this Fsm fsm, string stateName, FsmEvent fsmEvent, string toStateName) {
		FsmState state = fsm.GetState(stateName);
		state.Transitions = [..state.Transitions, new() {
			FsmEvent = fsmEvent,
			ToFsmState = fsm.GetState(toStateName),
			ToState = toStateName
		}];
	}
}
