using System;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUIWorldTab
	{
		public static void Draw()
		{
			if (GUIHelpers.DrawCollapsibleSection("SelectItemToSpawn", "? Select Item to Spawn", Color.white))
			{
				GUI.CreateItemsVerticalSelect();
				GUILayout.Space(5f);
				if (GUIHelpers.DrawCollapsibleSection("SelectPlayerForSpawn", "? Select Player", Color.white))
				{
					GUI.CreatePlayersVerticalSelect();
					GUIHelpers.EndCollapsibleSection();
				}
				GUILayout.Space(5f);
				GUI.CreateSpawnItemButtons();
				GUIHelpers.EndCollapsibleSection();
			}
			GUIItemManagement.Draw();
			GUILayout.Space(10f);
			if (GUIHelpers.DrawCollapsibleSection("SpawnEntity", "? Spawn Entity", Color.white))
			{
				GUI.CreateEntityDropdown();
				GUIHelpers.EndCollapsibleSection();
			}
			GUI.DrawEntityManager();
		}
	}
}
