# Users Module

This module manages user accounts for mentors and mentees.

-   Registration and authorization (email/password, Google login).
-   User profiles:
    -   Mentors: skills, industry, experience, programming languages, communication language.
    -   Mentees: desired skills, learning goals, communication language.
-   User roles (mentor, mentee, administrator).
-   Account settings management.
-   User management (active/inactive accounts).

## **Epic: Users Module Development**

**Description:** This epic includes all tasks related to managing user profiles, roles, mentor-mentee connections, and user management in the system.

---

### **1. User Profile Management**

**Story:** As a user, I want to **create and update my profile** with relevant details.

#### **Tasks:**

**USER-001 Extend IdentityUser for Custom User Properties**
Add properties for Industry, Skills, ProgrammingLanguages, Experience, CommunicationLanguage.
Use **flags enum** for Industry.

**USER-003 Implement User Profile API**
Add GET /api/users/profile (fetch profile data).
Add PUT /api/users/profile (update profile details).

**USER-004 Implement File Upload for Profile Avatars**
Create an Avatar field in the user table.
Use **Azure Blob Storage** or **local file storage**.
Add POST /api/users/avatar (upload and update avatar).

**USER-006 Add Email Confirmation & Verification**
Implement email verification on signup.
Use ASP.NET Identity's **Email Confirmation Token**.
Send confirmation email using Azure Communication Services.

**USER-007 Implement Password Reset Flow**
Use ASP.NET Identity's built-in reset password mechanism.
Implement POST /api/users/reset-password.

**USER-008 Implement Account Deactivation**
Add IsActive flag to IdentityUser.
Implement API DELETE /api/users/deactivate.
Send notification email using Azure Communication Services.

---

### **2. User Management (Admin)**

**Story:** As an admin, I want to **manage users, deactivate accounts, and moderate content**.

#### **Tasks:**

**ADMIN-001 Implement User Management API (Admin Panel)**
GET /api/admin/users (list all users).
PUT /api/admin/users/{id}/toggle-active (activate/deactivate user).
DELETE /api/admin/users/{id} (delete user).

**ADMIN-002 Implement Role Management API**
Allow role assignment via API (Admin, Mentor, Mentee).
POST /api/admin/assign-role.
