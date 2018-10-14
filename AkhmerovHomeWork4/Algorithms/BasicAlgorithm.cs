namespace AkhmerovHomeWork4.Algorithms
{
    using System;
    using System.Threading;
    using static Helpers.Helpers;

    /// <summary>
    /// Стандартный алгоритм решения задачи о ходе коня
    /// </summary>

    public class BasicAlgorithm
    {
        #region Операбельные переменные

        /// <summary>
        /// Хранилище ходов
        /// </summary>
        readonly int[] passTurns;
        /// <summary>
        /// Завершенные ходы
        /// </summary>
        readonly string[] finishTurns;
        /// <summary>
        /// Шахматное поле
        /// </summary>
        readonly char[,] chessField;
        /// <summary>
        /// Структура технических переменных
        /// </summary>
        private TechnicalVariables techVar;
        /// <summary>
        /// Структура переменных статистики
        /// </summary>
        private Statistics stats = new Statistics
        {
            turnOperations = 0,
            allOperations = 0
        };
        /// <summary>
        /// Фигура "Конь"
        /// </summary>
        private HorsePosition horse;
        /// <summary>
        /// Временно сохраненное положение фигуры "Конь"
        /// </summary>
        private HorsePosition tempHorse;
        /// <summary>
        /// Номер хода на данный момент
        /// </summary>
        private int thisTurn;

        #endregion

        /// <summary>
        /// Конструктор медленного алгоритма поиска решения задачи о ходе коня
        /// </summary>
        /// <param name="chessField">Двумерный массив (шахматное поле)</param>
        /// <param name="techVar">Структура технический переменных</param>

        public BasicAlgorithm(char[,] chessField, TechnicalVariables techVar)
        {
            this.chessField = chessField;
            this.techVar = techVar;

            passTurns = new int[chessField.Length];
            finishTurns = new string[chessField.Length];
        }

        /// <summary>
        /// Начало поиска возможного решения задачи
        /// </summary>

        public void StartSearching()
        {
            stats.startTime = DateTime.Now;

            for (var i = 0; i < techVar.fieldY; i++)
            {
                horse.posY = i;
                for (var j = 0; j < techVar.fieldX; j++)
                {
                    horse.posX = j;
                    
                    chessField[i, j] = Cells.horseCell;
                    DrawChessField(chessField);
                    stats.allOperations += 2;

                    Thread.Sleep(techVar.pauseValue);

                    for (var k = 0; k < 8; k++)
                    {
                        tempHorse = HorseTurn(k, horse, false);

                        if (!CheckTurnPossibly(tempHorse, chessField))
                        {
                            if (k < 7) continue;

                            if (thisTurn == 0)
                            {
                                chessField[horse.posY, horse.posX] = Cells.emptyCell;
                                break;
                            }

                            thisTurn--;
                            
                            if (passTurns[thisTurn] != 7)
                            {
                                k = passTurns[thisTurn];

                                chessField[horse.posY, horse.posX] = Cells.emptyCell;
                                horse = HorseTurn(k, horse, true);
                                chessField[horse.posY, horse.posX] = Cells.horseCell;
                                DrawChessField(chessField);
                                stats.allOperations += 3;
                            }
                            else
                            {
                                while (thisTurn >= 0 && passTurns[thisTurn] == 7)
                                {
                                    k = passTurns[thisTurn];

                                    chessField[horse.posY, horse.posX] = Cells.emptyCell;
                                    horse = HorseTurn(k, horse, true);
                                    chessField[horse.posY, horse.posX] = Cells.horseCell;
                                    DrawChessField(chessField);
                                    stats.allOperations += 3;

                                    thisTurn--;
                                }

                                if (thisTurn < 0)
                                {
                                    if (i < techVar.fieldY - 1)
                                    {
                                        thisTurn = 0;
                                        break;
                                    }
                                    ImpossibleTask();
                                    return;
                                }

                                k = passTurns[thisTurn];

                                chessField[horse.posY, horse.posX] = Cells.emptyCell;
                                horse = HorseTurn(k, horse, true);
                                chessField[horse.posY, horse.posX] = Cells.horseCell;
                                DrawChessField(chessField);
                                stats.allOperations += 3;
                            }

                            continue;
                        }
                        
                        finishTurns[thisTurn] =
                            $"{thisTurn + 1,2}-й ход: Y{horse.posY}, X{horse.posX} -> Y{tempHorse.posY}, X{tempHorse.posX}";

                        passTurns[thisTurn] = k;

                        chessField[horse.posY, horse.posX] = Cells.usedCell;
                        horse = tempHorse;
                        chessField[horse.posY, horse.posX] = Cells.horseCell;
                        DrawChessField(chessField);
                        stats.allOperations += 4;
                        stats.turnOperations++;

                        thisTurn++;
                        k = -1;

                        Thread.Sleep(techVar.pauseValue);

                        if (CheckFinish(chessField, finishTurns, ref stats))
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверка возможности сделать ход
        /// </summary>
        /// <param name="horsePos">Структура позиции фигуры "Конь"</param>
        /// <param name="chessField">Двумерный массив (шахматное поле)</param>
        /// <returns></returns>

        static bool CheckTurnPossibly(HorsePosition horsePos, char[,] chessField)
        {
            if (horsePos.posY < 0 || horsePos.posX < 0)
            {
                return false;
            }

            if (horsePos.posY >= chessField.GetLength(0) || horsePos.posX >= chessField.GetLength(1))
            {
                return false;
            }

            return chessField[horsePos.posY, horsePos.posX] != Cells.usedCell;
        }

        /// <summary>
        /// Ход фигуры "Конь" по индексу
        /// </summary>
        /// <param name="turnIndex">Индекс хода фигуры "Конь"</param>
        /// <param name="horsePos">Структура позиции фигуры "Конь"</param>
        /// <param name="undo">Флаг отмены хода</param>
        /// <returns></returns>

        static HorsePosition HorseTurn(int turnIndex, HorsePosition horsePos, bool undo)
        {
            if (undo)
            {
                ToUndo(ref turnIndex);
            }

            switch (turnIndex)
            {
                case 0:
                    horsePos.posY -= 2;
                    horsePos.posX -= 1;
                    break;
                case 1:
                    horsePos.posY -= 2;
                    horsePos.posX += 1;
                    break;
                case 2:
                    horsePos.posY -= 1;
                    horsePos.posX += 2;
                    break;
                case 3:
                    horsePos.posY += 1;
                    horsePos.posX += 2;
                    break;
                case 4:
                    horsePos.posY += 2;
                    horsePos.posX += 1;
                    break;
                case 5:
                    horsePos.posY += 2;
                    horsePos.posX -= 1;
                    break;
                case 6:
                    horsePos.posY += 1;
                    horsePos.posX -= 2;
                    break;
                case 7:
                    horsePos.posY -= 1;
                    horsePos.posX -= 2;
                    break;
                default:
                    return horsePos;
            }

            return horsePos;
        }

        /// <summary>
        /// Отмена хода (реверсия хода)
        /// </summary>
        /// <param name="turnIndex">Индекс хода фигуры "Конь"</param>

        static void ToUndo(ref int turnIndex)
        {
            switch (turnIndex)
            {
                case 0:
                    turnIndex = 4;
                    break;
                case 1:
                    turnIndex = 5;
                    break;
                case 2:
                    turnIndex = 6;
                    break;
                case 3:
                    turnIndex = 7;
                    break;
                case 4:
                    turnIndex = 0;
                    break;
                case 5:
                    turnIndex = 1;
                    break;
                case 6:
                    turnIndex = 2;
                    break;
                case 7:
                    turnIndex = 3;
                    break;
                default:
                    return;
            }
        }
    }
}
