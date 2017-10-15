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
    partial class MainForm : Form, IViewport
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

        public event EventHandler<ViewportUpdatedEventArgs> ViewportUpdated;
        protected void OnViewportUpdated(ViewportUpdatedEventArgs args)
        {
            ViewportUpdated?.Invoke(this, args);
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
           // pictureBoxMain.Invalidate();
            pictureBoxMain.Refresh();
        }

        private void pictureBoxMain_SizeChanged(object sender, EventArgs e)
        {
            OnViewportUpdated(new ViewportUpdatedEventArgs(pictureBoxMain.Width, pictureBoxMain.Height));
        }
    }
}
