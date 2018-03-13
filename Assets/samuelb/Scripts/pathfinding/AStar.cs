using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace pathfinding {

    public class AStar {
        public static List<Vector2Int> FindPath (Vector2Int origin, Vector2Int targetCell, Func<Vector2Int, bool> Collide) {
            List<Node> fermee = new List<Node> ();
            SortedList<float, Node> ouverte = new SortedList<float, Node> (new DuplicateKeyComparer<float> ());

            Node depart = new Node (origin, Vector2.Distance (origin, targetCell));
            depart.cout = 0;
            ouverte.Add (depart.GetFCost (), depart);
            int loopCount = 0;
            while (ouverte.Count > 0 && loopCount < 1000) {
                loopCount ++;
                Node current = ouverte.First ().Value;
                ouverte.RemoveAt (0);
                fermee.Add (current);

                if (current.position == targetCell) return MakePathFromLastNode (current, new List<Vector2Int> ());

                Node[] voisins = CreateAvailableNeighbours (current, targetCell, fermee);
                foreach (Node voisin in voisins) {
                    voisin.cout = current.cout + 1;
                    // On parcours le voisin si le nœud est parcourable 
                    // et s'il n'est pas déjà fermé avec un coût moindre
                    if (!IsClosed (voisin, fermee, voisin.GetFCost ()) &&
                        !Collide (voisin.position)
                    ) {
                        voisin.parent = current;
                        ouverte.Add (voisin.GetFCost (), voisin);
                    }
                }
                fermee.Add (current);
            }

            return null;
        }

        private static bool IsClosed (Node voisin, List<Node> fermee, float voisinFCost) {
            for (int i = 0; i < fermee.Count; i++) {
                Node noeudFerme = fermee[i];
                if (noeudFerme.position == voisin.position && noeudFerme.GetFCost () < voisinFCost) return true;
            }
            return false;
        }

        private static Node[] CreateAvailableNeighbours (Node current, Vector2Int destination, List<Node> closed) {
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