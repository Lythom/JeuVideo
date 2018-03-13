using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script doit être executer en premier !!!!!!!!!!!!
// edit -> project setting -> script execution order


// afin d'ajouter une carte ce qui est important c'est la position de map 
//(si la taille d'une carte fait x2 8 et y 16 par exemple)
// alors la position de la map doit est de + x 28 et y + 0 si elle est sur sa droite
// alors la position de la map doit est de + x 0 et y + 28 si elle est sur le haut
// si la map se place nimporte deplacer ensuite les tilemaps a l'intérieur de cette map

namespace vivion {
	public class Tracking : MonoBehaviour {


		public float trackingDistance = 0;
		public float stiffness = 0.05f;

		private Dictionary<string, GameObject> matrice = new Dictionary<string, GameObject>();

		public Transform personnage;

		public Transform grid;

		public float tailleMapX;

		public float tailleMapY;

		public int matriceEnCoursX = 0;
		public int matriceEnCoursY = 0;

		public float decalx = 0.18f;
		public float decaly = 0.25f;

		void Start () {
			prepareMatrice();	
			deplaceCamera();
		}

		public float persoY = 0;
		public float persoX = 0;
		public float mapx = 0;
		public float mapy = 0;
		void Update () {

			persoY = personnage.position.y;
			persoX = personnage.position.x;

			if(matrice.ContainsKey(getPositionMatrice())){

				mapx =  matrice[getPositionMatrice()].transform.position.x;
				mapy =  matrice[getPositionMatrice()].transform.position.y;

				if(personnage.position.y < matrice[getPositionMatrice()].transform.position.y - (tailleMapY/2)){
					changePositionMatrice(0,-1);
				}
				if(personnage.position.y >= matrice[getPositionMatrice()].transform.position.y + (tailleMapY/2)){
					changePositionMatrice(0,1);
				}
				if(personnage.position.x >= matrice[getPositionMatrice()].transform.position.x + (tailleMapX/2)){
					changePositionMatrice(1,0);
				}
				if(personnage.position.x < matrice[getPositionMatrice()].transform.position.x - (tailleMapX/2)){
					changePositionMatrice(-1,0);
				}
				goTracking(matrice[getPositionMatrice()].transform);
			}
		}



		public void goTracking(Transform MyMaps){
			Vector3 targetPos = new Vector3 (
				(MyMaps.position.x-decalx),
				(MyMaps.position.y-decaly),
				this.transform.position.z
			);
			Vector3 moveOffset = Vector3.zero;
			if (Vector2.Distance (MyMaps.position, this.transform.position) > trackingDistance) {
				moveOffset = (targetPos - new Vector3(this.transform.position.x, this.transform.position.y, 0)) * stiffness;
			}
			this.transform.position += new Vector3(moveOffset.x, moveOffset.y, 0);
		}


		public void prepareMatrice(){
			// matrice.Add("0_0", grid.transform.GetChild(0).gameObject);
			// matrice.Add("1_0", grid.transform.GetChild(1).gameObject);
			// matrice.Add("0_1", grid.transform.GetChild(2).gameObject);
			// matrice.Add("1_1", grid.transform.GetChild(3).gameObject);

			
			matrice.Add("0_0", grid.transform.GetChild(0).gameObject);
			matrice.Add("1_0", grid.transform.GetChild(1).gameObject);
			matrice.Add("2_0", grid.transform.GetChild(2).gameObject);
			matrice.Add("3_0", grid.transform.GetChild(3).gameObject);
			matrice.Add("4_0", grid.transform.GetChild(4).gameObject);

			
			matrice.Add("0_1", grid.transform.GetChild(5).gameObject);
			matrice.Add("1_1", grid.transform.GetChild(6).gameObject);
			matrice.Add("2_1", grid.transform.GetChild(7).gameObject);
			matrice.Add("3_1", grid.transform.GetChild(8).gameObject);
			matrice.Add("4_1", grid.transform.GetChild(9).gameObject);
			
			matrice.Add("0_2", grid.transform.GetChild(10).gameObject);
			matrice.Add("1_2", grid.transform.GetChild(11).gameObject);
			matrice.Add("2_2", grid.transform.GetChild(12).gameObject);
			matrice.Add("3_2", grid.transform.GetChild(13).gameObject);
			matrice.Add("4_2", grid.transform.GetChild(14).gameObject);

			matrice.Add("0_3", grid.transform.GetChild(15).gameObject);
			matrice.Add("1_3", grid.transform.GetChild(16).gameObject);
			matrice.Add("2_3", grid.transform.GetChild(17).gameObject);
			matrice.Add("3_3", grid.transform.GetChild(18).gameObject);
			matrice.Add("4_3", grid.transform.GetChild(19).gameObject);


		}

		public string getPositionMatrice(){
			return matriceEnCoursX.ToString()+"_"+matriceEnCoursY.ToString();
		}
		public void changePositionMatrice(int x, int y){
			matriceEnCoursX = matriceEnCoursX + x;
			matriceEnCoursY = matriceEnCoursY + y;
		}
		
		public void deplaceCamera(){
			this.transform.position = new Vector3(
				(matrice[getPositionMatrice()].transform.position.x-decalx),
				(matrice[getPositionMatrice()].transform.position.y-decaly),
				this.transform.position.z
			);
		}

		public GameObject mapEnCour(){
			return matrice[getPositionMatrice()];
		}
	}
}