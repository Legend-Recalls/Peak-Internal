using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class FieldOfView
	{
		public static void Apply()
		{
			if (!CheatConfig.SetFieldOfView || (Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			try
			{
				Camera.main.fieldOfView = CheatConfig.FieldOfView;
			}
			catch
			{
			}
		}
	}
}
