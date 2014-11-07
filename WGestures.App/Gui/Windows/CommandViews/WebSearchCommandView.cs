using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.Core.Commands;
using WGestures.Core.Commands.Impl;

namespace WGestures.App.Gui.Windows.CommandViews
{
    public partial class WebSearchCommandView : CommandViewUserControl
    {
        private WebSearchCommand _command;

        public override AbstractCommand Command
        {
            get { return _command; }
            set
            {
                _command = (WebSearchCommand) value;

                foreach (var s in combo_searchEngines.Items)
                {
                    var item = s as ComboBoxItem;
                    if (item == null)
                    {
                        combo_searchEngines.SelectedItem = s;
                        panel_customSearchEngine.Visible = true;                        
                        tb_url.Text = _command.SearchEngineUrl ?? defaultSearchEngines[0].Url;

                        break;
                    }

                    if (item.Url == _command.SearchEngineUrl)
                    {
                        combo_searchEngines.SelectedItem = item;                
                        panel_customSearchEngine.Visible = false;

                        break;
                    }
                }
            }
        }

        public WebSearchCommandView()
        {
            InitializeComponent();
            combo_searchEngines.Items.AddRange(defaultSearchEngines);
            combo_searchEngines.Items.Add("自定义");

        }

        private static ComboBoxItem[] defaultSearchEngines =
        {
            new ComboBoxItem("Google", "https://www.google.com/search?q={0}"),
            new ComboBoxItem("百度","http://www.baidu.com/#wd={0}"),
            new ComboBoxItem("必应","http://bing.com/search?q={0}") 
        };

        private class ComboBoxItem
        {
            public ComboBoxItem(string title, string url)
            {
                Title = title;
                Url = url;
            }

            public string Url { get; set; }
            public string Title { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }

        private void combo_searchEngines_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = combo_searchEngines.SelectedItem as ComboBoxItem;
            if (item == null)
            {
                panel_customSearchEngine.Visible = true;
                tb_url.Text = _command.SearchEngineUrl;
                _command.SearchEngingName = "自定义";
            }
            else
            {
                panel_customSearchEngine.Visible = false;
                _command.SearchEngineUrl = item.Url;
                _command.SearchEngingName = item.Title;
            }

            OnCommandValueChanged();
        }

        private void tb_url_TextChanged(object sender, EventArgs e)
        {
            _command.SearchEngineUrl = tb_url.Text;
        }
    }
}
