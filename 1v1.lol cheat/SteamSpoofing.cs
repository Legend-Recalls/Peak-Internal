using System;
using System.Reflection;
using Steamworks;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class SteamSpoofing
	{
		private delegate string GetUsernameDelegate();

		private delegate CSteamID GetSteamIDDelegate();

		private delegate(string, HAuthTicket) GetSteamAuthTicketDelegate();

		public static bool SpoofEnabled = false;

		public static string SpoofedUsername = "Player";

		public static ulong SpoofedSteamID = 0uL;

		public static string SpoofedPhotonUserID = "";

		private static MethodInfo originalGetUsername;

		private static MethodInfo originalGetSteamID;

		private static MethodInfo originalLoadUserID;

		private static MethodInfo originalGetSteamAuthTicket;

		private static Type networkingUtilitiesType;

		private static Type steamUserType;

		private static Type steamAuthTicketServiceType;

		private static bool initialized = false;

		private static bool methodsPatched = false;

		private const int MAX_PHOTON_NICKNAME_LENGTH = 32767;

		public static void Initialize()
		{
			if (initialized)
			{
				return;
			}
			try
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					networkingUtilitiesType = assemblies[i].GetType("Peak.Network.NetworkingUtilities");
					if (networkingUtilitiesType != null)
					{
						break;
					}
				}
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					steamUserType = assemblies[i].GetType("Steamworks.SteamUser");
					if (steamUserType != null)
					{
						break;
					}
				}
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					steamAuthTicketServiceType = assemblies[i].GetType("SteamAuthTicketService");
					if (steamAuthTicketServiceType != null)
					{
						break;
					}
				}
				if (networkingUtilitiesType != null)
				{
					originalGetUsername = networkingUtilitiesType.GetMethod("GetUsername", BindingFlags.Static | BindingFlags.Public);
					originalLoadUserID = networkingUtilitiesType.GetMethod("LoadUserID", BindingFlags.Static | BindingFlags.Public);
				}
				if (steamUserType != null)
				{
					originalGetSteamID = steamUserType.GetMethod("GetSteamID", BindingFlags.Static | BindingFlags.Public);
				}
				if (steamAuthTicketServiceType != null)
				{
					originalGetSteamAuthTicket = steamAuthTicketServiceType.GetMethod("GetSteamAuthTicket", BindingFlags.Static | BindingFlags.Public);
				}
				initialized = true;
				Debug.Log((object)"[SteamSpoofing] Initialized successfully");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[SteamSpoofing] Failed to initialize: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void SetSpoofingEnabled(bool enabled)
		{
			if (!initialized)
			{
				Initialize();
			}
			SpoofEnabled = enabled;
			if (enabled)
			{
				ApplySpoofing();
			}
			else
			{
				RemoveSpoofing();
			}
		}

		private static void ApplySpoofing()
		{
			try
			{
				if (SpoofedSteamID == 0L)
				{
					SpoofedSteamID = GenerateRandomSteamID();
				}
				if (string.IsNullOrEmpty(SpoofedPhotonUserID))
				{
					SpoofedPhotonUserID = Guid.NewGuid().ToString();
				}
				PatchGameMethods();
				if (PhotonNetwork.IsConnected)
				{
					string text2 = (PhotonNetwork.NickName = ((SpoofedUsername.Length > 32767) ? SpoofedUsername.Substring(0, 32767) : SpoofedUsername));
					Debug.Log((object)$"[SteamSpoofing] Set Photon nickname to: {text2} (length: {text2.Length})");
				}
				if (PhotonNetwork.AuthValues != null)
				{
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
					Debug.Log((object)("[SteamSpoofing] Set Photon UserID to: " + SpoofedPhotonUserID));
				}
				PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
				PlayerPrefs.Save();
				Debug.Log((object)$"[SteamSpoofing] Spoofing applied - Username: {SpoofedUsername}, SteamID: {SpoofedSteamID}, PhotonID: {SpoofedPhotonUserID}");
				Debug.Log((object)"[SteamSpoofing] P2P MODE: Since this is P2P, other players see your Photon UserId, not your Steam ID directly!");
				Debug.Log((object)"[SteamSpoofing] Your spoofed Photon UserId will be sent to other players via PublishUserId=true");
				Debug.LogWarning((object)"[SteamSpoofing] Note: If other players check Steam ID client-side, they may still see your real Steam ID. But Photon UserId spoofing should bypass most P2P ban lists!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[SteamSpoofing] Failed to apply spoofing: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		private static void PatchGameMethods()
		{
			if (methodsPatched)
			{
				return;
			}
			try
			{
				if (!PhotonNetwork.IsConnected)
				{
					if (PhotonNetwork.AuthValues == null)
					{
						PhotonNetwork.AuthValues = new AuthenticationValues();
					}
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
					string text2 = (PhotonNetwork.NickName = ((SpoofedUsername.Length > 32767) ? SpoofedUsername.Substring(0, 32767) : SpoofedUsername));
					Debug.Log((object)$"[SteamSpoofing] Pre-configured Photon with spoofed values (nickname length: {text2.Length})");
				}
				if (!string.IsNullOrEmpty(SpoofedPhotonUserID))
				{
					PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
					PlayerPrefs.Save();
					Debug.Log((object)("[SteamSpoofing] Patched PlayerPrefs UserID to: " + SpoofedPhotonUserID));
				}
				methodsPatched = true;
				Debug.Log((object)"[SteamSpoofing] Game methods patched for P2P spoofing");
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[SteamSpoofing] Could not patch game methods: " + ex.Message));
			}
		}

		private static void RemoveSpoofing()
		{
			try
			{
				methodsPatched = false;
				if (PhotonNetwork.IsConnected && SteamManager.Initialized)
				{
					try
					{
						string text = (PhotonNetwork.NickName = SteamFriends.GetPersonaName());
						Debug.Log((object)("[SteamSpoofing] Restored Photon nickname to: " + text));
					}
					catch
					{
					}
				}
				Debug.Log((object)"[SteamSpoofing] Spoofing removed");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[SteamSpoofing] Failed to remove spoofing: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void SetSpoofedSteamID(ulong steamID)
		{
			SpoofedSteamID = steamID;
			if (SpoofedSteamID == 0L)
			{
				SpoofedSteamID = GenerateRandomSteamID();
			}
			Debug.Log((object)$"[SteamSpoofing] Set spoofed Steam ID to: {SpoofedSteamID}");
		}

		public static ulong GetCurrentSteamID()
		{
			if (SpoofEnabled && SpoofedSteamID != 0L)
			{
				return SpoofedSteamID;
			}
			try
			{
				if (SteamManager.Initialized)
				{
					return SteamUser.GetSteamID().m_SteamID;
				}
			}
			catch
			{
			}
			return 0uL;
		}

		public static void SetSpoofedUsername(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				SpoofedUsername = "Player";
			}
			else if (username.Length > 32767)
			{
				SpoofedUsername = username.Substring(0, 32767);
				Debug.LogWarning((object)$"[SteamSpoofing] Username truncated from {username.Length} to {32767} characters to prevent Photon join issues");
			}
			else
			{
				SpoofedUsername = username;
			}
			if (SpoofEnabled && PhotonNetwork.IsConnected)
			{
				PhotonNetwork.NickName = SpoofedUsername;
				Debug.Log((object)$"[SteamSpoofing] Updated spoofed username to: {SpoofedUsername} (length: {SpoofedUsername.Length})");
			}
		}

		public static void SetSpoofedPhotonUserID(string userID)
		{
			if (string.IsNullOrEmpty(userID))
			{
				SpoofedPhotonUserID = Guid.NewGuid().ToString();
			}
			else
			{
				SpoofedPhotonUserID = userID;
			}
			if (SpoofEnabled)
			{
				if (PhotonNetwork.AuthValues != null)
				{
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
				}
				PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
				PlayerPrefs.Save();
				Debug.Log((object)("[SteamSpoofing] Updated spoofed Photon UserID to: " + SpoofedPhotonUserID));
			}
		}

		public static ulong GenerateRandomSteamID()
		{
			Random random = new Random();
			ulong num = 76561190000000000uL;
			ulong num2 = (ulong)random.Next(10000000, 99999999);
			return num + num2;
		}

		public static string GetCurrentUsername()
		{
			if (SpoofEnabled)
			{
				return SpoofedUsername;
			}
			try
			{
				if (SteamManager.Initialized)
				{
					return SteamFriends.GetPersonaName();
				}
			}
			catch
			{
			}
			return PhotonNetwork.NickName ?? "Player";
		}

		public static string GetCurrentPhotonUserID()
		{
			if (SpoofEnabled && !string.IsNullOrEmpty(SpoofedPhotonUserID))
			{
				return SpoofedPhotonUserID;
			}
			if (PhotonNetwork.AuthValues != null)
			{
				return PhotonNetwork.AuthValues.UserId;
			}
			if (PlayerPrefs.HasKey("UserID"))
			{
				return PlayerPrefs.GetString("UserID");
			}
			return "";
		}

		public static void Update()
		{
			if (SpoofEnabled)
			{
				if (SpoofedSteamID == 0L)
				{
					SpoofedSteamID = GenerateRandomSteamID();
				}
				if (string.IsNullOrEmpty(SpoofedPhotonUserID))
				{
					SpoofedPhotonUserID = Guid.NewGuid().ToString();
				}
				if (PhotonNetwork.AuthValues == null && PhotonNetwork.NetworkingClient != null)
				{
					PhotonNetwork.AuthValues = new AuthenticationValues();
				}
				if (PhotonNetwork.AuthValues != null && PhotonNetwork.AuthValues.UserId != SpoofedPhotonUserID)
				{
					PhotonNetwork.AuthValues.UserId = SpoofedPhotonUserID;
					Debug.Log((object)("[SteamSpoofing] Forced Photon UserID to: " + SpoofedPhotonUserID));
				}
				string text = ((SpoofedUsername.Length > 32767) ? SpoofedUsername.Substring(0, 32767) : SpoofedUsername);
				if (PhotonNetwork.NickName != text)
				{
					PhotonNetwork.NickName = text;
					Debug.Log((object)$"[SteamSpoofing] Forced Photon NickName to: {text} (length: {text.Length})");
				}
				if (PlayerPrefs.GetString("UserID") != SpoofedPhotonUserID)
				{
					PlayerPrefs.SetString("UserID", SpoofedPhotonUserID);
				}
			}
		}

		public static void ResetPhotonUserID()
		{
			SpoofedPhotonUserID = Guid.NewGuid().ToString();
			SetSpoofedPhotonUserID(SpoofedPhotonUserID);
		}
	}
}
