using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GA.Collections;
using Cell = GA.Ships.Pathfinding.NavigationGrid.Cell;

namespace GA.Ships.Pathfinding
{
    public class Pathfinder
    {
        public class FrontierEntry : IComparable<FrontierEntry>
        {
            public Cell Cell { get; }
            public int TotalCost { get; }

            public FrontierEntry(Cell cell, int totalCost)
            {
                Cell = cell;
                TotalCost = totalCost;
            }

            public int CompareTo(FrontierEntry other)
            {
                return other == null ? -1 : TotalCost.CompareTo(other.TotalCost);
            }
        }

        private NavigationGrid _grid = null;

        // Cells, which will be inspected.
        private PriorityQueue<FrontierEntry> _frontier = new PriorityQueue<FrontierEntry>();

        // Cells which have been inspected already.
        private HashSet<Cell> _visited = new HashSet<Cell>();

        public Pathfinder(NavigationGrid grid)
        {
            _grid = grid;
        }

        #region Breadth-First Search
        /// <summary>
        /// Performs a breadth-first search to find a path from the start position to the end position
        /// </summary>
        /// <param name="startPosition">Start position</param>
        /// <param name="endPosition">End position</param>
        /// <returns>List of positions representing the path, or null if no path is found</returns>
        public IList<Vector3> BreadthFirstSearch(Vector3 startPosition, Vector3 endPosition)
        {
            Queue<Cell> frontier = new Queue<Cell>();
            Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();

            Cell startCell = _grid.GetCell(startPosition);
            Cell endCell = _grid.GetCell(endPosition);

            frontier.Enqueue(startCell);
            cameFrom[startCell] = null;

            bool isEndReached = false; // Early exit flag

            while (frontier.Count > 0)
            {
                Cell current = frontier.Dequeue();

                isEndReached = current == endCell;

                if (isEndReached)
                {
                    // The end node is reached. Path is complete.
                    break;
                }

                IList<Cell> neighbours = _grid.GetNeighbours(current, includeDiagonal: false);

                foreach (Cell neighbour in neighbours)
                {
                    if (neighbour.IsWalkable && !cameFrom.ContainsKey(neighbour))
                    {
                        frontier.Enqueue(neighbour);
                        cameFrom[neighbour] = current;
                    }
                }
            }

            // If isEndReached is false here, there is no path to the end cell.
            if (isEndReached)
            {
                // Construct path
                return ConstructPath(startCell, endCell, cameFrom);
            }

            // There is no path between start and end positions.
            return null;
        }

        /// <summary>
        /// Finds all reachable cells from the start cell within a given number of steps.
        /// </summary>
        /// <param name="start">The starting cell</param>
        /// <param name="maxSteps">Maximum number of steps to explore</param>
        /// <returns>Reachable cells</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public IList<Cell> GetReachableCells(Cell start, int maxSteps)
        {
            if (start == null)
            {
                throw new ArgumentNullException($"{nameof(start)} cannot be null");
            }

            if (maxSteps < 0)
            {
                throw new InvalidOperationException($"{nameof(maxSteps)} cannot be lower than 0");
            }

            Queue<Cell> frontier = new Queue<Cell>();
            Dictionary<Cell, int> steps = new Dictionary<Cell, int>();
            IList<Cell> reachableCells = new List<Cell>();

            frontier.Enqueue(start);
            steps[start] = 0;
            reachableCells.Add(start);

            // Perform a breadth-first search to find all reachable cells within the maxSteps limit
            while (frontier.Count > 0)
            {
                Cell current = frontier.Dequeue();
                int currentSteps = steps[current];

                if (currentSteps >= maxSteps)
                {
                    continue;
                }

                IList<Cell> neighbours = _grid.GetNeighbours(current, includeDiagonal: false);

                foreach (Cell neighbour in neighbours)
                {
                    if (steps.ContainsKey(neighbour))
                    {
                        continue;
                    }

                    int neighbourSteps = currentSteps + 1;

                    steps[neighbour] = neighbourSteps;
                    frontier.Enqueue(neighbour);
                    reachableCells.Add(neighbour);
                }
            }

            return reachableCells;
        }

        private IList<Vector3> ConstructPath(Cell startCell, Cell endCell, Dictionary<Cell, Cell> cameFrom)
        {
            IList<Vector3> path = new List<Vector3>();
            Cell current = endCell;

            while (current != startCell)
            {
                path.Add(current.WorldPosition);
                current = cameFrom[current];
            }

            path.Reverse();

            return path;
        }
        #endregion

        #region Dijkstra's Algorithm

