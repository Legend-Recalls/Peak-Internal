using System;
using System.Collections;
using UnityEngine;

namespace _1v1.lol_cheat
{
	public static class TrollFeatures
	{
		public static void Initialize(MonoBehaviour runner)
		{
			TrollHelpers.Initialize(runner);
		}

		public static void MakePlayerFall(Character target, float seconds = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerFall(target, seconds, includeSelf);
		}

		public static void MakePlayerWalkOffCliff(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerWalkOffCliff(target, includeSelf);
		}

		public static void UnFallPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.UnFallPlayer(target, includeSelf);
		}

		public static void KillPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.KillPlayer(target, includeSelf);
		}

		public static void RevivePlayer(Character target, bool applyStatus = false, bool includeSelf = false)
		{
			PlayerManipulation.RevivePlayer(target, applyStatus, includeSelf);
		}

		public static void RevivePlayerAtPosition(Character target, Vector3 position, bool applyStatus = false, bool includeSelf = false)
		{
			PlayerManipulation.RevivePlayerAtPosition(target, position, applyStatus, includeSelf);
		}

		public static void StickPlayerBodyPart(Character target, BodypartType bodypartType, Vector3 position, STATUSTYPE statusType = 0, float statusAmount = 0f, bool includeSelf = false)
		{
			PlayerManipulation.StickPlayerBodyPart(target, bodypartType, position, statusType, statusAmount, includeSelf);
		}

		public static void UnstickPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.UnstickPlayer(target, includeSelf);
		}

		public static void StartCarryPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			PlayerManipulation.StartCarryPlayer(carrier, target, includeSelf);
		}

		public static void DropCarriedPlayer(Character carrier, Character target, bool includeSelf = false)
		{
			PlayerManipulation.DropCarriedPlayer(carrier, target, includeSelf);
		}

		public static void ZombifyPlayer(Character target, bool includeSelf = false)
		{
			PlayerManipulation.ZombifyPlayer(target, includeSelf);
		}

		public static void MakePlayerPassOut(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerPassOut(target, includeSelf);
		}

		public static void LaunchPlayer(Character target, Vector3 direction, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayer(target, direction, force, includeSelf);
		}

		public static void LaunchPlayerUp(Character target, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayerUp(target, force, includeSelf);
		}

		public static void LaunchPlayerTowardsMe(Character target, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayerTowardsMe(target, force, includeSelf);
		}

		public static void LaunchPlayerAway(Character target, float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchPlayerAway(target, force, includeSelf);
		}

		public static void MakePlayerSpin(Character target, float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerSpin(target, spinSpeed, duration, includeSelf);
		}

		public static void TeleportPlayerRandomly(Character target, float minDistance = 30f, float maxDistance = 100f, bool includeSelf = false)
		{
			PlayerManipulation.TeleportPlayerRandomly(target, minDistance, maxDistance, includeSelf);
		}

		public static void TeleportPlayerToMe(Character target, bool includeSelf = false)
		{
			PlayerManipulation.TeleportPlayerToMe(target, includeSelf);
		}

		public static void TeleportPlayerToPosition(Character target, Vector3 position, bool includeSelf = false)
		{
			PlayerManipulation.TeleportPlayerToPosition(target, position, includeSelf);
		}

		public static void MakePlayerRagdoll(Character target, float duration = 5f)
		{
			PlayerManipulation.MakePlayerRagdoll(target, duration);
		}

		public static void GrabPlayer(Character target)
		{
			PlayerManipulation.GrabPlayer(target);
		}

		public static void ThrowPlayer(Character target, Vector3 direction, float force = 30f)
		{
			PlayerManipulation.ThrowPlayer(target, direction, force);
		}

		public static void ApplyAntigrav(Character target, float duration = 10f, bool includeSelf = false)
		{
			PlayerManipulation.ApplyAntigrav(target, duration, includeSelf);
		}

		public static void MakePlayerGlow(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerGlow(target, includeSelf);
		}

		public static void MakePlayerJumpRepeatedly(Character target, float interval = 0.5f, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerJumpRepeatedly(target, interval, duration, includeSelf);
		}

		public static void FreezePlayer(Character target, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.FreezePlayer(target, duration, includeSelf);
		}

		public static void MakeEveryoneJump(bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryoneJump(includeSelf);
		}

		public static void MakeEveryoneFall(float seconds = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryoneFall(seconds, includeSelf);
		}

		public static void MakeEveryonePassOut(bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryonePassOut(includeSelf);
		}

		public static void LaunchEveryoneUp(float force = 50f, bool includeSelf = false)
		{
			PlayerManipulation.LaunchEveryoneUp(force, includeSelf);
		}

		public static void TeleportEveryoneToMe(bool includeSelf = false)
		{
			PlayerManipulation.TeleportEveryoneToMe(includeSelf);
		}

		public static void MakeEveryoneSpin(float spinSpeed = 720f, float duration = 5f, bool includeSelf = false)
		{
			PlayerManipulation.MakeEveryoneSpin(spinSpeed, duration, includeSelf);
		}

		public static void MakePlayerJumpOffCliff(Character target, bool includeSelf = false)
		{
			PlayerManipulation.MakePlayerJumpOffCliff(target, includeSelf);
		}

		public static GameObject SpawnScoutmasterAndTarget(Character targetPlayer, Vector3? spawnPosition = null, bool includeSelf = false)
		{
			return _1v1.lol_cheat.Troll.EntityControl.SpawnScoutmasterAndTarget(targetPlayer, spawnPosition, includeSelf);
		}

		public static void ForceScoutmasterTarget(GameObject scoutmaster, Character targetPlayer)
		{
			_1v1.lol_cheat.Troll.EntityControl.ForceScoutmasterTarget(scoutmaster, targetPlayer);
		}

		public static void ForceZombieTarget(GameObject zombie, Character targetPlayer)
		{
			_1v1.lol_cheat.Troll.EntityControl.ForceZombieTarget(zombie, targetPlayer);
		}

		public static void StopForceScoutmasterTarget(GameObject scoutmaster)
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceScoutmasterTarget(scoutmaster);
		}

		public static void StopForceZombieTarget(GameObject zombie)
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceZombieTarget(zombie);
		}

		public static void StopForceAllScoutmasters()
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceAllScoutmasters();
		}

		public static void StopForceAllZombies()
		{
			_1v1.lol_cheat.Troll.EntityControl.StopForceAllZombies();
		}

		public static void StartRemoteControl(Character targetPlayer, bool includeSelf = false)
		{
			_1v1.lol_cheat.Troll.EntityControl.StartRemoteControl(targetPlayer, includeSelf);
		}

		public static void StopRemoteControl()
		{
			_1v1.lol_cheat.Troll.EntityControl.StopRemoteControl();
		}

		public static void OpenAllLuggage()
		{
			ItemManipulation.OpenAllLuggage();
		}

		public static void TeleportAllItems(Vector3 position)
		{
			ItemManipulation.TeleportAllItems(position);
		}

		public static void TeleportAllItemsToPlayer(Character target)
		{
			ItemManipulation.TeleportAllItemsToPlayer(target);
		}

		public static void TeleportAllItemsInFrontOfMe()
		{
			ItemManipulation.TeleportAllItemsInFrontOfMe();
		}

		public static void ForceDropAllItems(Character target, bool includeSelf = false)
		{
			ItemManipulation.ForceDropAllItems(target, includeSelf);
		}

		public static void StopTrollEffects(Character target)
		{
			TrollHelpers.StopTrollEffects(target);
		}

		public static void StopAllTrollEffects()
		{
			TrollHelpers.StopAllTrollEffects();
		}

		public static void StopAllCrashCoroutines()
		{
			TrollHelpers.StopAllCrashCoroutines();
		}

		public static void CrashPlayer_TriggerRelayBounds(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_TriggerRelayBounds(target, includeSelf);
		}

		public static void CrashPlayer_TornadoNullRefs(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_TornadoNullRefs(target, includeSelf);
		}

		public static void CrashPlayer_RescueHookNull(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_RescueHookNull(target, includeSelf);
		}

		public static void CrashPlayer_SpiderNull(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_SpiderNull(target, includeSelf);
		}

		public static void CrashPlayer_CharacterGrabbingNull(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_CharacterGrabbingNull(target, includeSelf);
		}

		public static void CrashPlayer_GetFedItemInvalid(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_GetFedItemInvalid(target, includeSelf);
		}

		public static void CrashPlayer_DirectPlayerRPCs(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_DirectPlayerRPCs(target, includeSelf);
		}

		public static void CrashPlayer_AllNullRefs(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_AllNullRefs(target, includeSelf);
		}

		public static void CrashPlayer_Inventory(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Inventory(target, includeSelf);
		}

		public static void CrashPlayer_StatusArrayBounds(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_StatusArrayBounds(target, includeSelf);
		}

		public static void CrashPlayer_Deserialization(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Deserialization(target, includeSelf);
		}

		public static void CrashPlayer_NullReferences(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_NullReferences(target, includeSelf);
		}

		public static void CrashPlayer_DivisionByZero(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_DivisionByZero(target, includeSelf);
		}

		public static void CrashPlayer_ObjectSpam(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_ObjectSpam(target, includeSelf);
		}

		public static void CrashPlayer_MaxArraySize(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_MaxArraySize(target, includeSelf);
		}

		public static void CrashPlayer_Statuses(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Statuses(target, includeSelf);
		}

		public static void CrashPlayer_Afflictions(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Afflictions(target, includeSelf);
		}

		public static void CrashPlayer_Thorns(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Thorns(target, includeSelf);
		}

		public static void CrashPlayer_ReconnectData(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_ReconnectData(target, includeSelf);
		}

		public static void CrashPlayer_InvalidPhotonView(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_InvalidPhotonView(target, includeSelf);
		}

		public static void CrashPlayer_ExtremeValues(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_ExtremeValues(target, includeSelf);
		}

		public static void CrashPlayer_AllMethods(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_AllMethods(target, includeSelf);
		}

		public static void CrashPlayer_Ultimate(Character target, bool includeSelf = false)
		{
			CrashMethods.CrashPlayer_Ultimate(target, includeSelf);
		}

		public static void CrashAllPlayersAndDisconnect()
		{
			CrashMethods.CrashAllPlayersAndDisconnect();
		}

		public static void CrashAllPlayersStealth()
		{
			CrashMethods.CrashAllPlayersStealth();
		}

		public static void KickPlayer(Character target, bool includeSelf = false)
		{
			CrashMethods.KickPlayer(target, includeSelf);
		}

		public static void ForceHost()
		{
			CrashMethods.ForceHost();
		}
	}
}
