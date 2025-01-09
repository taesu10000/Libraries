using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignObject.UserControls.Context
{
    public partial class ContextBase : ToolStripControlHost
    {
        public ContextBase(Control ctrl, string name) : base(ctrl, name)
        {
            InitializeComponent();
            this.Size = ctrl.Size;
        }
        public ContextBase(Control ctrl) : base(ctrl)
        {
            InitializeComponent();
            this.Size = ctrl.Size;
        }
    }
}
