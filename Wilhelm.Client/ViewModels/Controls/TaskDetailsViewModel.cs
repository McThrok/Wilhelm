using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wilhelm.Client.Model;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.Support;
using Wilhelm.Client.ViewModels.Windows;
using Wilhelm.Client.Views.Windows;

namespace Wilhelm.Client.ViewModels.Controls
{
    public class TaskDetailsViewModel: INotifyPropertyChanged
    {
        private List<GroupHolder> _availableGroupsToAdd;
        private TaskHolder _shownTask;
        private readonly IHoldersService _holdersService;
        private Visibility _dataVisibility;
        public ICommand AssignGroupCmd { get; protected set; }
        public ICommand RemoveGroupCmd { get; protected set; }

        public TaskDetailsViewModel(IHoldersService holdersService)
        {
            _holdersService = holdersService;
            AssignGroupCmd = new DelegateCommand(AssignGroup);
            RemoveGroupCmd = new DelegateCommand<object>(RemoveGroup);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public void Initialize(TaskHolder choosenTask, List<GroupHolder> groups)
        {
            if (choosenTask == null)
            {
                DataVisibility = Visibility.Hidden;
                return;
            }

           DataVisibility = Visibility.Visible;
            _availableGroupsToAdd = new List<GroupHolder>();

            ShownTask = _holdersService.InitializeTaskDetails(_availableGroupsToAdd, choosenTask, groups);

        }

        private void AssignGroup()
        {
            var dialogContext = new ChooseItemWindowViewModel
            {
                Holders = _availableGroupsToAdd.Cast<NamedHolder>().ToList()
            };
            var dialog = new ChooseItemWindowView
            {
                DataContext = dialogContext
            };
            dialogContext.CloseAction = () => dialog.Close();
            dialog.ShowDialog();

            if (dialogContext.SelectedHolder is GroupHolder groupToAdd)
            {
                _availableGroupsToAdd.Remove(groupToAdd);
                ShownTask.Groups.Add(groupToAdd);
                groupToAdd.Tasks.Add(ShownTask);
            }
        }

        private void RemoveGroup(object obj)
        {
            var group = obj as GroupHolder;
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
                OnPropertyChanged(nameof(ShownTask));
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
