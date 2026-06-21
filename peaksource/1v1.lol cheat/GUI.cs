using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public class GUI : MonoBehaviour
	{
		[CompilerGenerated]
		private sealed class <InitializeTestDummy>d__232 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public GameObject dummy;

			public Vector3 spawnPos;

			private Character <charComponent>5__2;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <InitializeTestDummy>d__232(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
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
							if ((Object)(object)val == (Object)null)
							{
								val = dummy.GetComponentInChildren<PhotonView>();
							}
							if ((Object)(object)val != (Object)null && !val.IsMine && PhotonNetwork.IsMasterClient)
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

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		private sealed class <ResetJumpInput>d__215 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character character;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <ResetJumpInput>d__215(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
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
					if ((Object)(object)character != (Object)null && (Object)(object)character.input != (Object)null)
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

			[DebuggerHidden]
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
			get { return CheatConfig.Godmode; }
			set { CheatConfig.Godmode = value; }
		}

		public static bool clearStatuses
		{
			get { return CheatConfig.ClearStatuses; }
			set { CheatConfig.ClearStatuses = value; }
		}

		public static bool infiniteammo
		{
			get { return CheatConfig.InfiniteAmmo; }
			set { CheatConfig.InfiniteAmmo = value; }
		}

		public static bool randomoutfits
		{
			get { return CheatConfig.RandomOutfits; }
			set { CheatConfig.RandomOutfits = value; }
		}

		public static bool setfieldofview
		{
			get { return CheatConfig.SetFieldOfView; }
			set { CheatConfig.SetFieldOfView = value; }
		}

		public static bool rapidfire
		{
			get { return CheatConfig.RapidFire; }
			set { CheatConfig.RapidFire = value; }
		}

		public static bool Unlockall
		{
			get { return CheatConfig.UnlockAll; }
			set { CheatConfig.UnlockAll = value; }
		}

		public static bool BingbongSpam
		{
			get { return CheatConfig.BingBongSpam; }
			set { CheatConfig.BingBongSpam = value; }
		}

		public static int rapidcooldown
		{
			get { return CheatConfig.RapidCooldown; }
			set { CheatConfig.RapidCooldown = value; }
		}

		public static float fireratecooldown
		{
			get { return CheatConfig.FireRateCooldown; }
			set { CheatConfig.FireRateCooldown = value; }
		}

		public static float fieldofview
		{
			get { return CheatConfig.FieldOfView; }
			set { CheatConfig.FieldOfView = value; }
		}

		public static bool crasher
		{
			get { return CheatConfig.Crasher; }
			set { CheatConfig.Crasher = value; }
		}

		public static bool boxesp
		{
			get { return CheatConfig.BoxESP; }
			set { CheatConfig.BoxESP = value; }
		}

		public static bool boxfix
		{
			get { return CheatConfig.BoxFix; }
			set { CheatConfig.BoxFix = value; }
		}

		public static bool skeletonESP
		{
			get { return CheatConfig.SkeletonESP; }
			set { CheatConfig.SkeletonESP = value; }
		}

		public static bool nameESP
		{
			get { return CheatConfig.NameESP; }
			set { CheatConfig.NameESP = value; }
		}

		public static bool entityBoxESP
		{
			get { return CheatConfig.EntityBoxESP; }
			set { CheatConfig.EntityBoxESP = value; }
		}

		public static bool entityNameESP
		{
			get { return CheatConfig.EntityNameESP; }
			set { CheatConfig.EntityNameESP = value; }
		}

		public static bool entitySkeletonESP
		{
			get { return CheatConfig.EntitySkeletonESP; }
			set { CheatConfig.EntitySkeletonESP = value; }
		}

		public static bool entityAIStateESP
		{
			get { return CheatConfig.EntityAIStateESP; }
			set { CheatConfig.EntityAIStateESP = value; }
		}

		public static bool speed
		{
			get { return CheatConfig.Speed; }
			set { CheatConfig.Speed = value; }
		}

		public static float speedmultiply
		{
			get { return CheatConfig.SpeedMultiply; }
			set { CheatConfig.SpeedMultiply = value; }
		}

		public static bool flyMode
		{
			get { return CheatConfig.FlyMode; }
			set { CheatConfig.FlyMode = value; }
		}

		public static bool noClip
		{
			get { return CheatConfig.NoClip; }
			set { CheatConfig.NoClip = value; }
		}

		public static bool noFallDamage
		{
			get { return CheatConfig.NoFallDamage; }
			set { CheatConfig.NoFallDamage = value; }
		}

		public static bool superJump
		{
			get { return CheatConfig.SuperJump; }
			set { CheatConfig.SuperJump = value; }
		}

		public static float jumpMultiplier
		{
			get { return CheatConfig.JumpMultiplier; }
			set { CheatConfig.JumpMultiplier = value; }
		}

		public static float climbingSpeedMultiplier
		{
			get { return CheatConfig.ClimbingSpeedMultiplier; }
			set { CheatConfig.ClimbingSpeedMultiplier = value; }
		}

		public static float fallDamagePercent
		{
			get { return CheatConfig.FallDamagePercent; }
			set { CheatConfig.FallDamagePercent = value; }
		}

		public static bool reduceStaminaConsumption
		{
			get { return CheatConfig.ReduceStaminaConsumption; }
			set { CheatConfig.ReduceStaminaConsumption = value; }
		}

		public static float staminaConsumptionPercent
		{
			get { return CheatConfig.StaminaConsumptionPercent; }
			set { CheatConfig.StaminaConsumptionPercent = value; }
		}

		public static string itemFilterText
		{
			get { return CheatConfig.ItemFilterText; }
			set { CheatConfig.ItemFilterText = value; }
		}

		public static bool itemDropdownOpen
		{
			get { return CheatConfig.ItemDropdownOpen; }
			set { CheatConfig.ItemDropdownOpen = value; }
		}

		public static Vector2 itemDropdownScrollPos
		{
			get { return CheatConfig.ItemDropdownScrollPos; }
			set { CheatConfig.ItemDropdownScrollPos = value; }
		}

		public static string entityFilterText
		{
			get { return CheatConfig.EntityFilterText; }
			set { CheatConfig.EntityFilterText = value; }
		}

		public static bool entityDropdownOpen
		{
			get { return CheatConfig.EntityDropdownOpen; }
			set { CheatConfig.EntityDropdownOpen = value; }
		}

		public static Vector2 entityDropdownScrollPos
		{
			get { return CheatConfig.EntityDropdownScrollPos; }
			set { CheatConfig.EntityDropdownScrollPos = value; }
		}

		public static Vector2 entityManagerScrollPos
		{
			get { return CheatConfig.EntityManagerScrollPos; }
			set { CheatConfig.EntityManagerScrollPos = value; }
		}

		public static int selectedEntityIndex
		{
			get { return CheatConfig.SelectedEntityIndex; }
			set { CheatConfig.SelectedEntityIndex = value; }
		}

		public static GameObject currentlyControlledEntity
		{
			get { return CheatConfig.CurrentlyControlledEntity; }
			set { CheatConfig.CurrentlyControlledEntity = value; }
		}

		public static float entityControlSpeed
		{
			get { return CheatConfig.EntityControlSpeed; }
			set { CheatConfig.EntityControlSpeed = value; }
		}

		public static float entityFollowDistance
		{
			get { return CheatConfig.EntityFollowDistance; }
			set { CheatConfig.EntityFollowDistance = value; }
		}

		private static string[] filteredItems
		{
			get { return CheatConfig.FilteredItems; }
			set { CheatConfig.FilteredItems = value; }
		}

		private static string[] filteredEntities
		{
			get { return CheatConfig.FilteredEntities; }
			set { CheatConfig.FilteredEntities = value; }
		}

		private static string lastSpawnMessage
		{
			get { return CheatConfig.LastSpawnMessage; }
			set { CheatConfig.LastSpawnMessage = value; }
		}

		private static float lastSpawnMessageTime
		{
			get { return CheatConfig.LastSpawnMessageTime; }
			set { CheatConfig.LastSpawnMessageTime = value; }
		}

		private static string[] availableEntities
		{
			get { return CheatConfig.AvailableEntities; }
			set { CheatConfig.AvailableEntities = value; }
		}

		private static bool entitiesInitialized
		{
			get { return CheatConfig.EntitiesInitialized; }
			set { CheatConfig.EntitiesInitialized = value; }
		}

		public static List<GameObject> spawnedEntities => CheatConfig.SpawnedEntities;

		public static Dictionary<string, Character> playerDict
		{
			get { return CheatConfig.PlayerDict; }
			set { CheatConfig.PlayerDict = value; }
		}

		public static string localPlayerName
		{
			get { return CheatConfig.LocalPlayerName; }
			set { CheatConfig.LocalPlayerName = value; }
		}

		public static string[] items
		{
			get { return CheatConfig.Items; }
			set { CheatConfig.Items = value; }
		}

		private static float GetScrollViewHeight()
		{
			return Mathf.Max(((Rect)(ref GUIRect)).height - 140f, 500f);
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
			if (!((Object)(object)Character.localCharacter == (Object)null) && !((Object)(object)((MonoBehaviourPun)Character.localCharacter).photonView == (Object)null))
			{
				((MonoBehaviourPun)Character.localCharacter).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2] { position, poof });
			}
		}

		public static void MakeEveryoneJump()
		{
			Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
			foreach (Character val in array)
			{
				if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null && !((MonoBehaviourPun)val).photonView.IsMine)
				{
					((MonoBehaviourPun)val).photonView.RPC("JumpRpc", (RpcTarget)0, new object[1] { true });
				}
			}
			Debug.Log((object)"[Cheat] Made all other players jump!");
		}

		public static void MakeAllFall()
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (!((MonoBehaviourPun)allCharacter).photonView.IsMine)
				{
					allCharacter.refs.view.RPC("RPCA_Fall", (RpcTarget)0, new object[2] { allCharacter, 5 });
				}
			}
			Debug.Log((object)"[Cheat] Made all other players Fall!");
		}

		public static void MakeAllPassout()
		{
			Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
			foreach (Character val in array)
			{
				if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
				{
					val.refs.view.RPC("RPCA_PassOut", (RpcTarget)0, new object[2] { val, 5 });
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
						if ((Object)(object)val != (Object)null && (Object)(object)val.character != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
						{
							string nickName = ((MonoBehaviourPun)val).photonView.Owner.NickName;
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
				Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
				foreach (Character val2 in array)
				{
					if ((Object)(object)val2.player != (Object)null && (Object)(object)((MonoBehaviourPun)val2.player).photonView != (Object)null)
					{
						string nickName2 = ((MonoBehaviourPun)val2.player).photonView.Owner.NickName;
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
				if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null && ((MonoBehaviourPun)val).photonView.Owner != null)
				{
					flag = ((MonoBehaviourPun)val).photonView.Owner.IsMasterClient;
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
				if ((Object)(object)val3 != (Object)null && (Object)(object)((MonoBehaviourPun)val3).photonView != (Object)null && ((MonoBehaviourPun)val3).photonView.Owner != null)
				{
					flag2 = ((MonoBehaviourPun)val3).photonView.Owner.IsMasterClient;
				}
				string text4 = TruncateForDisplay(text3);
				string text5 = (flag2 ? (text4 + " [HOST]") : text4);
				GUILayout.Label("Selected: " + text5, new GUIStyle(labelStyle)
				{
					fontStyle = (FontStyle)1
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
			}) && (Object)(object)Cheat.instance != (Object)null)
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
			if (GUILayout.Button(text + (itemDropdownOpen ? " " : " ?"), buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
			{
				itemDropdownOpen = !itemDropdownOpen;
			}
			if (GUILayout.Button("Refresh", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(90f),
				GUILayout.Height(36f)
			}) && (Object)(object)Cheat.instance != (Object)null)
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
					fontStyle = (FontStyle)1
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
			if (GUILayout.Button(text + (entityDropdownOpen ? " " : " ?"), buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
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
					fontStyle = (FontStyle)1
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
			foreach (GameObject item in controlledEntities.Keys.Where((GameObject k) => (Object)(object)k == (Object)null).ToList())
			{
				controlledEntities.Remove(item);
				followPlayerEntities.Remove(item);
				targetPositions.Remove(item);
			}
		}

		public static void InitializeSpawnedEntity(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			try
			{
				Debug.Log((object)("[Cheat] Initializing spawned entity: " + ((Object)entity).name));
				entity.SetActive(true);
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null)
				{
					((Behaviour)component).enabled = true;
					Debug.Log((object)("[Cheat] Enabled Character component on " + ((Object)entity).name));
					if ((Object)(object)component != (Object)(object)Character.localCharacter && (Object)(object)((MonoBehaviourPun)component).photonView != (Object)null && ((MonoBehaviourPun)component).photonView.IsMine)
					{
						try
						{
							((MonoBehaviourPun)component).photonView.TransferOwnership(PhotonNetwork.MasterClient);
							Debug.Log((object)("[Cheat] Transferred ownership of " + ((Object)entity).name + " to master client"));
						}
						catch
						{
							Debug.LogWarning((object)("[Cheat] Could not transfer ownership of " + ((Object)entity).name));
						}
					}
				}
				CharacterMovement component2 = entity.GetComponent<CharacterMovement>();
				if ((Object)(object)component2 != (Object)null)
				{
					((Behaviour)component2).enabled = true;
					Debug.Log((object)("[Cheat] Enabled CharacterMovement on " + ((Object)entity).name));
				}
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if ((Object)(object)val != (Object)null)
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
					if ((Object)(object)component != (Object)null)
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
				if ((Object)(object)component3 != (Object)null)
				{
					component3.isKinematic = false;
					component3.useGravity = true;
					Debug.Log((object)("[Cheat] Enabled Rigidbody on " + ((Object)entity).name));
				}
				Collider[] componentsInChildren = entity.GetComponentsInChildren<Collider>();
				foreach (Collider val2 in componentsInChildren)
				{
					if ((Object)(object)val2 != (Object)null)
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
				CheatConfig._cachedEntityList.RemoveAll((GameObject e) => (Object)(object)e == (Object)null);
				return CheatConfig._cachedEntityList;
			}
			List<GameObject> list = new List<GameObject>();
			try
			{
				Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
				foreach (Character val in array)
				{
					try
					{
						if ((Object)(object)val == (Object)null || (Object)(object)((Component)val).gameObject == (Object)null || (Object)(object)val == (Object)(object)Character.localCharacter)
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
								if ((Object)(object)((Component)val).gameObject.GetComponent<Player>() == (Object)null)
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
								if (!((Object)(object)val.player == (Object)null))
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
									if (!((Object)(object)((Component)val).gameObject.GetComponent<Player>() == (Object)null))
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
						if (flag && !list.Contains(((Component)val).gameObject))
						{
							list.Add(((Component)val).gameObject);
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
						Object[] array2 = Object.FindObjectsByType(type, (FindObjectsSortMode)0);
						foreach (Object val2 in array2)
						{
							if (!(val2 == (Object)null))
							{
								GameObject val3 = (GameObject)(object)((val2 is GameObject) ? val2 : null);
								if (!((Object)(object)val3 == (Object)null) && !((Object)(object)val3.GetComponent<Character>() != (Object)null) && !list.Contains(val3))
								{
									list.Add(val3);
								}
							}
						}
					}
					Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Mob" && t.IsClass);
					if (type2 != null)
					{
						Object[] array2 = Object.FindObjectsByType(type2, (FindObjectsSortMode)0);
						foreach (Object val4 in array2)
						{
							if (!(val4 == (Object)null))
							{
								GameObject val5 = (GameObject)(object)((val4 is GameObject) ? val4 : null);
								if (!((Object)(object)val5 == (Object)null) && !((Object)(object)val5.GetComponent<Character>() != (Object)null) && !list.Contains(val5))
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
				if ((Object)(object)entity == (Object)null)
				{
					return "Unknown";
				}
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null)
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
							if (!((Object)(object)val == (Object)null))
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
						if ((Object)(object)((Component)component).gameObject.GetComponent<Player>() != (Object)null && (Object)(object)((MonoBehaviourPun)component).photonView != (Object)null && ((MonoBehaviourPun)component).photonView.Owner != null)
						{
							return TruncateForDisplay(((MonoBehaviourPun)component).photonView.Owner.NickName ?? "Unknown");
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
				if ((Object)(object)entity == (Object)null)
				{
					return "Unknown";
				}
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null)
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
							if ((Object)(object)val == (Object)null)
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
						if ((Object)(object)((Component)component).gameObject.GetComponent<Player>() != (Object)null && (Object)(object)((MonoBehaviourPun)component).photonView != (Object)null && ((MonoBehaviourPun)component).photonView.Owner != null)
						{
							return ((MonoBehaviourPun)component).photonView.IsMine ? "Player (You)" : "Player";
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
						if ((Object)(object)val2 == (Object)null)
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
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null && (Object)(object)component.input != (Object)null)
				{
					Vector3 normalized = ((Vector3)(ref component.data.lookDirection_Flat)).normalized;
					Vector3 val = Vector3.Cross(Vector3.up, normalized);
					Vector3 normalized2 = ((Vector3)(ref val)).normalized;
					Vector3 val2 = normalized * direction.z + normalized2 * direction.x;
					Vector3 value = entity.transform.position + ((Vector3)(ref val2)).normalized * 5f;
					targetPositions[entity] = value;
					followPlayerEntities.Remove(entity);
				}
				else
				{
					Vector3 value2 = entity.transform.position + ((Vector3)(ref direction)).normalized * 5f;
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
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null && (Object)(object)component.input != (Object)null)
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
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null && (Object)(object)component.input != (Object)null)
				{
					component.input.jumpIsPressed = true;
					if ((Object)(object)Cheat.instance != (Object)null)
					{
						((MonoBehaviour)Cheat.instance).StartCoroutine(ResetJumpInput(component));
					}
				}
				else
				{
					Rigidbody component2 = entity.GetComponent<Rigidbody>();
					if ((Object)(object)component2 != (Object)null)
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
			if ((Object)(object)Character.localCharacter == (Object)null || (Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			if ((Object)(object)currentlyControlledEntity == (Object)null && controlledEntities.Count > 0)
			{
				RestoreCameraToPlayer();
				controlledEntities.Clear();
				followPlayerEntities.Clear();
				targetPositions.Clear();
				return;
			}
			if ((Object)(object)currentlyControlledEntity != (Object)null)
			{
				bool flag = false;
				try
				{
					flag = (Object)(object)currentlyControlledEntity != (Object)null && (Object)(object)currentlyControlledEntity.transform != (Object)null;
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
					if ((Object)(object)val != (Object)null)
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
				if ((Object)(object)key == (Object)null || !item.Value)
				{
					followPlayerEntities.Remove(key);
					continue;
				}
				try
				{
					Vector3 position = ((Component)Camera.main).transform.position;
					Vector3 position2 = key.transform.position;
					Vector3 val2 = position - position2;
					if (((Vector3)(ref val2)).magnitude > entityFollowDistance)
					{
						val2.y = 0f;
						val2 = ((Vector3)(ref val2)).normalized;
						Character component = key.GetComponent<Character>();
						if ((Object)(object)component != (Object)null && (Object)(object)component.input != (Object)null)
						{
							Vector3 normalized = ((Vector3)(ref component.data.lookDirection_Flat)).normalized;
							val3 = Vector3.Cross(Vector3.up, normalized);
							Vector3 normalized2 = ((Vector3)(ref val3)).normalized;
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
				if ((Object)(object)key2 == (Object)null)
				{
					targetPositions.Remove(key2);
					continue;
				}
				Vector3 value = item2.Value;
				Vector3 position3 = key2.transform.position;
				Vector3 val4 = value - position3;
				if (((Vector3)(ref val4)).magnitude > 0.5f)
				{
					val4 = ((Vector3)(ref val4)).normalized;
					Character component2 = key2.GetComponent<Character>();
					if ((Object)(object)component2 != (Object)null && (Object)(object)component2.input != (Object)null)
					{
						Vector3 normalized3 = ((Vector3)(ref component2.data.lookDirection_Flat)).normalized;
						val3 = Vector3.Cross(Vector3.up, normalized3);
						Vector3 normalized4 = ((Vector3)(ref val3)).normalized;
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
			if ((Object)(object)val == (Object)null)
			{
				val = GetLocalPlayer();
			}
			object obj;
			if (!((Object)(object)val != (Object)null) || !((Object)(object)((MonoBehaviourPun)val).photonView != (Object)null) || !((MonoBehaviourPun)val).photonView.IsMine)
			{
				if (!((Object)(object)val != (Object)null) || !((Object)(object)((MonoBehaviourPun)val).photonView != (Object)null))
				{
					obj = "Spawn on Me";
				}
				else
				{
					Player owner = ((MonoBehaviourPun)val).photonView.Owner;
					obj = "Spawn on " + TruncateForDisplay(((owner != null) ? owner.NickName : null) ?? "Selected Player");
				}
			}
			else
			{
				obj = "Spawn on Me";
			}
			if (GUILayout.Button((string)obj, buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
			{
				if ((Object)(object)val == (Object)null)
				{
					RefreshPlayerDict();
					Debug.LogWarning((object)"No target player found for spawning item");
					return;
				}
				try
				{
					GameObject val2 = PhotonNetwork.Instantiate("0_Items/" + items[selectedItemIndex], val.Center + Vector3.up * 3f, Quaternion.identity, (byte)0, (object[])null);
					if ((Object)(object)val2 != (Object)null)
					{
						Item component = val2.GetComponent<Item>();
						if ((Object)(object)component != (Object)null)
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
				if ((Object)(object)value == (Object)null)
				{
					continue;
				}
				try
				{
					GameObject val3 = PhotonNetwork.Instantiate("0_Items/" + items[selectedItemIndex], value.Center + Vector3.up * 3f, Quaternion.identity, (byte)0, (object[])null);
					if ((Object)(object)val3 != (Object)null)
					{
						Item component2 = val3.GetComponent<Item>();
						if ((Object)(object)component2 != (Object)null)
						{
							component2.Interact(value);
						}
					}
				}
				catch (Exception ex2)
				{
					PhotonView photonView = ((MonoBehaviourPun)value).photonView;
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
			if ((Object)(object)localPlayer == (Object)null)
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
			if (GUILayout.Button("Warp to Player", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }) && (Object)(object)val != (Object)null)
			{
				((MonoBehaviourPun)localPlayer).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2]
				{
					((Component)val.refs.head).transform.position,
					false
				});
			}
			GUILayout.Space(4f);
			if (GUILayout.Button("Warp Player to Me", buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }) && (Object)(object)val != (Object)null)
			{
				((MonoBehaviourPun)val).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2]
				{
					((Component)localPlayer.refs.head).transform.position,
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
				if ((Object)(object)value != (Object)null && (Object)(object)((MonoBehaviourPun)value).photonView != (Object)null)
				{
					((MonoBehaviourPun)value).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2]
					{
						((Component)localPlayer.refs.head).transform.position,
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
				Character val = playerDict.Values.FirstOrDefault((Character x) => x.IsLocal || ((MonoBehaviourPun)x.player).photonView.IsMine);
				if ((Object)(object)val == (Object)null)
				{
					GUILayout.Label("Local player not found", labelStyle, Array.Empty<GUILayoutOption>());
					return;
				}
				Character val2 = playerDict[array[selectedPlayerIndex]];
				if ((Object)(object)val2 == (Object)null)
				{
					GUILayout.Label("Selected player not found", labelStyle, Array.Empty<GUILayoutOption>());
					return;
				}
				if (GUILayout.Button("Respawn self at player", Array.Empty<GUILayoutOption>()))
				{
					((MonoBehaviourPun)val).photonView.RPC("RPCA_ReviveAtPosition", (RpcTarget)0, new object[2]
					{
						((Component)val2.refs.head).transform.position,
						true
					});
				}
				if (GUILayout.Button("Respawn player at self", Array.Empty<GUILayoutOption>()))
				{
					((MonoBehaviourPun)val2).photonView.RPC("RPCA_ReviveAtPosition", (RpcTarget)0, new object[2]
					{
						((Component)val.refs.head).transform.position,
						true
					});
				}
				if (GUILayout.Button("Respawn player at Position", Array.Empty<GUILayoutOption>()))
				{
					Vector3 val3 = (((Object)(object)val2.Ghost != (Object)null) ? ((Component)val2.Ghost).transform.position : val2.Head);
					((MonoBehaviourPun)val2).photonView.RPC("RPCA_ReviveAtPosition", (RpcTarget)0, new object[2]
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
				Character val = playerDict.Values.First((Character x) => ((MonoBehaviourPun)x.player).photonView.Owner.NickName == playerDict.Keys.ToArray()[selectedPlayerIndex]);
				((MonoBehaviourPun)val).photonView.RPC("RPCA_Die", (RpcTarget)0, new object[1] { val.Center });
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
				if ((Object)(object)targetPlayer == (Object)null || (Object)(object)targetPlayer.player == (Object)null || targetPlayer.refs == null || (Object)(object)targetPlayer.refs.items == (Object)null)
				{
					return;
				}
				ItemSlot itemSlot = targetPlayer.player.GetItemSlot(slotID);
				if (itemSlot.IsEmpty() || (Object)(object)itemSlot.prefab == (Object)null)
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
						if ((Object)(object)val2 != (Object)null && (Object)(object)((Component)val2).transform != (Object)null)
						{
							val = targetPlayer.Center + ((Component)val2).transform.forward * 0.6f;
						}
					}
				}
				catch
				{
				}
				((MonoBehaviourPun)targetPlayer.refs.items).photonView.RPC("DropItemFromSlotRPC", (RpcTarget)0, new object[2] { slotID, val });
				string text = TruncateForDisplay(((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName);
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
				if ((Object)(object)Character.localCharacter == (Object)null || (Object)(object)Character.localCharacter.player == (Object)null || (Object)(object)targetPlayer == (Object)null || (Object)(object)targetPlayer.player == (Object)null)
				{
					return;
				}
				ItemSlot itemSlot = targetPlayer.player.GetItemSlot(slotID);
				if (!itemSlot.IsEmpty() && !((Object)(object)itemSlot.prefab == (Object)null))
				{
					ItemSlot val = default(ItemSlot);
					if (Character.localCharacter.player.AddItem(itemSlot.prefab.itemID, itemSlot.data, ref val))
					{
						targetPlayer.player.EmptySlot(Optionable<byte>.Some(slotID));
						string text = TruncateForDisplay(((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName);
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
				if ((Object)(object)targetPlayer == (Object)null || (Object)(object)targetPlayer.player == (Object)null)
				{
					return;
				}
				ItemSlot itemSlot = targetPlayer.player.GetItemSlot(slotID);
				if (itemSlot.IsEmpty() || (Object)(object)itemSlot.prefab == (Object)null)
				{
					return;
				}
				Item val = null;
				if ((Object)(object)targetPlayer.data.currentItem != (Object)null && targetPlayer.data.currentItem.itemID == itemSlot.prefab.itemID)
				{
					val = targetPlayer.data.currentItem;
				}
				else
				{
					Item[] array = Object.FindObjectsByType<Item>((FindObjectsSortMode)0);
					foreach (Item val2 in array)
					{
						if ((Object)(object)val2 != (Object)null && val2.itemID == itemSlot.prefab.itemID && val2.data != null && val2.data.guid == itemSlot.data.guid)
						{
							val = val2;
							break;
						}
					}
				}
				if ((Object)(object)val != (Object)null)
				{
					ItemCooking component = ((Component)val).GetComponent<ItemCooking>();
					if ((Object)(object)component != (Object)null && component.canBeCooked)
					{
						if ((Object)(object)((MonoBehaviourPun)val).photonView != (Object)null && ((MonoBehaviourPun)val).photonView.IsMine)
						{
							component.FinishCooking();
							string text = TruncateForDisplay(((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName);
							Debug.Log((object)("[Inventory] Cooked " + itemSlot.prefab.GetName() + " for " + text + "!"));
						}
						else
						{
							((MonoBehaviourPun)val).photonView.RPC("FinishCookingRPC", (RpcTarget)0, Array.Empty<object>());
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
			if ((Object)(object)Character.localCharacter == (Object)null || (Object)(object)Camera.main == (Object)null)
			{
				Debug.LogWarning((object)"[GUI] Cannot spawn test dummy - character or camera not found");
				return;
			}
			try
			{
				Vector3 val = ((Component)Camera.main).transform.position + ((Component)Camera.main).transform.forward * 3f;
				val.y = Character.localCharacter.Center.y;
				GameObject val2 = null;
				string[] array = new string[4] { "Character", "Player", "0_Characters/Character", "Characters/Character" };
				foreach (string text in array)
				{
					try
					{
						val2 = PhotonNetwork.Instantiate(text, val, Quaternion.identity, (byte)0, (object[])null);
						if ((Object)(object)val2 != (Object)null)
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
				if ((Object)(object)val2 == (Object)null)
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
					if ((Object)(object)val == (Object)null)
					{
						continue;
					}
					Character component = val.GetComponent<Character>();
					if ((Object)(object)component != (Object)null)
					{
						FieldInfo field = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
						bool flag = false;
						if (field != null)
						{
							flag = (bool)field.GetValue(component);
						}
						if ((flag || CheatConfig.SpawnedEntities.Contains(val)) && (Object)(object)((MonoBehaviourPun)component).photonView != (Object)null && ((MonoBehaviourPun)component).photonView.IsMine)
						{
							((MonoBehaviourPun)component).photonView.RPC("RPCA_Die", (RpcTarget)0, new object[1] { component.Center });
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
			if ((Object)(object)Player.localPlayer == (Object)null)
			{
				return null;
			}
			return playerDict?.Values.FirstOrDefault((Character c) => (Object)(object)c != (Object)null && (Object)(object)c.player == (Object)(object)Player.localPlayer);
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
			buttonStyle.alignment = (TextAnchor)4;
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
			toggleStyle.fontStyle = (FontStyle)1;
			toggleStyle.padding = new RectOffset(16, 16, 10, 10);
			toggleStyle.alignment = (TextAnchor)4;
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
			headerStyle.fontStyle = (FontStyle)1;
			headerStyle.alignment = (TextAnchor)3;
			headerStyle.padding = new RectOffset(0, 0, 4, 6);
			sectionHeaderStyle = new GUIStyle(GUI.skin.label);
			sectionHeaderStyle.normal.textColor = textColor;
			sectionHeaderStyle.fontSize = 13;
			sectionHeaderStyle.fontStyle = (FontStyle)1;
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
			foldoutStyle.fontStyle = (FontStyle)1;
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
				if ((Object)(object)val == (Object)null)
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
				if ((Object)(object)buttonStyle.normal.background == (Object)null)
				{
					buttonStyle.normal.background = GUIHelpers.MakeTex(2, 2, col3);
				}
				if ((Object)(object)buttonStyle.hover.background == (Object)null)
				{
					buttonStyle.hover.background = GUIHelpers.MakeTex(2, 2, col4);
				}
				if ((Object)(object)buttonStyle.active.background == (Object)null)
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
				if ((Object)(object)boxStyle.normal.background == (Object)null)
				{
					boxStyle.normal.background = GUIHelpers.MakeTex(2, 2, col2);
				}
				GUI.skin.box = boxStyle;
			}
			if (textFieldStyle != null)
			{
				if ((Object)(object)textFieldStyle.normal.background == (Object)null)
				{
					textFieldStyle.normal.background = GUIHelpers.MakeTex(2, 2, col6);
				}
				if ((Object)(object)textFieldStyle.focused.background == (Object)null)
				{
					textFieldStyle.focused.background = GUIHelpers.MakeTex(2, 2, col7);
				}
				GUI.skin.textField = textFieldStyle;
			}
			GUILayout.BeginArea(new Rect(8f, 30f, ((Rect)(ref GUIRect)).width - 16f, ((Rect)(ref GUIRect)).height - 38f));
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
					val2.fontStyle = (FontStyle)1;
					GUIHelpers.styleCache[key] = val2;
				}
				else
				{
					val2 = GUIHelpers.styleCache[key];
				}
				if (GUILayout.Button(tabnames[i], val2, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Height(40f),
					GUILayout.Width((((Rect)(ref GUIRect)).width - 32f) / (float)tabnames.Length)
				}))
				{
					selected_tab = i;
				}
				GUILayout.Space(2f);
			}
			GUILayout.Space(2f);
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUI.DrawTexture(GUILayoutUtility.GetRect(((Rect)(ref GUIRect)).width - 16f, 1f), (Texture)(object)GUIHelpers.MakeTex(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.8f)));
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
			string text = (currentState ? ("\uf111 " + label) : ("  " + label));
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
			string text = (flag ? title.Replace("?", "\uf078") : title);
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
}
