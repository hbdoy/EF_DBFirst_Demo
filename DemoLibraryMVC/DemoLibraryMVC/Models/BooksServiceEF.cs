using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using DemoLibraryMVC.Models;
using System.Data.Entity;

namespace Library.Models
{
    public class BooksServiceEF
    {
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
            using (GSSWEBEntities db = new GSSWEBEntities())
            {
                try
                {
                    return db.BOOK_DATA.Where(b =>
                                        (b.BOOK_ID == arg.BookId || arg.BookId == 0) &&
                                        (b.BOOK_NAME.ToUpper().Contains(arg.BookName.ToUpper() ?? string.Empty) || (arg.BookName ?? string.Empty) == "") &&
                                        (b.BOOK_CLASS_ID == (arg.BookClassId ?? string.Empty) || (arg.BookClassId ?? string.Empty) == "") &&
                                        (b.BOOK_KEEPER == (arg.KeeperId ?? string.Empty) || (arg.KeeperId ?? string.Empty) == "") &&
                                        (b.BOOK_STATUS == (arg.BookStatusCode ?? string.Empty) || (arg.BookStatusCode ?? string.Empty) == "")
                                    )
                                    .Include(b => b.MEMBER_M)
                                    .Include(b => b.BOOK_CLASS)
                                    .Include(b => b.BOOK_CODE)
                                    .OrderByDescending(b => b.BOOK_BOUGHT_DATE)
                                    .ThenBy(b => b.BOOK_ID)
                                    .ToList();
                }
                catch (Exception)
                {
                    return new List<BOOK_DATA>();
                }
            }
        }

        /// <summary>
        /// 取得圖書類別
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClass()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            using (GSSWEBEntities db = new GSSWEBEntities())
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
            return result;
        }

        /// <summary>
        /// 取得借閱人
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetMember()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            DataTable memberTable = new DataTable();
            string cmdText = @"SELECT USER_ID, USER_CNAME,  USER_ENAME
                                                FROM MEMBER_M";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(memberTable);
                conn.Close();
            }
            foreach (DataRow row in memberTable.Rows)
            {
                result.Add(new SelectListItem
                {
                    Value = row["USER_ID"].ToString(),
                    Text = row["USER_CNAME"].ToString() + "-" + row["USER_ENAME"].ToString()
                });
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
            DataTable statusTable = new DataTable();
            string cmdText = @"SELECT CODE_ID, CODE_NAME
                                                FROM BOOK_CODE
                                               WHERE CODE_TYPE = 'BOOK_STATUS' ";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(statusTable);
                conn.Close();
            }
            foreach (DataRow row in statusTable.Rows)
            {
                result.Add(new SelectListItem
                {
                    Value = row["CODE_ID"].ToString(),
                    Text = row["CODE_NAME"].ToString()
                });
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
            int insertResult = 0;
            using (GSSWEBEntities db = new GSSWEBEntities())
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
                catch (Exception)
                {
                    return insertResult;
                }
            }
        }

        /// <summary>
        /// 修改書籍資訊
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int UpdateBooks(BOOK_DATA book)
        {
            int updateResult = 0;
            //預設借閱狀態可借閱、借閱人為空、建立日期及修改日期為目前日期
            string cmdText =
                @"UPDATE BOOK_DATA
                    SET BOOK_NAME=@Name,
                            BOOK_AUTHOR=@Author,
                            BOOK_PUBLISHER=@Publisher,
                            BOOK_BOUGHT_DATE=@BoughtDate,
                            BOOK_NOTE=@Note,  
                            BOOK_CLASS_ID=@ClassId,
                         BOOK_STATUS=@StatusId,  
                            BOOK_KEEPER=@KeeperId,  
                            MODIFY_DATE=GETDATE()
                    WHERE BOOK_ID=@BookId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Name", book.BookName);
                cmd.Parameters.AddWithValue("@Author", book.BookAuthor);
                cmd.Parameters.AddWithValue("@Publisher", book.BookPublisher);
                cmd.Parameters.AddWithValue("@BoughtDate", book.BookBougthDate);
                cmd.Parameters.AddWithValue("@Note", book.BookNote);
                cmd.Parameters.AddWithValue("@ClassId", book.BookClassId);
                cmd.Parameters.AddWithValue("@StatusId", book.BookStatusCode);
                cmd.Parameters.AddWithValue("@KeeperId", book.KeeperId ?? string.Empty);
                cmd.Parameters.AddWithValue("@BookId", book.BookId);
                updateResult = Convert.ToInt16(cmd.ExecuteNonQuery());
                conn.Close();
            }
            return updateResult;
        }

        /// <summary>
        /// 檢查書籍未被借閱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool BookIsNotLend(string id)
        {
            bool checkResult = false;
            string cmdText =
                @"SELECT BOOK_STATUS FROM BOOK_DATA
                     WHERE BOOK_ID=@id";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string status = dr[0].ToString();
                    if (status == "A" || status == "U")
                        checkResult = true;
                }
                conn.Close();
            }
            return checkResult;
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBook(string id)
        {
            string cmdText =
                @"DELETE FROM BOOK_DATA
                     WHERE BOOK_ID=@id";
            try
            {
                using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}