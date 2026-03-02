using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

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
    /// 

    private const int GScoreStep = 1;

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        List<Vector2Int> availableTiles = new List<Vector2Int>();

        List<Node> currentNodes = new List<Node>();
        

        Node currentTile = new Node();  //the orign form where you are calculating
        currentTile.position = startPos;
        currentTile.GScore = 0;
        currentTile.HScore = currentTile.CalculateManhattan(endPos);

        //First point of path
        path.Add(currentTile.position);

        foreach (var tile in GetAvailibleNeigbours(currentTile, grid))
        {
            Node tileNode = new Node();
            tileNode.position = tile.gridPosition;
            tileNode.GScore = currentTile.GScore + GScoreStep;
            tileNode.HScore = tileNode.CalculateManhattan(endPos);

            currentNodes.Add(tileNode);
        }

        Node lowestNode = currentNodes[0];
        float minFScore = currentNodes[0].FScore;

        foreach (var node in currentNodes)
        {
            if(node.FScore < minFScore)
            {
                lowestNode = node;
                minFScore = node.FScore;
            }
        }

        path.Add(lowestNode.position);

        while (true)
        {
            break;
            if (currentTile.position == endPos)
                break;

            List<Vector2Int> newTiles = new List<Vector2Int>();

            foreach (var tile in availableTiles)
            {
                currentTile.position = tile;

                foreach (var t in GetAvailibleNeigbours(currentTile, grid))
                {
                    newTiles.Add(t.gridPosition);
                    path.Add(t.gridPosition);
                }
            }
            
            foreach(var tile in newTiles)
            {
                availableTiles.Add(tile);
            }
        }


        //always add this last
        currentTile.position = endPos;
        path.Add(endPos);

        //return path;
        return path;
    }

    private List<Cell> GetAvailibleNeigbours(Node current, Cell[,] grid)
    {
        Cell originCell = new Cell();   //The Cell it is being calculated from
        originCell.gridPosition = current.position;

        List<Cell> returnCells = new List<Cell>();

        List<Cell> neighbours = originCell.GetNeighbours(grid);

        foreach (var cell in neighbours)
        {
            if (cell.gridPosition.y > current.position.y)
            {
                if (cell.HasWall(Wall.DOWN))
                    continue;

                returnCells.Add(cell);
            }
            else if(cell.gridPosition.y < current.position.y)
            {
                if (cell.HasWall(Wall.UP))
                    continue;

                returnCells.Add(cell);
            }

            if (cell.gridPosition.x > current.position.x)
            {
                if (cell.HasWall(Wall.LEFT))
                    continue;

                returnCells.Add(cell);
            }
            else if(cell.gridPosition.x < current.position.x)
            {
                if (cell.HasWall(Wall.RIGHT))
                    continue;

                returnCells.Add(cell);
            }

        }



        return returnCells;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore
        { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //G score = the steps set +1
        public float HScore; //Distance estimated based on Heuristic

        /// <summary>
        /// Manhattan Distance calculation
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public float CalculateManhattan(Vector2Int a)
        {
            return Mathf.Abs(position.x - a.x) + Mathf.Abs(position.y - a.y);
        }

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
