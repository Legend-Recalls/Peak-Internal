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
			if ((Object)(object)Cheat.instance != (Object)null)
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
			if (PlayerPrefs.HasKey("PEAK_CHEAT_MenuToggleKey") && (Object)(object)Cheat.instance != (Object)null)
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
