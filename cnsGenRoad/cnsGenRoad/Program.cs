// Program.cs
using System;
using cnsGenRoad;

namespace cnsGenRoad
{
    class Application
    {
        static void Main()
        {
            var map = new RoadMap(20, 10);
            var isRunning = true;

            while (isRunning)
            {
                DisplayMenu();
                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.Q:
                        isRunning = false;
                        break;
                    case ConsoleKey.L:
                        HandleLineCreation(map);
                        break;
                    case ConsoleKey.R:
                        HandleRectangleCreation(map);
                        break;
                    case ConsoleKey.P:
                        map.Display();
                        break;
                    case ConsoleKey.F1:
                        HandleFileSave(map);
                        break;
                    case ConsoleKey.F2:
                        HandleFileLoad(map);
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\nRoad Map Creator");
            Console.WriteLine("L - Draw line");
            Console.WriteLine("R - Draw rectangle");
            Console.WriteLine("P - Print map");
            Console.WriteLine("F1 - Save map");
            Console.WriteLine("F2 - Load map");
            Console.WriteLine("Q - Quit");
            Console.Write("Select option: ");
        }

        static void HandleLineCreation(RoadMap map)
        {
            try
            {
                Console.Write("Enter start X,Y and end X,Y: ");
                var coords = Console.ReadLine().Split();
                if (coords.Length == 4 &&
                    int.TryParse(coords[0], out int x1) &&
                    int.TryParse(coords[1], out int y1) &&
                    int.TryParse(coords[2], out int x2) &&
                    int.TryParse(coords[3], out int y2))
                {
                    if (x1 == x2)
                    {
                        map.CreateVerticalLine(Math.Min(y1, y2), Math.Max(y1, y2), x1);
                    }
                    else if (y1 == y2)
                    {
                        map.CreateHorizontalLine(Math.Min(x1, x2), Math.Max(x1, x2), y1);
                    }
                    else
                    {
                        Console.WriteLine("Only straight lines are supported");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid coordinates format. Please enter four numbers separated by spaces.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void HandleRectangleCreation(RoadMap map)
        {
            try
            {
                Console.Write("Enter top-left and bottom-right coordinates (x1 y1 x2 y2): ");
                var coords = Console.ReadLine().Split();
                if (coords.Length == 4 &&
                    int.TryParse(coords[0], out int x1) &&
                    int.TryParse(coords[1], out int y1) &&
                    int.TryParse(coords[2], out int x2) &&
                    int.TryParse(coords[3], out int y2))
                {
                    map.CreateRectangle(x1, y1, x2, y2);
                }
                else
                {
                    Console.WriteLine("Invalid coordinates format. Please enter four numbers separated by spaces.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void HandleFileSave(RoadMap map)
        {
            try
            {
                Console.Write("Enter filename: ");
                var filename = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    map.ExportToFile(filename);
                    Console.WriteLine("Map saved successfully");
                }
                else
                {
                    Console.WriteLine("Filename cannot be empty");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid filename: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to save file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void HandleFileLoad(RoadMap map)
        {
            try
            {
                Console.Write("Enter filename: ");
                var filename = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(filename))
                {
                    map.ImportFromFile(filename);
                    Console.WriteLine("Map loaded successfully");
                }
                else
                {
                    Console.WriteLine("Filename cannot be empty");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid filename: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to load file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}