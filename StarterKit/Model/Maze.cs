
using System.Data;

namespace Model
{
    public class Maze
    {
        Random rng = new();
        public int[][] MazeArray { get; private set; }
        public int[,] MazeMDArray { get; private set; }
        public int[] Begin { get; private set; }
        public int[] End { get; private set; }

        public readonly int[][] moves = {           
            new int[] {  1,  0 },  //down
            new int[] { -1,  0 },  //up
            new int[] {  0, -1 },  //left
            new int[] {  0,  1 },  //right
        };
        
        private int[][] movesGen = {           
            new int[] {  1,  0 },  //down
            new int[] { -1,  0 },  //up
            new int[] {  0, -1 },  //left
            new int[] {  0,  1 },  //right
        }; //for generating maze and im gonna randomize the list
        public Maze() => GenerateMaze();
        public Maze(bool automatic = true) {if(automatic) GenerateMaze(); else GenerateFromText(MazeGrids.mazeText);}
        public Maze(int rows, int cols) {if(rows <= 0 && cols <= 0) GenerateFromText(MazeGrids.mazeText); else GenerateMaze(rows, cols);}
        public Maze(string lines) => GenerateFromText(lines);

        void GenerateFromText(string lines){
            MazeArray = ToMazeArray(lines);
            MazeMDArray = ToMazeMDArray(lines);
        }
        
        void GenerateMaze(int rows = 30, int cols = 40)
        {
            if(rows < 4 || cols < 4) {rows = 20; cols = 40;}
            if(rows % 2 != 0) {rows++;}
            if(cols % 2 != 0) {cols++;}

            //ToDo...

            // GenerateFromText(MazeGrids.mazeText); //remove this line and implement the task
            int[][] jaggedMaze = new int[rows][];
            for (int i = 0; i < jaggedMaze.Length; i++)
            {
                jaggedMaze[i] = new int[cols];
                for (int j = 0; j < jaggedMaze[i].Length; j++)
                {
                    jaggedMaze[i][j] = -1;
                }
            }

            int[,] mdMaze = new int[rows,cols];
            for (int i = 0; i < mdMaze.GetLength(0); i++)
            {
                for (int j = 0; j < mdMaze.GetLength(1); j++)
                {
                    mdMaze[i,j] = -1;
                }
            }

            // int randomRow;
            // int randomCol;  
            // while (true)
            // {
            //     randomRow = rng.Next(1, rows);
            //     randomCol = rng.Next(1, cols);
            //     if (IsValidPos(jaggedMaze, randomRow, randomCol) && jaggedMaze[randomRow][randomCol] == -1)
            //     {
            //         break;
            //     }
            // }

            Stack<int[]> backtrack = new();
            int row;
            int col;
            while (true)
            {
                row = rng.Next(mdMaze.GetLength(0) - 1);
                col = rng.Next(mdMaze.GetLength(1) - 1);
                if (row % 2 == 0)
                {
                    row = row + 1;
                }

                if (col % 2 == 0)
                {
                    col = col + 1;
                }
                if (IsValidPos(jaggedMaze, row, col))
                {
                    jaggedMaze[row][col] = 0;
                    mdMaze[row, col] = 0;
                    break;
                }
            }
            backtrack.Push([row, col]);
            while (backtrack.Count > 0)
            {
                int[] currentCell = backtrack.Pop();
                int currentRow = currentCell[0];
                int currentCol = currentCell[1];
                rng.Shuffle(movesGen);
                foreach (int[] step in movesGen)
                {
                    int newRow = currentRow + (step[0] * 2); //step of 2
                    int newCol = currentCol + (step[1] * 2);
                    int betweenRow = currentRow + step[0];
                    int betweenCol = currentCol + step[1];
                    // if (step[0] == -1)
                    if (IsValidPos(jaggedMaze, newRow, newCol) && jaggedMaze[newRow][newCol] == -1 ) // && IsValidPos(jaggedMaze, newRow + step[0], newCol + step[1]) && jaggedMaze[newRow + step[0]][newCol + step[1]] == -1
                    {
                        backtrack.Push(currentCell);
                        jaggedMaze[newRow][newCol] = 0;
                        mdMaze[newRow, newCol] = 0;
                        jaggedMaze[betweenRow][betweenCol] = 0;
                        mdMaze[betweenRow, betweenCol] = 0;
                        backtrack.Push([newRow, newCol]);
                        break;
                    }
                }
            }

            for (int i = 1; i < cols; i++)
            {
                if (jaggedMaze[1][i] == 0)
                {
                    jaggedMaze[1][i] = 1;
                    mdMaze[1,i] = 1;
                    Begin = [1,i];
                    break;
                }
            }


            for (int i = 1; i < cols; i++)
            {
                if (jaggedMaze[rows - i][cols - i] == 0)
                {
                    jaggedMaze[rows - i][cols - i] = 2;
                    mdMaze[rows -i, cols - i] = 2;
                    End = [rows -i, cols - i];
                    break;
                }
            }
            // jaggedMaze[1][1] = 1; //start linksboven
            // jaggedMaze[rows - 1][cols - 1] = 2; //einde rechtsonder
            // mdMaze[1,1] = 1;
            // mdMaze[rows -2, cols - 2] = 2;

            MazeArray = jaggedMaze;
            MazeMDArray = mdMaze;
            
            // Begin = [1,1];
            // End = [rows -2, cols - 2];
            
        }

