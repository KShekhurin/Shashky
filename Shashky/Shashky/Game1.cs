using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shashky
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Board board = new Board();

        Texture2D rectangleBlock;
        Texture2D rectangleBlock2;
        Texture2D circle1;
        Texture2D circle2;
        Texture2D ramka1;
        Texture2D ramka2;
        Texture2D targetPoint;

        Point selectedPosition;
        bool selected = false;

        bool buttonIsPressed = false;
        bool buttonIsPressed2 = false;

        bool blackTurn = false;

        bool needOneTurnMore = false;
        Point[] needOneTurnFor = { new Point(-1, -1), new Point(-1, -1), new Point(-1, -1), new Point(-1,-1)};

        Point positionOfNeedSelection;

        bool youCantBreackSelect = false;

        Point rightToTurnMore;
        Point leftToTurnMore;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 248 * 2 + 20;
            graphics.PreferredBackBufferWidth = 248 * 2;
        }

        protected override void Initialize()
        {
            base.Initialize();
            board.PlaysFigures();
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            rectangleBlock = Content.Load<Texture2D>("SiniyBlockPolia");
            rectangleBlock2 = Content.Load<Texture2D>("KorichneviyBlockPolia");

            circle1 = Content.Load<Texture2D>("BelaiaShashka");
            circle2 = Content.Load<Texture2D>("ChornayaShashka");

            ramka1 = Content.Load<Texture2D>("KrastaiaRamka");
            ramka2 = Content.Load<Texture2D>("SiniyaRamca");

            targetPoint = Content.Load<Texture2D>("TargetPoint");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && !youCantBreackSelect) { 
                selected = false;
                needOneTurnMore = false;
                needOneTurnFor[0] = new Point(-1, -1);
                needOneTurnFor[1] = new Point(-1, -1);
                leftToTurnMore = new Point(-1, -1);
                rightToTurnMore = new Point(-1, -1);
                board.ClearTarget();
            }
            
            board.ClearSelect();
            
           

            MouseState state = Mouse.GetState();

            if (state.LeftButton == ButtonState.Pressed)
            {
                buttonIsPressed = true;
            }
            else
            {
                buttonIsPressed = false;
            }

            Point position = GetPosition(state.X, state.Y);

            board.board[position.Y][position.X].mouseOn = true;
            if (!blackTurn)
            {
                if (state.LeftButton == ButtonState.Pressed && board.board[position.Y][position.X].type == TypeOfSegment.Wite && selected == false && buttonIsPressed == true && buttonIsPressed2 == false)
                {
                    board.ClearTarget();
                    SetTargets(position, false);
                    selectedPosition = position;
                    selected = true;
                    

                }
                else if (state.LeftButton == ButtonState.Pressed && board.board[position.Y][position.X].isTarget && board.board[position.Y][position.X].type == TypeOfSegment.Null && position != selectedPosition && buttonIsPressed == true && buttonIsPressed2 == false && selected == true)
                {
                    board.board[position.Y][position.X].type = board.board[selectedPosition.Y][selectedPosition.X].type;
                    board.board[selectedPosition.Y][selectedPosition.X].type = TypeOfSegment.Null;
                    if (position == leftToTurnMore || position == rightToTurnMore)
                    {
                        youCantBreackSelect = true;
                    }
                    else
                    {
                        needOneTurnMore = false;
                        youCantBreackSelect = false;
                    }
                    if (!needOneTurnMore)
                    {
                        selected = false;
                        blackTurn = true;
                    }
                    else
                    {
                        selected = true;
                    }
                    
                    if (board.board[position.Y][position.X].eatX != -1 && board.board[position.Y][position.X].eatY != -1)
                    {
                        board.board[board.board[position.Y][position.X].eatY][board.board[position.Y][position.X].eatX].type = TypeOfSegment.Null;
                        board.board[position.Y][position.X].eatX = -1;
                        board.board[position.Y][position.X].eatY = -1;
                    }
                    selectedPosition = position;
                    
                    board.ClearTarget();
                    if (needOneTurnMore)
                    {
                        SetTargets(position, false);
                    }

                    rightToTurnMore = new Point(-1, -1);
                    leftToTurnMore = new Point(-1, -1);
                }
            }
            else
            {
                if (state.LeftButton == ButtonState.Pressed && board.board[position.Y][position.X].type == TypeOfSegment.Black && selected == false && buttonIsPressed == true && buttonIsPressed2 == false)
                {
                    board.ClearTarget();
                    SetTargets(position, true);
                    selectedPosition = position;
                    selected = true;

                }
                else if (state.LeftButton == ButtonState.Pressed && board.board[position.Y][position.X].isTarget && board.board[position.Y][position.X].type == TypeOfSegment.Null && buttonIsPressed == true && buttonIsPressed2 == false && selected == true)
                {
                    board.board[position.Y][position.X].type = board.board[selectedPosition.Y][selectedPosition.X].type;
                    board.board[selectedPosition.Y][selectedPosition.X].type = TypeOfSegment.Null;

                    if (!needOneTurnMore)
                    {
                        selected = false;
                        blackTurn = false;
                    }
                    else
                    {
                        selected = true;
                    }
                    if (board.board[position.Y][position.X].eatX != -1 && board.board[position.Y][position.X].eatY != -1)
                    {
                        board.board[board.board[position.Y][position.X].eatY][board.board[position.Y][position.X].eatX].type = TypeOfSegment.Null;
                        board.board[position.Y][position.X].eatX = -1;
                        board.board[position.Y][position.X].eatY = -1;

                    }
                    selectedPosition = position;
                    board.ClearTarget();
                    if (needOneTurnMore)
                    {
                        SetTargets(position, true);
                    }
                }
            }

            

            if (state.LeftButton == ButtonState.Pressed)
            {
                buttonIsPressed2 = true;  
            }
            else
            {
                buttonIsPressed2 = false;
            }

            base.Update(gameTime);

            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            bool check = false;
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (!check)
                    {
                        spriteBatch.Draw(rectangleBlock, new Vector2(k * 62, i * 62), Color.White);
                        check = true;
                    }
                    else
                    {
                        spriteBatch.Draw(rectangleBlock2, new Vector2(k * 62, i * 62), Color.White);
                        check = false;
                    }
                }
                if (check)
                {
                    check = false;
                }
                else
                {
                    check = true;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (board.board[i][k].isTarget)
                    {
                        spriteBatch.Draw(targetPoint, new Vector2(k * 62, i * 62), Color.White);
                    }
                    if (selected)
                    {
                        spriteBatch.Draw(ramka1, new Vector2(selectedPosition.X * 62, selectedPosition.Y * 62), Color.White);
                        
                    }
                    if (board.board[i][k].mouseOn && selected)
                    {
                        spriteBatch.Draw(ramka2, new Vector2(k * 62, i * 62), Color.White);
                    }
                    else if (board.board[i][k].mouseOn && !selected)
                    {
                        spriteBatch.Draw(ramka1, new Vector2(k * 62, i * 62), Color.White);
                    }
                    if (board.board[i][k].type == TypeOfSegment.Black)
                    {
                        spriteBatch.Draw(circle2, new Vector2(k * 62, i * 62), Color.White);
                        check = true;
                    }
                    else if(board.board[i][k].type == TypeOfSegment.Wite)
                    {
                        spriteBatch.Draw(circle1, new Vector2(k * 62, i * 62), Color.White);
                        check = false;
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        Point GetPosition(int posX, int posY)
        {
            int x = 0;
            int y = 0;

            if (posX >= 0 && posX <= 31 * 2)
            {
                x = 0;
                y = GetY(posY);
            }
            else if (posX > 31 * 2 && posX <= 62 * 2)
            {
                x = 1;
                y = GetY(posY);
            }
            else if (posX > 62 * 2 && posX <= 93 * 2)
            {
                x = 2;
                y = GetY(posY);
            }
            else if (posX > 93 * 2 && posX <= 124 * 2)
            {
                x = 3;
                y = GetY(posY);
            }
            else if (posX > 124 * 2 && posX <= 155 * 2)
            {
                x = 4;
                y = GetY(posY);
            }
            else if (posX > 155 * 2 && posX <= 186 * 2)
            {
                x = 5;
                y = GetY(posY);
            }
            else if (posX > 186 * 2 && posX <= 217 * 2)
            {
                x = 6;
                y = GetY(posY);
            }
            else if (posX > 217 * 2 && posX <= 248 * 2)
            {
                x = 7;
                y = GetY(posY);
            }

            return new Point(x,y);
        }

        int GetY(int posY)
        {
            int y = 0; 
            if (posY >= 0 && posY <= 31 * 2)
            {
                y = 0;
            }
            else if (posY > 31 * 2 && posY <= 62 * 2)
            {
                y = 1;
            }
            else if (posY > 62 * 2 && posY <= 93 * 2)
            {
                y = 2;
            }
            else if (posY > 93 * 2 && posY <= 124 * 2)
            {
                y = 3;
            }
            else if (posY > 124 * 2 && posY <= 155 * 2)
            {
                y = 4;
            }
            else if (posY > 155 * 2 && posY <= 186 * 2)
            {
                y = 5;
            }
            else if (posY > 186 * 2 && posY <= 217 * 2)
            {
                y = 6;
            }
            else if (posY > 217 * 2 && posY <= 248 * 2)
            {
                y = 7;
            }
            return y;
        }

        public void SetTargets(Point point, bool isBlack)
        {
            board.ClearEater();
            if (isBlack)
            {
                if (point.Y != 7)
                {
                    if (point.X == 0)
                    {
                        while (true)
                        {
                            if (board.board[point.Y + 1][point.X + 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y + 1][point.X + 1].isTarget = true;
                                }
                                break;
                            }
                            else
                            {
                                if (point.Y != 6)
                                {
                                    if (board.board[point.Y + 1][point.X + 1].type == TypeOfSegment.Wite && board.board[point.Y + 2][point.X + 2].type == TypeOfSegment.Null)
                                    {
                                        board.board[point.Y + 2][point.X + 2].isTarget = true;
                                        board.board[point.Y + 2][point.X + 2].eatY = point.Y + 1;
                                        board.board[point.Y + 2][point.X + 2].eatX = point.X + 1;
                                        if (point.Y < 4 && point.X < 4)
                                        {
                                            if (board.board[point.Y + 3][point.X + 3].type == TypeOfSegment.Wite && board.board[point.Y + 4][point.X + 4].type == TypeOfSegment.Null)
                                            {
                                                needOneTurnFor[1] = new Point(point.Y + 2, point.X + 2);
                                                needOneTurnMore = true;
                                            }
                                            else
                                            {
                                                needOneTurnFor[1] = new Point(-1, -1);
                                                //needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }
                                        else
                                        {
                                            needOneTurnFor[1] = new Point(-1, -1);
                                            //needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (point.X == 7)
                    {
                        while (true)
                        {
                            if (board.board[point.Y + 1][point.X - 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y + 1][point.X - 1].isTarget = true;
                                }
                                break;
                            }
                            else
                            {
                                if (point.Y != 6)
                                {
                                    if (board.board[point.Y + 1][point.X - 1].type == TypeOfSegment.Wite && board.board[point.Y + 2][point.X - 2].type == TypeOfSegment.Null)
                                    {
                                        board.board[point.Y + 2][point.X - 2].isTarget = true;
                                        board.board[point.Y + 2][point.X - 2].eatY = point.Y + 1;
                                        board.board[point.Y + 2][point.X - 2].eatX = point.X - 1;
                                        if (point.Y < 4 && point.X > 3)
                                        {
                                            if (board.board[point.Y + 3][point.X - 3].type == TypeOfSegment.Wite && board.board[point.Y + 4][point.X - 4].type == TypeOfSegment.Null)
                                            {
                                                needOneTurnFor[0] = new Point(point.Y + 2, point.X - 2);
                                                needOneTurnMore = true;
                                            }
                                            else
                                            {
                                                //needOneTurnFor[1] = new Point(-1, -1);
                                                needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }
                                        else
                                        {
                                            //needOneTurnFor[1] = new Point(-1, -1);
                                            needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (board.board[point.Y + 1][point.X - 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y + 1][point.X - 1].isTarget = true;
                                }
                            }
                            else
                            {
                                if (point.X != 1 && point.Y != 6)
                                {
                                    if (board.board[point.Y + 1][point.X - 1].type == TypeOfSegment.Wite && board.board[point.Y + 2][point.X - 2].type == TypeOfSegment.Null)
                                    {
                                        board.board[point.Y + 2][point.X - 2].isTarget = true;
                                        board.board[point.Y + 2][point.X - 2].eatY = point.Y + 1;
                                        board.board[point.Y + 2][point.X - 2].eatX = point.X - 1;
                                        if (board.board[point.Y + 1][point.X - 1].type == TypeOfSegment.Wite && board.board[point.Y + 2][point.X - 2].type == TypeOfSegment.Null)
                                        {
                                            board.board[point.Y + 2][point.X - 2].isTarget = true;
                                            board.board[point.Y + 2][point.X - 2].eatY = point.Y + 1;
                                            board.board[point.Y + 2][point.X - 2].eatX = point.X - 1;
                                            if (point.Y < 4 && point.X > 3)
                                            {
                                                if (board.board[point.Y + 3][point.X - 3].type == TypeOfSegment.Wite && board.board[point.Y + 4][point.X - 4].type == TypeOfSegment.Null)
                                                {
                                                    needOneTurnFor[0] = new Point(point.Y + 2, point.X - 2);
                                                    needOneTurnMore = true;
                                                }
                                                else
                                                {
                                                    //needOneTurnFor[1] = new Point(-1, -1);
                                                    needOneTurnFor[0] = new Point(-1, -1);
                                                }
                                            }
                                            else
                                            {
                                                //needOneTurnFor[1] = new Point(-1, -1);
                                                needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }
                                    }
                                }
                            }
                            if (board.board[point.Y + 1][point.X + 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y + 1][point.X + 1].isTarget = true;
                                }
                            }
                            else
                            
                            if (point.X != 6 && point.Y != 6)
                            {
                                if (board.board[point.Y + 1][point.X + 1].type == TypeOfSegment.Wite && board.board[point.Y + 2][point.X + 2].type == TypeOfSegment.Null)
                                {
                                    board.board[point.Y + 2][point.X + 2].isTarget = true;
                                    board.board[point.Y + 2][point.X + 2].eatY = point.Y + 1;
                                    board.board[point.Y + 2][point.X + 2].eatX = point.X + 1;
                                    if (point.Y < 4 && point.X < 4)
                                    {
                                        if (board.board[point.Y + 3][point.X + 3].type == TypeOfSegment.Wite && board.board[point.Y + 4][point.X + 4].type == TypeOfSegment.Null)
                                        {
                                            needOneTurnFor[1] = new Point(point.Y + 2, point.X + 2);
                                            needOneTurnMore = true;
                                        }
                                        else
                                        {
                                            needOneTurnFor[1] = new Point(-1, -1);
                                            //needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                    else
                                    {
                                        needOneTurnFor[1] = new Point(-1, -1);
                                        //needOneTurnFor[0] = new Point(-1, -1);
                                    }
                                }
                                
                            }
                            break;
                        }
                    }
                }
                else
                {
                    needOneTurnFor[1] = new Point(-1, -1);
                    needOneTurnFor[0] = new Point(-1, -1);
                }
            }
            else
            {
                if (point.Y != 0)
                {
                    if (point.X == 0)
                    {
                        while(true)
                        {
                            if (board.board[point.Y - 1][point.X + 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y - 1][point.X + 1].isTarget = true;
                                }
                                break;
                            }
                            else
                            {
                                if (point.Y != 1)
                                {
                                    if (board.board[point.Y - 1][point.X + 1].type == TypeOfSegment.Black && board.board[point.Y - 2][point.X + 2].type == TypeOfSegment.Null)
                                    {
                                        board.board[point.Y - 2][point.X + 2].isTarget = true;
                                        board.board[point.Y - 2][point.X + 2].eatY = point.Y - 1;
                                        board.board[point.Y - 2][point.X + 2].eatX = point.X + 1;
                                        if (point.Y > 3 && point.X < 4)
                                        {
                                            if (board.board[point.Y - 3][point.X + 3].type == TypeOfSegment.Black && board.board[point.Y - 4][point.X + 4].type == TypeOfSegment.Null)
                                            {
                                                rightToTurnMore = new Point(point.X + 2, point.Y - 2);
                                                needOneTurnFor[1] = new Point(point.Y - 2, point.X + 2);
                                                needOneTurnMore = true;
                                            }
                                            else
                                            {
                                                needOneTurnFor[1] = new Point(-1, -1);
                                                //needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }

                                        else
                                        {
                                            needOneTurnFor[1] = new Point(-1, -1);
                                            //needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (point.X == 7)
                    {
                        while (true)
                        {
                            if (board.board[point.Y - 1][point.X - 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y - 1][point.X - 1].isTarget = true;
                                }
                                break;
                            }
                            else
                            {
                                if (point.Y != 1)
                                {
                                    if (board.board[point.Y - 1][point.X - 1].type == TypeOfSegment.Black && board.board[point.Y - 2][point.X - 2].type == TypeOfSegment.Null)
                                    {
                                        board.board[point.Y - 2][point.X - 2].isTarget = true;
                                        board.board[point.Y - 2][point.X - 2].eatY = point.Y - 1;
                                        board.board[point.Y - 2][point.X - 2].eatX = point.X - 1;
                                        if (point.Y > 3 && point.X > 3)
                                        {
                                            if (board.board[point.Y - 3][point.X - 3].type == TypeOfSegment.Black && board.board[point.Y - 4][point.X - 4].type == TypeOfSegment.Null)
                                            {
                                                leftToTurnMore = new Point(point.X - 2, point.Y - 2);
                                                needOneTurnFor[0] = new Point(point.Y - 2, point.X - 2);
                                                needOneTurnMore = true;
                                            }
                                            else
                                            {
                                                //needOneTurnFor[1] = new Point(-1, -1);
                                                needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }
                                        else
                                        {
                                            //needOneTurnFor[1] = new Point(-1, -1);
                                            needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (board.board[point.Y - 1][point.X - 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y - 1][point.X - 1].isTarget = true;
                                }
                            }
                            else
                            {
                                if (point.X != 1 && point.Y != 1)
                                {
                                    if (board.board[point.Y - 1][point.X - 1].type == TypeOfSegment.Black && board.board[point.Y - 2][point.X - 2].type == TypeOfSegment.Null && point.X != 1)
                                    {
                                        board.board[point.Y - 2][point.X - 2].isTarget = true;
                                        board.board[point.Y - 2][point.X - 2].eatY = point.Y - 1;
                                        board.board[point.Y - 2][point.X - 2].eatX = point.X - 1;
                                        if (point.Y > 3 && point.X > 3)
                                        {
                                            if (board.board[point.Y - 3][point.X - 3].type == TypeOfSegment.Black && board.board[point.Y - 4][point.X - 4].type == TypeOfSegment.Null)
                                            {
                                                leftToTurnMore = new Point(point.X - 2, point.Y - 2);
                                                needOneTurnFor[0] = new Point(point.Y - 2, point.X - 2);
                                                needOneTurnMore = true;
                                            }
                                            else
                                            {
                                               //needOneTurnFor[1] = new Point(-1, -1);
                                                needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }
                                        else
                                        {
                                            //needOneTurnFor[1] = new Point(-1, -1);
                                            needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                }
                            }
                            if (board.board[point.Y - 1][point.X + 1].type == TypeOfSegment.Null)
                            {
                                if (!needOneTurnMore)
                                {
                                    board.board[point.Y - 1][point.X + 1].isTarget = true;
                                }
                            }
                            else
                            {
                                if (point.X != 6 && point.Y != 1)
                                {
                                    if (board.board[point.Y - 1][point.X + 1].type == TypeOfSegment.Black && board.board[point.Y - 2][point.X + 2].type == TypeOfSegment.Null && point.X != 6)
                                    {
                                        board.board[point.Y - 2][point.X + 2].isTarget = true;
                                        board.board[point.Y - 2][point.X + 2].eatY = point.Y - 1;
                                        board.board[point.Y - 2][point.X + 2].eatX = point.X + 1;
                                        if (point.Y > 3 && point.X < 4)
                                        {
                                            if (board.board[point.Y - 3][point.X + 3].type == TypeOfSegment.Black && board.board[point.Y - 4][point.X + 4].type == TypeOfSegment.Null)
                                            {
                                                rightToTurnMore = new Point(point.X + 2, point.Y - 2);
                                                needOneTurnFor[1] = new Point(point.Y - 2, point.X + 2);
                                                needOneTurnMore = true;
                                            }
                                            else
                                            {
                                                needOneTurnFor[1] = new Point(-1, -1);
                                                //needOneTurnFor[0] = new Point(-1, -1);
                                            }
                                        }
                                        else
                                        {
                                            needOneTurnFor[1] = new Point(-1, -1);
                                            //needOneTurnFor[0] = new Point(-1, -1);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            if (needOneTurnFor[0].Y == -1 && needOneTurnFor[0].X == -1 && needOneTurnFor[1].Y == -1 && needOneTurnFor[1].X == -1)
            {
                needOneTurnMore = false;
            }
        }
    }
}
