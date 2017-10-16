using Cardgame.App.GameLogic;
using Cardgame.App.Rendering;
using Cardgame.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            var simpleGame = kernel.Get<IGameController>();

            form.GameController = simpleGame;

            
            Application.Run(form);

            kernel.Dispose();
        }

        static void InitializeDependencies(IKernel kernel, MainForm form)
        {
            kernel.Bind<FaceCache>().ToSelf().InSingletonScope();
            kernel.Bind<IGameController>().To<SimpleGameController>().InSingletonScope();
            kernel.Bind<GameRenderer>().ToSelf().InSingletonScope();
            kernel.Bind<IGameState>().To<SimpleGameState>().InSingletonScope();
            kernel.Bind<IInteractor>().To<Interactor>().InSingletonScope();
            
            kernel.Bind<IViewport>().ToMethod(context => form);
            kernel.Bind<IMouseInputProxy>().ToMethod(context => form);
        }
    }
}
