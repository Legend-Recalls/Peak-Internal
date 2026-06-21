using System;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUIVisualsTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("PlayerESP", "? Player ESP", Color.white))
			{
				CheatConfig.PlayerBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerBoxESP, "Box ESP");
				if (CheatConfig.PlayerBoxESP)
				{
					GUILayout.Space(2f);
					CheatConfig.PlayerBox3D = GUIHelpers.DrawToggleButton(CheatConfig.PlayerBox3D, "  3D Box");
				}
				GUILayout.Space(4f);
				CheatConfig.PlayerSkeletonESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerSkeletonESP, "Skeleton ESP");
				GUILayout.Space(4f);
				CheatConfig.PlayerNameESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.PlayerDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerDistanceESP, "Distance ESP");
				GUILayout.Space(4f);
				CheatConfig.PlayerHealthESP = GUIHelpers.DrawToggleButton(CheatConfig.PlayerHealthESP, "Health ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.PlayerESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.PlayerESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.PlayerESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("EntityESP", "? Entity ESP", Color.white))
			{
				CheatConfig.EntityBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityBoxESP, "Box ESP");
				if (CheatConfig.EntityBoxESP)
				{
					GUILayout.Space(2f);
					CheatConfig.EntityBox3D = GUIHelpers.DrawToggleButton(CheatConfig.EntityBox3D, "  3D Box");
				}
				GUILayout.Space(4f);
				CheatConfig.EntitySkeletonESP = GUIHelpers.DrawToggleButton(CheatConfig.EntitySkeletonESP, "Skeleton ESP");
				GUILayout.Space(4f);
				CheatConfig.EntityNameESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.EntityAIStateESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityAIStateESP, "AI State ESP");
				GUILayout.Space(4f);
				CheatConfig.EntityDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.EntityDistanceESP, "Distance ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.EntityESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.EntityESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.EntityESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ItemESP", "? Item ESP", Color.white))
			{
				CheatConfig.ItemBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.ItemBoxESP, "Box ESP");
				if (CheatConfig.ItemBoxESP)
				{
					GUILayout.Space(2f);
					CheatConfig.ItemBox3D = GUIHelpers.DrawToggleButton(CheatConfig.ItemBox3D, "  3D Box");
				}
				GUILayout.Space(4f);
				CheatConfig.ItemNameESP = GUIHelpers.DrawToggleButton(CheatConfig.ItemNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.ItemDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.ItemDistanceESP, "Distance ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.ItemESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.ItemESPMaxDistance, 10f, 200f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.ItemESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("LuggageESP", "? Luggage ESP", Color.white))
			{
				CheatConfig.LuggageBoxESP = GUIHelpers.DrawToggleButton(CheatConfig.LuggageBoxESP, "Box ESP");
				GUILayout.Space(4f);
				CheatConfig.LuggageNameESP = GUIHelpers.DrawToggleButton(CheatConfig.LuggageNameESP, "Name ESP");
				GUILayout.Space(4f);
				CheatConfig.LuggageDistanceESP = GUIHelpers.DrawToggleButton(CheatConfig.LuggageDistanceESP, "Distance ESP");
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.LuggageESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.LuggageESPMaxDistance, 10f, 200f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.LuggageESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("SporeShroomESP", "? Spore Shroom ESP", Color.white))
			{
				CheatConfig.SporeShroomESP = GUIHelpers.DrawToggleButton(CheatConfig.SporeShroomESP, "Box ESP");
				GUILayout.Label("Shows spore shrooms (explode on touch) with 3D boxes", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.SporeShroomESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.SporeShroomESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.SporeShroomESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("EnvironmentalESP", "? Environmental ESP", Color.white))
			{
				CheatConfig.EnvironmentalESP = GUIHelpers.DrawToggleButton(CheatConfig.EnvironmentalESP, "Weather Timers");
				GUILayout.Label("Shows wind and alpine blizzard timers", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.EnvironmentalESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.EnvironmentalESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.EnvironmentalESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ObjectNameESP", "? Object Name ESP", Color.white))
			{
				CheatConfig.ObjectNameESP = GUIHelpers.DrawToggleButton(CheatConfig.ObjectNameESP, "Show Object Names");
				GUILayout.Label("Shows names of all GameObjects (excludes items/entities)", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Distance:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
				CheatConfig.ObjectNameESPMaxDistance = GUILayout.HorizontalSlider(CheatConfig.ObjectNameESPMaxDistance, 10f, 500f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{CheatConfig.ObjectNameESPMaxDistance:F0}m", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("ClimbingFeatures", "? Climbing Features", Color.white))
			{
				CheatConfig.ClimbingPredictionEnabled = GUIHelpers.DrawToggleButton(CheatConfig.ClimbingPredictionEnabled, "Climbing Prediction Line");
				GUILayout.Label("Shows how far you can climb until stamina runs out", new GUIStyle(GUI.labelStyle)
				{
					fontSize = 10,
					fontStyle = (FontStyle)2,
					normal = new GUIStyleState
					{
						textColor = new Color(0.7f, 0.7f, 0.7f, 1f)
					}
				}, Array.Empty<GUILayoutOption>());
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("FieldOfView", "? Field of View", Color.white))
			{
				GUI.setfieldofview = GUIHelpers.DrawToggleButton(GUI.setfieldofview, "Set Field of View");
				if (GUI.setfieldofview)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("FOV:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(120f) });
					GUI.fieldofview = GUILayout.HorizontalSlider(GUI.fieldofview, 60f, 180f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.fieldofview:F0}", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUIHelpers.EndCollapsibleSection();
			}
		}
	}
}
