using FastFastTravel.FsmActions;

using GlobalEnums;

using HarmonyLib;

using HutongGames.PlayMaker.Actions;

using UObject = UnityEngine.Object;

namespace FastFastTravel;

partial class FastFastTravelPlugin {
	[HarmonyPatch(typeof(PlayMakerFSM), nameof(PlayMakerFSM.Start))]
	[HarmonyWrapSafe]
	[HarmonyPostfix]
	private static void ModifyFsm(PlayMakerFSM __instance) {
		if (FastTravelScenes._scenes.ContainsValue(
			__instance.GetBaseSceneName()
		)) {
			if (__instance is {
				name: "Bone Beast NPC",
				FsmName: "Interaction"
			}) {
				ModifyBellBeastFsm(__instance.Fsm);
				return;
			} else if (__instance is {
				name: "Bellway Toll Machine",
				FsmName: "Unlock Behaviour"
			}) {
				ModifyBellwayTollFsm(__instance.Fsm);
				return;
			}
		}

		if (FastTravelScenes._tubeScenes.ContainsValue(
			__instance.GetBaseSceneName()
		)) {
			if (__instance is {
				name: "City Travel Tube",
				FsmName: "Tube Travel"
			}) {
				ModifyTubeFsm(__instance.Fsm);
				return;
			} else if (__instance is {
				name: "tube_toll_machine",
				FsmName: "Unlock Behaviour"
			}) {
				ModifyTubeTollFsm(__instance.Fsm);
				return;
			}
		}

		if (__instance.gameObject.scene.name == nameof(UObject.DontDestroyOnLoad)) {
			if (__instance is {
				name: "Hero_Hornet(Clone)",
				FsmName: "Silk Specials"
			}) {
				ModifySilkSpecialsFsm(__instance.Fsm);
				return;
			} else if (__instance is {
				name: "Bone Beast Children Teleport(Clone)",
				FsmName: "bellbeast_child_teleport_arrive"
			}) {
				ModifySummonedChildrenFsm(__instance.Fsm);
				return;
			}
		}
	}

	#region Bellway

	private static void ModifyBellBeastFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Bell Beast FSM");

		// Standby everywhere
		EnumCompare actionCompareLocation = fsm.GetAction<EnumCompare>("Is Already Present?", 1);
		actionCompareLocation.notEqualEvent = actionCompareLocation.equalEvent;

		// Update current location when entering range
		fsm.InsertAction("First Enter?", 0, new InvokeAction(() =>
			PlayerData.instance.FastTravelNPCLocation = (FastTravelLocations) actionCompareLocation.compareTo.Value
		));

		// Always awake
		fsm.ChangeTransition("Start State", "SLEEP", "Wake Up");

		// Fast arrival
		fsm.DisableAction("Travel Arrive Start", 7);

		// HUD Fix
		fsm.AddAction("Wait Finished Entering", new InvokeAction(() => {
			if (HudCanvas.IsVisible) {
				return;
			}

			// Why TC
			HudCanvas canvas = HudCanvas.instance;
			canvas.targetFsm.SendEvent("IN");
			FSMUtility.SendEventToGameObject(canvas.gameObject, "INVENTORY OPEN COMPLETE", true);
		}));

		// Fast departure
		fsm.GetAction<ScreenFader>("Hero Jump", 0).duration = 0.25f;
		fsm.DisableAction("Hero Jump", 5);
		fsm.ChangeTransition("Hero Jump", FsmEvent.Finished.Name, "Time Passes");

		// Skip cutscene
		fsm.DisableAction("Choose Scene", 3); // Don't preload
		fsm.ReplaceAction("Go To Stag Cutscene", 7, new BeginSceneTransition() {
			sceneName = fsm.Variables.GetFsmString("To Scene"),
			entryGateName = "door_fastTravelExit",
			entryDelay = 0f,
			visualization = GameManager.SceneLoadVisualizations.Default,
			preventCameraFadeOut = true
		});

		// Don't sing on first entrance in range
		fsm.ChangeTransition("First Enter?", FsmEvent.Finished.Name, "Idle");

		// Auto call after unlock
		fsm.ChangeTransition("Can Appear 2", "TRUE", "Appear Delay");

