using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace pathfinding {

    public class AStar : MonoBehaviour {

        private class DebugPool {
            private List<GameObject> list = new List<GameObject> ();

            public void Init (int count, GameObject debugPrefab) {
                GameObject parent = new GameObject ("debugParent");
                for (int i = 0; i < count; i++) {
                    list.Add (Instantiate (debugPrefab, parent.transform));
                }
            }

            public void Insert (GameObject debug) {
                debug.name = "default";
                debug.transform.position = Vector3.zero;
                debug.GetComponent<SpriteRenderer> ().color = Color.white;

                list.Add (debug);
            }

            public GameObject Get () {
                GameObject next = list.First ();
                list.Remove (next);
                return next;
            }
        }

        public static void InitPool (GameObject debugPrefab) {
            pool = new DebugPool ();
            pool.Init (2000, debugPrefab);
        }

        private static DebugPool pool;

        public static List<Vector2Int> FindPath (Vector2Int origin, Vector2Int targetCell, Func<Vector2Int, bool> Collide) {

            GameObject debug = GameObject.Find ("debug");
            while (debug != null) {
                pool.Insert (debug);
                debug = GameObject.Find ("debug");
            }

            List<Node> fermee = new List<Node> ();
            SortedList<float, Node> ouverte = new SortedList<float, Node> (new DuplicateKeyComparer<float> ());

            Node depart = new Node (origin, Vector2.Distance (origin, targetCell));
            depart.cout = 0;
            ouverte.Add (depart.GetFCost (), depart);
            int loopCount = 0;
            while (ouverte.Count > 0 && loopCount < 1000) {
                loopCount++;
                Node current = ouverte.First ().Value;
                ouverte.RemoveAt (0);
                fermee.Add (current);

                debug = pool.Get ();
                debug.name = "debug";
                debug.transform.position = new Vector3 (current.position.x / 4f, current.position.y / 4f, 0);
                debug.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 0.4f);
                debug.GetComponent<SpriteRenderer> ().sortingOrder = loopCount;

                if (current.position == targetCell) return MakePathFromLastNode (current, new List<Vector2Int> ());

                Node[] voisins = CreateAvailableNeighbours (current, targetCell);
                foreach (Node voisin in voisins) {
                    voisin.cout = current.cout + 1;
                    // Parcours le voisin si le nœud est parcourable 
                    // et s'il n'est pas déjà fermé avec un coût moindre
                    if (!IsClosed (voisin, fermee) &&
                        !Collide (voisin.position)
                    ) {
                        // cherche s'il y a déjà un noeud ouvert
                        Node voisinOuvert = getFromOpenList (voisin, ouverte);
                        // s'il n'existe pas, créer et ajouter
                        if (voisinOuvert == null) {
                            voisinOuvert = voisin;
                            ouverte.Add (voisinOuvert.GetFCost (), voisinOuvert);
                            voisinOuvert.parent = current;
                        }
                        // s'il existait déjà mais qu'il est plus couteux, mettre à jour
                        if (voisinOuvert.GetFCost () > voisin.GetFCost ()) {
                            voisinOuvert.parent = current;
                            voisinOuvert.cout = voisin.cout;
                        }

                        debug = pool.Get ();
                        debug.name = "debug";
                        debug.transform.position = new Vector3 (voisinOuvert.position.x / 4f, voisinOuvert.position.y / 4f, 0);
                        debug.GetComponent<SpriteRenderer> ().color = new Color (0, 1, 0, 0.4f);
                        debug.GetComponent<SpriteRenderer> ().sortingOrder = loopCount;
                    }
                }
            }

            return null;
        }

        private static bool IsClosed (Node voisin, List<Node> fermee) {
            for (int i = 0; i < fermee.Count; i++) {
                Node noeudFerme = fermee[i];
                if (noeudFerme.position.x == voisin.position.x &&
                    noeudFerme.position.y == voisin.position.y
                ) {
                    return true;
                }
            }
            return false;
        }

        private static Node getFromOpenList (Node voisin, SortedList<float, Node> open) {
            for (int i = 0; i < open.Count; i++) {
                Node noeudOuvert = open.Values[i];
                if (noeudOuvert.position.x == voisin.position.x &&
                    noeudOuvert.position.y == voisin.position.y
                ) {
                    return noeudOuvert;
                }
            }
            return null;
        }

        private static Node[] CreateAvailableNeighbours (Node current, Vector2Int destination) {
            Node[] nodes = new Node[4];
            Vector2Int up = current.position + Vector2Int.up;
            nodes[0] = new Node (up, Vector2.Distance (up, destination));
            Vector2Int right = current.position + Vector2Int.right;
            nodes[1] = new Node (right, Vector2.Distance (right, destination));
            Vector2Int left = current.position + Vector2Int.left;
            nodes[2] = new Node (left, Vector2.Distance (left, destination));
            Vector2Int down = current.position + Vector2Int.down;
            nodes[3] = new Node (down, Vector2.Distance (down, destination));
            return nodes;
        }

        private static List<Vector2Int> MakePathFromLastNode (Node current, List<Vector2Int> list) {
            if (current.parent != null) {
                MakePathFromLastNode (current.parent, list);
            }
            list.Add (current.position);
            return list;
        }

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable {
            #region IComparer<TKey> Members

            public int Compare (TKey x, TKey y) {
                int result = x.CompareTo (y);

                if (result == 0)
                    return 1; // Handle equality as beeing greater
                else
                    return result;
            }

            #endregion
        }
    }
}