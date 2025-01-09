using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common.Vision.Insepction;

namespace Common.Vision.Cognex
{
    public interface ICognexInspection : IDominoInspection
    {
        Control GetToolBlockControl();
        void LoadVppFromFile(string fileName);

        event EventHandler<string> OnVppFromFileLoaded;

    }
}
