using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _1v1.lol_cheat
{
	public static class VersionBypass
	{
		private static bool initialized = false;

		private static string originalAppVersion = "";

		private static PropertyInfo appVersionProperty = null;

		private static FieldInfo appVersionField = null;

		private static float lastDialogCheckTime = 0f;

		private const float dialogCheckInterval = 0.5f;

		private static string detectedHostVersion = null;

		private static FieldInfo internalAppVersionField = null;

		private static PropertyInfo internalAppVersionProperty = null;

		public static void Initialize()
		{
			if (initialized)
			{
				return;
			}
			try
			{
				originalAppVersion = PhotonNetwork.AppVersion;
				Type typeFromHandle = typeof(PhotonNetwork);
				appVersionProperty = typeFromHandle.GetProperty("AppVersion", BindingFlags.Static | BindingFlags.Public);
				if (appVersionProperty == null)
				{
					appVersionField = typeFromHandle.GetField("AppVersion", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				FindInternalVersionFields(typeFromHandle);
				Debug.Log((object)("[VersionBypass] Initialized. Original version: " + originalAppVersion));
				initialized = true;
				if (CheatConfig.VersionBypassEnabled)
				{
					SetAppVersion("1.46");
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[VersionBypass] Error initializing: " + ex.Message));
			}
		}

		private static void FindInternalVersionFields(Type photonNetworkType)
		{
			try
			{
				FieldInfo[] fields = photonNetworkType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (FieldInfo fieldInfo in fields)
				{
					string text = fieldInfo.Name.ToLower();
					if ((text.Contains("appversion") || text.Contains("app_version") || text.Contains("version")) && fieldInfo.FieldType == typeof(string))
					{
						internalAppVersionField = fieldInfo;
						Debug.Log((object)("[VersionBypass] Found internal version field: " + fieldInfo.Name));
					}
				}
				PropertyInfo[] properties = photonNetworkType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (PropertyInfo propertyInfo in properties)
				{
					string text2 = propertyInfo.Name.ToLower();
					if ((text2.Contains("appversion") || text2.Contains("app_version") || text2.Contains("version")) && propertyInfo.PropertyType == typeof(string))
					{
						internalAppVersionProperty = propertyInfo;
						Debug.Log((object)("[VersionBypass] Found internal version property: " + propertyInfo.Name));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning((object)("[VersionBypass] Error finding internal version fields: " + ex.Message));
			}
		}

		private static float lastVersionCheckTime = 0f;
		private const float VERSION_CHECK_INTERVAL = 2f;

		public static void Update()
		{
			if (!CheatConfig.VersionBypassEnabled)
			{
				return;
			}
			try
			{
				if (!initialized)
				{
					Initialize();
				}
				float time = Time.time;
				if (time - lastVersionCheckTime < VERSION_CHECK_INTERVAL)
				{
					return;
				}
				lastVersionCheckTime = time;
				string text = DetectHostVersionFromDialogs();
				if (text != null)
				{
					detectedHostVersion = text;
				}
				string text2 = detectedHostVersion ?? "1.46";
				if (PhotonNetwork.AppVersion != text2)
				{
					SetAppVersion(text2);
				}
				HideVersionDialog();
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[VersionBypass] Error in Update: " + ex.Message));
			}
		}

		private static string DetectHostVersionFromDialogs()
		{
			try
			{
				GameObject[] array = Object.FindObjectsByType<GameObject>((FindObjectsSortMode)0);
				foreach (GameObject val in array)
				{
					if ((Object)(object)val == (Object)null || !val.activeInHierarchy)
					{
						continue;
					}
					TextMeshProUGUI[] componentsInChildren = val.GetComponentsInChildren<TextMeshProUGUI>(true);
					foreach (TextMeshProUGUI val2 in componentsInChildren)
					{
						if ((Object)(object)val2 != (Object)null && ((TMP_Text)val2).text != null)
						{
							string text = ExtractHostVersion(((TMP_Text)val2).text);
							if (text != null)
							{
								return text;
							}
						}
					}
					Text[] componentsInChildren2 = val.GetComponentsInChildren<Text>(true);
					foreach (Text val3 in componentsInChildren2)
					{
						if ((Object)(object)val3 != (Object)null && val3.text != null)
						{
							string text2 = ExtractHostVersion(val3.text);
							if (text2 != null)
							{
								return text2;
							}
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		private static string ExtractHostVersion(string text)
		{
			try
			{
				string text2 = text.ToLower();
				if (text2.Contains("host has different game version") || text2.Contains("version mismatch") || text2.Contains("game version mismatch"))
				{
					int num = text.IndexOf("version:", StringComparison.OrdinalIgnoreCase);
					if (num == -1)
					{
						num = text.IndexOf("version", StringComparison.OrdinalIgnoreCase);
					}
					if (num >= 0)
					{
						int num2 = text.IndexOf('[', num);
						if (num2 >= 0)
						{
							int num3 = text.IndexOf(']', num2);
							if (num3 > num2)
							{
								string text3 = text.Substring(num2 + 1, num3 - num2 - 1).Trim();
								if (Regex.IsMatch(text3, "^\\d+\\.\\d+$"))
								{
									Debug.Log((object)("[VersionBypass] Detected host version from dialog: " + text3));
									return text3;
								}
							}
						}
					}
					Match match = Regex.Match(text, "\\[?(\\d+\\.\\d+)\\]?");
					if (match.Success)
					{
						string value = match.Groups[1].Value;
						Debug.Log((object)("[VersionBypass] Detected host version from text: " + value));
						return value;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		private static void HideVersionDialog()
		{
			try
			{
				float time = Time.time;
				if (time - lastDialogCheckTime < 0.5f)
				{
					return;
				}
				lastDialogCheckTime = time;
				GameObject[] array = Object.FindObjectsByType<GameObject>((FindObjectsSortMode)0);
				foreach (GameObject val in array)
				{
					if ((Object)(object)val == (Object)null || !val.activeInHierarchy)
					{
						continue;
					}
					string text = ((Object)val).name.ToLower();
					if (!text.Contains("version") && !text.Contains("update") && !text.Contains("dialog") && !text.Contains("outofdate"))
					{
						continue;
					}
					bool flag = false;
					TextMeshProUGUI[] componentsInChildren = val.GetComponentsInChildren<TextMeshProUGUI>(true);
					foreach (TextMeshProUGUI val2 in componentsInChildren)
					{
						if ((Object)(object)val2 != (Object)null && ((TMP_Text)val2).text != null)
						{
							string text2 = ((TMP_Text)val2).text.ToLower();
							if (text2.Contains("version out of date") || text2.Contains("close and update") || text2.Contains("game version mismatch") || text2.Contains("version mismatch") || text2.Contains("host has different game version") || text2.Contains("version"))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						Text[] componentsInChildren2 = val.GetComponentsInChildren<Text>(true);
						foreach (Text val3 in componentsInChildren2)
						{
							if ((Object)(object)val3 != (Object)null && val3.text != null)
							{
								string text3 = val3.text.ToLower();
								if (text3.Contains("version out of date") || text3.Contains("close and update") || text3.Contains("game version mismatch") || text3.Contains("version mismatch") || text3.Contains("host has different game version") || text3.Contains("version"))
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (flag)
					{
						val.SetActive(false);
						Debug.Log((object)("[VersionBypass] Hid version dialog: " + ((Object)val).name));
					}
				}
			}
			catch
			{
			}
		}

		private static void SetAppVersion(string version)
		{
			try
			{
				bool flag = false;
				if (appVersionProperty != null)
				{
					appVersionProperty.SetValue(null, version);
					Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via public property)"));
					flag = true;
				}
				if (appVersionField != null)
				{
					appVersionField.SetValue(null, version);
					Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via public field)"));
					flag = true;
				}
				if (internalAppVersionProperty != null && internalAppVersionProperty.CanWrite)
				{
					try
					{
						internalAppVersionProperty.SetValue(null, version);
						Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via internal property)"));
						flag = true;
					}
					catch
					{
					}
				}
				if (internalAppVersionField != null)
				{
					try
					{
						internalAppVersionField.SetValue(null, version);
						Debug.Log((object)("[VersionBypass] Set AppVersion to: " + version + " (via internal field)"));
						flag = true;
					}
					catch
					{
					}
				}
				try
				{
					if ((Object)(object)PhotonNetwork.PhotonServerSettings != (Object)null && PhotonNetwork.PhotonServerSettings.AppSettings != null)
					{
						PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = version;
						Debug.Log((object)("[VersionBypass] Set PhotonServerSettings.AppSettings.AppVersion to: " + version));
						flag = true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning((object)("[VersionBypass] Could not set PhotonServerSettings.AppSettings.AppVersion: " + ex.Message));
				}
				try
				{
					Type type = typeof(PhotonNetwork).Assembly.GetType("Photon.Realtime.LoadBalancingClient");
					if (type != null)
					{
						FieldInfo field = type.GetField("AppVersion", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null)
						{
							PropertyInfo property = typeof(PhotonNetwork).GetProperty("NetworkingClient", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
							if (property != null)
							{
								object value = property.GetValue(null);
								if (value != null)
								{
									field.SetValue(value, version);
									Debug.Log((object)("[VersionBypass] Set AppVersion in LoadBalancingClient to: " + version));
									flag = true;
								}
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogWarning((object)("[VersionBypass] Could not set version in LoadBalancingClient: " + ex2.Message));
				}
				if (!flag)
				{
					Debug.LogWarning((object)"[VersionBypass] Could not find any AppVersion property/field to set");
				}
			}
			catch (Exception ex3)
			{
				Debug.LogError((object)("[VersionBypass] Error setting AppVersion: " + ex3.Message));
			}
		}

		public static bool ShouldBypassJoinError(short returnCode, string message)
		{
			if (!CheatConfig.VersionBypassEnabled)
			{
				return false;
			}
			if (message != null)
			{
				string text = message.ToLower();
				if (text.Contains("version") || text.Contains("mismatch"))
				{
					string text2 = ExtractHostVersion(message);
					if (text2 != null)
					{
						detectedHostVersion = text2;
						Debug.Log((object)("[VersionBypass] Detected host version from error: " + text2));
						SetAppVersion(text2);
					}
					Debug.Log((object)$"[VersionBypass] Detected version mismatch error: {message} (ReturnCode: {returnCode})");
					return true;
				}
			}
			return false;
		}

		public static void RestoreOriginalVersion()
		{
			if (!string.IsNullOrEmpty(originalAppVersion))
			{
				SetAppVersion(originalAppVersion);
				Debug.Log((object)("[VersionBypass] Restored original version: " + originalAppVersion));
			}
		}

		public static string GetDetectedHostVersion()
		{
			return detectedHostVersion;
		}

		public static void EnsureVersionSet()
		{
			if (!CheatConfig.VersionBypassEnabled)
			{
				return;
			}
			try
			{
				string text = detectedHostVersion ?? "1.46";
				if (PhotonNetwork.AppVersion != text)
				{
					SetAppVersion(text);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[VersionBypass] Error ensuring version set: " + ex.Message));
			}
		}
	}
}
