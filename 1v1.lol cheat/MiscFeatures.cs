using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class MiscFeatures
	{
		private static CharacterCustomization _customization = null;

		private static float _cosmeticCooldown = 0.07f;

		public static void ApplyUnlockAll()
		{
			if ((Object)(object)PassportManager.instance != (Object)null)
			{
				PassportManager.instance.testUnlockAll = CheatConfig.UnlockAll;
			}
		}

		private static float _bingBongCooldown = 0f;
		private const float BING_BONG_INTERVAL = 0.5f;

		public static void ApplyBingBongSpam()
		{
			if (CheatConfig.BingBongSpam && Time.time > _bingBongCooldown)
			{
				PhotonNetwork.Instantiate("0_Items/BingBong", ((Component)Camera.main).transform.position, ((Component)Camera.main).transform.rotation, (byte)0, (object[])null);
				_bingBongCooldown = Time.time + BING_BONG_INTERVAL;
			}
		}

		public static void ApplyClearStatuses()
		{
			if (CheatConfig.ClearStatuses && !((Object)(object)Character.localCharacter == (Object)null))
			{
				StatusEffects.Clear();
				CheatConfig.ClearStatuses = false;
			}
		}

		public static void ApplyRandomOutfits()
		{
			if (CheatConfig.RandomOutfits && Time.time > _cosmeticCooldown)
			{
				if ((Object)(object)_customization == (Object)null && (Object)(object)Character.localCharacter != (Object)null)
				{
					_customization = ((Component)Character.localCharacter).GetComponent<CharacterCustomization>();
				}
				CharacterCustomization customization = _customization;
				if (customization != null)
				{
					customization.RandomizeCosmetics();
				}
				_cosmeticCooldown = Time.time + 0.07f;
			}
		}
	}
}
