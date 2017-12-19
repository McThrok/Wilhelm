using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Support;
using Wilhelm.Frontend.ViewModels.Windows;
using Wilhelm.Frontend.Views.Windows;

namespace Wilhelm.Frontend.ViewModels.Controls
{
    public class GroupDetailsViewModel : INotifyPropertyChanged
    {
        private List<TaskHolder> _availableTasksToAdd;
        private GroupHolder _showGroup;
        private IHoldersService _holdersService;
        private Visibility _dataVisibility;
        public ICommand AssignTaskCmd { get; protected set; }
        public ICommand RemoveTaskCmd { get; protected set; }

        public GroupDetailsViewModel(IHoldersService holdersService)
        {
            _holdersService = holdersService;
            AssignTaskCmd = new DelegateCommand(AssignTask);
            RemoveTaskCmd = new DelegateCommand<object>(RemoveTask);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void Initialize(GroupHolder chooosenGroup, List<TaskHolder> tasks)
        {
            if (chooosenGroup == null)
            {
                DataVisibility = Visibility.Hidden;
                return;
            }

            DataVisibility = Visibility.Visible;
            _availableTasksToAdd = new List<TaskHolder>();

            ShownGroup = _holdersService.InitializeGroupDetails(_availableTasksToAdd, chooosenGroup, tasks);
        }

        private void AssignTask()
        {
            var dialogContext = new ChooseItemWindowViewModel
            {
                Holders = _availableTasksToAdd.Cast<NamedHolder>().ToList()
            };
            var dialog = new ChooseItemWindowView
            {
                DataContext = dialogContext
            };
            dialogContext.CloseAction = () => dialog.Close();
            dialog.ShowDialog();
            if (dialogContext.SelectedHolder is TaskHolder taskToAdd)
            {
                _availableTasksToAdd.Remove(taskToAdd);
                ShownGroup.Tasks.Add(taskToAdd);
                taskToAdd.Groups.Add(ShownGroup);
            }
        }
        private void RemoveTask(object task)
        {
            var taskholder = task as TaskHolder;
            taskholder.Groups.Remove(ShownGroup);
            ShownGroup.Tasks.Remove(taskholder);
            _availableTasksToAdd.Add(taskholder);
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
                OnPropertyChanged(nameof(ShownGroup));
            }
        }
        public Visibility DataVisibility
        {
            get
            {
                return _dataVisibility;
            }
            set
            {
                _dataVisibility = value;
                OnPropertyChanged(nameof(DataVisibility));
            }
        }
    }
}
