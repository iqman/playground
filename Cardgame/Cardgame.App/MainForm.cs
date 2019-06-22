using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Cardgame.App.Games;

namespace Cardgame.App
{
    partial class MainForm : Form, IViewport, IMouseInputProxy
    {
        private Point mouseDownLocation;
        private Point dragThreshold;
        const int SM_CXDRAG = 68;
        const int SM_CYDRAG = 69;
        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int index);

        public IGame Game { get; set; }

        public MainForm()
        {
            InitializeComponent();

            dragThreshold = new Point(GetSystemMetrics(SM_CXDRAG), GetSystemMetrics(SM_CYDRAG));
        }

        int IViewport.Width => ClientSize.Width;
        int IViewport.Height => ClientSize.Height;

        private void MainForm_Load(object sender, EventArgs e)
        {
            Game.Start();
        }

        public event EventHandler ViewportUpdated;
        protected void OnViewportUpdated()
        {
            ViewportUpdated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<MouseEventArgs> ViewPortMouseClick;
        protected void OnViewPortMouseClick(MouseEventArgs e)
        {
            ViewPortMouseClick?.Invoke(this, e);
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
            mouseDownLocation = e.Location;
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

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (IsWithinDragThreshold(e.Location))
            {
                OnViewPortMouseClick(e);
            }
        }

        private bool IsWithinDragThreshold(Point p)
        {
            return mouseDownLocation.X > p.X - dragThreshold.X && mouseDownLocation.X < p.X + dragThreshold.X &&
                mouseDownLocation.Y > p.Y - dragThreshold.Y && mouseDownLocation.Y < p.Y + dragThreshold.Y;
        }
    }
}
