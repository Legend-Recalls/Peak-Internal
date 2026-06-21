using System;
using UnityEngine;

namespace Loading
{
	public class Loader
	{
		private static GameObject gameObject;

		public static void Load()
		{
			if ((Object)(object)gameObject != (Object)null)
			{
				Debug.Log((object)"[Loader] Destroying old cheat instance");
				Object.DestroyImmediate((Object)(object)gameObject);
				gameObject = null;
			}
			if ((Object)(object)Cheat.instance != (Object)null)
			{
				Debug.Log((object)"[Loader] Destroying existing Cheat instance");
				try
				{
					if ((Object)(object)((Component)Cheat.instance).gameObject != (Object)null)
					{
						Object.DestroyImmediate((Object)(object)((Component)Cheat.instance).gameObject);
					}
				}
				catch
				{
				}
			}
			gameObject = new GameObject();
			((Object)gameObject).name = "PEAKCheatLoader";
			gameObject.AddComponent<Cheat>();
			Object.DontDestroyOnLoad((Object)(object)gameObject);
			Debug.Log((object)"[Loader] Cheat component created");
		}

		public static void Unload()
		{
			Object.Destroy((Object)(object)gameObject);
		}
	}
}
