using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUIPlayerManagement
	{
		public static void DrawInventoryManagement()
		{
			if (!GUIHelpers.DrawCollapsibleSection("InventoryManagement", "? Inventory Management", Color.white))
			{
				return;
			}
			Character val = null;
			if (GUI.playerDict != null && GUI.playerDict.Count > 0 && GUI.selectedPlayerIndex >= 0 && GUI.selectedPlayerIndex < GUI.playerDict.Keys.Count)
			{
				string[] array = GUI.playerDict.Keys.ToArray();
				val = GUI.playerDict[array[GUI.selectedPlayerIndex]];
			}
			if ((Object)(object)val == (Object)null || (Object)(object)val.player == (Object)null)
			{
				GUILayout.Label("Select a player to view inventory", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
				return;
			}
			GUILayout.Label("Viewing: " + GUI.playerDict.Keys.ToArray()[GUI.selectedPlayerIndex], new GUIStyle(GUI.labelStyle)
			{
				fontStyle = (FontStyle)1
			}, Array.Empty<GUILayoutOption>());
			GUILayout.Space(5f);
			float num = Mathf.Min(GUIHelpers.GetScrollViewHeight() - 150f, 300f);
			num = Mathf.Max(num, 150f);
			GUI.inventoryScrollPos = GUILayout.BeginScrollView(GUI.inventoryScrollPos, false, true, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(num) });
			for (byte b = 0; b < 3; b++)
			{
				ItemSlot itemSlot = val.player.GetItemSlot(b);
				string text = $"Slot {b}: ";
				text = ((!itemSlot.IsEmpty()) ? (text + (((Object)(object)itemSlot.prefab != (Object)null) ? itemSlot.prefab.GetName() : "Unknown Item")) : (text + "Empty"));
				bool flag = GUI.selectedInventorySlotIndex == b;
				if (GUILayout.Toggle(flag, text, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
				{
					GUI.selectedInventorySlotIndex = b;
				}
				else if (flag)
				{
					GUI.selectedInventorySlotIndex = -1;
				}
			}
			try
			{
				ItemSlot itemSlot2 = val.player.GetItemSlot((byte)3);
				string text2 = "Backpack: ";
				text2 = (itemSlot2.IsEmpty() ? (text2 + "Empty") : ((!((Object)(object)itemSlot2.prefab != (Object)null)) ? (text2 + "Backpack") : (text2 + itemSlot2.prefab.GetName())));
				bool flag2 = GUI.selectedInventorySlotIndex == 3;
				if (GUILayout.Toggle(flag2, text2, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
				{
					GUI.selectedInventorySlotIndex = 3;
				}
				else if (flag2)
				{
					GUI.selectedInventorySlotIndex = -1;
				}
				if (!itemSlot2.IsEmpty() && ((ItemSlot)val.player.backpackSlot).data != null)
				{
					try
					{
						MethodInfo method = typeof(ItemInstanceData).GetMethod("TryGetDataEntry", BindingFlags.Instance | BindingFlags.Public);
						if (method != null)
						{
							object obj = Enum.Parse(typeof(DataEntryKey), "BackpackData");
							MethodInfo methodInfo = method.MakeGenericMethod(typeof(BackpackData));
							object[] array2 = new object[2] { obj, null };
							if ((bool)methodInfo.Invoke(((ItemSlot)val.player.backpackSlot).data, array2) && array2[1] != null)
							{
								object obj2 = array2[1];
								BackpackData val2 = (BackpackData)((obj2 is BackpackData) ? obj2 : null);
								if (val2 != null && val2.itemSlots != null)
								{
									GUILayout.Space(3f);
									GUILayout.Label("  Contents:", new GUIStyle(GUI.labelStyle)
									{
										fontSize = 11,
										normal = new GUIStyleState
										{
											textColor = new Color(0.7f, 0.9f, 1f, 1f)
										}
									}, Array.Empty<GUILayoutOption>());
									for (int i = 0; i < val2.itemSlots.Length; i++)
									{
										ItemSlot val3 = val2.itemSlots[i];
										if (!val3.IsEmpty())
										{
											string arg = (((Object)(object)val3.prefab != (Object)null) ? val3.prefab.GetName() : "Unknown");
											GUILayout.Label($"    Slot {i}: {arg}", new GUIStyle(GUI.labelStyle)
											{
												fontSize = 10,
												normal = new GUIStyleState
												{
													textColor = new Color(0.8f, 0.8f, 0.8f, 1f)
												}
											}, Array.Empty<GUILayoutOption>());
										}
									}
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				GUILayout.Label("Backpack: Error (" + ex.Message + ")", GUI.labelStyle, Array.Empty<GUILayoutOption>());
			}
			ItemSlot itemSlot3 = val.player.GetItemSlot((byte)250);
			string text3 = "Temp Slot: ";
			text3 = ((!itemSlot3.IsEmpty()) ? (text3 + (((Object)(object)itemSlot3.prefab != (Object)null) ? itemSlot3.prefab.GetName() : "Unknown Item")) : (text3 + "Empty"));
			bool flag3 = GUI.selectedInventorySlotIndex == 250;
			if (GUILayout.Toggle(flag3, text3, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
			{
				GUI.selectedInventorySlotIndex = 250;
			}
			else if (flag3)
			{
				GUI.selectedInventorySlotIndex = -1;
			}
			GUILayout.EndScrollView();
			GUILayout.Space(5f);
			if (GUI.selectedInventorySlotIndex >= 0)
			{
				try
				{
					byte b2 = (byte)((GUI.selectedInventorySlotIndex == 250) ? 250u : ((uint)GUI.selectedInventorySlotIndex));
					ItemSlot itemSlot4 = val.player.GetItemSlot(b2);
					if (!itemSlot4.IsEmpty())
					{
						string text4 = "Unknown Item";
						if ((Object)(object)itemSlot4.prefab != (Object)null)
						{
							text4 = itemSlot4.prefab.GetName();
						}
						else if (b2 == 3)
						{
							text4 = "Backpack";
						}
						GUILayout.Label("Selected: " + text4, new GUIStyle(GUI.labelStyle)
						{
							fontStyle = (FontStyle)1
						}, Array.Empty<GUILayoutOption>());
						GUILayout.Space(5f);
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						if (GUILayout.Button("Drop Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							GUI.DropItemFromPlayer(val, b2);
							GUI.selectedInventorySlotIndex = -1;
						}
						if (GUILayout.Button("Steal Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							GUI.StealItemFromPlayer(val, b2);
							GUI.selectedInventorySlotIndex = -1;
						}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
						if (GUILayout.Button("Delete Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							val.player.EmptySlot(Optionable<byte>.Some(b2));
							GUI.selectedInventorySlotIndex = -1;
						}
						if (GUILayout.Button("Cook Item", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
						{
							GUI.CookPlayerItem(val, b2);
						}
						GUILayout.EndHorizontal();
					}
				}
				catch (Exception ex2)
				{
					GUILayout.Label("Error: " + ex2.Message, new GUIStyle(GUI.labelStyle)
					{
						normal = new GUIStyleState
						{
							textColor = new Color(1f, 0.4f, 0.4f, 1f)
						}
					}, Array.Empty<GUILayoutOption>());
				}
			}
			else
			{
				GUILayout.Label("Select an item slot above", new GUIStyle(GUI.labelStyle)
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
