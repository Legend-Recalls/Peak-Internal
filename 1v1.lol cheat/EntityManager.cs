using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class EntityManager
	{
		public static void InitializeSpawnedEntity(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			try
			{
				entity.SetActive(true);
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component != (Object)null)
				{
					((Behaviour)component).enabled = true;
				}
				CharacterMovement component2 = entity.GetComponent<CharacterMovement>();
				if ((Object)(object)component2 != (Object)null)
				{
					((Behaviour)component2).enabled = true;
				}
				EnableAIComponents(entity);
				CallInitializationMethods(entity);
				Rigidbody component3 = entity.GetComponent<Rigidbody>();
				if ((Object)(object)component3 != (Object)null)
				{
					component3.isKinematic = false;
					component3.useGravity = true;
				}
				Collider[] componentsInChildren = entity.GetComponentsInChildren<Collider>();
				foreach (Collider val in componentsInChildren)
				{
					if ((Object)(object)val != (Object)null)
					{
						val.enabled = true;
					}
				}
				Debug.Log((object)("[EntityManager] Initialized entity: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error initializing entity: " + ex.Message));
			}
		}

		public static void CleanupDestroyedEntities()
		{
			CheatConfig.SpawnedEntities.RemoveAll((GameObject entity) => (Object)(object)entity == (Object)null);
		}

		public static void KillEntity(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			try
			{
				if ((Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)(object)entity)
				{
					EntityControl.DisableControl(entity);
				}
				PhotonNetwork.Destroy(entity);
				CheatConfig.SpawnedEntities.Remove(entity);
				Debug.Log((object)("[EntityManager] Killed entity: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error killing entity: " + ex.Message));
			}
		}

		public static void TeleportEntityToMe(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null || (Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			try
			{
				Vector3 position = ((Component)Character.localCharacter).transform.position + Vector3.up * 1f;
				entity.transform.position = position;
				Debug.Log((object)("[EntityManager] Teleported entity to player: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error teleporting entity: " + ex.Message));
			}
		}

		public static void TeleportEntityToPlayer(GameObject entity, Character targetPlayer)
		{
			if ((Object)(object)entity == (Object)null || (Object)(object)targetPlayer == (Object)null)
			{
				return;
			}
			try
			{
				Vector3 position = ((Component)targetPlayer).transform.position + Vector3.up * 1f;
				entity.transform.position = position;
				Debug.Log((object)("[EntityManager] Teleported entity to player: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityManager] Error teleporting entity to player: " + ex.Message));
			}
		}

		public static List<MonoBehaviour> GetAIComponents(GameObject entity)
		{
			List<MonoBehaviour> list = new List<MonoBehaviour>();
			if ((Object)(object)entity == (Object)null)
			{
				return list;
			}
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (!((Object)(object)val == (Object)null))
				{
					string name = ((object)val).GetType().Name;
					if (name.Contains("AI") || name.Contains("Enemy") || name.Contains("Zombie") || name.Contains("Behavior") || name.Contains("Controller") || name.Contains("Agent") || name.Contains("StateMachine"))
					{
						list.Add(val);
					}
				}
			}
			return list;
		}

		private static void EnableAIComponents(GameObject entity)
		{
			foreach (MonoBehaviour aIComponent in GetAIComponents(entity))
			{
				if ((Object)(object)aIComponent != (Object)null)
				{
					((Behaviour)aIComponent).enabled = true;
				}
			}
		}

		private static void CallInitializationMethods(GameObject entity)
		{
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if ((Object)(object)val == (Object)null)
				{
					continue;
				}
				Type type = ((object)val).GetType();
				MethodInfo method = type.GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null && method.GetParameters().Length == 0)
				{
					try
					{
						method.Invoke(val, null);
					}
					catch
					{
					}
				}
				MethodInfo method2 = type.GetMethod("Activate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (method2 != null && method2.GetParameters().Length == 0)
				{
					try
					{
						method2.Invoke(val, null);
					}
					catch
					{
					}
				}
			}
		}
	}
}
