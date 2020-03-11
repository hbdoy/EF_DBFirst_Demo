using DemoLibraryMVC.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            GSSWEBEntities db = new GSSWEBEntities();

            db.Database.Log = (msg) => Console.WriteLine(msg);

            // Insert Sample
            //BOOK_CLASS bc = new BOOK_CLASS();
            //bc.BOOK_CLASS_ID = "Test";
            //bc.BOOK_CLASS_NAME = "測試文字";
            //db.BOOK_CLASS.Add(bc);
            //db.SaveChanges();

            // -------------------------------------------

            // Delete Sample
            //var delItem = db.BOOK_CLASS.FirstOrDefault(m => m.BOOK_CLASS_ID == "Test");
            //if (delItem != null)
            //{
            //    db.BOOK_CLASS.Remove(delItem);
            //}
            //db.SaveChanges();

            // -------------------------------------------

            // Update Sample
            //var updateItem = db.BOOK_DATA.FirstOrDefault(m => m.BOOK_ID == 4);
            //var updateItem = db.BOOK_DATA.Find(4);
            //updateItem.BOOK_NAME = "新書名";
            //db.SaveChanges();

            // Update Sample2
            //var updateItem2 = db.BOOK_DATA.FirstOrDefault(m => m.BOOK_ID == 4);
            //db.Entry(updateItem2).State = EntityState.Modified;
            //db.SaveChanges();

            // -------------------------------------------

            // Search Sample(Lambda)
            //var allBookData = db.BOOK_DATA.Where(e => e.BOOK_CLASS_ID == "DB");
            //foreach (var item in allBookData)
            //{
            //    Console.WriteLine(item.BOOK_NAME);
            //}

            // Search Sample(Lambda) 關聯屬性
            //var allBookDataClassName = db.BOOK_DATA.Where(e => e.BOOK_CLASS_ID == "DB");
            //foreach (var item in allBookDataClassName)
            //{
            //    Console.WriteLine(item.BOOK_CLASS.BOOK_CLASS_NAME);
            //}

            // Search Sample(LINQ)
            //var allbookdata = from a in db.BOOK_DATA
            //                  where a.BOOK_CLASS_ID == "db"
            //                  select a;
            //foreach (var item in allbookdata)
            //{
            //    Console.WriteLine(item.BOOK_NAME);
            //}

            // Search Sample2(LINQ)
            //var allBookName = (from a in db.BOOK_DATA
            //                   where a.BOOK_CLASS_ID == "DB"
            //                   orderby a.CREATE_DATE
            //                   select a.BOOK_NAME).ToList();
            //foreach (var item in allBookName)
            //{
            //    Console.WriteLine(item);
            //}

            // -------------------------------------------

            // Search Sample(SQL)
            //var allDBTypeBook = db.BOOK_DATA.SqlQuery("SELECT * FROM BOOK_DATA WHERE BOOK_CLASS_ID = 'DB'").ToList();
            //foreach (var item in allDBTypeBook)
            //{
            //    Console.WriteLine(item.BOOK_NAME);
            //}

            // Search Sample3(SQL)
            //var allDBTypeBook2 = db.Database.SqlQuery<BOOK_DATA>("SELECT * FROM BOOK_DATA WHERE BOOK_CLASS_ID = 'DB'").ToList();
            //foreach (var item in allDBTypeBook2)
            //{
            //    Console.WriteLine(item.BOOK_NAME);
            //}

            // Update Sample2(SQL)
            //var updateItem3 = db.Database.ExecuteSqlCommand("UPDATE BOOK_DATA SET BOOK_NAME = '新書名2' WHERE BOOK_ID = 4");

            // -------------------------------------------
            

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
