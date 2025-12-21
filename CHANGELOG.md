# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
