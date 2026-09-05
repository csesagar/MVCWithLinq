using MVCWithLinq.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace MVCWithLinq.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        MVC7DBDataContext dal = new MVC7DBDataContext(ConfigurationManager.ConnectionStrings["MVCDBConnectionString"].ConnectionString);
        public ViewResult DisplayStudents()
        { 
            List<Student> students = (from s in dal.Students where s.Status == true select s).ToList();

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
                if(!System.IO.Directory.Exists(physicalPath))
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