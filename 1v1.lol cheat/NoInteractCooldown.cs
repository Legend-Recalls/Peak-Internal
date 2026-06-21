using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class NoInteractCooldown
	{
		public static void Apply()
		{
			if (!CheatConfig.NoInteractCooldown || (Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			try
			{
				FieldInfo field = typeof(Character).GetField("interactCooldown", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					field.SetValue(Character.localCharacter, 0f);
				}
				FieldInfo field2 = typeof(Character).GetField("lastInteractTime", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field2 != null)
				{
					field2.SetValue(Character.localCharacter, 0f);
				}
				if (!((Object)(object)Interaction.instance != (Object)null))
				{
					return;
				}
				Interaction.instance.readyToInteract = true;
				Interaction.instance.readyToReleaseInteract = true;
				if (Interaction.instance.currentHeldInteractible == null || !(Interaction.instance.currentConstantInteractableTime > 0f))
				{
					return;
				}
				Interaction.instance.currentInteractableHeldTime = Interaction.instance.currentConstantInteractableTime;
				if (!(Interaction.instance.currentInteractableHeldTime >= Interaction.instance.currentConstantInteractableTime))
				{
					return;
				}
				try
				{
					Interaction.instance.currentHeldInteractible.Interact_CastFinished(Character.localCharacter);
					Interaction.instance.readyToReleaseInteract = false;
					if (!Interaction.instance.currentHeldInteractible.holdOnFinish)
					{
						MethodInfo method = typeof(Interaction).GetMethod("CancelHeldInteract", BindingFlags.Instance | BindingFlags.NonPublic);
						if (method != null)
						{
							method.Invoke(Interaction.instance, null);
						}
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}
	}
}
