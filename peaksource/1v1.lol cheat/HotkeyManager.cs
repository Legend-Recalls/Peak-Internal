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
		if ((Object)(object)Cheat.instance == (Object)null || Cheat.instance.toggled)
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
