using System;
using System.Collections.Generic;
using UnityEngine;
using _1v1.lol_cheat;

namespace _1v1.lol_cheat
{
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
}
