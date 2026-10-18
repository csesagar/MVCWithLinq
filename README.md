# ASP.NET MVC - LINQ to SQL Database Connection

## Project Overview

This guide explains how to connect an ASP.NET MVC application to SQL Server using **LINQ to SQL** in Visual Studio.

Instead of manually creating model classes and writing ADO.NET database connection code, LINQ to SQL can automatically generate entity classes and a DataContext class from your database tables.

You can use the generated `DataContext` in your controller to perform database operations such as Select, Insert, Update, and Delete.

## Technologies Used

- ASP.NET MVC
- C#
- LINQ to SQL
- SQL Server
- Visual Studio
- LINQ Queries

## Step 1: Add LINQ to SQL to the Models Folder

1. Open your ASP.NET MVC project in Visual Studio.
2. Right-click the `Models` folder.
3. Select **Add → New Item**.
4. Search for **LINQ to SQL Classes**.
5. Select it and give it a name, such as `StudentData.dbml`.
6. Click **Add**.

Visual Studio creates a LINQ to SQL designer file (`.dbml`) with associated generated code files.

## Step 2: Create a Database Connection

1. Open **Server Explorer** in Visual Studio.
2. Right-click **Data Connections**.
3. Select **Add Connection**.
4. Choose Microsoft SQL Server.
5. Enter your SQL Server name.
6. Select the authentication method and provide credentials if required.
7. Choose your database, such as `MVCDB`.
8. Click **Test Connection**, then click **OK**.

Your database connection will now appear under Data Connections.

## Step 3: Open the LINQ to SQL Designer

Open the `StudentData.dbml` file.

The LINQ to SQL designer provides two main areas:

- **Left side — Tables:** Database tables and views.
- **Right side — Stored Procedures:** Stored procedures and supported database functions.

These areas allow you to map database objects to your application.

## Step 4: Drag and Drop Tables and Stored Procedures

1. Expand your database connection in Server Explorer.
2. Expand the **Tables** section.
3. Drag the required table, such as `Student`, to the left side of the LINQ to SQL designer.
4. Expand the **Stored Procedures** section.
5. Drag the required stored procedures to the right side of the designer.
6. Save the `.dbml` file.

Visual Studio automatically generates the corresponding entity classes for the tables and methods for supported stored procedures.

**Example:**

- `Student` table → generates a `Student` entity class.
- `StudentDataDataContext` → represents the database context.
- Stored procedures → generate callable methods in the DataContext when mapped successfully.

The exact generated names depend on your `.dbml` filename and database objects.

## Step 5: Understand the Generated Files

After saving the `.dbml` file, Visual Studio typically generates:

- `StudentData.dbml` — Contains the database mapping and designer configuration.
- `StudentData.dbml.layout` — Stores the designer layout, when generated.
- `StudentData.designer.cs` — Contains generated entity classes, DataContext, and mapped methods.

You generally do not need to write these generated entity classes manually.

**Important:** Do not manually edit generated code because Visual Studio can overwrite it when the mapping is regenerated.

## Step 6: Use the DataContext in the Controller

Once the DataContext has been generated, use it in your MVC controller to perform database operations.

### Example: Display Students

```csharp
using System.Linq;
using System.Web.Mvc;
using YourProject.Models;

public class StudentController : Controller
{
    private StudentDataDataContext db =
        new StudentDataDataContext();

    public ActionResult DisplayStudents()
    {
        var students = db.Students.ToList();

        return View(students);
    }
}
```

Replace `StudentDataDataContext` and `db.Students` with the actual generated DataContext and table entity names in your project.

### Explanation

- `StudentDataDataContext`: Connects your application to the database using the generated mapping.
- `db.Students`: Accesses the mapped Student table.
- `ToList()`: Executes the query and loads the student records.
- `View(students)`: Sends the records to the Razor View.

## Step 7: Perform Database Operations

### 1. Select Records

```csharp
var students = db.Students.ToList();
```

### 2. Insert a Record

```csharp
Student student = new Student
{
    Name = "Rahul",
    Class = 10,
    Fees = 15000
};

db.Students.InsertOnSubmit(student);
db.SubmitChanges();
```

### 3. Update a Record

```csharp
var student = db.Students
    .FirstOrDefault(s => s.Sid == 1);

if (student != null)
{
    student.Name = "Amit";
    db.SubmitChanges();
}
```

### 4. Delete a Record

```csharp
var student = db.Students
    .FirstOrDefault(s => s.Sid == 1);

if (student != null)
{
    db.Students.DeleteOnSubmit(student);
    db.SubmitChanges();
}
```

These examples assume that your generated entity exposes the shown properties and that the table is mapped as `Students`. Adjust the names and property values to match your database schema.

## Step 8: Use Stored Procedures

If you dragged a stored procedure into the designer, LINQ to SQL can generate a corresponding method in the DataContext.

For example, if the generated method is named `Student_Select`, call it through your DataContext:

```csharp
var students = db.Student_Select().ToList();
```

The exact method signature depends on the stored procedure's parameters and its generated return type.

## Execution Flow

1. Create a LINQ to SQL class (`.dbml`).
2. Connect to SQL Server through Server Explorer.
3. Drag tables and stored procedures into the designer.
4. Save the designer to generate entity classes and DataContext code.
5. Create or use the generated DataContext in your controller.
6. Execute LINQ queries or mapped stored procedures.
7. Pass the resulting data to the Razor View.

## Advantages of LINQ to SQL

- Automatically generates entity classes.
- Reduces manual ADO.NET connection and mapping code.
- Supports LINQ queries for retrieving and filtering records.
- Supports insert, update, and delete operations.
- Allows stored procedures to be mapped to DataContext methods.

## Important Notes

- LINQ to SQL is primarily intended for classic .NET Framework applications; it is not the standard ORM for modern ASP.NET Core projects.
- Use the generated DataContext and entity names shown in your own project.
- Dispose of the DataContext appropriately, especially in larger applications.
- Keep database credentials out of public repositories.
- For modern ASP.NET Core applications, consider Entity Framework Core.

## Learning Outcomes

After completing this guide, you will understand how to:

- Create a LINQ to SQL mapping using Visual Studio.
- Connect SQL Server to an ASP.NET MVC project.
- Generate models and a DataContext automatically.
- Use the DataContext in a controller.
- Perform CRUD operations using LINQ to SQL.
- Map and execute stored procedures.
