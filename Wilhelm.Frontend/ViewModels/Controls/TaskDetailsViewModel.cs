using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Support;
using Wilhelm.Frontend.ViewModels.Windows;
using Wilhelm.Frontend.Views.Windows;

namespace Wilhelm.Frontend.ViewModels.Controls
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
            RemoveGroupCmd = new DelegateCommand(RemoveGroup);
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

        private void AssignGroup(object obj)
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
