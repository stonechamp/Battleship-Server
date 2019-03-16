//-----------------------------------------------------------
//File:   Prog5.cs
//Desc:   Battleship Server by Stone Champion
//-----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Battleship.Model
{
    public abstract class ResponseMessage
    {
        public abstract string Serialize(Game game, StreamWriter writer, Action<string> logAction, string ip);

        
    }

    public class AttackResponseMessage : ResponseMessage
    {
        public const string MSG_TYPE = "AttackResponse";
        public string attackType = "test";
        public int Row { get; set; }
        public int Col { get; set; }

        public string AttackType { get { return attackType; } set { attackType = value; } }

        public AttackResponseMessage() { }

        public AttackResponseMessage(int row, int col)
        {
            Row = row;
            Col = col;
            
        }

        //Takes the parsed data and applies it to the game and returns a formatted message.
        public override string Serialize(Game game, StreamWriter writer, Action<string> logAction, string ip)
        {
            if (game.EnemyBoard[Row, Col] == Game.PositionState.Filled)
            {
                AttackType = "sink";
                game.EnemyBoard[Row, Col] = Game.PositionState.Hit;
                game.enemyShipsHit++;
            } else if (game.EnemyBoard[Row, Col] == Game.PositionState.Open)
            {
                AttackType = "miss";
                game.EnemyBoard[Row, Col] = Game.PositionState.Missed;
            } else
            {
                AttackType = "dup";
            }
            logAction("Player " + ip + " attacked at " + "[" + Row + ", " + Col + "]" + " in game: " + game.Name);
            game.EnemyAttack();
            game.SetStatus();
            return string.Format("{0}: {1} [{2}, {3}]", MSG_TYPE, AttackType, Row, Col);
        }

        

    }

    public class GameStateResponseMessage : ResponseMessage
    {
        public const string MSG_TYPE = "GameStateResponse";

        public Game TheList { get; set; }

        public GameStateResponseMessage()
        {
            
        }
        //Returns a nice formatted message for the GameState command.
        public override string Serialize(Game game, StreamWriter writer, Action<string> logAction, string ip)
        {
            logAction("Player " + ip + " has requested the Game State for game: " + game.Name);
            
            return string.Format(@"{0}: {1}
{2}
---
{3}", MSG_TYPE, game.Status, game.PlayerBoardString(), game.EnemyBoardString());
            
        }

        
        
        
    }
}

