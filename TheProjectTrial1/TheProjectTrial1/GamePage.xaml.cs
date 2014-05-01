using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net.Browser;


namespace TheProjectTrial1
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        Texture2D background;
        Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
       
        Texture2D LeftHand;
        Texture2D RightHand;
        float RotationAngle = 0f;
        Vector2 position;
        int count=0;
        int ButtonPressedState=0;
        int ActionState=0;
        int check;
        Texture2D rock;
        Texture2D scissors;
        Texture2D paper;
        Texture2D LeftHandPaper;
        Texture2D LeftHandScissors;
        static int height=GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        static int width=GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        Microsoft.Xna.Framework.Rectangle ScreenRect = new Microsoft.Xna.Framework.Rectangle(0,0,height,width);
        Microsoft.Xna.Framework.Rectangle LeftHandRect = new Microsoft.Xna.Framework.Rectangle(-200,170,450,450);
        Microsoft.Xna.Framework.Rectangle RightHandRect = new Microsoft.Xna.Framework.Rectangle(475,95,450,450);
        Microsoft.Xna.Framework.Rectangle rockrect = new Microsoft.Xna.Framework.Rectangle(300,50,60,60);
        Microsoft.Xna.Framework.Rectangle paperrect = new Microsoft.Xna.Framework.Rectangle(360, 50, 60, 60);
        Microsoft.Xna.Framework.Rectangle scissorsrect = new Microsoft.Xna.Framework.Rectangle(420, 50, 60, 60);
        SocketAsyncEventArgs f = new SocketAsyncEventArgs();
        
        Vector2 origin;
        float RotationAngle2 = 0f;
        GestureSample gesture;

        public GamePage()
        {
            InitializeComponent();
            
            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            TouchPanel.EnabledGestures = GestureType.Tap;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
            origin.X = -200;
            origin.Y = 150;
            position.X = -425;
            position.Y = 155;
            //var url = "http://pusitahw.herobo.com/hg.php?value=10";
            //HttpWebRequest web = (HttpWebRequest)WebRequest.Create(url);
            //web.Method = "POST";
            //web.ContentType = "application/x-www-form-urlencoded";
            //web.BeginGetResponse(new AsyncCallback(Requestcallback), web);
        }
       

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            background = contentManager.Load<Texture2D>("stage2");
            LeftHand =contentManager.Load<Texture2D>("lefthand");
            RightHand = contentManager.Load<Texture2D>("righthand");
            rock = contentManager.Load<Texture2D>("rock1");
            scissors = contentManager.Load<Texture2D>("scissors");
            paper = contentManager.Load<Texture2D>("paper");
            LeftHandPaper = contentManager.Load<Texture2D>("lefthandpaper");
            LeftHandScissors = contentManager.Load<Texture2D>("lefthandscissors");
            int x = 150;
            int y = 250;
            // TODO: use this.content to load your game content here

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            //bool g=soc.ReceiveFromAsync(f);
            
            if (TouchPanel.IsGestureAvailable == true)
            {
                gesture = TouchPanel.ReadGesture();
                buttonAction();
            }
            if ((ButtonPressedState == 1 || ButtonPressedState == 2 || ButtonPressedState == 3)&&check==0)
            {
                
                RotationAngle2 = RotationAngle2 + 1f;
                if (RotationAngle2 < 15f)
                {
                    RotationAngle = RotationAngle2 % 15f;
                    RotationAngle = -RotationAngle * (MathHelper.Pi) / 180;
                }
                else if (RotationAngle2 >= 15f && RotationAngle2 < 30f)
                {
                    RotationAngle = 15f - (RotationAngle2 % 15f);
                    RotationAngle = -RotationAngle * (MathHelper.Pi) / 180;

                }
                else if (RotationAngle2 == 30f)
                    RotationAngle = 0;
                else if (RotationAngle2 > 30 && RotationAngle2 < 45)
                {
                    RotationAngle = RotationAngle2 % 30f;
                    RotationAngle = -RotationAngle * (MathHelper.Pi) / 180;
                }
                else if (RotationAngle2 >= 45 && RotationAngle2 < 60)
                {
                    RotationAngle = 15f - (RotationAngle2 % 45f);
                    RotationAngle = -RotationAngle * (MathHelper.Pi) / 180;
                }
                else if (RotationAngle2 == 60)
                    RotationAngle = 0;
                else if (RotationAngle2 > 60 && RotationAngle2 <= 75)
                {
                    RotationAngle = RotationAngle2 % 60f;
                    RotationAngle = -RotationAngle * (MathHelper.Pi) / 180;
                }
                else if (RotationAngle2 > 75 && RotationAngle2 < 90)
                {
                    RotationAngle = 15f - (RotationAngle2 % 75f);
                    RotationAngle = -RotationAngle * (MathHelper.Pi) / 180;
                }
                else if (RotationAngle2 == 90)
                {
                    RotationAngle = 0;
                    RotationAngle2 = 0;
                    check = 1;
                   
                    count++;
                }


            }
            if (ButtonPressedState == 1 &&count!=0)
            {


                ActionState = 1;
                count = 0;


            }
            else if (ButtonPressedState == 2 && count != 0)
            {
                ActionState = 2;
                count = 0;
            }
            else if (ButtonPressedState == 3 && count != 0)
            {
                ActionState = 3;
                count = 0;
            }
            OnDraw(this, e);

            // TODO: Add your update logic here
        }
        private void buttonAction()
        
        {
            if (gesture.Position.X > 300 && gesture.Position.X < 360)
            {
                if (gesture.Position.Y > 50 && gesture.Position.Y < 110)
                {
                    ButtonPressedState = 1;
                    check = 0;
                    ActionState = 0;
                }
            }
            else if (gesture.Position.X > 360 && gesture.Position.X < 420)
            {
                if (gesture.Position.Y > 50 && gesture.Position.Y < 110)
                {
                    ButtonPressedState = 2;
                    check = 0;
                    ActionState = 0;
                }
               
            }
            else if (gesture.Position.X > 420 && gesture.Position.X < 480)
            {
                if (gesture.Position.Y > 50 && gesture.Position.Y < 110)
                {
                    ButtonPressedState = 3;
                    check = 0;
                    ActionState = 0;
                }

            }
            
            
        }


        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(background, ScreenRect, Color.White);
            spriteBatch.Draw(rock, rockrect, Color.White);
            spriteBatch.Draw(scissors, scissorsrect, Color.White);
            spriteBatch.Draw(paper, paperrect, Color.White);
            spriteBatch.Draw(RightHand, RightHandRect, Color.White);
            
            if (ActionState == 0||ActionState==1)
            {
                
                spriteBatch.Draw(LeftHand, position, null, Color.White, RotationAngle, origin, 1f, SpriteEffects.None, 0f);
            }
            else if (ActionState == 2)
            {
                spriteBatch.Draw(LeftHandPaper, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None,0f);
                
            }
            else if (ActionState == 3)
            {
                spriteBatch.Draw(LeftHandScissors, position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            // TODO: Add your drawing code here
        }
    }
}