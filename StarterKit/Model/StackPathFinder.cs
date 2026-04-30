
using System.Threading.Tasks.Dataflow;

namespace Model
{
    public class StackPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Stack;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            Random rng = new();
            //ToDo implement this method
            if (maze.MazeArray == null || maze.MazeArray.Length == 0 || maze.MazeArray[0].Length == 0 ||
                pos == null || pos.Length != 2 ||
                !maze.IsValidMove(pos[0], pos[1]) ||
                visitedPositions.Any(_ => _[0] == pos[0] && _[1] == pos[1]) ||
                visitedPositions.Any(_ => _[0] == maze.End[0] && _[1] == maze.End[1])
            )
            {
                return;
            }

            int[][] moves = {           
                        new int[] {  1,  0 },  //down
                        new int[] { -1,  0 },  //up
                        new int[] {  0, -1 },  //left
                        new int[] {  0,  1 },  //right
                    };


            Stack<int[]> backtrack = new();
            backtrack.Push(pos);
            while (backtrack.Count > 0)
            {
                int[] currentCell = backtrack.Pop();
                visitedPositions.Enqueue(currentCell);
                int currentRow = currentCell[0];
                int currentCol = currentCell[1];
                rng.Shuffle(moves);
                foreach (int[] step in moves)
                {
                    int newRow = currentRow + step[0];
                    int newCol = currentCol + step[1];
                    if (maze.IsValidMove(newRow, newCol) && !visitedPositions.Any(_ => _[0] == newRow && _[1] == newCol))
                    {
                        backtrack.Push(currentCell);
                        backtrack.Push([newRow, newCol]);
                        visitedPositions.Enqueue([newRow, newCol]);
                        break;
                    }
                }

                if (visitedPositions.Any(_ => _[0] == maze.End[0] && _[1] == maze.End[1]))
                {
                    return;
                }
            }               

        }       
    }
}

            

