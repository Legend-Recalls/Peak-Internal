using System;
using System.Collections.Generic;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class GUIHelpers
	{
		private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();

		public static Dictionary<string, GUIStyle> styleCache = new Dictionary<string, GUIStyle>();

		public static float GetScrollViewHeight()
		{
			return Mathf.Max(((Rect)(ref GUI.GUIRect)).height - 140f, 500f);
		}

		public static string TruncateForDisplay(string text, int maxLength = 64)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (text.Length <= maxLength)
			{
				return text;
			}
			return text.Substring(0, maxLength - 3) + "...";
		}

		public static Texture2D MakeTex(int width, int height, Color col)
		{
			Color key = default(Color);
			((Color)(ref key))..ctor(Mathf.Round(col.r * 1000f) / 1000f, Mathf.Round(col.g * 1000f) / 1000f, Mathf.Round(col.b * 1000f) / 1000f, Mathf.Round(col.a * 1000f) / 1000f);
			if (textureCache.ContainsKey(key))
			{
				return textureCache[key];
			}
			Color[] array = (Color[])(object)new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D val = new Texture2D(width, height);
			val.SetPixels(array);
			val.Apply();
			textureCache[key] = val;
			return val;
		}

		public static void ClearCache()
		{
			foreach (Texture2D value in textureCache.Values)
			{
				if ((Object)(object)value != (Object)null)
				{
					Object.DestroyImmediate((Object)(object)value);
				}
			}
			textureCache.Clear();
			styleCache.Clear();
		}

		public static bool DrawToggleButton(bool currentState, string label)
		{
			Color backgroundColor = GUI.backgroundColor;
			Color contentColor = GUI.contentColor;
			Color val = default(Color);
			((Color)(ref val))..ctor(0f, 0.5f, 0.85f, 1f);
			Color col = default(Color);
			((Color)(ref col))..ctor(0.1f, 0.6f, 0.9f, 1f);
			Color val2 = default(Color);
			((Color)(ref val2))..ctor(0.12f, 0.12f, 0.12f, 1f);
			Color col2 = default(Color);
			((Color)(ref col2))..ctor(0.18f, 0.18f, 0.18f, 1f);
			if (currentState)
			{
				GUI.backgroundColor = val;
				GUI.contentColor = Color.white;
			}
			else
			{
				GUI.backgroundColor = val2;
				GUI.contentColor = new Color(0.88f, 0.88f, 0.92f, 1f);
			}
			string key = $"toggle_{currentState}";
			GUIStyle val3;
			if (!styleCache.ContainsKey(key))
			{
				val3 = new GUIStyle(GUI.buttonStyle);
				if (currentState)
				{
					val3.normal.background = MakeTex(2, 2, val);
					val3.hover.background = MakeTex(2, 2, col);
					val3.active.background = MakeTex(2, 2, new Color(0f, 0.75f, 0.38f, 1f));
					val3.normal.textColor = Color.white;
					val3.hover.textColor = Color.white;
				}
				else
				{
					val3.normal.background = MakeTex(2, 2, val2);
					val3.hover.background = MakeTex(2, 2, col2);
					val3.active.background = MakeTex(2, 2, new Color(0.12f, 0.14f, 0.17f, 1f));
					val3.normal.textColor = new Color(0.88f, 0.88f, 0.92f, 1f);
					val3.hover.textColor = new Color(0.95f, 0.95f, 1f, 1f);
				}
				val3.alignment = (TextAnchor)3;
				val3.padding = new RectOffset(20, 14, 12, 12);
				val3.fontSize = 14;
				val3.fontStyle = (FontStyle)0;
				val3.border = new RectOffset(6, 6, 6, 6);
				styleCache[key] = val3;
			}
			else
			{
				val3 = styleCache[key];
			}
			string text = (currentState ? (" " + label) : ("  " + label));
			bool result = GUILayout.Toggle(currentState, text, val3, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(40f) });
			GUI.backgroundColor = backgroundColor;
			GUI.contentColor = contentColor;
			return result;
		}

		public static bool DrawCollapsibleSection(string key, string title, Color? titleColor = null)
		{
			if (!CheatConfig.CollapsibleSections.ContainsKey(key))
			{
				CheatConfig.CollapsibleSections[key] = false;
			}
			bool flag = CheatConfig.CollapsibleSections[key];
			Color val = (Color)(((??)titleColor) ?? new Color(0.9f, 0.95f, 1f, 1f));
			string key2 = $"foldout_{key}_{((object)(Color)(ref val)).GetHashCode()}_{flag}";
			GUIStyle val2;
			if (!styleCache.ContainsKey(key2))
			{
				val2 = new GUIStyle(GUI.foldoutStyle);
				if (flag)
				{
					val2.normal.textColor = val;
					val2.onNormal.textColor = val;
					val2.hover.textColor = Color.white;
					val2.onHover.textColor = Color.white;
				}
				else
				{
					val2.normal.textColor = new Color(val.r * 0.7f, val.g * 0.7f, val.b * 0.7f, 1f);
					val2.onNormal.textColor = val;
					val2.hover.textColor = new Color(val.r * 0.9f, val.g * 0.9f, val.b * 0.9f, 1f);
					val2.onHover.textColor = Color.white;
				}
				styleCache[key2] = val2;
			}
			else
			{
				val2 = styleCache[key2];
			}
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Space(4f);
			string text = (flag ? title.Replace("?", "") : title);
			bool flag2 = GUILayout.Toggle(flag, text, val2, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(36f) });
			GUILayout.EndHorizontal();
			if (flag2 != flag)
			{
				CheatConfig.CollapsibleSections[key] = flag2;
			}
			if (flag2)
			{
				GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), Array.Empty<GUILayoutOption>());
				GUILayout.Space(6f);
			}
			return flag2;
		}

		public static void EndCollapsibleSection()
		{
			GUILayout.EndVertical();
			GUILayout.Space(8f);
		}
	}
}
