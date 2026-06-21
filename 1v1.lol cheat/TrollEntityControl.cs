using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Photon.Pun;
using UnityEngine;

namespace _1v1.lol_cheat.Troll
{
	public static class EntityControl
	{
		private sealed class <SetScoutmasterTargetDelayed>d__1 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;
			private object <>2__current;
			public GameObject scoutmaster;
			public Character target;

			object IEnumerator<object>.Current => <>2__current;
			object IEnumerator.Current => <>2__current;

			public <SetScoutmasterTargetDelayed>d__1(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			void IDisposable.Dispose()
			{
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				switch (<>1__state)
				{
				default:
					return false;
				case 0:
					<>1__state = -1;
					<>2__current = new WaitForSeconds(0.5f);
					<>1__state = 1;
					return true;
				case 1:
					<>1__state = -1;
					if ((Object)(object)scoutmaster != (Object)null && (Object)(object)target != (Object)null)
					{
						ForceScoutmasterTarget(scoutmaster, target);
					}
					return false;
				}
			}

			bool IEnumerator.MoveNext() => MoveNext();
			void IEnumerator.Reset() { throw new NotSupportedException(); }
		}

		public static GameObject SpawnScoutmasterAndTarget(Character targetPlayer, Vector3? spawnPosition = null, bool includeSelf = false)
		{
			if ((Object)(object)targetPlayer == (Object)null || (Object)(object)((MonoBehaviourPun)targetPlayer).photonView == (Object)null)
				return null;
			if (!includeSelf && ((MonoBehaviourPun)targetPlayer).photonView.IsMine)
				return null;
			if (!PhotonNetwork.InRoom)
				return null;
			try
			{
				Vector3 val = (Vector3)(((??)spawnPosition) ?? (targetPlayer.Center + Vector3.up * 5f + Vector3.forward * 10f));
				GameObject val2 = null;
				if (PhotonNetwork.IsMasterClient)
				{
					try { val2 = PhotonNetwork.InstantiateRoomObject("Character_Scoutmaster", val, Quaternion.identity, (byte)0, (object[])null); }
					catch (Exception ex) { Debug.Log("[Troll] InstantiateRoomObject failed: " + ex.Message + ", trying regular Instantiate"); }
				}
				if ((Object)(object)val2 == (Object)null)
				{
					try { val2 = PhotonNetwork.Instantiate("Character_Scoutmaster", val, Quaternion.identity, (byte)0, (object[])null); }
					catch (Exception ex2) { Debug.LogWarning("[Troll] Regular Instantiate also failed: " + ex2.Message); }
				}
				if ((Object)(object)val2 != (Object)null)
				{
					MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
					if ((Object)(object)coroutineRunner != (Object)null)
						coroutineRunner.StartCoroutine(SetScoutmasterTargetDelayed(val2, targetPlayer));
					Debug.Log("[Troll] Spawned scoutmaster targeting " + ((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName + "!");
				}
				return val2;
			}
			catch (Exception ex3) { Debug.LogWarning("[Troll] Failed to spawn scoutmaster: " + ex3.Message); return null; }
		}

		private static IEnumerator SetScoutmasterTargetDelayed(GameObject scoutmaster, Character target)
		{
			return new <SetScoutmasterTargetDelayed>d__1(0) { scoutmaster = scoutmaster, target = target };
		}

		public static void ForceScoutmasterTarget(GameObject scoutmaster, Character targetPlayer)
		{
			if ((Object)(object)scoutmaster == (Object)null || (Object)(object)targetPlayer == (Object)null) return;
			try
			{
				Scoutmaster component = scoutmaster.GetComponent<Scoutmaster>();
				if ((Object)(object)component != (Object)null)
				{
					MethodInfo method = typeof(Scoutmaster).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(component, new object[2] { targetPlayer, 999f });
						Debug.Log("[Troll] Forced scoutmaster to target " + ((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName + "!");
					}
				}
			}
			catch (Exception ex) { Debug.LogWarning("[Troll] Failed to force scoutmaster target: " + ex.Message); }
		}

		public static void ForceZombieTarget(GameObject zombie, Character targetPlayer)
		{
			if ((Object)(object)zombie == (Object)null || (Object)(object)targetPlayer == (Object)null) return;
			try
			{
				MushroomZombie component = zombie.GetComponent<MushroomZombie>();
				if (!((Object)(object)component != (Object)null)) return;
				MethodInfo method = typeof(MushroomZombie).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(component, new object[2] { targetPlayer, 999f });
					FieldInfo field = typeof(MushroomZombie).GetField("_currentState", BindingFlags.Instance | BindingFlags.Public);
					if (field != null) field.SetValue(component, (object)(State)3);
					Debug.Log("[Troll] Forced zombie to target " + ((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName + "!");
				}
			}
			catch (Exception ex) { Debug.LogWarning("[Troll] Failed to force zombie target: " + ex.Message); }
		}

		public static void StopForceScoutmasterTarget(GameObject scoutmaster)
		{
			if ((Object)(object)scoutmaster == (Object)null) return;
			try
			{
				Scoutmaster component = scoutmaster.GetComponent<Scoutmaster>();
				if (!((Object)(object)component != (Object)null)) return;
				MethodInfo method = typeof(Scoutmaster).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(component, new object[2] { null, 0f });
					FieldInfo field = typeof(Scoutmaster).GetField("targetForcedUntil", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field != null) field.SetValue(component, 0f);
					Debug.Log("[Troll] Stopped forcing scoutmaster target");
				}
			}
			catch (Exception ex) { Debug.LogWarning("[Troll] Failed to stop forcing scoutmaster target: " + ex.Message); }
		}

		public static void StopForceZombieTarget(GameObject zombie)
		{
			if ((Object)(object)zombie == (Object)null) return;
			try
			{
				MushroomZombie component = zombie.GetComponent<MushroomZombie>();
				if (!((Object)(object)component != (Object)null)) return;
				MethodInfo method = typeof(MushroomZombie).GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					if ((Object)(object)Character.localCharacter != (Object)null)
						method.Invoke(component, new object[2] { Character.localCharacter, 0f });
					FieldInfo field = typeof(MushroomZombie).GetField("_currentState", BindingFlags.Instance | BindingFlags.Public);
					if (field != null) field.SetValue(component, (object)(State)2);
					Debug.Log("[Troll] Stopped forcing zombie target");
				}
			}
			catch (Exception ex) { Debug.LogWarning("[Troll] Failed to stop forcing zombie target: " + ex.Message); }
		}

		public static void StopForceAllScoutmasters()
		{
			Scoutmaster[] array = Object.FindObjectsByType<Scoutmaster>((FindObjectsSortMode)0);
			foreach (Scoutmaster val in array)
			{
				if ((Object)(object)val != (Object)null && (Object)(object)((Component)val).gameObject != (Object)null)
					StopForceScoutmasterTarget(((Component)val).gameObject);
			}
			Debug.Log("[Troll] Stopped forcing all scoutmaster targets");
		}

		public static void StopForceAllZombies()
		{
			MushroomZombie[] array = Object.FindObjectsByType<MushroomZombie>((FindObjectsSortMode)0);
			foreach (MushroomZombie val in array)
			{
				if ((Object)(object)val != (Object)null && (Object)(object)((Component)val).gameObject != (Object)null)
					StopForceZombieTarget(((Component)val).gameObject);
			}
			Debug.Log("[Troll] Stopped forcing all zombie targets");
		}

		public static void StartRemoteControl(Character targetPlayer, bool includeSelf = false)
		{
			if (!((Object)(object)targetPlayer == (Object)null) && !((Object)(object)((MonoBehaviourPun)targetPlayer).photonView == (Object)null) && (includeSelf || !((MonoBehaviourPun)targetPlayer).photonView.IsMine) && (Object)(object)((Component)targetPlayer).gameObject != (Object)null)
			{
				_1v1.lol_cheat.EntityControl.EnableControl(((Component)targetPlayer).gameObject);
				CheatConfig.CurrentlyControlledEntity = ((Component)targetPlayer).gameObject;
				Debug.Log("[Troll] Started remote controlling " + ((MonoBehaviourPun)targetPlayer).photonView.Owner.NickName + "!");
			}
		}

		public static void StopRemoteControl()
		{
			if ((Object)(object)CheatConfig.CurrentlyControlledEntity != (Object)null)
			{
				_1v1.lol_cheat.EntityControl.DisableControl(CheatConfig.CurrentlyControlledEntity);
				CheatConfig.CurrentlyControlledEntity = null;
				Debug.Log("[Troll] Stopped remote control!");
			}
		}
	}
}
