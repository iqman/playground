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
    partial class MainForm : Form, IRenderTargetHolder
    {
        public IGameController GameController { get; set; }
        Image IRenderTargetHolder.Target { get => pictureBoxMain.Image; set => pictureBoxMain.Image = value; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameController.Start();
            pictureBoxMain.Invalidate();
        }
    }
}
