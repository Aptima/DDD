using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace Aptima.Asim.DDD.CommonComponents.ThreeStateButton
{

    public partial class ThreeStateButton : System.Windows.Forms.Panel
    {
        private static Bitmap redImg;
        private static Bitmap greenImg;
        private static Bitmap yellowImg;

        public enum ButtonStates
        { 
            STOPPED = 0,
            INBETWEEN = 1,
            RUNNING = 2,
        }
        private ButtonStates currentState = ButtonStates.STOPPED;

        public ThreeStateButton()
        {
            InitializeComponent();
            
        }
        public ButtonStates CurrentState()
        {
            return currentState;
        }
        public void SetState(ButtonStates newState)
        {
            this.currentState = newState;
            SetColor();
        }
        private void SetColor()
        {
            switch (currentState)
            { 
                case ButtonStates.STOPPED:
                    this.BackgroundImage = redImg;
                    break;
                case ButtonStates.RUNNING:
                    this.BackgroundImage = greenImg;//new Bitmap(greenImg);
                    break;
                case ButtonStates.INBETWEEN:
                    this.BackgroundImage = yellowImg;//new Bitmap((yellowImg);
                    break;
                default:
                    this.BackgroundImage = redImg;//new Bitmap(redImg);
                    break;
            }
        
        }

        public ThreeStateButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.MinimumSize = new Size(15, 15);
            this.MaximumSize = new Size(15, 15);
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(this.GetType());
            redImg = (Bitmap)rm.GetObject("red");
            greenImg = (Bitmap)rm.GetObject("green");
            yellowImg = (Bitmap)rm.GetObject("yellow");
            //redImg = new Bitmap("red.bmp");
            //greenImg = new Bitmap("green.bmp");
            //yellowImg = new Bitmap("yellow.bmp");
            SetColor();
        }
    }
}
