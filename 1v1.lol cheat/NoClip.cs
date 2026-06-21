using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class NoClip
	{
		public static void Apply()
		{
			try
			{
				if ((Object)(object)Character.localCharacter?.refs?.ragdoll == (Object)null || Character.localCharacter.refs.ragdoll.partList == null)
				{
					return;
				}
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)((part != null) ? ((Component)part).gameObject : null) == (Object)null)
					{
						continue;
					}
					try
					{
						Collider component = ((Component)part).gameObject.GetComponent<Collider>();
						if ((Object)(object)component != (Object)null)
						{
							if (CheatConfig.NoClip)
							{
								component.isTrigger = true;
								component.enabled = false;
							}
							else
							{
								component.isTrigger = false;
								component.enabled = true;
							}
						}
					}
					catch
					{
					}
				}
				if (!((Object)(object)((Component)Character.localCharacter).gameObject != (Object)null))
				{
					return;
				}
				try
				{
					Collider component2 = ((Component)Character.localCharacter).gameObject.GetComponent<Collider>();
					if ((Object)(object)component2 != (Object)null)
					{
						if (CheatConfig.NoClip)
						{
							component2.isTrigger = true;
							component2.enabled = false;
						}
						else
						{
							component2.isTrigger = false;
							component2.enabled = true;
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

		public static void Disable()
		{
			CheatConfig.NoClip = false;
			if (!((Object)(object)Character.localCharacter != (Object)null) || !((Object)(object)Character.localCharacter.refs?.ragdoll != (Object)null))
			{
				return;
			}
			foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
			{
				if ((Object)(object)((part != null) ? ((Component)part).gameObject : null) != (Object)null)
				{
					Collider component = ((Component)part).gameObject.GetComponent<Collider>();
					if ((Object)(object)component != (Object)null && component.isTrigger)
					{
						component.isTrigger = false;
					}
				}
			}
		}
	}
}
