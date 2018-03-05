using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace networked {

	public class OnLinkBreak : MonoBehaviour {

		private void OnJointBreak2D (Joint2D brokenJoint) {
			EventManager<Joint2D>.TriggerEvent ("OnJointBreak2D", brokenJoint);
		}
	}
}