using System.Collections.Generic;
using UnityEngine;
using AlphaGame.Core.GridSystem.Building;

namespace AlphaGame.Core.GridSystem.Pathfinding
{
    public class Pathfinder
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        private static Pathfinder instance;
        public static Pathfinder Instance
        {
            get
            {
                if(instance == null) instance = new Pathfinder(Globals.gridXCount, Globals.gridZCount);
                return instance;
            }
        }
        public Grid<PathNode> grid;
        private List<PathNode> openList;
        private List<PathNode> closedList;
        public Vector3 pathOffset { get { if(grid != null) return new Vector3(grid.scale, 0, grid.scale) * 0.5f; return Vector3.zero;} }
        public Pathfinder(int xGridCount, int zGridCount)
        {
            grid = new Grid<PathNode>(xGridCount, zGridCount, 1f, Vector3.zero, (Grid<PathNode> g, int x, int z) => new PathNode(g, x, z));
            var buildingGrid = BuildingController.Instance.grid;
            UpdateNeighbours();
        }
        private Pathfinder(Grid<BuildingNode> buildingGrid, float scale)
        {
            int xMulti = Mathf.RoundToInt(buildingGrid.scale / scale);
            int zMulti = Mathf.RoundToInt(buildingGrid.scale / scale);
            grid = new Grid<PathNode>(buildingGrid.xGridCount * xMulti, buildingGrid.zGridCount * zMulti, scale, Vector3.zero, (Grid<PathNode> g, int x, int z) => new PathNode(g, x, z));
            UpdateNeighbours();
        }
        public Pathfinder ChangeGridScale(float scale)
        {
            return new Pathfinder(BuildingController.Instance.grid, scale);
        }
        public void UpdateNeighbours()
        {
            var buildingGrid = BuildingController.Instance.grid;
            foreach(PathNode pathNode in grid.gridNodes)
            {
                pathNode.CopyFromBuildingGrid(buildingGrid); // TODO: Add ability to copy from different scales not just same scale.
            }
            // TODO: CopyBuildingNodesToPathNodes(buildingGrid);
        }
        
        public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ)
        {
            PathNode startNode = grid.GetGridObject(startX, startZ);
            PathNode endNode = grid.GetGridObject(endX, endZ);
            
            if(startNode == null || endNode == null) return null;
            if(!startNode.isWalkable || !endNode.isWalkable) return null;


            openList = new List<PathNode>(){ startNode };
            closedList = new List<PathNode>();
            for(int x = 0; x < grid.xGridCount; x++)
            {
                for(int z = 0; z < grid.zGridCount; z++)
                {
                    PathNode pathNode = grid.GetGridObject(x, z);
                    pathNode.gCost = int.MaxValue;
                    pathNode.previousNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = GetDistance(startNode, endNode);

            while(openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if(currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach(PathNode neighbour in currentNode.GetNeighbours())
                {
                    if(closedList.Contains(neighbour)) continue;
                    if(!neighbour.isWalkable)
                    {
                        closedList.Add(neighbour);
                        continue;
                    }
                    
                    int tentativeGCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(tentativeGCost < neighbour.gCost)
                    {
                        neighbour.previousNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = GetDistance(neighbour, endNode);

                        if(!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private PathNode GetNode(int x, int z)
        {
            return grid.GetGridObject(x, z);
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while(currentNode.previousNode != null)
            {
                path.Add(currentNode.previousNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();
            return path;
        }

        private int GetDistance(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int zDistance = Mathf.Abs(a.z - b.z);
            int remaining = Mathf.Abs(xDistance - zDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for(int i = 1; i < pathNodeList.Count; i++)
            {
                if(pathNodeList[i].fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }
        public void CopyBuildingNodesToPathNodes(Grid<BuildingNode> buildGrid)
        {
            foreach(BuildingNode buildNode in buildGrid.gridNodes)
            {
                PathNode pathNode = grid.GetGridObject(buildNode.x, buildNode.z);
                
            }
        }
    }
    
}