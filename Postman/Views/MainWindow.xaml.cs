using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using GalaSoft;
using Postman.ViewModel;
using Postman.Views;

namespace Postman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            requestGrid.Children.Add(new RequestParamsUserControl());
            responseGrid.Children.Add(new ResponseBodyUserControl());
        }

        private void RequestMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if(button == requestParamsBtn)
            {
                if (requestGrid.Children[0] is RequestParamsUserControl) return;

                requestParamsBtn.BorderThickness = new Thickness(0, 0, 0, 3);
                requestHeadersBtn.BorderThickness = new Thickness(0);
                requestBodyBtn.BorderThickness = new Thickness(0);

                requestGrid.Children.Clear();
                requestGrid.Children.Add(new RequestParamsUserControl());
            }
            else if(button == requestHeadersBtn)
            {
                if (requestGrid.Children[0] is RequestHeadersUserControl) return;

                requestHeadersBtn.BorderThickness = new Thickness(0, 0, 0, 3);
                requestParamsBtn.BorderThickness = new Thickness(0);
                requestBodyBtn.BorderThickness = new Thickness(0);

                requestGrid.Children.Clear();
                requestGrid.Children.Add(new RequestHeadersUserControl());
            }
            else if(button == requestBodyBtn)
            {
                if (requestGrid.Children[0] is RequestBodyUserControl) return;

                requestBodyBtn.BorderThickness = new Thickness(0, 0, 0, 3);
                requestParamsBtn.BorderThickness = new Thickness(0);
                requestHeadersBtn.BorderThickness = new Thickness(0);

                requestGrid.Children.Clear();
                requestGrid.Children.Add(new RequestBodyUserControl());
            }
        }
        private void ResponseMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == responseBodyBtn)
            {
                if (responseGrid.Children.Count > 0 && responseGrid.Children[0] is ResponseBodyUserControl) return;
                responseBodyBtn.BorderThickness = new Thickness(0, 0, 0, 3);
                responseHeadersBtn.BorderThickness = new Thickness(0);

                responseGrid.Children.Clear();
                responseGrid.Children.Add(new ResponseBodyUserControl());
            }
            else if(button == responseHeadersBtn)
            {
                if (responseGrid.Children.Count > 0 && responseGrid.Children[0] is ResponseHeadersUserControl) return;
                responseHeadersBtn.BorderThickness = new Thickness(0, 0, 0, 3);
                responseBodyBtn.BorderThickness = new Thickness(0);

                responseGrid.Children.Clear();
                responseGrid.Children.Add(new ResponseHeadersUserControl());
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
