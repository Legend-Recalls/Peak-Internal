using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class Stamina
	{
		public static void ApplyReduction()
		{
			if (CheatConfig.ReduceStaminaConsumption && !((Object)(object)Character.localCharacter == (Object)null) && ((MonoBehaviourPun)Character.localCharacter).photonView.IsMine && !((Object)(object)Character.localCharacter.data == (Object)null))
			{
				float num = CheatConfig.StaminaConsumptionPercent / 100f;
				float maxStamina = Character.localCharacter.GetMaxStamina();
				if (CheatConfig.LastStaminaForReduction < 0f)
				{
					CheatConfig.LastStaminaForReduction = Character.localCharacter.data.currentStamina;
				}
				float currentStamina = Character.localCharacter.data.currentStamina;
				if (currentStamina < CheatConfig.LastStaminaForReduction)
				{
					float num2 = CheatConfig.LastStaminaForReduction - currentStamina;
					float num3 = num2 * num;
					float num4 = num2 - num3;
					Character.localCharacter.data.currentStamina = Mathf.Min(currentStamina + num4, maxStamina);
					GUIManager.instance.bar.ChangeBar();
					currentStamina = Character.localCharacter.data.currentStamina;
				}
				CheatConfig.LastStaminaForReduction = currentStamina;
			}
		}

		public static void ResetTracking()
		{
			if (!CheatConfig.ReduceStaminaConsumption)
			{
				CheatConfig.LastStaminaForReduction = -1f;
			}
		}
	}
}
