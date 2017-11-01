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
using System.Windows.Shapes;
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Windows
{
    /// <summary>
    /// Interaction logic for ChooseGroupWindow.xaml
    /// </summary>
    public partial class ChooseItemWindow : Window
    {
        public NamedHolder SelectedHolder { get; private set; }
        private List<NamedHolder> holders;
        public ChooseItemWindow(List<NamedHolder> groups)
        {
            InitializeComponent();
            holders = groups;
            HoldersListView.ItemsSource = holders;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedHolder = holders[HoldersListView.SelectedIndex];
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedHolder = null;
            Close();
        }
    }
}
