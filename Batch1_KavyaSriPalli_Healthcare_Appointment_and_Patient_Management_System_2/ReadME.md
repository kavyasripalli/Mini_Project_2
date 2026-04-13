# 🏥 Healthcare Appointment & Patient Management System

## 📌 Project Overview

This is a **web-based Healthcare Management System** developed using **ASP.NET Core MVC with Entity Framework Core (Code First approach)**.
The application helps clinics efficiently manage:

* 👨‍⚕️ Doctors
* 🧑‍🤝‍🧑 Patients
* 📅 Appointments

The system eliminates manual record-keeping and ensures **data consistency, validation, and appointment scheduling without conflicts**.

---

## 🚀 Tech Stack

| Layer             | Technology                     |
| ----------------- | ---------------------          |
| UI Layer          | ASP.NET Core MVC               |
| Business Layer    | C# Class Library               |
| Data Access Layer | Entity Framework Core + Dapper |
| Database          | SQL Server                     |
| Architecture      | Layered Architecture           |

---

## 🏗️ Solution Architecture

```
HealthcareApp.sln
│
├── HealthcareApp.Web        (MVC Layer)
├── HealthcareApp.BLL        (Business Logic Layer)
├── HealthcareApp.DAL        (Data Access Layer)
├── HealthcareApp.Entities   (Domain Models)
```

---

## 📊 Core Modules

### 👤 Patient Management

* Add / Edit / Delete Patients
* Search functionality
* Validations (Age, Phone, Email)

---

### 👨‍⚕️ Doctor Management

* Add / Edit / Delete Doctors
* Manage availability time slots

---

### 📅 Appointment Management

* Book Appointment
* Prevent duplicate booking
* Update status:

  * Booked
  * Completed
  * Cancelled
* Filter by:

  * Date
  * Status
* Pagination support
* Table + Card view UI

---

## ⚙️ Business Rules Implemented

* ❌ Appointment date cannot be in the past
* ❌ Doctor cannot have overlapping appointments
* ❌ Patient cannot have overlapping appointments
* ❌ Editing/Deleting **completed appointments is restricted**
* ⏱️ 15-minute slot validation between appointments

---

## ✅ Validations Implemented

| Field | Validation             |
| ----- | ---------------------- |
| Name  | Required               |
| Age   | Range (1–120)          |
| Phone | 10-digit validation    |
| Email | Proper format          |
| Date  | Must be current/future |

---

## 🎨 UI Features

* Responsive design using **Bootstrap**
* Dashboard with:

  * 📊 Appointment status pie chart
  * 📌 Today's upcoming appointments
* Table & Card toggle view
* Navigation bar with icons
* Toast notifications (Success/Error)

---

## 📈 Advanced Features

* 🔍 Server-side filtering (Date, Status, Search)
* 📄 Pagination (Previous / Next)
* 🔒 Action restrictions for completed appointments
* 📊 Chart.js integration (Pie Chart)
* 🧠 Clean layered architecture

---

## 🪵 Logging

Logging is implemented using **ILogger** in controllers:

* Tracks:

  * Appointment creation
  * Updates
  * Deletions
  * Errors & exceptions
* Helps in debugging and monitoring application behavior

---

## 🗄️ Database

* SQL Server (Code First)
* Migrations used:

  ```bash
  Add-Migration InitialCreate
  Update-Database
  ```

---

## 🌐 Deployment

The application is successfully deployed using:

### 🔧 IIS (Internet Information Services)

Steps followed:

1. Published project using:

   ```
   Build → Publish → Folder
   ```
2. Configured IIS:

   * Created new site
   * Set physical path to published folder
   * Configured application pool
3. Accessed application via IIS local hosting

---


## 🎯 Key Learnings

* Implementation of **Layered Architecture**
* Hands-on with **Entity Framework Core (Code First)**
* Building **real-world CRUD applications**
* Applying **business rules and validations**
* Integrating **charts and UI enhancements**
* Deploying applications using **IIS**

---

## 📌 Conclusion

This project demonstrates a **complete end-to-end ASP.NET Core MVC application** with:

* ✔ Clean architecture
* ✔ Strong validation logic
* ✔ User-friendly UI
* ✔ Real-world business rules
* ✔ Deployment experience

---