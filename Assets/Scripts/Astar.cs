using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions 
    /// which describes a path from the startPos to the endPos
    /// 
    /// NOTE:
    /// that you will probably need to add some helper functions
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Cell nextCell = null;

        Cell startCell = new Cell();
        startCell.gridPosition = startPos;
        Node startNode = new Node();
        startNode.position = startPos;

        //First point of path
        path.Add(startPos);

        Cell endCell = new Cell();
        endCell.gridPosition = endPos;
        Node endNode = new Node();
        endNode.position = endPos;

        
        foreach (Cell cell in startCell.GetNeighbours(grid))
        {
            if(!cell.HasWall(Wall.UP) || !cell.HasWall(Wall.LEFT))
                path.Add(cell.gridPosition);
        }

        //Always add this last
        path.Add(endPos);
       
        //return path;
        return path;
    }

    /// <summary>
    /// Note:
    /// Check if it has walls,
    /// Set & Get the: FScore, GScore & HScore
    /// </summary>
    private void CalculateNextNode()
    {

    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance (Step distance + previous step distance)
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, int GScore, int HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
