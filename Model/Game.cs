//-----------------------------------------------------------
//File:   Prog5.cs
//Desc:   Battleship Server by Stone Champion
//-----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Model
{
    public class Game
    {

        public int size;
        public string name;
        public int playerShipsHit = 0;
        public int enemyShipsHit = 0;

        public static bool isCheatOn = true;
        public bool playerTurn = true;
        public static bool resetColor = false;
        public string status = "active";

        public string gameBoard;
        

        static Random rand = new Random();

        public enum PositionState { Open, Hit, Filled, Missed, /*Colorless*/ }

        public PositionState[,] playerBoard;
        public PositionState[,] enemyBoard;
        public PositionState[,] PlayerBoard { get { return playerBoard; } set { playerBoard = value; } }
        public PositionState[,] EnemyBoard
        {
            get { return enemyBoard; }
            set { enemyBoard = value; }
        }

        public static Random Random { get { return rand; } }

        public string Status { get { return status; } set { status = value; } }
        public static bool IsCheatOn { get { return isCheatOn; } set { isCheatOn = value; } }
        public bool PlayerTurn { get { return playerTurn; } set { playerTurn = value; } }
        public static bool ResetColor { get { return resetColor; } set { resetColor = value; } }
        public int Size { get { return size; } set { size = value; } }
        public string Name { get { return name; } set { name = value; } }
        public Game(int s, string name)
        {
            Size = s;
            playerBoard = new PositionState[s, s];
            enemyBoard = new PositionState[s, s];
            PlaceEnemyShips();
            PlacePlayerShips();
            this.name = name;
            
        }

        //Updates the 2d array of position states.
        public void EnemyAttack()
        {
            int row = Random.Next(size);
            int col = Random.Next(size);

            while (PlayerBoard[row, col] != PositionState.Hit || PlayerBoard[row, col] != PositionState.Missed)
            {
                row = Random.Next(size);
                col = Random.Next(size);
                if (PlayerBoard[row, col] == PositionState.Open)
                {
                    PlayerBoard[row, col] = PositionState.Missed;
                    break;
                }
                if (PlayerBoard[row, col] == PositionState.Filled)
                {
                    PlayerBoard[row, col] = PositionState.Hit;
                    playerShipsHit++;
                    break;
                }
            }
            PlayerTurn = true;
        }

        

        

        //Places enemy positions in enemy randomly
        public void PlaceEnemyShips()
        {

            for (int i = 0; i < 5; i++)
            {
                int row;
                int col;
                do
                {

                    row = Random.Next(size);
                    col = Random.Next(size);
                } while (enemyBoard[row, col] != PositionState.Open);

                enemyBoard[row, col] = PositionState.Filled;
            }
        }

        //Places the player positions randomly.
        public void PlacePlayerShips()
        {

            for (int i = 0; i < 5; i++)
            {
                int row;
                int col;
                do
                {

                    row = Random.Next(size);
                    col = Random.Next(size);
                } while (PlayerBoard[row, col] != PositionState.Open);

                playerBoard[row, col] = PositionState.Filled;
            }
        }
        //Prints the playerbhoard.
        public  void printPlayerBoard(StreamWriter writer)
        {
            

            for (int i = 0; i < playerBoard.GetLength(0); i++)
            {
                for (int j = 0; j < playerBoard.GetLength(1); j++)
                {


                    writer.Write(Translate(playerBoard[i, j]));


                }
                writer.WriteLine();

            }


        }
        //Turns player board enum to string.
        public string PlayerBoardString()
        {

            string player = "";
            for (int i = 0; i < playerBoard.GetLength(0); i++)
            {
                for (int j = 0; j < playerBoard.GetLength(1); j++)
                {


                    player += Translate(playerBoard[i, j]);


                }
                player += "\r\n";

            }
            
            return player;
        }
        //Prints enemy board
        public void printEnemyBoard(StreamWriter writer)
        {
            

            for (int i = 0; i < playerBoard.GetLength(0); i++)
            {
                for (int j = 0; j < playerBoard.GetLength(1); j++)
                {


                    writer.Write(Translate(enemyBoard[i, j]));


                }
                writer.WriteLine();

            }
            writer.WriteLine("");

        }
        //Prints enemy board as a string.
        public string EnemyBoardString()
        {
            

            string enemy = "";
            for (int i = 0; i < playerBoard.GetLength(0); i++)
            {
                for (int j = 0; j < playerBoard.GetLength(1); j++)
                {


                    enemy += Translate(enemyBoard[i, j]);


                }
                enemy += "\r\n";

            }
            
            return enemy;
        }
        //Send the game state of the game to the client.
        public void SendGameState(StreamWriter writer, Game game)
        {

            writer.WriteLine("GameStateResponse: " + game.Status);
            printPlayerBoard(writer);
            writer.WriteLine("---");
            printEnemyBoard(writer);
            writer.Flush();
        }
        //Set the game state status.
        public void SetStatus()
        {
 
            if (playerShipsHit == 5)
            {
                Status = "ended enemy";
            } else if (enemyShipsHit == 5)
            {
                Status = "ended player";
            } else
            {
                Status = "active";
            }


        }
        //Translates the posisiton enums to a string.
        public string Translate(PositionState position)
        {
            switch (position)
            {
                case PositionState.Filled:
                    return "X";
                case PositionState.Missed:
                    return "O";
                case PositionState.Hit:
                    return "*";
                case PositionState.Open:
                default:
                    return "~";
            }

        }


    }

   
}
