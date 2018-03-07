using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public struct MyDestination {
    public MyNode node;
    public float distance;
    public MyDestination (MyNode n, float d) {
        node = n;
        distance = d;  // distance parcourue entre le noeud courant et le depart(g)
    }
}

public class MyNode {
    public float[] position;
    public MyNode parent;
	public float cout;
    public float heuristique;
    public MyDestination[] destinations;
	public int num;

    public MyNode (int tnum, float[] n, float h) {
        position = n;		// position du noeud [x, y]
		cout = 0;			// total entre g et h
        heuristique = h; 	// distance entre le noeud courant et la fin (h) (vole d'oiseau)
        destinations = null;
		parent = null;
		num = tnum;			// profondeur dans le jeux
    }
	public MyNode(int tnum, float[] n, float h , float f){
		parent = null;
        position = n;		// position du noeud [x, y]
		cout = f;			// total entre g et h
        heuristique = h; 	// distance entre le noeud et la fin (h)
        destinations = null;
		num = tnum;
	}
	public MyNode(int tnum, float[] n, float h , float f, MyNode myNodeParent){
		parent = myNodeParent;
        position = n;		// position du noeud [x, y]
		cout = f;			// total entre g et h
        heuristique = h; 	// distance entre le noeud et la fin (h)
        destinations = null;
		num = tnum;
	}
}

public class pathfinding : MonoBehaviour {

	public Transform personnage;

	private float unPas = 1;

	private MyNode nodeDuMob;

	private int numEnCours = 0;
	

	void Start () {
		// a l'init ont définit au mob le chemin a prendre
		nodeDuMob = exploreWithPathFinding(new List<MyNode>());
	}
	

	void Update () {
		if(nodeDuMob != null){
			float[] direction = directionAPrendreDuMob();
			mouve(direction[0], direction[1]);
		}
		nodeDuMob = exploreWithPathFinding(new List<MyNode>());
	}

	// grace a la node genrerer depuis la fonction exploreWithPathFinding()
	// nous pouvons recuperer le dernier parent, cela représente le premier deplacement du mob
	// nous mettons ensuite a null le dernier afin de continuer (un deplacement par frame)
	public float[] directionAPrendreDuMob(){
		bool stop = true;
		
		MyNode node = nodeDuMob;
		
		float[] direction = new float[2];
		direction[0] = 0;
		direction[1] = 0;
		while(stop){
			if(node.parent.num == numEnCours){
				//Debug.Log(node.position[0] + " "+node.position[1]);

				if(leMobTaTape(node, personnage.transform.position.x, personnage.transform.position.y)){
					return direction;
				}
				if(node.position[0] > this.transform.position.x) direction[0] = diffPOs(node.position[0], this.transform.position.x);
				if(node.position[0] < this.transform.position.x) direction[0] = -diffPOs(node.position[0], this.transform.position.x);
				if(node.position[1] > this.transform.position.y) direction[1] = diffPOs(node.position[1], this.transform.position.y);
				if(node.position[1] < this.transform.position.y) direction[1] = -diffPOs(node.position[1], this.transform.position.y);
				// tant que notre mob n'est pas arriver a ca position ont continue a le faire avancer
				if(bienArriver(node.position[0], node.position[1])){
					numEnCours = node.num;
				}
				// sortie de la boucle
				stop = false;
			}else{
				node = node.parent;
			}
			
		}
		// renvoyer 1 ou -1 pour x et y
		return direction;
	}
	public float diffPOs(float f1, float f2){
		return Mathf.Max(f1,f2) - Mathf.Min(f1,f2);
	}

	public bool bienArriver(float x, float y){
		if(x.Equals(Mathf.Round(this.transform.position.x*100f)/100f) && y.Equals(Mathf.Round(this.transform.position.y*100f)/100f)) return true;
		return false;
	}

	/*public int[] directionAPrendre(float posEnCoursX, float posEnCoursY){
		int[] direction = new int[2];

		if(personnage.position.y > posEnCoursY){
			direction[1] = 1;
		}else if(personnage.position.y < posEnCoursY){
			direction[1] = -1;
		}else{
			direction[1] = 0;
		}

		if(personnage.position.x > posEnCoursX){
			direction[0] = 1;
		}else if(personnage.position.x < posEnCoursX){
			direction[0] = -1;
		}else{
			direction[0] = 0;
		}

		return direction;
	}*/

