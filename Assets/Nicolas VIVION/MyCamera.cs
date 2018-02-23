using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyCamera : MonoBehaviour {


	private Dictionary<string, GameObject> matrice = new Dictionary<string, GameObject>();

	public Transform personnage;

	public Transform grid;

	public float tailleMapX;

	public float tailleMapY;

	private int matriceEnCoursX = 0;
	private int matriceEnCoursy = 0;


	void Start () {
		prepareMatrice();	
		deplaceCamera();
	}
	
	// Update is called once per frame
	void Update () {
	
	if(matrice.ContainsKey(getPositionMatrice())){
		if(personnage.position.y < matrice[getPositionMatrice()].transform.position.y - tailleMapY){
			changePositionMatrice(0,-1);
			deplaceCamera();
		}
		if(personnage.position.y >= matrice[getPositionMatrice()].transform.position.y + tailleMapY){
			changePositionMatrice(0,1);
			deplaceCamera();
		}
		if(personnage.position.x >= matrice[getPositionMatrice()].transform.position.x + tailleMapX){
			changePositionMatrice(1,0);
			deplaceCamera();
		}
		if(personnage.position.x < matrice[getPositionMatrice()].transform.position.x - tailleMapX){
			changePositionMatrice(-1,0);
			deplaceCamera();
		}
	}
		

	}

	public void deplaceCamera(){
		this.transform.position = new Vector3(
			matrice[getPositionMatrice()].transform.position.x,
			matrice[getPositionMatrice()].transform.position.y,
			this.transform.position.z
		);
	}

	public void prepareMatrice(){
		matrice.Add("0_0",grid.transform.GetChild(0).gameObject);
		matrice.Add("0_-1",grid.transform.GetChild(1).gameObject);
		matrice.Add("1_-1",grid.transform.GetChild(2).gameObject);
		matrice.Add("1_0",grid.transform.GetChild(3).gameObject);
	}

	public string getPositionMatrice()
	{
		return matriceEnCoursX.ToString()+"_"+matriceEnCoursy.ToString();
	}
	public void changePositionMatrice(int x, int y){
		matriceEnCoursX = matriceEnCoursX + x;
		matriceEnCoursy = matriceEnCoursy + y;
	}
}
