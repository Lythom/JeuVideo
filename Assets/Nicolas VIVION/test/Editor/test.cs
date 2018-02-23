using System.Collections;

using System.Collections.Generic;

using NUnit.Framework;

using UnityEditor;

using UnityEngine;

using UnityEngine.TestTools;



public class AStar {



	[Test]

	public void AStarSimplePasses () {

		Node start = PathFindingGraph.generateGraph ();

		var list = new List<Node> ();

		list.Add (start);

		PathFindingGraph.Crawl (4, list);

	}



}