public static class AutoPathfinder
{
	private static bool enabled = false;

	private static bool followingPath = false;

	private static Vector3 targetPosition = Vector3.zero;

	private static float lastTargetCheck = 0f;

	private static float targetCheckInterval = 1f;

	private static List<Vector3> pathLine = new List<Vector3>();

	public static bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			bool flag = enabled;
			enabled = value;
			if (!enabled)
			{
				StopFollowing();
			}
			else if (!flag)
			{
				StartPathfinding();
			}
		}
	}

	public static bool FollowingPath => followingPath;

	public static int PathNodeCount => pathLine.Count;

	public static void Initialize(MonoBehaviour runner)
	{
	}

	private static void StartPathfinding()
	{
		if (!(Character.localCharacter == null))
		{
			StopFollowing();
			Vector3 val = FindCampfire();
			if (val == Vector3.zero)
			{
				Debug.Log("[AutoPathfinder] No campfire found - disabling feature");
				enabled = false;
				CheatConfig.AutoPathfinderEnabled = false;
			}
			else
			{
				targetPosition = val;
				followingPath = true;
				Debug.Log($"[AutoPathfinder] Started pathfinding to campfire at {targetPosition}");
			}
		}
	}

	private static void StopFollowing()
	{
		followingPath = false;
		targetPosition = Vector3.zero;
		pathLine.Clear();
	}

	public static void Update()
	{
		if (!enabled || Character.localCharacter == null)
		{
			if (followingPath)
			{
				StopFollowing();
			}
			return;
		}
		if (Time.time - lastTargetCheck > targetCheckInterval)
		{
			lastTargetCheck = Time.time;
			if (targetPosition == Vector3.zero || Vector3.Distance(Character.localCharacter.Center, targetPosition) < 3f)
			{
				Vector3 val = FindCampfire();
				if (val == Vector3.zero)
				{
					Debug.Log("[AutoPathfinder] Lost campfire - disabling feature");
					enabled = false;
					CheatConfig.AutoPathfinderEnabled = false;
					StopFollowing();
					return;
				}
				targetPosition = val;
			}
		}
		if (followingPath && targetPosition != Vector3.zero)
		{
			WalkTowardsTarget();
			UpdatePathLine();
		}
	}

	private static void WalkTowardsTarget()
	{
		if (Character.localCharacter == null || Character.localCharacter.input == null)
		{
			return;
		}
		Vector3 center = Character.localCharacter.Center;
		Vector3 val = targetPosition;
		float flatDistance = GetFlatDistance(center, val);
		float num = Vector3.Distance(center, val);
		LookAtTarget(val);
		float num2 = 1f;
		if (num < 5f)
		{
			if (flatDistance < 2.5f)
			{
				num2 = 0f;
			}
			else if (flatDistance < 1.5f)
			{
				num2 = -1f;
			}
		}
		Character.localCharacter.input.movementInput = new Vector2(0f, num2);
		if (Character.localCharacter.refs?.climbing != null)
		{
			TryClimb(Character.localCharacter.refs.climbing);
		}
		RaycastHit val2 = HelperFunctions.LineCheck(center, center + Vector3.down * 3f, (LayerType)1, 0f, (QueryTriggerInteraction)1);
		if (val2.transform == null)
		{
			Character.localCharacter.input.jumpWasPressed = true;
		}
		if (num > 10f)
		{
			Character.localCharacter.input.sprintIsPressed = true;
			Character.localCharacter.data.isSprinting = true;
		}
		else
		{
			Character.localCharacter.input.sprintIsPressed = false;
			Character.localCharacter.data.isSprinting = false;
		}
	}

	private static void LookAtTarget(Vector3 targetPos)
	{
		if (Character.localCharacter == null)
		{
			return;
		}
		Vector3 val = targetPos - Character.localCharacter.Head;
		try
		{
			Type type = Type.GetType("HelperFunctions");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("DirectionToLook", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null)
				{
					object obj = method.Invoke(null, new object[1] { val });
					if (obj is Vector3 val2)
					{
						Character.localCharacter.data.lookValues = new Vector2(val2.x, val2.y);
					}
					else if (obj is Vector2 lookValues)
					{
						Character.localCharacter.data.lookValues = lookValues;
					}
					return;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(("[AutoPathfinder] Error calling DirectionToLook: " + ex.Message));
		}
		Vector3 normalized = val.normalized;
		float num = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
		float num2 = (0f - Mathf.Asin(normalized.y)) * 57.29578f;
		Character.localCharacter.data.lookValues = new Vector2(num, num2);
	}

	private static float GetFlatDistance(Vector3 from, Vector3 to)
	{
		try
		{
			Type type = Type.GetType("HelperFunctions");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("FlatDistance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null)
				{
					return (float)method.Invoke(null, new object[2] { from, to });
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(("[AutoPathfinder] Error calling FlatDistance: " + ex.Message));
		}
		Vector3 val = new Vector3(from.x, 0f, from.z);
		Vector3 val2 = new Vector3(to.x, 0f, to.z);
		return Vector3.Distance(val, val2);
	}

	private static void TryClimb(CharacterClimbing climbing)
	{
		if (climbing == null)
		{
			return;
		}
		try
		{
			MethodInfo method = typeof(CharacterClimbing).GetMethod("TryClimb", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method != null)
			{
				method.Invoke(climbing, null);
				return;
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(("[AutoPathfinder] Error calling TryClimb: " + ex.Message));
		}
		try
		{
			MethodInfo[] methods = typeof(CharacterClimbing).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.Name.Contains("Climb") && methodInfo.GetParameters().Length == 0)
				{
					methodInfo.Invoke(climbing, null);
					break;
				}
			}
		}
		catch
		{
		}
	}

	private static void UpdatePathLine()
	{
		if (Character.localCharacter == null || targetPosition == Vector3.zero)
		{
			return;
		}
		pathLine.Clear();
		Vector3 center = Character.localCharacter.Center;
		Vector3 val = targetPosition;
		int num = Mathf.Max(5, Mathf.RoundToInt(Vector3.Distance(center, val) / 5f));
		num = Mathf.Min(num, 50);
		RaycastHit val3 = default(RaycastHit);
		for (int i = 0; i <= num; i++)
		{
			float num2 = (float)i / (float)num;
			Vector3 val2 = Vector3.Lerp(center, val, num2);
			if (Physics.Raycast(val2 + Vector3.up * 10f, Vector3.down, ref val3, 20f))
			{
				val2.y = val3.point.y + 1f;
			}
			pathLine.Add(val2);
		}
	}

	private static Vector3 FindCampfire()
	{
		if (Character.localCharacter == null)
		{
			return Vector3.zero;
		}
		Vector3 center = Character.localCharacter.Center;
		Vector3 val = Vector3.zero;
		float num = float.MaxValue;
		try
		{
			Type type = Type.GetType("Campfire");
			if (type != null)
			{
				Object[] array = Object.FindObjectsByType(type, (FindObjectsSortMode)0);
				foreach (Object obj in array)
				{
					MonoBehaviour val2 = obj as MonoBehaviour;
					if (val2 != null)
					{
						float num2 = Vector3.Distance(center, val2.transform.position);
						if (num2 < num)
						{
							num = num2;
							val = val2.transform.position;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning(("[AutoPathfinder] Error finding campfires via component: " + ex.Message));
		}
		if (val == Vector3.zero)
		{
			GameObject[] array2 = Object.FindObjectsByType<GameObject>((FindObjectsSortMode)0);
			foreach (GameObject val3 in array2)
			{
				if (val3 == null)
				{
					continue;
				}
				string text = val3.name.ToLower();
				if (text.Contains("campfire") || text.Contains("fire") || text.Contains("checkpoint"))
				{
					float num3 = Vector3.Distance(center, val3.transform.position);
					if (num3 < num)
					{
						num = num3;
						val = val3.transform.position;
					}
				}
			}
		}
		return val;
	}

	public static void DrawPath()
	{
		if (enabled && pathLine.Count >= 2 && !(Camera.main == null))
		{
			CheatConfig.ESPMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
			GL.modelview = Camera.main.worldToCameraMatrix;
			GL.Begin(1);
			GL.Color(new Color(0f, 1f, 1f, 0.8f));
			for (int i = 0; i < pathLine.Count - 1; i++)
			{
				GL.Vertex(pathLine[i]);
				GL.Vertex(pathLine[i + 1]);
			}
			GL.End();
			GL.PopMatrix();
		}
	}
}
