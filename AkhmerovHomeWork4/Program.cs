namespace AkhmerovHomeWork4
{
    using System;
    using Algorithms;
    using static System.Console;
    using static Helpers.Helpers;

    #region Структуры

    /// <summary>
    /// Структура обозначений клеток на шахматном поле
    /// </summary>
    public struct Cells
    {
        /// <summary>
        /// Обозначение пустой клетки
        /// </summary>
        public const char emptyCell = '.';
        /// <summary>
        /// Обозначение клетки с фигурой
        /// </summary>
        public const char horseCell = 'O';
        /// <summary>
        /// Обозначение использованных фигурой клеток
        /// </summary>
        public const char usedCell = 'X';
    }

    /// <summary>
    /// Структура позиций фигуры "Конь"
    /// </summary>

    struct HorsePosition
    {
        /// <summary>
        /// Позиция фигуры по вертикали
        /// </summary>
        public int posY;
        /// <summary>
        /// Позиция фигуры по горизонтали
        /// </summary>
        public int posX;
    }

    /// <summary>
    /// Структура переменных статистики
    /// </summary>

    struct Statistics
    {
        /// <summary>
        /// Статистика ходов фигуры
        /// </summary>
        public long turnOperations;
        /// <summary>
        /// Статистика всех операций 
        /// (включая постановку фигуры, обратные ходы, отрисовку поля,
        /// подсчет следующего хода фигуры, возможности постановки фигуры, проверку завершения задачи)
        /// </summary>
        public long allOperations;
        /// <summary>
        /// Время начала работы алгоритма
        /// </summary>
        public DateTime startTime;
    }

    /// <summary>
    /// Структура технических переменных
    /// </summary>
    public struct TechnicalVariables
    {
        /// <summary>
        /// Значение паузы после отрисовки шахматного поля
        /// </summary>
        public int pauseValue;
        /// <summary>
        /// Значение высоты поля
        /// </summary>
        public int fieldY;
        /// <summary>
        /// Значение ширины поля
        /// </summary>
        public int fieldX;
    }

    #endregion

    class Program
    {
        /// <summary>
        /// Шахматное поле
        /// </summary>
        private static char[,] chessField;
        /// <summary>
        /// Технические переменные
        /// </summary>
        private static TechnicalVariables techVar;

        static void Main()
        {
            techVar = new TechnicalVariables
            {
                pauseValue = 0,
                fieldY = 4,
                fieldX = 4
            };
            
            chessField = new char[techVar.fieldY, techVar.fieldX];

            CreateChessField(ref chessField);
            DrawChessField(chessField);

            var algorithm = new BasicAlgorithm(chessField, techVar);
            algorithm.StartSearching();

            ReadKey();
        }
    }
}
