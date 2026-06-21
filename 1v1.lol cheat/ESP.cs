	public static class ESP
	{
		private class EnvironmentalEventInfo
		{
			public string EventType;

			public float NextEventTime;

			public Color Color;
		}

		private static readonly string WATERMARK_TEXT = "protonn - PEAK";

		private static readonly string WATERMARK_CHECK = "protonn - PEAK";

		private static readonly int WATERMARK_HASH = WATERMARK_TEXT.GetHashCode();

		private static GUIStyle _textStyle;

		private static GUIStyle _outlineStyle;

		private static Texture2D _whiteTexture;

		private static Material _lineMaterial;

		private static Material _chamsMaterial;

		private static List<Item> _cachedItems = new List<Item>();

		private static List<GameObject> _cachedLuggage = new List<GameObject>();

		private static List<GameObject> _cachedEntities = new List<GameObject>();

		private static List<GameObject> _cachedSporeShrooms = new List<GameObject>();

		private static List<GameObject> _cachedEnvironmentalEvents = new List<GameObject>();

		private static List<GameObject> _cachedObjects = new List<GameObject>();

		private static HashSet<GameObject> _checkedGameObjects = new HashSet<GameObject>();

		private static bool _initialCacheComplete = false;

		private static float _itemCacheTime = 0f;

		private static float _luggageCacheTime = 0f;

		private static float _entityCacheTime = 0f;

		private static float _sporeShroomCacheTime = 0f;

		private static float _environmentalCacheTime = 0f;

		private static float _objectCacheTime = 0f;

		private static int _frameCounter = 0;

		private static List<Character> _cachedTargets = new List<Character>();

		private static float _targetsCacheTime = 0f;

		private const float TARGETS_CACHE_INTERVAL = 2f;

		private const float CACHE_UPDATE_INTERVAL = 20f;

		private const float MAX_RENDER_DISTANCE = 50f;

		private const int MAX_SKELETONS_PER_FRAME = 2;

		private const int MAX_BOXES_PER_FRAME = 3;

		private const int MAX_ENTITIES_PER_FRAME = 3;

		private const int RENDER_SKIP_FRAMES = 2;

		private static Type _luggageComponentType = null;

		private static WindChillZone _cachedWindZone = null;

		private static float _windZoneCacheTime = 0f;

		private static Object[] _cachedStormVisuals = null;

		private static float _stormVisualsCacheTime = 0f;

		private static Type _stormVisualType = null;

		private static FieldInfo _untilSwitchField = null;

		private static FieldInfo _windActiveField = null;

		private static FieldInfo _stormTypeField = null;

		private static Type _lavaRisingType = null;

		private static Object _cachedLavaRising = null;

		private static float _lavaRisingCacheTime = 0f;

		private static FieldInfo _timeTraveledField = null;

		private static FieldInfo _travelTimeField = null;

		private static FieldInfo _startedField = null;

		private static FieldInfo _endedField = null;

		private static PropertyInfo _timeTraveledProp = null;

		private static PropertyInfo _travelTimeProp = null;

		private static PropertyInfo _startedProp = null;

		private static PropertyInfo _endedProp = null;

		private const float WEATHER_CACHE_UPDATE_INTERVAL = 1f;

		private static readonly Color PlayerColor = new Color(0.2f, 0.8f, 1f);

		private static readonly Color ZombieColor = new Color(1f, 0.2f, 0.2f);

		private static readonly Color ScoutmasterColor = new Color(1f, 0.6f, 0f);

		private static readonly Color BeetleColor = new Color(0.6f, 0.3f, 0.1f);

		private static readonly Color ItemColor = new Color(0.8f, 0.8f, 0.2f);

		private static readonly Color LuggageColor = new Color(1f, 0.84f, 0f);

		private static ChamsRenderer _chamsRenderer = null;

		private static float _fpsUpdateInterval = 0.5f;

		private static float _fpsAccumulator = 0f;

		private static int _fpsFrames = 0;

		private static float _fpsTimeLeft = 0f;

		private static int _currentFPS = 0;

		private static void InitializeStyles()
		{
			if (_textStyle == null)
			{
				_textStyle = new GUIStyle();
				_textStyle.fontSize = 12;
				_textStyle.fontStyle = (FontStyle)1;
				_textStyle.alignment = (TextAnchor)4;
				_textStyle.normal.textColor = Color.white;
			}
			if (_outlineStyle == null)
			{
				_outlineStyle = new GUIStyle(_textStyle);
				_outlineStyle.normal.textColor = Color.black;
			}
			if ((Object)(object)_whiteTexture == (Object)null)
			{
				_whiteTexture = new Texture2D(1, 1);
				_whiteTexture.SetPixel(0, 0, Color.white);
				_whiteTexture.Apply();
			}
			if ((Object)(object)_lineMaterial == (Object)null)
			{
				_lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
				((Object)_lineMaterial).hideFlags = (HideFlags)61;
				_lineMaterial.SetInt("_SrcBlend", 5);
				_lineMaterial.SetInt("_DstBlend", 10);
				_lineMaterial.SetInt("_Cull", 0);
				_lineMaterial.SetInt("_ZWrite", 0);
			}
			if (!((Object)(object)_chamsMaterial == (Object)null))
			{
				return;
			}
			Shader val = Shader.Find("Unlit/Color");
			if ((Object)(object)val == (Object)null)
			{
				val = Shader.Find("Hidden/Internal-Colored");
			}
			if ((Object)(object)val == (Object)null)
			{
				val = Shader.Find("Standard");
			}
			_chamsMaterial = new Material(val);
			((Object)_chamsMaterial).hideFlags = (HideFlags)61;
			_chamsMaterial.renderQueue = 5000;
			try
			{
				if (_chamsMaterial.HasProperty("_ZWrite"))
				{
					_chamsMaterial.SetInt("_ZWrite", 0);
				}
				if (_chamsMaterial.HasProperty("_ZTest"))
				{
					_chamsMaterial.SetInt("_ZTest", 8);
				}
				if (_chamsMaterial.HasProperty("_SrcBlend"))
				{
					_chamsMaterial.SetInt("_SrcBlend", 5);
				}
				if (_chamsMaterial.HasProperty("_DstBlend"))
				{
					_chamsMaterial.SetInt("_DstBlend", 10);
				}
				if (_chamsMaterial.HasProperty("_Cull"))
				{
					_chamsMaterial.SetInt("_Cull", 0);
				}
				if (_chamsMaterial.HasProperty("_Glossiness"))
				{
					_chamsMaterial.SetFloat("_Glossiness", 0f);
				}
				if (_chamsMaterial.HasProperty("_Metallic"))
				{
					_chamsMaterial.SetFloat("_Metallic", 0f);
				}
			}
			catch
			{
			}
		}

		private static Vector3 GetLocalPlayerPosition(Vector3 fallbackPos)
		{
			if ((Object)(object)Character.localCharacter != (Object)null && (Object)(object)((MonoBehaviourPun)Character.localCharacter).photonView != (Object)null && ((MonoBehaviourPun)Character.localCharacter).photonView.IsMine && (Object)(object)Character.localCharacter.player != (Object)null)
			{
				return Character.localCharacter.Center;
			}
			if (Character.AllCharacters != null)
			{
				foreach (Character allCharacter in Character.AllCharacters)
				{
					if ((Object)(object)allCharacter != (Object)null && (Object)(object)((MonoBehaviourPun)allCharacter).photonView != (Object)null && ((MonoBehaviourPun)allCharacter).photonView.IsMine && (Object)(object)allCharacter.player != (Object)null)
					{
						return allCharacter.Center;
					}
				}
			}
			return fallbackPos;
		}

		public static void ResetCaches()
		{
			_cachedItems.Clear();
			_cachedLuggage.Clear();
			_cachedEntities.Clear();
			_cachedSporeShrooms.Clear();
			_cachedEnvironmentalEvents.Clear();
			_cachedObjects.Clear();
			_cachedTargets.Clear();
			_checkedGameObjects.Clear();
			_initialCacheComplete = false;
			_itemCacheTime = 0f;
			_luggageCacheTime = 0f;
			_entityCacheTime = 0f;
			_sporeShroomCacheTime = 0f;
			_environmentalCacheTime = 0f;
			_targetsCacheTime = 0f;
			Debug.Log((object)"[ESP] Caches reset - ready for new level");
		}

		public static void RenderAll()
		{
			try
			{
				if (!((Object)(object)Camera.main == (Object)null))
				{
					_frameCounter++;
					if (_frameCounter % 2 == 0)
					{
						InitializeStyles();
						InitializeChamsRenderer();
						Vector3 position = ((Component)Camera.main).transform.position;
						Vector3 localPlayerPosition = GetLocalPlayerPosition(position);
						UpdateCaches(position);
						RenderWithGL(position, localPlayerPosition);
						RenderTextOverlays(localPlayerPosition);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ESP] Error in RenderAll: " + ex.Message));
			}
		}

		private static void RenderWithGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			try
			{
				if (!((Object)(object)Camera.main == (Object)null) && !((Object)(object)_lineMaterial == (Object)null))
				{
					_lineMaterial.SetPass(0);
					GL.PushMatrix();
					GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
					GL.modelview = Camera.main.worldToCameraMatrix;
					GL.Begin(1);
					if (CheatConfig.PlayerBoxESP || CheatConfig.PlayerSkeletonESP)
					{
						RenderPlayersGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.EntityBoxESP || CheatConfig.EntitySkeletonESP)
					{
						RenderEntitiesGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.ItemBoxESP)
					{
						RenderItemsGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.LuggageBoxESP)
					{
						RenderLuggageGL(cameraPos, localPlayerPos);
					}
					if (CheatConfig.SporeShroomESP)
					{
						RenderSporeShroomsGL(cameraPos, localPlayerPos);
					}
					GL.End();
					GL.PopMatrix();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("[ESP] Error in RenderWithGL: " + ex.Message));
				try
				{
					GL.End();
					GL.PopMatrix();
				}
				catch
				{
				}
			}
		}

		public static void InitializeChamsRenderer()
		{
			if (!((Object)(object)_chamsRenderer != (Object)null) && !((Object)(object)Camera.main == (Object)null))
			{
				_chamsRenderer = ((Component)Camera.main).GetComponent<ChamsRenderer>();
				if ((Object)(object)_chamsRenderer == (Object)null)
				{
					_chamsRenderer = ((Component)Camera.main).gameObject.AddComponent<ChamsRenderer>();
				}
			}
		}

		public static void RenderChamsPostRender()
		{
		}

		public static void RenderChams()
		{
			InitializeChamsRenderer();
		}

		private static void RenderTextOverlays(Vector3 localPlayerPos)
		{
			if (CheatConfig.PlayerNameESP || CheatConfig.PlayerDistanceESP || CheatConfig.PlayerHealthESP)
			{
				RenderPlayersText(localPlayerPos);
			}
			if (CheatConfig.EntityNameESP || CheatConfig.EntityDistanceESP || CheatConfig.EntityAIStateESP)
			{
				RenderEntitiesText(localPlayerPos);
			}
			if (CheatConfig.ItemNameESP || CheatConfig.ItemDistanceESP)
			{
				RenderItemsText(localPlayerPos);
			}
			if (CheatConfig.LuggageNameESP || CheatConfig.LuggageDistanceESP)
			{
				RenderLuggageText(localPlayerPos);
			}
			if (CheatConfig.EnvironmentalESP)
			{
				RenderWeatherTimer();
			}
			if (CheatConfig.ObjectNameESP)
			{
				RenderObjectNames(localPlayerPos);
			}
		}

		private static void UpdateCaches(Vector3 cameraPos)
		{
			float time = Time.time;
			if (!_initialCacheComplete)
			{
				UpdateItemCache(cameraPos);
				UpdateLuggageCache(cameraPos);
				UpdateSporeShroomCache(cameraPos);
				UpdateEntityCache(cameraPos);
				UpdateEnvironmentalCache(cameraPos);
				_initialCacheComplete = true;
				_itemCacheTime = time;
				_luggageCacheTime = time;
				_sporeShroomCacheTime = time;
				_entityCacheTime = time;
				_environmentalCacheTime = time;
				return;
			}
			if (time - _itemCacheTime > 1f)
			{
				UpdateItemCache(cameraPos);
				_itemCacheTime = time;
			}
			if (time - _luggageCacheTime > 2f)
			{
				UpdateLuggageCache(cameraPos);
				_luggageCacheTime = time;
			}
			if (time - _sporeShroomCacheTime > 2f)
			{
				UpdateSporeShroomCache(cameraPos);
				_sporeShroomCacheTime = time;
			}
			if (time - _entityCacheTime > 2f)
			{
				UpdateEntityCache(cameraPos);
				_entityCacheTime = time;
			}
			if (time - _environmentalCacheTime > 20f)
			{
				UpdateEnvironmentalCache(cameraPos);
				_environmentalCacheTime = time;
			}
		}

		private static void UpdateItemCache(Vector3 cameraPos)
		{
			Vector3 localPlayerPosition = GetLocalPlayerPosition(cameraPos);
			_cachedItems.RemoveAll((Item item) => (Object)(object)item == (Object)null || (Object)(object)((Component)item).gameObject == (Object)null);
			Item[] array = Object.FindObjectsByType<Item>((FindObjectsSortMode)0);
			HashSet<Item> currentItemsInRange = new HashSet<Item>();
			Item[] array2 = array;
			foreach (Item val in array2)
			{
				if (!((Object)(object)val == (Object)null) && !((Object)(object)((Component)val).gameObject == (Object)null) && Vector3.Distance(localPlayerPosition, ((Component)val).transform.position) <= CheatConfig.ItemESPMaxDistance)
				{
					currentItemsInRange.Add(val);
					if (!_cachedItems.Contains(val))
					{
						_cachedItems.Add(val);
					}
				}
			}
			_cachedItems.RemoveAll((Item item) => (Object)(object)item == (Object)null || !currentItemsInRange.Contains(item));
		}

		private static void UpdateLuggageCache(Vector3 cameraPos)
		{
			_cachedLuggage.RemoveAll((GameObject luggage) => (Object)(object)luggage == (Object)null);
			Vector3 localPlayerPosition = GetLocalPlayerPosition(cameraPos);
			if (_luggageComponentType == null)
			{
				try
				{
					string[] array = new string[6] { "Luggage", "Container", "Storage", "Chest", "Suitcase", "Bag" };
					foreach (string typeName in array)
					{
						Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == typeName && typeof(MonoBehaviour).IsAssignableFrom(t));
						if (type != null)
						{
							_luggageComponentType = type;
							break;
						}
					}
				}
				catch
				{
				}
			}
			HashSet<GameObject> currentLuggageInRange = new HashSet<GameObject>();
			if (_luggageComponentType != null && Object.FindObjectsByType(_luggageComponentType, (FindObjectsSortMode)0) is MonoBehaviour[] array2)
			{
				MonoBehaviour[] array3 = array2;
				foreach (MonoBehaviour val in array3)
				{
					if (!((Object)(object)val == (Object)null) && !((Object)(object)((Component)val).gameObject == (Object)null) && Vector3.Distance(localPlayerPosition, ((Component)val).transform.position) <= CheatConfig.LuggageESPMaxDistance)
					{
						currentLuggageInRange.Add(((Component)val).gameObject);
						if (!_cachedLuggage.Contains(((Component)val).gameObject))
						{
							_cachedLuggage.Add(((Component)val).gameObject);
						}
					}
				}
			}
			PhotonView[] array4 = Object.FindObjectsByType<PhotonView>((FindObjectsSortMode)0);
			foreach (PhotonView val2 in array4)
			{
				if ((Object)(object)val2 == (Object)null || (Object)(object)((Component)val2).gameObject == (Object)null)
				{
					continue;
				}
				string text = ((Object)((Component)val2).gameObject).name.ToLower();
				if ((text.Contains("luggage") || text.Contains("suitcase") || text.Contains("chest") || text.Contains("container") || text.Contains("bag") || text.Contains("case")) && !currentLuggageInRange.Contains(((Component)val2).gameObject) && Vector3.Distance(localPlayerPosition, ((Component)val2).transform.position) <= CheatConfig.LuggageESPMaxDistance)
				{
					currentLuggageInRange.Add(((Component)val2).gameObject);
					if (!_cachedLuggage.Contains(((Component)val2).gameObject))
					{
						_cachedLuggage.Add(((Component)val2).gameObject);
					}
				}
			}
			_cachedLuggage.RemoveAll((GameObject luggage) => (Object)(object)luggage == (Object)null || !currentLuggageInRange.Contains(luggage));
		}

		private static void UpdateEntityCache(Vector3 cameraPos)
		{
			_cachedEntities.RemoveAll((GameObject entity) => (Object)(object)entity == (Object)null);
			GetLocalPlayerPosition(cameraPos);
			try
			{
				Character[] array = Object.FindObjectsByType<Character>((FindObjectsSortMode)0);
				int num = 0;
				Character[] array2 = array;
				foreach (Character val in array2)
				{
					if ((Object)(object)val == (Object)null || (Object)(object)((Component)val).gameObject == (Object)null || (Object)(object)val == (Object)(object)Character.localCharacter || (_initialCacheComplete && _checkedGameObjects.Contains(((Component)val).gameObject) && !_cachedEntities.Contains(((Component)val).gameObject)))
					{
						continue;
					}
					bool flag = false;
					bool flag2 = false;
					try
					{
						FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
						FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
						FieldInfo field3 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
						if (field != null && (bool)field.GetValue(val))
						{
							flag = true;
						}
						if (field2 != null && (bool)field2.GetValue(val))
						{
							flag = true;
						}
						if (field3 != null && (bool)field3.GetValue(val))
						{
							flag = true;
						}
						if (!flag)
						{
							if ((Object)(object)val.player != (Object)null)
							{
								flag2 = true;
							}
							else if ((Object)(object)((Component)val).gameObject.GetComponent<Player>() != (Object)null)
							{
								flag2 = true;
							}
							else
							{
								flag = true;
							}
						}
					}
					catch
					{
						if ((Object)(object)val.player == (Object)null)
						{
							flag = true;
						}
						else
						{
							flag2 = true;
						}
					}
					_checkedGameObjects.Add(((Component)val).gameObject);
					if (!flag2 && flag && !_cachedEntities.Contains(((Component)val).gameObject))
					{
						_cachedEntities.Add(((Component)val).gameObject);
						num++;
					}
				}
				try
				{
					Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Beetle");
					if (type != null)
					{
						Object[] array3 = Object.FindObjectsByType(type, (FindObjectsSortMode)0);
						foreach (Object val2 in array3)
						{
							if (!(val2 == (Object)null))
							{
								GameObject val3 = (GameObject)(object)((val2 is GameObject) ? val2 : null);
								if (!((Object)(object)val3 == (Object)null) && !_cachedEntities.Contains(val3) && !_cachedEntities.Contains(val3))
								{
									_cachedEntities.Add(val3);
									num++;
								}
							}
						}
					}
					Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "Mob" && t.IsClass);
					if (type2 != null)
					{
						Object[] array3 = Object.FindObjectsByType(type2, (FindObjectsSortMode)0);
						foreach (Object val4 in array3)
						{
							if (!(val4 == (Object)null))
							{
								GameObject val5 = (GameObject)(object)((val4 is GameObject) ? val4 : null);
								if (!((Object)(object)val5 == (Object)null) && !_cachedEntities.Contains(val5) && !((Object)(object)val5.GetComponent<Character>() != (Object)null))
								{
									_cachedEntities.Add(val5);
									num++;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.Log((object)("[ESP] Error finding additional entities: " + ex.Message));
				}
				if (num > 0)
				{
					Debug.Log((object)$"[ESP] Updated entity cache: {num} entities found (all non-player entities)");
				}
			}
			catch (Exception ex2)
			{
				Debug.LogError((object)("[ESP] Error updating entity cache: " + ex2.Message + "\n" + ex2.StackTrace));
			}
		}

		private static void UpdateEnvironmentalCache(Vector3 cameraPos)
		{
			_cachedEnvironmentalEvents.Clear();
		}

		private static void UpdateObjectCache()
		{
			_cachedObjects.Clear();
		}

		private static void RenderPlayersGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			float time = Time.time;
			if (_cachedTargets.Count == 0 || time - _targetsCacheTime > 2f)
			{
				if (CheatConfig.Targets != null && CheatConfig.Targets.Count > 0)
				{
					_cachedTargets = new List<Character>(CheatConfig.Targets);
				}
				else
				{
					_cachedTargets.Clear();
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
								if (flag)
								{
									_cachedTargets.Add(allCharacter);
								}
							}
						}
					}
				}
				_targetsCacheTime = time;
			}
			List<Character> cachedTargets = _cachedTargets;
			int num = 0;
			int num2 = 0;
			foreach (Character item in cachedTargets)
			{
				if ((Object)(object)item == (Object)null || (Object)(object)item == (Object)(object)Character.localCharacter || (Object)(object)((Component)item).transform == (Object)null || ((Object)(object)item.data != (Object)null && item.data.dead))
				{
					continue;
				}
				float num3 = Vector3.Distance(localPlayerPos, item.Center);
				if (num3 > 50f)
				{
					continue;
				}
				Color distanceColor = GetDistanceColor(num3, PlayerColor);
				if (CheatConfig.PlayerBoxESP && num < 3)
				{
					Bounds characterBounds = GetCharacterBounds(item);
					if (CheatConfig.PlayerBox3D)
					{
						Draw3DBox(characterBounds, distanceColor, 2f);
					}
					else
					{
						DrawSimple2DBox(characterBounds, distanceColor, 2f);
					}
					num++;
				}
				if (CheatConfig.PlayerSkeletonESP && num2 < 2)
				{
					DrawSkeleton(item, distanceColor);
					num2++;
				}
			}
		}

		private static void RenderPlayersText(Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			float time = Time.time;
			if (_cachedTargets.Count == 0 || time - _targetsCacheTime > 2f)
			{
				if (CheatConfig.Targets != null && CheatConfig.Targets.Count > 0)
				{
					_cachedTargets = new List<Character>(CheatConfig.Targets);
				}
				else
				{
					_cachedTargets.Clear();
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
								if (flag)
								{
									_cachedTargets.Add(allCharacter);
								}
							}
						}
					}
				}
				_targetsCacheTime = time;
			}
			foreach (Character cachedTarget in _cachedTargets)
			{
				if ((Object)(object)cachedTarget == (Object)null || (Object)(object)cachedTarget == (Object)(object)Character.localCharacter || (Object)(object)((Component)cachedTarget).transform == (Object)null || ((Object)(object)cachedTarget.data != (Object)null && cachedTarget.data.dead))
				{
					continue;
				}
				float num = Vector3.Distance(localPlayerPos, cachedTarget.Center);
				if (num > 50f)
				{
					continue;
				}
				CharacterRefs refs = cachedTarget.refs;
				Vector3? obj;
				if (refs == null)
				{
					obj = null;
				}
				else
				{
					Bodypart head = refs.head;
					obj = ((head != null) ? new Vector3?(((Component)head).transform.position) : null);
				}
				Vector3 val = (Vector3)(((??)obj) ?? (cachedTarget.Center + Vector3.up * 0.5f));
				Vector3 val2 = Camera.main.WorldToScreenPoint(val);
				if (val2.z <= 0f)
				{
					continue;
				}
				List<string> list = new List<string>();
				Color distanceColor = GetDistanceColor(num, PlayerColor);
				if (CheatConfig.PlayerNameESP)
				{
					Player player = cachedTarget.player;
					object obj2;
					if (player == null)
					{
						obj2 = null;
					}
					else
					{
						PhotonView photonView = ((MonoBehaviourPun)player).photonView;
						if (photonView == null)
						{
							obj2 = null;
						}
						else
						{
							Player owner = photonView.Owner;
							obj2 = ((owner != null) ? owner.NickName : null);
						}
					}
					if (obj2 == null)
					{
						obj2 = "Unknown";
					}
					string text = (string)obj2;
					if (text.Length > 64)
					{
						text = text.Substring(0, 61) + "...";
					}
					list.Add(text);
				}
				if (CheatConfig.PlayerDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				if (CheatConfig.PlayerHealthESP)
				{
					float characterHealth = GetCharacterHealth(cachedTarget);
					list.Add($"HP: {characterHealth:F0}%");
				}
				float num2 = 0f;
				foreach (string item in list)
				{
					DrawTextWithOutline(new Vector2(val2.x, (float)Screen.height - val2.y + num2), item, distanceColor, 14, num);
					num2 += 16f;
				}
			}
		}

		private static void RenderEntitiesGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null || (!CheatConfig.EntityBoxESP && !CheatConfig.EntitySkeletonESP))
			{
				return;
			}
			float time = Time.time;
			if (_cachedEntities.Count == 0 || time - _entityCacheTime > 20f)
			{
				UpdateEntityCache(cameraPos);
				_entityCacheTime = time;
			}
			if (_cachedEntities.Count == 0)
			{
				return;
			}
			GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (GameObject cachedEntity in _cachedEntities)
			{
				if ((Object)(object)cachedEntity == (Object)null || ((Object)(object)currentlyControlledEntity != (Object)null && (Object)(object)cachedEntity == (Object)(object)currentlyControlledEntity))
				{
					continue;
				}
				Character component = cachedEntity.GetComponent<Character>();
				if (Vector3.Distance(localPlayerPos, cachedEntity.transform.position) > 50f)
				{
					continue;
				}
				bool flag = false;
				try
				{
					int selectedEntityIndex = CheatConfig.SelectedEntityIndex;
					if (selectedEntityIndex >= 0)
					{
						List<GameObject> allEntitiesInGame = GUI.GetAllEntitiesInGame();
						if (allEntitiesInGame != null && selectedEntityIndex < allEntitiesInGame.Count && (Object)(object)allEntitiesInGame[selectedEntityIndex] == (Object)(object)cachedEntity)
						{
							flag = true;
						}
					}
				}
				catch
				{
				}
				if (!flag && num3 >= 3)
				{
					continue;
				}
				Color color = GetEntityColor(GetEntityType(cachedEntity));
				if (flag)
				{
					color = Color.yellow;
				}
				if (CheatConfig.EntityBoxESP && (flag || num < 3))
				{
					float lineWidth = (flag ? 3f : 2f);
					Bounds bounds = ((!((Object)(object)component != (Object)null)) ? GetObjectBounds(cachedEntity) : GetCharacterBounds(component));
					if (CheatConfig.EntityBox3D)
					{
						Draw3DBox(bounds, color, lineWidth);
					}
					else
					{
						DrawSimple2DBox(bounds, color, lineWidth);
					}
					if (!flag)
					{
						num++;
					}
				}
				if (CheatConfig.EntitySkeletonESP && (Object)(object)component != (Object)null && (flag || num2 < 2))
				{
					DrawSkeleton(component, color);
					if (!flag)
					{
						num2++;
					}
				}
				if (!flag)
				{
					num3++;
				}
			}
		}

		private static void RenderEntitiesText(Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null || (!CheatConfig.EntityNameESP && !CheatConfig.EntityAIStateESP) || _cachedEntities.Count == 0)
			{
				return;
			}
			foreach (GameObject cachedEntity in _cachedEntities)
			{
				if ((Object)(object)cachedEntity == (Object)null)
				{
					continue;
				}
				Character component = cachedEntity.GetComponent<Character>();
				float num = Vector3.Distance(localPlayerPos, cachedEntity.transform.position);
				Vector3 val = ((!((Object)(object)component != (Object)null)) ? (cachedEntity.transform.position + Vector3.up * 1.5f) : GetEntityHeadPosition(cachedEntity, component));
				Vector3 val2 = Camera.main.WorldToScreenPoint(val);
				if (val2.z <= 0f)
				{
					continue;
				}
				string entityType = GetEntityType(cachedEntity);
				Color entityColor = GetEntityColor(entityType);
				List<string> list = new List<string>();
				if (CheatConfig.EntityNameESP)
				{
					list.Add(entityType);
				}
				if (CheatConfig.EntityDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				if (CheatConfig.EntityAIStateESP)
				{
					string entityAIState = GetEntityAIState(cachedEntity, component);
					if (!string.IsNullOrEmpty(entityAIState))
					{
						list.Add("AI: " + entityAIState);
					}
				}
				float num2 = 0f;
				foreach (string item in list)
				{
					DrawTextWithOutline(new Vector2(val2.x, (float)Screen.height - val2.y + num2), item, entityColor, 13, num);
					num2 += 15f;
				}
			}
		}

		private static void RenderItemsGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			int num = 0;
			foreach (Item cachedItem in _cachedItems)
			{
				if ((Object)(object)cachedItem == (Object)null || (Object)(object)((Component)cachedItem).gameObject == (Object)null || IsItemHeld(cachedItem))
				{
					continue;
				}
				Vector3 position = ((Component)cachedItem).transform.position;
				float num2 = Vector3.Distance(localPlayerPos, position);
				if (!(num2 > CheatConfig.ItemESPMaxDistance) && !(Camera.main.WorldToScreenPoint(position).z <= 0f))
				{
					if (num >= 8)
					{
						break;
					}
					Bounds itemBounds = GetItemBounds(cachedItem);
					Color itemColor = GetItemColor(GetItemName(cachedItem));
					itemColor = GetDistanceColor(num2, itemColor);
					if (CheatConfig.ItemBoxESP)
					{
						Draw3DBox(itemBounds, itemColor, 1.5f);
						num++;
					}
				}
			}
		}

		private static bool IsItemHeld(Item item)
		{
			if ((Object)(object)item == (Object)null)
			{
				return false;
			}
			try
			{
				FieldInfo field = typeof(Item).GetField("_holderCharacter", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null && (Object)/*isinst with value type is only supported in some contexts*/ != (Object)null)
				{
					return true;
				}
			}
			catch
			{
			}
			try
			{
				PropertyInfo property = typeof(Item).GetProperty("itemState");
				if (property != null)
				{
					object value = property.GetValue(item);
					if (value != null && value.ToString() == "Held")
					{
						return true;
					}
				}
			}
			catch
			{
			}
			return false;
		}

		private static void RenderItemsChams(Vector3 cameraPos)
		{
			if ((Object)(object)Camera.main == (Object)null || (Object)(object)_chamsMaterial == (Object)null)
			{
				return;
			}
			Vector3 localPlayerPosition = GetLocalPlayerPosition(cameraPos);
			foreach (Item cachedItem in _cachedItems)
			{
				if ((Object)(object)cachedItem == (Object)null || (Object)(object)((Component)cachedItem).gameObject == (Object)null || IsItemHeld(cachedItem))
				{
					continue;
				}
				Vector3 position = ((Component)cachedItem).transform.position;
				if (!(Camera.main.WorldToScreenPoint(position).z <= 0f))
				{
					float distance = Vector3.Distance(localPlayerPosition, position);
					Color val = GetItemColor(GetItemName(cachedItem));
					val = GetDistanceColor(distance, val);
					val.a = 0.6f;
					if (val.r > 0.7f && val.g > 0.7f && val.b < 0.4f)
					{
						((Color)(ref val))..ctor(val.r, val.g * 0.6f, val.b * 0.3f);
					}
					RenderChamsForItem(((Component)cachedItem).gameObject, val);
				}
			}
		}

		private static void RenderChamsForItem(GameObject obj, Color color)
		{
			if ((Object)(object)obj == (Object)null || (Object)(object)_chamsMaterial == (Object)null || (Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>(true);
			MeshFilter[] componentsInChildren2 = obj.GetComponentsInChildren<MeshFilter>(true);
			Dictionary<Transform, MeshFilter> dictionary = new Dictionary<Transform, MeshFilter>();
			MeshFilter[] array = componentsInChildren2;
			foreach (MeshFilter val in array)
			{
				if ((Object)(object)val != (Object)null && (Object)(object)((Component)val).transform != (Object)null)
				{
					dictionary[((Component)val).transform] = val;
				}
			}
			Material val2 = new Material(_chamsMaterial);
			val2.color = color;
			MeshRenderer[] array2 = componentsInChildren;
			foreach (MeshRenderer val3 in array2)
			{
				if ((Object)(object)val3 == (Object)null)
				{
					continue;
				}
				Transform transform = ((Component)val3).transform;
				if (!dictionary.ContainsKey(transform))
				{
					continue;
				}
				MeshFilter val4 = dictionary[transform];
				if ((Object)(object)val4 == (Object)null)
				{
					continue;
				}
				Mesh sharedMesh = val4.sharedMesh;
				if (!((Object)(object)sharedMesh == (Object)null))
				{
					Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
					for (int j = 0; j < sharedMesh.subMeshCount; j++)
					{
						Graphics.DrawMesh(sharedMesh, localToWorldMatrix, val2, 0, Camera.main, j, (MaterialPropertyBlock)null, (ShadowCastingMode)0, false);
					}
				}
			}
			if ((Object)(object)val2 != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)val2);
			}
		}

		private static void RenderItemsText(Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			foreach (Item cachedItem in _cachedItems)
			{
				if ((Object)(object)cachedItem == (Object)null || (Object)(object)((Component)cachedItem).gameObject == (Object)null || IsItemHeld(cachedItem))
				{
					continue;
				}
				Vector3 position = ((Component)cachedItem).transform.position;
				float num = Vector3.Distance(localPlayerPos, position);
				if (num > CheatConfig.ItemESPMaxDistance)
				{
					continue;
				}
				Vector3 val = Camera.main.WorldToScreenPoint(position);
				if (val.z <= 0f)
				{
					continue;
				}
				string itemName = GetItemName(cachedItem);
				Color itemColor = GetItemColor(itemName);
				itemColor = GetDistanceColor(num, itemColor);
				List<string> list = new List<string>();
				if (CheatConfig.ItemNameESP)
				{
					list.Add(itemName);
				}
				if (CheatConfig.ItemDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				float num2 = -18f;
				foreach (string item in list)
				{
					DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y + num2), item, itemColor, 12, num);
					num2 += 14f;
				}
			}
		}

		private static void RenderLuggageGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			foreach (GameObject item in _cachedLuggage)
			{
				if (!((Object)(object)item == (Object)null))
				{
					Vector3 position = item.transform.position;
					float num = Vector3.Distance(localPlayerPos, position);
					if (!(num > CheatConfig.LuggageESPMaxDistance) && !(Camera.main.WorldToScreenPoint(position).z <= 0f))
					{
						Bounds luggageBounds = GetLuggageBounds(item);
						Color distanceColor = GetDistanceColor(num, LuggageColor);
						Draw3DBox(luggageBounds, distanceColor, 2f);
					}
				}
			}
		}

		private static Bounds GetLuggageBounds(GameObject luggage)
		{
			Bounds result;
			if ((Object)(object)luggage == (Object)null)
			{
				result = default(Bounds);
				return result;
			}
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(luggage.transform.position, Vector3.zero);
			bool flag = false;
			Renderer[] componentsInChildren = luggage.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (!((Object)(object)val != (Object)null))
				{
					continue;
				}
				result = val.bounds;
				Vector3 size = ((Bounds)(ref result)).size;
				if (((Vector3)(ref size)).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val.bounds);
					}
				}
			}
			Collider[] componentsInChildren2 = luggage.GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if ((Object)(object)val2 != (Object)null)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag)
			{
				MeshFilter[] componentsInChildren3 = luggage.GetComponentsInChildren<MeshFilter>(true);
				Bounds val6 = default(Bounds);
				foreach (MeshFilter val3 in componentsInChildren3)
				{
					if ((Object)(object)val3 != (Object)null && (Object)(object)val3.sharedMesh != (Object)null)
					{
						Bounds bounds = val3.sharedMesh.bounds;
						Vector3 val4 = ((Component)val3).transform.TransformPoint(((Bounds)(ref bounds)).center);
						Vector3 val5 = ((Component)val3).transform.TransformVector(((Bounds)(ref bounds)).size);
						((Bounds)(ref val6))..ctor(val4, val5);
						if (!flag)
						{
							result2 = val6;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val6);
						}
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(luggage.transform.position, Vector3.one * 1.5f);
			}
			return result2;
		}

		private static void RenderLuggageText(Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			foreach (GameObject item in _cachedLuggage)
			{
				if ((Object)(object)item == (Object)null)
				{
					continue;
				}
				Vector3 position = item.transform.position;
				float num = Vector3.Distance(localPlayerPos, position);
				if (num > CheatConfig.LuggageESPMaxDistance)
				{
					continue;
				}
				Vector3 val = Camera.main.WorldToScreenPoint(position);
				if (val.z <= 0f)
				{
					continue;
				}
				string luggageName = GetLuggageName(item);
				List<string> list = new List<string>();
				if (CheatConfig.LuggageNameESP)
				{
					list.Add(luggageName);
				}
				if (CheatConfig.LuggageDistanceESP)
				{
					list.Add($"{num:F1}m");
				}
				float num2 = -18f;
				foreach (string item2 in list)
				{
					DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y + num2), item2, LuggageColor, 11, num);
					num2 += 15f;
				}
			}
		}

		private static void UpdateSporeShroomCache(Vector3 cameraPos)
		{
			_cachedSporeShrooms.RemoveAll((GameObject shroom) => (Object)(object)shroom == (Object)null);
			try
			{
				PhotonView[] array;
				MonoBehaviour[] array2;
				if (!_initialCacheComplete)
				{
					array = Object.FindObjectsByType<PhotonView>((FindObjectsSortMode)0);
					foreach (PhotonView val in array)
					{
						if (!((Object)(object)val == (Object)null) && !((Object)(object)((Component)val).gameObject == (Object)null))
						{
							GameObject gameObject = ((Component)val).gameObject;
							if (IsSporeShroom(gameObject))
							{
								_checkedGameObjects.Add(gameObject);
								_cachedSporeShrooms.Add(gameObject);
							}
						}
					}
					array2 = Object.FindObjectsByType<MonoBehaviour>((FindObjectsSortMode)0);
					foreach (MonoBehaviour val2 in array2)
					{
						if (!((Object)(object)val2 == (Object)null) && !((Object)(object)((Component)val2).gameObject == (Object)null))
						{
							GameObject gameObject2 = ((Component)val2).gameObject;
							if (!_checkedGameObjects.Contains(gameObject2) && ((object)val2).GetType().Name == "TriggerEvent" && IsSporeShroom(gameObject2))
							{
								_checkedGameObjects.Add(gameObject2);
								_cachedSporeShrooms.Add(gameObject2);
							}
						}
					}
					return;
				}
				array = Object.FindObjectsByType<PhotonView>((FindObjectsSortMode)0);
				foreach (PhotonView val3 in array)
				{
					if ((Object)(object)val3 == (Object)null || (Object)(object)((Component)val3).gameObject == (Object)null)
					{
						continue;
					}
					GameObject gameObject3 = ((Component)val3).gameObject;
					if (!_checkedGameObjects.Contains(gameObject3))
					{
						_checkedGameObjects.Add(gameObject3);
						if (IsSporeShroom(gameObject3))
						{
							_cachedSporeShrooms.Add(gameObject3);
						}
					}
				}
				array2 = Object.FindObjectsByType<MonoBehaviour>((FindObjectsSortMode)0);
				foreach (MonoBehaviour val4 in array2)
				{
					if ((Object)(object)val4 == (Object)null || (Object)(object)((Component)val4).gameObject == (Object)null)
					{
						continue;
					}
					GameObject gameObject4 = ((Component)val4).gameObject;
					if (!_checkedGameObjects.Contains(gameObject4) && ((object)val4).GetType().Name == "TriggerEvent")
					{
						_checkedGameObjects.Add(gameObject4);
						if (IsSporeShroom(gameObject4))
						{
							_cachedSporeShrooms.Add(gameObject4);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("[ESP] Error updating spore shroom cache: " + ex.Message));
			}
		}

		private static bool IsSporeShroom(GameObject obj)
		{
			if ((Object)(object)obj == (Object)null || (Object)(object)obj.transform == (Object)null)
			{
				return false;
			}
			if ((Object)(object)obj.GetComponent<Item>() != (Object)null)
			{
				return false;
			}
			if ((Object)(object)obj.GetComponent<Character>() != (Object)null)
			{
				return false;
			}
			if (_luggageComponentType != null && (Object)(object)obj.GetComponent(_luggageComponentType) != (Object)null)
			{
				return false;
			}
			string text = (((Object)obj).name ?? "").ToLower();
			bool result = text.Contains("sporeshroom") || text.Contains("sporefungus") || text.Contains("spore_shroom") || text.Contains("spore_fungus") || (text.Contains("spore") && text.Contains("shroom")) || (text.Contains("spore") && text.Contains("fungus"));
			bool flag = false;
			bool flag2 = false;
			MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if (!((Object)(object)val == (Object)null))
				{
					string name = ((object)val).GetType().Name;
					if (name == "TriggerEvent")
					{
						flag = true;
					}
					if (name == "SpawnGameObject")
					{
						flag2 = true;
					}
				}
			}
			if (flag && flag2)
			{
				result = true;
			}
			return result;
		}

		private static void RenderSporeShroomsGL(Vector3 cameraPos, Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			float time = Time.time;
			if (_cachedSporeShrooms.Count == 0 || time - _sporeShroomCacheTime > 20f)
			{
				UpdateSporeShroomCache(cameraPos);
				_sporeShroomCacheTime = time;
			}
			Color distanceColor = default(Color);
			foreach (GameObject cachedSporeShroom in _cachedSporeShrooms)
			{
				if (!((Object)(object)cachedSporeShroom == (Object)null))
				{
					Vector3 position = cachedSporeShroom.transform.position;
					float num = Vector3.Distance(localPlayerPos, position);
					if (!(num > 200f) && !(Camera.main.WorldToScreenPoint(position).z <= 0f))
					{
						Bounds objectBounds = GetObjectBounds(cachedSporeShroom);
						((Color)(ref distanceColor))..ctor(0.8f, 0.2f, 1f, 1f);
						distanceColor = GetDistanceColor(num, distanceColor);
						Draw3DBox(objectBounds, distanceColor, 2f);
					}
				}
			}
		}

		private static void RenderSporeShroomsText(Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			Color distanceColor = default(Color);
			foreach (GameObject cachedSporeShroom in _cachedSporeShrooms)
			{
				if ((Object)(object)cachedSporeShroom == (Object)null)
				{
					continue;
				}
				Vector3 position = cachedSporeShroom.transform.position;
				float num = Vector3.Distance(localPlayerPos, position);
				if (!(num > 200f))
				{
					Vector3 val = Camera.main.WorldToScreenPoint(position);
					if (!(val.z <= 0f))
					{
						string text = ((Object)cachedSporeShroom).name ?? "SporeShroom";
						text = text.Replace("(Clone)", "").Trim();
						((Color)(ref distanceColor))..ctor(0.8f, 0.2f, 1f, 1f);
						distanceColor = GetDistanceColor(num, distanceColor);
						DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y), text, distanceColor, 12, num);
					}
				}
			}
		}

		private static void RenderWeatherTimer()
		{
			InitializeStyles();
			float num = 30f;
			float time = Time.time;
			if (time - _windZoneCacheTime > 1f)
			{
				_cachedWindZone = WindChillZone.instance;
				if ((Object)(object)_cachedWindZone == (Object)null)
				{
					_cachedWindZone = Object.FindFirstObjectByType<WindChillZone>();
				}
				_windZoneCacheTime = time;
				if (_untilSwitchField == null)
				{
					_untilSwitchField = typeof(WindChillZone).GetField("untilSwitch", BindingFlags.Instance | BindingFlags.NonPublic);
				}
				if (_windActiveField == null)
				{
					_windActiveField = typeof(WindChillZone).GetField("windActive", BindingFlags.Instance | BindingFlags.Public);
				}
			}
			if ((Object)(object)_cachedWindZone != (Object)null && _untilSwitchField != null && _windActiveField != null)
			{
				try
				{
					float num2 = (float)_untilSwitchField.GetValue(_cachedWindZone);
					bool flag = (bool)_windActiveField.GetValue(_cachedWindZone);
					if (num2 > 0f)
					{
						string obj = (flag ? "Wind Active" : "Next Wind");
						string text = FormatTime(num2);
						string text2 = obj + ": " + text;
						Color textColor = (flag ? new Color(0.5f, 0.8f, 1f) : new Color(0.7f, 0.7f, 0.7f));
						DrawTextWithOutline(new Vector2(150f, num), text2, textColor, 14);
						num += 20f;
					}
				}
				catch
				{
				}
			}
			if (time - _stormVisualsCacheTime > 1f)
			{
				if (_stormVisualType == null)
				{
					_stormVisualType = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "StormVisual");
					if (_stormVisualType != null)
					{
						_stormTypeField = _stormVisualType.GetField("stormType", BindingFlags.Instance | BindingFlags.Public);
					}
				}
				if (_stormVisualType != null)
				{
					_cachedStormVisuals = Object.FindObjectsByType(_stormVisualType, (FindObjectsSortMode)0);
				}
				_stormVisualsCacheTime = time;
			}
			if (_cachedStormVisuals != null && _stormVisualType != null && _stormTypeField != null)
			{
				try
				{
					Object[] cachedStormVisuals = _cachedStormVisuals;
					foreach (Object val in cachedStormVisuals)
					{
						if (val == (Object)null)
						{
							continue;
						}
						object value = _stormTypeField.GetValue(val);
						if (value == null)
						{
							continue;
						}
						string text3 = value.ToString();
						if (!text3.Contains("Snow") && !text3.Contains("Alpine"))
						{
							continue;
						}
						FieldInfo field = _stormVisualType.GetField("zone", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (!(field != null))
						{
							break;
						}
						object value2 = field.GetValue(val);
						WindChillZone val2 = (WindChillZone)((value2 is WindChillZone) ? value2 : null);
						if ((Object)(object)val2 != (Object)null && _untilSwitchField != null && _windActiveField != null)
						{
							float num3 = (float)_untilSwitchField.GetValue(val2);
							bool flag2 = (bool)_windActiveField.GetValue(val2);
							if (num3 > 0f)
							{
								string obj3 = (flag2 ? "Alpine Blizzard Active" : "Next Alpine Blizzard");
								string text4 = FormatTime(num3);
								string text5 = obj3 + ": " + text4;
								Color textColor2 = (flag2 ? new Color(0.7f, 0.9f, 1f) : new Color(0.8f, 0.8f, 0.9f));
								DrawTextWithOutline(new Vector2(150f, num), text5, textColor2, 14);
								num += 20f;
							}
						}
						break;
					}
				}
				catch
				{
				}
			}
			if (time - _lavaRisingCacheTime > 1f)
			{
				if (_lavaRisingType == null)
				{
					_lavaRisingType = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "LavaRising");
					if (_lavaRisingType != null)
					{
						_timeTraveledProp = _lavaRisingType.GetProperty("timeTraveled", BindingFlags.Instance | BindingFlags.Public);
						_travelTimeProp = _lavaRisingType.GetProperty("travelTime", BindingFlags.Instance | BindingFlags.Public);
						_startedProp = _lavaRisingType.GetProperty("started", BindingFlags.Instance | BindingFlags.Public);
						_endedProp = _lavaRisingType.GetProperty("ended", BindingFlags.Instance | BindingFlags.Public);
						if (_timeTraveledProp == null)
						{
							_timeTraveledField = _lavaRisingType.GetField("timeTraveled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (_travelTimeProp == null)
						{
							_travelTimeField = _lavaRisingType.GetField("travelTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (_startedProp == null)
						{
							_startedField = _lavaRisingType.GetField("started", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (_endedProp == null)
						{
							_endedField = _lavaRisingType.GetField("ended", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
					}
				}
				if (_lavaRisingType != null)
				{
					try
					{
						PropertyInfo property = _lavaRisingType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
						if (property == null)
						{
							property = _lavaRisingType.GetProperty("instance", BindingFlags.Static | BindingFlags.Public);
						}
						if (property != null)
						{
							object value3 = property.GetValue(null);
							_cachedLavaRising = (Object)((value3 is Object) ? value3 : null);
						}
						else
						{
							_cachedLavaRising = Object.FindFirstObjectByType(_lavaRisingType);
						}
					}
					catch
					{
					}
				}
				_lavaRisingCacheTime = time;
			}
			if (!(_cachedLavaRising != (Object)null) || !(_lavaRisingType != null))
			{
				return;
			}
			try
			{
				bool flag3 = false;
				bool flag4 = false;
				float num4 = 0f;
				float num5 = 60f;
				if (_startedProp != null)
				{
					object value4 = _startedProp.GetValue(_cachedLavaRising);
					if (value4 is bool)
					{
						flag3 = (bool)value4;
					}
				}
				else if (_startedField != null)
				{
					object value5 = _startedField.GetValue(_cachedLavaRising);
					if (value5 is bool)
					{
						flag3 = (bool)value5;
					}
				}
				if (_endedProp != null)
				{
					object value6 = _endedProp.GetValue(_cachedLavaRising);
					if (value6 is bool)
					{
						flag4 = (bool)value6;
					}
				}
				else if (_endedField != null)
				{
					object value7 = _endedField.GetValue(_cachedLavaRising);
					if (value7 is bool)
					{
						flag4 = (bool)value7;
					}
				}
				if (_timeTraveledProp != null)
				{
					object value8 = _timeTraveledProp.GetValue(_cachedLavaRising);
					if (value8 is float)
					{
						num4 = (float)value8;
					}
				}
				else if (_timeTraveledField != null)
				{
					object value9 = _timeTraveledField.GetValue(_cachedLavaRising);
					if (value9 is float)
					{
						num4 = (float)value9;
					}
				}
				if (_travelTimeProp != null)
				{
					object value10 = _travelTimeProp.GetValue(_cachedLavaRising);
					if (value10 is float)
					{
						num5 = (float)value10;
					}
				}
				else if (_travelTimeField != null)
				{
					object value11 = _travelTimeField.GetValue(_cachedLavaRising);
					if (value11 is float)
					{
						num5 = (float)value11;
					}
				}
				if (flag3 && !flag4 && num5 > 0f)
				{
					float num6 = num5 - num4;
					if (num6 > 0f)
					{
						string text6 = FormatTime(num6);
						string text7 = "Lava Rising" + ": " + text6 + " remaining";
						Color textColor3 = default(Color);
						((Color)(ref textColor3))..ctor(1f, 0.4f, 0.2f);
						DrawTextWithOutline(new Vector2(150f, num), text7, textColor3, 14);
						num += 20f;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("[ESP] Error displaying lava timer: " + ex.Message));
			}
		}

		private static void RenderObjectNames(Vector3 localPlayerPos)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			InitializeStyles();
			Color baseColor = default(Color);
			((Color)(ref baseColor))..ctor(0.8f, 0.6f, 1f, 1f);
			float time = Time.time;
			if (_cachedObjects.Count == 0 || time - _objectCacheTime > 2f)
			{
				UpdateObjectCache();
				_objectCacheTime = time;
			}
			try
			{
				foreach (GameObject cachedObject in _cachedObjects)
				{
					if ((Object)(object)cachedObject == (Object)null || (Object)(object)cachedObject.transform == (Object)null)
					{
						continue;
					}
					float num = Vector3.Distance(localPlayerPos, cachedObject.transform.position);
					if (num > 10f)
					{
						continue;
					}
					Vector3 val = Camera.main.WorldToScreenPoint(cachedObject.transform.position);
					if (val.z <= 0f)
					{
						continue;
					}
					string text = ((Object)cachedObject).name ?? "GameObject";
					text = text.Replace("(Clone)", "").Trim();
					MonoBehaviour[] components = cachedObject.GetComponents<MonoBehaviour>();
					List<string> list = new List<string>();
					MonoBehaviour[] array = components;
					foreach (MonoBehaviour val2 in array)
					{
						if (!((Object)(object)val2 == (Object)null))
						{
							string name = ((object)val2).GetType().Name;
							if (name.Contains("Emitter") || name.Contains("Trigger") || name.Contains("Spawn") || name.Contains("Break") || name.Contains("AOE") || name.Contains("Fungus") || name.Contains("Shroom") || name.Contains("Cluster") || name.Contains("Spore") || name.Contains("Pod") || name.Contains("Puff") || name.Contains("Cloud"))
							{
								list.Add(name);
							}
						}
					}
					string text2 = text;
					if (list.Count > 0)
					{
						text2 = text2 + " [" + string.Join(", ", list) + "]";
					}
					Color distanceColor = GetDistanceColor(num, baseColor);
					DrawTextWithOutline(new Vector2(val.x, (float)Screen.height - val.y), text2, distanceColor, 11, num);
				}
			}
			catch (Exception ex)
			{
				Debug.Log((object)("[ESP] Error in RenderObjectNames: " + ex.Message));
			}
		}

		private static void DrawSimple2DBox(Bounds bounds, Color color, float lineWidth)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			Vector3 center = ((Bounds)(ref bounds)).center;
			Vector3 extents = ((Bounds)(ref bounds)).extents;
			Vector3[] array = (Vector3[])(object)new Vector3[8]
			{
				center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, extents.y, extents.z),
				center + new Vector3(0f - extents.x, extents.y, extents.z)
			};
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < 8; i++)
			{
				Vector3 val = Camera.main.WorldToScreenPoint(array[i]);
				if (val.z > 0f)
				{
					list.Add(array[i]);
					num = Mathf.Min(num, val.x);
					num2 = Mathf.Max(num2, val.x);
					num3 = Mathf.Min(num3, val.y);
					num4 = Mathf.Max(num4, val.y);
				}
			}
			if (list.Count < 4)
			{
				return;
			}
			try
			{
				float[] array2 = new float[list.Count];
				float[] array3 = new float[list.Count];
				float[] array4 = new float[list.Count];
				float[] array5 = new float[list.Count];
				for (int j = 0; j < list.Count; j++)
				{
					Vector3 val2 = Camera.main.WorldToScreenPoint(list[j]);
					array2[j] = Mathf.Abs(val2.x - num);
					array3[j] = Mathf.Abs(val2.x - num2);
					array4[j] = Mathf.Abs(val2.y - num3);
					array5[j] = Mathf.Abs(val2.y - num4);
				}
				int num5 = 0;
				float num6 = array2[0] + array4[0];
				for (int k = 1; k < list.Count; k++)
				{
					float num7 = array2[k] + array4[k];
					if (num7 < num6)
					{
						num6 = num7;
						num5 = k;
					}
				}
				int num8 = 0;
				num6 = array3[0] + array4[0];
				for (int l = 1; l < list.Count; l++)
				{
					float num9 = array3[l] + array4[l];
					if (num9 < num6)
					{
						num6 = num9;
						num8 = l;
					}
				}
				int num10 = 0;
				num6 = array3[0] + array5[0];
				for (int m = 1; m < list.Count; m++)
				{
					float num11 = array3[m] + array5[m];
					if (num11 < num6)
					{
						num6 = num11;
						num10 = m;
					}
				}
				int num12 = 0;
				num6 = array2[0] + array5[0];
				for (int n = 1; n < list.Count; n++)
				{
					float num13 = array2[n] + array5[n];
					if (num13 < num6)
					{
						num6 = num13;
						num12 = n;
					}
				}
				if (num5 < list.Count && num8 < list.Count && num10 < list.Count && num12 < list.Count)
				{
					GL.Color(color);
					GL.Vertex(list[num5]);
					GL.Vertex(list[num8]);
					GL.Vertex(list[num8]);
					GL.Vertex(list[num10]);
					GL.Vertex(list[num10]);
					GL.Vertex(list[num12]);
					GL.Vertex(list[num12]);
					GL.Vertex(list[num5]);
				}
			}
			catch
			{
			}
		}

		private static void Draw3DBox(Bounds bounds, Color color, float lineWidth)
		{
			if (!((Object)(object)Camera.main == (Object)null))
			{
				Vector3 center = ((Bounds)(ref bounds)).center;
				Vector3 extents = ((Bounds)(ref bounds)).extents;
				Vector3[] obj = new Vector3[8]
				{
					center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
					center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
					center + new Vector3(extents.x, extents.y, 0f - extents.z),
					center + new Vector3(0f - extents.x, extents.y, 0f - extents.z),
					center + new Vector3(0f - extents.x, 0f - extents.y, extents.z),
					center + new Vector3(extents.x, 0f - extents.y, extents.z),
					center + new Vector3(extents.x, extents.y, extents.z),
					center + new Vector3(0f - extents.x, extents.y, extents.z)
				};
				GL.Color(color);
				GL.Vertex(obj[0]);
				GL.Vertex(obj[1]);
				GL.Vertex(obj[1]);
				GL.Vertex(obj[2]);
				GL.Vertex(obj[2]);
				GL.Vertex(obj[3]);
				GL.Vertex(obj[3]);
				GL.Vertex(obj[0]);
				GL.Vertex(obj[4]);
				GL.Vertex(obj[5]);
				GL.Vertex(obj[5]);
				GL.Vertex(obj[6]);
				GL.Vertex(obj[6]);
				GL.Vertex(obj[7]);
				GL.Vertex(obj[7]);
				GL.Vertex(obj[4]);
				GL.Vertex(obj[0]);
				GL.Vertex(obj[4]);
				GL.Vertex(obj[1]);
				GL.Vertex(obj[5]);
				GL.Vertex(obj[2]);
				GL.Vertex(obj[6]);
				GL.Vertex(obj[3]);
				GL.Vertex(obj[7]);
			}
		}

		private static void DrawCornerBoxFromBounds(Bounds bounds, Color color, float lineWidth, bool is3D = false)
		{
			if ((Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			Vector3 center = ((Bounds)(ref bounds)).center;
			Vector3 extents = ((Bounds)(ref bounds)).extents;
			Vector3[] array = (Vector3[])(object)new Vector3[8]
			{
				center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, 0f - extents.y, extents.z),
				center + new Vector3(extents.x, extents.y, extents.z),
				center + new Vector3(0f - extents.x, extents.y, extents.z)
			};
			Vector3[] screenCorners = (Vector3[])(object)new Vector3[8];
			bool[] array2 = new bool[8];
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				screenCorners[i] = Camera.main.WorldToScreenPoint(array[i]);
				array2[i] = screenCorners[i].z > 0f;
				if (array2[i])
				{
					num++;
				}
			}
			if (num < 4)
			{
				return;
			}
			GL.Color(color);
			if (is3D)
			{
				if (array2[0] && array2[1])
				{
					GL.Vertex(array[0]);
					GL.Vertex(array[1]);
				}
				if (array2[1] && array2[2])
				{
					GL.Vertex(array[1]);
					GL.Vertex(array[2]);
				}
				if (array2[2] && array2[3])
				{
					GL.Vertex(array[2]);
					GL.Vertex(array[3]);
				}
				if (array2[3] && array2[0])
				{
					GL.Vertex(array[3]);
					GL.Vertex(array[0]);
				}
				if (array2[4] && array2[5])
				{
					GL.Vertex(array[4]);
					GL.Vertex(array[5]);
				}
				if (array2[5] && array2[6])
				{
					GL.Vertex(array[5]);
					GL.Vertex(array[6]);
				}
				if (array2[6] && array2[7])
				{
					GL.Vertex(array[6]);
					GL.Vertex(array[7]);
				}
				if (array2[7] && array2[4])
				{
					GL.Vertex(array[7]);
					GL.Vertex(array[4]);
				}
				if (array2[0] && array2[4])
				{
					GL.Vertex(array[0]);
					GL.Vertex(array[4]);
				}
				if (array2[1] && array2[5])
				{
					GL.Vertex(array[1]);
					GL.Vertex(array[5]);
				}
				if (array2[2] && array2[6])
				{
					GL.Vertex(array[2]);
					GL.Vertex(array[6]);
				}
				if (array2[3] && array2[7])
				{
					GL.Vertex(array[3]);
					GL.Vertex(array[7]);
				}
				return;
			}
			List<int> list = new List<int>();
			for (int j = 0; j < 8; j++)
			{
				if (array2[j])
				{
					list.Add(j);
				}
			}
			list.Sort((int a, int b) => screenCorners[a].z.CompareTo(screenCorners[b].z));
			int[] array3 = new int[4];
			int num2 = 0;
			for (int k = 0; k < Mathf.Min(list.Count, 6); k++)
			{
				if (num2 >= 4)
				{
					break;
				}
				int num3 = list[k];
				if (num2 < 4)
				{
					array3[num2] = num3;
					num2++;
				}
			}
			if (num2 == 4)
			{
				Array.Sort(array3, delegate(int a, int b)
				{
					int num4 = screenCorners[a].y.CompareTo(screenCorners[b].y);
					return (num4 != 0) ? num4 : screenCorners[a].x.CompareTo(screenCorners[b].x);
				});
				GL.Vertex(array[array3[0]]);
				GL.Vertex(array[array3[1]]);
				GL.Vertex(array[array3[1]]);
				GL.Vertex(array[array3[2]]);
				GL.Vertex(array[array3[2]]);
				GL.Vertex(array[array3[3]]);
				GL.Vertex(array[array3[3]]);
				GL.Vertex(array[array3[0]]);
			}
			else if (array2[0] && array2[1] && array2[2] && array2[3])
			{
				GL.Vertex(array[0]);
				GL.Vertex(array[1]);
				GL.Vertex(array[1]);
				GL.Vertex(array[2]);
				GL.Vertex(array[2]);
				GL.Vertex(array[3]);
				GL.Vertex(array[3]);
				GL.Vertex(array[0]);
			}
		}

		private static void DrawSkeleton(Character character, Color color)
		{
			if ((Object)(object)character == (Object)null || character.refs == null || (Object)(object)Camera.main == (Object)null)
			{
				return;
			}
			if ((Object)(object)character.refs.ragdoll != (Object)null && character.refs.ragdoll.partDict != null)
			{
				DrawSkeletonFromRagdoll(character, color);
				return;
			}
			Animator val = character.refs?.animator ?? ((Component)character).GetComponent<Animator>();
			if ((Object)(object)val != (Object)null && val.isHuman)
			{
				DrawSkeletonFromAnimator(val, color);
			}
		}

		private static void DrawSkeletonFromRagdoll(Character character, Color color)
		{
			Dictionary<BodypartType, Bodypart> partDict = character.refs.ragdoll.partDict;
			if (partDict != null)
			{
				Transform val = ((partDict.ContainsKey((BodypartType)4) && (Object)(object)partDict[(BodypartType)4] != (Object)null) ? ((Component)partDict[(BodypartType)4]).transform : null);
				Transform val2 = ((partDict.ContainsKey((BodypartType)3) && (Object)(object)partDict[(BodypartType)3] != (Object)null) ? ((Component)partDict[(BodypartType)3]).transform : null);
				Transform val3 = ((partDict.ContainsKey((BodypartType)2) && (Object)(object)partDict[(BodypartType)2] != (Object)null) ? ((Component)partDict[(BodypartType)2]).transform : null);
				Transform val4 = ((partDict.ContainsKey((BodypartType)0) && (Object)(object)partDict[(BodypartType)0] != (Object)null) ? ((Component)partDict[(BodypartType)0]).transform : null);
				Transform val5 = ((partDict.ContainsKey((BodypartType)25) && (Object)(object)partDict[(BodypartType)25] != (Object)null) ? ((Component)partDict[(BodypartType)25]).transform : null);
				Transform val6 = ((partDict.ContainsKey((BodypartType)5) && (Object)(object)partDict[(BodypartType)5] != (Object)null) ? ((Component)partDict[(BodypartType)5]).transform : null);
				Transform val7 = ((partDict.ContainsKey((BodypartType)6) && (Object)(object)partDict[(BodypartType)6] != (Object)null) ? ((Component)partDict[(BodypartType)6]).transform : null);
				Transform val8 = ((partDict.ContainsKey((BodypartType)7) && (Object)(object)partDict[(BodypartType)7] != (Object)null) ? ((Component)partDict[(BodypartType)7]).transform : null);
				Transform val9 = ((partDict.ContainsKey((BodypartType)26) && (Object)(object)partDict[(BodypartType)26] != (Object)null) ? ((Component)partDict[(BodypartType)26]).transform : null);
				Transform val10 = ((partDict.ContainsKey((BodypartType)8) && (Object)(object)partDict[(BodypartType)8] != (Object)null) ? ((Component)partDict[(BodypartType)8]).transform : null);
				Transform val11 = ((partDict.ContainsKey((BodypartType)9) && (Object)(object)partDict[(BodypartType)9] != (Object)null) ? ((Component)partDict[(BodypartType)9]).transform : null);
				Transform val12 = ((partDict.ContainsKey((BodypartType)10) && (Object)(object)partDict[(BodypartType)10] != (Object)null) ? ((Component)partDict[(BodypartType)10]).transform : null);
				Transform val13 = ((partDict.ContainsKey((BodypartType)11) && (Object)(object)partDict[(BodypartType)11] != (Object)null) ? ((Component)partDict[(BodypartType)11]).transform : null);
				Transform val14 = ((partDict.ContainsKey((BodypartType)12) && (Object)(object)partDict[(BodypartType)12] != (Object)null) ? ((Component)partDict[(BodypartType)12]).transform : null);
				Transform val15 = ((partDict.ContainsKey((BodypartType)13) && (Object)(object)partDict[(BodypartType)13] != (Object)null) ? ((Component)partDict[(BodypartType)13]).transform : null);
				Transform val16 = ((partDict.ContainsKey((BodypartType)14) && (Object)(object)partDict[(BodypartType)14] != (Object)null) ? ((Component)partDict[(BodypartType)14]).transform : null);
				Transform val17 = ((partDict.ContainsKey((BodypartType)15) && (Object)(object)partDict[(BodypartType)15] != (Object)null) ? ((Component)partDict[(BodypartType)15]).transform : null);
				Transform val18 = ((partDict.ContainsKey((BodypartType)16) && (Object)(object)partDict[(BodypartType)16] != (Object)null) ? ((Component)partDict[(BodypartType)16]).transform : null);
				if ((Object)(object)val != (Object)null && (Object)(object)val2 != (Object)null)
				{
					DrawGLLineWorldSpace(val.position, val2.position, color);
				}
				if ((Object)(object)val2 != (Object)null && (Object)(object)val3 != (Object)null)
				{
					DrawGLLineWorldSpace(val2.position, val3.position, color);
				}
				if ((Object)(object)val3 != (Object)null && (Object)(object)val4 != (Object)null)
				{
					DrawGLLineWorldSpace(val3.position, val4.position, color);
				}
				if ((Object)(object)val2 != (Object)null && (Object)(object)val5 != (Object)null)
				{
					DrawGLLineWorldSpace(val2.position, val5.position, color);
				}
				if ((Object)(object)val5 != (Object)null && (Object)(object)val6 != (Object)null)
				{
					DrawGLLineWorldSpace(val5.position, val6.position, color);
				}
				if ((Object)(object)val6 != (Object)null && (Object)(object)val7 != (Object)null)
				{
					DrawGLLineWorldSpace(val6.position, val7.position, color);
				}
				if ((Object)(object)val7 != (Object)null && (Object)(object)val8 != (Object)null)
				{
					DrawGLLineWorldSpace(val7.position, val8.position, color);
				}
				if ((Object)(object)val2 != (Object)null && (Object)(object)val9 != (Object)null)
				{
					DrawGLLineWorldSpace(val2.position, val9.position, color);
				}
				if ((Object)(object)val9 != (Object)null && (Object)(object)val10 != (Object)null)
				{
					DrawGLLineWorldSpace(val9.position, val10.position, color);
				}
				if ((Object)(object)val10 != (Object)null && (Object)(object)val11 != (Object)null)
				{
					DrawGLLineWorldSpace(val10.position, val11.position, color);
				}
				if ((Object)(object)val11 != (Object)null && (Object)(object)val12 != (Object)null)
				{
					DrawGLLineWorldSpace(val11.position, val12.position, color);
				}
				if ((Object)(object)val4 != (Object)null && (Object)(object)val13 != (Object)null)
				{
					DrawGLLineWorldSpace(val4.position, val13.position, color);
				}
				if ((Object)(object)val13 != (Object)null && (Object)(object)val14 != (Object)null)
				{
					DrawGLLineWorldSpace(val13.position, val14.position, color);
				}
				if ((Object)(object)val14 != (Object)null && (Object)(object)val15 != (Object)null)
				{
					DrawGLLineWorldSpace(val14.position, val15.position, color);
				}
				if ((Object)(object)val4 != (Object)null && (Object)(object)val16 != (Object)null)
				{
					DrawGLLineWorldSpace(val4.position, val16.position, color);
				}
				if ((Object)(object)val16 != (Object)null && (Object)(object)val17 != (Object)null)
				{
					DrawGLLineWorldSpace(val16.position, val17.position, color);
				}
				if ((Object)(object)val17 != (Object)null && (Object)(object)val18 != (Object)null)
				{
					DrawGLLineWorldSpace(val17.position, val18.position, color);
				}
			}
		}

		private static void DrawSkeletonFromAnimator(Animator animator, Color color)
		{
			DrawBoneLineGL(animator, (HumanBodyBones)10, (HumanBodyBones)9, color);
			DrawBoneLineGL(animator, (HumanBodyBones)9, (HumanBodyBones)7, color);
			DrawBoneLineGL(animator, (HumanBodyBones)7, (HumanBodyBones)0, color);
			DrawBoneLineGL(animator, (HumanBodyBones)9, (HumanBodyBones)11, color);
			DrawBoneLineGL(animator, (HumanBodyBones)11, (HumanBodyBones)13, color);
			DrawBoneLineGL(animator, (HumanBodyBones)13, (HumanBodyBones)15, color);
			DrawBoneLineGL(animator, (HumanBodyBones)15, (HumanBodyBones)17, color);
			DrawBoneLineGL(animator, (HumanBodyBones)9, (HumanBodyBones)12, color);
			DrawBoneLineGL(animator, (HumanBodyBones)12, (HumanBodyBones)14, color);
			DrawBoneLineGL(animator, (HumanBodyBones)14, (HumanBodyBones)16, color);
			DrawBoneLineGL(animator, (HumanBodyBones)16, (HumanBodyBones)18, color);
			DrawBoneLineGL(animator, (HumanBodyBones)0, (HumanBodyBones)1, color);
			DrawBoneLineGL(animator, (HumanBodyBones)1, (HumanBodyBones)3, color);
			DrawBoneLineGL(animator, (HumanBodyBones)3, (HumanBodyBones)5, color);
			DrawBoneLineGL(animator, (HumanBodyBones)0, (HumanBodyBones)2, color);
			DrawBoneLineGL(animator, (HumanBodyBones)2, (HumanBodyBones)4, color);
			DrawBoneLineGL(animator, (HumanBodyBones)4, (HumanBodyBones)6, color);
		}

		private static void DrawBoneLineGL(Animator animator, HumanBodyBones bone1, HumanBodyBones bone2, Color color)
		{
			if (!((Object)(object)animator == (Object)null))
			{
				Transform boneTransform = animator.GetBoneTransform(bone1);
				Transform boneTransform2 = animator.GetBoneTransform(bone2);
				if ((Object)(object)boneTransform != (Object)null && (Object)(object)boneTransform2 != (Object)null)
				{
					DrawGLLineWorldSpace(boneTransform.position, boneTransform2.position, color);
				}
			}
		}

		private static void DrawGLLineWorldSpace(Vector3 start, Vector3 end, Color color)
		{
			GL.Color(color);
			GL.Vertex(start);
			GL.Vertex(end);
		}

		private static void DrawChamsBoxScreenSpace(Bounds bounds, Color color)
		{
			if ((Object)(object)Camera.main == (Object)null || (Object)(object)_whiteTexture == (Object)null)
			{
				return;
			}
			Vector3 center = ((Bounds)(ref bounds)).center;
			Vector3 extents = ((Bounds)(ref bounds)).extents;
			Vector3[] array = (Vector3[])(object)new Vector3[4]
			{
				center + new Vector3(0f - extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, 0f - extents.y, 0f - extents.z),
				center + new Vector3(extents.x, extents.y, 0f - extents.z),
				center + new Vector3(0f - extents.x, extents.y, 0f - extents.z)
			};
			Vector2?[] array2 = new Vector2?[4];
			bool flag = true;
			for (int i = 0; i < 4; i++)
			{
				Vector3 val = Camera.main.WorldToScreenPoint(array[i]);
				if (val.z > 0f)
				{
					array2[i] = new Vector2(val.x, (float)Screen.height - val.y);
					continue;
				}
				flag = false;
				break;
			}
			if (flag)
			{
				GUI.color = color;
				if (array2[0].HasValue && array2[1].HasValue && array2[2].HasValue && array2[3].HasValue)
				{
					float num = Mathf.Min(new float[4]
					{
						array2[0].Value.x,
						array2[1].Value.x,
						array2[2].Value.x,
						array2[3].Value.x
					});
					float num2 = Mathf.Max(new float[4]
					{
						array2[0].Value.x,
						array2[1].Value.x,
						array2[2].Value.x,
						array2[3].Value.x
					});
					float num3 = Mathf.Min(new float[4]
					{
						array2[0].Value.y,
						array2[1].Value.y,
						array2[2].Value.y,
						array2[3].Value.y
					});
					float num4 = Mathf.Max(new float[4]
					{
						array2[0].Value.y,
						array2[1].Value.y,
						array2[2].Value.y,
						array2[3].Value.y
					});
					GUI.DrawTexture(new Rect(num, num3, num2 - num, num4 - num3), (Texture)(object)_whiteTexture);
					Color color2 = color;
					color2.a = 1f;
					DrawLineScreenSpace(array2[0].Value, array2[1].Value, color2, 2f);
					DrawLineScreenSpace(array2[1].Value, array2[2].Value, color2, 2f);
					DrawLineScreenSpace(array2[2].Value, array2[3].Value, color2, 2f);
					DrawLineScreenSpace(array2[3].Value, array2[0].Value, color2, 2f);
				}
				GUI.color = Color.white;
			}
		}

		private static void DrawLineScreenSpace(Vector2 start, Vector2 end, Color color, float width)
		{
			if (!((Object)(object)_whiteTexture == (Object)null))
			{
				Vector2 val = end - start;
				Vector2 normalized = ((Vector2)(ref val)).normalized;
				float num = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
				float num2 = Vector2.Distance(start, end);
				Matrix4x4 matrix = GUI.matrix;
				GUI.color = color;
				GUIUtility.RotateAroundPivot(num, start);
				GUI.DrawTexture(new Rect(start.x, start.y - width / 2f, num2, width), (Texture)(object)_whiteTexture);
				GUI.matrix = matrix;
				GUI.color = Color.white;
			}
		}

		private static void DrawTextWithOutline(Vector2 position, string text, Color textColor, int fontSize, float distance = 0f)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			Color textColor2 = textColor;
			int num = 1;
			if (distance > 50f)
			{
				float num2 = Mathf.Clamp01((distance - 50f) / 100f);
				textColor2 = Color.Lerp(textColor, Color.white, num2 * 0.7f);
				textColor2.a = Mathf.Lerp(textColor.a, 1f, num2 * 0.5f);
				num = ((!(distance > 100f)) ? 1 : 2);
			}
			_textStyle.fontSize = fontSize;
			_textStyle.normal.textColor = textColor2;
			_outlineStyle.fontSize = fontSize;
			_outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 0.9f);
			Vector2 val = _textStyle.CalcSize(new GUIContent(text));
			Vector2 val2 = default(Vector2);
			((Vector2)(ref val2))..ctor(position.x - val.x / 2f, position.y);
			for (int i = -num; i <= num; i++)
			{
				for (int j = -num; j <= num; j++)
				{
					if (i != 0 || j != 0)
					{
						GUI.Label(new Rect(val2.x + (float)i, val2.y + (float)j, val.x, val.y), text, _outlineStyle);
					}
				}
			}
			GUI.Label(new Rect(val2.x, val2.y, val.x, val.y), text, _textStyle);
		}

		private static Bounds GetCharacterBounds(Character character)
		{
			Bounds result;
			if ((Object)(object)character == (Object)null)
			{
				result = default(Bounds);
				return result;
			}
			Vector3 position = ((Component)character).transform.position;
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(position, Vector3.zero);
			bool flag = false;
			Vector3 size;
			if (character.refs != null && (Object)(object)character.refs.ragdoll != (Object)null && character.refs.ragdoll.partDict != null)
			{
				foreach (KeyValuePair<BodypartType, Bodypart> item in character.refs.ragdoll.partDict)
				{
					if (!((Object)(object)item.Value != (Object)null) || !((Object)(object)((Component)item.Value).transform != (Object)null))
					{
						continue;
					}
					Collider component = ((Component)item.Value).GetComponent<Collider>();
					if ((Object)(object)component != (Object)null)
					{
						result = component.bounds;
						size = ((Bounds)(ref result)).size;
						if (((Vector3)(ref size)).magnitude > 0.01f)
						{
							if (!flag)
							{
								result2 = component.bounds;
								flag = true;
							}
							else
							{
								((Bounds)(ref result2)).Encapsulate(component.bounds);
							}
							continue;
						}
					}
					Vector3 position2 = ((Component)item.Value).transform.position;
					if (!flag)
					{
						result2 = new Bounds(position2, Vector3.zero);
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(position2);
					}
				}
			}
			if (!flag)
			{
				Renderer[] componentsInChildren = ((Component)character).GetComponentsInChildren<Renderer>(true);
				foreach (Renderer val in componentsInChildren)
				{
					if (!((Object)(object)val != (Object)null))
					{
						continue;
					}
					result = val.bounds;
					size = ((Bounds)(ref result)).size;
					if (((Vector3)(ref size)).magnitude > 0.01f)
					{
						if (!flag)
						{
							result2 = val.bounds;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val.bounds);
						}
					}
				}
			}
			Collider[] componentsInChildren2 = ((Component)character).GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if (!((Object)(object)val2 != (Object)null))
				{
					continue;
				}
				result = val2.bounds;
				size = ((Bounds)(ref result)).size;
				if (((Vector3)(ref size)).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag && character.refs != null && (Object)(object)character.refs.head != (Object)null && (Object)(object)character.refs.hip != (Object)null)
			{
				Vector3 position3 = ((Component)character.refs.head).transform.position;
				Vector3 position4 = ((Component)character.refs.hip).transform.position;
				float num = Vector3.Distance(position3, position4) * 1.5f;
				Vector3 val3 = (position3 + position4) / 2f;
				((Bounds)(ref result2))..ctor(val3, new Vector3(0.5f, num, 0.5f));
				flag = true;
			}
			if (!flag)
			{
				Animator val4 = character.refs?.animator ?? ((Component)character).GetComponent<Animator>();
				if ((Object)(object)val4 != (Object)null && val4.isHuman)
				{
					Transform boneTransform = val4.GetBoneTransform((HumanBodyBones)10);
					Transform boneTransform2 = val4.GetBoneTransform((HumanBodyBones)5);
					if ((Object)(object)boneTransform != (Object)null && (Object)(object)boneTransform2 != (Object)null)
					{
						float num2 = Vector3.Distance(boneTransform.position, boneTransform2.position);
						((Bounds)(ref result2))..ctor(position + Vector3.up * (num2 / 2f), new Vector3(0.5f, num2, 0.5f));
						flag = true;
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(position + Vector3.up * 0.875f, new Vector3(0.5f, 1.75f, 0.5f));
			}
			size = ((Bounds)(ref result2)).size;
			if (((Vector3)(ref size)).magnitude < 0.1f)
			{
				((Bounds)(ref result2))..ctor(((Bounds)(ref result2)).center, new Vector3(0.5f, 1.75f, 0.5f));
			}
			return result2;
		}

		private static Bounds GetItemBounds(Item item)
		{
			Bounds result;
			if ((Object)(object)item == (Object)null || (Object)(object)((Component)item).gameObject == (Object)null)
			{
				result = default(Bounds);
				return result;
			}
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(((Component)item).transform.position, Vector3.zero);
			bool flag = false;
			Renderer[] componentsInChildren = ((Component)item).GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (!((Object)(object)val != (Object)null))
				{
					continue;
				}
				result = val.bounds;
				Vector3 size = ((Bounds)(ref result)).size;
				if (((Vector3)(ref size)).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val.bounds);
					}
				}
			}
			Collider[] componentsInChildren2 = ((Component)item).GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if ((Object)(object)val2 != (Object)null)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag)
			{
				MeshFilter[] componentsInChildren3 = ((Component)item).GetComponentsInChildren<MeshFilter>(true);
				Bounds val6 = default(Bounds);
				foreach (MeshFilter val3 in componentsInChildren3)
				{
					if ((Object)(object)val3 != (Object)null && (Object)(object)val3.sharedMesh != (Object)null)
					{
						Bounds bounds = val3.sharedMesh.bounds;
						Vector3 val4 = ((Component)val3).transform.TransformPoint(((Bounds)(ref bounds)).center);
						Vector3 val5 = ((Component)val3).transform.TransformVector(((Bounds)(ref bounds)).size);
						((Bounds)(ref val6))..ctor(val4, val5);
						if (!flag)
						{
							result2 = val6;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val6);
						}
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(((Component)item).transform.position, Vector3.one * 0.5f);
			}
			return result2;
		}

		private static Bounds GetObjectBounds(GameObject obj)
		{
			Bounds result;
			if ((Object)(object)obj == (Object)null)
			{
				result = default(Bounds);
				return result;
			}
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))..ctor(obj.transform.position, Vector3.zero);
			bool flag = false;
			Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer val in componentsInChildren)
			{
				if (!((Object)(object)val != (Object)null))
				{
					continue;
				}
				result = val.bounds;
				Vector3 size = ((Bounds)(ref result)).size;
				if (((Vector3)(ref size)).magnitude > 0.01f)
				{
					if (!flag)
					{
						result2 = val.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val.bounds);
					}
				}
			}
			Collider[] componentsInChildren2 = obj.GetComponentsInChildren<Collider>(true);
			foreach (Collider val2 in componentsInChildren2)
			{
				if ((Object)(object)val2 != (Object)null)
				{
					if (!flag)
					{
						result2 = val2.bounds;
						flag = true;
					}
					else
					{
						((Bounds)(ref result2)).Encapsulate(val2.bounds);
					}
				}
			}
			if (!flag)
			{
				MeshFilter[] componentsInChildren3 = obj.GetComponentsInChildren<MeshFilter>(true);
				Bounds val6 = default(Bounds);
				foreach (MeshFilter val3 in componentsInChildren3)
				{
					if ((Object)(object)val3 != (Object)null && (Object)(object)val3.sharedMesh != (Object)null)
					{
						Bounds bounds = val3.sharedMesh.bounds;
						Vector3 val4 = ((Component)val3).transform.TransformPoint(((Bounds)(ref bounds)).center);
						Vector3 val5 = ((Component)val3).transform.TransformVector(((Bounds)(ref bounds)).size);
						((Bounds)(ref val6))..ctor(val4, val5);
						if (!flag)
						{
							result2 = val6;
							flag = true;
						}
						else
						{
							((Bounds)(ref result2)).Encapsulate(val6);
						}
					}
				}
			}
			if (!flag)
			{
				((Bounds)(ref result2))..ctor(obj.transform.position, Vector3.one * 0.5f);
			}
			return result2;
		}

		private static Vector3 GetEntityHeadPosition(GameObject entity, Character charComponent)
		{
			if ((Object)(object)charComponent != (Object)null && charComponent.refs != null && (Object)(object)charComponent.refs.head != (Object)null)
			{
				return ((Component)charComponent.refs.head).transform.position + Vector3.up * 0.3f;
			}
			if ((Object)(object)charComponent != (Object)null)
			{
				Animator component = ((Component)charComponent).GetComponent<Animator>();
				if ((Object)(object)component != (Object)null)
				{
					Transform boneTransform = component.GetBoneTransform((HumanBodyBones)10);
					if ((Object)(object)boneTransform != (Object)null)
					{
						return boneTransform.position + Vector3.up * 0.3f;
					}
				}
			}
			return entity.transform.position + Vector3.up * 1.8f;
		}

		private static string GetEntityType(GameObject entity)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return "Unknown";
			}
			Character component = entity.GetComponent<Character>();
			MonoBehaviour[] components;
			if ((Object)(object)component != (Object)null)
			{
				try
				{
					FieldInfo field = typeof(Character).GetField("isZombie", BindingFlags.Instance | BindingFlags.Public);
					FieldInfo field2 = typeof(Character).GetField("isScoutmaster", BindingFlags.Instance | BindingFlags.Public);
					if (field != null && (bool)field.GetValue(component))
					{
						return "Zombie";
					}
					if (field2 != null && (bool)field2.GetValue(component))
					{
						return "Scoutmaster";
					}
				}
				catch
				{
				}
				components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if ((Object)(object)val == (Object)null)
					{
						continue;
					}
					string name = ((object)val).GetType().Name;
					switch (name)
					{
					case "Scoutmaster":
						return "Scoutmaster";
					case "MushroomZombie":
						return "Zombie";
					case "Beetle":
						return "Beetle";
					}
					if (name.Contains("Scoutmaster"))
					{
						return "Scoutmaster";
					}
					if (name.Contains("Zombie"))
					{
						return "Zombie";
					}
					if (name.Contains("Beetle"))
					{
						return "Beetle";
					}
				}
				if ((Object)(object)((Component)component).gameObject.GetComponent<Player>() != (Object)null && (Object)(object)((MonoBehaviourPun)component).photonView != (Object)null && ((MonoBehaviourPun)component).photonView.Owner != null)
				{
					if (!((MonoBehaviourPun)component).photonView.IsMine)
					{
						return "Player";
					}
					return "Player (You)";
				}
				try
				{
					FieldInfo field3 = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
					if (field3 != null && (bool)field3.GetValue(component))
					{
						return "Bot";
					}
				}
				catch
				{
				}
			}
			components = entity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val2 in components)
			{
				if ((Object)(object)val2 == (Object)null)
				{
					continue;
				}
				string name2 = ((object)val2).GetType().Name;
				switch (name2)
				{
				case "Scoutmaster":
					return "Scoutmaster";
				case "MushroomZombie":
					return "Zombie";
				case "Beetle":
					return "Beetle";
				}
				if (name2.Contains("Scoutmaster"))
				{
					return "Scoutmaster";
				}
				if (name2.Contains("Zombie"))
				{
					return "Zombie";
				}
				if (name2.Contains("Beetle"))
				{
					return "Beetle";
				}
				if (name2.Contains("Enemy"))
				{
					return "Enemy";
				}
				if (name2.Contains("NPC"))
				{
					return "NPC";
				}
			}
			string name3 = ((Object)entity).name;
			if (name3.Contains("Scoutmaster") || name3.Contains("ScoutMaster") || name3.Contains("Scout Master"))
			{
				return "Scoutmaster";
			}
			if (name3.Contains("Zombie"))
			{
				return "Zombie";
			}
			if (name3.Contains("Beetle"))
			{
				return "Beetle";
			}
			if (name3.Contains("Enemy"))
			{
				return "Enemy";
			}
			return "Entity";
		}

		private static string GetTargetName(Character target)
		{
			if ((Object)(object)target == (Object)null)
			{
				return "Unknown";
			}
			try
			{
				if ((Object)(object)((MonoBehaviourPun)target).photonView != (Object)null && ((MonoBehaviourPun)target).photonView.Owner != null)
				{
					string text = ((MonoBehaviourPun)target).photonView.Owner.NickName ?? "Unknown";
					if (text.Length > 20)
					{
						text = text.Substring(0, 20) + "...";
					}
					return text;
				}
				if ((Object)(object)target == (Object)(object)Character.localCharacter)
				{
					return "You";
				}
				FieldInfo field = typeof(Character).GetField("isBot", BindingFlags.Instance | BindingFlags.Public);
				if (field != null && (bool)field.GetValue(target))
				{
					return "Bot";
				}
				return "Unknown";
			}
			catch
			{
				return "Unknown";
			}
		}

		private static Color GetEntityColor(string entityType)
		{
			return (Color)(entityType.ToLower() switch
			{
				"zombie" => ZombieColor, 
				"scoutmaster" => ScoutmasterColor, 
				"beetle" => BeetleColor, 
				"enemy" => new Color(1f, 0.2f, 0.2f), 
				"bot" => new Color(0.5f, 0.5f, 1f), 
				_ => new Color(1f, 0.7f, 0.3f), 
			});
		}

		private static string GetItemName(Item item)
		{
			if ((Object)(object)item == (Object)null)
			{
				return "Unknown";
			}
			try
			{
				FieldInfo field = typeof(Item).GetField("itemName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					object value = field.GetValue(item);
					if (value != null)
					{
						return value.ToString();
					}
				}
				FieldInfo field2 = typeof(Item).GetField("displayName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field2 != null)
				{
					object value2 = field2.GetValue(item);
					if (value2 != null)
					{
						return value2.ToString();
					}
				}
			}
			catch
			{
			}
			string text = ((Object)item).name;
			if (text.Contains("(Clone)"))
			{
				text = text.Replace("(Clone)", "");
			}
			return text;
		}

		private static Color GetItemColor(string itemName)
		{
			string text = itemName.ToLower();
			if (text.Contains("fruit") || text.Contains("apple") || text.Contains("berry") || text.Contains("banana") || text.Contains("orange"))
			{
				return new Color(1f, 0.5f, 0.5f);
			}
			if (text.Contains("gun") || text.Contains("weapon") || text.Contains("rifle") || text.Contains("pistol") || text.Contains("knife"))
			{
				return new Color(0.8f, 0.2f, 0.2f);
			}
			if (text.Contains("med") || text.Contains("health") || text.Contains("bandage") || text.Contains("heal"))
			{
				return new Color(0.2f, 1f, 0.2f);
			}
			if (text.Contains("ammo") || text.Contains("bullet") || text.Contains("magazine"))
			{
				return new Color(1f, 1f, 0.2f);
			}
			return ItemColor;
		}

		private static string GetLuggageName(GameObject luggage)
		{
			if ((Object)(object)luggage == (Object)null)
			{
				return "Luggage";
			}
			try
			{
				if (_luggageComponentType != null)
				{
					Component component = luggage.GetComponent(_luggageComponentType);
					MonoBehaviour val = (MonoBehaviour)(object)((component is MonoBehaviour) ? component : null);
					if ((Object)(object)val != (Object)null)
					{
						FieldInfo field = _luggageComponentType.GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null)
						{
							object value = field.GetValue(val);
							if (value != null)
							{
								return value.ToString();
							}
						}
					}
				}
			}
			catch
			{
			}
			string text = ((Object)luggage).name;
			if (text.Contains("(Clone)"))
			{
				text = text.Replace("(Clone)", "");
			}
			return text;
		}

		private static float GetCharacterHealth(Character character)
		{
			if ((Object)(object)character == (Object)null || (Object)(object)character.data == (Object)null)
			{
				return 0f;
			}
			try
			{
				FieldInfo field = typeof(CharacterData).GetField("health", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null && field.GetValue(character.data) is float result)
				{
					return result;
				}
				FieldInfo field2 = typeof(CharacterData).GetField("maxHealth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field2 != null && field2.GetValue(character.data) is float num && num > 0f)
				{
					PropertyInfo property = typeof(CharacterData).GetProperty("Health", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property != null)
					{
						object value = property.GetValue(character.data);
						if (value is float)
						{
							return (float)value / num * 100f;
						}
					}
				}
			}
			catch
			{
			}
			return 100f;
		}

		private static Color GetDistanceColor(float distance, Color baseColor)
		{
			if (distance < 20f)
			{
				return baseColor;
			}
			if (distance < 50f)
			{
				return Color.Lerp(baseColor, baseColor * 0.7f, (distance - 20f) / 30f);
			}
			if (distance < 100f)
			{
				return Color.Lerp(baseColor * 0.7f, baseColor * 0.5f, (distance - 50f) / 50f);
			}
			return baseColor * 0.5f;
		}

		private static string GetEntityAIState(GameObject entity, Character charComponent)
		{
			if ((Object)(object)entity == (Object)null)
			{
				return "";
			}
			try
			{
				MonoBehaviour[] components = entity.GetComponents<MonoBehaviour>();
				foreach (MonoBehaviour val in components)
				{
					if ((Object)(object)val == (Object)null)
					{
						continue;
					}
					string name = ((object)val).GetType().Name;
					if (name == "MushroomZombie")
					{
						FieldInfo field = ((object)val).GetType().GetField("_currentState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field == null)
						{
							field = ((object)val).GetType().GetField("currentState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						}
						if (field != null)
						{
							object value = field.GetValue(val);
							if (value != null)
							{
								string text = value.ToString();
								FieldInfo field2 = ((object)val).GetType().GetField("_currentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								if (field2 == null)
								{
									PropertyInfo property = ((object)val).GetType().GetProperty("currentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
									if (property != null)
									{
										object value2 = property.GetValue(val);
										Character val2 = (Character)((value2 is Character) ? value2 : null);
										if ((Object)(object)val2 != (Object)null)
										{
											string targetName = GetTargetName(val2);
											return text + "  " + targetName;
										}
									}
								}
								else
								{
									object value3 = field2.GetValue(val);
									Character val3 = (Character)((value3 is Character) ? value3 : null);
									if ((Object)(object)val3 != (Object)null)
									{
										string targetName2 = GetTargetName(val3);
										return text + "  " + targetName2;
									}
								}
								return text;
							}
						}
					}
					switch (name)
					{
					case "Scoutmaster":
					{
						PropertyInfo property2 = ((object)val).GetType().GetProperty("currentTarget", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (property2 != null)
						{
							object value4 = property2.GetValue(val);
							Character val4 = (Character)((value4 is Character) ? value4 : null);
							if ((Object)(object)val4 != (Object)null)
							{
								string targetName3 = GetTargetName(val4);
								FieldInfo field3 = ((object)val).GetType().GetField("isThrowing", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								if (field3 != null && (bool)field3.GetValue(val))
								{
									return "Throwing  " + targetName3;
								}
								return "Hunting  " + targetName3;
							}
						}
						return "Patrolling";
					}
					case "Beetle":
					case "Mob":
					{
						FieldInfo field4 = ((object)val).GetType().GetField("_mobState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field4 != null)
						{
							object value5 = field4.GetValue(val);
							if (value5 != null)
							{
								string text2 = value5.ToString();
								FieldInfo field5 = ((object)val).GetType().GetField("_targetChar", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
								if (field5 != null)
								{
									object value6 = field5.GetValue(val);
									Character val5 = (Character)((value6 is Character) ? value6 : null);
									if ((Object)(object)val5 != (Object)null)
									{
										string targetName4 = GetTargetName(val5);
										return text2 + "  " + targetName4;
									}
								}
								return text2;
							}
						}
						if ((Object)(object)charComponent != (Object)null && (Object)(object)charComponent.input != (Object)null)
						{
							if (((Vector2)(ref charComponent.input.movementInput)).magnitude > 0.1f)
							{
								return "Moving";
							}
							return "Idle";
						}
						return "Active";
					}
					}
					if (!name.Contains("AI") && !name.Contains("Enemy") && !name.Contains("Behavior") && !name.Contains("StateMachine"))
					{
						continue;
					}
					PropertyInfo property3 = ((object)val).GetType().GetProperty("State", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property3 != null)
					{
						object value7 = property3.GetValue(val);
						if (value7 != null)
						{
							return value7.ToString();
						}
					}
					FieldInfo field6 = ((object)val).GetType().GetField("currentState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (field6 != null)
					{
						object value8 = field6.GetValue(val);
						if (value8 != null)
						{
							return value8.ToString();
						}
					}
				}
				if ((Object)(object)charComponent != (Object)null && (Object)(object)charComponent.input != (Object)null)
				{
					if (((Vector2)(ref charComponent.input.movementInput)).magnitude > 0.1f)
					{
						return "Moving";
					}
					if (charComponent.input.jumpIsPressed)
					{
						return "Jumping";
					}
					return "Idle";
				}
			}
			catch
			{
			}
			return "";
		}

		private static string FormatTime(float seconds)
		{
			if (seconds < 60f)
			{
				return $"{seconds:F0}s";
			}
			int num = Mathf.FloorToInt(seconds / 60f);
			int num2 = Mathf.FloorToInt(seconds % 60f);
			return $"{num}:{num2:D2}";
		}

		private static EnvironmentalEventInfo GetEnvironmentalEvent(GameObject obj)
		{
			if ((Object)(object)obj == (Object)null)
			{
				return null;
			}
			MonoBehaviour[] components = obj.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if ((Object)(object)val == (Object)null)
				{
					continue;
				}
				Type type = ((object)val).GetType();
				string text = type.Name.ToLower();
				if (text.Contains("wind") || text.Contains("root"))
				{
					float nextWindTime = GetNextWindTime(val);
					if (nextWindTime >= 0f)
					{
						return new EnvironmentalEventInfo
						{
							EventType = "Wind",
							NextEventTime = nextWindTime,
							Color = new Color(0.5f, 0.8f, 1f)
						};
					}
				}
				if (text.Contains("cold") || text.Contains("snow") || text.Contains("blizzard") || text.Contains("freeze"))
				{
					float nextColdStormTime = GetNextColdStormTime(val);
					if (nextColdStormTime >= 0f)
					{
						return new EnvironmentalEventInfo
						{
							EventType = "Cold Storm",
							NextEventTime = nextColdStormTime,
							Color = new Color(0.7f, 0.9f, 1f)
						};
					}
				}
				try
				{
					FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					foreach (FieldInfo fieldInfo in fields)
					{
						if (!(fieldInfo.FieldType == typeof(float)))
						{
							continue;
						}
						string text2 = fieldInfo.Name.ToLower();
						if (text2.Contains("timer") || text2.Contains("next") || text2.Contains("event"))
						{
							float num = (float)fieldInfo.GetValue(val);
							if (num > Time.time)
							{
								return new EnvironmentalEventInfo
								{
									EventType = type.Name,
									NextEventTime = num,
									Color = new Color(0.8f, 0.8f, 0.8f)
								};
							}
						}
					}
				}
				catch
				{
				}
			}
			return null;
		}

		private static float GetNextWindTime(MonoBehaviour comp)
		{
			if ((Object)(object)comp == (Object)null)
			{
				return -1f;
			}
			try
			{
				Type type = ((object)comp).GetType();
				FieldInfo field = type.GetField("nextWindTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field2 = type.GetField("windTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null && field.GetValue(comp) is float result)
				{
					return result;
				}
				if (field2 != null)
				{
					object value = field2.GetValue(comp);
					if (value is float)
					{
						return Time.time + (float)value;
					}
				}
			}
			catch
			{
			}
			return -1f;
		}

		private static float GetNextColdStormTime(MonoBehaviour comp)
		{
			if ((Object)(object)comp == (Object)null)
			{
				return -1f;
			}
			try
			{
				Type type = ((object)comp).GetType();
				FieldInfo field = type.GetField("nextColdStormTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field2 = type.GetField("coldTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field3 = type.GetField("stormTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null && field.GetValue(comp) is float result)
				{
					return result;
				}
				if (field2 != null)
				{
					object value = field2.GetValue(comp);
					if (value is float)
					{
						return Time.time + (float)value;
					}
				}
				if (field3 != null)
				{
					object value2 = field3.GetValue(comp);
					if (value2 is float)
					{
						return Time.time + (float)value2;
					}
				}
			}
			catch
			{
			}
			return -1f;
		}

		public static void DrawWatermark()
		{
			if (!CheatConfig.ShowWatermark)
			{
				return;
			}
			try
			{
				if (WATERMARK_TEXT != WATERMARK_CHECK || WATERMARK_TEXT.GetHashCode() != WATERMARK_HASH)
				{
					Application.Quit();
					Environment.Exit(1);
					return;
				}
			}
			catch
			{
				Application.Quit();
				Environment.Exit(1);
				return;
			}
			InitializeStyles();
			_textStyle.fontSize = 14;
			_textStyle.normal.textColor = new Color(1f, 1f, 1f, 0.7f);
			_textStyle.alignment = (TextAnchor)2;
			Vector2 val = _textStyle.CalcSize(new GUIContent(WATERMARK_TEXT));
			float num = 10f;
			GUI.Label(new Rect((float)Screen.width - val.x - 10f, num, val.x, val.y), WATERMARK_TEXT, _textStyle);
			UpdateFPS();
			string text = $"{_currentFPS} FPS";
			Vector2 val2 = _textStyle.CalcSize(new GUIContent(text));
			num += val.y + 5f;
			GUI.Label(new Rect((float)Screen.width - val2.x - 10f, num, val2.x, val2.y), text, _textStyle);
		}

		private static void UpdateFPS()
		{
			_fpsTimeLeft -= Time.deltaTime;
			_fpsAccumulator += Time.timeScale / Time.deltaTime;
			_fpsFrames++;
			if (_fpsTimeLeft <= 0f)
			{
				_currentFPS = Mathf.RoundToInt(_fpsAccumulator / (float)_fpsFrames);
				_fpsAccumulator = 0f;
				_fpsFrames = 0;
				_fpsTimeLeft = _fpsUpdateInterval;
			}
		}

		public static void DrawEntityControlKeybinds()
		{
			if ((Object)(object)CheatConfig.CurrentlyControlledEntity == (Object)null)
			{
				return;
			}
			GameObject currentlyControlledEntity = CheatConfig.CurrentlyControlledEntity;
			if ((Object)(object)currentlyControlledEntity == (Object)null)
			{
				return;
			}
			Character component = currentlyControlledEntity.GetComponent<Character>();
			if ((Object)(object)component == (Object)null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			MonoBehaviour[] components = currentlyControlledEntity.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour val in components)
			{
				if ((Object)(object)val != (Object)null)
				{
					string name = ((object)val).GetType().Name;
					if (name == "MushroomZombie")
					{
						flag = true;
					}
					else if (name == "Scoutmaster")
					{
						flag2 = true;
					}
				}
			}
			if (!flag && !flag2)
			{
				return;
			}
			InitializeStyles();
			_textStyle.alignment = (TextAnchor)6;
			_textStyle.fontSize = 12;
			_textStyle.normal.textColor = Color.yellow;
			_textStyle.wordWrap = false;
			float num = Screen.height - 80;
			float num2 = 10f;
			float num3 = 20f;
			string text = (flag ? "Zombie" : "Scoutmaster");
			GUI.Label(new Rect(num2, num, 300f, num3), "Controlling: " + text, _textStyle);
			num += num3;
			if (flag)
			{
				GUI.Label(new Rect(num2, num, 300f, num3), "LMB / E - Lunge Attack", _textStyle);
				num += num3;
			}
			if (!flag2)
			{
				return;
			}
			bool flag3 = false;
			try
			{
				FieldInfo field = typeof(CharacterData).GetField("grabbedPlayer", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					object value = field.GetValue(component.data);
					flag3 = (Object)((value is Character) ? value : null) != (Object)null;
				}
			}
			catch
			{
			}
			if (flag3)
			{
				GUI.Label(new Rect(num2, num, 300f, num3), "RMB / F - Throw Player", _textStyle);
			}
			else
			{
				GUI.Label(new Rect(num2, num, 300f, num3), "RMB / F - Grab Player (nearby)", _textStyle);
			}
		}
	}
