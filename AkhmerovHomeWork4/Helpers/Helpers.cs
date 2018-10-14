namespace AkhmerovHomeWork4.Helpers
{
    using System;
    using static System.Console;

    /// <summary>
    /// Класс методов-помощников
    /// </summary>

    class Helpers
    {
        #region Операбельные переменные

        /// <summary>
        /// Высота шахматного поля
        /// </summary>
        private static int height;
        /// <summary>
        /// Ширина шахматного поля
        /// </summary>
        private static int width;
        /// <summary>
        /// Сообщение при невозможном решении задачи
        /// </summary>
        const string impossible = "Задачу невозможно решить.";
        /// <summary>
        /// Сообщение при успешном решении задачи
        /// </summary>
        const string finish = "Задача успешно завершена!";
        /// <summary>
        /// Время затраченное на решение задачи.
        /// </summary>
        private static TimeSpan timeSpent;
        /// <summary>
        /// Количество миллисекунд затраченных на решение задачи.
        /// </summary>
        private static double msSpent;
        /// <summary>
        /// Количество секунд затраченных на решение задачи.
        /// </summary>
        private static double secSpent;
        /// <summary>
        /// Количество минут затраченных на решение задачи.
        /// </summary>
        private static double minSpent;
        /// <summary>
        /// Текущий множитель округления миллисекунд.
        /// </summary>
        private static int currentMultiSec;
        /// <summary>
        /// Множитель необходимый для корректного округления миллисекунд при определенном значении секунд.
        /// </summary>
        private const int multiSec = 10;
        /// <summary>
        /// Количество секунд/минут в минуте/часе.
        /// </summary>
        const int secMinPerMinHour = 60;
        #endregion
        
        /// <summary>
        /// Прорисовка шахматного поля на основе полученного двумерного массива
        /// </summary>
        /// <param name="chessField">Двумерный массив (шахматное поле)</param>
        
        public static void DrawChessField(char[,] chessField)
        {
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
        /// <param name="_cells">Структура обозначений шахматного поля</param>
        /// <returns>Двумерный массив (шахматное поле)</returns>
        /// 
        public static void CreateChessField(ref char[,] chessField)
        {
            height = chessField.GetLength(0);
            width = chessField.GetLength(1);
            
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    chessField[i, j] = Cells.emptyCell;
                }
            }
        }

        /// <summary>
        /// Невозможность решить заданную задачу.
        /// </summary>

        public static void ImpossibleTask()
        {
            Clear();
            
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
        
        public static bool CheckFinish(char[,] field, string[] finishTurns, ref Statistics stats)
        {
            for (var i = 0; i < height; i++)
            {
                stats.allOperations++;
                for (var j = 0; j < width; j++)
                {
                    stats.allOperations++;
                    if (field[i, j] == Cells.emptyCell)
                    {
                        return false;
                    }
                }
            }

            TaskFinish(stats, finishTurns);
            return true;
        }

        /// <summary>
        /// Подсчет итогов успешного выполнения задачи
        /// </summary>
        /// <param name="stats">Структура статистики</param>
        /// <param name="finishTurns">Массив успешных ходов</param>

        private static void TaskFinish(Statistics stats, string[] finishTurns)
        {
            WriteLine("\n\n");

            CursorLeft = WindowWidth / 2 - finish.Length;
            ForegroundColor = ConsoleColor.Green;
            Write(finish);
            ResetColor();

            WriteLine("\n Выполненные ходы: \n");

            foreach (var turn in finishTurns)
            {
                WriteLine(turn);
            }

            timeSpent = DateTime.Now - stats.startTime;
            
            WriteLine($"\nЧистых операций (только постановка фигуры): {stats.turnOperations:N0}\nВсех операций: {stats.allOperations:N0}\n");
            CalculateSpentTime(stats.allOperations);
        }

        /// <summary>
        /// Подсчет затраченного времени и средней производительности.
        /// </summary>
        /// <param name="totalOperations">Всего выполненных операций</param>

        private static void CalculateSpentTime(long totalOperations)
        {
            msSpent = timeSpent.TotalMilliseconds;
            secSpent = timeSpent.TotalSeconds;
            minSpent = timeSpent.TotalMinutes;
            currentMultiSec = multiSec;

            if (secSpent < 1)
            {
                WriteLine($"Всего времени на задачу затрачено: {msSpent:F2}мс.\n");
                WriteLine($"Времени затрачено на 1 операцию примерно {msSpent / totalOperations:F2}мс");
            }
            else
            {
                if (secSpent < currentMultiSec)
                {
                    msSpent %= 1000;
                }
                else
                {
                    while (secSpent >= currentMultiSec && secSpent < currentMultiSec * multiSec)
                    {
                        currentMultiSec *= multiSec;
                    }

                    msSpent %= 1000 * currentMultiSec;
                }

                WriteLine(secSpent < secMinPerMinHour
                    ? $"Времени затрачено: {secSpent:N0}сек, {msSpent:F2}мс\n"
                    : $"Времени затрачено: {minSpent:N0}мин, {secSpent % secMinPerMinHour:N0}сек, {msSpent:F2}мс");
                Write($"Средняя производительность: {totalOperations / timeSpent.TotalSeconds:F2} операций в секунду.");
            }
        }
    }
}
