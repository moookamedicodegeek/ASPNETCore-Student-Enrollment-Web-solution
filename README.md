ASP.NET Core Web Solution – Student/Admin Authentication System
📌 Overview
This project is a web-based solution built using ASP.NET Core (MVC with Onion Architecture) that provides a structured and secure system for authentication and registration of both students and administrators.

🚀 Features
🔐 Built-in authentication and user registration for students and admins

🧅 Onion Architecture for a clean separation of concerns

🎨 Enhanced user interface using HTML and CSS

🔍 Functional search capabilities for quick access to data

📄 Fully styled and responsive CRUD operations:

Create

Edit

Details

Delete

🛠️ Technologies Used
ASP.NET Core MVC

C#

HTML & CSS

Entity Framework Core

ASP.Authentication

SQLiteServer

📁 Project Structure (Onion Layered)
www.root
Copy
Edit
/SolutionName
│
├── /Core (Interfaces, Domain Models)
├── /Infrastructure (Data Access Layer)
├── /Application (Business Logic)
└── /Web (UI Layer - ASP.NET Core MVC)
🎯 Getting Started
Clone the Repository

bash
Copy
Edit
git clone https://github.com/yourusername/your-repo-name.git
cd your-repo-name
Configure the Database

Update your connection string in appsettings.json.

Apply Migrations & Update Database

bash
Copy
Edit
dotnet ef database update
Run the Application

bash
Copy
Edit
dotnet run


✅ Future Improvements
Role-based dashboard views

Email confirmation & password recovery : To login as admin :Username is "admin@gmail.com" and Password is"Test!123";
There are limited priviledges to both Admin and user
To check Student detail as user, first register then enroll as student

User activity logging

🤝 Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you'd like to change.

📄 License
MIT
