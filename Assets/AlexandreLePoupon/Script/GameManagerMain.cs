using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

	public class GameManagerMain : MonoBehaviour {

		public Text fireAttackText;
		public Text fireDefenseText;
		public Text frostAttackText;
		public Text frostDefenseText;

		public int fireDefense = 0;
		public int fireAttack = 0;
		public int frostDefense = 0;
		public int frostAttack = 0;

		void Update () {
			//SBO: oui
			fireAttackText.text = fireAttack.ToString()+" %";
			fireDefenseText.text = fireDefense.ToString()+" %";
			frostAttackText.text = frostAttack.ToString()+" %";
			frostDefenseText.text = frostDefense.ToString()+" %";
		}
}
