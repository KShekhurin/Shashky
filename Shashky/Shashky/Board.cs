using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shashky
{
    class Board
    {
        public List<List<Segment>> board = new List<List<Segment>>();

        public Board()
        {

        }

        public void PlaysFigures()
        {
            bool placed = false;
            for (int k = 0; k < 8; k++)
            {
                board.Add(new List<Segment>());
                for (int i = 0; i < 8; i++)
                {
                    board[k].Add(new Segment());
                }
            }
            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!placed)
                    {
                        board[k][i].type = TypeOfSegment.Black;
                        placed = true;
                    }
                    else
                    {
                        placed = false;
                    }
                }
                if (placed)
                {
                    placed = false;
                }
                else
                {
                    placed = true;
                }
            }

            for (int k = 7; k > 4; k--)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (!placed)
                    {
                        board[k][i].type = TypeOfSegment.Wite;
                        placed = true;
                    }
                    else
                    {
                        placed = false;
                    }
                }
                if (placed)
                {
                    placed = false;
                }
                else
                {
                    placed = true;
                }
            }
        }

        public void ClearSelect()
        {
            for (int k = 0; k < 8; k++)
            {
                
                for (int i = 0; i < 8; i++)
                {
                    board[k][i].mouseOn = false;
                }
            }
        }

        public void ClearEater()
        {
            for (int k = 0; k < 8; k++)
            {

                for (int i = 0; i < 8; i++)
                {
                    board[k][i].eatX = -1;
                    board[k][i].eatY = -1;
                }
            }
        }

        public void ClearTarget()
        {
            for (int k = 0; k < 8; k++)
            {

                for (int i = 0; i < 8; i++)
                {
                    board[k][i].isTarget = false;
                }
            }
        }
    }
}