        /// <summary>
        /// Performs Dijkstra's algorithm to find the shortest path from the start position to the end position
        /// </summary>
        /// <param name="startPosition">Start position</param>
        /// <param name="endPosition">End position</param>
        /// <returns></returns>
        public IList<Vector3> Dijkstra(Vector3 startPosition, Vector3 endPosition)
        {
            Cell startCell = _grid.GetCell(startPosition);
            Cell endCell = _grid.GetCell(endPosition);

            if (startCell == null || endCell == null || startCell == endCell ||
                !startCell.IsWalkable || !endCell.IsWalkable)
            {
                // Early exit in case there is no valid path possible
                return null;
            }

            _frontier.Clear();
            _visited.Clear();
            _grid.ResetPathData();

            startCell.Parent = null;
            startCell.GCost = 0; // At the beginning the cost is 0. We haven't travelled anywhere yet.
            startCell.HCost = 0; // Has to be zeroed if A* was used between two Djikstra calls.

            _frontier.Enqueue(new FrontierEntry(startCell, 0));

            while (_frontier.Count > 0)
            {
                FrontierEntry entry = _frontier.Dequeue();
                Cell current = entry.Cell;

                if (!_visited.Add(current))
                {
                    //Outdated entry. The cell was already processed through a cheaper route.
                    continue;
                }

                if (current == endCell)
                {
                    // Early exit.
                    // We have reached the end cell. No need to continue.
                    break;
                }

                IList<Cell> neighbours = _grid.GetNeighbours(current, PathfindingConfig.AllowDiagonalPathfinding);

                foreach (Cell neighbour in neighbours)
                {
                    if (!neighbour.IsWalkable || _visited.Contains(neighbour))
                    {
                        // Skip this neighbour if it's not walkable or if it has been already visited.
                        continue;
                    }

                    int costToNeighbour = _grid.GetCostToNeighbour(current, neighbour);

                    if (costToNeighbour <= 0)
                    {
                        // The neighbour is not a neighbour of the current cell.
                        GD.PrintErr("Invalid cost to the neighbour! Did GetNeighbour return a Cell which is not a neighbour?");
                        continue;
                    }

                    // The total cost of the path so far
                    int costSoFar = current.GCost + costToNeighbour;

                    // If there is a better path to neighbour
                    if (costSoFar < neighbour.GCost)
                    {
                        // Update the cost to the neighbour from the current cell.
                        neighbour.GCost = costSoFar;
                        neighbour.HCost = 0;

                        // Add to the frontier in order to process its neighbours
                        _frontier.Enqueue(new FrontierEntry(neighbour, costSoFar));

                        // It's cheapest to navigate to this neighbour from the current cell.
                        neighbour.Parent = current;
                    }
                }
            }

            return RetracePath(startCell, endCell);
        }

        /// <summary>
        /// Finds all reachable cells from the start cell within a given number of movement points.
        /// This method uses Dijkstra's algorithm to explore the grid and determine which cells can be reached
        /// without exceeding the specified movement points.
        /// </summary>
        /// <param name="start">the starting cell</param>
        /// <param name="maxMovementPoints">the maximum number of movement points available</param>
        /// <param name="useDiagonals">is diagonal movement allowed</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IList<Cell> GetReachableCells(Cell start, int maxMovementPoints, bool useDiagonals)
        {
            if (start == null)
            {
                throw new ArgumentNullException($"{nameof(start)} cannot be null");
            }

            if (maxMovementPoints < 0)
            {
                throw new InvalidOperationException($"{nameof(maxMovementPoints)} cannot be lower than 0");
            }

            if (!start.IsWalkable)
            {
                throw new ArgumentException($"{nameof(start)} must be walkable");
            }

            _frontier.Clear();
            _visited.Clear();
            _grid.ResetPathData();

            IList<Cell> reachableCells = new List<Cell>();

            start.Parent = null;
            start.GCost = 0;
            start.HCost = 0;

            _frontier.Enqueue(new FrontierEntry(start, 0));

            while (_frontier.Count > 0)
            {
                FrontierEntry entry = _frontier.Dequeue();
                Cell current = entry.Cell;

                if (!_visited.Add(current))
                {
                    //Outdated entry. The cell was already processed through a cheaper route.
                    continue;
                }

                if (entry.TotalCost > maxMovementPoints)
                {
                    // Early exit.
                    // The cheapest route to this cell exceeds movement points.
                    break;
                }

                if (current != start)
                {
                    // Add the current cell to the list of reachable cells, excluding the start cell.
                    reachableCells.Add(current);
                }

                IList<Cell> neighbours = _grid.GetNeighbours(current, useDiagonals);

                foreach (Cell neighbour in neighbours)
                {
                    if (!neighbour.IsWalkable || _visited.Contains(neighbour))
                    {
                        // Skip this neighbour if it's not walkable or if it has been already visited.
                        continue;
                    }

                    int movementCost = _grid.GetCostToNeighbour(current, neighbour);

                    if (movementCost <= 0)
                    {
                        continue;
                    }

                    // The total cost of the path so far
                    int newMovementCost = current.GCost + movementCost;

                    if (newMovementCost > maxMovementPoints)
                    {
                        // Skip this neighbour if the total cost exceeds the maximum movement points.
                        continue;
                    }

                    if (newMovementCost < neighbour.GCost)
                    {
                        neighbour.GCost = newMovementCost;
                        neighbour.HCost = 0;

                        _frontier.Enqueue(new FrontierEntry(neighbour, newMovementCost));

                        neighbour.Parent = current;
                    }
                }
            }

            return reachableCells;
        }
        #endregion

