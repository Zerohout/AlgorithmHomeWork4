namespace AkhmerovHomeWork4.Helpers
{
    using System;
    using static System.Console;

    class Helpers
    {

        /// <summary>
        /// Прорисовка шахматного поля на основе полученного двумерного массива
        /// </summary>
        /// <param name="chessField">Двумерный массив (шахматное поле)</param>

        public static void DrawChessField(char[,] chessField)
        {
            var height = chessField.GetLength(0);
            var width = chessField.GetLength(1);
            Clear();
            WriteLine();
            for (var i = 0; i < chessField.GetLength(0); i++)
            {
                CursorTop = (WindowHeight / 2 - height) + i * 2;
                CursorLeft = WindowWidth / 2 - (int)(width * 1.5);

                for (var j = 0; j < chessField.GetLength(1); j++)
                {
                    Write($" {chessField[i, j]} ");
                }
            }
        }

        /// <summary>
        /// Создание шахматного поля на основе полученного двумерного массива
        /// </summary>
        /// <param name="chessField">Двумерный массив с размером шахматного поля</param>
        /// <param name="emptyCell">Символ пустой клетки</param>
        /// <returns>Двумерный массив (шахматное поле)</returns>
        public static void CreateChessField(ref char[,] chessField, char emptyCell)
        {
            for (var i = 0; i < chessField.GetLength(0); i++)
            {
                for (var j = 0; j < chessField.GetLength(1); j++)
                {
                    chessField[i, j] = emptyCell;
                }
            }
        }

        /// <summary>
        /// Невозможность решить заданную задачу.
        /// </summary>

        public static void ImpossibleTask()
        {
            Clear();
            const string impossible = "Задачу невозможно решить.";
            CursorTop = WindowHeight / 2;
            CursorLeft = WindowWidth / 2 - impossible.Length;
            ForegroundColor = ConsoleColor.Red;
            Write(impossible);
            ResetColor();
        }

        /// <summary>
        /// Проверка успешности решения задачи
        /// </summary>
        /// <param name="field">Двумерный массив (Шахматное поле)</param>
        /// <param name="finishTurns">Массив с успешными ходами</param>
        /// <param name="stats">Структура статистики</param>
        /// <param name="cells">Структура обозначений</param>

        public static void CheckFinish(char[,] field, string[] finishTurns, Statistics stats, Cells cells)
        {
            var height = field.GetLength(0);
            var width = field.GetLength(1);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (field[i, j] == cells.emptyCell)
                    {
                        return;
                    }
                }
            }

            TaskFinish(stats, finishTurns);
        }

        /// <summary>
        /// Задача выполнена
        /// </summary>
        /// <param name="stats">Структура статистики</param>
        /// <param name="finishTurns">Массив успешных ходов</param>

        private static void TaskFinish(Statistics stats, string[] finishTurns)
        {
            stats.allOperations += stats.clearOperations;

            ForegroundColor = ConsoleColor.Green;
            const string finish = "Задача успешно завершена!";
            WriteLine("\n\n");
            CursorLeft = WindowWidth / 2 - finish.Length;
            Write(finish);
            ResetColor();

            WriteLine("\n Выполненные ходы: \n");

            foreach (var turn in finishTurns)
            {
                WriteLine(turn);
            }

            Write($"\nЧистых операций (без учета количества отмен ходов): {stats.clearOperations:N0}\nВсех операций: {stats.allOperations:N0}");
        }
    }
}
