using Cardgame.App.Rendering;
using Cardgame.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cardgame.App.Games;
using Cardgame.App.Games.Klondike;
using Cardgame.App.Games.Simple;

namespace Cardgame.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var kernel = new StandardKernel();

            var form = new MainForm();

            InitializeDependencies(kernel, form);

            var simpleGame = kernel.Get<IGame>();
            var renderer = kernel.Get<GameRenderer>();

            form.Game = simpleGame;

            
            Application.Run(form);

            kernel.Dispose();
        }

        static void InitializeDependencies(IKernel kernel, MainForm form)
        {
            kernel.Bind<FaceCache>().ToSelf().InSingletonScope();
            kernel.Bind<IGame>().To<Simple>().InSingletonScope();
            kernel.Bind<GameRenderer>().ToSelf().InSingletonScope();
            kernel.Bind<IGameState>().To<SimpleGameState>().InSingletonScope();
            kernel.Bind<IInteractor>().To<Interactor>().InSingletonScope();
            kernel.Bind<ICardShuffler>().To<CardShuffler>().InSingletonScope();

            kernel.Bind<IViewport>().ToMethod(context => form);
            kernel.Bind<IMouseInputProxy>().ToMethod(context => form);
        }
    }
}
