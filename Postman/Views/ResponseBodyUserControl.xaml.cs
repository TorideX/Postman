using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Postman.Views
{
    /// <summary>
    /// Interaction logic for ResponseBodyUserControl.xaml
    /// </summary>
    public partial class ResponseBodyUserControl : UserControl
    {
        public ResponseBodyUserControl()
        {
            InitializeComponent();
            viewGrid.Children.Add(new ResponseBodyRawUserControl());
        }

        private void ResponseMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == rawBtn)
            {
                if (viewGrid.Children.Count > 0 && viewGrid.Children[0] is ResponseBodyRawUserControl) return;
                viewGrid.Children.Clear();
                viewGrid.Children.Add(new ResponseBodyRawUserControl());
            }
            else if (button == previewBtn)
            {
                if (viewGrid.Children.Count > 0 && viewGrid.Children[0] is ResponseBodyPreviewUserControl) return;
                viewGrid.Children.Clear();
                viewGrid.Children.Add(new ResponseBodyPreviewUserControl());
            }
        }
    }
}
