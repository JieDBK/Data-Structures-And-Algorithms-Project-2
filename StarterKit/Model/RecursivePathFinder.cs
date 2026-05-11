
using System.Drawing;

namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        Random rng = new();
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set { } }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            //ToDo implement this method
            bool[,] visited = new bool[maze.MazeMDArray.GetLength(0), maze.MazeMDArray.GetLength(1)];
            if (RecFindPath(maze, pos, visitedPositions, visited)) return;
        }

        public bool RecFindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions, bool[,] visited)
        {
            if (pos == null || pos.Length != 2) return false;
            if (visited[pos[0], pos[1]]) return false;
            if (pos[0] == maze.End[0] && pos[1] == maze.End[1])
            {
                visitedPositions.Enqueue(pos);
                return true;
            }

            visited[pos[0], pos[1]] = true; //mark pos visited
            visitedPositions.Enqueue(pos);

            int[][] localMoves = (int[][])maze.moves.Clone();
            rng.Shuffle(localMoves);

            foreach (int[] move in localMoves)
            {
                int newRow = pos[0] + move[0];
                int newCol = pos[1] + move[1];

                int[] newPos = { newRow, newCol };

                if (!maze.IsValidMove(newRow, newCol)) continue;
                if (RecFindPath(maze, newPos, visitedPositions, visited)) return true;
            }

            visited[pos[0], pos[1]] = false;
            return false;
        }
    }
}
