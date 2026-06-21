using System;
using System.Collections.Generic;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUINetworkTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("ConnectionControl", "? Connection Control", Color.white))
			{
				GUILayout.Label("Current Status:", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)1
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
}
