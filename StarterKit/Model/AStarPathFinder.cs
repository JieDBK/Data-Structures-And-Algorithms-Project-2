
using System.Reflection.Emit;

namespace Model
{
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
        public PathFinderType algType { get => _algType; set {} }

        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
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
                        smallestDistance = distance[position[0], position[1]] + Heuristic(maze, currentPos); 
                    }
                }
                ToVisitPositions.Remove(currentPos);

                if(currentPos == maze.End) break;

                foreach(int[] move in maze.moves)
                {
                    int newRow = currentPos[0] + move[0];
                    int newCol = currentPos[1] + move[1];

                    int[] newPos = { newRow, newCol };
                    if(!maze.IsValidMove(newRow, newCol)) continue;

                    int newDistance = distance[currentPos[0], currentPos[1]] + 1 + Heuristic(maze, newPos);

                    if(newDistance < distance[newRow, newCol])
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
            
            route.Reverse();
            foreach(int[] place in route)
            {
                visitedPositions.Enqueue(place);
            }
        }

        public int Heuristic(Maze maze, int[] fromPos)
        {
            return Math.Abs(fromPos[0] - maze.End[0]) + Math.Abs(fromPos[1] - maze.End[1]);
        }
    }
}

            

