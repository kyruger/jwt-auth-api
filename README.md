# JWT Authentication API (.NET 8)

A RESTful authentication API built with **ASP.NET Core (.NET 8)** that demonstrates **JWT-based authentication with Refresh Token rotation**, following real-world backend best practices.

This project was built as a hands-on learning project and is designed to reflect **production-ready authentication flows** rather than a simple demo.

---

## 🚀 Features

* User registration and login
* Password hashing (SHA256)
* JWT Access Token generation
* Refresh Token implementation
* Refresh Token Rotation
* Refresh Token stored securely in **HttpOnly Cookies**
* Logout endpoint with refresh token revocation
* Role-based authorization support
* Token reuse protection logic
* Entity Framework Core with SQL Server
* Swagger support for API testing

---

## 🔐 Authentication Flow

### Login

* User logs in with email & password
* Server returns:

  * **Access Token** (JWT) in response body
  * **Refresh Token** stored in HttpOnly Cookie

### Access Token

* Short-lived
* Sent via `Authorization: Bearer <token>` header
* Used to access protected endpoints

### Refresh Token

* Long-lived
* Stored securely in HttpOnly Cookie
* Used to request a new access token when expired

### Refresh Token Rotation

* Old refresh token is revoked on use
* A new refresh token is issued and saved
* Prevents token reuse attacks

### Logout

* Refresh token is revoked in the database
* Refresh token cookie is deleted

---

## 🧪 Testing

The API can be tested using **Swagger** or **Postman**:

1. Login to receive an access token
2. Use the access token for protected endpoints
3. Let the access token expire
4. Call the refresh token endpoint (cookie is sent automatically)
5. Receive a new access token
6. Logout to invalidate the refresh token

---

## ⚙️ Technologies Used

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQL Server
* JWT (JSON Web Tokens)
* Swagger / OpenAPI

---

## 📌 Notes

* Refresh tokens are never exposed to JavaScript
* Token rotation is implemented to increase security
* Project structure and patterns follow clean backend practices

---

## 📄 License

This project is for educational and portfolio purposes.

