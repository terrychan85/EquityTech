# IncidentApiTaskA - API Testing & Sample Inputs/Outputs

Swagger UI was added so you can easily test and explore the Incident API directly from your browser, without needing extra tools.

When you run the project (`dotnet run`), just open the application URL (e.g., http://localhost:5148) and you’ll see an interactive homepage where you can try out all API endpoints and view live documentation.

---

## Host Address

```
IncidentApiTaskA_HostAddress: http://localhost:5148
```

---

## Sample Input/Output Scenarios

### A. Successful Creation
**Request:**
```http
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json

{
  "title": "Server Down",
  "description": "Main server is not responding.",
  "severity": "High"
}
```
**Response:**
```http
201 Created
Content-Type: application/json
{
  "id": 1,
  "message": "Incident created successfully."
}
```

---

### B. Duplicate Incident (within 24 hours)
**Request:**
```http
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{
  "title": "Server Down",
  "description": "Main server is not responding.",
  "severity": "High"
}
```
**Response:**
```http
409 Conflict
Content-Type: application/json
{
  "error": "Duplicate incident detected within 24 hours."
}
```

---

### C. Invalid Severity
**Request:**
```http
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{
  "title": "Network Issue",
  "description": "WiFi is unstable.",
  "severity": "Critical"
}
```
**Response:**
```http
400 Bad Request
Content-Type: application/json
{
  "error": "Severity must be one of: Low, Medium, High."
}
```

---

### D. Missing Required Field
**Request:**
```http
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{
  "title": "",
  "description": "No title provided.",
  "severity": "Low"
}
```
**Response:**
```http
400 Bad Request
Content-Type: application/json
{
  "error": "Title, Description, and Severity are required."
}
```

---

### E. Invalid JSON
**Request:**
```http
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{ not valid json }
```
**Response:**
```http
400 Bad Request
Content-Type: application/problem+json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "..."
}
``` 