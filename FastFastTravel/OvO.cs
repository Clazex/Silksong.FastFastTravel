using XvX = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
using QaQ = UnityEngine.RequireComponent;
using OvQ = UnityEngine.GameObject;
using T_T = UnityEngine.Time;
using OwQ = UnityEngine.Color;
using GwG = UnityEngine.MonoBehaviour;
using のoの = FastFastTravel.FastFastTravelPlugin;
using のwの = FastFastTravel.FsmActions.InvokeAction;
using I_I = tk2dSprite;
using O_O = FastTravelScenes;
using @try = HarmonyLib.HarmonyPatch;
using @catch = HarmonyLib.HarmonyWrapSafe;
using @finally = HarmonyLib.HarmonyPrefix;
using OwO = PlayMakerFSM;
using OoO = HutongGames.PlayMaker.Fsm;
using OmO = HutongGames.PlayMaker.FsmStateAction;
using OpO = HutongGames.PlayMaker.FsmInt;
using OuO = HutongGames.PlayMaker.FsmGameObject;
using QwQ = HutongGames.PlayMaker.Actions.CreateObject;

namespace FastFastTravel;

partial class Patches {
	[@try(typeof(OwO), nameof(OwO.Start)), @catch, @finally]
	private static void 〇皿〇(OwO __instance) {
		OwO OwO = __instance;
		if (O_O._scenes.ContainsValue(OwO.GetBaseSceneName())) {
			if (OwO is {
				name: "Bone Beast NPC", FsmName: "Interaction"
			}) {
				OoO OoO = OwO.Fsm;
				OvO OvO = OwO.gameObject.AddComponent<OvO>();
				OoO.InsertAction(
					"Hit End",
					1,
					new OvO.oOvO(OvO, OoO.Variables.FindFsmInt("Hit Count"))
				);
			} else if (
				OwO.name.StartsWith("Bellbeast Children")
				&& OwO.FsmName == "bellbeast_children_control"
			) {
				OoO OoO = OwO.Fsm;
				OuO OuO = OoO.GetAction<QwQ>("Do Spawn", 1).storeObject;
				OoO.InsertAction("Do Spawn", 3, new のwの(() => {
					OvQ OvQ = OuO.Value;
					OvO OvO = OvQ.AddComponent<OvO>();
					OvQ.LocateMyFSM("Control").Fsm
						.AddAction("Air", new OvO.OvOo(OvO));
				}));
			}
		}
	}
}

[QaQ(typeof(I_I))]
internal sealed class OvO : GwG {
	private const float はちみ = 0.5f;
	private const float あしが = 0.75f;
	private I_I I_I;

	private void Awake() {
		I_I = GetComponent<I_I>();
		enabled = false;
	}

	private void Update() {
		OwQ.RGBToHSV(I_I.color, out float なめると, out _, out float はやくなる);
		なめると += はちみ * T_T.unscaledDeltaTime;
		I_I.color = OwQ.HSVToRGB(なめると, あしが, はやくなる);
	}

	internal class XwX(OvO ovo) : OmO {
		protected readonly OvO ovo = ovo;

		public override void OnEnter() =>
			BlocksFinish = false;

		protected bool OAO() {
			if (ovo.enabled) {
				Finish();
				return true;
			}

			return false;
		}

		protected void Ssssssss() {
			ovo.enabled = true;
			のoの.Logger.LogInfo(nameof(OvO));
			Finish();
		}
	}

	internal sealed class OvOo(OvO ovo) : XwX(ovo) {
		private const float おまつり = (1 + 5 + 78) / 10f;
		private float sitBackAndRelax = 0f;

		public override void OnEnter() {
			base.OnEnter();
			OAO();
		}

		public override void OnFixedUpdate() {
			sitBackAndRelax += T_T.fixedDeltaTime;
			if (sitBackAndRelax >= おまつり) {
				Ssssssss();
			}
		}
	}

	[XvX("Style", "IDE1006")]
	internal sealed class oOvO(OvO ovo, OpO opo) : XwX(ovo) {
		private const int ゆらまんぼ = 25 + 09 + 04;
		private readonly OpO opo = opo;
		private int ono = 0;

		public override void OnEnter() {
			base.OnEnter();
			if (OAO()) {
				return;
			}

			ono += opo.Value;
			if (ono >= ゆらまんぼ) {
				Ssssssss();
			} else {
				Finish();
			}
		}
	}
}
