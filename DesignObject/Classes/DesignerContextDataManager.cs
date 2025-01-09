using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignObject
{
    public class DesignerContextDataManager
    {
        object ItemToInterface { get; set; }
        public ContextMap Map { get; set; }
        public DesignerContextDataManager(ContextMap map)
        {
            Map = map;
        }

        public DesignerContextDataManager(ContextMap map, object itemToInterface) : this(map)
        {
            ItemToInterface = itemToInterface;
        }
        public ToolStripMenuItem CreateContextMenu(string name)
        {
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(name);
            toolStripMenuItem.Click += Tool_Click;
            return toolStripMenuItem;
        }

        private void Tool_Click(object sender, EventArgs e)
        {
            IEnumerable<object> list = ItemToInterface as IEnumerable<object>;
            if (list.Count() > 0)
            {
                var repitem = list.ElementAt(0);
                var repwriteable = repitem as IWritable;
                if (repwriteable != null)
                {
                    var tagProp = new TagProp(repwriteable.Text.String, repwriteable.TagParam);
                    DesignObject.DesignerEvent.OnClickTagProp(tagProp);
                    if (tagProp.Cancel == false)
                    {
                        for (int i = 0; i < list.Count(); i++)
                        {
                            var item = list.ElementAt(i);
                            var writeable = item as IWritable;
                            if (writeable != null)
                            {
                                writeable.Text.String = tagProp.Text;
                                string param = tagProp.TagParam.ToString();
                                if (tagProp.IsSelItemDetail)
                                    param += i.ToString("00");
                                writeable.TagParam = param;
                            }
                        }
                    }
                }

            }
            DesignObject.DesignerEvent.OnInvalidate();
        }
    }
}
