using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

namespace _1v1.lol_cheat.Troll
{
	public static class ItemManipulation
	{
		public static void OpenAllLuggage()
		{
			try
			{
				Luggage[] array = Object.FindObjectsByType<Luggage>((FindObjectsSortMode)0);
				int num = 0;
				Luggage[] array2 = array;
				foreach (Luggage val in array2)
				{
					if (!((Object)(object)val != (Object)null) || !((Object)(object)((MonoBehaviourPun)val).photonView != (Object)null))
					{
						continue;
					}
					FieldInfo field = typeof(Luggage).GetField("state", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field != null)
					{
						object value = field.GetValue(val);
						if (value != null && value.ToString() == "Closed")
						{
							((MonoBehaviourPun)val).photonView.RPC("OpenLuggageRPC", (RpcTarget)0, new object[1] { true });
							num++;
						}
					}
					else
					{
						((MonoBehaviourPun)val).photonView.RPC("OpenLuggageRPC", (RpcTarget)0, new object[1] { true });
						num++;
					}
				}
				if (num > 0)
				{
					Debug.Log((object)$"[Troll] Opened {num} luggage!");
					return;
				}
				FieldInfo field2 = typeof(Luggage).GetField("ALL_LUGGAGE", BindingFlags.Static | BindingFlags.Public);
				if (!(field2 != null) || !(field2.GetValue(null) is List<Luggage> list))
				{
					return;
				}
				num = 0;
				foreach (Luggage item in list)
				{
					if ((Object)(object)item != (Object)null && (Object)(object)((MonoBehaviourPun)item).photonView != (Object)null)
					{
						((MonoBehaviourPun)item).photonView.RPC("OpenLuggageRPC", (RpcTarget)0, new object[1] { true });
						num++;
					}
				}
				Debug.Log((object)$"[Troll] Opened {num} luggage (via static list)!");
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to open all luggage: " + ex.Message));
			}
		}

		public static void TeleportAllItems(Vector3 position)
		{
			try
			{
				Item[] array = Object.FindObjectsByType<Item>((FindObjectsSortMode)0);
				int num = 0;
				Item[] array2 = array;
				foreach (Item val in array2)
				{
					if ((Object)(object)val != (Object)null && (Object)(object)((MonoBehaviourPun)val).photonView != (Object)null)
					{
						((MonoBehaviourPun)val).photonView.RPC("SetKinematicRPC", (RpcTarget)3, new object[3]
						{
							true,
							position,
							Quaternion.identity
						});
						num++;
					}
				}
				Debug.Log((object)$"[Troll] Teleported {num} items to position!");
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to teleport all items: " + ex.Message));
			}
		}

		public static void TeleportAllItemsToPlayer(Character target)
		{
			if (!((Object)(object)target == (Object)null))
			{
				TeleportAllItems(target.Center + Vector3.up * 2f);
			}
		}

		public static void TeleportAllItemsInFrontOfMe()
		{
			if (!((Object)(object)Character.localCharacter == (Object)null) && !((Object)(object)Camera.main == (Object)null))
			{
				Vector3 forward = ((Component)Camera.main).transform.forward;
				Vector3 position = ((Component)Camera.main).transform.position + forward * 3f;
				position.y = Character.localCharacter.Center.y;
				TeleportAllItems(position);
			}
		}

		public static void ForceDropAllItems(Character target, bool includeSelf = false)
		{
			if ((Object)(object)target == (Object)null || (Object)(object)((MonoBehaviourPun)target).photonView == (Object)null || TrollHelpers.ShouldSkipPlayer(target, includeSelf))
			{
				return;
			}
			try
			{
				MethodInfo method = typeof(CharacterItems).GetMethod("DropAllItems", BindingFlags.Instance | BindingFlags.NonPublic);
				if (method != null && target.refs != null && (Object)(object)target.refs.items != (Object)null)
				{
					method.Invoke(target.refs.items, new object[1] { true });
					Debug.Log((object)("[Troll] Forced " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " to drop all items!"));
				}
				else if (PhotonNetwork.IsMasterClient)
				{
					for (byte b = 0; b < 3; b++)
					{
						target.player.EmptySlot(Optionable<byte>.Some(b));
					}
					target.player.EmptySlot(Optionable<byte>.Some((byte)3));
					target.player.EmptySlot(Optionable<byte>.Some((byte)250));
					Debug.Log((object)("[Troll] Forced " + ((MonoBehaviourPun)target).photonView.Owner.NickName + " to drop all items (via slots)!"));
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[Troll] Failed to force drop all items: " + ex.Message));
			}
		}
	}
}
