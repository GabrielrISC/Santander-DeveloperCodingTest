# Hacker News Best Stories API

A simple RESTful API built with **ASP.NET Core 8** that exposes a single endpoint to fetch the best stories from the official Hacker News API.

## 🚀 Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

You will need the following installed:

* **.NET 8 SDK** or later.
* An IDE like **Visual Studio 2022** or **Visual Studio Code** with the C# Dev Kit extension.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd <repository-directory>
    ```

2.  **Restore dependencies:**
    The project uses implicit package restore, but you can explicitly run:
    ```bash
    dotnet restore
    ```

### Running the API

1.  **Run the application from the project root directory:**
    ```bash
    dotnet run
    ```
    This will start the Kestrel web server, typically on `https://localhost:7001` (or a similar port). The exact URL will be shown in the console output.

2.  **Access the Swagger UI (Optional):**
    If the project is configured with Swagger, you can usually view the documentation and test the endpoint at a URL like `https://localhost:7001/swagger`.

---

## 💻 API Endpoint

The API exposes a single `GET` endpoint to retrieve the best stories, which are mapped to a simplified DTO for the client.

### Get Best Stories

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/HackerStories/best` | Retrieves a list of the current best stories from Hacker News. |

#### Parameters

| Name | Type | Description | Default |
| :--- | :--- | :--- | :--- |
| `limit` | `integer` | The maximum number of best stories to return. Must be greater than 0. | 5 |

#### Example Request
