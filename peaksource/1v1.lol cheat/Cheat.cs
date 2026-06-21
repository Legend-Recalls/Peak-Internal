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
			if ((Object)(object)instance != (Object)null && (Object)(object)instance != (Object)(object)this)
			{
				Debug.Log((object)"[Cheat] Destroying old cheat instance");
				try
				{
					if ((Object)(object)((Component)instance).gameObject != (Object)null)
					{
						Object.DestroyImmediate((Object)(object)((Component)instance).gameObject);
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
					TrollFeatures.Initialize((MonoBehaviour)(object)this);
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
					AutoPathfinder.Initialize((MonoBehaviour)(object)this);
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
			if (CheatConfig.ShowWatermark)
			{
				ESP.DrawWatermark();
			}
			if (CheatConfig.PlayerESP || CheatConfig.EntityESP || CheatConfig.ItemESP || CheatConfig.LuggageESP || CheatConfig.SporeShroomESP || CheatConfig.EnvironmentalESP || CheatConfig.ObjectNameESP)
			{
				ESP.RenderAll();
				ESP.RenderChams();
			}
			if (CheatConfig.ClimbingPredictionEnabled)
			{
				ClimbingPrediction.Draw();
			}
			if (CheatConfig.AutoPathfinderEnabled)
			{
				AutoPathfinder.DrawPath();
			}
			if (EntityControl.IsControllingEntity())
			{
				ESP.DrawEntityControlKeybinds();
			}
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
				Cursor.lockState = (CursorLockMode)0;
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
				EntityControl.ProcessInput((MonoBehaviour)(object)this, toggled);
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
				Cursor.lockState = (CursorLockMode)0;
			}
			if (EntityControl.IsControllingEntity())
			{
				EntityControl.UpdateCamera();
			}
			if (EntityControl.IsControllingEntity() && !toggled)
			{
				GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
				if ((Object)(object)currentlyControlledEntity != (Object)null)
				{
					Character component = currentlyControlledEntity.GetComponent<Character>();
					if ((Object)(object)component != (Object)null)
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
			if (CheatConfig.ClimbingPredictionEnabled)
			{
				ClimbingPrediction.Update();
			}
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
			if (CheatConfig.SpoofEnabled)
			{
				SteamSpoofing.Update();
			}
			VersionBypass.Update();
			CheaterDetection.Update();
			if (!toggled)
			{
				GravityGun();
			}
		}

		private void DisableGameInput()
		{
			if ((Object)(object)Character.localCharacter != (Object)null && (Object)(object)Character.localCharacter.input != (Object)null)
			{
				Character.localCharacter.input.itemSwitchBlocked = true;
			}
		}

		private void EnableGameInput()
		{
			if ((Object)(object)Character.localCharacter != (Object)null && (Object)(object)Character.localCharacter.input != (Object)null)
			{
				Character.localCharacter.input.itemSwitchBlocked = false;
			}
		}

		public static void TeleportToCoords(float x, float y, float z)
		{
			try
			{
				Character localCharacter = Character.localCharacter;
				if (!((Object)(object)localCharacter == (Object)null) && !localCharacter.data.dead)
				{
					PhotonView photonView = ((MonoBehaviourPun)localCharacter).photonView;
					if (!((Object)(object)photonView == (Object)null))
					{
						Vector3 val = default(Vector3);
						((Vector3)(ref val))..ctor(x, y, z);
						photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2] { val, true });
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
			if ((Object)(object)CheatConfig.CurrentlyControlledEntity != (Object)null)
			{
				EntityControl.DisableControl(CheatConfig.CurrentlyControlledEntity);
			}
			Loader.Unload();
		}

		public static void RefreshPlayerDict()
		{
			CheatConfig.PlayerDict = new Dictionary<string, Character>();
			Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
			foreach (Character val in array)
			{
				if ((Object)(object)val.player != (Object)null && (Object)(object)((MonoBehaviourPun)val.player).photonView != (Object)null)
				{
					string nickName = ((MonoBehaviourPun)val.player).photonView.Owner.NickName;
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
			string name = ((Scene)(ref activeScene)).name;
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
												if (val != null && (Object)(object)val != (Object)null)
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
									if (val2 != null && (Object)(object)val2 != (Object)null)
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
						if ((Object)(object)val3 != (Object)null && (Object)(object)val3.GetComponent<Item>() != (Object)null)
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
								if ((Object)(object)item3.Value != (Object)null && (Object)(object)item3.Value.GetComponent<Item>() != (Object)null)
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

		private static float _gravityGunRpcCooldown = 0f;
		private const float GRAVITY_GUN_RPC_INTERVAL = 0.05f;

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
				if (Physics.Raycast(new Ray(((Component)Camera.main).transform.position, ((Component)Camera.main).transform.forward), ref val2, laserRange))
				{
					Item componentInParent = ((Component)((RaycastHit)(ref val2)).collider).GetComponentInParent<Item>();
					if ((Object)(object)componentInParent != (Object)null && (Object)(object)((MonoBehaviourPun)componentInParent).photonView != (Object)null)
					{
						GrabbedItem = componentInParent;
						gravityGunDistance = 6f;
						((MonoBehaviourPun)componentInParent).photonView.RPC("SetKinematicRPC", (RpcTarget)0, new object[3]
						{
							true,
							((Component)Camera.main).transform.position + ((Component)Camera.main).transform.forward * gravityGunDistance,
							Quaternion.identity
						});
						_gravityGunRpcCooldown = Time.time + GRAVITY_GUN_RPC_INTERVAL;
					}
				}
				if ((Object)(object)GrabbedItem != (Object)null && (Object)(object)((MonoBehaviourPun)GrabbedItem).photonView != (Object)null && Time.time > _gravityGunRpcCooldown)
				{
					((MonoBehaviourPun)GrabbedItem).photonView.RPC("SetKinematicRPC", (RpcTarget)0, new object[3]
					{
						true,
						((Component)Camera.main).transform.position + ((Component)Camera.main).transform.forward * gravityGunDistance,
						Quaternion.identity
					});
					_gravityGunRpcCooldown = Time.time + GRAVITY_GUN_RPC_INTERVAL;
				}
			}
			else if ((Object)(object)GrabbedItem != (Object)null && (Object)(object)((MonoBehaviourPun)GrabbedItem).photonView != (Object)null)
			{
				((MonoBehaviourPun)GrabbedItem).photonView.RPC("SetKinematicRPC", (RpcTarget)0, new object[3]
				{
					false,
					((Component)GrabbedItem).transform.position,
					((Component)GrabbedItem).transform.rotation
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
}
