using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class PlayerModifications
	{
		public static void ApplyModifications()
		{
			if (!((Object)(object)Character.localCharacter == (Object)null) && ((MonoBehaviourPun)Character.localCharacter).photonView.IsMine)
			{
				bool anyEnabled = CheatConfig.Godmode || CheatConfig.Speed || CheatConfig.SuperJump ||
					CheatConfig.NoClip || CheatConfig.FlyMode || CheatConfig.InfiniteAmmo ||
					CheatConfig.RapidFire || CheatConfig.NoInteractCooldown || CheatConfig.UnlockAll ||
					CheatConfig.BingBongSpam || CheatConfig.ClearStatuses || CheatConfig.RandomOutfits ||
					CheatConfig.SetFieldOfView || CheatConfig.AntiFallOver || CheatConfig.ClimbingSpeedMultiplier != 1f;

				if (!anyEnabled)
					return;

				Godmode.Apply();
				Movement.ApplySpeedHack();
				Movement.ApplySuperJump();
				Movement.ApplyClimbingSpeed();
				NoClip.Apply();
				Movement.ApplyFlyMode();
				InfiniteAmmo.Apply();
				RapidFire.Apply();
				NoInteractCooldown.Apply();
				MiscFeatures.ApplyUnlockAll();
				MiscFeatures.ApplyBingBongSpam();
				MiscFeatures.ApplyClearStatuses();
				MiscFeatures.ApplyRandomOutfits();
				FieldOfView.Apply();
				AntiFallOver.Apply();
			}
		}

		public static void Update()
		{
			FieldOfView.Apply();
		}

		public static void DisableAll()
		{
			Godmode.Disable();
			Movement.Disable();
			NoClip.Disable();
		}
	}
}
