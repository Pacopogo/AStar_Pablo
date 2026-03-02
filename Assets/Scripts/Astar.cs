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
    private Cell[,] worldGrid = new Cell[0,0];
    private int iterationsCounter = 0;
    private bool isWalking = true;

    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        worldGrid = grid;
        isWalking = true;
        iterationsCounter = 0;

        List<Node> Maze = new List<Node>(); //The collective tiles that are viewed
        List<Vector2Int> path = new List<Vector2Int>(); //The Path that will be returned

        List<Vector2Int> availableTiles = new List<Vector2Int>();   //The tiles that have been avaible from the neighbours


        Node currentTile = new Node();  //the orign form where you are calculating
        currentTile.position = startPos;
        currentTile.GScore = 0;
        currentTile.HScore = currentTile.CalculateManhattan(endPos);
        currentTile.hasVisited = false;
        Maze.Add(currentTile);

        while (isWalking)
        {
            ++iterationsCounter;
            Maze = IterateAStar(Maze, endPos);
            foreach (var node in Maze)
            {
                if (node.hasVisited)
                {
                    isWalking = node.HScore > 0;
                }
            }
        }

        path = ReverseCreatePath(Maze);

        return path;
    }

    private List<Node> IterateAStar(List<Node> nodes, Vector2Int target)
    {
        List<Node> maze = new List<Node>(nodes);
        int bestF = int.MaxValue;
        int bestIndex = -1;

        for (int i = 0; i < maze.Count; i++)
        {
            if (!maze[i].hasVisited)
            {
                if (maze[i].FScore < bestF)
                {
                    bestF = maze[i].FScore;
                    bestIndex = i;
                }
            }
        }

        maze[bestIndex].hasVisited = true;

        //if no unchecked nodes remain
        if (bestIndex == -1)
            return maze;

        //Defining directions for the next steps
        Vector2Int[] directions = {
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0)
        };

        foreach (Vector2Int direction in directions)
        {
            bool isFlagged = false;
            Vector2Int nextLocation = direction + maze[bestIndex].position;

            foreach (Node node in maze)
            {
                if (node.position == nextLocation)
                {
                    isFlagged = true;
                    break;
                }
            }

            if (isFlagged)
                continue;

            if (IsFree(maze[bestIndex], direction))
            {
                Node nextNode = new Node();
                nextNode.position = nextLocation;
                nextNode.GScore = maze[bestIndex].GScore + GScoreStep;
                nextNode.HScore = nextNode.CalculateManhattan(target);
                nextNode.hasVisited = false;

                maze.Add(nextNode);
            }
        }

        return maze;
    }

    //NOTE: If it doesn't work in practice revert the Y axis
    private bool IsFree(Node currentNode, Vector2Int direction)
    {
        bool hasWall = false;

        Cell thisCell = new Cell();
        int x = currentNode.position.x;
        int y = currentNode.position.y;
        thisCell = worldGrid[x,y];

        if (direction == new Vector2Int(-1, 0))
        {
            hasWall = thisCell.HasWall(Wall.LEFT);
        }
        else if (direction == new Vector2Int(1, 0))
        {
            hasWall = thisCell.HasWall(Wall.RIGHT);
        }
        else if(direction == new Vector2Int(0, -1))
        {
            hasWall = thisCell.HasWall(Wall.DOWN);
        }
        else if (direction == new Vector2Int(0, 1))
        {
            hasWall = thisCell.HasWall(Wall.UP);
        }

        return !hasWall;
    }

    private bool isNeighbour(Vector2Int posA, Vector2Int posB)
    {
        int dx = Mathf.Abs(posA.x - posB.x);
        int dy = Mathf.Abs(posA.y - posB.y);
        return (dx + dy) == 1;
    }

    private List<Vector2Int> ReverseCreatePath(List<Node> maze)
    {
        List<Vector2Int > path = new List<Vector2Int>();

        Node workNode = maze.First(node => node.HScore == 0);
        int g = workNode.GScore;

        //Start with the target and walk back to the starting point
        path.Add(workNode.position);

        while (g > 0)
        {
            g -= GScoreStep;
            //Find a neighbour with the pervious G score
            foreach (var node in maze)
            {
                if(isNeighbour(node.position,workNode.position ) && node.GScore == g)
                {
                    workNode = node;
                    path.Add(node.position);
                    break;
                }
            }
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public bool hasVisited;
        public int FScore
        { //GScore + HScore
            get { return GScore + HScore; }
        }
        public int GScore; //G score = the steps set +1
        public int HScore; //Distance estimated based on Heuristic

        /// <summary>
        /// Manhattan Distance calculation
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public int CalculateManhattan(Vector2Int a)
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