        #region A*

        public IList<Vector3> AStar(Vector3 startPosition, Vector3 endPosition)
        {
            Cell startCell = _grid.GetCell(startPosition);
            Cell endCell = _grid.GetCell(endPosition);

            if (startCell == null || endCell == null || startCell == endCell ||
                !startCell.IsWalkable || !endCell.IsWalkable)
            {
                // Early exit in case there is no valid path possible
                return null;
            }

            _frontier.Clear();
            _visited.Clear();
            _grid.ResetPathData();

            startCell.Parent = null;
            startCell.GCost = 0;
            startCell.HCost = GetEstimatedCost(startCell, endCell);

            _frontier.Enqueue(new FrontierEntry(startCell, startCell.FCost));

            while (_frontier.Count > 0)
            {
                FrontierEntry entry = _frontier.Dequeue();
                Cell current = entry.Cell;

                if (!_visited.Add(current))
                {
                    //Outdated entry. The cell was already processed through a cheaper route.
                    continue;
                }

                if (current == endCell)
                {
                    // Early exit.
                    // We have reached the end cell. No need to continue.
                    break;
                }

                IList<Cell> neighbours = _grid.GetNeighbours(current, PathfindingConfig.AllowDiagonalPathfinding);

                foreach (Cell neighbour in neighbours)
                {
                    if (!neighbour.IsWalkable || _visited.Contains(neighbour))
                    {
                        // Skip this neighbour if it's not walkable or if it has been already visited.
                        continue;
                    }

                    int costToNeighbour = _grid.GetCostToNeighbour(current, neighbour);

                    if (costToNeighbour <= 0)
                    {
                        // GetCostToNeighbour returns a negative value when the cells are not neighbours.
                        GD.PrintErr("Invalid cost to the neighbour! Did GetNeighbour return a Cell which is not a neighbour?");
                        continue;
                    }

                    int costSoFar = current.GCost + costToNeighbour;

                    if (costSoFar < neighbour.GCost) // Not reached yet (int.MaxValue) or a cheaper path was found.
                    {
                        neighbour.GCost = costSoFar;
                        neighbour.HCost = GetEstimatedCost(neighbour, endCell);
                        neighbour.Parent = current;

                        _frontier.Enqueue(new FrontierEntry(neighbour, neighbour.FCost));
                    }
                }
            }

            return RetracePath(startCell, endCell);
        }

#pragma warning disable CS0162

        private int GetEstimatedCost(Cell startCell, Cell endCell)
        {
            if (PathfindingConfig.AllowDiagonalPathfinding)
            {
                return GetEstimatedDiagonalDistance(startCell, endCell);
            }

            return GetEstimatedAxialDistance(startCell, endCell);
        }
#pragma warning restore CS0162

        private int GetEstimatedAxialDistance(Cell startCell, Cell endCell)
        {
            // Manhattan distance
            return (Mathf.Abs(startCell.X - endCell.X) + Mathf.Abs(startCell.Y - endCell.Y)) * 10;
        }

        private int GetEstimatedDiagonalDistance(Cell startCell, Cell endCell)
        {
            int xDistance = Mathf.Abs(startCell.X - endCell.X);
            int yDistance = Mathf.Abs(startCell.Y - endCell.Y);

            int diagonalSteps = Mathf.Min(xDistance, yDistance);
            int axialSteps = Mathf.Max(xDistance, yDistance) - diagonalSteps;

            return diagonalSteps * 14 + axialSteps * 10;
        }

        #endregion

        #region Common

        private IList<Vector3> RetracePath(Cell startCell, Cell endCell)
        {
            IList<Vector3> path = new List<Vector3>();

            Cell current = endCell;
            bool isValid = true;

            while (current != startCell && (isValid = current != null))
            {
                path.Add(current.WorldPosition);
                current = current.Parent;
            }

            if (!isValid)
            {
                // The path is invalid. The end cell was not reached
                return null;
            }

            path.Reverse();

            return path;
        }
        #endregion
    }
}
