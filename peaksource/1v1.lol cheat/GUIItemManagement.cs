using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUIItemManagement
	{
		public static void Draw()
		{
			if (!GUIHelpers.DrawCollapsibleSection("WorldItems", "? World Items", Color.white))
			{
				return;
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Button("Refresh Items", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			List<Item> list = (from item in Object.FindObjectsByType<Item>((FindObjectsSortMode)0)
				where (Object)(object)item != (Object)null && (int)item.itemState == 0
				select item).ToList();
			if (list.Count == 0)
			{
				GUILayout.Label("No items found on the map", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
				return;
			}
			GUILayout.Label($"Found {list.Count} items", new GUIStyle(GUI.labelStyle)
			{
				fontSize = 11
			}, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			int num = 8;
			int num2 = Mathf.Min(list.Count, num);
			if (GUI.selectedWorldItemIndex < 0 || GUI.selectedWorldItemIndex >= list.Count)
			{
				GUI.selectedWorldItemIndex = -1;
			}
			for (int i = 0; i < num2; i++)
			{
				Item val = list[i];
				if (!((Object)(object)val == (Object)null))
				{
					string name = val.GetName();
					float num3 = (((Object)(object)Character.localCharacter != (Object)null) ? Vector3.Distance(Character.localCharacter.Center, val.Center()) : 0f);
					string text = $"{name} ({num3:F1}m)";
					bool flag = GUI.selectedWorldItemIndex == i;
					if (GUILayout.Toggle(flag, text, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(24f) }))
					{
						GUI.selectedWorldItemIndex = i;
					}
					else if (flag)
					{
						GUI.selectedWorldItemIndex = -1;
					}
				}
			}
			if (list.Count > num)
			{
				GUILayout.Label($"... and {list.Count - num} more items (scroll to see)", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
			}
			GUILayout.Space(5f);
			if (GUI.selectedWorldItemIndex >= 0 && GUI.selectedWorldItemIndex < list.Count)
			{
				Item val2 = list[GUI.selectedWorldItemIndex];
				if ((Object)(object)val2 != (Object)null)
				{
					GUILayout.Label("Selected: " + val2.GetName(), new GUIStyle(GUI.labelStyle)
					{
						fontStyle = (FontStyle)1
					}, Array.Empty<GUILayoutOption>());
					GUILayout.Space(5f);
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Teleport To Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }) && (Object)(object)Character.localCharacter != (Object)null)
					{
						Vector3 val3 = Character.localCharacter.Center + Vector3.up * 2f;
						((MonoBehaviourPun)val2).photonView.RPC("SetKinematicRPC", (RpcTarget)3, new object[3]
						{
							true,
							val3,
							Quaternion.identity
						});
					}
					if (GUILayout.Button("Launch Up", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }) && (Object)(object)val2.rig != (Object)null)
					{
						val2.rig.linearVelocity = Vector3.up * 20f;
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Cook Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
					{
						ItemCooking component = ((Component)val2).GetComponent<ItemCooking>();
						if ((Object)(object)component != (Object)null && component.canBeCooked && (Object)(object)((MonoBehaviourPun)val2).photonView != (Object)null)
						{
							((MonoBehaviourPun)val2).photonView.RPC("FinishCookingRPC", (RpcTarget)0, Array.Empty<object>());
						}
					}
					if (GUILayout.Button("Delete Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }) && (Object)(object)((MonoBehaviourPun)val2).photonView != (Object)null && (((MonoBehaviourPun)val2).photonView.IsMine || PhotonNetwork.IsMasterClient))
					{
						PhotonNetwork.Destroy(((Component)val2).gameObject);
					}
					GUILayout.EndHorizontal();
				}
			}
			else
			{
				GUILayout.Label("Select an item above", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 11,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
}
