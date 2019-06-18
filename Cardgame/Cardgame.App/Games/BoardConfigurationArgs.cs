using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardgame.App.Games
{
    class BoardConfigurationArgs
    {
        public BoardConfiguration Config { get; }

        public BoardConfigurationArgs(BoardConfiguration config)
        {
            Config = config;
        }
    }
}
