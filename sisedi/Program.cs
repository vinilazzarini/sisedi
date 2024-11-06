using sisedi;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

using sisediview;

namespace sisedi
{
    internal class Program
    {

        static void Main(string[] args)
        {
            menuPrincipal menu = new menuPrincipal();
            menu.exibirMenu();
        }
    }
}

