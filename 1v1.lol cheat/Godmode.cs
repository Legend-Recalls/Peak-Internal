using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class Godmode
	{
		public static void Apply()
		{
			if ((Object)(object)Character.localCharacter == (Object)null || !((MonoBehaviourPun)Character.localCharacter).photonView.IsMine)
			{
				return;
			}
			if (CheatConfig.Godmode && !CheatConfig.GodmodeWasEnabled)
			{
				CheatConfig.GodmodeWasEnabled = true;
				PropertyInfo property = typeof(Character).GetProperty("infiniteStam", BindingFlags.Instance | BindingFlags.Public);
				if (property != null)
				{
					MethodInfo setMethod = property.GetSetMethod(nonPublic: true);
					if (setMethod != null)
					{
						setMethod.Invoke(Character.localCharacter, new object[1] { true });
					}
				}
				Character.localCharacter.data.currentStamina = 1f;
				Character.localCharacter.AddExtraStamina(100f);
				GUIManager.instance.bar.ChangeBar();
				StatusEffects.ClearAll();
			}
			else if (!CheatConfig.Godmode && CheatConfig.GodmodeWasEnabled)
			{
				CheatConfig.GodmodeWasEnabled = false;
				PropertyInfo property2 = typeof(Character).GetProperty("infiniteStam", BindingFlags.Instance | BindingFlags.Public);
				if (property2 != null)
				{
					MethodInfo setMethod2 = property2.GetSetMethod(nonPublic: true);
					if (setMethod2 != null)
					{
						setMethod2.Invoke(Character.localCharacter, new object[1] { false });
					}
				}
			}
			if (CheatConfig.Godmode)
			{
				Character.localCharacter.data.currentStamina = Mathf.Max(Character.localCharacter.data.currentStamina, Character.localCharacter.GetMaxStamina());
				GUIManager.instance.bar.ChangeBar();
				StatusEffects.ClearAll();
			}
		}

		public static void Disable()
		{
			CheatConfig.Godmode = false;
			CheatConfig.GodmodeWasEnabled = false;
			if (!((Object)(object)Character.localCharacter != (Object)null))
			{
				return;
			}
			PropertyInfo property = typeof(Character).GetProperty("infiniteStam", BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				MethodInfo setMethod = property.GetSetMethod(nonPublic: true);
				if (setMethod != null)
				{
					setMethod.Invoke(Character.localCharacter, new object[1] { false });
				}
			}
		}
	}
}
