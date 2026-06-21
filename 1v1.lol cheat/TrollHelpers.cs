using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace _1v1.lol_cheat.Troll
{
	public static class TrollHelpers
	{
		private static Dictionary<Character, Coroutine> activeTrollEffects = new Dictionary<Character, Coroutine>();

		private static List<Coroutine> activeCrashCoroutines = new List<Coroutine>();

		private static MonoBehaviour coroutineRunner;

		public static void Initialize(MonoBehaviour runner)
		{
			coroutineRunner = runner;
		}

		public static PhotonView GetPhotonView(Character target)
		{
			if ((Object)(object)target == (Object)null)
			{
				return null;
			}
			if (target.refs != null && (Object)(object)target.refs.view != (Object)null)
			{
				return target.refs.view;
			}
			return ((MonoBehaviourPun)target).photonView;
		}

		public static bool CallRPCMethod(Character target, string methodName, RpcTarget rpcTarget, params object[] parameters)
		{
			if ((Object)(object)target == (Object)null)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: target is null for " + methodName));
				return false;
			}
			if (!PhotonNetwork.IsConnected)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: PhotonNetwork not connected for " + methodName));
				return false;
			}
			PhotonView photonView = GetPhotonView(target);
			if ((Object)(object)photonView == (Object)null)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: Could not get PhotonView for " + methodName));
				return false;
			}
			if (photonView.ViewID == 0)
			{
				Debug.LogWarning((object)("[Troll] CallRPCMethod: PhotonView has invalid ViewID (0) for " + methodName));
				return false;
			}
			try
			{
				photonView.RPC(methodName, rpcTarget, parameters);
				Debug.Log((object)$"[Troll] Successfully called RPC {methodName} on ViewID {photonView.ViewID}");
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] RPC call failed for " + methodName + ": " + ex.Message + "\n" + ex.StackTrace));
				try
				{
					MethodInfo method = typeof(Character).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						object[] customAttributes = method.GetCustomAttributes(typeof(PunRPC), inherit: false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							method.Invoke(target, parameters);
							Debug.Log((object)("[Troll] Called " + methodName + " directly via reflection (local only - not networked)"));
							return true;
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogWarning((object)("[Troll] Direct method call also failed for " + methodName + ": " + ex2.Message));
				}
				return false;
			}
		}

		public static bool ShouldSkipPlayer(Character target, bool includeSelf, bool isCrashEffect = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null)
			{
				return true;
			}
			bool isMine = ((MonoBehaviourPun)target).photonView.IsMine;
			if (isCrashEffect && isMine)
			{
				return true;
			}
			if (isMine)
			{
				if (GUI.selectedPlayerIndex >= 0 && GUI.playerDict != null && GUI.playerDict.Count > 0)
				{
					string[] array = GUI.playerDict.Keys.ToArray();
					if (GUI.selectedPlayerIndex < array.Length)
					{
						Character val = GUI.playerDict[array[GUI.selectedPlayerIndex]];
						if ((Object)(object)val != (Object)null && (Object)(object)val == (Object)(object)Character.localCharacter)
						{
							return false;
						}
					}
				}
				if (!includeSelf)
				{
					return true;
				}
			}
			return false;
		}

		public static void RegisterTrollEffect(Character target, Coroutine coroutine)
		{
			if ((Object)(object)target != (Object)null && coroutine != null)
			{
				activeTrollEffects[target] = coroutine;
			}
		}

		public static void RegisterCrashCoroutine(Coroutine coroutine)
		{
			if (coroutine != null)
			{
				activeCrashCoroutines.Add(coroutine);
			}
		}

		public static void StopTrollEffects(Character target)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)coroutineRunner == (Object)null) && activeTrollEffects.ContainsKey(target))
			{
				coroutineRunner.StopCoroutine(activeTrollEffects[target]);
				activeTrollEffects.Remove(target);
			}
		}

		public static void StopAllTrollEffects()
		{
			if ((Object)(object)coroutineRunner == (Object)null)
			{
				return;
			}
			foreach (Coroutine value in activeTrollEffects.Values)
			{
				if (value != null)
				{
					coroutineRunner.StopCoroutine(value);
				}
			}
			activeTrollEffects.Clear();
		}

		public static void StopAllCrashCoroutines()
		{
			if ((Object)(object)coroutineRunner == (Object)null)
			{
				return;
			}
			foreach (Coroutine activeCrashCoroutine in activeCrashCoroutines)
			{
				if (activeCrashCoroutine != null)
				{
					try
					{
						coroutineRunner.StopCoroutine(activeCrashCoroutine);
					}
					catch
					{
					}
				}
			}
			activeCrashCoroutines.Clear();
			Debug.Log((object)"[Crash] Stopped all active crash coroutines");
		}

		public static MonoBehaviour GetCoroutineRunner()
		{
			return coroutineRunner;
		}
	}
}
