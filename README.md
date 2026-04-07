# 🌌 UttuHub | The Bento Portfolio API

**UttuHub** is a high-performance, specialized backend built with **ASP.NET Core 10** and **SQL Server**. It serves as the engine for a modern "Bento-style" portfolio, managing projects, a live life-feed, and visitor inquiries.

---

## 🚀 Sprint 1: The Walking Skeleton (Completed)
- [x] **Relational Data Modeling:** Architected a 5-table schema for Users, Projects, Categories, FeedItems, and Contacts.
- [x] **Entity Framework Core Integration:** Configured `AppDbContext` with Fluent API for strict data relationships.
- [x] **Database Migration:** Successfully deployed the schema to `SQLEXPRESS` via EF Migrations.
- [x] **Swagger/OpenAPI Setup:** Integrated interactive documentation for endpoint testing.

---

## 🛠️ Tech Stack
* **Framework:** .NET 10 (ASP.NET Core API)
* **ORM:** Entity Framework Core
* **Database:** MS SQL Server (Express)
* **Architecture:** Layered (Models, Data, Controllers)
* **Tools:** Swagger/OpenAPI, Git, NuGet

---

## 📊 Database Architecture (ERD)
The system uses a centralized `User` identity to own all content, ensuring future scalability for multi-tenant support.

* **Projects:** Detailed technical showcase with GitHub and Live URL links.
* **FeedItems:** Social/Bento tiles categorized for personal brand building.
* **Categories:** The taxonomy engine that powers the frontend filtering logic.
* **Contacts:** Independent lead-generation system for visitor messages.

---


