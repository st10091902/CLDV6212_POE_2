# 🌐 ABC Retail – Azure Storage Solution

This project is an ASP.NET Core MVC web application that integrates with various **Azure Storage services**.  
It was developed as part of the CLDV6212 module (Cloud Development) to demonstrate the implementation of cloud-based storage functionalities for a retail scenario.

---

## 📌 Project Overview

**ABC Retail** is a cloud-enabled web solution that allows:
- Storing customer and product data using **Azure Table Storage**
- Uploading and managing product images using **Azure Blob Storage**
- Sending and receiving order information through **Azure Queue Storage**
- Uploading contract files to **Azure File Share**

This solution demonstrates how cloud services can be leveraged to build scalable, secure, and reliable data-driven applications.

---

## 🏗️ Technologies Used

- **ASP.NET Core 8 (MVC)**  
- **C#**  
- **Azure Storage Services**:
  - Table Storage
  - Blob Storage
  - Queue Storage
  - File Share
- **Entity Framework Core**
- Visual Studio 2022

---

## ☁️ Azure Services Setup

This application connects to the following Azure services:

| Service Type       | Name                | Description |
|--------------------|---------------------|-------------|
| **Storage Account**| abcretailstorage…   | Central storage for tables, blobs, queues & files |
| **Table Storage**  | Customers / Products| Stores customer & product data |
| **Blob Container** | product-images      | Stores product images |
| **Queue**          | orders              | Stores JSON order messages |
| **File Share**     | contracts           | Stores uploaded contract files |
| **App Service**    | abcretail-st10091902| Hosts the deployed website |

---

## ⚙️ Configuration

All configuration is stored in `appsettings.json` or in **Azure App Service → Configuration**.

```json
"Storage": {
  "ConnectionString": "DefaultEndpointsProtocol=...;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net",
  "BlobContainer": "product-images",
  "QueueName": "orders",
  "FileShareName": "contracts",
  "TableCustomers": "Customers",
  "TableProducts": "Products"
}
