using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using DemoLibraryMVC.Models;
using System.Data.Entity;

namespace Library.Models
{
    public class BooksServiceEF
    {
        public readonly GSSWEBEntities db = new GSSWEBEntities();

        /// <summary>
        /// 取得資料庫連結
        /// </summary>
        /// <returns></returns>
        public string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 取得書籍資訊
        /// </summary>
        /// <returns></returns>
        public List<BOOK_DATA> GetBooks(BooksSearchArg arg)
        {
            try
            {
                return db.BOOK_DATA
                                    .Include(b => b.MEMBER_M)
                                    .Include(b => b.BOOK_CLASS)
                                    .Include(b => b.BOOK_CODE)
                                    .Where(b =>
                                        (b.BOOK_ID == arg.BookId || arg.BookId == 0) &&
                                        (b.BOOK_NAME.ToUpper().Contains(arg.BookName.ToUpper() ?? string.Empty) || (arg.BookName ?? string.Empty) == "") &&
                                        (b.BOOK_CLASS_ID == (arg.BookClassId ?? string.Empty) || (arg.BookClassId ?? string.Empty) == "") &&
                                        (b.BOOK_KEEPER == (arg.KeeperId ?? string.Empty) || (arg.KeeperId ?? string.Empty) == "") &&
                                        (b.BOOK_STATUS == (arg.BookStatusCode ?? string.Empty) || (arg.BookStatusCode ?? string.Empty) == "")
                                    )
                                    .OrderByDescending(b => b.BOOK_BOUGHT_DATE)
                                    .ThenBy(b => b.BOOK_ID)
                                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<BOOK_DATA>();
            }
        }

        /// <summary>
        /// 取得圖書類別
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClass()
        {
            List<SelectListItem> result = new List<SelectListItem>();

            try
            {
                var data = db.BOOK_CLASS;
                foreach (BOOK_CLASS item in data)
                {
                    result.Add(new SelectListItem
                    {
                        Value = item.BOOK_CLASS_ID,
                        Text = item.BOOK_CLASS_NAME
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 取得借閱人
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetMember()
        {
            List<SelectListItem> result = new List<SelectListItem>();

            try
            {
                var users = db.MEMBER_M;
                foreach (MEMBER_M person in users)
                {
                    result.Add(new SelectListItem
                    {
                        Value = person.USER_ID,
                        Text = person.USER_CNAME + "-" + person.USER_ENAME
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 取得借閱狀態
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookStatus()
        {
            List<SelectListItem> result = new List<SelectListItem>();

            try
            {
                var bookStatus = db.BOOK_CODE.Where(b => b.CODE_TYPE == "BOOK_STATUS");

                foreach (BOOK_CODE code in bookStatus)
                {
                    result.Add(new SelectListItem
                    {
                        Value = code.CODE_ID,
                        Text = code.CODE_NAME
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 新增書籍資訊
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int InsertBooks(BOOK_DATA book)
        {
            try
            {
                //預設借閱狀態可借閱、借閱人為空、建立日期及修改日期為目前日期
                book.BOOK_STATUS = "A";
                book.BOOK_KEEPER = "";
                book.CREATE_DATE = DateTime.Now;
                book.MODIFY_DATE = DateTime.Now;
                db.BOOK_DATA.Add(book);
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改書籍資訊
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int UpdateBooks(BOOK_DATA book)
        {
            try
            {
                //預設借閱狀態可借閱、借閱人為空、建立日期及修改日期為目前日期
                book.MODIFY_DATE = DateTime.Now;
                book.BOOK_KEEPER = book.BOOK_KEEPER ?? string.Empty;
                db.Entry(book).State = EntityState.Modified;
                return db.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 檢查書籍未被借閱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool BookIsNotLend(string id)
        {
            try
            {
                var bookStatus = db.BOOK_DATA.FirstOrDefault(b => b.BOOK_ID.ToString() == id);
                string status = bookStatus.BOOK_STATUS;
                return (status == "A" || status == "U");
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBook(string id)
        {
            try
            {
                var deleteItem = db.BOOK_DATA.FirstOrDefault(m => m.BOOK_ID.ToString() == id);
                db.BOOK_DATA.Remove(deleteItem);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}