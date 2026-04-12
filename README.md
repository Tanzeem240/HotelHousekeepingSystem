#  Hotel Housekeeping & Room Status Management System

##  Overview
This project is a web-based internal application developed for **COMP 2154 – System Development Project** at George Brown College.

The system centralizes hotel housekeeping operations by automating room status updates, task assignments, and maintenance tracking, replacing manual processes with a structured digital workflow.

---

## Features

### Authentication & Authorization
- Role-based login system (Manager & Worker)
- Session-based authentication
- Access control using custom role filters

### Room Management
- Room lifecycle tracking:
  - Available → Occupied → Dirty → Cleaning → Clean → Available
- Room categories (Regular, Deluxe, Suite, Presidential)
- Automated status updates on check-in/check-out

###  Housekeeping Task Workflow
- Auto-create cleaning tasks after check-out
- Task assignment by manager
- Worker task updates (Mark Cleaned)
- Manager approval workflow

###  Maintenance Module
- Report issues with priority levels
- Critical issues mark room **Out of Service**
- Issue resolution restores room availability

###  Dashboard & Reports
- Real-time room status overview
- Cleaning performance analytics (Chart.js)
- Summary statistics (fastest, average cleaning time)

###  Work Summary
- Clock In / Clock Out system
- Work session tracking
- Daily and historical work logs

###  Worker Profiles
- Staff information management
- Contact details and personal data

---

## Tech Stack

| Layer | Technology |
|------|----------|
| Framework | ASP.NET Core MVC (.NET 9) |
| Language | C# |
| ORM | Entity Framework Core |
| Database | PostgreSQL (Supabase) |
| Frontend | Razor Views + Bootstrap 5 |
| Charts | Chart.js |
| IDE | JetBrains Rider |

---

##  Architecture
The system follows the **MVC (Model-View-Controller)** architecture:

- **Models** → Database structure and entities  
- **Views** → UI (Razor pages)  
- **Controllers** → Business logic and workflows  

---

##  Setup Instructions

### Prerequisites
- .NET 9 SDK
- JetBrains Rider / Visual Studio
- Internet connection (for Supabase DB)

### Steps
```bash
# Clone repository
git clone https://github.com/Tanzeem240/HotelHousekeepingSystem

# Open project
Open HotelHousekeepingSystem.sln in IDE

# Run application
Start using http://localhost:5002
