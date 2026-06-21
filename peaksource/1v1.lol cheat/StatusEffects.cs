using System;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class StatusEffects
	{
		public static void ClearAll()
		{
			if ((Object)(object)Character.localCharacter == (Object)null || (Object)(object)Character.localCharacter.refs?.afflictions == (Object)null)
			{
				return;
			}
			try
			{
				int num = Enum.GetNames(typeof(STATUSTYPE)).Length;
				int num2 = -1;
				for (int i = 0; i < num; i++)
				{
					STATUSTYPE val = (STATUSTYPE)i;
					if ((int)val != 7 && (int)val != 5 && (int)val != 4 && (int)val != 9)
					{
						num2 = i;
					}
				}
				for (int j = 0; j < num; j++)
				{
					STATUSTYPE val2 = (STATUSTYPE)j;
					if ((int)val2 != 7 && (int)val2 != 5 && (int)val2 != 4 && (int)val2 != 9)
					{
						Character.localCharacter.refs.afflictions.SetStatus(val2, 0f, j == num2);
					}
				}
			}
			catch
			{
			}
		}

		public static void Clear()
		{
			if ((Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			int num = Enum.GetNames(typeof(STATUSTYPE)).Length;
			for (int i = 0; i < num; i++)
			{
				STATUSTYPE val = (STATUSTYPE)i;
				if ((int)val != 7 && (int)val != 5 && (int)val != 4)
				{
					Character.localCharacter.refs.afflictions.SetStatus(val, 0f, true);
				}
			}
		}
	}
}
