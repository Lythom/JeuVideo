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

	void Start () {
		prepareMatrice();	
		deplaceCamera();
	}

	
	void Update () {

		if(matrice.ContainsKey(getPositionMatrice())){
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
			MyMaps.position.x,
			MyMaps.position.y,
			this.transform.position.z
		);
		Vector3 moveOffset = Vector3.zero;
		if (Vector2.Distance (MyMaps.position, this.transform.position) > trackingDistance) {
			moveOffset = (targetPos - new Vector3(this.transform.position.x, this.transform.position.y, 0)) * stiffness;
		}
		this.transform.position += new Vector3(moveOffset.x, moveOffset.y, 0);
	}


	public void prepareMatrice(){
		matrice.Add("0_0", grid.transform.GetChild(0).gameObject);
		matrice.Add("1_0", grid.transform.GetChild(1).gameObject);
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
			matrice[getPositionMatrice()].transform.position.x,
			matrice[getPositionMatrice()].transform.position.y,
			this.transform.position.z
		);
	}

	public GameObject mapEnCour(){
		return matrice[getPositionMatrice()];
	}
}