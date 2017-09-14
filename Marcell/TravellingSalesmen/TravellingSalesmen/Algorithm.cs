using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmen
{
    interface Algorithm
    {
        string GetName();

        void Initialize();

        void NextTurn();

    }
}
