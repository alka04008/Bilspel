using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bil_spel;
public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D pixel;
    Texture2D YellowBuggy;
    SpriteFont font;

    Rectangle bilkropp = new Rectangle (180,190,75,90);
    Rectangle hjultl = new Rectangle (200,210,10,8);
    Rectangle hjultr = new Rectangle (230,210,10,8);
    Rectangle hjulbl = new Rectangle (200,250,10,8);
    Rectangle hjulbr = new Rectangle (230,250,10,8);

    Rectangle Vägtopp = new Rectangle(795,175,5,5);
    Rectangle Vägbotten = new Rectangle(795,330,5,5);
    Rectangle Vägstartup = new Rectangle (0,175,800,5);
    Rectangle Vägstartner = new Rectangle (0,330,800,5);
    Rectangle textlinje = new Rectangle (0,28,800,2);
   
    int lifehjultl =1; int lifehjultr=1; int lifehjulbl=1; int lifehjulbr=1;
    int CarSpeedY = 5;
    string Speed = "SLOW";
    float Points = 0f;
    float timeSinceLastRoad = 0f;
    bool gameend = false;
    bool Gamepaused = false;

    List<Rectangle> väglista = new List<Rectangle>();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        pixel = Content.Load<Texture2D>("Pixel 2");
        YellowBuggy = Content.Load<Texture2D>("YellowBuggy_1");
        font = Content.Load<SpriteFont>("File");

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
         KeyboardState kstate = Keyboard.GetState();

        if(kstate.IsKeyDown(Keys.P))
            Gamepaused = true;
        if(kstate.IsKeyDown(Keys.R))
            Gamepaused = false;

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (!Gamepaused)
        {
            if(kstate.IsKeyDown(Keys.W) && bilkropp.Y - hjultl.Height >=5)
            {
                bilkropp.Y -= CarSpeedY;
                hjultl.Y -= CarSpeedY;
                hjultr.Y -= CarSpeedY;
                hjulbl.Y -= CarSpeedY;
                hjulbr.Y -= CarSpeedY;
            }
            if(kstate.IsKeyDown(Keys.S) && bilkropp.Y + bilkropp.Height + hjulbl.Height <=480)
            {
                bilkropp.Y += CarSpeedY;
                hjultl.Y += CarSpeedY;
                hjultr.Y += CarSpeedY;
                hjulbl.Y += CarSpeedY;
                hjulbr.Y += CarSpeedY;
            }   
            
        
            // TODO: Add your update logic here

            base.Update(gameTime);

        
            if(gameend == false)
                Points += (float)gameTime.TotalGameTime.TotalSeconds;

            if (Points >= 500 && Points <= 1000)
                Speed = "MEDIUM";
            else if (Points >=1000 && Points <=1500)
                Speed = "FAST";
            else if (Points >= 1500)
                Speed = "VERY FAST";

            timeSinceLastRoad += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastRoad > 0.05f)
            {   

                int tal = 0;

                if(Speed == "SLOW")
                {
                    Random rnd = new Random ();
                    tal = rnd.Next(1,20);
                }
                else if(Speed == "MEDIUM")
                {
                    Random rnd = new Random ();
                    tal = rnd.Next(1,15);
                }

                else if(Speed == "FAST")
                {
                    Random rnd = new Random ();
                    tal = rnd.Next(1,10);
                }

                else if(Speed == "VERY FAST")
                {
                    Random rnd = new Random ();
                    tal = rnd.Next(1,5);
                }

                if(tal == 1 && Vägtopp.Y >=25)
                {
                    Vägtopp.Y -= 10;
                    Vägbotten.Y -= 10;
                    timeSinceLastRoad= 0;
                    väglista.Add(new Rectangle(800,Vägtopp.Y-5,30,10));
                    väglista.Add(new Rectangle(800, Vägbotten.Y-5,30,10));
                }
                else if (tal == 2 && Vägbotten.Y <= 475)
                {
                    Vägtopp.Y += 10;
                    Vägbotten.Y += 10;
                    timeSinceLastRoad = 0;
                    väglista.Add(new Rectangle(800,Vägtopp.Y+5,30,10));
                    väglista.Add(new Rectangle(800, Vägbotten.Y+5,30,10));
                }

                väglista.Add(new Rectangle(800,Vägtopp.Y,15,10));
                väglista.Add(new Rectangle(800, Vägbotten.Y,15,10));
                }

                var item = väglista.ToArray();
                for(int i = 0; väglista.Count>i;i++)
                {
                    item[i].X -= 5;
                    if (väglista[i].X < -5)
                    {
                        väglista.RemoveAt(i);
                        i--;
                    }
                }
                väglista = new List<Rectangle>(item);

            if (Vägstartup.X >= -900)
                {
                Vägstartup.X -= 5;
                Vägstartner.X -= 5;
                }

            if(hjultl.Intersects(Vägstartup) || hjultl.Intersects(Vägstartner))
                lifehjultl =- 1;
            if(hjultr.Intersects(Vägstartup) || hjultr.Intersects(Vägstartner))
                lifehjultr =- 1;
            if(hjulbl.Intersects(Vägstartup) || hjulbl.Intersects(Vägstartner))
                lifehjulbl =- 1;
            if(hjulbr.Intersects(Vägstartup) || hjulbr.Intersects(Vägstartner))
                lifehjulbr =- 1;
            
                var item1 = väglista.ToArray();
                for(int i = 0; väglista.Count>i;i++)
                {
                    if(item[i].Intersects(hjultl))
                        lifehjultl-=1;
                    else if (item[i].Intersects(hjultr))
                        lifehjultr-=1;
                    else if (item[i].Intersects(hjulbl))
                        lifehjulbl-=1;
                    else if (item[i].Intersects(hjulbr))
                        lifehjulbr-=1;
                }
                väglista = new List<Rectangle>(item);

            if (lifehjultl !=1 && bilkropp.Y-hjultl.Height >=5)
                {
                    bilkropp.Y -=1;
                    hjultl.Y -=1;
                    hjultr.Y -=1;
                    hjulbl.Y -=1;
                    hjulbr.Y -=1;
                }
            if (lifehjultr !=1 && bilkropp.Y-hjultl.Height >=5)
                {
                    bilkropp.Y -=1;
                    hjultl.Y -=1;
                    hjultr.Y -=1;
                    hjulbl.Y -=1;
                    hjulbr.Y -=1;
                }
            if (lifehjulbl !=1 && bilkropp.Y+bilkropp.Height + hjulbl.Height <=480)
                {
                    bilkropp.Y +=1;
                    hjultl.Y +=1;
                    hjultr.Y +=1;
                    hjulbl.Y +=1;
                    hjulbr.Y +=1;
                }
            if (lifehjulbr !=1 && bilkropp.Y+bilkropp.Height +hjulbl.Height <=480)
                {
                    bilkropp.Y +=1;
                    hjultl.Y +=1;
                    hjultr.Y +=1;
                    hjulbl.Y +=1;
                    hjulbr.Y +=1;
                }

            if (lifehjultl !=1 && lifehjultr !=1)
                gameend = true;       
            else if ( lifehjulbl !=1 && lifehjulbr !=1)
                gameend = true;
            // funkar ej Points += GameTime.ElapsedGameTime.TotalSecounds;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        if(gameend == false)
        {
            if (Gamepaused) 
            {
                _spriteBatch.DrawString(font, "Game Paused", new Vector2(300,240), Color.GreenYellow);
                _spriteBatch.Draw(pixel, textlinje, Color.DimGray);
                _spriteBatch.DrawString(font, "SPEED:  " + Speed, new Vector2(20,10), Color.GreenYellow);
                _spriteBatch.DrawString(font, "POINTS: " + Convert.ToInt32(Points), new Vector2(640,10), Color.GreenYellow);
            }
            else 
            {
                _spriteBatch.Draw(YellowBuggy, bilkropp, Color.White);
                if(lifehjultl==1)
                    _spriteBatch.Draw(pixel, hjultl, Color.DarkGray);
                else
                _spriteBatch.Draw(pixel, hjultl, Color.Red);

                if(lifehjultr==1)
                    _spriteBatch.Draw(pixel, hjultr, Color.DarkGray);
                else
                    _spriteBatch.Draw(pixel, hjultr, Color.Red);

                if(lifehjulbl==1)
                    _spriteBatch.Draw(pixel, hjulbl, Color.DarkGray);
                else
                    _spriteBatch.Draw(pixel, hjulbl, Color.Red);

                if(lifehjulbr==1)
                    _spriteBatch.Draw(pixel, hjulbr, Color.DarkGray);
                else
                    _spriteBatch.Draw(pixel, hjulbr, Color.Red);

                _spriteBatch.Draw(pixel, Vägtopp, Color.LightSlateGray);
                _spriteBatch.Draw(pixel, Vägbotten, Color.LightSlateGray);
                _spriteBatch.Draw(pixel, Vägstartup, Color.LightSlateGray);
                _spriteBatch.Draw(pixel, Vägstartner, Color.LightSlateGray);
                
                foreach(Rectangle element in väglista)
                {
                    _spriteBatch.Draw(pixel,element, Color.LightSlateGray);
                }

                _spriteBatch.Draw(pixel, textlinje, Color.DimGray);
                _spriteBatch.DrawString(font, "SPEED:  " + Speed, new Vector2(20,10), Color.GreenYellow);
                _spriteBatch.DrawString(font, "POINTS: " + Convert.ToInt32(Points), new Vector2(640,10), Color.GreenYellow);
            }
        }
       
        else
        {
            _spriteBatch.DrawString(font, "YOUR FINAL SCORE: " + Convert.ToInt32(Points), new Vector2(300,240), Color.GreenYellow);
            _spriteBatch.DrawString(font, "SPEED:  " + Speed, new Vector2(300,260), Color.GreenYellow);
        }
            _spriteBatch.End();
            base.Draw(gameTime);
    }
}