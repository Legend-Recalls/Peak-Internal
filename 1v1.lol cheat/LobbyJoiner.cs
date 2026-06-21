using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Steamworks;
using UnityEngine;

namespace _1v1.lol_cheat
{
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
			LastJoinMessage = $"Brute forcing lobby IDs around {baseLobbyID} (\u00b1{range})... This may take a while...";
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
}
