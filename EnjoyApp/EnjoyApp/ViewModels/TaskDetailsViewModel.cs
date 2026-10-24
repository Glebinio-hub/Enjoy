using EnjoyApp.Models;
using EnjoyApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace EnjoyApp.ViewModels
{

    public class TaskDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public TaskItem Task { get; }

        private TaskService TaskService { get; }
        public bool HasSubTasks => Task?.SubTasks?.Count > 0;

        public TaskDetailsViewModel(TaskItem task, TaskService taskService)
        {
            Task = task;
            TaskService = taskService;
            Task.SubTasks.CollectionChanged += OnSubTasksChanged;
        }

        public void SetCompleted(bool isCompleted)
        {
            TaskService.SetCompleted(Task, isCompleted);
        }

        public void SetSubTaskCompleted(SubTaskItem subTask, bool isCompleted)
        {
            TaskService.SetCompletedSubTask(Task, subTask, isCompleted);
        }

        private void OnSubTasksChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(nameof(HasSubTasks)));
        }


        public TaskDetailsViewModel()
        {
            Task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Name = "New Task",
                Description = "Task Description",
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                IsCompleted = false,
                SubTasks = new System.Collections.ObjectModel.ObservableCollection<SubTaskItem>()
            };

            Task.SubTasks.CollectionChanged += OnSubTasksChanged;

        }



    }
}
