using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class RapidFire
	{
		private static Dictionary<Item, float> _originalUsingTimes = new Dictionary<Item, float>();

		public static void Apply()
		{
			if ((Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			if ((Object)(object)Character.localCharacter.data.currentItem != (Object)null)
			{
				try
				{
					Item currentItem = Character.localCharacter.data.currentItem;
					if (CheatConfig.RapidFire)
					{
						if (!_originalUsingTimes.ContainsKey(currentItem))
						{
							FieldInfo field = typeof(Item).GetField("usingTimePrimary", BindingFlags.Instance | BindingFlags.Public);
							if (field != null)
							{
								float value = (float)field.GetValue(currentItem);
								_originalUsingTimes[currentItem] = value;
								field.SetValue(currentItem, 0.001f);
							}
						}
					}
					else if (_originalUsingTimes.ContainsKey(currentItem))
					{
						FieldInfo field2 = typeof(Item).GetField("usingTimePrimary", BindingFlags.Instance | BindingFlags.Public);
						if (field2 != null)
						{
							field2.SetValue(currentItem, _originalUsingTimes[currentItem]);
							_originalUsingTimes.Remove(currentItem);
						}
					}
				}
				catch
				{
				}
			}
			if (CheatConfig.RapidFire)
			{
				return;
			}
			foreach (KeyValuePair<Item, float> item in _originalUsingTimes.ToList())
			{
				if ((Object)(object)item.Key == (Object)null)
				{
					_originalUsingTimes.Remove(item.Key);
					continue;
				}
				try
				{
					FieldInfo field3 = typeof(Item).GetField("usingTimePrimary", BindingFlags.Instance | BindingFlags.Public);
					if (field3 != null)
					{
						field3.SetValue(item.Key, item.Value);
					}
					_originalUsingTimes.Remove(item.Key);
				}
				catch
				{
				}
			}
		}
	}
}
