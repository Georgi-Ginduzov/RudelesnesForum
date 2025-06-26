# Project: Forum with Automatic Moderation of Rude Comments

**Student:** \[Your Name]
**Faculty Number:** \[Your Faculty Number]
**Semester:** \[Semester]

---

## Table of Contents

1. [Assignment Description](#assignment-description)
2. [Solution Overview](#solution-overview)
3. [Features](#features)
4. [Technology Stack](#technology-stack)
5. [Architecture](#architecture)
6. [Installation](#installation)
7. [Configuration](#configuration)
8. [Usage](#usage)
9. [ML Model Training](#ml-model-training)
10. [Moderation Workflow](#moderation-workflow)
11. [Roles & Permissions](#roles--permissions)
12. [Code Formatting & Documentation](#code-formatting--documentation)
13. [References](#references)

---

## Assignment Description

This project implements a web-based forum with automatic moderation of rude comments using sentiment analysis. The solution is divided into two key components: a fully functional .NET Core application coded in C#, and comprehensive documentation describing the implementation in detail.

The forum supports three user roles:

* **Users:** Can post comments.
* **Moderators:** Receive flagged (potentially rude) comments in a dedicated interface and decide to approve or reject.
* **Administrators:** Manage user accounts (activate/deactivate), assign or revoke moderator rights (but cannot moderate comments).

Every comment is analyzed in real-time by an ML.NET 2.0 sentiment analysis model (NAS‑BERT) pretrained on user-provided comments. Comments deemed rude are flagged for moderator review; others are published automatically.

---

## Solution Overview

The application is built on .NET Core and C#. It uses ML.NET 2.0 for sentiment classification with a NAS‑BERT model. The front-end employs Razor pages for UI, with a SQL Server database backend storing users, comments, roles, and moderation status.

Documentation is provided as a separate PDF in Moodle; this README focuses on repository setup, running the application, and understanding core components.

---

## Features

* **Real-time Sentiment Analysis:** Each comment is classified as “clean” or “rude.”
* **Automated Publishing:** Clean comments appear immediately in threads.
* **Moderator Queue:** Rude comments are queued for manual approval.
* **Role Management:** Admins can enable/disable users, assign moderator rights.
* **Scalable ML Model:** Easy retraining and replacement of sentiment model.

---

## Technology Stack

* **Language & Framework:** C#, .NET Core 6.0 (or later)
* **ML Library:** ML.NET 2.0
* **Model Architecture:** NAS‑BERT
* **Data Storage:** SQL Server (LocalDB or full)
* **Front-end:** ASP.NET Core Razor Pages

---

## Architecture

```plaintext
[User] ↔ [Razor UI] ↔ [Controller] ↔ [CommentService]
                             ↕            ↕
                         [MLModel]    [Repository]
                             ↕            ↕
                         [ML.NET]   [SQL Server]
```

* **Razor UI:** Page handlers for posting/viewing comments.
* **CommentService:** Orchestrates ML calls and repository operations.
* **MLModel:** Loads and predicts sentiment via ML.NET.
* **Repository:** Entity Framework Core handles data persistence.

---

## Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/your-org/forum-moderation.git
   cd forum-moderation
   ```
2. **Restore packages:**

   ```bash
   dotnet restore
   ```
3. **Set up database:**

   * Ensure SQL Server or LocalDB is installed.
   * Modify `appsettings.json` connection string as needed.
   * Run migrations:

     ```bash
     dotnet ef database update
     ```

---

## Configuration

Configure settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ForumDb;Trusted_Connection=True;"
  },
  "SentimentModelPath": "MLModels/SentimentModel.zip"
}
```

* **DefaultConnection:** SQL Server connection string.
* **SentimentModelPath:** Path to the pretrained ML.NET model file.

---

## Usage

1. **Train or load ML model** (see [ML Model Training](#ml-model-training)).
2. **Start the application:**

   ```bash
   dotnet run --project src/ForumApp
   ```
3. **Browse to:** `https://localhost:5001`
4. **Register** as a User.
5. **Post comments** in threads—clean comments appear immediately; rude comments go to Moderator Dashboard.
6. **As Admin,** manage user accounts and assign moderator roles via Admin Panel.

---

## ML Model Training

### Data Preparation

1. Collect and label comments as `Clean` or `Rude`.
2. Export to a CSV with columns: `Text`, `Label`.

### Training

Use the C# notebook example in `Notebooks/E2E-Text-Classification-API-with-Yelp-Dataset.ipynb` adapted for your data. Or follow:

```csharp
var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(CommentData.Text))
    .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());
```

Save the model:

```csharp
mlContext.Model.Save(model, data.Schema, "MLModels/SentimentModel.zip");
```

### Loading & Prediction

In `SentimentService.cs`:

```csharp
_dataView = mlContext.Model.Load(modelPath, out var schema);
_predictionEngine = mlContext.Model.CreatePredictionEngine<CommentData, CommentPrediction>(_dataView);
```

---

## Moderation Workflow

1. **User posts** a comment.
2. **Service analyzes** sentiment.
3. **If flag = Rude:** comment saved with `Status = Pending`; appears in `Moderation` page.
4. **Moderator reviews:** clicks Approve or Reject.
5. **Approved:** status changes to `Approved` and comment becomes visible.
6. **Rejected:** record removed or status `Rejected`.

---

## Roles & Permissions

| Role          | Permissions                                         |
| ------------- | --------------------------------------------------- |
| **User**      | Post comments                                       |
| **Moderator** | Review pending comments, Approve/Reject             |
| **Admin**     | Activate/Deactivate users, Assign/Revoke moderators |

---

## Code Formatting & Documentation

* Code follows C# coding conventions and naming guidelines.
* XML comments used for public methods and classes.
* Formatting enforced by `.editorconfig` and `dotnet format`.

---

## References

* ML.NET Sentiment Sample: [TextClassification\_Sentiment\_Razor README](https://github.com/dotnet/machinelearning-samples/blob/main/samples/modelbuilder/TextClassification_Sentiment_Razor/README.md)
* C# E2E Text Classification with Yelp Dataset: [Notebook](https://github.com/dotnet/csharp-notebooks/blob/main/machine-learning/E2E-Text-Classification-API-with-Yelp-Dataset.ipynb)

---

*End of README*
