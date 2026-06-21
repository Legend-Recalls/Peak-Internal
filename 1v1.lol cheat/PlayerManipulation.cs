using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Debug = UnityEngine.Debug;

namespace _1v1.lol_cheat.Troll
{
	public static class PlayerManipulation
	{
		[CompilerGenerated]
		private sealed class <AntigravCoroutine>d__26 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			public float duration;

			private float <elapsed>5__2;

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
			public <AntigravCoroutine>d__26(int <>1__state)
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
					<elapsed>5__2 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && (Object)(object)target != (Object)null && !target.data.dead)
				{
					try
					{
						MethodInfo method = typeof(Character).GetMethod("AddForce", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
						{
							typeof(Vector3),
							typeof(float),
							typeof(float)
						}, null);
						if (method != null)
						{
							method.Invoke(target, new object[3]
							{
								Vector3.up * 9.81f * 0.5f,
								1f,
								1f
							});
						}
						else if (target.refs != null && (Object)(object)target.refs.ragdoll != (Object)null)
						{
							foreach (Bodypart part in target.refs.ragdoll.partList)
							{
								if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
								{
									part.Rig.AddForce(Vector3.up * 9.81f * 0.5f, (ForceMode)5);
								}
							}
						}
					}
					catch
					{
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				return false;
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

		[CompilerGenerated]
		private sealed class <FreezePlayerCoroutine>d__31 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			public float duration;

			private float <elapsed>5__2;

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
			public <FreezePlayerCoroutine>d__31(int <>1__state)
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
					<elapsed>5__2 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && (Object)(object)target != (Object)null && !target.data.dead)
				{
					try
					{
						if ((Object)(object)target.input != (Object)null)
						{
							target.input.movementInput = Vector2.zero;
							target.input.jumpIsPressed = false;
							target.input.sprintIsPressed = false;
						}
						if (target.refs != null && (Object)(object)target.refs.ragdoll != (Object)null)
						{
							foreach (Bodypart part in target.refs.ragdoll.partList)
							{
								if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
								{
									part.Rig.linearVelocity = Vector3.zero;
									part.Rig.angularVelocity = Vector3.zero;
								}
							}
						}
					}
					catch
					{
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				return false;
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

		[CompilerGenerated]
		private sealed class <JumpRepeatedlyCoroutine>d__29 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public float interval;

			public Character target;

			public float duration;

			private float <elapsed>5__2;

			private float <lastJumpTime>5__3;

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
			public <JumpRepeatedlyCoroutine>d__29(int <>1__state)
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
					<elapsed>5__2 = 0f;
					<lastJumpTime>5__3 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && (Object)(object)target != (Object)null && !target.data.dead)
				{
					if (<elapsed>5__2 - <lastJumpTime>5__3 >= interval)
					{
						try
						{
							MethodInfo method = ((object)((MonoBehaviourPun)target).photonView).GetType().GetMethod("JumpRpc", BindingFlags.Instance | BindingFlags.Public);
							if (method != null)
							{
								method.Invoke(((MonoBehaviourPun)target).photonView, new object[1] { true });
							}
						}
						catch
						{
						}
						<lastJumpTime>5__3 = <elapsed>5__2;
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				TrollHelpers.StopTrollEffects(target);
				return false;
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

		[CompilerGenerated]
		private sealed class <SpinPlayerCoroutine>d__18 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			public float spinSpeed;

			public float duration;

			private float <elapsed>5__2;

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
			public <SpinPlayerCoroutine>d__18(int <>1__state)
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
					<elapsed>5__2 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__2 < duration && (Object)(object)target != (Object)null && !target.data.dead)
				{
					try
					{
						if (target.refs != null && (Object)(object)target.refs.ragdoll != (Object)null)
						{
							Vector3 angularVelocity = default(Vector3);
							((Vector3)(ref angularVelocity))..ctor(0f, spinSpeed, 0f);
							foreach (Bodypart part in target.refs.ragdoll.partList)
							{
								if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
								{
									part.Rig.angularVelocity = angularVelocity;
									Vector3 val = target.Center - part.Rig.worldCenterOfMass;
									if (((Vector3)(ref val)).magnitude > 0.1f)
									{
										part.Rig.AddForce(((Vector3)(ref val)).normalized * 5f, (ForceMode)5);
									}
								}
							}
						}
					}
					catch
					{
					}
					<elapsed>5__2 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				try
				{
					if ((Object)(object)target != (Object)null && target.refs != null && (Object)(object)target.refs.ragdoll != (Object)null)
					{
						foreach (Bodypart part2 in target.refs.ragdoll.partList)
						{
							if ((Object)(object)part2 != (Object)null && (Object)(object)part2.Rig != (Object)null)
							{
								part2.Rig.angularVelocity = Vector3.zero;
							}
						}
					}
				}
				catch
				{
				}
				TrollHelpers.StopTrollEffects(target);
				return false;
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

		[CompilerGenerated]
		private sealed class <WalkOffCliffCoroutine>d__2 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int <>1__state;

			private object <>2__current;

			public Character target;

			private float <duration>5__2;

			private float <elapsed>5__3;

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
			public <WalkOffCliffCoroutine>d__2(int <>1__state)
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
					<duration>5__2 = 3f;
					<elapsed>5__3 = 0f;
					break;
				case 1:
					<>1__state = -1;
					break;
				}
				if (<elapsed>5__3 < <duration>5__2 && (Object)(object)target != (Object)null && (Object)(object)target.input != (Object)null)
				{
					FieldInfo field = typeof(CharacterInput).GetField("movementInput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field != null)
					{
						Vector2 val = default(Vector2);
						((Vector2)(ref val))..ctor(0f, 1f);
						field.SetValue(target.input, val);
					}
					<elapsed>5__3 += Time.deltaTime;
					<>2__current = null;
					<>1__state = 1;
					return true;
				}
				if ((Object)(object)target != (Object)null && (Object)(object)target.input != (Object)null)
				{
					FieldInfo field2 = typeof(CharacterInput).GetField("movementInput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field2 != null)
					{
						field2.SetValue(target.input, Vector2.zero);
					}
				}
				return false;
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

		public static void MakePlayerFall(Character target, float seconds = 5f, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Fall", (RpcTarget)0, seconds))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)$"[Troll] Made {obj} fall for {seconds} seconds!");
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to make player fall - RPC call failed");
			}
		}

		public static void MakePlayerWalkOffCliff(Character target, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				if ((Object)(object)target.data == (Object)null || (Object)(object)target.input == (Object)null)
				{
					return;
				}
				Vector3 lookDirection = target.data.lookDirection;
				lookDirection.y = 0f;
				((Vector3)(ref lookDirection)).Normalize();
				FieldInfo field = typeof(CharacterInput).GetField("movementInput", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					Vector2 val = default(Vector2);
					((Vector2)(ref val))..ctor(0f, 1f);
					field.SetValue(target.input, val);
					MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
					if ((Object)(object)coroutineRunner != (Object)null)
					{
						coroutineRunner.StartCoroutine(WalkOffCliffCoroutine(target, lookDirection));
					}
					Debug.Log((object)("[Troll] Made " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " walk off cliff!"));
					return;
				}
				Vector3 val2 = lookDirection * 10f;
				if (target.refs != null && (Object)(object)target.refs.ragdoll != (Object)null && target.refs.ragdoll.partList != null)
				{
					foreach (Bodypart part in target.refs.ragdoll.partList)
					{
						if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
						{
							part.Rig.AddForce(val2, (ForceMode)2);
						}
					}
				}
				Debug.Log((object)("[Troll] Applied forward force to " + ((MonoBehaviourPun)target).photonView.Owner.NickName + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to make player walk off cliff: " + ex.Message));
			}
		}

		[IteratorStateMachine(typeof(<WalkOffCliffCoroutine>d__2))]
		private static IEnumerator WalkOffCliffCoroutine(Character target, Vector3 direction)
		{
			return new <WalkOffCliffCoroutine>d__2(0)
			{
				target = target
			};
		}

		public static void UnFallPlayer(Character target, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_UnFall", (RpcTarget)0))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Stopped " + (string)obj + " from falling!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to unfall player - RPC call failed");
			}
		}

		public static void KillPlayer(Character target, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Die", (RpcTarget)0, Vector3.zero))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Killed " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to kill player - RPC call failed");
			}
		}

		public static void RevivePlayer(Character target, bool applyStatus = false, bool includeSelf = false)
		{
			if (TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Revive", (RpcTarget)0, applyStatus))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Revived " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to revive player - RPC call failed");
			}
		}

		public static void RevivePlayerAtPosition(Character target, Vector3 position, bool applyStatus = false, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				if (TrollHelpers.CallRPCMethod(target, "RPCA_ReviveAtPosition", (RpcTarget)0, position, applyStatus))
				{
					Debug.Log((object)("[Troll] Revived " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " at position!"));
				}
				else
				{
					Debug.LogError((object)"[Troll] Failed to revive player at position - RPC call failed");
				}
			}
		}

		public static void StickPlayerBodyPart(Character target, BodypartType bodypartType, Vector3 position, STATUSTYPE statusType = 0, float statusAmount = 0f, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Stick", (RpcTarget)0, bodypartType, position, position, statusType, statusAmount))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)$"[Troll] Stuck {obj}'s {bodypartType}!");
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to stick player body part - RPC call failed");
			}
		}

