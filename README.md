# Data-Structures-And-Algorithms-Project-2
Mohamed: 
- Recursive Path finder
- Visualization

Duy:
- Stack Path finder
- A* Algorithm
- Maze generation BFS and DFS

Hidde:
- Dijkstra Path finder
- Maze generation BT
- Extra path


The jagged array is used for methods such as IsValidPos and IsValidMove because indexing a jagged array (array[row][col]) is faster than indexing a multidimensional array (array[row, col]). With a multidimensional array, the memory address is internally calculated using row × width + col, whereas a jagged array retrieves the element directly. The multidimensional array is used to ensure that the maze maintains a fixed rectangular structure.
