using System;
using System.Collections.Generic;
using System.Linq;

namespace Gold_Digger
{
    class Program
    {
        public static int allDiamonds = 0;
        public static int diamondsLeft = 0;
        public static int ourGuyDiamondsCounter = 0;
        public static int someGuyDiamondsCounter = 0;

        static void Main(string[] args)
        {
            string[,] field = CreateField();
            Console.Clear();

            field = FillField(field);
            Console.Clear();

            DrawField(field);
            string direction = Direction();
            Console.Clear();

            while (direction != "Esc" && diamondsLeft != 0)
            {
                MoveOurGuy(field, direction);
                MoveSomeGuy(field, direction);
                System.Threading.Thread.Sleep(100);
                Console.Clear();
                DrawField(field);
                direction = Direction();
            }

            Console.Clear();

            int ourGuyDiamondProcent = ourGuyDiamondsCounter * 100 / allDiamonds;
            int someGuyDiamondProcent = someGuyDiamondsCounter * 100 / allDiamonds;
            Console.WriteLine($" All Diamonds: {allDiamonds}.");
            Console.WriteLine($"OurGuy diamonds procent: {ourGuyDiamondProcent}%.");
            Console.WriteLine($"SomeGuy diamonds procent: {someGuyDiamondProcent}%.");
            Console.WriteLine("Game over!");
        }

        public static int[] GetFieldBounds(int maxX, int maxY)
        {
            Console.WriteLine("Define bordaries for the field:");

            int rows = int.Parse(Console.ReadLine());
            int cols = int.Parse(Console.ReadLine());

            var fieldBounds = new int[2];

            if (!(rows >= 10 && rows <= maxX && cols >= 10 && cols <= maxY))
            {
                Console.WriteLine("Invalid field size!");
            }
            else
            {
                fieldBounds[0] = rows;
                fieldBounds[1] = cols;
            }

            return fieldBounds;
        }


        static string[,] CreateField()
        {
            int[] fieldBounds = GetFieldBounds(50, 50);

            int rows = fieldBounds[0];
            int cols = fieldBounds[1];

            var field = new string[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    field[i, j] = "#";
                }
            }

            Random rnd = new Random();

            int ourGuyRow = rnd.Next(0, rows);
            int ourGuyCol = rnd.Next(0, cols);

            field[ourGuyRow, ourGuyCol] = "O";


            return field;
        }

