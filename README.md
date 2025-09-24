🎉 Cavista Event Celebration

The Cavista Event Celebration solution is a full-stack application that manages employee events and sends automated notifications. It helps organizations celebrate important milestones like birthdays, work anniversaries, wedding anniversaries, and any custom events defined by the user.

🤝 Contributors and Authors (SparkHub)

- [@E-kene](https://www.github.com/E-kenny)
- [@mulki1](https://www.github.com/mulki1)
- [@sakinlade](https://www.github.com/sakinlade)
- [@Toyin](https://www.github.com)
- [@Ruth](https://www.github.com/)

## Acknowledgements

 - [Cavista Technology](https://www.cavistatech.com/)

It includes a .NET backend (Web API with background jobs) and a React frontend for user-friendly management.

🚀 Features

👤 User Management

Add, update, delete employees

Search and paginate employee records

📅 Event Management

Create, update, delete custom events (not limited to birthdays or anniversaries)

Flexible HTML templates for different event types

📧 Notifications

Personalized email notifications with HTML templates

Send celebratory messages to Microsoft Teams channels

Placeholder tokens for dynamic personalization

⚙️ Background Jobs

Automated event checks using Hangfire (daily scheduled jobs)

🔐 Authentication & Authorization

JWT-based authentication

Role-based access control

💻 Frontend (React)

Manage employees & events with a clean UI

Secure login and dashboard

Event list, search, and notifications overview

🛠️ Technologies Used

Backend

.NET 8 (Web API)

Entity Framework Core (Database ORM)

Hangfire (Background jobs & dashboard)

Serilog (Logging)

Frontend

React (Vite setup)

Axios (API communication)

TailwindCSS / Bootstrap (styling)

Other

SQL Server / PostgreSQL (Database)

SMTP (Email notifications)

Microsoft Teams (via channel email address)

Docker (Deployment & containerization)

📂 Project Structure
CavistaEventCelebration/
│
├── CavistaEventCelebration.Api/        # Backend Web API
│   ├── Controllers/                    # API endpoints
│   ├── Models/                         # Entities (Employee, Event, etc.)
│   ├── Services/                       # Business logic
│   │   ├── Interfaces/                 
│   │   └── Implementation/             
│   ├── Repositories/                   # Data access
│   ├── EmailTemplates/                 # HTML email templates
│   ├── Program.cs / Startup.cs         
│   └── ServiceRegistration.cs          
│
├── Client.React/      # Frontend (React + Vite)
│   ├── src/
│   │   ├── components/                 # UI components
│   │   ├── pages/                      # Pages (Login, Dashboard, Events, Employees)
│   │   ├── services/                   # Centralized API client
│   │   ├── hooks/                      # Custom React hooks
│   │   └── App.tsx                     
│   └── package.json                    
│
├── CavistaEventCelebration.Tests/      # Unit & integration tests
│
├── docker-compose.yml                  
├── CavistaEventCelebration.sln         
└── README.md                           

⚙️ Installation & Setup
Prerequisites

.NET 7 SDK

Node.js 18+ (for React frontend)

SQL Server / PostgreSQL

Docker (optional, for containerized deployment)

Backend Setup

Clone the repository

git clone https://github.com/your-org/cavista-event-celebration.git
cd cavista-event-celebration


Update appsettings.json (see sample below)

Run database migrations

dotnet ef database update --project CavistaEventCelebration.Api


Run the backend

dotnet run --project CavistaEventCelebration.Api


Backend available at:

Swagger: https://localhost:5001/swagger

Hangfire Dashboard: https://localhost:5001/hangfire

Frontend Setup

Navigate to the frontend project:

cd CavistaEventCelebration.React


Install dependencies:

npm install


Run the React app:

npm run dev


Frontend available at: http://localhost:5173

📧 Notifications

Email

Uses HTML templates in EmailTemplates/EventTemplate.html

Supports placeholders like {FirstName}, {EventTitle}, etc. for personalization

Teams

Uses channel email address to send notifications directly to a Teams channel

📜 Sample appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CavistaEvents;User Id=sa;Password=YourPassword;"
  },
  "Jwt": {
    "Key": "SuperSecretKeyHere",
    "Issuer": "Cavista",
    "Audience": "CavistaUsers"
  },
  "SmtpSettings": {
    "Host": "smtp.yourmail.com",
    "Port": 587,
    "UserName": "no-reply@yourdomain.com",
    "Password": "yourpassword",
    "EnableSsl": true
  },
  "Teams": {
    "ChannelEmail": "your-team-channel@teams.microsoft.com"
  }
}

🚦 Limitations

❌ Bulk email personalization is template-based (placeholders), not per-email loop

❌ Requires SMTP & Teams configuration to work

❌ Limited notification channels (currently Email + Teams only)

📌 Future Improvements

✅ Extend notifications to Slack, WhatsApp, and SMS

✅ Add employee self-service portal

✅ Improve template management (admin UI for templates)

✅ Enhance analytics & reporting (event trends, participation)

🤝 Contributing

Fork the repo

Create a feature branch

Commit your changes

Open a pull request