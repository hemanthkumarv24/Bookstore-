# Book Management API (.NET 8)

A minimal ASP.NET Core Web API for managing books with JWT authentication, role-based authorization, and in-memory storage (no database).

## Features
- In-memory lists for users, books, and active JWT tokens
- BCrypt password hashing
- JWT login/authentication
- Role-based authorization:
  - **Admin**: Create, Update, Delete
  - **User**: Read only
- Custom middleware to validate tokens against the private active-token queue
- Swagger UI with JWT Bearer support

## Run the project
```bash
dotnet restore
dotnet run
```

## Swagger URL
- `http://localhost:5233/swagger`

## Sample login credentials
- **Admin**
  - Email: `admin@bookstore.com`
  - Password: `Admin@123`
- **User**
  - Email: `user@bookstore.com`
  - Password: `User@123`

## Use JWT token in Swagger
1. Call `POST /api/auth/login` with valid credentials.
2. Copy the `token` from the response.
3. Click **Authorize** in Swagger.
4. Enter: `Bearer <token>`
5. Call protected endpoints like `/api/books`.
