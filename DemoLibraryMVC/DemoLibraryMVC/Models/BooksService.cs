using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Library.Models
{
    public class BooksService
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
        public List<Models.Books> GetBooks (BooksSearchArg arg)
        {
            DataTable booksTable = new DataTable();
            string cmdText = 
                @"SELECT b.BOOK_ID,  b.BOOK_NAME,  b.BOOK_CLASS_ID,  
                                  bClass.BOOK_CLASS_NAME, b.BOOK_AUTHOR, 
                                  CONVERT( varchar(12), b.BOOK_BOUGHT_DATE, 23) BOOK_BOUGHT_DATE,
                                  b.BOOK_PUBLISHER, b.BOOK_NOTE,  b.BOOK_STATUS,
                                  bCode.CODE_NAME, b.BOOK_KEEPER,
                                  m.USER_CNAME,
                                  m.USER_ENAME
                    FROM BOOK_DATA b
                    LEFT JOIN BOOK_CLASS bClass
	                        ON b.BOOK_CLASS_ID=bClass.BOOK_CLASS_ID
                    LEFT JOIN BOOK_CODE bCode
                            ON b.BOOK_STATUS=bCode.CODE_ID
	                        AND bCode.CODE_TYPE='BOOK_STATUS'
                    LEFT JOIN MEMBER_M m
	                        ON b.BOOK_KEEPER=m.[USER_ID]
                   WHERE (b.BOOK_ID=@BOOK_ID or @BOOK_ID=0) AND 
                             (UPPER(b.BOOK_NAME) LIKE '%'+UPPER(@BOOK_NAME)+'%' or @BOOK_NAME='') AND
                             (b.BOOK_CLASS_ID=@BOOK_CLASS_ID or @BOOK_CLASS_ID='') AND
                             (b.BOOK_KEEPER=@BOOK_KEEPER or @BOOK_KEEPER='') AND
                             (b.BOOK_STATUS=@BOOK_STATUS or @BOOK_STATUS='') 
                   Order By b.BOOK_BOUGHT_DATE DESC,    BOOK_ID";
            
            using(SqlConnection conn=new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@BOOK_ID", arg.BookId);
                cmd.Parameters.AddWithValue("@BOOK_NAME", arg.BookName ?? string.Empty );
                cmd.Parameters.AddWithValue("@BOOK_CLASS_ID", arg.BookClassId ?? string.Empty);
                cmd.Parameters.AddWithValue("@BOOK_KEEPER", arg.KeeperId ?? string.Empty);
                cmd.Parameters.AddWithValue("@BOOK_STATUS", arg.BookStatusCode ?? string.Empty);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(booksTable);
                conn.Close();
            }

            return MapBooksDataToList(booksTable);
        }

        public List<Models.Books> MapBooksDataToList(DataTable booksData)
        {
            List<Models.Books> result = new List<Books>();
            foreach(DataRow row in booksData.Rows)
            {
                result.Add(new Books()
                {
                    BookId = (int)row["BOOK_ID"],
                    BookName = row["BOOK_NAME"].ToString(),
                    BookClass = row["BOOK_CLASS_NAME"].ToString(),
                    BookClassId = row["BOOK_CLASS_ID"].ToString(),
                    BookAuthor = row["BOOK_AUTHOR"].ToString(),
                    BookBougthDate =row["BOOK_BOUGHT_DATE"].ToString(),
                    BookNote = row["BOOK_NOTE"].ToString(),
                    BookStatusCode = row["BOOK_STATUS"].ToString(),
                    BookStatus = row["CODE_NAME"].ToString(),
                    BookPublisher = row["BOOK_PUBLISHER"].ToString(),
                    KeeperId = row["BOOK_KEEPER"].ToString(),
                    KeeperCName = row["USER_CNAME"].ToString(),
                    KeeperEName = row["USER_ENAME"].ToString()
                });
                
            }
            return result;
        }

        /// <summary>
        /// 取得圖書類別
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClass()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            DataTable classTable = new DataTable();
            string cmdText = @"SELECT BOOK_CLASS_ID, BOOK_CLASS_NAME
                                                FROM BOOK_CLASS";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(classTable);
                conn.Close();
            }
            foreach(DataRow row in classTable.Rows)
            {
                result.Add(new SelectListItem
                {
                    Value = row["BOOK_CLASS_ID"].ToString(),
                    Text= row["BOOK_CLASS_NAME"].ToString()
                });
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
                    Text = row["USER_CNAME"].ToString()+"-"+ row["USER_ENAME"].ToString()
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
        public int InsertBooks(Books book)
        {
            int insertResult = 0;
            //預設借閱狀態可借閱、借閱人為空、建立日期及修改日期為目前日期
            string cmdText =
                @"INSERT INTO BOOK_DATA
                    (
	                    BOOK_NAME,  BOOK_AUTHOR,  BOOK_PUBLISHER,
	                    BOOK_BOUGHT_DATE,   BOOK_NOTE,  BOOK_CLASS_ID,
	                    BOOK_STATUS,  BOOK_KEEPER,  CREATE_DATE,  MODIFY_DATE
                    )
                    VALUES 
                    (
	                    @Name,  @Author,  @Publisher,
	                    @BoughtDate,  @Note,  @ClassId,
	                    'A',    '', GETDATE(),  GETDATE()
                    )
                    SELECT SCOPE_IDENTITY()" ;
            using (SqlConnection conn= new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@Name", book.BookName);
                cmd.Parameters.AddWithValue("@Author", book.BookAuthor);
                cmd.Parameters.AddWithValue("@Publisher", book.BookPublisher);
                cmd.Parameters.AddWithValue("@BoughtDate", book.BookBougthDate);
                cmd.Parameters.AddWithValue("@Note", book.BookNote);
                cmd.Parameters.AddWithValue("@ClassId", book.BookClassId);
                insertResult = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();
            }
                return insertResult;
        }

        /// <summary>
        /// 修改書籍資訊
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int UpdateBooks(Books book)
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
                cmd.Parameters.AddWithValue("@KeeperId", book.KeeperId??string.Empty);
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
            bool checkResult=false;
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
                using(SqlConnection conn = new SqlConnection(GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cmdText, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
