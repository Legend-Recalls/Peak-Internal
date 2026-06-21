using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class InfiniteAmmo
	{
		private static Item _lastInfiniteAmmoItem = null;

		private static Dictionary<Item, bool> _patchedItems = new Dictionary<Item, bool>();

		private static Type _actionReduceUsesType = null;

		private static MethodInfo _originalRunActionMethod = null;

		private static Dictionary<object, Action> _originalActions = new Dictionary<object, Action>();

		public static void Apply()
		{
			if ((Object)(object)Character.localCharacter == (Object)null)
			{
				return;
			}
			if (CheatConfig.InfiniteAmmo)
			{
				if (Character.localCharacter.data.dead || Character.localCharacter.data.fullyPassedOut)
				{
					((MonoBehaviourPun)Character.localCharacter).photonView.RPC("RPCA_Revive", (RpcTarget)0, new object[1] { true });
					StatusEffects.ClearAll();
				}
				if (!((Object)(object)Character.localCharacter.data.currentItem != (Object)null))
				{
					return;
				}
				try
				{
					Item currentItem = Character.localCharacter.data.currentItem;
					if ((Object)(object)_lastInfiniteAmmoItem != (Object)(object)currentItem)
					{
						_lastInfiniteAmmoItem = currentItem;
						_patchedItems.Remove(currentItem);
					}
					if (!_patchedItems.ContainsKey(currentItem))
					{
						PatchReduceUses(currentItem);
						_patchedItems[currentItem] = true;
					}
					return;
				}
				catch
				{
					return;
				}
			}
			if ((Object)(object)_lastInfiniteAmmoItem != (Object)null && _patchedItems.ContainsKey(_lastInfiniteAmmoItem))
			{
				UnpatchReduceUses(_lastInfiniteAmmoItem);
				_patchedItems.Remove(_lastInfiniteAmmoItem);
			}
		}

		private static void PatchReduceUses(Item item)
		{
			try
			{
				if (_actionReduceUsesType == null)
				{
					_actionReduceUsesType = Type.GetType("Action_ReduceUses");
					if (_actionReduceUsesType != null)
					{
						_originalRunActionMethod = _actionReduceUsesType.GetMethod("RunAction", BindingFlags.Instance | BindingFlags.Public);
					}
				}
				if (_actionReduceUsesType == null || _originalRunActionMethod == null)
				{
					return;
				}
				Component component = ((Component)item).GetComponent(_actionReduceUsesType);
				if ((Object)(object)component == (Object)null || _originalActions.ContainsKey(component))
				{
					return;
				}
				FieldInfo field = typeof(Item).GetField("OnPrimaryFinishedCast", BindingFlags.Instance | BindingFlags.Public);
				if (!(field != null) || !(field.GetValue(item) is Action action))
				{
					return;
				}
				Delegate[] invocationList = action.GetInvocationList();
				Action action2 = null;
				Delegate[] array = invocationList;
				foreach (Delegate @delegate in array)
				{
					if (@delegate.Target == component && @delegate.Method == _originalRunActionMethod)
					{
						Action originalDelegate = (Action)@delegate;
						_originalActions[component] = originalDelegate;
						action2 = delegate
						{
							if (!CheatConfig.InfiniteAmmo)
							{
								originalDelegate();
							}
						};
					}
					else if (action2 == null)
					{
						if (@delegate is Action action3)
						{
							action2 = action3;
						}
					}
					else
					{
						action2 = (Action)Delegate.Combine(action2, @delegate as Action);
					}
				}
				if (action2 != null)
				{
					field.SetValue(item, action2);
				}
			}
			catch
			{
			}
		}

		private static void UnpatchReduceUses(Item item)
		{
			try
			{
				if (_actionReduceUsesType == null)
				{
					return;
				}
				Component component = ((Component)item).GetComponent(_actionReduceUsesType);
				if (!((Object)(object)component == (Object)null) && _originalActions.ContainsKey(component))
				{
					FieldInfo field = typeof(Item).GetField("OnPrimaryFinishedCast", BindingFlags.Instance | BindingFlags.Public);
					if (field != null && field.GetValue(item) is Action a && _originalActions.ContainsKey(component))
					{
						Action value = (Action)Delegate.Combine(a, _originalActions[component]);
						field.SetValue(item, value);
					}
				}
			}
			catch
			{
			}
		}
	}
}
