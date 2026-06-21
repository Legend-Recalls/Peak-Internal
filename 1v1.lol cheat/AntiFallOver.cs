public static class AntiFallOver
{
	public static void Apply()
	{
		if (!CheatConfig.AntiFallOver || (Object)(object)Character.localCharacter == (Object)null || !((MonoBehaviourPun)Character.localCharacter).photonView.IsMine)
		{
			return;
		}
		try
		{
			if ((Object)(object)Character.localCharacter.data != (Object)null)
			{
				if (Character.localCharacter.data.fallSeconds > 0f)
				{
					Character.localCharacter.data.fallSeconds = 0f;
				}
				if ((Object)(object)Character.localCharacter.refs?.view != (Object)null)
				{
					Character.localCharacter.refs.view.RPC("RPCA_UnFall", (RpcTarget)0, Array.Empty<object>());
				}
			}
		}
		catch
		{
		}
	}
}
