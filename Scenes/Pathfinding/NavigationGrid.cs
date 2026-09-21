using Godot;
using System;

namespace GA.Ships.Pathfinding
{
    public partial class NavigationGrid : Node3D
    {
        /// <summary>
        /// The size of the grid in the game world units (meters).
        /// </summary>
        [Export] public Vector2I GridSize { get; set; } = new Vector2I(10, 10);

        /// <summary>
        /// The cell size. Cell is always a square
        /// </summary>
        [Export] public int CellSize { get; set; } = 1;

        [Export] public bool DrawDebugGrid { get; set; } = false;

        /// <summary>
        /// Cell count horizontally.
        /// </summary>
        public int Width => GridSize.X / CellSize;

        /// <summary>
        /// Cell count vertically.
        /// </summary>
        public int Height => GridSize.Y / CellSize;

        private Cell[,] _cells;

        public override void _Ready()
        {
            // Initialize the grid.
            BuildGraph();
        }

        private void BuildGraph()
        {
            if (CellSize <= 0)
            {
                throw new InvalidOperationException($"{nameof(CellSize)} must be greater than 0.");
            }

            if (GridSize.X <= 0 || GridSize.Y <= 0)
            {
                throw new InvalidOperationException($"{nameof(GridSize)} must be positive.");
            }

            _cells = new Cell[Width, Height];

            // The half width of the whole grid.
            float halfWidth = Width * CellSize * 0.5f;

            // The half height of the whole grid.
            float halfHeigth = Height * CellSize * 0.5f;

            Vector3 origin = GlobalPosition;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float worldX = origin.X + ((x + 0.5f) * CellSize) - halfWidth;
                    float worldY = origin.Z + ((y + 0.5f) * CellSize) - halfHeigth;

                    // TODO: Calculate correct cost!
                    _cells[x, y] = new Cell(x, y, new Vector3(worldX, origin.Y, worldY), 1);
                }
            }
        }

        /// <summary>
        /// Represents one cell in the grid graph.
        /// </summary>
        public class Cell : IComparable<Cell>
        {
            public int X { get; }
            public int Y { get; }
            public Vector3 WorldPosition { get; }
            public int Cost { get; set; }

            public Cell(int x, int y, Vector3 worldPosition, int cost)
            {
                X = x;
                Y = y;
                WorldPosition = worldPosition;
                Cost = cost;
            }

            public int CompareTo(Cell other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
