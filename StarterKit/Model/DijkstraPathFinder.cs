namespace Model
{
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
        public PathFinderType algType { get => _algType; set {} }
        
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            //ToDo implement this method
            int[,] distance = new int[maze.MazeMDArray.GetLength(0),maze.MazeMDArray.GetLength(1)];
            for(int i = 0; i < distance.GetLength(0); i++)
            {
                for(int j = 0; j < distance.GetLength(1); j++)
                {
                    distance[i,j] = int.MaxValue;   
                }
            }

            distance[pos[0], pos[1]] = 0;
            Dictionary<(int, int), (int, int)> Parents = new();
            List<int[]> ToVisitPositions = [];
            ToVisitPositions.Add(pos);

            while(ToVisitPositions.Count > 0)
            {
                int[] currentPos = { -1, -1};
                int smallestDistance = int.MaxValue;

                foreach(int[] position in ToVisitPositions)
                {
                    if(distance[position[0], position[1]] < smallestDistance)
                    {
                        currentPos = position;
                        smallestDistance = distance[position[0], position[1]]; 
                    }
                }
                ToVisitPositions.Remove(currentPos);

                if(currentPos[0] == maze.End[0] && currentPos[1] == maze.End[1]) break;

                foreach(int[] move in maze.moves)
                {
                    int newRow = currentPos[0] + move[0];
                    int newCol = currentPos[1] + move[1];

                    int[] newPos = { newRow, newCol };
                    if(!maze.IsValidMove(newRow, newCol)) continue;

                    int newDistance = distance[currentPos[0], currentPos[1]] + 1;

                    if(newDistance < distance[newRow, newCol]) //kijken of de route naar die positie korter is of niet
                    {
                        distance[newRow, newCol] = newDistance;
                        Parents[(newRow, newCol)] = (currentPos[0], currentPos[1]); //child/neighbour is de key, parent is de value, want je werkt van eind naar begin  
                        ToVisitPositions.Add(newPos);
                    }
                }
            }

            List<int[]> route = [];
            int[] current = maze.End;
            while(Parents.ContainsKey((current[0], current[1]))) //de startlocatie is een parent en geen child en heeft dus geen key en dan stopt de loop
            {
                route.Add(current);
                (int row, int col) parent = Parents[(current[0], current[1])];
                current = new int[] { parent.row, parent.col };
            }
            
            route.Add(current);
            route.Reverse();
            foreach(int[] place in route)
            {
                visitedPositions.Enqueue(place);
            }
        }
   }
}
