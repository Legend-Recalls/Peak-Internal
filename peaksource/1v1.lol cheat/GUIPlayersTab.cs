using System;
using System.Linq;
using UnityEngine;

namespace _1v1.lol_cheat
{
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
						fontStyle = (FontStyle)1,
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
							fontStyle = (FontStyle)1
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
			if ((Object)(object)val == (Object)null || (Object)(object)val.player == (Object)null)
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
				if (GUILayout.Button("Kill", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
				{
					((MonoBehaviourPun)val).photonView.RPC("RPCA_Die", (RpcTarget)0, new object[1] { val.Center });
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
				if (GUILayout.Button("Warp to Player", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null && (Object)(object)((MonoBehaviourPun)Character.localCharacter).photonView != (Object)null)
				{
					((MonoBehaviourPun)Character.localCharacter).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2]
					{
						((Component)val.refs.head).transform.position,
						false
					});
				}
				if (GUILayout.Button("Warp Player to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
				{
					((MonoBehaviourPun)val).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2]
					{
						((Component)Character.localCharacter.refs.head).transform.position,
						false
					});
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Walk Off Cliff", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }))
				{
					TrollFeatures.MakePlayerWalkOffCliff(val, CheatConfig.TrollIncludeSelf);
				}
				if (GUILayout.Button("Warp Everyone to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null && (Object)(object)((MonoBehaviourPun)Character.localCharacter).photonView != (Object)null)
				{
					Vector3 position = ((Component)Character.localCharacter.refs.head).transform.position;
					foreach (Character value in GUI.playerDict.Values)
					{
						if ((Object)(object)value != (Object)null && (Object)(object)((MonoBehaviourPun)value).photonView != (Object)null)
						{
							((MonoBehaviourPun)value).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2] { position, false });
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
				if (GUILayout.Button("Make Me Carry", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null)
				{
					TrollFeatures.StartCarryPlayer(Character.localCharacter, val, CheatConfig.TrollIncludeSelf);
				}
				GUILayout.EndHorizontal();
				if (GUILayout.Button("Drop Carried", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(32f) }) && (Object)(object)Character.localCharacter != (Object)null)
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
					fontStyle = (FontStyle)1,
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
					fontStyle = (FontStyle)1,
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
					fontStyle = (FontStyle)1,
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
					fontStyle = (FontStyle)1,
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
					fontStyle = (FontStyle)1,
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
					fontStyle = (FontStyle)1,
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
}
