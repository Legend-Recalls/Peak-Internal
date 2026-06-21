using System;
using UnityEngine;

namespace _1v1.lol_cheat
{
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
			if ((Object)(object)val != (Object)null)
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
						Vector3 position = (((Object)(object)Character.localCharacter != (Object)null) ? (Character.localCharacter.Center + Vector3.up * 2f) : val.Center);
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
					if (GUILayout.Button("Make Me Carry", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null)
					{
						TrollFeatures.StartCarryPlayer(Character.localCharacter, val, CheatConfig.TrollIncludeSelf);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Drop Carried", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null && (Object)(object)Character.localCharacter.data.carriedPlayer == (Object)(object)val)
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
}
