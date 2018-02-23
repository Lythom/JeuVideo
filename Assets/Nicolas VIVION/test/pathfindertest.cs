using System;

using System.Collections.Generic;

using System.Linq;

using UnityEngine;



public struct Destination {

    public Node node;

    public float distance;

    public Destination (Node n, float d) {

        node = n;

        distance = d;

    }

}

// avoir une class permet de garder la référence (le struct ne fonctionne que par copie)

// lorsqu'on explore un Noeud une nouvelle fois depuis un autre parent, 

// Créer une copie du noeud pour mettre dans la liste ouverte.

// Attention a ne pas modifier la référence du noeud qui serait déjà dans la liste ouverte ou fermée.

public class Node {

    public string name;

    public float cout;

    public float heuristique;

    public Node parent;

    public Destination[] destinations;



    public Node (string n, float h) {

        name = n;

        cout = -1; // cout cumulé à mettre à jour lors de l'exploration

        heuristique = h;

        destinations = null;

    }

}



public static class PathFindingGraph {

    public static Node generateGraph () {

        Node S = new Node ("S", 5);

        Node A = new Node ("A", 5);

        Node B = new Node ("B", 3);

        Node C = new Node ("C", 4);

        Node D = new Node ("D", 6);

        Node E = new Node ("E", 5);

        Node F = new Node ("F", 6);

        Node G1 = new Node ("G1", 0);

        Node G2 = new Node ("G2", 0);

        Node G3 = new Node ("G3", 0);

        S.destinations = new Destination[] {

            new Destination (A, 5),

            new Destination (B, 9),

            new Destination (D, 6),

        };

        A.destinations = new Destination[] {

            new Destination (B, 3),

            new Destination (G1, 9),

        };

        B.destinations = new Destination[] {

            new Destination (A, 2),

            new Destination (C, 1),

        };

        C.destinations = new Destination[] {

            new Destination (S, 6),

            new Destination (G2, 5),

            new Destination (F, 7),

        };

        D.destinations = new Destination[] {

            new Destination (S, 1),

            new Destination (C, 2),

            new Destination (E, 2),

        };

        E.destinations = new Destination[] {

            new Destination (G3, 7),

        };

        F.destinations = new Destination[] {

            new Destination (G3, 8),

        };



        return S;

    }



    // explore brutallement tous les chemins de profondeur maxlevel

    public static void Crawl (int maxlevel, List<Node> path) {



        Debug.Log (String.Join ("->", path.Select (node => node.name).ToArray ()));

        if (maxlevel == 0) return;



        Destination[] dests = path.Last ().destinations;

        if (dests == null || dests.Length == 0) return;



        foreach (Destination d in dests) {

            List<Node> newPath = path.ToList ();

            newPath.Add (d.node);

            Crawl (maxlevel - 1, newPath);

        }

    }

}