		public static void UnstickPlayer(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Unstick", (RpcTarget)0))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Unstuck " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to unstick player - RPC call failed");
			}
		}

		public static void StartCarryPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			if ((Object)(object)carrier == (Object)null || (Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)carrier).photonView == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || (!includeSelf && (((MonoBehaviourPun)carrier).photonView.IsMine || ((MonoBehaviourPun)target).photonView.IsMine)))
			{
				return;
			}
			try
			{
				((MonoBehaviourPun)carrier).photonView.RPC("RPCA_StartCarry", (RpcTarget)0, new object[1] { ((MonoBehaviourPun)target).photonView });
				Debug.Log((object)("[Troll] Made " + ((MonoBehaviourPun)carrier).photonView.Owner.NickName + " carry " + ((MonoBehaviourPun)target).photonView.Owner.NickName + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to start carry: " + ex.Message));
			}
		}

		public static void DropCarriedPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			if ((Object)(object)carrier == (Object)null || (Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)carrier).photonView == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || (!includeSelf && (((MonoBehaviourPun)carrier).photonView.IsMine || ((MonoBehaviourPun)target).photonView.IsMine)))
			{
				return;
			}
			try
			{
				((MonoBehaviourPun)carrier).photonView.RPC("RPCA_Drop", (RpcTarget)0, new object[1] { ((MonoBehaviourPun)target).photonView });
				Debug.Log((object)("[Troll] Made " + ((MonoBehaviourPun)carrier).photonView.Owner.NickName + " drop " + ((MonoBehaviourPun)target).photonView.Owner.NickName + "!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to drop carried player: " + ex.Message));
			}
		}

		public static void ZombifyPlayer(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			Vector3 val = target.Center + Vector3.up * 0.2f + Vector3.forward * 0.1f;
			if (TrollHelpers.CallRPCMethod(target, "RPCA_Zombify", (RpcTarget)0, val))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Zombified " + (string)obj + "!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to zombify player - RPC call failed");
			}
		}

		public static void MakePlayerPassOut(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			if (TrollHelpers.CallRPCMethod(target, "RPCA_PassOut", (RpcTarget)0))
			{
				PhotonView photonView = ((MonoBehaviourPun)target).photonView;
				object obj;
				if (photonView == null)
				{
					obj = null;
				}
				else
				{
					Player owner = photonView.Owner;
					obj = ((owner != null) ? owner.NickName : null);
				}
				if (obj == null)
				{
					obj = "Unknown";
				}
				Debug.Log((object)("[Troll] Made " + (string)obj + " pass out!"));
			}
			else
			{
				Debug.LogError((object)"[Troll] Failed to make player pass out - RPC call failed");
			}
		}

		public static void LaunchPlayer(Character target, Vector3 direction, float force = 50f, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				Vector3 val = ((Vector3)(ref direction)).normalized * force;
				if (target.refs != null && (Object)(object)target.refs.ragdoll != (Object)null && target.refs.ragdoll.partList != null)
				{
					foreach (Bodypart part in target.refs.ragdoll.partList)
					{
						if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
						{
							part.Rig.AddForceAtPosition(val, part.Rig.worldCenterOfMass, (ForceMode)1);
							part.Rig.AddForce(val * 0.5f, (ForceMode)1);
						}
					}
					Rigidbody component = ((Component)target).GetComponent<Rigidbody>();
					if ((Object)(object)component != (Object)null)
					{
						component.AddForce(val, (ForceMode)1);
					}
					Player owner = ((MonoBehaviourPun)target).photonView.Owner;
					Debug.Log((object)string.Format("[Troll] Launched {0} with force {1} (using bodypart impulse)!", ((owner != null) ? owner.NickName : null) ?? "Unknown", force));
					return;
				}
				MethodInfo method = typeof(Character).GetMethod("AddForceAtPosition", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(Vector3),
					typeof(Vector3),
					typeof(float)
				}, null);
				if (method != null)
				{
					Vector3 center = target.Center;
					method.Invoke(target, new object[3] { val, center, 10f });
					Player owner2 = ((MonoBehaviourPun)target).photonView.Owner;
					Debug.Log((object)string.Format("[Troll] Launched {0} with force {1} (using AddForceAtPosition)!", ((owner2 != null) ? owner2.NickName : null) ?? "Unknown", force));
					return;
				}
				MethodInfo method2 = typeof(Character).GetMethod("AddForce", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(Vector3),
					typeof(float),
					typeof(float)
				}, null);
				if (method2 != null)
				{
					method2.Invoke(target, new object[3] { val, 1f, 1f });
					Player owner3 = ((MonoBehaviourPun)target).photonView.Owner;
					Debug.Log((object)string.Format("[Troll] Launched {0} with force {1}!", ((owner3 != null) ? owner3.NickName : null) ?? "Unknown", force));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to launch player: " + ex.Message));
			}
		}

		public static void LaunchPlayerUp(Character target, float force = 50f, bool includeSelf = false)
		{
			LaunchPlayer(target, Vector3.up, force, includeSelf);
		}

		public static void LaunchPlayerTowardsMe(Character target, float force = 50f, bool includeSelf = false)
		{
			if (!((Object)(object)Character.localCharacter == (Object)null) && !((Object)(object)target == (Object)null))
			{
				Vector3 val = Character.localCharacter.Center - target.Center;
				Vector3 normalized = ((Vector3)(ref val)).normalized;
				LaunchPlayer(target, normalized, force, includeSelf);
			}
		}

		public static void LaunchPlayerAway(Character target, float force = 50f, bool includeSelf = false)
		{
			if (!((Object)(object)Character.localCharacter == (Object)null) && !((Object)(object)target == (Object)null))
			{
				Vector3 val = target.Center - Character.localCharacter.Center;
				Vector3 normalized = ((Vector3)(ref val)).normalized;
				LaunchPlayer(target, normalized, force, includeSelf);
			}
		}

		public static void MakePlayerSpin(Character target, float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!((Object)(object)coroutineRunner == (Object)null))
				{
					TrollHelpers.StopTrollEffects(target);
					Coroutine coroutine = coroutineRunner.StartCoroutine(SpinPlayerCoroutine(target, spinSpeed, duration));
					TrollHelpers.RegisterTrollEffect(target, coroutine);
					Debug.Log((object)("[Troll] Making " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " spin!"));
				}
			}
		}

		[IteratorStateMachine(typeof(<SpinPlayerCoroutine>d__18))]
		private static IEnumerator SpinPlayerCoroutine(Character target, float spinSpeed, float duration)
		{
			return new <SpinPlayerCoroutine>d__18(0)
			{
				target = target,
				spinSpeed = spinSpeed,
				duration = duration
			};
		}

		public static void TeleportPlayerRandomly(Character target, float minDistance = 30f, float maxDistance = 100f, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				Vector3 val = target.Center + Random.onUnitSphere * Random.Range(minDistance, maxDistance);
				val.y = target.Center.y;
				RaycastHit val2 = default(RaycastHit);
				if (Physics.Raycast(val + Vector3.up * 100f, Vector3.down, ref val2, 200f))
				{
					val = ((RaycastHit)(ref val2)).point + Vector3.up * 2f;
				}
				((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2] { val, true });
				Debug.Log((object)("[Troll] Teleported " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " randomly!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to teleport player: " + ex.Message));
			}
		}

		public static void TeleportPlayerToMe(Character target, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)Character.localCharacter == (Object)null))
			{
				TeleportPlayerToPosition(target, Character.localCharacter.Center + Vector3.up * 2f, includeSelf);
			}
		}

		public static void TeleportPlayerToPosition(Character target, Vector3 position, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", (RpcTarget)0, new object[2] { position, true });
				Debug.Log((object)("[Troll] Teleported " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " to position!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to teleport player: " + ex.Message));
			}
		}

		public static void MakePlayerRagdoll(Character target, float duration = 5f)
		{
			MakePlayerFall(target, duration);
		}

		public static void GrabPlayer(Character target)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || ((MonoBehaviourPun)target).photonView.IsMine || (Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			try
			{
				CharacterGrabbing component = ((Component)Character.localCharacter).GetComponent<CharacterGrabbing>();
				if (!((Object)(object)component == (Object)null))
				{
					MethodInfo method = typeof(CharacterGrabbing).GetMethod("RPCA_GrabAttach", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						BodypartType val = (BodypartType)2;
						Vector3 zero = Vector3.zero;
						method.Invoke(component, new object[3]
						{
							((MonoBehaviourPun)target).photonView,
							(int)val,
							zero
						});
						Debug.Log((object)("[Troll] Grabbed " + ((MonoBehaviourPun)target).photonView.Owner.NickName + "!"));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to grab player: " + ex.Message));
			}
		}

		public static void ThrowPlayer(Character target, Vector3 direction, float force = 30f)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null)
			{
				return;
			}
			try
			{
				Vector3 val = ((Vector3)(ref direction)).normalized * force;
				MethodInfo method = typeof(Character).GetMethod("AddForce", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[3]
				{
					typeof(Vector3),
					typeof(float),
					typeof(float)
				}, null);
				if (method != null)
				{
					method.Invoke(target, new object[3] { val, 1f, 1f });
					Debug.Log((object)("[Troll] Threw " + ((MonoBehaviourPun)target).photonView.Owner.NickName + "!"));
				}
				else
				{
					if (target.refs == null || !((Object)(object)target.refs.ragdoll != (Object)null))
					{
						return;
					}
					foreach (Bodypart part in target.refs.ragdoll.partList)
					{
						if ((Object)(object)part != (Object)null && (Object)(object)part.Rig != (Object)null)
						{
							part.Rig.AddForce(val, (ForceMode)5);
						}
					}
					Debug.Log((object)("[Troll] Threw " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " (using direct bodypart force)!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to throw player: " + ex.Message));
			}
		}

		public static void ApplyAntigrav(Character target, float duration = 10f, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!((Object)(object)coroutineRunner == (Object)null))
				{
					coroutineRunner.StartCoroutine(AntigravCoroutine(target, duration));
				}
			}
		}

		[IteratorStateMachine(typeof(<AntigravCoroutine>d__26))]
		private static IEnumerator AntigravCoroutine(Character target, float duration)
		{
			return new <AntigravCoroutine>d__26(0)
			{
				target = target,
				duration = duration
			};
		}

		public static void MakePlayerGlow(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || target.refs == null || (Object)(object)target.refs.afflictions == (Object)null || (!includeSelf && (Object)(object)((MonoBehaviourPun)target).photonView != (Object)null && ((MonoBehaviourPun)target).photonView.IsMine))
			{
				return;
			}
			try
			{
				MethodInfo method = typeof(CharacterAfflictions).GetMethod("AddAffliction", BindingFlags.Instance | BindingFlags.Public);
				if (!(method != null))
				{
					return;
				}
				Type type = Type.GetType("Affliction_Glowing");
				if (type != null)
				{
					object obj = Activator.CreateInstance(type);
					method.Invoke(target.refs.afflictions, new object[1] { obj });
					PhotonView photonView = ((MonoBehaviourPun)target).photonView;
					object obj2;
					if (photonView == null)
					{
						obj2 = null;
					}
					else
					{
						Player owner = photonView.Owner;
						obj2 = ((owner != null) ? owner.NickName : null);
					}
					if (obj2 == null)
					{
						obj2 = "player";
					}
					Debug.Log((object)("[Troll] Made " + (string)obj2 + " glow!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to make player glow: " + ex.Message));
			}
		}

		public static void MakePlayerJumpRepeatedly(Character target, float interval = 0.5f, float duration = 5f, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!((Object)(object)coroutineRunner == (Object)null))
				{
					TrollHelpers.StopTrollEffects(target);
					Coroutine coroutine = coroutineRunner.StartCoroutine(JumpRepeatedlyCoroutine(target, interval, duration));
					TrollHelpers.RegisterTrollEffect(target, coroutine);
					Debug.Log((object)("[Troll] Making " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " jump repeatedly!"));
				}
			}
		}

		[IteratorStateMachine(typeof(<JumpRepeatedlyCoroutine>d__29))]
		private static IEnumerator JumpRepeatedlyCoroutine(Character target, float interval, float duration)
		{
			return new <JumpRepeatedlyCoroutine>d__29(0)
			{
				target = target,
				interval = interval,
				duration = duration
			};
		}

		public static void FreezePlayer(Character target, float duration = 5f, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				MonoBehaviour coroutineRunner = TrollHelpers.GetCoroutineRunner();
				if (!((Object)(object)coroutineRunner == (Object)null))
				{
					coroutineRunner.StartCoroutine(FreezePlayerCoroutine(target, duration));
					Debug.Log((object)("[Troll] Freezing " + ((MonoBehaviourPun)target).photonView.Owner.NickName + "!"));
				}
			}
		}

		[IteratorStateMachine(typeof(<FreezePlayerCoroutine>d__31))]
		private static IEnumerator FreezePlayerCoroutine(Character target, float duration)
		{
			return new <FreezePlayerCoroutine>d__31(0)
			{
				target = target,
				duration = duration
			};
		}

		public static void MakeEveryoneJump(bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if (!((Object)(object)allCharacter != (Object)null) || !((Object)(object)((MonoBehaviourPun)allCharacter).photonView != (Object)null) || (!includeSelf && ((MonoBehaviourPun)allCharacter).photonView.IsMine))
				{
					continue;
				}
				try
				{
					MethodInfo method = ((object)((MonoBehaviourPun)allCharacter).photonView).GetType().GetMethod("JumpRpc", BindingFlags.Instance | BindingFlags.Public);
					if (method != null)
					{
						method.Invoke(((MonoBehaviourPun)allCharacter).photonView, new object[1] { true });
					}
				}
				catch
				{
				}
			}
			Debug.Log((object)"[Troll] Made everyone jump!");
		}

		public static void MakeEveryoneFall(float seconds = 5f, bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if ((Object)(object)allCharacter != (Object)null)
				{
					MakePlayerFall(allCharacter, seconds, includeSelf);
				}
			}
		}

		public static void MakeEveryonePassOut(bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if ((Object)(object)allCharacter != (Object)null)
				{
					MakePlayerPassOut(allCharacter, includeSelf);
				}
			}
		}

		public static void LaunchEveryoneUp(float force = 50f, bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if ((Object)(object)allCharacter != (Object)null)
				{
					LaunchPlayerUp(allCharacter, force, includeSelf);
				}
			}
		}

		public static void TeleportEveryoneToMe(bool includeSelf = false)
		{
			if ((Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			Vector3 center = Character.localCharacter.Center;
			int num = 0;
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if ((Object)(object)allCharacter != (Object)null && (includeSelf || !((MonoBehaviourPun)allCharacter).photonView.IsMine))
				{
					float num2 = (float)num * 360f / (float)Character.AllCharacters.Count * ((float)Math.PI / 180f);
					Vector3 val = new Vector3(Mathf.Cos(num2), 0f, Mathf.Sin(num2)) * 3f;
					TeleportPlayerToPosition(allCharacter, center + val, includeSelf);
					num++;
				}
			}
		}

		public static void MakeEveryoneSpin(float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			foreach (Character allCharacter in Character.AllCharacters)
			{
				if ((Object)(object)allCharacter != (Object)null)
				{
					MakePlayerSpin(allCharacter, spinSpeed, duration, includeSelf);
				}
			}
		}

		public static void MakePlayerJumpOffCliff(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				Vector3 lookDirection = target.data.lookDirection;
				lookDirection.y = 0f;
				((Vector3)(ref lookDirection)).Normalize();
				LaunchPlayer(target, lookDirection + Vector3.up * 0.5f, 30f, includeSelf);
				Debug.Log((object)("[Troll] Made " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " jump off cliff!"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to make player jump off cliff: " + ex.Message));
			}
		}
	}
}
