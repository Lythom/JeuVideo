using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace vivion {
	public class NodeAstar {
		public Vector2Int position;
		public NodeAstar parent = null;
		public float cout;
		public float heuristique;

		public NodeAstar(Vector2Int pos, float h){
			position = pos;
			cout = 0;
			heuristique = h;
		}

		public float getTotal(){
			return heuristique + cout;
		}
	}
	public class PathfindingAstar : MonoBehaviour {

		public Transform personnage;

		private List<Vector2Int> deplacement = null;

		public int enCours = 0;

		private GameObject myCam;
		private Tracking myScript;

		// Use this for initialization
		void Start () {
			myCam = GameObject.FindWithTag("MainCamera");
			myScript = (Tracking) myCam.GetComponent(typeof(Tracking));
			Vector2Int origin = WorldToCell(new Vector2(this.transform.position.x, this.transform.position.y));
			Vector2Int target = WorldToCell(new Vector2(personnage.position.x, personnage.position.y));
			deplacement = exploreWithPathFinding(origin, target);
		}
		
        private float accumulateur = 0;
        private float accMob = 0;
		
		void Update () {
			transform.rotation = Quaternion.Euler(0,0,0);

     		float frameDurationMob = GetFrameDurationInSecMob();
            accMob += Time.deltaTime;
            while(accMob > frameDurationMob) {

				if(deplacement != null && enCours < deplacement.Count){
					Vector2Int direction = deplacement[enCours];
					enCours++;
					mouve(direction);
				}
				accMob -= frameDurationMob;
			}

     		float frameDuration = GetFrameDurationInSec();
            accumulateur += Time.deltaTime;
            while (accumulateur > frameDuration && frameDuration > 0) {
				Vector2Int origin = WorldToCell(new Vector2(this.transform.position.x, this.transform.position.y));
				Vector2Int target = WorldToCell(new Vector2(personnage.position.x, personnage.position.y));
				deplacement = exploreWithPathFinding(origin, target);
				enCours = 0;
                accumulateur -= frameDuration;
			}

		}
		
		private float GetFrameDurationInSec () {
            return 1.5f;
        }
		private float GetFrameDurationInSecMob () {
            return 1f/5;
        }
	
		public List<Vector2Int> exploreWithPathFinding(Vector2Int origin, Vector2Int target){
			List<NodeAstar> lOuverte = new List<NodeAstar>();
			List<NodeAstar> lFerme = new List<NodeAstar>();
		
			NodeAstar depart = new NodeAstar(origin, Vector2.Distance(origin, target));
			lOuverte.Add(depart);

			int loopStop = 0;
			while(lOuverte.Count > 0 && loopStop < 200){
				loopStop++;
				NodeAstar current = getBestNode(lOuverte);
				lOuverte.Remove(current);
				lFerme.Add(current);

				if(TapeToPerso(current.position, target)) return makePath(current, new List<Vector2Int>());
				
				NodeAstar[] voisins = CreateAvailableNeighbours (current, target, lFerme);
				
               	foreach (NodeAstar voisin in voisins) {
					if(voisin != null){
						voisin.cout = current.cout + 1;
						if (!IsClosed (voisin, lFerme, voisin.getTotal ()) && !detectColision (voisin.position)){
							voisin.parent = current;
							lOuverte.Add(voisin);
						}
					}
               }
           	}

			return null;
		}
		
		private bool TapeToPerso(Vector2Int pos, Vector2Int target){
			if(Vector2.Distance(pos, target) < 0.2) {
				return true;
			}
			return false;
		}
		
		public bool detectColision(Vector2Int pos){
			GameObject map = myScript.mapEnCour();
			Tilemap tilemap = null;
			TileBase maTile = null;
			
			for (int i = 0; i < map.transform.childCount; i++){
				if(map.transform.GetChild(i).name == "Tilemap_col"){
					tilemap = map.transform.GetChild(i).GetComponent<Tilemap>();
				}
			}
		
			maTile = tilemap.GetTile(new Vector3Int(pos.x, pos.y, 0));
			if(!(maTile == null)) return true;

			return false;
		}

		public NodeAstar getBestNode(List<NodeAstar> lOuverte){
			float maxTotal = 99999999999999;
			NodeAstar node = null;
			for (int i = 0; i < lOuverte.Count; i++){
				if(lOuverte[i].getTotal() < maxTotal){
					maxTotal = lOuverte[i].getTotal();
					node = lOuverte[i];	
				}
			}
			return node;
		}

		private static bool IsClosed (NodeAstar voisin, List<NodeAstar> fermee, float voisinFCost) {
           for (int i = 0; i < fermee.Count; i++) {
               NodeAstar noeudFerme = fermee[i];
               if (noeudFerme.position == voisin.position && noeudFerme.getTotal () < voisinFCost) return true;
           }
           return false;
       	}

		private List<Vector2Int> makePath(NodeAstar current, List<Vector2Int> list){
			if(current.parent != null){
				makePath(current.parent, list);
			}
			list.Add(current.position);
			return list;
		}

		private NodeAstar[] CreateAvailableNeighbours (NodeAstar current, Vector2Int destination, List<NodeAstar> closed) {
			
			GameObject map = myScript.mapEnCour();
			

			NodeAstar[] nodes = new NodeAstar[4];

			Vector2Int up = current.position + Vector2Int.up;
			if(up.y > (map.transform.position.y + (myScript.tailleMapX/2))){
				nodes[0] = null;
			}else{
				nodes[0] = new NodeAstar(up, Vector2.Distance (up, destination));
			}

			Vector2Int right = current.position + Vector2Int.right;
			if(right.x > (map.transform.position.x + (myScript.tailleMapY/2))){
				nodes[1] = null;	
			}else{
				nodes[1] = new NodeAstar(right, Vector2.Distance (right, destination));
			}

			Vector2Int left = current.position + Vector2Int.left;
			if(left.x < (map.transform.position.x - (myScript.tailleMapX/2))){
				nodes[2] = null;
			}else{
				nodes[2] = new NodeAstar(left, Vector2.Distance (left, destination));
			}

			Vector2Int down = current.position + Vector2Int.down;
			if(down.y < (map.transform.position.y - (myScript.tailleMapY/2))){
				nodes[3] = null;
			}else{
				nodes[3] = new NodeAstar(down, Vector2.Distance (down, destination));
			}

			return nodes;

        }

		public Vector2Int WorldToCell (Vector2 worldPos) {
			return new Vector2Int ((int) Mathf.Floor (worldPos.x / 1f), (int) Mathf.Floor (worldPos.y / 1f));
		}

		public void mouve(Vector2Int direction){
			this.transform.position = new Vector3(direction.x, direction.y, 0);
		}
	}
}