	public MyNode exploreWithPathFinding(List<MyNode> mesNodesListFerme){
	
		// Ajout de mon premier noeud (représente le mob)
		float[] pathPos = new float[2];
		pathPos[0] = this.transform.position.x;
		pathPos[1] = this.transform.position.y;
		
		MyNode pathMyNode = new MyNode(0, pathPos, calculHeuristique(this.transform.position.x, this.transform.position.y, personnage.position.x, personnage.position.y));
		mesNodesListFerme.Add(pathMyNode);
		bool tape = true;
		while(tape){

			List<MyNode> pathMesNodes;
			MyNode pathMaNodesAvecDirections;
			MyNode pathMyNextNode;
			// Ajout des nodes suivantes + les directions vers les nodes
			pathMesNodes = addNodeSuivante(pathMyNode);
			
			pathMaNodesAvecDirections = addDirectionToMyNode(pathMyNode, pathMesNodes);
			
			// ajouter la node parente au enfant 			
			pathMaNodesAvecDirections = addParent(pathMaNodesAvecDirections, pathMesNodes);
			
			// choix de la node suivante (celle qui a le cout le plus faible)
			pathMyNextNode = choixNodeSuivante(pathMaNodesAvecDirections, mesNodesListFerme);
			if(pathMyNextNode == null){
				tape = true;
				exploreWithPathFinding(mesNodesListFerme);
			}else{
				// une fois une node calculer ont l'ajoute a la liste déjà parcourue
				mesNodesListFerme.Add(pathMyNextNode);

				// passage a la variable pour continuer la boucle
				pathMyNode = pathMyNextNode;

				Debug.Log(pathMyNextNode.position[0] + " "+pathMyNextNode.position[1]);
				// si arrive au personnage ont garde le chemin
				if(leMobTaTape(pathMyNextNode, personnage.position.x, personnage.position.y)){
					// jackpot on retourne ont sort de la boucle
					tape = false;
				}
			}

			
		}
		
		return pathMyNode;
	}

	// ont verifie si nous somme arrivé a la position du personnage 
	// ont vérifie sur la position personne + unPas (le deplacement ce fait unPas par unPas, ont peut donc ne pas tomber pile poile sur la position du personnage)
	// return true si ont y est
	public bool leMobTaTape(MyNode myNode, float persoX, float persoY){
		if(Mathf.Abs(myNode.position[0]-persoX) < unPas && Mathf.Abs(myNode.position[1]-persoY) < unPas){	
			return true;
		}
		return false;
	}

	public MyNode choixNodeSuivante(MyNode maNode, List<MyNode> mesNodesListFerme){
		MyNode node = null;
		float maxCout = 99999999999;
	
		for (int i = 0; i < maNode.destinations.Length; i++)
		{
			if(maNode.destinations[i].node.cout < maxCout){
				// si deja dans la liste ferme ont y repasse pas
				if(detectListFerme(maNode.destinations[i].node, mesNodesListFerme)){
					maxCout = maNode.destinations[i].node.cout;
					node = maNode.destinations[i].node;
				}
			}
		}
		return node;
	}

	// Prend en params la position courante et la position a atteindre
	// Calcul de type vole d'oiseau
	public float calculHeuristique(float posx, float posy, float persoX, float persoY){
		float tempoPersoX =persoX;
		float tempoPersoY =persoY;
		float tempoPosX =posx;
		float tempoPosY =posy;
		float retourx = 0;
		float retoury = 0;

		if(persoX < 0) tempoPersoX = persoX*-1;
		if(persoY < 0) tempoPersoY = persoY*-1;
		if(posx < 0) tempoPosX = posx*-1;
		if(posy < 0) tempoPosY = posy*-1;

		retourx = Mathf.Max(tempoPosX,tempoPersoX) -  Mathf.Min(tempoPosX,tempoPersoX);
		if(retourx < 0) retourx = retourx*-1;

		retoury = Mathf.Max(tempoPosY,tempoPersoY) -  Mathf.Min(tempoPosY,tempoPersoY);
		if(retoury < 0) retoury = retoury*-1;

		return retourx+retoury;
	}
	
	// Ajout des nodes suivante a la node passer en param
	public List<MyNode> addNodeSuivante(MyNode myNode){
		List<MyNode> mesNodes = new List<MyNode>();
		//prepa des futurs positions
		List<float[]> mesPositionsfuturNodes = preparePositionNextNode(myNode.position);

		// pour toutes les positions des futurs nodes
		mesPositionsfuturNodes.ForEach(items =>{
			// if de la collision == true pas de colision sinon ont ne créer pas de node // 
			if(detectColision(items)){
				MyNode myNodeNext = new MyNode(myNode.num+1, items, calculHeuristique(items[0], items[1], personnage.position.x, personnage.position.y));
				mesNodes.Add(myNodeNext);
			}
		});
		
		return mesNodes;
	}
	
	// renvoi true si non compris dans la liste ferme
	public bool detectListFerme(MyNode node , List<MyNode> mesNodesListFerme){
		for (int i = 0; i < mesNodesListFerme.Count; i++){ 
			if(node.position[0] == mesNodesListFerme[i].position[0] && node.position[1] == mesNodesListFerme[i].position[1]) return false;
		}
		return true;	
	}

