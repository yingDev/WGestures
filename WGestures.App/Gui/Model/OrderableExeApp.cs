using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WGestures.Core;

namespace WGestures.App.Gui.Model
{
    [Serializable]
    internal class OrderableExeApp : ExeApp,IComparable<OrderableExeApp>
    {
        //显示顺序
        public int Order { get; set; }

        public bool Exists
        {
            get { return System.IO.File.Exists(ExecutablePath); }
        }

        public OrderableExeApp()
        {
            
        }

        public OrderableExeApp(ExeApp from)
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


        public int CompareTo(OrderableExeApp other)
        {
            if (other.Order == Order) return 0;
            if (other.Order > Order) return -1;

            return 1;
        }
    }
}
