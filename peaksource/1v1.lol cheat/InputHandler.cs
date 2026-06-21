public static class InputHandler
{
	private const int VK_DELETE = 46;

	private const int VK_INSERT = 45;

	private const int VK_LEFT_MOUSE = 1;

	private const int VK_MIDDLE_MOUSE = 4;

	[DllImport("user32.dll")]
	private static extern short GetAsyncKeyState(int vKey);

	public static void CheckMenuToggle(ref bool toggled, ref float lastToggleTime, ref float lastUninjectTime, float toggleDelay, int toggleKey)
	{
		if (GetAsyncKeyState(toggleKey) < 0 && Time.time - lastToggleTime >= toggleDelay)
		{
			toggled = !toggled;
			lastToggleTime = Time.time;
		}
		if (GetAsyncKeyState(46) < 0 && Time.time - lastUninjectTime >= toggleDelay)
		{
			Cheat.Uninject();
			lastUninjectTime = Time.time;
		}
	}

	public static bool IsLeftMousePressed()
	{
		return (GetAsyncKeyState(1) & 0x8000) != 0;
	}

	public static bool IsMiddleMousePressed()
	{
		return (GetAsyncKeyState(4) & 0x8001) != 0;
	}

	public static void HandleCursor(bool menuOpen, ref bool lastMenuState, ref bool wasCursorVisibleBeforeMenu)
	{
		if (menuOpen)
		{
			Cursor.visible = true;
			Cursor.lockState = (CursorLockMode)0;
			if (!lastMenuState)
			{
				wasCursorVisibleBeforeMenu = Cursor.visible;
			}
		}
		else
		{
			Scene activeScene = SceneManager.GetActiveScene();
			string name = ((Scene)(ref activeScene)).name;
			if (name.Contains("Menu") || name.Contains("Main") || name.Contains("Lobby") || name == "MainMenu" || !name.Contains("Game"))
			{
				Cursor.visible = true;
				Cursor.lockState = (CursorLockMode)0;
			}
			else
			{
				Cursor.visible = false;
				Cursor.lockState = (CursorLockMode)1;
			}
		}
		lastMenuState = menuOpen;
	}
}
