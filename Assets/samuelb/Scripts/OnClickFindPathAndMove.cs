using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using pathfinding;
using UnityEngine;

public class OnClickFindPathAndMove : MonoBehaviour {

	public MyGrid grid;
	public Transform objectToMove;
	public Polyline currentPath;
	public Collider2D pathCollider;

	public GameObject debugPrefab;

	public Vector3? moveTo;

	void Start () {
		AStar.InitPool(debugPrefab);
	}

	void Update () {
		if (grid != null && Input.GetMouseButtonUp (0)) {
			Vector3 origin = objectToMove.position;
			var v3 = Input.mousePosition;
			v3.z = 10.0f;
			Vector3 targetCell = Camera.main.ScreenToWorldPoint (v3);
			var path = AStar
				.FindPath (grid.WorldToCell (origin), grid.WorldToCell (targetCell), Collide);
			if (path != null) {
				currentPath.nodes = path
					.Select (pos => (Vector3) grid.CellToWorld (pos))
					.ToList<Vector3> ();
			}

			moveTo = null;
		}
		if (moveTo.HasValue && Vector2.Distance (moveTo.Value, objectToMove.position) < 0.1f) {
			moveTo = null;
		}
		if (currentPath.nodes.Count > 0) {
			if (!moveTo.HasValue) {
				moveTo = currentPath.nodes[0];
				currentPath.nodes.RemoveAt (0);
			}
		}
		if (moveTo.HasValue) {
			this.objectToMove.position = Vector2.MoveTowards (this.objectToMove.position, moveTo.Value, 0.05f);
		}
	}

	private bool Collide (Vector2Int cellPos) {
		if (pathCollider == null) return false;
		return pathCollider.OverlapPoint (grid.CellToWorld (cellPos));
	}
}