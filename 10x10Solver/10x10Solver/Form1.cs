using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _10x10Solver.Bricks;
using _10x10Solver.Solvers;

namespace _10x10Solver
{
    public partial class Form1 : Form
    {
        private Board b;
        private BoardRenderer br;
        private NextBricksSet nb;
        private ISolver solver;
        private int brickCount;

        public Form1()
        {
            InitializeComponent();

            nb = new NextBricksSet();
            b = new Board();
            b.Seed();
            br = new BoardRenderer(b, nb, this.pictureBoxBoard.Size);
            pictureBoxBoard.Image = br.RenderingImage;

            br.RenderingComplete += () => pictureBoxBoard.Invalidate();
            br.RenderBoard();

            solver = SolverFactory.CreateSolver(b, nb);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            b.Clear();
            brickCount = 0;
            try
            {
                while(true)
                {
                    solver.Solve();
                    pictureBoxBoard.Refresh();
                    nb.GenerateNextBricks();
                    brickCount++;
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Died at brick {0} with score of {1}", brickCount, b.Score));
            }
        }

        private void buttonStep_Click(object sender, EventArgs e)
        {
            try
            {
                solver.Solve();
                pictureBoxBoard.Refresh();
                nb.GenerateNextBricks();
                brickCount++;
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Died at brick {0} with score of {1}", brickCount, b.Score));
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            b.Clear();
            brickCount = 0;
            pictureBoxBoard.Refresh();
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBoxBoard.Image = null;
            br.UpdateViewportSize(pictureBoxBoard.Size);
            pictureBoxBoard.Image = br.RenderingImage;
        }

        private void pictureBoxBoard_MouseDown(object sender, MouseEventArgs e)
        {
            var p = new Point(e.X, e.Y);
            if (br.IsInsideBoard(p))
            {
                var bc = br.ToBoardCoordinates(p);

                b.PutBrick(new Brick_2X2(), bc);
                br.RenderBoard();
            }

            //if (br.IsInsideNextBrickArea(p))
            //{
            //    var index = br.NextBrickAreaIndex(p);
            //    br.StartDraggingNextBrick(index);
            //}
        }

        private void pictureBoxBoard_MouseUp(object sender, MouseEventArgs e)
        {
            var p = new Point(e.X, e.Y);
        }

        private void pictureBoxBoard_MouseMove(object sender, MouseEventArgs e)
        {
            var p = new Point(e.X, e.Y);
        }

        private void buttonWeighting_Click(object sender, EventArgs e)
        {
            const float stepSize = .25f;
            const float min = 0;
            const float max = 1;
            int variableCount = Enum.GetNames(typeof (ScoreComponent)).Length;
            const int runsPerConfig = 4;

            int stepsPerVariable = (int)Math.Round((max - min)/stepSize);
            int totalRuns = (int)Math.Pow(stepsPerVariable, variableCount) * runsPerConfig;
            progressBarWeigting.Maximum = totalRuns + 1;
            var progress = 0;

            var configs = new List<Tuple<int, Dictionary<ScoreComponent, float>>>();
            
            for (float cohesion = min; cohesion < max; cohesion += stepSize)
            {
                for (float holes = min; holes < max; holes += stepSize)
                {
                    for (float boardFill = min; boardFill < max; boardFill += stepSize)
                    {
                        for (float islands = min; islands < max; islands += stepSize)
                        {
                            for (float rowColumnBuilding = min; rowColumnBuilding < max; rowColumnBuilding += stepSize)
                            {
                                for (int i = 0; i < runsPerConfig; i++)
                                {
                                    b.Clear();
                                    brickCount = 0;

                                    var config = new Dictionary<ScoreComponent, float>
                                    {
                                        {ScoreComponent.Cohesion, cohesion},
                                        {ScoreComponent.Holes, holes},
                                        {ScoreComponent.BoardFill, boardFill},
                                        {ScoreComponent.Islands, islands},
                                        {ScoreComponent.RowColumnBuilding, rowColumnBuilding},
                                    };

                                    try
                                    {
                                        while (true)
                                        {
                                            solver.Solve();
                                            //pictureBoxBoard.Refresh();
                                            nb.GenerateNextBricks();
                                            brickCount++;
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        progress++;
                                        if (progressBarWeigting.Maximum <= progress)
                                        {
                                            progressBarWeigting.Maximum = progress;
                                            progressBarWeigting.Value = progress;
                                        }
                                        
                                        configs.Add(Tuple.Create(brickCount, config));
                                        listBoxWeighting.Items.Add(PrintConfig(config, brickCount));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var bestConfigs = configs.OrderByDescending(x => x.Item1).Take(10)
                                .Select(y => PrintConfig(y.Item2, y.Item1)).ToArray();
            MessageBox.Show(string.Join(Environment.NewLine, bestConfigs), "Done");
        }

        private string PrintConfig(IDictionary<ScoreComponent, float> config, int brickCount)
        {
            return string.Format("{0},{1},{2},{3},{4} = {5}",
                config[ScoreComponent.Cohesion],
                config[ScoreComponent.Holes],
                config[ScoreComponent.BoardFill],
                config[ScoreComponent.Islands],
                config[ScoreComponent.RowColumnBuilding],
                brickCount);
        }
    }
}
