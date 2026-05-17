namespace Model
{
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
        public PathFinderType algType { get => _algType; set {} }
        
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            //ToDo implement this method
            int[,] distance = new int[maze.MazeArray.GetLength(0),maze.MazeArray.GetLength(1)];
            for(int i = 0; i < distance.GetLength(0); i++)
            {
                for(int j = 0; j < distance.GetLength(1); j++)
                {
                    distance[i,j] = int.MaxValue;   
                }
            }
        }
   }
}
