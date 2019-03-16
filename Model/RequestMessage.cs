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
    public abstract class RequestMessage
    {
        public abstract ResponseMessage Execute();

        public abstract string Serialize();

        //Splits the message given by the client and uses the parsed data for specific actions.
        public static RequestMessage Deserialize(string msg)
        {
            string[] parts = msg.Split(' ');
            string msgType = parts[0];
            switch (msgType)
            {
                case AttackRequestMessage.MSG_TYPE:
                    return AttackRequestMessage.Deserialize(parts);
                case GameStateRequestMessage.MSG_TYPE:
                    return new GameStateRequestMessage();
                default:
                    throw new Exception("Unknown response message type " + msgType);
            }
        }

    }

    public class AttackRequestMessage : RequestMessage
    {
        public const string MSG_TYPE = "Attack";

        public int Row { get; set; }

        public int Col { get; set; }

        

        public AttackRequestMessage(int row, int col)
        {
            Row = row;
            Col = col;
            
        }

        public override ResponseMessage Execute()
        {
            
            return new AttackResponseMessage(Row, Col);
        }

        public override string Serialize()
        {
            
            return string.Format("{0}: {1} {2}", MSG_TYPE, Row, Col);
        }

        public static AttackRequestMessage Deserialize(string[] parts)
        {
            return new AttackRequestMessage(int.Parse(parts[1]), int.Parse(parts[2]));
        }

    }

    public class GameStateRequestMessage : RequestMessage
    {
        public const string MSG_TYPE = "GameState";

        public override ResponseMessage Execute()
        {
            return new GameStateResponseMessage();
        }

        public override string Serialize()
        {
            return MSG_TYPE;
        }


    }
}
