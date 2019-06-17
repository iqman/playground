using System;
using System.Windows.Forms;
using Cardgame.App.Games;

namespace Cardgame.App
{
    partial class MainForm : Form, IViewport, IMouseInputProxy
    {
        public IGame Game { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            Game.Start();
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

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            OnViewportMouseDown(e);
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            OnViewportMouseMove(e);
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
