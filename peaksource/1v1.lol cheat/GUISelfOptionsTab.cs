using System;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUISelfOptionsTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("SpeedMovement", "? Speed & Movement", Color.white))
			{
				GUI.speed = GUIHelpers.DrawToggleButton(GUI.speed, "Speed Hack");
				if (GUI.speed)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Speed Multiplier:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.speedmultiply = GUILayout.HorizontalSlider(GUI.speedmultiply, 1f, 15f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.speedmultiply:F1}x", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Climbing Speed:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
				GUI.climbingSpeedMultiplier = GUILayout.HorizontalSlider(GUI.climbingSpeedMultiplier, 0.1f, 10f, Array.Empty<GUILayoutOption>());
				GUILayout.Label($"{GUI.climbingSpeedMultiplier:F2}x", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
				GUILayout.EndHorizontal();
				GUILayout.Space(4f);
				GUI.reduceStaminaConsumption = GUIHelpers.DrawToggleButton(GUI.reduceStaminaConsumption, "Reduce Stamina Consumption");
				if (GUI.reduceStaminaConsumption)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Stamina Usage:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.staminaConsumptionPercent = GUILayout.HorizontalSlider(GUI.staminaConsumptionPercent, 1f, 100f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.staminaConsumptionPercent:F0}%", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUI.flyMode = GUIHelpers.DrawToggleButton(GUI.flyMode, "Fly Mode");
				if (GUI.flyMode)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Fly Speed:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					CheatConfig.FlySpeed = GUILayout.HorizontalSlider(CheatConfig.FlySpeed, 10f, 200f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{CheatConfig.FlySpeed:F0}", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUI.noClip = GUIHelpers.DrawToggleButton(GUI.noClip, "No Clip");
				GUILayout.Space(4f);
				GUI.superJump = GUIHelpers.DrawToggleButton(GUI.superJump, "Super Jump");
				if (GUI.superJump)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Jump Multiplier:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.jumpMultiplier = GUILayout.HorizontalSlider(GUI.jumpMultiplier, 1f, 5f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.jumpMultiplier:F1}x", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("CombatSurvival", "? Survival", Color.white))
			{
				GUI.godmode = GUIHelpers.DrawToggleButton(GUI.godmode, "God Mode");
				GUILayout.Space(4f);
				GUI.infiniteammo = GUIHelpers.DrawToggleButton(GUI.infiniteammo, "Infinite Charges");
				GUILayout.Space(4f);
				GUI.rapidfire = GUIHelpers.DrawToggleButton(GUI.rapidfire, "No Item Cooldown");
				GUILayout.Space(4f);
				CheatConfig.NoInteractCooldown = GUIHelpers.DrawToggleButton(CheatConfig.NoInteractCooldown, "No Interact Cooldown");
				GUILayout.Space(4f);
				GUI.clearStatuses = GUIHelpers.DrawToggleButton(GUI.clearStatuses, "Clear Status Effects");
				GUILayout.Space(4f);
				GUI.noFallDamage = GUIHelpers.DrawToggleButton(GUI.noFallDamage, "No Fall Damage");
				if (!GUI.noFallDamage)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Fall Damage:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.fallDamagePercent = GUILayout.HorizontalSlider(GUI.fallDamagePercent, 0f, 200f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.fallDamagePercent:F0}%", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				CheatConfig.AntiFallOver = GUIHelpers.DrawToggleButton(CheatConfig.AntiFallOver, "Anti Fall Over");
				GUIHelpers.EndCollapsibleSection();
			}
			if (GUIHelpers.DrawCollapsibleSection("OtherFeatures", "? Other Features", Color.white))
			{
				GUI.Unlockall = GUIHelpers.DrawToggleButton(GUI.Unlockall, "Unlock All Items");
				GUILayout.Space(4f);
				GUI.rapidfire = GUIHelpers.DrawToggleButton(GUI.rapidfire, "Time Modifier");
				if (GUI.rapidfire)
				{
					GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
					GUILayout.Label("Time:", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(140f) });
					GUI.fireratecooldown = GUILayout.HorizontalSlider(GUI.fireratecooldown, 0f, 48f, Array.Empty<GUILayoutOption>());
					GUILayout.Label($"{GUI.fireratecooldown:F1}", GUI.labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(50f) });
					GUILayout.EndHorizontal();
				}
				GUILayout.Space(4f);
				GUI.randomoutfits = GUIHelpers.DrawToggleButton(GUI.randomoutfits, "Randomize Outfits");
				GUIHelpers.EndCollapsibleSection();
			}
			if (!GUIHelpers.DrawCollapsibleSection("AutoPathfinder", "? Auto Pathfinder", Color.white))
			{
				return;
			}
			CheatConfig.AutoPathfinderEnabled = GUIHelpers.DrawToggleButton(CheatConfig.AutoPathfinderEnabled, "Auto Pathfind to End");
			if (CheatConfig.AutoPathfinderEnabled)
			{
				if (AutoPathfinder.FollowingPath)
				{
					GUILayout.Label($"Following path ({AutoPathfinder.PathNodeCount} nodes)", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
				else
				{
					GUILayout.Label("Calculating path...", GUI.labelStyle, Array.Empty<GUILayoutOption>());
				}
			}
			GUIHelpers.EndCollapsibleSection();
		}
	}
}
