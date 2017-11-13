using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Windows;

namespace Wilhelm.Frontend.Controls
{
    /// <summary>
    /// Interaction logic for TaskDetailsControl.xaml
    /// </summary>
    public partial class GroupDetailsControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<TaskHolder> _tasks;
        private GroupHolder _showGroup;

        public GroupDetailsControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        public void Initialize(GroupHolder chooosenGroup, List<TaskHolder> tasks)
        {
            if (chooosenGroup == null)
            {
                GroupDetailsPanel.Visibility = Visibility.Hidden;
                return;
            }

            GroupDetailsPanel.Visibility = Visibility.Visible;

            ShownGroup = new GroupHolder()
            {
                Id = chooosenGroup.Id,
                Name = chooosenGroup.Name,
                Description = chooosenGroup.Description,
                Archivized = chooosenGroup.Archivized,
            };

            Tasks = new ObservableCollection<TaskHolder>();
            foreach (var task in tasks)
            {
                var newTask = new TaskHolder
                {
                    Name = task.Name,
                    Description = task.Description,
                    Archivized = task.Archivized
                };
                Tasks.Add(newTask);

                if (task.Groups.Contains(chooosenGroup))
                {
                    newTask.Groups.Add(ShownGroup);
                    ShownGroup.Tasks.Add(newTask);
                }
            }

        }

        private void AssignTask_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ChooseItemWindow(Tasks.Where(x => !x.Groups.Contains(ShownGroup)).Cast<NamedHolder>().ToList());
            dialog.ShowDialog();
            var taskToAdd = dialog.SelectedHolder as TaskHolder;
            if (taskToAdd != null)
            {
                ShownGroup.Tasks.Add(taskToAdd);
                taskToAdd.Groups.Add(ShownGroup);
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button.Tag as TaskHolder;
            task.Groups.Remove(ShownGroup);
            ShownGroup.Tasks.Remove(task);
        }

        public GroupHolder ShownGroup
        {
            get
            {
                return _showGroup;
            }
            set
            {
                _showGroup = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShownGroup)));
            }
        }
        public ObservableCollection<TaskHolder> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tasks)));
            }
        }
    }
}
