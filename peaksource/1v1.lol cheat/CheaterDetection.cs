using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class CheaterDetection
	{
		private static Dictionary<string, Dictionary<string, int>> playerRPCCounts = new Dictionary<string, Dictionary<string, int>>();

		private static Dictionary<string, float> playerRPCTimestamps = new Dictionary<string, float>();

		private static Dictionary<string, Dictionary<string, object>> playerLastRPCData = new Dictionary<string, Dictionary<string, object>>();

		private const float RPC_RATE_CHECK_INTERVAL = 1f;

		private const int MAX_RPC_PER_SECOND = 20;

		private const float SUSPICIOUS_RPC_COOLDOWN = 5f;

		private static Dictionary<string, float> lastDetectionTime = new Dictionary<string, float>();

		public static void Initialize()
		{
			try
			{
				HookPhotonRPC();
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[CheaterDetection] Failed to initialize: " + ex.Message));
			}
		}

		private static void HookPhotonRPC()
		{
			try
			{
				if (typeof(PhotonNetwork).GetMethod("LoadLevel", BindingFlags.Static | BindingFlags.Public) != null)
				{
					Debug.Log((object)"[CheaterDetection] Photon RPC hook initialized");
				}
			}
			catch
			{
			}
		}

		public static void OnRPCReceived(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (!CheatConfig.CheaterDetectionEnabled || string.IsNullOrEmpty(playerName) || (Object)(object)view == (Object)null || view.IsMine)
			{
				return;
			}
			try
			{
				float time = Time.time;
				if (!playerRPCCounts.ContainsKey(playerName))
				{
					playerRPCCounts[playerName] = new Dictionary<string, int>();
					playerRPCTimestamps[playerName] = time;
					playerLastRPCData[playerName] = new Dictionary<string, object>();
				}
				if (!playerRPCCounts[playerName].ContainsKey(rpcMethodName))
				{
					playerRPCCounts[playerName][rpcMethodName] = 0;
				}
				playerRPCCounts[playerName][rpcMethodName]++;
				if (time - playerRPCTimestamps[playerName] >= 1f)
				{
					int num = playerRPCCounts[playerName].Values.Sum();
					if (num > 20)
					{
						DetectCheater(playerName, $"Excessive RPC rate: {num}/sec");
					}
					playerRPCCounts[playerName].Clear();
					playerRPCTimestamps[playerName] = time;
				}
				CheckSuspiciousRPC(playerName, rpcMethodName, parameters, view);
			}
			catch
			{
			}
		}

		private static void CheckSuspiciousRPC(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			try
			{
				if (!lastDetectionTime.ContainsKey(playerName) || !(Time.time - lastDetectionTime[playerName] < 5f))
				{
					if (CheatConfig.DetectionType_ImpossibleRevive && CheckImpossibleRevive(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible revive RPC");
					}
					else if (CheatConfig.DetectionType_ImpossibleTeleport && CheckImpossibleTeleport(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible teleport RPC");
					}
					else if (CheatConfig.DetectionType_UnauthorizedItemControl && CheckUnauthorizedItemControl(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Unauthorized item control RPC");
					}
					else if (CheatConfig.DetectionType_ImpossibleStatus && CheckImpossibleStatus(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible status manipulation RPC");
					}
					else if (CheatConfig.DetectionType_ImpossibleItemSpawn && CheckImpossibleItemSpawn(playerName, rpcMethodName, parameters, view))
					{
						DetectCheater(playerName, "Impossible item spawn RPC");
					}
				}
			}
			catch
			{
			}
		}

		private static bool CheckImpossibleRevive(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName == "RPCA_Revive" || rpcMethodName == "RPCA_ReviveAtPosition")
			{
				try
				{
					Character val = ((view != null) ? ((Component)view).GetComponent<Character>() : null);
					if ((Object)(object)val != (Object)null && (Object)(object)val.data != (Object)null && !val.data.dead && !val.data.fullyPassedOut)
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckImpossibleTeleport(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName == "WarpPlayerRPC" || rpcMethodName == "RPCA_ReviveAtPosition")
			{
				try
				{
					if (parameters != null && parameters.Length != 0 && parameters[0] is Vector3)
					{
						Vector3 val = (Vector3)parameters[0];
						Character val2 = ((view != null) ? ((Component)view).GetComponent<Character>() : null);
						if ((Object)(object)val2 != (Object)null && Vector3.Distance(val2.Center, val) > 500f)
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckUnauthorizedItemControl(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName == "SetKinematicRPC" || rpcMethodName == "SetKinematicAndResetSyncData")
			{
				try
				{
					Item val = ((view != null) ? ((Component)view).GetComponent<Item>() : null);
					if ((Object)(object)val != (Object)null)
					{
						if ((Object)(object)val.holderCharacter == (Object)null || (Object)(object)((MonoBehaviourPun)val.holderCharacter).photonView == (Object)null)
						{
							return true;
						}
						Player owner = ((MonoBehaviourPun)val.holderCharacter).photonView.Owner;
						if (((owner != null) ? owner.NickName : null) != playerName)
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckImpossibleStatus(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName.Contains("Status") || rpcMethodName.Contains("Affliction"))
			{
				try
				{
					Character val = ((view != null) ? ((Component)view).GetComponent<Character>() : null);
					if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null && !((MonoBehaviourPun)val).photonView.IsMine && rpcMethodName == "SyncStatusesRPC" && parameters != null && parameters.Length != 0)
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static bool CheckImpossibleItemSpawn(string playerName, string rpcMethodName, object[] parameters, PhotonView view)
		{
			if (rpcMethodName.Contains("Spawn") || rpcMethodName.Contains("Instantiate"))
			{
				try
				{
					if ((Object)(object)view != (Object)null && view.Owner != null && view.Owner.NickName == playerName && !PhotonNetwork.IsMasterClient)
					{
						Player masterClient = PhotonNetwork.MasterClient;
						if (playerName != ((masterClient != null) ? masterClient.NickName : null))
						{
							return true;
						}
					}
				}
				catch
				{
				}
			}
			return false;
		}

		private static void DetectCheater(string playerName, string reason)
		{
			if (!CheatConfig.DetectedCheaters.ContainsKey(playerName))
			{
				CheatConfig.DetectedCheaters[playerName] = true;
				lastDetectionTime[playerName] = Time.time;
				Debug.LogWarning((object)("[CheaterDetection] CHEATER DETECTED: " + playerName + " - " + reason));
			}
		}

		public static void Update()
		{
			if (!CheatConfig.CheaterDetectionEnabled)
			{
				return;
			}
			try
			{
				foreach (string item in CheatConfig.DetectedCheaters.Keys.ToList())
				{
					if (GUI.playerDict == null || !GUI.playerDict.ContainsKey(item))
					{
						CheatConfig.DetectedCheaters.Remove(item);
						playerRPCCounts.Remove(item);
						playerRPCTimestamps.Remove(item);
						playerLastRPCData.Remove(item);
						lastDetectionTime.Remove(item);
					}
				}
			}
			catch
			{
			}
		}
	}
}
