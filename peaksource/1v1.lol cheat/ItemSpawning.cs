using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class ItemSpawning
	{
		public static void SpawnItem(string itemName, Character targetPlayer = null)
		{
			if (!PhotonNetwork.InRoom)
			{
				return;
			}
			try
			{
				Vector3 val = (((Object)(object)targetPlayer != (Object)null && targetPlayer.refs != null && (Object)(object)targetPlayer.refs.head != (Object)null) ? (((Component)targetPlayer.refs.head).transform.position + Vector3.up * 2f) : ((!((Object)(object)Character.localCharacter != (Object)null) || Character.localCharacter.refs == null || !((Object)(object)Character.localCharacter.refs.head != (Object)null)) ? (((Object)(object)Camera.main != (Object)null) ? (((Component)Camera.main).transform.position + ((Component)Camera.main).transform.forward * 2f) : Vector3.zero) : (((Component)Character.localCharacter.refs.head).transform.position + Vector3.up * 2f)));
				if ((Object)(object)PhotonNetwork.Instantiate(itemName, val, Quaternion.identity, (byte)0, (object[])null) != (Object)null)
				{
					Debug.Log((object)$"[ItemSpawning] Spawned {itemName} at {val}");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ItemSpawning] Failed to spawn item " + itemName + ": " + ex.Message));
			}
		}

		public static void SpawnEntity(string entityName, Character targetPlayer = null)
		{
			if (!PhotonNetwork.InRoom)
			{
				return;
			}
			try
			{
				Vector3 val = (((Object)(object)targetPlayer != (Object)null && targetPlayer.refs != null && (Object)(object)targetPlayer.refs.head != (Object)null) ? (((Component)targetPlayer.refs.head).transform.position + Vector3.up * 2f) : ((!((Object)(object)Character.localCharacter != (Object)null) || Character.localCharacter.refs == null || !((Object)(object)Character.localCharacter.refs.head != (Object)null)) ? (((Object)(object)Camera.main != (Object)null) ? (((Component)Camera.main).transform.position + ((Component)Camera.main).transform.forward * 2f) : Vector3.zero) : (((Component)Character.localCharacter.refs.head).transform.position + Vector3.up * 2f)));
				if ((Object)(object)PhotonNetwork.Instantiate(entityName, val, Quaternion.identity, (byte)0, (object[])null) != (Object)null)
				{
					Debug.Log((object)$"[ItemSpawning] Spawned entity {entityName} at {val}");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ItemSpawning] Failed to spawn entity " + entityName + ": " + ex.Message));
			}
		}

		public static void InitializeEntities()
		{
			if (CheatConfig.EntitiesInitialized)
			{
				return;
			}
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>();
			try
			{
				if (PhotonNetwork.PrefabPool != null)
				{
					try
					{
						FieldInfo field = ((object)PhotonNetwork.PrefabPool).GetType().GetField("resourceCache", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null && field.GetValue(PhotonNetwork.PrefabPool) is Dictionary<string, GameObject> dictionary)
						{
							foreach (string key in dictionary.Keys)
							{
								if (string.IsNullOrEmpty(key))
								{
									continue;
								}
								string text = key.ToLower();
								if ((text.Contains("item") && !text.Contains("zombie") && !text.Contains("scoutmaster") && !text.Contains("beetle")) || key == "Player" || key == "Character")
								{
									continue;
								}
								GameObject val = dictionary[key];
								if ((Object)(object)val != (Object)null)
								{
									bool flag = (Object)(object)val.GetComponent<PhotonView>() != (Object)null;
									bool flag2 = (Object)(object)val.GetComponent<Character>() != (Object)null;
									bool flag3 = (Object)(object)val.GetComponent<Item>() != (Object)null;
									if ((flag || flag2) && !flag3 && !hashSet.Contains(key))
									{
										list.Add(key);
										hashSet.Add(key);
										Debug.Log((object)("[ItemSpawning] Found entity in pool: " + key));
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Debug.Log((object)("[ItemSpawning] Could not access PhotonNetwork prefab pool: " + ex.Message));
					}
				}
				string[] array = new string[13]
				{
					"", "0_Characters", "Characters", "Prefabs", "0_Prefabs", "Entities", "NPCs", "AI", "Enemies", "0_Entities",
					"0_NPCs", "0_AI", "0_Enemies"
				};
				foreach (string text2 in array)
				{
					try
					{
						GameObject[] array2 = Resources.LoadAll<GameObject>(text2);
						foreach (GameObject val2 in array2)
						{
							if ((Object)(object)val2 == (Object)null)
							{
								continue;
							}
							string name = ((Object)val2).name;
							string text3 = (string.IsNullOrEmpty(text2) ? name : (text2 + "/" + name));
							if (hashSet.Contains(text3) || hashSet.Contains(name))
							{
								continue;
							}
							string text4 = name.ToLower();
							if ((!text4.Contains("item") || text4.Contains("zombie") || text4.Contains("scoutmaster") || text4.Contains("beetle")) && !(name == "Player") && !(name == "Character"))
							{
								bool flag4 = (Object)(object)val2.GetComponent<PhotonView>() != (Object)null;
								bool flag5 = (Object)(object)val2.GetComponent<Character>() != (Object)null;
								bool flag6 = (Object)(object)val2.GetComponent<Item>() != (Object)null;
								if ((flag4 || flag5) && !flag6)
								{
									list.Add(text3);
									hashSet.Add(text3);
									hashSet.Add(name);
									Debug.Log((object)("[ItemSpawning] Found entity in Resources: " + text3));
								}
							}
						}
					}
					catch (Exception ex2)
					{
						Debug.Log((object)("[ItemSpawning] Could not load prefabs from folder '" + text2 + "': " + ex2.Message));
					}
				}
				array = new string[13]
				{
					"Zombie", "Scoutmaster", "ScoutMaster", "Beetle", "0_Characters/Zombie", "0_Characters/Scoutmaster", "0_Characters/Beetle", "Characters/Zombie", "Characters/Scoutmaster", "Characters/Beetle",
					"Prefabs/Zombie", "Prefabs/Scoutmaster", "Prefabs/Beetle"
				};
				foreach (string text5 in array)
				{
					if (hashSet.Contains(text5))
					{
						continue;
					}
					try
					{
						GameObject val3 = Resources.Load<GameObject>(text5);
						if ((Object)(object)val3 != (Object)null)
						{
							bool flag7 = (Object)(object)val3.GetComponent<Item>() != (Object)null;
							bool flag8 = (Object)(object)val3.GetComponent<PhotonView>() != (Object)null;
							bool flag9 = (Object)(object)val3.GetComponent<Character>() != (Object)null;
							if (!flag7 && (flag8 || flag9))
							{
								list.Add(text5);
								hashSet.Add(text5);
								Debug.Log((object)("[ItemSpawning] Found common entity: " + text5));
							}
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception ex3)
			{
				Debug.LogError((object)("[ItemSpawning] Error discovering entities: " + ex3.Message));
			}
			List<string> list2 = new List<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			try
			{
				if (CheatConfig.Items != null && CheatConfig.Items.Length != 0)
				{
					string[] array = CheatConfig.Items;
					foreach (string text6 in array)
					{
						if (!string.IsNullOrEmpty(text6))
						{
							hashSet2.Add(text6.ToLower());
						}
					}
				}
			}
			catch
			{
			}
			foreach (string item in list)
			{
				string text7 = item.ToLower();
				bool flag10 = false;
				foreach (string item2 in hashSet2)
				{
					if (text7 == item2.ToLower() || text7.EndsWith("/" + item2.ToLower()))
					{
						flag10 = true;
						break;
					}
				}
				if (!text7.Contains("zombie") && !text7.Contains("scoutmaster") && !text7.Contains("beetle") && text7.Contains("item"))
				{
					flag10 = true;
				}
				if (!flag10)
				{
					list2.Add(item);
				}
			}
			CheatConfig.AvailableEntities = list2.ToArray();
			CheatConfig.EntitiesInitialized = true;
			Debug.Log((object)string.Format("[ItemSpawning] Discovered {0} spawnable entities: {1}...", CheatConfig.AvailableEntities.Length, string.Join(", ", CheatConfig.AvailableEntities.Take(10))));
		}

		public static string[] FilterEntities(string searchText)
		{
			if (string.IsNullOrEmpty(searchText))
			{
				return CheatConfig.AvailableEntities;
			}
			string searchLower = searchText.ToLower();
			return CheatConfig.AvailableEntities.Where((string e) => e.ToLower().Contains(searchLower)).ToArray();
		}
	}
}
