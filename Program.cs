using System;
using System.Collections.Generic;
using System.Threading;

namespace ConRan
{
    internal class Program
    {
        static int width = 40; // Chiều rộng màn hình
        static int height = 20; // Chiều cao màn hình
        static int score = 0;
        static List<(int x, int y)> snake = new List<(int x, int y)>();
        static (int x, int y) direction = (0, 1); // Hướng ban đầu: đi xuống
        static (int x, int y) food;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            InitializeGame();

            while (true)
            {
                DrawScreen();
                Update();
                Thread.Sleep(100); // Điều chỉnh tốc độ di chuyển (ms)
            }
        }

        static void InitializeGame()
        {
            // Vị trí ban đầu của con rắn (ở giữa màn hình)
            snake.Clear();
            snake.Add((width / 2, height / 2));
            score = 0;

            // Sinh thức ăn
            GenerateFood();
        }

        static void GenerateFood()
        {
            Random random = new Random();
            do
            {
                food = (random.Next(1, width - 1), random.Next(1, height - 1));
            } while (snake.Contains(food)); // Bảo đảm thức ăn không xuất hiện trên thân rắn
        }

        static void DrawScreen()
        {
            Console.Clear();

            // Vẽ viền
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    if (x == 0 || x == width || y == 0 || y == height)
                        Console.Write("#"); // Viền
                    else if (snake.Contains((x, y)))
                        Console.Write("O"); // Thân rắn
                    else if ((x, y) == food)
                        Console.Write("*"); // Thức ăn
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }

            // Hiển thị điểm
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine($"Score: {score}");
        }

        static void Update()
        {
            HandleInput();

            // Tính vị trí mới của đầu rắn
            var newHead = (snake[0].x + direction.x, snake[0].y + direction.y);

            // Kiểm tra va chạm với tường hoặc thân rắn
            if (newHead.Item1 <= 0 || newHead.Item1 >= width || newHead.Item2 <= 0 || newHead.Item2 >= height || snake.Contains(newHead))
            {
                GameOver();
                return;
            }

            // Thêm đầu mới vào rắn
            snake.Insert(0, newHead);

            // Kiểm tra nếu ăn thức ăn
            if (newHead == food)
            {
                score++;
                GenerateFood();
            }
            else
            {
                // Xóa đuôi nếu không ăn thức ăn
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void HandleInput()
        {
            if (!Console.KeyAvailable) return;

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (direction != (0, 1)) direction = (0, -1); // Lên
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != (0, -1)) direction = (0, 1); // Xuống
                    break;
                case ConsoleKey.LeftArrow:
                    if (direction != (1, 0)) direction = (-1, 0); // Trái
                    break;
                case ConsoleKey.RightArrow:
                    if (direction != (-1, 0)) direction = (1, 0); // Phải
                    break;
            }
        }

        static void GameOver()
        {
            Console.Clear();
            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.WriteLine("Game Over!");
            Console.SetCursorPosition(width / 2 - 7, height / 2 + 1);
            Console.WriteLine($"Your Score: {score}");
            Console.SetCursorPosition(width / 2 - 9, height / 2 + 2);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
