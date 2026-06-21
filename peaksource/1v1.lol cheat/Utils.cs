using System.Collections.Generic;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public class Utils : MonoBehaviour
	{
		public static List<Character> GetTargets()
		{
			List<Character> list = new List<Character>();
			if (Character.AllCharacters != null)
			{
				foreach (Character allCharacter in Character.AllCharacters)
				{
					if (!((Object)(object)allCharacter == (Object)null) && !((Object)(object)allCharacter == (Object)(object)Character.localCharacter) && !((Object)(object)((MonoBehaviourPun)allCharacter).photonView == (Object)null) && !((MonoBehaviourPun)allCharacter).photonView.IsMine)
					{
						bool flag = (Object)(object)allCharacter.player != (Object)null;
						if (!flag)
						{
							GameObject gameObject = ((Component)allCharacter).gameObject;
							flag = (Object)(object)((gameObject != null) ? gameObject.GetComponent<Player>() : null) != (Object)null;
						}
						if (flag && !((Object)(object)allCharacter.data == (Object)null) && !allCharacter.data.dead)
						{
							list.Add(allCharacter);
						}
					}
				}
			}
			return list;
		}
	}
}
