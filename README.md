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

## Step 6: Use the DataContext in the Controller

```csharp
namespace MVCWithLinq.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        MVC7DBDataContext dal = new MVC7DBDataContext(ConfigurationManager.ConnectionStrings["MVCDBConnectionString"].ConnectionString);

        public ViewResult DisplayStudents()
        {
            List<Student> students = (from s in dal.Students
                                      where s.Status == true
                                      select s).ToList();

            return View(students);
        }

        public ViewResult DisplayStudent(int sid)
        {
            Student student = dal.Students.FirstOrDefault(s => s.Sid == sid);
            return View(student);
        }

        public ViewResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddStudent(Student s, HttpPostedFileBase selectedFiles)
        {
            if (selectedFiles != null)
            {
                string physicalPath = Server.MapPath("~/Uploads/");

                if (!System.IO.Directory.Exists(physicalPath))
                {
                    System.IO.Directory.CreateDirectory(physicalPath);
                }

                selectedFiles.SaveAs(physicalPath + selectedFiles.FileName);
                s.Photo = selectedFiles.FileName;
            }

            dal.Students.InsertOnSubmit(s);   // this will add record to the local table in memory, but not to the database yet
            dal.SubmitChanges();             // this will save the changes to the database

            return RedirectToAction("DisplayStudents");
        }

        public ViewResult EditStudent(int Sid)
        {
            Student student = dal.Students.Single(s => s.Sid == Sid);
            TempData["Photo"] = student.Photo;

            return View(student);
        }

        public ActionResult UpdateStudent(Student newS, HttpPostedFileBase selectedFile)
        {
            if (selectedFile != null)
            {
                string PhysicalPath = Server.MapPath("~/Uploads/");

                if (!Directory.Exists(PhysicalPath))
                {
                    Directory.CreateDirectory(PhysicalPath);
                }

                selectedFile.SaveAs(PhysicalPath + selectedFile.FileName);
                newS.Photo = selectedFile.FileName;
            }
            else
            {
                newS.Photo = TempData["Photo"].ToString();
            }

            Student old = dal.Students.Single(s => s.Sid == newS.Sid);

            old.Name = newS.Name;
            old.Class = newS.Class;
            old.Fees = newS.Fees;
            old.Photo = newS.Photo;

            dal.SubmitChanges();

            return RedirectToAction("DisplayStudents");
        }

        public RedirectToRouteResult DeleteStudent(int Sid)
        {
            Student stud = dal.Students.Single(s => s.Sid == Sid);

            // dal.Students.DeleteOnSubmit(stud);  // permanently delete the record from the database

            stud.Status = false;
            dal.SubmitChanges();

            return RedirectToAction("DisplayStudents");
        }
    }
}
```
