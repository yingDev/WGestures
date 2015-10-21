using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

                var browsers = BrowserList;
                var selectedBroser = 0;
                for(int i = 0; i<browsers.Count; i++)
                {
                    var b = browsers[i];
                    if(b.Path == _command.UseBrowser)
                    {
                        selectedBroser = i;
                    }
                }
                combo_browsers.DataSource = browsers;
                combo_browsers.SelectedIndex = selectedBroser;

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
            new ComboBoxItem("百度","https://www.baidu.com/s?wd={0}"),
            new ComboBoxItem("必应","https://bing.com/search?q={0}") 
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

        private List<Browser> BrowserList
        {
            get
             {
                var lst = new List<Browser>();
                lst.Add(new Browser { Name = "(系统默认)" });


                RegistryKey browserKeys;
                //on 64bit the browsers are in a different location
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
                if (browserKeys == null)
                    browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");

                string[] browserNames = browserKeys.GetSubKeyNames();

                for (int i = 0; i < browserNames.Length; i++)
                {
                    Browser browser = new Browser();
                    RegistryKey browserKey = browserKeys.OpenSubKey(browserNames[i]);
                    browser.Name = (string)browserKey.GetValue(null);
                    RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                    browser.Path = (string)browserKeyPath.GetValue(null);
                    RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
                    //browser.IconPath = (string)browserIconPath.GetValue(null);
                    lst.Add(browser);
                }

#if DEBUG
                Console.WriteLine("Browsers:");
                foreach(var b in lst)
                {
                    Console.WriteLine("{0}: {1}", b.Name, b.Path);
                }
#endif 

                return lst;
            }
        }
        public struct Browser
        {
            public string Name { get; set; }
            public string Path { get; set; } //null == default
            //public string IconPath { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private void combo_browsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var browser = (Browser) combo_browsers.SelectedValue;
            _command.UseBrowser  = browser.Path;
        }
    }
}
