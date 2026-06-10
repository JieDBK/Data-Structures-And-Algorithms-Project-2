
using System.Drawing;

namespace Model
{
    public class RecursivePathFinder : IPathFinder
    {
        readonly Random rng = new();
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set { } }
        public List<int[]> correctPath { get; set; } = [];

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            //ToDo implement this method
            correctPath.Clear(); //incase algorithm runs multiple times
            bool[,] visited = new bool[maze.MazeMDArray.GetLength(0), maze.MazeMDArray.GetLength(1)];

            if (RecFindPath(maze, pos, visitedPositions, visited, correctPath))
            {
                correctPath.Reverse();
                return;
            }
        }

        public bool RecFindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions, bool[,] visited, List<int[]> correctPath)
        {
            if (pos == null || pos.Length != 2) return false;
            if (visited[pos[0], pos[1]]) return false;
            if (pos[0] == maze.End[0] && pos[1] == maze.End[1])
            {
                visitedPositions.Enqueue(pos);
                correctPath.Add(pos);
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
                if (RecFindPath(maze, newPos, visitedPositions, visited, correctPath))
                {
                    correctPath.Add(pos);
                    return true;
                }
            }

            visited[pos[0], pos[1]] = false;
            return false;
        }
    }
}
