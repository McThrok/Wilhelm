﻿using System;
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
    public partial class TaskDetailsControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<GroupHolder> _availableGroupsToAdd;
        private TaskHolder _shownTask;

        public TaskDetailsControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        public void Initialize(TaskHolder choosenTask, List<GroupHolder> groups)
        {
            if (choosenTask == null)
            {
                TaskDetailsPanel.Visibility = Visibility.Hidden;
                return;
            }

            TaskDetailsPanel.Visibility = Visibility.Visible;
            _availableGroupsToAdd = new List<GroupHolder>();

            ShownTask = new TaskHolder()
            {
                Id = choosenTask.Id,
                Name = choosenTask.Name,
                Description = choosenTask.Description,
                StartDate = choosenTask.StartDate,
                Frequency = choosenTask.Frequency,
                Archivized = choosenTask.Archivized,
                Groups = new ObservableCollection<GroupHolder>(),
            };

            foreach (var group in groups)
            {
                var newGroup = new GroupHolder
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    Archivized = group.Archivized,
                    Tasks = new ObservableCollection<TaskHolder>(),
                };

                if (group.Tasks.Contains(choosenTask))
                {
                    newGroup.Tasks.Add(ShownTask);
                    ShownTask.Groups.Add(newGroup);
                }
                else
                {
                    _availableGroupsToAdd.Add(newGroup);
                }
            }

        }

        private void AssignToGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ChooseItemWindow(_availableGroupsToAdd.Cast<NamedHolder>().ToList());
            dialog.ShowDialog();

            if (dialog.SelectedHolder is GroupHolder groupToAdd)
            {
                _availableGroupsToAdd.Remove(groupToAdd);
                ShownTask.Groups.Add(groupToAdd);
                groupToAdd.Tasks.Add(ShownTask);
            }
        }

        private void RemoveFromGroup_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var group = button.Tag as GroupHolder;
            group.Tasks.Remove(ShownTask);
            ShownTask.Groups.Remove(group);
            _availableGroupsToAdd.Add(group);
        }

        public TaskHolder ShownTask
        {
            get
            {
                return _shownTask;
            }
            set
            {
                _shownTask = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShownTask)));
            }
        }

    }
}