        int[][] ToMazeArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            int[][] outArray = new int[arrayLines.Length][];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                // row array:
                var row = new int[line.Length];
                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                            row[colIdx] = -1;  //walls
                            break;
                        case '1':
                            row[colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            row[colIdx] = 2;   //end 
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            row[colIdx] = 0;   //not visited
                            break;
                    }
                }
                // row in the output jagged array.
                outArray[rowIdx] = row;
            }

            return outArray;
            
        }

        int[,] ToMazeMDArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            var lineLength = 0;
            if (arrayLines != null && arrayLines.Length > 0)
                lineLength = arrayLines[0].Length;
            else
            throw new Exception($"Maze incorrect");
            
            for (var rowIdx = 0; arrayLines != null && rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                if (arrayLines[rowIdx] == null || line.Length != lineLength)
                    throw new Exception($"Not same line length for rows in maze:\n at row 0: {lineLength}, at row {rowIdx}: {line.Length}");
            }
            
            int[,] outArray = new int[arrayLines.Length, lineLength];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];

                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                            outArray[rowIdx, colIdx] = -1;  //walls
                            break;
                        case '1':
                            outArray[rowIdx, colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            outArray[rowIdx, colIdx] = 2;   //end 
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            outArray[rowIdx, colIdx] = 0;   //not visited
                            break;
                    }
                }
            }
            return outArray;
        }

        static int CountNotVisited(int[][] maze)
        {
            int cnt = 0;
            if (maze != null && maze.Length > 0)
            {
                for (int rowIdx = 0; rowIdx < maze.Length; rowIdx++)
                {
                    for (int colIdx = 0; maze[rowIdx] != null && colIdx < maze[rowIdx].Length; colIdx++)
                    {
                        cnt = maze[rowIdx][colIdx] == 0 ? cnt + 1 : cnt;
                    }
                }
            }
            return cnt;
        }

        public int CountNotVisited() => CountNotVisited(MazeArray);

        static bool IsValidPos(int[][] array, int newRow, int newColumn)
        {
            // ... Ensure position is within the array bounds.
            /*
            if (newRow < 0) return false;
            if (newColumn < 0) return false;
            if (newRow >= array.Length) return false;
            if (newColumn >= array[newRow].Length) return false;
            return true;
            */
            return !(newRow < 0)
                    && !(newColumn < 0)
                    && !(newRow >= array.Length)
                    && !(newColumn >= array[newRow].Length);
        }
        
        // Make sure the position is within the maze array bounds.
        // no walls
        public bool IsValidMove(int newRow, int newColumn) => 
            IsValidPos(MazeArray, newRow, newColumn) &&
            !(MazeArray[newRow][newColumn] == -1); //no walls 

        //Marking strategy
        public bool IsValidMove(int newRow, int newColumn, bool notVisited = true)
        {
            // Make sure the position is within the maze array bounds.
            // no walls, not yet visited ? (flag notVisited: false)
            return notVisited ?
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1)  //no walls, but already visited -> ok
                    :
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1 || MazeArray[newRow][newColumn] == 4); //no walls, not yet visited 
        }
        
    }

    public static class MazeGrids
    {
      public static string mazeText = @"
xxxxxx1xxxxxxxxxxxxxxxxxxxxxxx.
 x   x   x                    .
xx2x xxx   x xxxxxxxx    x xx .
x  x xxxxxxx xxxxxxxxxxxxx xxx.
 x x xx      x                .
x  x xx xxxxx  x xxxx xxxxx  x.
xx    x xxx   xx xxx  xxx   xx.
xxx   xxx   x xxxx   xx   x xx.
xx     xx   x xxxx   xx   x xx.
xxxx    xxxxx xx xxxx xxxxx xx.
xx            xx            xx.";
    }
}
