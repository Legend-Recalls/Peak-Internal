public static class ClimbingPrediction
{
	private static bool enabled = true;

	private static List<Vector3> predictionLine = new List<Vector3>();

	private static Material lineMaterial;

	public static bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			enabled = value;
		}
	}

	public static void SetEnabled(bool value)
	{
		enabled = value;
	}

	public static void Initialize()
	{
		lineMaterial = new Material(Shader.Find("Sprites/Default"));
		lineMaterial.color = new Color(0f, 1f, 0f, 0.8f);
	}

	public static void Update()
	{
		enabled = CheatConfig.ClimbingPredictionEnabled;
		if (!enabled || (Object)(object)Character.localCharacter == (Object)null)
		{
			return;
		}
		Character localCharacter = Character.localCharacter;
		if (!localCharacter.data.isClimbing && (Object)(object)localCharacter.data.currentClimbHandle == (Object)null)
		{
			if ((Object)(object)Camera.main != (Object)null)
			{
				RaycastHit val = default(RaycastHit);
				if (Physics.Raycast(new Ray(((Component)Camera.main).transform.position, ((Component)Camera.main).transform.forward), ref val, 5f))
				{
					if ((Object)(object)((Component)((RaycastHit)(ref val)).collider).GetComponent<ClimbHandle>() != (Object)null || Vector3.Angle(((RaycastHit)(ref val)).normal, Vector3.up) > 45f)
					{
						CalculatePredictionLine(localCharacter, ((RaycastHit)(ref val)).point, ((RaycastHit)(ref val)).normal);
					}
					else
					{
						predictionLine.Clear();
					}
				}
				else
				{
					predictionLine.Clear();
				}
			}
			else
			{
				predictionLine.Clear();
			}
		}
		else if (localCharacter.data.isClimbing)
		{
			CalculatePredictionLine(localCharacter, localCharacter.data.climbPos, localCharacter.data.climbNormal);
		}
		else
		{
			predictionLine.Clear();
		}
	}

	private static void CalculatePredictionLine(Character character, Vector3 startPos, Vector3 surfaceNormal)
	{
		predictionLine.Clear();
		if ((Object)(object)character == (Object)null || (Object)(object)character.refs?.climbing == (Object)null)
		{
			return;
		}
		try
		{
			Type type = ((object)character.refs.climbing).GetType();
			FieldInfo field = type.GetField("climbSpeed", BindingFlags.Instance | BindingFlags.Public);
			FieldInfo field2 = type.GetField("climbSpeedMod", BindingFlags.Instance | BindingFlags.Public);
			FieldInfo field3 = type.GetField("maxStaminaUsage", BindingFlags.Instance | BindingFlags.Public);
			FieldInfo field4 = type.GetField("minStaminaUsage", BindingFlags.Instance | BindingFlags.Public);
			FieldInfo field5 = type.GetField("climbingStamMinimumMultiplier", BindingFlags.Instance | BindingFlags.Public);
			float num = ((field != null) ? ((float)field.GetValue(character.refs.climbing)) : 3f);
			if (field2 != null)
			{
				_ = (float)field2.GetValue(character.refs.climbing);
			}
			float num2 = ((field3 != null) ? ((float)field3.GetValue(character.refs.climbing)) : 0.2f);
			float num3 = ((field4 != null) ? ((float)field4.GetValue(character.refs.climbing)) : 0.02f);
			float num4 = ((field5 != null) ? ((float)field5.GetValue(character.refs.climbing)) : 1f);
			float num5 = character.data.currentStamina;
			Vector2 val = (((Object)(object)character.input != (Object)null) ? character.input.movementInput : Vector2.zero);
			if (((Vector2)(ref val)).magnitude < 0.1f)
			{
				val = Vector2.up;
			}
			Vector3 val2 = startPos;
			Vector3 val3 = surfaceNormal;
			ClimbModifierSurface val4 = null;
			RaycastHit val5 = default(RaycastHit);
			if (Physics.Raycast(startPos + val3 * 0.5f, -val3, ref val5, 2f))
			{
				val4 = ((Component)((RaycastHit)(ref val5)).collider).GetComponent<ClimbModifierSurface>();
			}
			Vector3 val6 = Vector3.ProjectOnPlane(Vector3.up, val3);
			Vector3 normalized = ((Vector3)(ref val6)).normalized;
			val6 = Vector3.Cross(normalized, val3);
			Vector3 normalized2 = ((Vector3)(ref val6)).normalized;
			val6 = normalized * val.y + normalized2 * val.x;
			Vector3 val7 = ((Vector3)(ref val6)).normalized;
			if (((Vector3)(ref val7)).magnitude < 0.1f)
			{
				val7 = normalized;
			}
			float num6 = 0.2f;
			float num7 = num6 / num;
			int num8 = 500;
			int num9 = 0;
			predictionLine.Add(val2);
			Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetTypes()).FirstOrDefault((Type t) => t.Name == "HelperFunctions");
			MethodInfo lineCheckMethod = null;
			if (type2 != null)
			{
				lineCheckMethod = type2.GetMethod("LineCheck", BindingFlags.Static | BindingFlags.Public, null, new Type[3]
				{
					typeof(Vector3),
					typeof(Vector3),
					typeof(object)
				}, null);
			}
			while (num5 > 0f && num9 < num8)
			{
				Vector3 samplePos = val2 + val7 * num6;
				RaycastHit val8 = SampleWallAtPosition(samplePos, val3, type2, lineCheckMethod);
				if ((Object)(object)((RaycastHit)(ref val8)).transform == (Object)null)
				{
					break;
				}
				float num10 = Vector3.Angle(((RaycastHit)(ref val8)).normal, Vector3.up);
				ClimbModifierSurface component = ((Component)((RaycastHit)(ref val8)).collider).GetComponent<ClimbModifierSurface>();
				if ((Object)(object)component != (Object)null)
				{
					MethodInfo method = ((object)component).GetType().GetMethod("OverrideClimbAngle", BindingFlags.Instance | BindingFlags.NonPublic);
					if (method != null)
					{
						object obj = method.Invoke(component, new object[2] { character, num10 });
						if (obj != null)
						{
							num10 = (float)obj;
						}
					}
				}
				float num11 = num10 - 90f;
				bool flag = true;
				if (num11 > 0f)
				{
					if (Mathf.Abs(num11) > 80f)
					{
						flag = false;
					}
				}
				else if (Mathf.Abs(num11) > 40f)
				{
					flag = false;
				}
				if (!flag)
				{
					break;
				}
				samplePos = ((RaycastHit)(ref val8)).point;
				val3 = ((RaycastHit)(ref val8)).normal;
				val4 = component;
				float num12 = num2 * Mathf.Clamp(((Vector2)(ref val)).magnitude, 0f, 1f);
				float num13 = num3 * num4;
				num12 = Mathf.Clamp(num12, num13, num2);
				float num14 = 1f;
				if (!((Object)(object)val4 != (Object)null) || !val4.staticClimbCost)
				{
					float num15 = Mathf.InverseLerp(40f, 60f, num10);
					num14 = Mathf.Lerp(0.2f, 1f, num15);
				}
				num12 *= num14;
				if ((Object)(object)val4 != (Object)null)
				{
					FieldInfo field6 = ((object)val4).GetType().GetField("staminaUsageMultiplier", BindingFlags.Instance | BindingFlags.Public);
					if (field6 != null)
					{
						float num16 = (float)field6.GetValue(val4);
						num12 *= num16;
					}
				}
				num12 *= character.data.staminaMod;
				float num17 = num12 * num7;
				if (!(num5 < num17))
				{
					num5 -= num17;
					val6 = Vector3.ProjectOnPlane(Vector3.up, val3);
					normalized = ((Vector3)(ref val6)).normalized;
					val6 = Vector3.Cross(normalized, val3);
					normalized2 = ((Vector3)(ref val6)).normalized;
					val6 = normalized * val.y + normalized2 * val.x;
					val7 = ((Vector3)(ref val6)).normalized;
					if (((Vector3)(ref val7)).magnitude < 0.1f)
					{
						val7 = normalized;
					}
					predictionLine.Add(samplePos);
					val2 = samplePos;
					num9++;
					continue;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning((object)("[ClimbingPrediction] Error calculating prediction: " + ex.Message));
			predictionLine.Clear();
		}
	}

	private static RaycastHit SampleWallAtPosition(Vector3 samplePos, Vector3 currentNormal, Type helperFunctionsType, MethodInfo lineCheckMethod)
	{
		RaycastHit result = default(RaycastHit);
		Vector3 val = samplePos + currentNormal * 0.4f;
		Vector3 val2 = samplePos + currentNormal * 0.5f;
		Vector3 val3 = samplePos + currentNormal * -1f;
		if (lineCheckMethod != null && helperFunctionsType != null)
		{
			try
			{
				Type nestedType = helperFunctionsType.GetNestedType("LayerType");
				if (nestedType != null)
				{
					object obj = Enum.GetValues(nestedType).Cast<object>().FirstOrDefault((object v) => v.ToString() == "TerrainMap");
					if (obj != null)
					{
						object obj2 = lineCheckMethod.Invoke(null, new object[3] { val, val2, obj });
						if (obj2 != null)
						{
							result = (RaycastHit)obj2;
							if ((Object)(object)((RaycastHit)(ref result)).transform != (Object)null)
							{
								return result;
							}
						}
						obj2 = lineCheckMethod.Invoke(null, new object[3] { val, val3, obj });
						if (obj2 != null)
						{
							result = (RaycastHit)obj2;
							if ((Object)(object)((RaycastHit)(ref result)).transform != (Object)null)
							{
								return result;
							}
						}
					}
				}
			}
			catch
			{
			}
		}
		if ((Object)(object)((RaycastHit)(ref result)).transform == (Object)null)
		{
			Vector3 val4 = val2 - val;
			if (Physics.Raycast(val, ((Vector3)(ref val4)).normalized, ref result, 2f))
			{
				return result;
			}
			val4 = val3 - val;
			Physics.Raycast(val, ((Vector3)(ref val4)).normalized, ref result, 2f);
			return result;
		}
		return result;
	}

	public static void Draw()
	{
		if (enabled && predictionLine.Count >= 2 && !((Object)(object)Camera.main == (Object)null))
		{
			if ((Object)(object)lineMaterial == (Object)null)
			{
				Initialize();
			}
			CheatConfig.ESPMaterial.SetPass(0);
			GL.PushMatrix();
			GL.LoadProjectionMatrix(Camera.main.projectionMatrix);
			GL.modelview = Camera.main.worldToCameraMatrix;
			GL.Begin(1);
			GL.Color(new Color(0f, 1f, 0f, 0.8f));
			for (int i = 0; i < predictionLine.Count - 1; i++)
			{
				GL.Vertex(predictionLine[i]);
				GL.Vertex(predictionLine[i + 1]);
			}
			GL.End();
			GL.PopMatrix();
		}
	}
}
