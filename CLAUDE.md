# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is an ASP.NET MVC exam result management system ("Quản lý điểm thi") written in C#. The application manages exam results, students, exam rooms, and administrative functions for educational institutions.

## Development Commands

Since this is a classic ASP.NET MVC project, development primarily uses Visual Studio:

- **Build**: Use Visual Studio Build menu or `msbuild "Quản lý điểm thi.sln"`
- **Run**: F5 in Visual Studio or IIS Express
- **Package Restore**: Visual Studio automatically restores NuGet packages from `packages.config`

## Database Configuration

The application uses Entity Framework 6 with SQL Server:
- Connection string in `Web.config` points to local SQL Server instance `DESKTOP-BAIF60J` with database `QLDT`
- Entity Framework models are in `Models/Model1.edmx` (Database First approach)
- Main DbContext class: `QLDTEntities1`

## Architecture and Structure

### Controllers Structure
The application follows standard MVC pattern with these main controllers:
- `HomeController` - Main dashboard, student search, authentication
- `StudentController` - Student management
- `ExamController` - Exam management
- `KetQuaController` - Results management
- `UserController` - User account management
- `ManagementUserController` - User administration
- Various lookup controllers (Gender, ExamRoom, Settings, etc.)

### Key Models
- `Student` - Student information and exam data
- `StudentModel` - ViewModel for student display
- `Exam` - Exam sessions
- `ExamRoom` - Exam room assignments
- `KetQua` - Exam results
- `User` - System users with role-based access

### Authentication & Authorization
- Session-based authentication using `Session["IsLogin"]`
- Role-based access control with `Session["RoleIDofUser"]`
- User roles defined in `UserRole.cs`

### File Handling
The application handles:
- Excel file imports/exports using EPPlus library
- PDF document management
- Image uploads for user profiles
- File storage in organized directory structure under `Excel/`, `PDF/`, and `Image/` folders

### Database Entities
Key entities include:
- DienUuTien (Priority categories)
- Exam, ExamRoom
- Students with exam room assignments
- Results (KetQua)
- Administrative entities (Truong, HoiDongThi)
- Classification entities (XepLoai*)

### Frontend
- Uses AdminLTE template for admin interface
- Bootstrap 3.x for responsive design
- jQuery for client-side interactions
- Custom JavaScript files for specific functionality in `scripts/` folder

## Important Notes

- The application uses Vietnamese language throughout (controller names, database fields, UI)
- File paths contain Unicode characters - ensure proper encoding when working with files
- Excel templates are provided in `Content/Excel/` for data import/export
- The system manages exam results with complex relationships between students, exam rooms, and exam sessions