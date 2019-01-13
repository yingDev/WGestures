using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGestures.Core;

namespace WGestures.App.Gui.Model
{
    [Serializable]
    class OrderableIntent : GestureIntent
    {
        public int Order { get; set; }

        
        public OrderableIntent()
        {
            
        }


        public OrderableIntent(GestureIntent from)
        {
            var t = from.GetType();
            foreach (var fieldInf in t.GetFields())
            {
                fieldInf.SetValue(this, fieldInf.GetValue(from));
            }
            foreach (var propInf in t.GetProperties())
            {
                propInf.SetValue(this, propInf.GetValue(from,null),null);
            }
        }
    }
}