	// Prend en parametre les positions d'une node 
	// Renvoi la liste des positions des nodes autour
	public List<float[]> preparePositionNextNode(float[] position){
		var list = new List<float[]>();
		// celle du dessus
		float[] dessus = new float[2];
		dessus[0] = position[0];
		dessus[1] = position[1]+unPas;
		// celle du dessous
		float[] dessous = new float[2];
		dessous[0] = position[0];
		dessous[1] = position[1]-unPas;
		// celle du gauche
		float[] gauche = new float[2];
		gauche[0] = position[0]-unPas;
		gauche[1] = position[1];
		// celle du droite
		float[] droite = new float[2];
		droite[0] = position[0]+unPas;
		droite[1] = position[1];

		list.Add(dessus);
		list.Add(dessous);
		list.Add(droite);
		list.Add(gauche);
		return list;
	}

	// Calcul la distance entre le noeud courant et le point de depart
	// Prend en param la node parent + la node en cours d'ajout
	// Retourne la distance
	public float calculDistance(MyNode myNode, MyNode NodeEnCours){
		int distanceUneCase = 1;
		// Si pas de parent alors ont est au debut, ont init la distance a 1
		if(myNode.parent == null) return distanceUneCase;

		float dest = 0;
		
		for(int i = 0; i < myNode.parent.destinations.Length; i++){
			if(myNode.parent.destinations[i].Equals(NodeEnCours)){
				dest = myNode.parent.destinations[i].distance;
			}
		}
		// toujours 1 de distance vu qu'on le fait toutes les 1x ou 1y ? 
		return dest + distanceUneCase;
	}

	// params : une node et les nodes qui lui doivent être liée
	// ajoute les directions entres les nodes
	public MyNode addDirectionToMyNode(MyNode myNode, List<MyNode> mesNodes){
		MyNode MyNodeReturn = new MyNode(myNode.num, myNode.position, myNode.heuristique, myNode.cout, myNode.parent);
		MyDestination[] listdestinations = new MyDestination[mesNodes.Count];

		// pour chaque node ont ajoute une destinations pour l'atteindre avec le calcul de la distance
		for(int i = 0; i < mesNodes.Count; i++){
			float dist = calculDistance(myNode, mesNodes[i]);
			// mise a jour du cout
			mesNodes[i].cout = mesNodes[i].heuristique + dist;
			listdestinations[i] = new MyDestination(mesNodes[i], dist);
		}
		
		MyNodeReturn.destinations = listdestinations;
		return MyNodeReturn;
	}

	public MyNode addParent(MyNode myNode, List<MyNode> mesNodes){
		for(int i = 0; i < myNode.destinations.Length; i++){
			foreach (MyNode node in mesNodes)
			{
				if(myNode.destinations[i].node.Equals(node)){
					node.parent = myNode;
					myNode.destinations[i].node = node;
				}
			}
		}
		return myNode;
	}
	// Check si la position passé  en param est en colision avec le decor
	// Params : float[x, y] position du mob
	// Return true si pas de colision
	public bool detectColision(float[] futurPosition){
		GameObject myCam = GameObject.FindWithTag("MainCamera");
		Tracking myScript = (Tracking) myCam.GetComponent(typeof(Tracking));
		GameObject map = myScript.mapEnCour();
		Tilemap tilemap = null;

		for (int i = 0; i < map.transform.childCount; i++){
			if(map.transform.GetChild(i).name == "Tilemap_col"){
				tilemap = map.transform.GetChild(i).GetComponent<Tilemap>();
			}
		}
	
		int x = 0;
		if(futurPosition[0] < 0){
			x = Mathf.CeilToInt(futurPosition[0]);
		}else{
			x = Mathf.FloorToInt(futurPosition[0]);
		}
		int y = 0;
		if(futurPosition[1] < 0){
			y = Mathf.CeilToInt(futurPosition[1]);
		}else{
			y = Mathf.FloorToInt(futurPosition[1]);
		}
		TileBase maTile = tilemap.GetTile(new Vector3Int(x, y, 0));

		if(maTile == null){
			return true;
		}else{
			return false;
		}
	}

	// Debut deplacement du mob // 
	public float acceleration = 1f; // unit per second, per second
	public float maxSpeed = 1f; // unit per second
	public Vector3 currentSpeed;
	public void mouve(float x, float y){
		
		this.transform.position += new Vector3(x,y,0);

		// de -1 a 1 le retour de inputgetaxis
		// Vector3 currentAcceleration = new Vector3 (
		// 	x * acceleration,
		// 	y * acceleration,
		// 	0
		// );
		// currentSpeed += currentAcceleration * Time.deltaTime;
		// currentSpeed = Vector3.ClampMagnitude(currentSpeed, maxSpeed);
		// if (currentAcceleration.magnitude == 0) currentSpeed *= 0.8f;
		// this.transform.position += currentSpeed * Time.deltaTime;
	}
	// Fin deplacement du mob //
}
