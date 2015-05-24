﻿using UnityEngine;
using System.Collections;

namespace GameStates {
	public class WaitingForNextTurnState : AbstractGameObjectState {
		private PoolGameController gameController;
		private GameObject cue;
		private GameObject cueBall;
		private GameObject redBalls;
		private GameObject mainCamera;

		private Vector3 cameraOffset;
		private Vector3 cueOffset;
		private Quaternion cameraRotation;
		private Quaternion cueRotation;

		public WaitingForNextTurnState(MonoBehaviour parent) : base(parent) {
			gameController = (PoolGameController)parent;

			cue = gameController.cue;
			cueBall = gameController.cueBall;
			redBalls = gameController.redBalls;
			mainCamera = gameController.mainCamera;
			
			cameraOffset = cueBall.transform.position - mainCamera.transform.position;
			cameraRotation = mainCamera.transform.rotation;
			cueOffset = cueBall.transform.position - cue.transform.position;
			cueRotation = cue.transform.rotation;
		}

		public override void FixedUpdate() {
			if (!cueBall.GetComponent<Rigidbody>().IsSleeping())
				return;

			foreach (var rigidbody in redBalls.GetComponentsInChildren<Rigidbody>()) {
				if (!rigidbody.IsSleeping())
					return;
			}

			// If all balls are sleeping, time for the next turn
			// This is kinda hacky but gets the job done
			gameController.currentState = new WaitingForStrikeState(gameController);
		}

		public override void LateUpdate() {
			mainCamera.transform.position = cueBall.transform.position - cameraOffset;
			mainCamera.transform.rotation = cameraRotation;
			
			cue.transform.position = cueBall.transform.position - cueOffset;
			cue.transform.rotation = cueRotation;
		}
	}
}