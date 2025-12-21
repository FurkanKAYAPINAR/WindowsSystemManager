<p align="center">
  <img src="app_icon.png" alt="Windows System Manager" width="128" height="128">
</p>

<h1 align="center">Windows System Manager</h1>

<p align="center">
  <strong>🖥️ A powerful Windows system management tool for Services, Tasks, and Processes</strong>
</p>

<p align="center">
  <a href="https://github.com/FurkanKAYAPINAR/WindowsSystemManager/releases/latest">
    <img src="https://img.shields.io/github/v/release/FurkanKAYAPINAR/WindowsSystemManager?style=for-the-badge&logo=github&color=blue" alt="Latest Release">
  </a>
  <a href="https://github.com/FurkanKAYAPINAR/WindowsSystemManager/blob/main/LICENSE">
    <img src="https://img.shields.io/github/license/FurkanKAYAPINAR/WindowsSystemManager?style=for-the-badge&color=green" alt="License">
  </a>
  <a href="https://dotnet.microsoft.com/download/dotnet/8.0">
    <img src="https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet" alt=".NET 8.0">
  </a>
  <a href="https://github.com/FurkanKAYAPINAR/WindowsSystemManager/stargazers">
    <img src="https://img.shields.io/github/stars/FurkanKAYAPINAR/WindowsSystemManager?style=for-the-badge&logo=github&color=yellow" alt="Stars">
  </a>
</p>

<p align="center">
  <a href="#-features">Features</a> •
  <a href="#-installation">Installation</a> •
  <a href="#-usage">Usage</a> •
  <a href="#-screenshots">Screenshots</a> •
  <a href="#-contributing">Contributing</a> •
  <a href="#-license">License</a>
</p>

---

## 📋 Overview

**Windows System Manager** is a modern WPF application that provides an all-in-one solution for managing Windows Services, Scheduled Tasks, and Running Processes. Built with .NET 8.0, it offers a clean, intuitive interface with powerful management capabilities.

## ✨ Features

### 🔧 Services Management
- **View all Windows services** with detailed information (Name, Status, Start Type, Publisher)
- **Start, Stop, and Restart** services with a single click
- **Search and filter** services by name or display name
- **Bulk operations** - Select multiple services for batch actions
- **Open service folder** - Navigate directly to service executable location

### 📅 Scheduled Tasks Management
- **Browse all scheduled tasks** with comprehensive details
- **Run, Stop, Enable, and Disable** tasks
- **Delete tasks** with confirmation
- **View task properties** (Author, State, Last Run, Next Run)
- **Search functionality** for quick task location
- **Open task folder** - Access task executable path

### 💻 Process Management
- **Real-time process list** with resource usage
- **Memory and CPU monitoring** for each process
- **End processes** safely with confirmation
- **Search by name, PID, or window title**
- **Open process location** in File Explorer
- **View active window titles**

### 🎨 User Experience
- **Modern dark theme** with sleek design
- **System tray integration** - Minimize to tray
- **Tab-based navigation** for easy switching
- **Real-time status updates**
- **Bulk selection** with "Select All" options
- **Responsive UI** with async operations

## 💾 Installation

### Option 1: Download Release (Recommended)

1. Go to the [Releases](https://github.com/FurkanKAYAPINAR/WindowsSystemManager/releases) page
2. Download the latest `WindowsSystemManager.zip`
3. Extract to your preferred location
4. Run `WindowsSystemManager.exe` as Administrator

### Option 2: Build from Source

**Prerequisites:**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (or later) with WPF workload

**Steps:**

```bash
# Clone the repository
git clone https://github.com/FurkanKAYAPINAR/WindowsSystemManager.git

# Navigate to project directory
cd WindowsSystemManager

# Restore dependencies
dotnet restore

# Build the project
dotnet build --configuration Release

# Run the application
dotnet run
```

## 🚀 Usage

> ⚠️ **Important:** This application requires **Administrator privileges** to manage services and tasks.

### Managing Services

1. Navigate to the **Services** tab
2. Use the search box to filter services
3. Select one or more services using checkboxes
4. Click the appropriate action button:
   - ▶️ **Start** - Start selected services
   - ⏹️ **Stop** - Stop selected services
   - 🔄 **Restart** - Restart selected services
   - 📁 **Folder** - Open service executable location

### Managing Scheduled Tasks

1. Navigate to the **Tasks** tab
2. Search for tasks by name or path
3. Select tasks and choose an action:
   - ▶️ **Run** - Execute task immediately
   - ⏹️ **Stop** - Terminate running task
   - ✅ **Enable** - Enable disabled task
   - ❌ **Disable** - Disable enabled task
   - 🗑️ **Delete** - Remove task permanently

### Managing Processes

1. Navigate to the **Processes** tab
2. Search by process name, PID, or window title
3. Select processes and:
   - ⏹️ **End** - Terminate selected processes
   - 📁 **Folder** - Open process file location

## 📸 Screenshots

<details>
<summary>Click to view screenshots</summary>

### Services Tab
![Services Management](docs/screenshots/services.png)

### Tasks Tab
![Task Scheduler](docs/screenshots/tasks.png)

### Processes Tab
![Process Manager](docs/screenshots/processes.png)

</details>

## 🔧 System Requirements

| Requirement | Minimum |
|-------------|---------|
| **OS** | Windows 10 (1809+) / Windows 11 |
| **Framework** | .NET 8.0 Runtime |
| **RAM** | 256 MB |
| **Disk Space** | 50 MB |
| **Privileges** | Administrator (for full functionality) |

## 📦 Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| [TaskScheduler](https://www.nuget.org/packages/TaskScheduler/) | 2.11.0 | Windows Task Scheduler API |
| [System.ServiceProcess.ServiceController](https://www.nuget.org/packages/System.ServiceProcess.ServiceController/) | 8.0.0 | Windows Services Management |

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. **Fork** the repository
2. **Create** your feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## 📝 Changelog

See [CHANGELOG.md](CHANGELOG.md) for a list of changes and version history.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Furkan KAYAPINAR**

- GitHub: [@FurkanKAYAPINAR](https://github.com/FurkanKAYAPINAR)

## 🙏 Acknowledgments

- Microsoft for .NET 8.0 and WPF
- [David Hall](https://github.com/dahall) for the TaskScheduler library
- All contributors who help improve this project

---

<p align="center">
  Made with ❤️ by <a href="https://github.com/FurkanKAYAPINAR">Furkan KAYAPINAR</a>
</p>

<p align="center">
  ⭐ Star this repository if you find it helpful!
</p>
