# Stillness

## Overview

Stillness is a web application for managing and displaying images. Users can register, log in, and view their profile with personal info and posts. The main page features a beautiful gallery of photos. Clicking a photo opens a detailed view with its description. On the detail page, clicking the photo immediately downloads the image to the user’s computer.

---

## Technologies

- ASP.NET Core  
- C#  
- Dependency Injection  
- Sessions  
- SQL Database  
- MVC with ViewModels (single and multiple models)  
- Bootstrap  
- JavaScript  
- Masonry Grid Layout  

---

## Features

- User registration and authentication via sessions  
- User profile page with user info and posts  
- Responsive gallery on the main page using masonry grid  
- Photo detail page with description accessible by clicking a photo  
- Direct download of the photo when clicking on the image in the detail view  

---

## Installation

1. Clone the repository.  
2. In the root folder, locate the `Database.sql` script.  
3. Use this script to create and initialize the SQL database. For example, run it in your SQL Server or SQLite tool.  
4. Configure the database connection string in `appsettings.json`.  
5. Build and run the project.  
6. Register a user and start uploading/viewing photos.  

---