		// Fast call
		fsm.DisableAction("Appear Delay", 5);
		fsm.DisableAction("Start Shake", 8);
	}

	private static void ModifyBellwayTollFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Bellway Toll FSM");

		// Fast strum
		fsm.DisableAction("Return Control", 5);
		fsm.AddAction("Return Control", new SetAnimator() {
			target = new(),
			active = true
		});
		fsm.AddAction("Return Control", new ChangeAnimatorSpeed(20f));
		fsm.DisableAction("Sequence Strum", 0);
		fsm.DisableAction("Stop", 1);

		// Fast floor open
		fsm.DisableAction("Open Floor", 3);
		fsm.DisableAction("Open Floor", 5);
		fsm.GetAction<CallMethodProper>("Open Floor", 0)
			.gameObject.GameObject.Value
			.GetComponent<Animator>().speed = 10f;
	}

	#endregion

	#region Ventrica

	private static void ModifyTubeFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Tube FSM");

		// Fast arrival
		fsm.DisableAction("Tube Start Away", 3);
		fsm.GetAction<SendEventByName>("Tube Start Away", 4)
			.sendEvent = "START OPEN";
		fsm.ChangeTransition("Start In Tube", FsmEvent.Finished.Name, "Break Items");
		fsm.AddTransition("Break Items", FsmEvent.Finished, "Open");
		fsm.DisableAction("Open", 3);
		fsm.AddTransition("Open", FsmEvent.Finished, "Hop Out Antic");

		// Fast departure
		fsm.ChangeTransition("Preload Scene", FsmEvent.Finished.Name, "Close");
		fsm.AddTransition("Close", FsmEvent.Finished, "Save State");
		fsm.GetAction<ScreenFader>("Fade Out", 2).duration = 0.25f;
		fsm.GetAction<Wait>("Fade Out", 3).time = 0.25f;

		// Fast unlock arrival
		fsm.GetAction<SendEventByName>("Unlock Open", 1).sendEvent = "START OPEN";
		fsm.AddTransition("Unlock Open", FsmEvent.Finished, "Unlock");
	}

	private static void ModifyTubeTollFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Tube Toll FSM");

		// Fast unlock
		fsm.DisableAction("Retract Animation", 0);
		fsm.AddAction("Retract Animation", new ChangeAnimatorSpeed(100f));
		fsm.DisableAction("After Retract Pause", 1);
	}

	#endregion

	#region Beastling Call

	private static void ModifySilkSpecialsFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Hornet Silk Specials FSM");

		fsm.DisableActions("Hornet Jump Antic", 1, 4, 5);
		fsm.DisableActions("Hornet Jump", 1, 2, 3, 4, 5, 6, 7, 8);
		fsm.AddTransition("Hornet Jump", FsmEvent.Finished, "Hornet Fall");
		fsm.DisableActions("Hornet Fall", 0, 3, 5, 6, 7);
		fsm.AddTransition("Hornet Fall", FsmEvent.Finished, "Children Leave Fade");

		fsm.GetAction<ScreenFader>("Children Leave Fade", 6).duration = 0.25f;
		fsm.GetAction<Wait>("Children Leave Fade", 7).time = 0.25f;

		ModifyNeedolinFsm(fsm.GetAction<RunFSM>("Needolin Sub", 2).fsmTemplateControl.RunFsm);
	}

	private static void ModifyNeedolinFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Hornet Needolin SubFSM");

		fsm.GetAction<BoolTestDelay>("Needolin FT Wait", 4).delay = 0f;
		fsm.GetAction<Wait>("Can Fast Travel?", 1).time = 0f;

		fsm.GetAction<Wait>("Needolin FT Antic", 5).time = 2f; // 3f -> 2f
		fsm.AddAction("Needolin FT Antic", new ListenForSkipBeastlingCall(FsmEvent.Finished));
	}

	private static void ModifySummonedChildrenFsm(Fsm fsm) {
		Logger.LogDebug("Modifying Summoned Bell Beast Childern FSM");

		fsm.AddAction("Init", new InvokeAction(() => {
			// Somehow TC decided that Play(clip, 0f) does not reset clip time is clip
			// is current clip, so do reset them here.
			MirrorTk2dAnimDelayed mirrorer = fsm.FsmComponent.GetComponent<MirrorTk2dAnimDelayed>();
			mirrorer.mirrorAnimator.PlayFromFrame(0);
			mirrorer.animator.PlayFromFrame(0);
			mirrorer.animator.Stop(); // This will be started by mirrorer later
		}));
	}

	#endregion
}
