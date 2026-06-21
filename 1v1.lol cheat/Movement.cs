using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class Movement
	{
		private static bool _isFlying = false;

		private static Vector3 _flyVelocity = Vector3.zero;

		private static float _acceleration = 200f;

		public static void ApplySpeedHack()
		{
			if ((Object)(object)Character.localCharacter?.refs?.movement == (Object)null)
			{
				return;
			}
			if (CheatConfig.BaseMovementModifier <= 0.1f)
			{
				CheatConfig.BaseMovementModifier = Character.localCharacter.refs.movement.movementModifier;
				if (CheatConfig.BaseMovementModifier <= 0.1f)
				{
					CheatConfig.BaseMovementModifier = 1f;
				}
			}
			if (CheatConfig.Speed)
			{
				Character.localCharacter.refs.movement.movementModifier = CheatConfig.BaseMovementModifier * CheatConfig.SpeedMultiply;
			}
			else
			{
				Character.localCharacter.refs.movement.movementModifier = CheatConfig.BaseMovementModifier;
			}
		}

		public static void ApplySuperJump()
		{
			if ((Object)(object)Character.localCharacter?.refs?.movement == (Object)null)
			{
				return;
			}
			if (CheatConfig.BaseJumpImpulse < 0f)
			{
				CheatConfig.BaseJumpImpulse = Character.localCharacter.refs.movement.jumpImpulse;
				if (CheatConfig.BaseJumpImpulse <= 0f)
				{
					CheatConfig.BaseJumpImpulse = 10f;
				}
			}
			if (CheatConfig.SuperJump)
			{
				Character.localCharacter.refs.movement.jumpImpulse = CheatConfig.BaseJumpImpulse * CheatConfig.JumpMultiplier;
			}
			else
			{
				Character.localCharacter.refs.movement.jumpImpulse = CheatConfig.BaseJumpImpulse;
			}
		}

		public static void ApplyClimbingSpeed()
		{
			if ((Object)(object)Character.localCharacter?.refs?.climbing == (Object)null)
			{
				return;
			}
			if (CheatConfig.BaseClimbingSpeed <= 0.1f)
			{
				CheatConfig.BaseClimbingSpeed = Character.localCharacter.refs.climbing.climbSpeed;
				if (CheatConfig.BaseClimbingSpeed <= 0.1f)
				{
					CheatConfig.BaseClimbingSpeed = 1f;
				}
			}
			Character.localCharacter.refs.climbing.climbSpeed = CheatConfig.BaseClimbingSpeed * CheatConfig.ClimbingSpeedMultiplier;
		}

		public static void ApplyFlyMode()
		{
			if ((Object)(object)Character.localCharacter == (Object)null || (Object)(object)Character.localCharacter.refs?.ragdoll == (Object)null)
			{
				return;
			}
			if (_isFlying != CheatConfig.FlyMode)
			{
				_isFlying = CheatConfig.FlyMode;
				foreach (Bodypart part in Character.localCharacter.refs.ragdoll.partList)
				{
					if ((Object)(object)((part != null) ? part.Rig : null) != (Object)null)
					{
						part.Rig.useGravity = !_isFlying;
					}
				}
			}
			if (_isFlying)
			{
				HandleFlying();
			}
		}

		private static void HandleFlying()
		{
			if (!_isFlying || (Object)(object)Character.localCharacter == (Object)null || (Object)(object)Character.localCharacter.refs?.ragdoll == (Object)null)
			{
				return;
			}
			Character localCharacter = Character.localCharacter;
			localCharacter.data.isGrounded = true;
			localCharacter.data.sinceGrounded = 0f;
			localCharacter.data.sinceJump = 0f;
			Vector3 val = Vector2.op_Implicit(localCharacter.input.movementInput);
			Vector3 normalized = ((Vector3)(ref localCharacter.data.lookDirection_Flat)).normalized;
			Vector3 val2 = Vector3.Cross(Vector3.up, normalized);
			Vector3 normalized2 = ((Vector3)(ref val2)).normalized;
			Vector3 val3 = normalized * val.y + normalized2 * val.x;
			if (localCharacter.input.jumpIsPressed)
			{
				val3 += Vector3.up;
			}
			if (localCharacter.input.crouchIsPressed)
			{
				val3 += Vector3.down;
			}
			_flyVelocity = Vector3.Lerp(_flyVelocity, ((Vector3)(ref val3)).normalized * CheatConfig.FlySpeed, Time.deltaTime * _acceleration);
			foreach (Bodypart part in localCharacter.refs.ragdoll.partList)
			{
				if ((Object)(object)((part != null) ? part.Rig : null) != (Object)null)
				{
					part.Rig.useGravity = false;
					part.Rig.linearVelocity = _flyVelocity;
				}
			}
		}

		public static void Disable()
		{
			CheatConfig.Speed = false;
			CheatConfig.FlyMode = false;
			CheatConfig.SuperJump = false;
		}
	}
}
