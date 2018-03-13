using System;
using UnityEngine;

namespace pathfinding {

	/**
		Cette grille est plaquée sur la grille par défaut de unity avec une échelle différente.
		Une fonction de transformation est proposée pour changer de référentiel.
		Les tilemap possèdent déjà ces fonctions.
	 */
	public class MyGrid : MonoBehaviour {

		public float cellWidth = 0.5f;
		public float cellHeight = 0.5f;

		public Vector2 CellToWorld (Vector2Int gridPos) {
			return new Vector2 (gridPos.x * cellWidth + cellWidth / 2, gridPos.y * cellHeight + cellHeight / 2);
		}

		public Vector2Int WorldToCell (Vector2 worldPos) {
			return new Vector2Int ((int) Mathf.Floor (worldPos.x / cellWidth), (int) Mathf.Floor (worldPos.y / cellHeight));
		}

		private void OnDrawGizmos () {
			Vector3 worldPos = UnityEditor.HandleUtility.GUIPointToWorldRay (Event.current.mousePosition).origin;
			Vector2Int targetCell = WorldToCell (worldPos);
			Vector2 cellPos = CellToWorld (targetCell);
			Gizmos.DrawCube (new Vector3 (cellPos.x, cellPos.y, 0), new Vector3 (cellWidth, cellHeight, 0.1f));
		}
	}

	// avoir une class permet de garder la référence (le struct ne fonctionne que par copie)
	// lorsqu'on explore un Noeud une nouvelle fois depuis un autre parent, 
	// Créer une copie du noeud pour mettre dans la liste ouverte.
	// Attention a ne pas modifier la référence du noeud qui serait déjà dans la liste ouverte ou fermée.
	public class Node : System.IComparable<Node> {
		public Vector2Int position;
		public float cout;
		public float heuristique;
		public Node parent = null;

		public Node (Vector2Int p, float h) {
			position = p;
			cout = -1; // cout cumulé à mettre à jour lors de l'exploration
			heuristique = h;
		}

		public float GetFCost() {
			return heuristique + cout;
		}

		public int CompareTo (Node obj) {
			return GetFCost().CompareTo (obj.GetFCost());
		}
	}
}