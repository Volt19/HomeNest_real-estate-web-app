# HomeNest

HomeNest is a real estate listing web application built with **Blazor Server**, **.NET 9**, and **SQLite**. It is designed for the Bulgarian market (specifically Varna) and provides property search, publishing, user authentication, favorites, contact messaging, and an admin dashboard.

---

## Table of Contents

1. [Features](#features)
2. [Technology Stack](#technology-stack)
3. [Project Structure](#project-structure)
4. [Getting Started](#getting-started)
5. [Configuration](#configuration)
6. [Database](#database)
7. [Authentication & Authorization](#authentication--authorization)
8. [Services Overview](#services-overview)
9. [Pages & Routes](#pages--routes)
10. [Deployment Notes](#deployment-notes)

---

## Features

- **Property Search** — Filter by price, area, floor, district, property type, and sort results.
- **Property Publishing** — Authenticated users can publish properties with image uploads (JPG, PNG, WEBP, GIF up to 5MB).
- **Favorites** — Logged-in users can save properties to a personal favorites list.
- **User Authentication** — Custom cookie-based authentication with BCrypt password hashing.
- **Admin Dashboard** — Admin-only panel for viewing contact messages and newsletter subscribers.
- **Contact Form** — Visitors can send inquiries; messages are stored in the database.
- **Newsletter Subscription** — Email subscription component across multiple pages.
- **Responsive Design** — Mobile-first CSS with custom styling and Bootstrap 5 base.

---

## Technology Stack

| Layer | Technology |
|-------|------------|
| Framework | .NET 9.0 Web SDK |
| UI | Blazor Server (Interactive Server render mode) |
| Database | SQLite (`Microsoft.EntityFrameworkCore.Sqlite` 9.0.15) |
| ORM | Entity Framework Core 9.0.15 |
| Authentication | Cookie Authentication (`Microsoft.AspNetCore.Authentication.Cookies`) |
| Password Hashing | BCrypt.Net-Next 4.1.0 |
| Styling | Custom CSS (~3,500 lines) + Bootstrap 5 |
| Fonts | Google Fonts (Poppins, DM Sans, Inter) |

---

## Project Structure

```
HomeNest/
├── Components/
│   ├── App.razor                 # Root HTML shell
│   ├── Routes.razor              # Router configuration
│   ├── _Imports.razor            # Global using statements
│   ├── Auth/
│   │   └── AuthInitializer.razor # Auth state hydration
│   ├── AuthModal.razor           # Login/Register modal component
│   ├── Icons/                    # SVG icon components
│   ├── Layout/
│   │   ├── MainLayout.razor      # Page layout wrapper
│   │   ├── NavMenu.razor         # Navigation bar
│   │   └── Footer.razor          # Page footer
│   ├── NewsletterSection.razor   # Newsletter subscription UI
│   ├── PropertyCard.razor        # Reusable property card
│   └── Pages/                    # All page components
│       ├── Home.razor
│       ├── SearchPage.razor
│       ├── PropertyDetailPage.razor
│       ├── PublishProperty.razor
│       ├── EditProperty.razor
│       ├── MyListings.razor
│       ├── Favorites.razor
│       ├── Login.razor
│       ├── Register.razor
│       ├── Profile.razor
│       ├── AdminDashboard.razor
│       ├── About.razor
│       ├── Contact.razor
│       ├── Team.razor
│       └── Error.razor
├── Data/
│   ├── HomeNestDbContext.cs      # EF Core DbContext
│   ├── HomeNestDbContextFactory.cs
│   └── Models/                   # Entity classes
│       ├── User.cs
│       ├── Property.cs
│       ├── Favorite.cs
│       ├── ContactMessage.cs
│       ├── NewsletterSubscriber.cs
│       └── UserSessionDto.cs
├── Services/
│   ├── UserStateService.cs       # Authentication & user state
│   ├── PropertyService.cs        # Property CRUD & search
│   ├── ContactService.cs         # Contact message management
│   └── NewsletterService.cs      # Newsletter subscriptions
├── wwwroot/
│   ├── app.css                   # Main stylesheet
│   ├── js/auth.js                # Authentication JavaScript helpers
│   ├── images/                   # Static images (hero, team, avatars, etc.)
│   ├── uploads/                  # User-uploaded property images
│   └── lib/bootstrap/            # Bootstrap 5 files
├── appsettings.json
├── appsettings.Development.json
└── homenest.db                   # SQLite database
```

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Run Locally

```bash
# Navigate to the project directory
cd HomeNest

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The application will start on `https://localhost:5001` (or similar — check console output).

### Default Admin Credentials

On first run, the database is seeded with an admin user:

| Field | Value |
|-------|-------|
| Email | `admin@homenest.bg` |
| Password | `admin123` |
| Name | Администратор |

> **Security Note:** Change the default admin password in production.

---

## Configuration

The application uses minimal configuration. The SQLite connection is hardcoded in `Program.cs`:

```csharp
options.UseSqlite("Data Source=homenest.db")
```

`appsettings.json` contains standard logging levels and `AllowedHosts`.

---

## Database

### Entities

| Entity | Description |
|--------|-------------|
| **User** | Registered users with BCrypt-hashed passwords. `IsAdmin` flag for admin access. |
| **Property** | Real estate listings with title, description, price, area, rooms, district, type, floor, furnished status, image, and features. |
| **Favorite** | Many-to-many join between users and saved properties. Unique constraint on `(UserId, PropertyId)`. |
| **ContactMessage** | Messages submitted via the contact form. Includes `IsRead` flag. |
| **NewsletterSubscriber** | Email addresses subscribed to the newsletter. Unique constraint on email. |

### Migrations

To create a new migration after model changes:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

> Ensure the `Microsoft.EntityFrameworkCore.Design` package is available.

---

## Authentication & Authorization

### Architecture

- **Cookie Authentication**: Cookies are `HttpOnly`, `SameSite=Strict`, with 7-day expiry and sliding expiration.
- **Dual-track Login**: The login page calls a JavaScript helper (`auth.js`), which POSTs to a minimal API endpoint (`/api/auth/login`) to set the HTTP cookie. Blazor state is then hydrated via `PersistentComponentState`.
- **BCrypt Hashing**: All passwords are hashed with BCrypt (salted automatically).
- **Admin Access**: Controlled by the `IsAdmin` boolean on the `User` entity. The `[Authorize(Roles = "Admin")]` attribute is used on the admin page.

### Auth Flow

1. User submits login form on `/login`
2. `auth.js` sends credentials to `/api/auth/login`
3. Server validates credentials and issues cookie
4. Page reloads; `AuthInitializer.razor` hydrates auth state from `HttpContext`
5. `UserStateService` maintains reactive state for the UI

---

## Services Overview

### UserStateService

- Manages login state (`IsLoggedIn`, `UserId`, `UserName`, `Email`, `Phone`, `IsAdmin`)
- Handles registration and logout
- Provides favorites toggle/check/get operations
- Raises `OnChange` events for UI reactivity

### PropertyService

- CRUD operations for properties
- Search filtering and sorting
- Similar properties lookup
- Owner-scoped update/delete with ownership verification

### ContactService

- Create/read contact messages
- Unread message count
- Mark messages as read

### NewsletterService

- Subscribe with email validation
- List subscribers and check subscription status

---

## Pages & Routes

| Route | Page | Access |
|-------|------|--------|
| `/` | Home | Public |
| `/search` | Property Search | Public |
| `/property/{id}` | Property Detail | Public |
| `/property/publish` | Publish Property | Authenticated |
| `/property/edit/{id}` | Edit Property | Owner or Admin |
| `/my-listings` | My Listings | Authenticated |
| `/favorites` | Favorites | Authenticated |
| `/login` | Login | Anonymous |
| `/register` | Register | Anonymous |
| `/profile` | Profile Dashboard | Authenticated |
| `/admin` | Admin Panel | Admin only |
| `/about` | About Us | Public |
| `/contact` | Contact | Public |
| `/team` | Team | Public |

---

## Deployment Notes

1. **SQLite**: The database file (`homenest.db`) and uploaded images (`wwwroot/uploads/`) should be persisted between deployments. Ensure your hosting environment allows write access to these locations.

2. **Image Uploads**: The `wwwroot/uploads/` directory must be writable by the application pool / process.

3. **Admin Seeding**: The admin user is seeded on every startup if not present. In production, change the default password immediately after first login.

4. **Allowed Hosts**: Update `AllowedHosts` in `appsettings.json` for production environments.

5. **HTTPS**: Ensure HTTPS is configured in production. The development certificate is not suitable for production use.

---

## License

This project is proprietary and intended for educational/demonstration purposes.