        public static string[,] FillField(string[,] field)
        {
            int rows = field.GetLength(0);
            int cols = field.GetLength(1);

            int fieldCellsCount = rows * cols;

            int ground = (int)(0.4 * fieldCellsCount);
            int grass = (int)(0.3 * fieldCellsCount);
            int trees = (int)(0.2 * fieldCellsCount);
            int stones = (int)(0.1 * fieldCellsCount);
            int diamonds = (int)(0.1 * fieldCellsCount);
            int someGuys = (int)(0.05 * fieldCellsCount);
            allDiamonds = diamonds;
            diamondsLeft = diamonds;

            Dictionary<string, int> elementsCount = new Dictionary<string, int>
            {
                ["G"] = ground,
                ["Gr"] = grass,
                ["T"] = trees,
                ["S"] = stones,
                ["D"] = diamonds,
                ["SG"] = someGuys
            };

            List<string> elements = new List<string>
            {
                "G",
                "Gr",
                "T",
                "S",
                "D",
                "SG"
            };

            Random random = new Random();

            int currentElementIndex = random.Next(elements.Count);
            string currentElement = elements[currentElementIndex];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (elementsCount[currentElement] != 0 && field[row, col] != "O")
                    {
                        field[row, col] = currentElement;
                        elementsCount[currentElement]--;
                    }

                    currentElementIndex = random.Next(elements.Count);
                    currentElement = elements[currentElementIndex];
                }
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (field[row, col] == "#")
                    {
                        field[row, col] = "Gr";
                    }
                }
            }

            return field;
        }

        public static void DrawField(string[,] field)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    string currentCell = field[row, col];

                    switch (currentCell)
                    {
                        case "G":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write('\u2592');
                                Console.ResetColor();
                            }

                            break;
                        case "Gr":
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write('\u2593');
                                Console.ResetColor();
                            }

                            break;
                        case "T":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write('\u2663');
                                Console.ResetColor();
                            }

                            break;
                        case "S":
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write('\u00A9');
                                Console.ResetColor();
                            }

                            break;
                        case "D":
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write('\u2666');
                                Console.ResetColor();
                            }

                            break;
                        case "SG":
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write('\u263b');
                                Console.ResetColor();
                            }

                            break;
                        case "O":
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write('\u263a');
                                Console.ResetColor();
                            }

                            break;
                    }
                }

                Console.WriteLine();
            }
        }
        public static bool isInside(string[,] board, int desiredRow, int desiredCol)
        {
            return desiredRow < board.GetLength(0)
                   && desiredRow >= 0
                   && desiredCol < board.GetLength(1)
                   && desiredCol >= 0;
        }

        public static string Direction()
        {
            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    return "Up";
                case ConsoleKey.DownArrow:
                    return "Down";
                case ConsoleKey.LeftArrow:
                    return "Left";
                case ConsoleKey.RightArrow:
                    return "Right";
                case ConsoleKey.Escape:
                    return "Esc";
            }

            return null;
        }

        public static void MoveGuy(string[,] field, int currentRow, int currentCol, string direction, string guy)
        {
            switch (direction)
            {
                case "Up":
                    if (currentRow - 1 >= 0 && CanMove(field, currentRow - 1, currentCol))
                    {
                        DiamondCounter(field, currentRow - 1, currentCol, guy);
                        field[currentRow - 1, currentCol] = guy;
                        field[currentRow, currentCol] = "G";
                    }
                    return;
                case "Down":
                    if (currentRow + 1 < field.GetLength(0) && CanMove(field, currentRow + 1, currentCol))
                    {
                        DiamondCounter(field, currentRow + 1, currentCol, guy);
                        field[currentRow + 1, currentCol] = guy;
                        field[currentRow, currentCol] = "G";
                    }
                    return;
                case "Left":
                    if (currentCol - 1 >= 0  && CanMove(field, currentRow, currentCol - 1))
                    {
                        DiamondCounter(field, currentRow, currentCol - 1, guy);
                        field[currentRow, currentCol - 1] = guy;
                        field[currentRow, currentCol] = "G";
                    }
                    return;
                case "Right":
                    if (currentCol + 1 < field.GetLength(1) && CanMove(field, currentRow, currentCol + 1))
                    {
                        DiamondCounter(field, currentRow, currentCol + 1, guy);
                        field[currentRow, currentCol + 1] = guy;
                        field[currentRow, currentCol] = "G";
                    }
                    return;
            }
        }

        public static Tuple<int, Queue<int>> Search(string[,] field, string element)
        {
            Queue<int> coordinates = new Queue<int>();
            int elementCount = 0;

            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    if (field[row, col] == element)
                    {
                        coordinates.Enqueue(row);
                        coordinates.Enqueue(col);
                        elementCount++;
                    }
                }
            }

            return Tuple.Create(elementCount, coordinates);
        }

        public static void MoveOurGuy(string[,] field, string direction)
        {
            var tuple = Search(field, "O");
            Queue<int> coordinates = tuple.Item2;
            int row = coordinates.Dequeue();
            int col = coordinates.Dequeue();
            MoveGuy(field, row, col, direction, "O");
        }

        public static void MoveSomeGuy(string[,] field, string direction)
        {
            var elementCoordinates = Search(field, "SG");
            var coordinates = elementCoordinates.Item2;
            int someGuysCount = elementCoordinates.Item1;

            for (int i = 0; i < someGuysCount; i++)
            {
                int row = coordinates.Dequeue();
                int col = coordinates.Dequeue();
                MoveGuy(field, row, col, direction, "SG");
            }
        }

        public static bool CanMove(string[,] field, int row, int col)
        {
            if (field[row, col] == "G" || field[row, col] == "Gr" || field[row, col] == "D")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DiamondCounter(string[,] field, int row, int col, string guy)
        {
            if (field[row, col] == "D")
            {
                if (guy == "O")
                {
                    ourGuyDiamondsCounter++;
                    diamondsLeft--;
                }
                else if (guy == "SG")
                {
                    someGuyDiamondsCounter++;
                    diamondsLeft--;
                }
            }
        }
    }

}
