using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUIEntityManagement
	{
		public static void Draw()
		{
			try
			{
				GUI.CleanupDestroyedEntities();
				List<GameObject> list = null;
				if (CheatConfig._cachedEntityList != null && CheatConfig._cachedEntityList.Count > 0 && !CheatConfig._entityListNeedsRefresh)
				{
					list = CheatConfig._cachedEntityList;
					list.RemoveAll((GameObject e) => (Object)(object)e == (Object)null);
				}
				else
				{
					list = GUI.GetAllEntitiesInGame();
					CheatConfig._entityListNeedsRefresh = false;
				}
				if (GUIHelpers.DrawCollapsibleSection("EntityList", "? All Entities in Game", Color.white))
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Entities:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Refresh", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
					{
						GUILayout.Width(90f),
						GUILayout.Height(32f)
					}))
					{
						CheatConfig._entityListNeedsRefresh = true;
						list = GUI.GetAllEntitiesInGame();
					}
					GUILayout.EndHorizontal();
					GUILayout.Space(4f);
					if (list == null || list.Count == 0)
					{
						GUILayout.Label("No entities found in game. Click Refresh to search.", GUI.labelStyle, Array.Empty<GUILayoutOption>());
					}
					else
					{
						GUILayout.Label($"Total: {list.Count} entities", new GUIStyle(GUI.labelStyle)
						{
							fontSize = 11
						}, Array.Empty<GUILayoutOption>());
						GUILayout.Space(4f);
						int num = 50;
						int num2 = Mathf.Min(list.Count, num);
						for (int i = 0; i < num2; i++)
						{
							try
							{
								if ((Object)(object)list[i] == (Object)null)
								{
									continue;
								}
								GameObject val = list[i];
								if ((Object)(object)val == (Object)null)
								{
									continue;
								}
								string text = "Unknown";
								string arg = "Unknown";
								try
								{
									text = GUI.GetEntityDisplayName(val);
									arg = GUI.GetEntityType(val);
								}
								catch (Exception ex)
								{
									Debug.LogWarning((object)("[GUI] Error getting entity name/type: " + ex.Message));
									text = ((Object)val).name ?? "Unknown";
								}
								string text2 = $"[{i}] {text} ({arg})";
								try
								{
									if ((Object)(object)Character.localCharacter != (Object)null && (Object)(object)Camera.main != (Object)null && (Object)(object)val.transform != (Object)null)
									{
										float num3 = Vector3.Distance(val.transform.position, ((Component)Camera.main).transform.position);
										text2 += $" ({num3:F1}m away)";
									}
								}
								catch
								{
								}
								bool flag = GUI.selectedEntityIndex == i;
								Color backgroundColor = GUI.backgroundColor;
								Color contentColor = GUI.contentColor;
								if (flag)
								{
									GUI.backgroundColor = new Color(0.2f, 0.6f, 1f, 0.3f);
									GUI.contentColor = Color.white;
								}
								GUILayout.BeginHorizontal(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
								if (GUILayout.Toggle(flag, "", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) }))
								{
									GUI.selectedEntityIndex = i;
								}
								else if (flag)
								{
									GUI.selectedEntityIndex = -1;
								}
								GUILayout.Label(text2, GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
								GUILayout.EndHorizontal();
								GUI.backgroundColor = backgroundColor;
								GUI.contentColor = contentColor;
							}
							catch (Exception ex2)
							{
								Debug.LogWarning((object)$"[GUI] Error rendering entity {i}: {ex2.Message}");
							}
						}
						if (list.Count > num)
						{
							GUILayout.Label($"... and {list.Count - num} more entities (scroll to see)", new GUIStyle(GUI.labelStyle)
							{
								fontSize = 10,
								normal = new GUIStyleState
								{
									textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
								}
							}, Array.Empty<GUILayoutOption>());
						}
					}
					GUIHelpers.EndCollapsibleSection();
				}
				if (list != null && GUI.selectedEntityIndex >= 0 && GUI.selectedEntityIndex < list.Count && (Object)(object)list[GUI.selectedEntityIndex] != (Object)null)
				{
					try
					{
						GameObject val2 = list[GUI.selectedEntityIndex];
						string text3 = "Unknown";
						try
						{
							text3 = GUI.GetEntityDisplayName(val2);
						}
						catch
						{
							text3 = ((Object)val2).name ?? "Unknown";
						}
						if (GUIHelpers.DrawCollapsibleSection("EntityActions", "? Actions for: " + text3, Color.white))
						{
							GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
							if (GUILayout.Button("Teleport to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
							{
								try
								{
									GUI.TeleportEntityToMe(val2);
								}
								catch (Exception ex3)
								{
									Debug.LogError((object)("[GUI] Error teleporting entity: " + ex3.Message));
								}
							}
							if (GUILayout.Button("Kill", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
							{
								try
								{
									GUI.KillEntity(val2);
									GUI.selectedEntityIndex = -1;
								}
								catch (Exception ex4)
								{
									Debug.LogError((object)("[GUI] Error killing entity: " + ex4.Message));
								}
							}
							GUILayout.EndHorizontal();
							GUILayout.Space(4f);
							bool flag2 = false;
							try
							{
								flag2 = GUI.controlledEntities.ContainsKey(val2) && GUI.controlledEntities[val2];
								bool flag3 = GUIHelpers.DrawToggleButton(flag2, "Control Entity (WASD + Camera)");
								if (flag3 != flag2)
								{
									try
									{
										GUI.controlledEntities[val2] = flag3;
										if (flag3)
										{
											GUI.EnableEntityControl(val2);
											GUI.SwitchCameraToEntity(val2);
										}
										else
										{
											GUI.DisableEntityControl(val2);
											GUI.RestoreCameraToPlayer();
										}
										flag2 = flag3;
									}
									catch (Exception ex5)
									{
										Debug.LogError((object)("[GUI] Error toggling entity control: " + ex5.Message));
									}
								}
								if (flag2 && (Object)(object)GUI.currentlyControlledEntity == (Object)(object)val2)
								{
									GUILayout.Label(" Currently controlling this entity", new GUIStyle(GUI.labelStyle)
									{
										normal = new GUIStyleState
										{
											textColor = Color.green
										},
										fontSize = 11
									}, Array.Empty<GUILayoutOption>());
								}
							}
							catch (Exception ex6)
							{
								Debug.LogWarning((object)("[GUI] Error in control toggle: " + ex6.Message));
								try
								{
									flag2 = GUI.controlledEntities.ContainsKey(val2) && GUI.controlledEntities[val2];
								}
								catch
								{
								}
							}
							try
							{
								bool flag4 = GUI.followPlayerEntities.ContainsKey(val2) && GUI.followPlayerEntities[val2];
								bool flag5 = GUIHelpers.DrawToggleButton(flag4, "Follow Player");
								if (flag5 != flag4)
								{
									try
									{
										GUI.followPlayerEntities[val2] = flag5;
										if (flag5)
										{
											GUI.targetPositions.Remove(val2);
										}
									}
									catch (Exception ex7)
									{
										Debug.LogError((object)("[GUI] Error toggling follow player: " + ex7.Message));
									}
								}
							}
							catch (Exception ex8)
							{
								Debug.LogWarning((object)("[GUI] Error in follow toggle: " + ex8.Message));
							}
							GUILayout.Space(4f);
							if (GUIHelpers.DrawCollapsibleSection("AIControl", "? AI Control", Color.white))
							{
								try
								{
									MonoBehaviour[] aIComponents = GUI.GetAIComponents(val2);
									if (aIComponents != null && aIComponents.Length != 0)
									{
										MonoBehaviour[] array = aIComponents;
										foreach (MonoBehaviour val3 in array)
										{
											try
											{
												if ((Object)(object)val3 != (Object)null)
												{
													string name = ((object)val3).GetType().Name;
													bool enabled = ((Behaviour)val3).enabled;
													bool flag6 = GUIHelpers.DrawToggleButton(enabled, name);
													if (flag6 != enabled)
													{
														((Behaviour)val3).enabled = flag6;
														Debug.Log((object)("[Cheat] " + (flag6 ? "Enabled" : "Disabled") + " AI component: " + name));
													}
												}
											}
											catch (Exception ex9)
											{
												Debug.LogWarning((object)("[GUI] Error processing AI component: " + ex9.Message));
											}
										}
									}
									else
									{
										GUILayout.Label("No AI components found", new GUIStyle(GUI.labelStyle)
										{
											fontSize = 11
										}, Array.Empty<GUILayoutOption>());
									}
								}
								catch (Exception ex10)
								{
									Debug.LogWarning((object)("[GUI] Error getting AI components: " + ex10.Message));
									GUILayout.Label("Error loading AI components", new GUIStyle(GUI.labelStyle)
									{
										fontSize = 11
									}, Array.Empty<GUILayoutOption>());
								}
								GUILayout.Space(4f);
								if (GUILayout.Button("Reinitialize Entity", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
								{
									try
									{
										GUI.InitializeSpawnedEntity(val2);
									}
									catch (Exception ex11)
									{
										Debug.LogError((object)("[GUI] Error reinitializing entity: " + ex11.Message));
									}
								}
								GUIHelpers.EndCollapsibleSection();
							}
							GUILayout.Space(4f);
							if (flag2)
							{
								GUILayout.Label("Movement Controls:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								GUILayout.Label("Speed:", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(60f) });
								GUI.entityControlSpeed = GUILayout.HorizontalSlider(GUI.entityControlSpeed, 1f, 20f, Array.Empty<GUILayoutOption>());
								GUILayout.Label(GUI.entityControlSpeed.ToString("F1"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(40f) });
								GUILayout.EndHorizontal();
								GUILayout.Space(4f);
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								if (GUILayout.Button("Move Forward", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.forward);
								}
								if (GUILayout.Button("Move Back", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.back);
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								if (GUILayout.Button("Move Left", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.left);
								}
								if (GUILayout.Button("Move Right", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MoveEntity(val2, Vector3.right);
								}
								GUILayout.EndHorizontal();
								GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
								if (GUILayout.Button("Jump", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.MakeEntityJump(val2);
								}
								if (GUILayout.Button("Stop", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(34f) }))
								{
									GUI.StopEntity(val2);
								}
								GUILayout.EndHorizontal();
							}
							GUILayout.Space(4f);
							GUILayout.Label("Teleport to Player:", GUI.labelStyle, Array.Empty<GUILayoutOption>());
							if (GUI.playerDict != null && GUI.playerDict.Count > 0)
							{
								string[] array2 = GUI.playerDict.Keys.ToArray();
								int num4 = GUILayout.SelectionGrid(-1, array2, 2, GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) });
								if (num4 >= 0 && num4 < array2.Length)
								{
									Character val4 = GUI.playerDict[array2[num4]];
									if ((Object)(object)val4 != (Object)null)
									{
										GUI.TeleportEntityToPlayer(val2, val4);
									}
								}
							}
							else
							{
								GUILayout.Label("No players found", new GUIStyle(GUI.labelStyle)
								{
									fontSize = 11
								}, Array.Empty<GUILayoutOption>());
							}
							GUIHelpers.EndCollapsibleSection();
						}
					}
					catch (Exception ex12)
					{
						Debug.LogError((object)("[GUI] Error in actions section: " + ex12.Message));
						GUIHelpers.EndCollapsibleSection();
					}
				}
				else if (GUIHelpers.DrawCollapsibleSection("NoEntitySelected", "? Actions", (Color?)new Color(0.7f, 0.7f, 0.7f, 1f)))
				{
					GUILayout.Label("Select an entity above to manage it", new GUIStyle(GUI.labelStyle)
					{
						fontSize = 11,
						fontStyle = (FontStyle)2
					}, Array.Empty<GUILayoutOption>());
					GUIHelpers.EndCollapsibleSection();
				}
				if (!GUIHelpers.DrawCollapsibleSection("BulkActions", "? Bulk Actions", Color.white))
				{
					return;
				}
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Kill All", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					foreach (GameObject item in GUI.GetAllEntitiesInGame())
					{
						if ((Object)(object)item != (Object)null)
						{
							GUI.KillEntity(item);
						}
					}
					GUI.selectedEntityIndex = -1;
				}
				if (GUILayout.Button("Teleport All to Me", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) }))
				{
					foreach (GameObject item2 in GUI.GetAllEntitiesInGame())
					{
						if ((Object)(object)item2 != (Object)null)
						{
							GUI.TeleportEntityToMe(item2);
						}
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
				if (GUILayout.Button("Refresh List", GUI.buttonStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(28f) }))
				{
					GUI.CleanupDestroyedEntities();
				}
				GUIHelpers.EndCollapsibleSection();
			}
			catch (Exception ex13)
			{
				Debug.LogError((object)("[GUI] Error in DrawEntityManager: " + ex13.Message + "\n" + ex13.StackTrace));
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				GUILayout.Label("Error loading entity manager", new GUIStyle(GUI.labelStyle)
				{
					normal = new GUIStyleState
					{
						textColor = Color.red
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Label("Error: " + ex13.Message, new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10
				}, Array.Empty<GUILayoutOption>());
				GUILayout.EndVertical();
			}
		}
	}
}
