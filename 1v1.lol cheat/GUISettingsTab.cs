using System;
using System.Collections.Generic;
using UnityEngine;

namespace _1v1.lol_cheat
{
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
					fontStyle = (FontStyle)1
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
						GUILayout.Label("", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) });
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
						fontStyle = (FontStyle)1
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
						fontStyle = (FontStyle)1
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
						fontStyle = (FontStyle)1
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
}
