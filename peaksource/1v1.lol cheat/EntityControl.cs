using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class EntityControl
	{
		[CompilerGenerated]
		private sealed class <ResetJumpInput>d__45 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character character;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <ResetJumpInput>d__45(int <>1__state)
			{
				this.<>1__state = <>1__state;
			}

			[DebuggerHidden]
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
					<>2__current = null;
					<>1__state = 1;
					return true;
				case 1:
					<>1__state = -1;
					if ((Object)(object)character != (Object)null && (Object)(object)character.input != (Object)null)
					{
						character.input.jumpIsPressed = false;
					}
					return false;
				}
			}

			bool IEnumerator.MoveNext()
			{
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private static Dictionary<GameObject, List<MonoBehaviour>> _disabledAIComponentsByEntity = new Dictionary<GameObject, List<MonoBehaviour>>();

		private static Dictionary<Bodypart, bool> _playerBodypartKinematicStates = new Dictionary<Bodypart, bool>();

		private static Dictionary<Bodypart, RigidbodyConstraints> _playerBodypartConstraintStates = new Dictionary<Bodypart, RigidbodyConstraints>();

		private static Dictionary<GameObject, Dictionary<Renderer, bool>> _originalHeadRendererStates = new Dictionary<GameObject, Dictionary<Renderer, bool>>();

		private static GameObject _cameraOverrideObject = null;

		private static Vector3 _frozenPlayerPosition = Vector3.zero;

		private static bool _playerPositionFrozen = false;

		public static Vector3 FrozenPlayerPosition => _frozenPlayerPosition;

		public static bool PlayerPositionFrozen => _playerPositionFrozen;

		public static void EnableControl(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				Debug.LogError((object)"[EntityControl] Cannot enable control - entity is null");
				return;
			}
			if ((Object)(object)CheatConfig.CurrentlyControlledEntity != (Object)null && (Object)(object)CheatConfig.CurrentlyControlledEntity != (Object)(object)entity)
			{
				DisableControl(CheatConfig.CurrentlyControlledEntity);
			}
			try
			{
				Character component = entity.GetComponent<Character>();
				if ((Object)(object)component == (Object)null)
				{
					Debug.LogError((object)("[EntityControl] Entity " + ((Object)entity).name + " has no Character component"));
					return;
				}
				FreezePlayer();
				MakeHeadInvisible(entity, component);
				PrepareEntityForPlayerControl(entity);
				EnableEntityComponents(entity, component);
				TransferOwnership(entity);
				SetEntitySpectatable(component);
				CheatConfig.CurrentlyControlledEntity = entity;
				SwitchCameraToEntity(entity);
				Debug.Log((object)("[EntityControl] Successfully enabled control for entity: " + ((Object)entity).name));
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityControl] Failed to enable control: " + ex.Message));
			}
		}

		public static void DisableControl(GameObject entity)
		{
			if (!((Object)(object)entity == (Object)null) && (Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)(object)entity)
			{
				CheatConfig.CurrentlyControlledEntity = null;
				UnfreezePlayer();
				RestoreHeadVisibility(entity);
				RestoreEntityAI(entity);
				RestoreCameraToPlayer();
				Debug.Log((object)"[EntityControl] Disabled entity control");
			}
		}

		public static void ProcessInput(MonoBehaviour coroutineRunner, bool menuOpen)
		{
			if ((Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)null)
			{
				return;
			}
			GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
			if ((Object)(object)currentlyControlledEntity == (Object)null)
			{
				RestoreCameraToPlayer();
				DisableControl(null);
				return;
			}
			KeepPlayerFrozen();
			if (menuOpen)
			{
				return;
			}
			try
			{
				Character component = currentlyControlledEntity.GetComponent<Character>();
				if (!((Object)(object)component == (Object)null) && !((Object)(object)component.input == (Object)null) && !((Object)(object)component.data == (Object)null))
				{
					EnsureOwnership(currentlyControlledEntity);
					ProcessDirectMovementInput(component);
					ProcessMouseLookOnly(component);
					ProcessJumpInput(component, coroutineRunner);
					ProcessSprintInput(component);
					ProcessSpecialAbilities(currentlyControlledEntity, component);
					DisableAITargetFinding(currentlyControlledEntity, component);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityControl] Error processing input: " + ex.Message));
			}
		}

		private static void ProcessDirectMovementInput(Character charComponent)
		{
			if ((Object)(object)charComponent.input == (Object)null || (Object)(object)charComponent.data == (Object)null)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				zero.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				zero.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				zero.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				zero.x += 1f;
			}
			if (((Vector2)(ref zero)).magnitude > 1f)
			{
				((Vector2)(ref zero)).Normalize();
			}
			charComponent.input.movementInput = zero;
			if (((Vector2)(ref zero)).magnitude < 0.1f)
			{
				charComponent.input.movementInput = Vector2.zero;
			}
			if (!(((Vector2)(ref zero)).magnitude > 0.1f) || charComponent.data.isGrounded)
			{
				return;
			}
			Type type = Type.GetType("HelperFunctions");
			if (!(type != null))
			{
				return;
			}
			MethodInfo method = type.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public);
			if (!(method != null))
			{
				return;
			}
			Type type2 = Type.GetType("HelperFunctions+LayerType");
			if (!(type2 != null))
			{
				return;
			}
			object obj = Enum.Parse(type2, "TerrainMap");
			object obj2 = method.Invoke(null, new object[3]
			{
				charComponent.Center,
				charComponent.Center + Vector3.down * 2f,
				obj
			});
			if (obj2 != null)
			{
				PropertyInfo property = obj2.GetType().GetProperty("transform");
				if (property != null && property.GetValue(obj2) != null)
				{
					charComponent.data.isGrounded = true;
					charComponent.data.sinceGrounded = 0f;
				}
			}
		}

		private static void ProcessSprintInput(Character charComponent)
		{
			if (!((Object)(object)charComponent.input == (Object)null) && !((Object)(object)charComponent.data == (Object)null))
			{
				bool key = Input.GetKey((KeyCode)304);
				charComponent.input.sprintIsPressed = key;
				charComponent.data.isSprinting = key;
				if (key)
				{
					charComponent.input.sprintToggleIsPressed = true;
				}
			}
		}

		private static void DisableAITargetFinding(GameObject entity, Character charComponent)
		{
			if ((Object)(object)entity == (Object)null || (Object)(object)charComponent == (Object)null)
			{
				return;
			}
			try
			{
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if ((Object)(object)val == (Object)null)
					{
						continue;
					}
					Type type = ((object)val).GetType();
					string name = type.Name;
					if (name == "MushroomZombie")
					{
						PropertyInfo property = type.GetProperty("currentTarget");
						if (property != null && property.CanWrite)
						{
							property.SetValue(val, null);
						}
						PropertyInfo property2 = type.GetProperty("currentState");
						if (property2 != null)
						{
							Type nestedType = type.GetNestedType("State");
							if (nestedType != null)
							{
								object value = Enum.Parse(nestedType, "Chasing");
								property2.SetValue(val, value);
							}
						}
					}
					if (name == "Scoutmaster")
					{
						PropertyInfo property3 = type.GetProperty("currentTarget");
						if (property3 != null && property3.CanWrite)
						{
							property3.SetValue(val, null);
						}
					}
				}
			}
			catch
			{
			}
		}

		private static Vector3 CalculatePlayerTargetPosition(Character charComponent)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)Camera.main == (Object)null)
			{
				return charComponent.Center;
			}
			Vector2 val = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				val.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				val.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				val.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				val.x += 1f;
			}
			if (((Vector2)(ref val)).magnitude > 1f)
			{
				val = ((Vector2)(ref val)).normalized;
			}
			if (((Vector2)(ref val)).magnitude < 0.1f)
			{
				return charComponent.Center;
			}
			Vector3 forward = ((Component)Camera.main).transform.forward;
			Vector3 right = ((Component)Camera.main).transform.right;
			forward.y = 0f;
			right.y = 0f;
			((Vector3)(ref forward)).Normalize();
			((Vector3)(ref right)).Normalize();
			Vector3 val2 = forward * val.y + right * val.x;
			Vector3 normalized = ((Vector3)(ref val2)).normalized;
			float num = 5f;
			Vector3 result = charComponent.Center + normalized * num;
			result.y = charComponent.Center.y;
			return result;
		}

		private static void SetAITargetToPlayerInput(GameObject entity, Character charComponent, Vector3 targetPosition)
		{
			if ((Object)(object)charComponent == (Object)null)
			{
				return;
			}
			try
			{
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				bool flag = false;
				MonoBehaviour[] array = components;
				foreach (MonoBehaviour val in array)
				{
					if (!((Object)(object)val == (Object)null))
					{
						string name = ((object)val).GetType().Name;
						if (name == "MushroomZombie")
						{
							SetZombieAITarget(val, charComponent, targetPosition);
							flag = true;
							break;
						}
						if (name == "Scoutmaster")
						{
							SetScoutmasterAITarget(val, charComponent, targetPosition);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					ProcessMovementInputDirectly(charComponent, targetPosition);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Error setting AI target: " + ex.Message));
				ProcessMovementInputDirectly(charComponent, targetPosition);
			}
		}

		private static void SetZombieAITarget(MonoBehaviour zombieComponent, Character charComponent, Vector3 targetPosition)
		{
			try
			{
				Type type = ((object)zombieComponent).GetType();
				PropertyInfo property = type.GetProperty("currentTarget");
				if (property != null && property.CanWrite)
				{
					property.SetValue(zombieComponent, null);
				}
				PropertyInfo property2 = type.GetProperty("currentState");
				if (property2 == null)
				{
					return;
				}
				object value = property2.GetValue(zombieComponent);
				Type nestedType = type.GetNestedType("State");
				if (nestedType == null)
				{
					return;
				}
				object obj = Enum.Parse(nestedType, "Chasing");
				object obj2 = Enum.Parse(nestedType, "Idle");
				if (value == null || (!value.Equals(obj) && !value.Equals(obj2)))
				{
					object obj3 = Enum.Parse(nestedType, "Dead");
					object obj4 = Enum.Parse(nestedType, "Lunging");
					object obj5 = Enum.Parse(nestedType, "LungeRecovery");
					object obj6 = Enum.Parse(nestedType, "WakingUp");
					if (!value.Equals(obj3) && !value.Equals(obj4) && !value.Equals(obj5) && !value.Equals(obj6))
					{
						MethodInfo method = type.GetMethod("StartIdle", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method != null)
						{
							method.Invoke(zombieComponent, null);
						}
					}
				}
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
					charComponent.data.isSprinting = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Error setting zombie AI target: " + ex.Message));
			}
		}

		private static void SetScoutmasterAITarget(MonoBehaviour scoutmasterComponent, Character charComponent, Vector3 targetPosition)
		{
			try
			{
				PropertyInfo property = ((object)scoutmasterComponent).GetType().GetProperty("currentTarget");
				if (property != null && property.CanWrite)
				{
					property.SetValue(scoutmasterComponent, null);
				}
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Error setting scoutmaster AI target: " + ex.Message));
			}
		}

		private static void ProcessMovementInputDirectly(Character charComponent, Vector3 targetPosition)
		{
			if (!((Object)(object)charComponent == (Object)null) && !((Object)(object)charComponent.input == (Object)null))
			{
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
				}
			}
		}

		private static void MaintainEntityInputAfterAI(Character charComponent, Vector3 targetPosition)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)charComponent.input == (Object)null)
			{
				return;
			}
			Vector2 zero = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				zero.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				zero.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				zero.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				zero.x += 1f;
			}
			if (((Vector2)(ref zero)).magnitude > 0.1f)
			{
				EntityWalkTowards(charComponent, targetPosition);
				if (Input.GetKey((KeyCode)304))
				{
					charComponent.input.sprintIsPressed = true;
					charComponent.data.isSprinting = true;
				}
			}
		}

		public static void MaintainPlayerInputAfterAI(Character charComponent)
		{
			if (!((Object)(object)charComponent == (Object)null) && !((Object)(object)charComponent.input == (Object)null))
			{
				Vector3 targetPosition = CalculatePlayerTargetPosition(charComponent);
				MaintainEntityInputAfterAI(charComponent, targetPosition);
			}
		}

		private static void HelpZombieRecoverFromLunge(GameObject entity, Character charComponent)
		{
			MonoBehaviour component = (MonoBehaviour)(object)entity.GetComponent<MushroomZombie>();
			if ((Object)(object)component == (Object)null)
			{
				return;
			}
			try
			{
				Type type = ((object)component).GetType();
				if (type.Name != "MushroomZombie")
				{
					return;
				}
				PropertyInfo property = type.GetProperty("currentState");
				if (property == null)
				{
					return;
				}
				object value = property.GetValue(component);
				Type nestedType = type.GetNestedType("State");
				if (nestedType == null)
				{
					return;
				}
				object obj = Enum.Parse(nestedType, "LungeRecovery");
				object obj2 = Enum.Parse(nestedType, "Lunging");
				if (value == null || (!value.Equals(obj) && !value.Equals(obj2)))
				{
					return;
				}
				FieldInfo field = type.GetField("timeSpentRecoveringFromLunge", BindingFlags.Instance | BindingFlags.NonPublic);
				FieldInfo field2 = type.GetField("lungeRecoveryTime", BindingFlags.Instance | BindingFlags.Public);
				FieldInfo field3 = type.GetField("timeSpentLunging", BindingFlags.Instance | BindingFlags.NonPublic);
				FieldInfo field4 = type.GetField("lungeTime", BindingFlags.Instance | BindingFlags.Public);
				bool flag = false;
				if (value.Equals(obj))
				{
					if (field != null && field2 != null)
					{
						float num = (float)field.GetValue(component);
						_ = (float)field2.GetValue(component);
						if (num > 0.1f)
						{
							flag = true;
						}
					}
				}
				else if (value.Equals(obj2) && field3 != null && field4 != null)
				{
					float num2 = (float)field3.GetValue(component);
					float num3 = (float)field4.GetValue(component);
					if (num2 >= num3)
					{
						flag = true;
					}
				}
				if (flag)
				{
					charComponent.data.fallSeconds = 0f;
					charComponent.data.passedOut = false;
					charComponent.data.fullyPassedOut = false;
					charComponent.data.isGrounded = true;
					charComponent.data.sinceGrounded = 0f;
					if (field != null && field2 != null)
					{
						float num4 = (float)field2.GetValue(component);
						field.SetValue(component, num4 + 1f);
					}
					MethodInfo method = type.GetMethod("StartChasing", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(component, null);
						Debug.Log((object)"[EntityControl] Force recovered zombie from lunge");
					}
				}
			}
			catch
			{
			}
		}

		private static void ProcessSpecialAbilities(GameObject entity, Character charComponent)
		{
			if (Input.GetKeyDown((KeyCode)323) || Input.GetKeyDown((KeyCode)101))
			{
				TryZombieLunge(entity, charComponent);
			}
			if (Input.GetKey((KeyCode)324) || Input.GetKey((KeyCode)102))
			{
				TryScoutmasterThrow(entity, charComponent);
				return;
			}
			PhotonView component = entity.GetComponent<PhotonView>();
			if (!((Object)(object)component != (Object)null) || !component.IsMine || !charComponent.data.isReaching)
			{
				return;
			}
			FieldInfo field = typeof(CharacterData).GetField("grabbedPlayer", BindingFlags.Instance | BindingFlags.NonPublic);
			Character val = null;
			if (field != null)
			{
				object value = field.GetValue(charComponent.data);
				val = (Character)((value is Character) ? value : null);
			}
			if ((Object)(object)val == (Object)null)
			{
				component.RPC("RPCA_StopReaching", (RpcTarget)0, Array.Empty<object>());
				FieldInfo field2 = typeof(CharacterData).GetField("sincePressReach", BindingFlags.Instance | BindingFlags.Public);
				if (field2 != null)
				{
					field2.SetValue(charComponent.data, 10f);
				}
			}
		}

		private static void TryZombieLunge(GameObject entity, Character charComponent)
		{
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			MonoBehaviour val = null;
			Type type = null;
			MonoBehaviour[] array = components;
			foreach (MonoBehaviour val2 in array)
			{
				if ((Object)(object)val2 != (Object)null && ((object)val2).GetType().Name == "MushroomZombie")
				{
					val = val2;
					type = ((object)val2).GetType();
					break;
				}
			}
			if ((Object)(object)val == (Object)null || type == null)
			{
				return;
			}
			try
			{
				PropertyInfo property = type.GetProperty("currentState");
				if (property == null || !charComponent.data.isGrounded)
				{
					return;
				}
				object value = property.GetValue(val);
				Type nestedType = type.GetNestedType("State");
				PropertyInfo property2 = type.GetProperty("currentTarget");
				Character val3 = null;
				if (property2 != null)
				{
					object value2 = property2.GetValue(val);
					val3 = (Character)((value2 is Character) ? value2 : null);
					if ((Object)(object)val3 == (Object)null)
					{
						val3 = GetNearestPlayer(charComponent, 50f, includeBots: true);
						if ((Object)(object)val3 != (Object)null)
						{
							MethodInfo method = type.GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							if (method != null)
							{
								method.Invoke(val, new object[2] { val3, 0f });
							}
						}
					}
				}
				if (!((Object)(object)val3 != (Object)null) || !(nestedType != null))
				{
					return;
				}
				object obj = Enum.Parse(nestedType, "Chasing");
				if (value == null || !value.Equals(obj))
				{
					MethodInfo method2 = type.GetMethod("StartChasing", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method2 != null)
					{
						method2.Invoke(val, null);
					}
				}
				float num = Vector3.Distance(charComponent.Center, val3.Center);
				float num2 = 10f;
				FieldInfo field = type.GetField("zombieLungeDistance", BindingFlags.Instance | BindingFlags.Public);
				if (field != null)
				{
					num2 = (float)field.GetValue(val);
				}
				bool flag = EntityCanSeeTarget(charComponent, val3);
				float num3 = 3f;
				FieldInfo field2 = type.GetField("chaseTimeBeforeSprint", BindingFlags.Instance | BindingFlags.Public);
				if (field2 != null)
				{
					num3 = (float)field2.GetValue(val);
				}
				float num4 = 0f;
				FieldInfo field3 = type.GetField("timeSpentChasing", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field3 != null)
				{
					num4 = (float)field3.GetValue(val);
				}
				bool flag2 = num4 > num3;
				if (num <= num2 * 1.5f && flag)
				{
					object value3 = property.GetValue(val);
					if (value3 == null || !value3.Equals(obj))
					{
						MethodInfo method3 = type.GetMethod("StartChasing", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method3 != null)
						{
							method3.Invoke(val, null);
						}
					}
					charComponent.input.sprintIsPressed = true;
					charComponent.data.isSprinting = true;
					EntityLookAt(charComponent, val3.Head);
					MethodInfo method4 = type.GetMethod("StartLunging", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method4 != null)
					{
						method4.Invoke(val, null);
						Debug.Log((object)$"[EntityControl] Triggered zombie lunge at distance {num:F1}m");
						return;
					}
					object value4 = Enum.Parse(nestedType, "Lunging");
					property.SetValue(val, value4);
					FieldInfo field4 = type.GetField("lungeTargetForward", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field4 != null)
					{
						Vector3 val4 = charComponent.Center + (val3.Head - charComponent.Center) * 100f;
						field4.SetValue(val, val4);
					}
					charComponent.input.jumpWasPressed = true;
					Debug.Log((object)$"[EntityControl] Manually triggered zombie lunge state at distance {num:F1}m");
				}
				else if (num > num2 * 1.5f)
				{
					if (charComponent.data.isClimbing)
					{
						EntityClimbTowards(charComponent, val3.Head);
					}
					else
					{
						EntityWalkTowards(charComponent, val3.Head);
					}
					float num5 = 20f;
					FieldInfo field5 = type.GetField("zombieSprintDistance", BindingFlags.Instance | BindingFlags.Public);
					if (field5 != null)
					{
						num5 = (float)field5.GetValue(val);
					}
					if (flag2 && num < num5 && flag)
					{
						charComponent.input.sprintIsPressed = true;
						charComponent.data.isSprinting = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Could not trigger zombie lunge: " + ex.Message));
			}
		}

		private static void TryScoutmasterThrow(GameObject entity, Character charComponent)
		{
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			MonoBehaviour val = null;
			Type type = null;
			MonoBehaviour[] array = components;
			foreach (MonoBehaviour val2 in array)
			{
				if ((Object)(object)val2 != (Object)null && ((object)val2).GetType().Name == "Scoutmaster")
				{
					val = val2;
					type = ((object)val2).GetType();
					break;
				}
			}
			if ((Object)(object)val == (Object)null || type == null)
			{
				return;
			}
			try
			{
				PhotonView component = entity.GetComponent<PhotonView>();
				if ((Object)(object)component == (Object)null || !component.IsMine)
				{
					return;
				}
				FieldInfo field = typeof(CharacterData).GetField("grabbedPlayer", BindingFlags.Instance | BindingFlags.NonPublic);
				Character val3 = null;
				if (field != null)
				{
					object value = field.GetValue(charComponent.data);
					val3 = (Character)((value is Character) ? value : null);
				}
				if ((Object)(object)val3 != (Object)null)
				{
					if ((Object)(object)val3.data != (Object)null)
					{
						val3.data.sinceGrounded = 0f;
					}
					charComponent.input.useSecondaryIsPressed = true;
					Vector3 lookDirection = charComponent.data.lookDirection;
					lookDirection.y = 0.6f;
					((Vector3)(ref lookDirection)).Normalize();
					Vector3 lookAtPos = charComponent.Head + lookDirection * 10f;
					EntityLookAt(charComponent, lookAtPos);
					FieldInfo field2 = type.GetField("isThrowing", BindingFlags.Instance | BindingFlags.NonPublic);
					bool flag = false;
					if (field2 != null)
					{
						flag = (bool)field2.GetValue(val);
					}
					if (!flag)
					{
						component.RPC("RPCA_Throw", (RpcTarget)0, Array.Empty<object>());
						Debug.Log((object)"[EntityControl] Triggered scoutmaster throw");
					}
					return;
				}
				Character nearestPlayer = GetNearestPlayer(charComponent, 3.5f, includeBots: true);
				if ((Object)(object)nearestPlayer != (Object)null)
				{
					MethodInfo method = type.GetMethod("SetCurrentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(val, new object[2] { nearestPlayer, 5f });
					}
					EntityLookAt(charComponent, nearestPlayer.Head);
					float num = Vector3.Distance(charComponent.Center, nearestPlayer.Center);
					if (charComponent.data.isGrounded && !charComponent.data.isClimbing && num < 3.5f)
					{
						FieldInfo field3 = typeof(CharacterData).GetField("sincePressReach", BindingFlags.Instance | BindingFlags.Public);
						if (field3 != null)
						{
							field3.SetValue(charComponent.data, 0f);
						}
						charComponent.input.useSecondaryIsPressed = true;
						if (!charComponent.data.isReaching)
						{
							component.RPC("RPCA_StartReaching", (RpcTarget)0, Array.Empty<object>());
						}
						Debug.Log((object)$"[EntityControl] Scoutmaster grab active - target at distance {num:F1}m");
					}
					else
					{
						if (charComponent.data.isClimbing)
						{
							EntityClimbTowards(charComponent, nearestPlayer.Head);
						}
						else
						{
							EntityWalkTowards(charComponent, nearestPlayer.Head);
						}
						if (num < 15f)
						{
							charComponent.input.sprintIsPressed = true;
						}
					}
				}
				else
				{
					FieldInfo field4 = typeof(CharacterData).GetField("sincePressReach", BindingFlags.Instance | BindingFlags.Public);
					if (field4 != null)
					{
						field4.SetValue(charComponent.data, 0f);
					}
					charComponent.input.useSecondaryIsPressed = true;
					if (!charComponent.data.isReaching)
					{
						component.RPC("RPCA_StartReaching", (RpcTarget)0, Array.Empty<object>());
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] Could not trigger scoutmaster throw: " + ex.Message));
			}
		}

		private static Character GetNearestPlayer(Character entityChar, float maxDistance = 50f, bool includeBots = false)
		{
			Character result = null;
			float num = float.MaxValue;
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (!((Object)(object)allCharacter == (Object)(object)entityChar) && !((Object)(object)allCharacter == (Object)(object)Character.localCharacter) && !((Object)(object)allCharacter.data == (Object)null) && !allCharacter.data.dead && !allCharacter.data.fullyPassedOut && (includeBots || !allCharacter.isBot))
				{
					float num2 = Vector3.Distance(entityChar.Center, allCharacter.Center);
					if (num2 < num && num2 <= maxDistance)
					{
						result = allCharacter;
						num = num2;
					}
				}
			}
			return result;
		}

		public static void UpdateCamera()
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			Character controlledEntityCharacter = GetControlledEntityCharacter();
			if ((Object)(object)controlledEntityCharacter == (Object)null || (Object)(object)controlledEntityCharacter.data == (Object)null || controlledEntityCharacter.refs == null)
			{
				return;
			}
			try
			{
				Vector3 zero = Vector3.zero;
				if (controlledEntityCharacter.refs != null && (Object)(object)controlledEntityCharacter.refs.head != (Object)null)
				{
					float num = -0.3f;
					zero = ((Component)controlledEntityCharacter.refs.head).transform.TransformPoint(Vector3.up * 0.1f + Vector3.forward * num);
				}
				else
				{
					zero = ((Component)controlledEntityCharacter).transform.position + Vector3.up * 1.6f;
				}
				Quaternion cameraRot = ((controlledEntityCharacter.refs == null || !((Object)(object)controlledEntityCharacter.refs.head != (Object)null)) ? Quaternion.LookRotation(controlledEntityCharacter.data.lookDirection) : ((Component)controlledEntityCharacter.refs.head).transform.rotation);
				ApplyGameFeelRotation(ref cameraRot);
				((Component)Camera.main).transform.position = zero;
				((Component)Camera.main).transform.rotation = cameraRot;
				UpdateCameraOverride(zero, cameraRot);
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[EntityControl] Error updating camera: " + ex.Message));
			}
		}

		private static void FreezePlayer()
		{
			if ((Object)(object)Character.localCharacter == (Object)null || Character.localCharacter.refs == null)
			{
				return;
			}
			if (!_playerPositionFrozen)
			{
				_frozenPlayerPosition = ((Component)Character.localCharacter).transform.position;
				_playerPositionFrozen = true;
			}
			if ((Object)(object)Character.localCharacter.refs.ragdoll != (Object)null && Character.localCharacter.refs.ragdoll.partList != null)
			{
				if (_playerBodypartKinematicStates.Count == 0)
				{
					foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
					{
						if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
						{
							_playerBodypartKinematicStates[part] = part.Rig.isKinematic;
							_playerBodypartConstraintStates[part] = part.Rig.constraints;
						}
					}
				}
				foreach (Bodypart part2 in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)part2 != (Object)null && (Object)(object)part2.Rig != (Object)null)
					{
						part2.Rig.isKinematic = true;
						part2.Rig.linearVelocity = Vector3.zero;
						part2.Rig.angularVelocity = Vector3.zero;
					}
				}
			}
			MainCameraMovement instance = Singleton<MainCameraMovement>.Instance;
			if ((Object)(object)instance != (Object)null)
			{
				FieldInfo field = typeof(MainCameraMovement).GetField("isGodCam", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(instance, true);
				}
			}
			if ((Object)(object)Character.localCharacter.refs.movement != (Object)null)
			{
				((Behaviour)Character.localCharacter.refs.movement).enabled = false;
			}
		}

		private static void KeepPlayerFrozen()
		{
			if (!_playerPositionFrozen || (Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			((Component)Character.localCharacter).transform.position = _frozenPlayerPosition;
			if (Character.localCharacter.refs?.ragdoll?.partList != null)
			{
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)((part != null) ? part.Rig : null) != (Object)null)
					{
						part.Rig.isKinematic = true;
						part.Rig.linearVelocity = Vector3.zero;
						part.Rig.angularVelocity = Vector3.zero;
					}
				}
			}
			if ((Object)(object)Character.localCharacter.data != (Object)null)
			{
				Character.localCharacter.data.isGrounded = true;
				Character.localCharacter.data.sinceGrounded = 0f;
			}
			if ((Object)(object)Character.localCharacter.input != (Object)null)
			{
				Character.localCharacter.input.movementInput = Vector2.zero;
				Character.localCharacter.input.lookInput = Vector2.zero;
				Character.localCharacter.input.jumpIsPressed = false;
				Character.localCharacter.input.jumpWasPressed = false;
				Character.localCharacter.input.sprintIsPressed = false;
				Character.localCharacter.input.sprintWasPressed = false;
				Character.localCharacter.input.usePrimaryIsPressed = false;
				Character.localCharacter.input.useSecondaryIsPressed = false;
				Character.localCharacter.input.interactIsPressed = false;
				Character.localCharacter.input.dropIsPressed = false;
			}
		}

		private static void UnfreezePlayer()
		{
			if ((Object)(object)Character.localCharacter == (Object)null || Character.localCharacter.refs == null)
			{
				return;
			}
			if ((Object)(object)Character.localCharacter.refs.ragdoll != (Object)null && Character.localCharacter.refs.ragdoll.partList != null)
			{
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
					{
						if (_playerBodypartKinematicStates.ContainsKey(part))
						{
							part.Rig.isKinematic = _playerBodypartKinematicStates[part];
						}
						if (_playerBodypartConstraintStates.ContainsKey(part))
						{
							part.Rig.constraints = _playerBodypartConstraintStates[part];
						}
					}
				}
				_playerBodypartKinematicStates.Clear();
				_playerBodypartConstraintStates.Clear();
			}
			_playerPositionFrozen = false;
			if ((Object)(object)Character.localCharacter.refs.movement != (Object)null)
			{
				((Behaviour)Character.localCharacter.refs.movement).enabled = true;
			}
			MainCameraMovement instance = Singleton<MainCameraMovement>.Instance;
			if ((Object)(object)instance != (Object)null)
			{
				FieldInfo field = typeof(MainCameraMovement).GetField("isGodCam", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(instance, false);
				}
			}
		}

		private static void MakeHeadInvisible(GameObject entity, Character charComponent)
		{
			if ((Object)(object)charComponent.refs?.head == (Object)null)
			{
				return;
			}
			if (!_originalHeadRendererStates.ContainsKey(entity))
			{
				_originalHeadRendererStates[entity] = new Dictionary<Renderer, bool>();
			}
			Renderer[] componentsInChildren = ((Component)charComponent.refs.head).gameObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if ((Object)(object)val != (Object)null && !_originalHeadRendererStates[entity].ContainsKey(val))
				{
					_originalHeadRendererStates[entity][val] = val.enabled;
					val.enabled = false;
				}
			}
			SkinnedMeshRenderer[] componentsInChildren2 = ((Component)charComponent.refs.head).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer val2 in componentsInChildren2)
			{
				if ((Object)(object)val2 != (Object)null && !_originalHeadRendererStates[entity].ContainsKey((Renderer)(object)val2))
				{
					_originalHeadRendererStates[entity][(Renderer)(object)val2] = ((Renderer)val2).enabled;
					((Renderer)val2).enabled = false;
				}
			}
		}

		private static void RestoreHeadVisibility(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			Character component = entity.GetComponent<Character>();
			if ((Object)(object)component?.refs?.head == (Object)null)
			{
				if (_originalHeadRendererStates.ContainsKey(entity))
				{
					_originalHeadRendererStates.Remove(entity);
				}
				return;
			}
			if (_originalHeadRendererStates.ContainsKey(entity))
			{
				foreach (KeyValuePair<Renderer, bool> item in _originalHeadRendererStates[entity])
				{
					if ((Object)(object)item.Key != (Object)null)
					{
						try
						{
							item.Key.enabled = item.Value;
						}
						catch
						{
						}
					}
				}
				_originalHeadRendererStates.Remove(entity);
				return;
			}
			Renderer[] componentsInChildren = ((Component)component.refs.head).gameObject.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if ((Object)(object)val != (Object)null)
				{
					try
					{
						val.enabled = true;
					}
					catch
					{
					}
				}
			}
			SkinnedMeshRenderer[] componentsInChildren2 = ((Component)component.refs.head).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer val2 in componentsInChildren2)
			{
				if ((Object)(object)val2 != (Object)null)
				{
					try
					{
						((Renderer)val2).enabled = true;
					}
					catch
					{
					}
				}
			}
		}

		private static void PrepareEntityForPlayerControl(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return;
			}
			MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (!((Object)(object)val == (Object)null))
				{
					string name = ((object)val).GetType().Name;
					if (name == "Scoutmaster" || name == "MushroomZombie")
					{
						Debug.Log((object)("[EntityControl] " + name + " component kept enabled - will use AI logic with player input"));
					}
				}
			}
			if (!_disabledAIComponentsByEntity.ContainsKey(entity))
			{
				_disabledAIComponentsByEntity[entity] = new List<MonoBehaviour>();
			}
		}

		private static void RestoreEntityAI(GameObject entity)
		{
			if (!((Object)(object)entity == (Object)null))
			{
				if (_disabledAIComponentsByEntity.ContainsKey(entity))
				{
					_disabledAIComponentsByEntity.Remove(entity);
				}
				Debug.Log((object)"[EntityControl] Entity AI restored to normal operation");
			}
		}

		private static void EnableEntityComponents(GameObject entity, Character charComponent)
		{
			CharacterMovement component = entity.GetComponent<CharacterMovement>();
			if ((Object)(object)component != (Object)null && !((Behaviour)component).enabled)
			{
				((Behaviour)component).enabled = true;
			}
			if (!((Behaviour)charComponent).enabled)
			{
				((Behaviour)charComponent).enabled = true;
			}
		}

		private static void TransferOwnership(GameObject entity)
		{
			PhotonView component = entity.GetComponent<PhotonView>();
			if ((Object)(object)component != (Object)null && !component.IsMine)
			{
				try
				{
					component.TransferOwnership(PhotonNetwork.LocalPlayer);
				}
				catch
				{
				}
			}
		}

		private static void SetEntitySpectatable(Character charComponent)
		{
			if (!((Object)(object)charComponent.data == (Object)null))
			{
				PropertyInfo property = typeof(CharacterData).GetProperty("canBeSpectated", BindingFlags.Instance | BindingFlags.Public);
				if (property != null && property.CanWrite)
				{
					property.SetValue(charComponent.data, true);
				}
			}
		}

		private static void SwitchCameraToEntity(GameObject entity)
		{
			if (!((Object)(object)entity.GetComponent<Character>() == (Object)null))
			{
				MainCamera instance = MainCamera.instance;
				if ((Object)(object)instance != (Object)null)
				{
					GameObject val = new GameObject("EntityCameraOverride");
					CameraOverride cameraOverride = val.AddComponent<CameraOverride>();
					instance.SetCameraOverride(cameraOverride);
					_cameraOverrideObject = val;
				}
			}
		}

		private static void RestoreCameraToPlayer()
		{
			MainCamera instance = MainCamera.instance;
			if ((Object)(object)instance != (Object)null)
			{
				instance.SetCameraOverride((CameraOverride)null);
			}
			if ((Object)(object)_cameraOverrideObject != (Object)null)
			{
				Object.Destroy((Object)(object)_cameraOverrideObject);
				_cameraOverrideObject = null;
			}
		}

		private static void EnsureOwnership(GameObject entity)
		{
			PhotonView component = entity.GetComponent<PhotonView>();
			if ((Object)(object)component != (Object)null && !component.IsMine)
			{
				try
				{
					component.TransferOwnership(PhotonNetwork.LocalPlayer);
				}
				catch
				{
				}
			}
		}

		private static void ProcessMovementInput(Character charComponent)
		{
			Vector2 val = Vector2.zero;
			if (Input.GetKey((KeyCode)119))
			{
				val.y += 1f;
			}
			if (Input.GetKey((KeyCode)115))
			{
				val.y -= 1f;
			}
			if (Input.GetKey((KeyCode)97))
			{
				val.x -= 1f;
			}
			if (Input.GetKey((KeyCode)100))
			{
				val.x += 1f;
			}
			if (((Vector2)(ref val)).magnitude > 1f)
			{
				val = ((Vector2)(ref val)).normalized;
			}
			charComponent.input.movementInput = val;
			if ((Object)(object)charComponent.refs?.climbing != (Object)null && val.y > 0.1f)
			{
				try
				{
					MethodInfo method = ((object)charComponent.refs.climbing).GetType().GetMethod("TryClimb", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(charComponent.refs.climbing, new object[1] { 1.25f });
					}
				}
				catch
				{
				}
			}
			if ((Object)(object)charComponent.data != (Object)null && charComponent.data.isClimbing && Input.GetKey((KeyCode)304) && ((Vector2)(ref val)).magnitude > 0.1f)
			{
				charComponent.input.sprintIsPressed = true;
				charComponent.input.sprintToggleIsPressed = true;
				charComponent.input.sprintWasPressed = true;
				charComponent.input.sprintToggleWasPressed = true;
			}
		}

		private static void ProcessJumpInput(Character charComponent, MonoBehaviour coroutineRunner)
		{
			if (Input.GetKeyDown((KeyCode)32))
			{
				if (!charComponent.input.jumpIsPressed && !charComponent.input.jumpWasPressed)
				{
					charComponent.input.jumpWasPressed = true;
					charComponent.input.jumpIsPressed = true;
					coroutineRunner.StartCoroutine(ResetJumpInput(charComponent));
				}
			}
			else if (Input.GetKeyUp((KeyCode)32))
			{
				charComponent.input.jumpIsPressed = false;
				charComponent.input.jumpWasPressed = false;
			}
		}

		[IteratorStateMachine(typeof(<ResetJumpInput>d__45))]
		private static IEnumerator ResetJumpInput(Character character)
		{
			return new <ResetJumpInput>d__45(0)
			{
				character = character
			};
		}

		public static void ProcessMouseLookOnly(Character charComponent)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)charComponent.input == (Object)null || (Object)(object)charComponent.data == (Object)null)
			{
				return;
			}
			float axis = Input.GetAxis("Mouse X");
			float axis2 = Input.GetAxis("Mouse Y");
			charComponent.input.lookInput = new Vector2(axis, axis2);
			if (!((Object)(object)charComponent.refs?.movement != (Object)null))
			{
				return;
			}
			CharacterMovement movement = charComponent.refs.movement;
			float num = 0.1f;
			int num2 = 1;
			int num3 = 1;
			try
			{
				FieldInfo field = typeof(CharacterMovement).GetField("mouseSensSetting", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					object value = field.GetValue(movement);
					if (value != null)
					{
						PropertyInfo property = value.GetType().GetProperty("Value");
						if (property != null)
						{
							float num4 = (float)property.GetValue(value);
							num = 0.1f * num4;
						}
					}
				}
				FieldInfo field2 = typeof(CharacterMovement).GetField("invertXSetting", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field2 != null)
				{
					object value2 = field2.GetValue(movement);
					if (value2 != null)
					{
						PropertyInfo property2 = value2.GetType().GetProperty("Value");
						if (property2 != null)
						{
							object value3 = property2.GetValue(value2);
							if (value3 != null && value3.ToString() != "OFF" && value3.ToString() != "0")
							{
								num2 = -1;
							}
						}
					}
				}
				FieldInfo field3 = typeof(CharacterMovement).GetField("invertYSetting", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field3 != null)
				{
					object value4 = field3.GetValue(movement);
					if (value4 != null)
					{
						PropertyInfo property3 = value4.GetType().GetProperty("Value");
						if (property3 != null)
						{
							object value5 = property3.GetValue(value4);
							if (value5 != null && value5.ToString() != "OFF" && value5.ToString() != "0")
							{
								num3 = -1;
							}
						}
					}
				}
			}
			catch
			{
			}
			Vector2 lookInput = charComponent.input.lookInput;
			if (((Vector2)(ref lookInput)).magnitude > 0.01f)
			{
				charComponent.data.lookValues.x += lookInput.x * num * (float)num2;
				charComponent.data.lookValues.y += lookInput.y * num * (float)num3;
				charComponent.data.lookValues.y = Mathf.Clamp(charComponent.data.lookValues.y, -85f, 85f);
				MethodInfo method = typeof(Character).GetMethod("RecalculateLookDirections", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(charComponent, null);
				}
			}
		}

		private static void ProcessMouseLook(Character charComponent)
		{
			ProcessMouseLookOnly(charComponent);
		}

		private static void EntityLookAt(Character charComponent, Vector3 lookAtPos)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)charComponent.data == (Object)null || charComponent.refs == null)
			{
				return;
			}
			try
			{
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("DirectionToLook", BindingFlags.Static | BindingFlags.Public, null, new Type[1] { typeof(Vector3) }, null);
					if (method != null)
					{
						Vector3 val = lookAtPos - charComponent.Head;
						Vector2 lookValues = (Vector2)method.Invoke(null, new object[1] { val });
						charComponent.data.lookValues = lookValues;
						MethodInfo method2 = typeof(Character).GetMethod("RecalculateLookDirections", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method2 != null)
						{
							method2.Invoke(charComponent, null);
						}
						return;
					}
				}
				Vector3 val2 = lookAtPos - charComponent.Head;
				Vector3 normalized = ((Vector3)(ref val2)).normalized;
				float num = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
				float num2 = (0f - Mathf.Asin(normalized.y)) * 57.29578f;
				charComponent.data.lookValues = new Vector2(num, num2);
				MethodInfo method3 = typeof(Character).GetMethod("RecalculateLookDirections", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method3 != null)
				{
					method3.Invoke(charComponent, null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityLookAt failed: " + ex.Message));
			}
		}

		private static void EntityWalkTowards(Character charComponent, Vector3 targetPos, float mult = 1f, bool tryClimb = true, bool tryJump = true)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)charComponent.input == (Object)null || (Object)(object)charComponent.data == (Object)null)
			{
				return;
			}
			try
			{
				EntityLookAt(charComponent, targetPos);
				float num = 0f;
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("FlatDistance", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
					{
						typeof(Vector3),
						typeof(Vector3)
					}, null);
					if (method != null)
					{
						num = (float)method.Invoke(null, new object[2] { charComponent.Center, targetPos });
					}
				}
				else
				{
					Vector3 val = targetPos - charComponent.Center;
					val.y = 0f;
					num = ((Vector3)(ref val)).magnitude;
				}
				if (Vector3.Distance(charComponent.Center, targetPos) < 5f)
				{
					if (num < 2.5f)
					{
						mult *= 0f;
					}
					else if (num < 1.5f)
					{
						mult *= -1f;
					}
				}
				charComponent.input.movementInput = new Vector2(0f, mult);
				if (tryClimb && (Object)(object)charComponent.refs?.climbing != (Object)null)
				{
					MethodInfo method2 = ((object)charComponent.refs.climbing).GetType().GetMethod("TryClimb", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (method2 != null)
					{
						method2.Invoke(charComponent.refs.climbing, new object[1] { 1.25f });
					}
				}
				if (!tryJump || !((Object)(object)charComponent.data != (Object)null))
				{
					return;
				}
				Type type2 = Type.GetType("HelperFunctions");
				if (!(type2 != null))
				{
					return;
				}
				MethodInfo method3 = type2.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public);
				if (!(method3 != null))
				{
					return;
				}
				Type type3 = Type.GetType("HelperFunctions+LayerType");
				if (!(type3 != null))
				{
					return;
				}
				object obj = Enum.Parse(type3, "TerrainMap");
				object obj2 = method3.Invoke(null, new object[3]
				{
					charComponent.Center,
					charComponent.Center + Vector3.down * 3f,
					obj
				});
				if (obj2 != null)
				{
					PropertyInfo property = obj2.GetType().GetProperty("transform");
					if (property != null && property.GetValue(obj2) == null)
					{
						charComponent.input.jumpWasPressed = true;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityWalkTowards failed: " + ex.Message));
			}
		}

		private static void EntityClimbTowards(Character charComponent, Vector3 targetPos, float mult = 1f)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)charComponent.input == (Object)null || (Object)(object)charComponent.data == (Object)null)
			{
				return;
			}
			try
			{
				EntityLookAt(charComponent, targetPos);
				MethodInfo method = typeof(Character).GetMethod("GetBodypart", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[1] { typeof(BodypartType) }, null);
				if (method != null)
				{
					object obj = method.Invoke(charComponent, new object[1] { (object)(BodypartType)2 });
					Bodypart val = (Bodypart)((obj is Bodypart) ? obj : null);
					if ((Object)(object)val != (Object)null && (Object)(object)((Component)val).transform != (Object)null)
					{
						float num = Mathf.Clamp(((Component)val).transform.InverseTransformPoint(targetPos).x * 0.25f, -1f, 1f);
						charComponent.input.movementInput = new Vector2(num, mult);
					}
				}
				else if (charComponent.refs != null && (Object)(object)charComponent.refs.ragdoll != (Object)null)
				{
					foreach (Bodypart part in charComponent.refs.ragdoll.partList)
					{
						if ((Object)(object)part != (Object)null && (int)part.partType == 2 && (Object)(object)((Component)part).transform != (Object)null)
						{
							float num2 = Mathf.Clamp(((Component)part).transform.InverseTransformPoint(targetPos).x * 0.25f, -1f, 1f);
							charComponent.input.movementInput = new Vector2(num2, mult);
							break;
						}
					}
				}
				if ((Object)(object)charComponent.data != (Object)null)
				{
					charComponent.data.currentStamina = 1f;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityClimbTowards failed: " + ex.Message));
			}
		}

		private static bool EntityCanSeeTarget(Character charComponent, Character target)
		{
			if ((Object)(object)charComponent == (Object)null || (Object)(object)target == (Object)null)
			{
				return false;
			}
			try
			{
				Type type = Type.GetType("HelperFunctions");
				if (type != null)
				{
					MethodInfo method = type.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public);
					if (method != null)
					{
						Type type2 = Type.GetType("HelperFunctions+LayerType");
						if (type2 != null)
						{
							object obj = Enum.Parse(type2, "TerrainMap");
							Vector3 val = target.Center + Random.insideUnitSphere * 0.5f;
							object obj2 = method.Invoke(null, new object[3] { charComponent.Head, val, obj });
							if (obj2 != null)
							{
								PropertyInfo property = obj2.GetType().GetProperty("transform");
								if (property != null)
								{
									return property.GetValue(obj2) == null;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[EntityControl] EntityCanSeeTarget failed: " + ex.Message));
			}
			return false;
		}

		private static void ApplyGameFeelRotation(ref Quaternion cameraRot)
		{
			try
			{
				GamefeelHandler instance = GamefeelHandler.instance;
				if ((Object)(object)instance != (Object)null)
				{
					FieldInfo field = typeof(GamefeelHandler).GetField("rotation", BindingFlags.Instance | BindingFlags.Public);
					if (field != null)
					{
						Vector3 val = (Vector3)field.GetValue(instance);
						cameraRot *= Quaternion.Euler(val);
					}
				}
			}
			catch
			{
			}
		}

		private static void UpdateCameraOverride(Vector3 position, Quaternion rotation)
		{
			MainCamera instance = MainCamera.instance;
			if ((Object)(object)instance == (Object)null)
			{
				return;
			}
			FieldInfo field = typeof(MainCamera).GetField("camOverride", BindingFlags.Instance | BindingFlags.NonPublic);
			if (!(field != null))
			{
				return;
			}
			CameraOverride val = (CameraOverride)field.GetValue(instance);
			if (!((Object)(object)val != (Object)null))
			{
				return;
			}
			((Component)val).transform.position = position;
			((Component)val).transform.rotation = rotation;
			try
			{
				MainCameraMovement instance2 = Singleton<MainCameraMovement>.Instance;
				if ((Object)(object)instance2 != (Object)null)
				{
					MethodInfo method = typeof(MainCameraMovement).GetMethod("GetFov", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						float fieldOfView = (val.fov = (float)method.Invoke(instance2, null));
						Camera.main.fieldOfView = fieldOfView;
					}
				}
			}
			catch
			{
			}
		}

		public static bool IsControllingEntity()
		{
			return (Object)(object)CheatConfig.CurrentlyControlledEntity != (Object)null;
		}

		public static Character GetControlledEntityCharacter()
		{
			if ((Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)null)
			{
				return null;
			}
			return CheatConfig.CurrentlyControlledEntity.GetComponent<Character>();
		}
	}
}
