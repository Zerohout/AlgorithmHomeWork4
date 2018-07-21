namespace AkhmerovHomeWork4.Algorithms
{
    using System.Threading;
    using static Helpers.Helpers;

    /// <summary>
    /// Медленный алгоритм решения задачи о ходе коня
    /// </summary>

    class SlowAlgorithm
    {
        readonly int[] tempTurns;
        readonly string[] finishTurns;
        readonly char[,] chessField;

        private Cells cells;
        private TechnicalVariables techVar;

        /// <summary>
        /// Структура переменных статистики
        /// </summary>

        private Statistics stats = new Statistics
        {
            clearOperations = 0,
            allOperations = 0
        };

        /// <summary>
        /// Конструктор медленного алгоритма поиска решения задачи о ходе коня
        /// </summary>
        /// <param name="chessField">Двумерный массив (шахматное поле)</param>
        /// <param name="cells">Структура обозначений</param>
        /// <param name="techVar">Структура технический переменных</param>

        public SlowAlgorithm(char[,] chessField, Cells cells, TechnicalVariables techVar)
        {
            this.chessField = chessField;
            this.cells = cells;
            this.techVar = techVar;
            
            tempTurns = new int[chessField.Length];
            finishTurns = new string[chessField.Length];
        }

        /// <summary>
        /// Начало поиска возможного решения задачи
        /// </summary>

        public void StartSearching()
        {
            var horse = new HorsePosition();
            var thisTurn = 0;

            for (var i = 0; i < techVar.fieldY; i++)
            {
                horse.posY = i;
                for (var j = 0; j < techVar.fieldX; j++)
                {
                    horse.posX = j;

                    chessField[i, j] = cells.horseCell;
                    DrawChessField(chessField);
                    Thread.Sleep(techVar.pauseValue);

                    for (var k = 0; k < 8; k++)
                    {
                        var tempHorse = AvailableHorseTurns(k, horse, false);

                        if (!CheckTurnPossibly(tempHorse, chessField) || 
                            chessField[tempHorse.posY, tempHorse.posX] == cells.usedTurnCell)
                        {
                            if (k == 7)
                            {
                                if (horse.posY == i && horse.posX == j)
                                {
                                    chessField[horse.posY, horse.posX] = cells.emptyCell;
                                    break;
                                }

                                thisTurn--;

                                if (thisTurn < 0)
                                {
                                    ImpossibleTask();
                                    return;
                                }

                                if (tempTurns[thisTurn] != 7)
                                {
                                    k = tempTurns[thisTurn];
                                    chessField[horse.posY, horse.posX] = cells.emptyCell;
                                    horse = AvailableHorseTurns(k, horse, true);
                                    chessField[horse.posY, horse.posX] = cells.horseCell;
                                    stats.allOperations++;
                                    DrawChessField(chessField);
                                }
                                else
                                {
                                    while (tempTurns[thisTurn] == 7)
                                    {
                                        k = tempTurns[thisTurn];
                                        chessField[horse.posY, horse.posX] = cells.emptyCell;
                                        horse = AvailableHorseTurns(k, horse, true);
                                        chessField[horse.posY, horse.posX] = cells.horseCell;
                                        stats.allOperations++;
                                        DrawChessField(chessField);

                                        thisTurn--;

                                        if (thisTurn < 0)
                                        {
                                            ImpossibleTask();
                                            return;
                                        }
                                    }

                                    k = tempTurns[thisTurn];
                                    chessField[horse.posY, horse.posX] = cells.emptyCell;
                                    horse = AvailableHorseTurns(k, horse, true);
                                    chessField[horse.posY, horse.posX] = cells.horseCell;
                                    stats.clearOperations++;
                                    DrawChessField(chessField);
                                }
                            }
                            continue;
                        }

                        if (chessField[tempHorse.posY, tempHorse.posX] == cells.usedTurnCell) continue;

                        finishTurns[thisTurn] =
                            $"{thisTurn}-й ход: Y{horse.posY}, X{horse.posX} -> Y{tempHorse.posY}, X{tempHorse.posX}";

                        chessField[horse.posY, horse.posX] = cells.usedTurnCell;
                        horse = tempHorse;
                        chessField[horse.posY, horse.posX] = cells.horseCell;
                        tempTurns[thisTurn] = k;

                        thisTurn++;
                        k = -1;

                        DrawChessField(chessField);
                        Thread.Sleep(techVar.pauseValue);
                        CheckFinish(chessField, finishTurns, stats, cells);
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

            return horsePos.posY < chessField.GetLength(0) && horsePos.posX < chessField.GetLength(1);
        }
        
        /// <summary>
        /// Возможные ходы фигуры "Конь"
        /// </summary>
        /// <param name="turnValue">Индекс хода фигуры "Конь"</param>
        /// <param name="horsePos">Структура позиции фигуры "Конь"</param>
        /// <param name="undo">Флаг отмены хода</param>
        /// <returns></returns>

        static HorsePosition AvailableHorseTurns(int turnValue, HorsePosition horsePos, bool undo)
        {
            if (undo)
            {
                ToUndo(ref turnValue);
            }

            switch (turnValue)
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
        /// <param name="turnValue">Индекс хода фигуры "Конь"</param>

        static void ToUndo(ref int turnValue)
        {
            switch (turnValue)
            {
                case 0:
                    turnValue = 4;
                    break;
                case 1:
                    turnValue = 5;
                    break;
                case 2:
                    turnValue = 6;
                    break;
                case 3:
                    turnValue = 7;
                    break;
                case 4:
                    turnValue = 0;
                    break;
                case 5:
                    turnValue = 1;
                    break;
                case 6:
                    turnValue = 2;
                    break;
                case 7:
                    turnValue = 3;
                    break;
                default:
                    return;
            }
        }
    }
}
