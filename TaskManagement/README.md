
---

## 🧠 System Architecture

The project follows a clean layered architecture (DDD-style):

- API Layer
- Application Layer
- Infrastructure Layer
- Domain Layer

---

## ⚡ Background Processing

When a task is created:
- It is saved in the database
- It is pushed to a background queue
- A BackgroundService simulates processing in the background

---

## 🧊 Redis Cache

- Used in `Get Task By Id` endpoint
- First request → data loaded from database
- Next requests → data served from Redis cache
- Cache is invalidated when task is updated

---

## 🚦 Rate Limiting

- Each user is limited to **5 requests per minute**
- Implemented using custom middleware

---

## ⚠️ Global Exception Handling

- Centralized exception handling middleware
- Returns consistent error responses for all APIs

---

## 🧪 Swagger UI

Swagger is enabled for easy API testing.

Run the project and open:
