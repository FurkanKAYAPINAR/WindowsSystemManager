# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.6.0] - 2024-12-28

### Added
- **🔍 Process Watcher Tab** - Real-time process monitoring using WMI
  - Live process start/stop event tracking
  - Command line capture for started processes
  - Parent process ID and name resolution
  - User account information for each process
  - Statistics panel (Total events, Starts, Stops)
  - Start/Stop monitoring toggle button
  - Auto-scroll option for real-time viewing
  - Event filtering by process name, PID, or command line
  - **Export** events to JSON or CSV format
  - Maximum 1000 events kept in memory

### Technical
- Added `System.Management` package for WMI event subscription
- Implemented `Win32_ProcessStartTrace` and `Win32_ProcessStopTrace` event handlers
- Color-coded event badges (green=Start, red=Stop)

## [1.5.0] - 2025-12-26

### Added
- **Network Connections Tab**
  - View all TCP/UDP connections with process info
  - Protocol, local/remote address, state information
  - Search and filter functionality
- **Drives Tab**
  - View all disk drives with usage statistics
  - Visual progress bars for disk usage
- **Startup Manager Tab**
  - Manage Windows startup programs
  - Enable/Disable startup items
  - View startup item status and impact
- **Header Performance Indicators**
  - Real-time CPU usage percentage
  - RAM usage with percentage and size
  - Network download/upload speed

### Changed
- Copyright year updated to 2025
- Improved UI with modern design elements
- Enhanced data grid styling

### Fixed
- Services tab now sorts Running services first by default
- Memory optimizations for large data sets

## [1.4.0] - 2024-12-24

### Added
- **Services Tab - New Features**
  - ⛔ **Disable Service** - Disable services to prevent automatic startup
  - 🗑️ **Delete Service** - Remove services with double confirmation (requires typing service name)
  - 📋 **Service Properties** - View detailed service information in a popup window
  - Context menu with all service actions
- **Tasks Tab - Microsoft Filter**
  - ☑️ **Hide Microsoft Tasks** - Filter out Microsoft system tasks (enabled by default)
  - Cleaner task list focusing on user/third-party tasks
- **UI Improvements**
  - Enhanced search box styling
  - Better visual feedback for actions

### Fixed
- **Next Run Display** - Now correctly shows "Disabled" or "Trigger-based" for tasks without scheduled time
- Search box visual alignment issues

### Security
- Delete service requires double confirmation with service name verification
- Prevents accidental deletion of critical system services

## [1.3.0] - 2024-12-21


### Added
- **System Tray Integration**
  - Application now minimizes to system tray instead of closing
  - Tray icon with context menu (Show/Exit options)
  - Double-click tray icon to restore window
- **Process Management Tab**
  - View all running processes with details
  - Memory and CPU usage monitoring
  - End processes functionality
  - Open process file location
  - Search by name, PID, or window title
- **Enhanced UI/UX**
  - Improved dark theme styling
  - Better status bar feedback
  - Select all functionality for all tabs

### Changed
- Upgraded to .NET 8.0
- Improved async operations for better responsiveness
- Enhanced error handling with detailed messages

### Fixed
- Namespace conflicts between WPF and WinForms components
- Memory leak in service monitoring
- UI freeze during large data loads

## [1.2.0] - 2024-12-15

### Added
- **Scheduled Tasks Management**
  - View all Windows scheduled tasks
  - Run, Stop, Enable, Disable tasks
  - Delete tasks with confirmation
  - Open task executable folder
  - Search and filter functionality

### Changed
- Reorganized UI with tab-based navigation
- Improved service list loading performance

## [1.1.0] - 2024-12-10

### Added
- Service filtering by status (Running/Stopped)
- Bulk service operations (Start/Stop/Restart multiple)
- Open service executable folder feature
- Service publisher and description display

### Changed
- Enhanced dark theme with better contrast
- Improved button styling

## [1.0.0] - 2024-12-01

### Added
- Initial release
- **Windows Services Management**
  - View all installed services
  - Start, Stop, Restart services
  - View service status and start type
  - Search services by name
- Modern WPF dark theme interface
- Administrator privilege requirement
- Real-time status updates

---

## Upcoming Features

- [ ] Service startup type modification
- [ ] Create new scheduled tasks
- [ ] Export/Import service configurations
- [ ] Performance monitoring graphs
- [ ] Multi-language support
- [ ] Portable version (no installation required)
