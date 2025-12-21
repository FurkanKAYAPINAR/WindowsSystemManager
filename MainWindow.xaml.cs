// ============================================================================
// Windows System Manager - Main Window
// Author: FurkanKAYAPINAR
// Version: 1.3.0
// Description: Windows Task Scheduler and Services Management Application
// Copyright © 2024 FurkanKAYAPINAR - All Rights Reserved
// ============================================================================

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32.TaskScheduler;
using Forms = System.Windows.Forms;
using DrawingColor = System.Drawing.Color;
using DrawingIcon = System.Drawing.Icon;
using SystemIcons = System.Drawing.SystemIcons;

namespace WindowsSystemManager
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ServiceItem> _allServices = new();
        private ObservableCollection<ServiceItem> _filteredServices = new();
        private ObservableCollection<TaskItem> _allTasks = new();
        private ObservableCollection<TaskItem> _filteredTasks = new();
        private ObservableCollection<ProcessItem> _allProcesses = new();
        private ObservableCollection<ProcessItem> _filteredProcesses = new();
        
        private Forms.NotifyIcon? _notifyIcon;
        private bool _isExiting = false;

        public MainWindow()
        {
            InitializeComponent();
            ServicesDataGrid.ItemsSource = _filteredServices;
            TasksDataGrid.ItemsSource = _filteredTasks;
            ProcessesDataGrid.ItemsSource = _filteredProcesses;
            
            // Initialize placeholder visibility
            ServicesSearchBox.TextChanged += (s, e) => 
                ServicesSearchPlaceholder.Visibility = string.IsNullOrEmpty(ServicesSearchBox.Text) 
                    ? Visibility.Visible : Visibility.Collapsed;
            TasksSearchBox.TextChanged += (s, e) => 
                TasksSearchPlaceholder.Visibility = string.IsNullOrEmpty(TasksSearchBox.Text) 
                    ? Visibility.Visible : Visibility.Collapsed;
            ProcessesSearchBox.TextChanged += (s, e) => 
                ProcessesSearchPlaceholder.Visibility = string.IsNullOrEmpty(ProcessesSearchBox.Text) 
                    ? Visibility.Visible : Visibility.Collapsed;
            
            // Setup System Tray
            SetupNotifyIcon();
            
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
        }

        private void SetupNotifyIcon()
        {
            _notifyIcon = new Forms.NotifyIcon();
            
            // Load icon from file
            var iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
            if (System.IO.File.Exists(iconPath))
            {
                _notifyIcon.Icon = new DrawingIcon(iconPath);
            }
            else
            {
                _notifyIcon.Icon = SystemIcons.Application;
            }
            
            _notifyIcon.Text = "Windows System Manager - by FurkanKAYAPINAR";
            _notifyIcon.Visible = true;
            
            // Double-click to show window
            _notifyIcon.DoubleClick += (s, e) => ShowWindow();
            
            // Context menu
            var contextMenu = new Forms.ContextMenuStrip();
            
            var showItem = new Forms.ToolStripMenuItem("📺 Show");
            showItem.Click += (s, e) => ShowWindow();
            contextMenu.Items.Add(showItem);
            
            contextMenu.Items.Add(new Forms.ToolStripSeparator());
            
            var exitItem = new Forms.ToolStripMenuItem("❌ Exit");
            exitItem.Click += (s, e) => ExitApplication();
            contextMenu.Items.Add(exitItem);
            
            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void ShowWindow()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void ExitApplication()
        {
            _isExiting = true;
            _notifyIcon?.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (!_isExiting)
            {
                // Minimize to tray instead of closing
                e.Cancel = true;
                this.Hide();
                
                _notifyIcon?.ShowBalloonTip(
                    2000, 
                    "Windows System Manager", 
                    "Uygulama arka planda çalışmaya devam ediyor. Çıkmak için sağ tık → Exit", 
                    Forms.ToolTipIcon.Info);
            }
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAllData();
        }

        private async System.Threading.Tasks.Task RefreshAllData()
        {
            SetStatus("Loading data...");
            await LoadServicesAsync();
            await LoadTasksAsync();
            await LoadProcessesAsync();
            SetStatus("Ready");
        }

        #region Services

        private async System.Threading.Tasks.Task LoadServicesAsync()
        {
            SetStatus("Loading services...");
            
            await System.Threading.Tasks.Task.Run(() =>
            {
                var services = ServiceController.GetServices()
                    .Select(s => new ServiceItem
                    {
                        ServiceName = s.ServiceName,
                        DisplayName = s.DisplayName,
                        Publisher = GetServicePublisher(s.ServiceName),
                        Description = GetServiceDescription(s.ServiceName),
                        Status = s.Status.ToString(),
                        StartType = GetStartType(s.ServiceName),
                        StatusBackground = GetStatusBrush(s.Status)
                    })
                    .OrderBy(s => s.DisplayName)
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    _allServices.Clear();
                    foreach (var service in services)
                    {
                        _allServices.Add(service);
                    }
                    FilterServices();
                    UpdateServicesCount();
                });
            });
        }

        private string GetStartType(string serviceName)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Services\{serviceName}");
                if (key != null)
                {
                    var start = key.GetValue("Start");
                    if (start != null)
                    {
                        return (int)start switch
                        {
                            0 => "Boot",
                            1 => "System",
                            2 => "Automatic",
                            3 => "Manual",
                            4 => "Disabled",
                            _ => "Unknown"
                        };
                    }
                }
            }
            catch { }
            return "Unknown";
        }

        private string GetServiceDescription(string serviceName)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Services\{serviceName}");
                if (key != null)
                {
                    var description = key.GetValue("Description") as string;
                    if (!string.IsNullOrEmpty(description))
                    {
                        // Truncate if too long
                        return description.Length > 100 ? description.Substring(0, 100) + "..." : description;
                    }
                }
            }
            catch { }
            return "";
        }

        private string GetServicePublisher(string serviceName)
        {
            try
            {
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Services\{serviceName}");
                if (key != null)
                {
                    var imagePath = key.GetValue("ImagePath") as string;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        // Clean up the path
                        imagePath = imagePath.Trim('"').Split('"')[0].Trim();
                        if (imagePath.StartsWith("\\"))
                            imagePath = imagePath.TrimStart('\\').Replace("??\\", "");
                        
                        // Expand environment variables
                        imagePath = Environment.ExpandEnvironmentVariables(imagePath);
                        
                        // Get just the exe path
                        var exePath = imagePath.Split(' ')[0];
                        
                        if (System.IO.File.Exists(exePath))
                        {
                            var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(exePath);
                            return versionInfo.CompanyName ?? "";
                        }
                    }
                }
            }
            catch { }
            return "";
        }

        private SolidColorBrush GetStatusBrush(ServiceControllerStatus status)
        {
            var brush = status switch
            {
                ServiceControllerStatus.Running => new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 197, 94)),
                ServiceControllerStatus.Stopped => new SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68)),
                ServiceControllerStatus.Paused => new SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 158, 11)),
                _ => new SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 114, 128))
            };
            brush.Freeze();
            return brush;
        }

        private void FilterServices()
        {
            var searchText = ServicesSearchBox.Text?.ToLower() ?? "";
            var filtered = _allServices.Where(s => 
                s.ServiceName.ToLower().Contains(searchText) || 
                s.DisplayName.ToLower().Contains(searchText));
            
            _filteredServices.Clear();
            foreach (var service in filtered)
            {
                _filteredServices.Add(service);
            }
            UpdateServicesCount();
        }

        private void UpdateServicesCount()
        {
            var selected = _filteredServices.Count(s => s.IsSelected);
            ServicesCountText.Text = $"{selected} selected of {_filteredServices.Count} services";
        }

        private void ServicesSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterServices();
        }

        private void SelectAllServices_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = SelectAllServicesCheckBox.IsChecked ?? false;
            foreach (var service in _filteredServices)
            {
                service.IsSelected = isChecked;
            }
            ServicesDataGrid.Items.Refresh();
            UpdateServicesCount();
        }

        private async void StartServices_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredServices.Where(s => s.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one service.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Start {selected.Count} service(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            foreach (var service in selected)
            {
                await ControlServiceAsync(service.ServiceName, "start");
            }
            await LoadServicesAsync();
        }

        private async void StopServices_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredServices.Where(s => s.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one service.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Stop {selected.Count} service(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            foreach (var service in selected)
            {
                await ControlServiceAsync(service.ServiceName, "stop");
            }
            await LoadServicesAsync();
        }

        private async void RestartServices_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredServices.Where(s => s.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one service.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Restart {selected.Count} service(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            foreach (var service in selected)
            {
                await ControlServiceAsync(service.ServiceName, "stop");
                await ControlServiceAsync(service.ServiceName, "start");
            }
            await LoadServicesAsync();
        }

        private async System.Threading.Tasks.Task ControlServiceAsync(string serviceName, string action)
        {
            SetStatus($"{action}ing {serviceName}...");
            try
            {
                using var sc = new ServiceController(serviceName);
                switch (action.ToLower())
                {
                    case "start":
                        if (sc.Status != ServiceControllerStatus.Running)
                        {
                            sc.Start();
                            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        }
                        break;
                    case "stop":
                        if (sc.Status != ServiceControllerStatus.Stopped)
                        {
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error {action}ing {serviceName}: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RefreshServices_Click(object sender, RoutedEventArgs e)
        {
            await LoadServicesAsync();
            SetStatus("Services refreshed");
        }

        private void OpenServiceFolder_Click(object sender, RoutedEventArgs e)
        {
            var selected = ServicesDataGrid.SelectedItem as ServiceItem;
            if (selected == null)
            {
                System.Windows.MessageBox.Show("Please select a service.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Services\{selected.ServiceName}");
                if (key != null)
                {
                    var imagePath = key.GetValue("ImagePath") as string;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        // Clean up the path
                        imagePath = imagePath.Trim('"').Split('"')[0].Trim();
                        if (imagePath.StartsWith("\\"))
                            imagePath = imagePath.TrimStart('\\').Replace("??\\", "");
                        
                        imagePath = Environment.ExpandEnvironmentVariables(imagePath);
                        var exePath = imagePath.Split(' ')[0];
                        
                        if (System.IO.File.Exists(exePath))
                        {
                            var folder = System.IO.Path.GetDirectoryName(exePath);
                            if (!string.IsNullOrEmpty(folder))
                            {
                                System.Diagnostics.Process.Start("explorer.exe", folder);
                                SetStatus($"Opened folder: {folder}");
                                return;
                            }
                        }
                    }
                }
                System.Windows.MessageBox.Show("Could not find the service executable path.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error opening folder: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Scheduled Tasks

        private async System.Threading.Tasks.Task LoadTasksAsync()
        {
            SetStatus("Loading scheduled tasks...");
            
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    using var ts = new TaskService();
                    var tasks = GetAllTasks(ts.RootFolder)
                        .Select(t => new TaskItem
                        {
                            Name = t.Name,
                            Path = t.Path,
                            State = t.State.ToString(),
                            Author = t.Definition?.RegistrationInfo?.Author ?? "Unknown",
                            LastRunTime = t.LastRunTime == DateTime.MinValue ? "Never" : t.LastRunTime.ToString("g"),
                            NextRunTime = t.NextRunTime == DateTime.MinValue ? "Not scheduled" : t.NextRunTime.ToString("g"),
                            StateBackground = GetTaskStateBrush(t.State)
                        })
                        .OrderBy(t => t.Path)
                        .ToList();

                    Dispatcher.Invoke(() =>
                    {
                        _allTasks.Clear();
                        foreach (var task in tasks)
                        {
                            _allTasks.Add(task);
                        }
                        FilterTasks();
                        UpdateTasksCount();
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        System.Windows.MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }

        private System.Collections.Generic.IEnumerable<Microsoft.Win32.TaskScheduler.Task> GetAllTasks(TaskFolder folder)
        {
            foreach (var task in folder.Tasks)
            {
                yield return task;
            }
            foreach (var subFolder in folder.SubFolders)
            {
                foreach (var task in GetAllTasks(subFolder))
                {
                    yield return task;
                }
            }
        }

        private SolidColorBrush GetTaskStateBrush(TaskState state)
        {
            var brush = state switch
            {
                TaskState.Ready => new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 197, 94)),
                TaskState.Running => new SolidColorBrush(System.Windows.Media.Color.FromRgb(59, 130, 246)),
                TaskState.Disabled => new SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68)),
                _ => new SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 114, 128))
            };
            brush.Freeze();
            return brush;
        }

        private void FilterTasks()
        {
            var searchText = TasksSearchBox.Text?.ToLower() ?? "";
            var filtered = _allTasks.Where(t => 
                t.Name.ToLower().Contains(searchText) || 
                t.Path.ToLower().Contains(searchText));
            
            _filteredTasks.Clear();
            foreach (var task in filtered)
            {
                _filteredTasks.Add(task);
            }
            UpdateTasksCount();
        }

        private void UpdateTasksCount()
        {
            var selected = _filteredTasks.Count(t => t.IsSelected);
            TasksCountText.Text = $"{selected} selected of {_filteredTasks.Count} tasks";
        }

        private void TasksSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterTasks();
        }

        private void SelectAllTasks_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = SelectAllTasksCheckBox.IsChecked ?? false;
            foreach (var task in _filteredTasks)
            {
                task.IsSelected = isChecked;
            }
            TasksDataGrid.Items.Refresh();
            UpdateTasksCount();
        }

        private async void RunTasks_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredTasks.Where(t => t.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one task.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Run {selected.Count} task(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            await System.Threading.Tasks.Task.Run(() =>
            {
                using var ts = new TaskService();
                foreach (var taskItem in selected)
                {
                    try
                    {
                        var task = ts.GetTask(taskItem.Path);
                        task?.Run();
                        Dispatcher.Invoke(() => SetStatus($"Started: {taskItem.Name}"));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() => 
                            System.Windows.MessageBox.Show($"Error running {taskItem.Name}: {ex.Message}", "Error"));
                    }
                }
            });
            await LoadTasksAsync();
        }

        private async void StopTasks_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredTasks.Where(t => t.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one task.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Stop {selected.Count} task(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            await System.Threading.Tasks.Task.Run(() =>
            {
                using var ts = new TaskService();
                foreach (var taskItem in selected)
                {
                    try
                    {
                        var task = ts.GetTask(taskItem.Path);
                        task?.Stop();
                        Dispatcher.Invoke(() => SetStatus($"Stopped: {taskItem.Name}"));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() => 
                            System.Windows.MessageBox.Show($"Error stopping {taskItem.Name}: {ex.Message}", "Error"));
                    }
                }
            });
            await LoadTasksAsync();
        }

        private async void EnableTasks_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredTasks.Where(t => t.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one task.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Enable {selected.Count} task(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            await System.Threading.Tasks.Task.Run(() =>
            {
                using var ts = new TaskService();
                foreach (var taskItem in selected)
                {
                    try
                    {
                        var task = ts.GetTask(taskItem.Path);
                        if (task != null)
                        {
                            task.Enabled = true;
                        }
                        Dispatcher.Invoke(() => SetStatus($"Enabled: {taskItem.Name}"));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() => 
                            System.Windows.MessageBox.Show($"Error enabling {taskItem.Name}: {ex.Message}", "Error"));
                    }
                }
            });
            await LoadTasksAsync();
        }

        private async void DisableTasks_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredTasks.Where(t => t.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one task.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"Disable {selected.Count} task(s)?", "Confirm", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            await System.Threading.Tasks.Task.Run(() =>
            {
                using var ts = new TaskService();
                foreach (var taskItem in selected)
                {
                    try
                    {
                        var task = ts.GetTask(taskItem.Path);
                        if (task != null)
                        {
                            task.Enabled = false;
                        }
                        Dispatcher.Invoke(() => SetStatus($"Disabled: {taskItem.Name}"));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() => 
                            System.Windows.MessageBox.Show($"Error disabling {taskItem.Name}: {ex.Message}", "Error"));
                    }
                }
            });
            await LoadTasksAsync();
        }

        private async void DeleteTasks_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredTasks.Where(t => t.IsSelected).ToList();
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one task.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show(
                $"⚠️ DELETE {selected.Count} task(s)?\n\nThis action cannot be undone!", 
                "Confirm Delete", 
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            await System.Threading.Tasks.Task.Run(() =>
            {
                using var ts = new TaskService();
                foreach (var taskItem in selected)
                {
                    try
                    {
                        ts.RootFolder.DeleteTask(taskItem.Path, false);
                        Dispatcher.Invoke(() => SetStatus($"Deleted: {taskItem.Name}"));
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() => 
                            System.Windows.MessageBox.Show($"Error deleting {taskItem.Name}: {ex.Message}", "Error"));
                    }
                }
            });
            await LoadTasksAsync();
        }

        private async void RefreshTasks_Click(object sender, RoutedEventArgs e)
        {
            await LoadTasksAsync();
            SetStatus("Tasks refreshed");
        }

        private void OpenTaskFolder_Click(object sender, RoutedEventArgs e)
        {
            var selected = TasksDataGrid.SelectedItem as TaskItem;
            if (selected == null)
            {
                System.Windows.MessageBox.Show("Please select a task.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                using var ts = new TaskService();
                var task = ts.GetTask(selected.Path);
                if (task?.Definition?.Actions?.Count > 0)
                {
                    foreach (var action in task.Definition.Actions)
                    {
                        if (action is Microsoft.Win32.TaskScheduler.ExecAction execAction)
                        {
                            var exePath = Environment.ExpandEnvironmentVariables(execAction.Path ?? "");
                            if (System.IO.File.Exists(exePath))
                            {
                                var folder = System.IO.Path.GetDirectoryName(exePath);
                                if (!string.IsNullOrEmpty(folder))
                                {
                                    System.Diagnostics.Process.Start("explorer.exe", folder);
                                    SetStatus($"Opened folder: {folder}");
                                    return;
                                }
                            }
                        }
                    }
                }
                System.Windows.MessageBox.Show("Could not find the task executable path.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error opening folder: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Processes

        private async System.Threading.Tasks.Task LoadProcessesAsync()
        {
            SetStatus("Loading processes...");
            
            await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var processes = Process.GetProcesses()
                        .Select(p => 
                        {
                            string filePath = "";
                            string windowTitle = "";
                            long memoryBytes = 0;
                            double cpuPercent = 0;
                            string uptime = "";
                            
                            try
                            {
                                filePath = p.MainModule?.FileName ?? "";
                            }
                            catch { }
                            
                            try
                            {
                                windowTitle = p.MainWindowTitle;
                            }
                            catch { }
                            
                            try
                            {
                                memoryBytes = p.WorkingSet64;
                            }
                            catch { }
                            
                            try
                            {
                                var startTime = p.StartTime;
                                var duration = DateTime.Now - startTime;
                                if (duration.TotalDays >= 1)
                                    uptime = $"{(int)duration.TotalDays}d {duration.Hours}h";
                                else if (duration.TotalHours >= 1)
                                    uptime = $"{(int)duration.TotalHours}h {duration.Minutes}m";
                                else
                                    uptime = $"{duration.Minutes}m {duration.Seconds}s";
                            }
                            catch { uptime = "-"; }
                            
                            try
                            {
                                var totalCpu = p.TotalProcessorTime.TotalMilliseconds;
                                var processAge = (DateTime.Now - p.StartTime).TotalMilliseconds;
                                if (processAge > 0)
                                {
                                    cpuPercent = (totalCpu / processAge / Environment.ProcessorCount) * 100;
                                }
                            }
                            catch { }
                            
                            return new ProcessItem
                            {
                                ProcessId = p.Id,
                                ProcessName = p.ProcessName,
                                WindowTitle = windowTitle,
                                MemoryBytes = memoryBytes,
                                MemoryMB = $"{memoryBytes / 1024.0 / 1024.0:F1}",
                                CpuPercent = $"{cpuPercent:F1}",
                                FilePath = filePath,
                                Uptime = uptime
                            };
                        })
                        .OrderBy(p => p.ProcessName)
                        .ToList();

                    Dispatcher.Invoke(() =>
                    {
                        _allProcesses.Clear();
                        _filteredProcesses.Clear();
                        foreach (var proc in processes)
                        {
                            _allProcesses.Add(proc);
                            _filteredProcesses.Add(proc);
                        }
                        ProcessesCountText.Text = $"({_filteredProcesses.Count} processes)";
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => System.Windows.MessageBox.Show($"Error loading processes: {ex.Message}", "Error"));
                }
            });
        }

        private void FilterProcesses()
        {
            var searchText = ProcessesSearchBox.Text.ToLower();
            _filteredProcesses.Clear();
            
            var filtered = string.IsNullOrEmpty(searchText)
                ? _allProcesses
                : _allProcesses.Where(p => 
                    p.ProcessName.ToLower().Contains(searchText) ||
                    p.WindowTitle.ToLower().Contains(searchText) ||
                    p.FilePath.ToLower().Contains(searchText) ||
                    p.ProcessId.ToString().Contains(searchText));
            
            foreach (var proc in filtered)
            {
                _filteredProcesses.Add(proc);
            }
            ProcessesCountText.Text = $"({_filteredProcesses.Count} processes)";
        }

        private void ProcessesSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterProcesses();
        }

        private void SelectAllProcesses_Click(object sender, RoutedEventArgs e)
        {
            var isChecked = SelectAllProcessesCheckBox.IsChecked == true;
            foreach (var proc in _filteredProcesses)
            {
                proc.IsSelected = isChecked;
            }
        }

        private void EndProcess_Click(object sender, RoutedEventArgs e)
        {
            var selected = _filteredProcesses.Where(p => p.IsSelected).ToList();
            if (!selected.Any())
            {
                // Try single selected from DataGrid
                var item = ProcessesDataGrid.SelectedItem as ProcessItem;
                if (item != null)
                {
                    selected = new List<ProcessItem> { item };
                }
            }
            
            if (!selected.Any())
            {
                System.Windows.MessageBox.Show("Please select at least one process.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = System.Windows.MessageBox.Show($"End {selected.Count} process(es)?\n\nWarning: This may cause data loss!", 
                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            foreach (var proc in selected)
            {
                try
                {
                    var p = Process.GetProcessById(proc.ProcessId);
                    p.Kill();
                    SetStatus($"Ended: {proc.ProcessName}");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error ending {proc.ProcessName}: {ex.Message}", "Error");
                }
            }
            
            _ = LoadProcessesAsync();
        }

        private async void RefreshProcesses_Click(object sender, RoutedEventArgs e)
        {
            await LoadProcessesAsync();
            SetStatus("Processes refreshed");
        }

        private void OpenProcessFolder_Click(object sender, RoutedEventArgs e)
        {
            var selected = ProcessesDataGrid.SelectedItem as ProcessItem;
            if (selected == null)
            {
                System.Windows.MessageBox.Show("Please select a process.", "No Selection", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(selected.FilePath) && System.IO.File.Exists(selected.FilePath))
                {
                    var folder = System.IO.Path.GetDirectoryName(selected.FilePath);
                    if (!string.IsNullOrEmpty(folder))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", folder);
                        SetStatus($"Opened folder: {folder}");
                        return;
                    }
                }
                System.Windows.MessageBox.Show("Could not find the process executable path.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error opening folder: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region General

        private async void RefreshAll_Click(object sender, RoutedEventArgs e)
        {
            await RefreshAllData();
        }

        private void SetStatus(string message)
        {
            StatusText.Text = message;
        }

        #endregion
    }

    #region Models

    public class ServiceItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
        
        public string ServiceName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Publisher { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public string StartType { get; set; } = "";
        public SolidColorBrush StatusBackground { get; set; } = System.Windows.Media.Brushes.Gray;
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class TaskItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
        
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string State { get; set; } = "";
        public string Author { get; set; } = "";
        public string LastRunTime { get; set; } = "";
        public string NextRunTime { get; set; } = "";
        public SolidColorBrush StateBackground { get; set; } = System.Windows.Media.Brushes.Gray;
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class ProcessItem : INotifyPropertyChanged
    {
        private bool _isSelected;
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
        
        public int ProcessId { get; set; }
        public string ProcessName { get; set; } = "";
        public string WindowTitle { get; set; } = "";
        public string MemoryMB { get; set; } = "";
        public long MemoryBytes { get; set; }
        public string CpuPercent { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string Uptime { get; set; } = "";
        
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    #endregion
}
