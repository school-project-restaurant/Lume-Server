# Your API Name

This document provides details about the API endpoints available for **Your API Name**.

## Base URL

`https://localhost/api`

---

# **/customer** 
## **Method:** `GET`

#### Description
Retrieves a list of all customers

#### Request
```http
GET /api/customers HTTP/1.1
Host: yourapi.domain.com
Content-Type: application/json
```
#### Sample Response
```json
{
  "customers": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "baderotipu",
      "surname": "lupine",
      "email": "baderotipulupine@example.com",
      "phoneNumber": "+11234567890",
      "reservationsId": [
        "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "2a1cfaad-b261-4e3a-9b3a-d1fbe8ab1234"
      ]
    },
    {
      "id": "123e4567-e89b-12d3-a456-426614174001",
      "name": "cefoguhako",
      "surname": "mireline",
      "email": "cefoguhakomireline@example.com",
      "phoneNumber": "+19876543210",
      "reservationsId": [
        "d9428888-122b-11e1-b85c-61cd3cbb3210",
        "a12f64e2-3b4e-4dce-bbf6-3f0fe27cde99",
        "f47ac10b-58cc-4372-a567-0e02b2c3d479"
      ]
    },
    {
      "id": "123e4567-e89b-12d3-a456-426614174002",
      "name": "dafelimonu",
      "surname": "ternavo",
      "email": "dafelimonuternavo@example.com",
      "phoneNumber": "+17654321098",
      "reservationsId": [
        "9c858901-8a57-4791-81fe-4c455b099bc9",
        "7c9e6679-7425-40de-944b-e07fc1f90ae7",
        "16fd2706-8baf-433b-82eb-8c7fada847da",
        "fdda765f-fc57-5604-a269-52a7df8164ec"
      ]
    }
  ]  
}
```
## **Method:** `POST`

#### Description
Add a list of customer to the database, can also be 1

## Request
```json
{
  "customers": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440000",
      "name": "baderotipu",
      "surname": "lupine",
      "email": "baderotipulupine@example.com",
      "phoneNumber": "+11234567890",
      "reservationsId": [
        "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "2a1cfaad-b261-4e3a-9b3a-d1fbe8ab1234"
      ]
    },
    {
      "id": "123e4567-e89b-12d3-a456-426614174001",
      "name": "cefoguhako",
      "surname": "mireline",
      "email": "cefoguhakomireline@example.com",
      "phoneNumber": "+19876543210",
      "reservationsId": [
        "d9428888-122b-11e1-b85c-61cd3cbb3210",
        "a12f64e2-3b4e-4dce-bbf6-3f0fe27cde99",
        "f47ac10b-58cc-4372-a567-0e02b2c3d479"
      ]
    },
    {
      "id": "123e4567-e89b-12d3-a456-426614174002",
      "name": "dafelimonu",
      "surname": "ternavo",
      "email": "dafelimonuternavo@example.com",
      "phoneNumber": "+17654321098",
      "reservationsId": [
        "9c858901-8a57-4791-81fe-4c455b099bc9",
        "7c9e6679-7425-40de-944b-e07fc1f90ae7",
        "16fd2706-8baf-433b-82eb-8c7fada847da",
        "fdda765f-fc57-5604-a269-52a7df8164ec"
      ]
    }
  ]  
}
```
## Sample Response
```http
201 Created
```
or
```http
404 Not found
```

