using Cardgame.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cardgame.App
{
    partial class MainForm : Form, IViewport, IMouseInputProxy
    {
        public IGameController GameController { get; set; }

        int IViewport.Width => pictureBoxMain.Width;

        int IViewport.Height => pictureBoxMain.Height;

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            GameController.MeasurementComplete += (s, args) => labelMeasurement.Text = args.Result.ToString();
        }

        public event EventHandler ViewportUpdated;
        protected void OnViewportUpdated()
        {
            ViewportUpdated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<MouseEventArgs> ViewportMouseUp;
        protected void OnViewportMouseUp(MouseEventArgs args)
        {
            ViewportMouseUp?.Invoke(this, args);
        }

        public event EventHandler<MouseEventArgs> ViewportMouseDown;
        protected void OnViewportMouseDown(MouseEventArgs args)
        {
            ViewportMouseDown?.Invoke(this, args);
        }

        public event EventHandler<MouseEventArgs> ViewportMouseMove;
        protected void OnViewportMouseMove(MouseEventArgs args)
        {
            ViewportMouseMove?.Invoke(this, args);
        }

        public event EventHandler ViewportMouseLeave;
        protected void OnViewportMouseLeave()
        {
            ViewportMouseLeave?.Invoke(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameController.Start();
        }

        void IViewport.SetImage(Image image)
        {
            pictureBoxMain.Image = image;
        }

        void IViewport.Invalidate()
        {
            pictureBoxMain.Invalidate();
           // pictureBoxMain.Refresh();
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            OnViewportMouseDown(e);
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBoxMain.Bounds.Contains(e.Location))
            {
                OnViewportMouseMove(e);
            }
        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            OnViewportMouseUp(e);
        }

        private void pictureBoxMain_MouseLeave(object sender, EventArgs e)
        {
            OnViewportMouseLeave();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            OnViewportUpdated();
        }
    }
}
