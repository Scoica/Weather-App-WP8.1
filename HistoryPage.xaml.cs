using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        private Windows.Storage.ApplicationDataContainer localHistory;
        
        public HistoryPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame. && STOCARE PERSISTENTA
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            localHistory = Windows.Storage.ApplicationData.Current.LocalSettings;
            localHistory.Values.Remove("New");
            localHistory.Values.Remove("New1");
            ICollection<string> collection = localHistory.Values.Keys;
            string variable;

            foreach (string item in collection)
            {
                variable = localHistory.Values[item].ToString();
                stackPanel_History.Children.Add(new TextBlock{ Text = variable, FontSize = 19, TextWrapping = TextWrapping.Wrap });
            }
        }
    }
}