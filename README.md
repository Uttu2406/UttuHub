# UttuHub | The Bento Portfolio API

**UttuHub** is a high-performance, specialized backend built with **ASP.NET Core 8** and **SQL Server**. It serves as the engine for a modern "Bento-style" portfolio, managing projects, a live life-feed, and visitor inquiries.

---

## Sprint 1: The Walking Skeleton (Completed)
- [x] **Relational Data Modeling:** Architected a 5-table schema for Users, Projects, Categories, FeedItems, and Contacts.
- [x] **Database Migration:** Successfully deployed the schema to `SQLEXPRESS` with `isVerified` access control.
- [x] **Identity Security:** Implemented **BCrypt.Net** for secure password hashing and verification.
- [x] **JWT Authentication:** Integrated JSON Web Tokens with a global "Authorize" padlock in Swagger.
- [x] **Stable Environment:** Reverted to .NET 8 for robust Swashbuckle/OpenAPI support and namespace stability.

---

## Tech Stack
* **Framework:** .NET 8 (ASP.NET Core API)
* **Security:** JWT Bearer Authentication & BCrypt.Net-Next
* **ORM:** Entity Framework Core
* **Database:** MS SQL Server (Express)
* **Architecture:** 3-Tier (Models, Data, Controllers)
* **Tools:** Swagger/OpenAPI (Swashbuckle), SSMS

---

## Feature Highlights

### **Secure Identity Management**
* **Verification Gate:** New users are registered with `isVerified = false` by default, requiring manual admin approval.
* **Stateless Auth:** Uses JWT to handle secure requests across the Portfolio frontend.

### **Database Architecture (ERD)**
The system uses a centralized `User` identity to own all content, ensuring future scalability for multi-tenant support.

* **Projects:** Detailed technical showcase with GitHub and Live URL links.
* **FeedItems:** Social/Bento tiles categorized for personal brand building.
* **Categories:** The taxonomy engine that powers frontend filtering logic.
* **Contacts:** Independent lead-generation system for visitor messages.
