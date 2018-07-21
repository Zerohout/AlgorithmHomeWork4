namespace AkhmerovHomeWork4
{
    using Algorithms;
    using static System.Console;
    using static Helpers.Helpers;

    #region Структуры

    /// <summary>
    /// Структура обозначений клеток на шахматном поле
    /// </summary>

    struct Cells
    {
        /// <summary>
        /// Обозначение пустой клетки
        /// </summary>
        public char emptyCell;
        /// <summary>
        /// Обозначение клетки с фигурой
        /// </summary>
        public char horseCell;
        /// <summary>
        /// Обозначение использованных фигурой клеток
        /// </summary>
        public char usedTurnCell;
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
        /// Статистика чистых операций (без подсчета отмены ходов)
        /// </summary>
        public long clearOperations;
        /// <summary>
        /// Статистика всех операций (включая отмены хода)
        /// </summary>
        public long allOperations;
    }

    /// <summary>
    /// Структура технических переменных
    /// </summary>

    struct TechnicalVariables
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
        static void Main()
        {
            var cells = new Cells
            {
                emptyCell = '.',
                horseCell = 'O',
                usedTurnCell = 'X'
            };

            var techVar = new TechnicalVariables
            {
                pauseValue = 0,
                fieldY = 5,
                fieldX = 5
            };

            var testChessField = new char[techVar.fieldY, techVar.fieldX];

            CreateChessField(ref testChessField, cells.emptyCell);
            DrawChessField(testChessField);

            var algorithm = new SlowAlgorithm(testChessField, cells, techVar);
            algorithm.StartSearching();

            ReadKey();
        }
    }
}
