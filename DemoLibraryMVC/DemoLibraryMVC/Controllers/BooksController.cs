using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        readonly Models.BooksService booksService = new Models.BooksService();
        // GET: Books
        public ActionResult Index()
        {
            SetDropDownListItmes();
            return View();
        }

        /// <summary>
        /// 書籍查詢
        /// </summary>
        [HttpPost()]
        [ValidateInput(false)]
        public ActionResult Index(Models.BooksSearchArg arg)
        {
            //HtmlEncode
            ModelHtmlEncode(arg);

            //BookData
            List<Models.Books>searchResult= this.booksService.GetBooks(arg);           
            foreach(Models.Books book in searchResult)
            {
                ModelHtmlDecode(book);
            }
            ViewBag.SearchResult = searchResult;

            SetDropDownListItmes();
            return View("Index");
        }

        /// <summary>
        /// 書籍明細查詢畫面
        /// </summary>
        [HttpGet]
        public ActionResult BooksDetail(int id)
        {
            //HtmlEncode
            //id = Server.HtmlEncode(id);

            SetDropDownListItmes();
            return View("BooksDetail",GetBookDetail(id));
        }

        /// <summary>
        /// 書籍內容修改畫面
        /// </summary>
        [HttpGet]
        public ActionResult BooksUpdate(int id)
        {
            //HtmlEncode
            //id = Server.HtmlEncode(id);

            SetDropDownListItmes();
            return View("BooksUpdate", GetBookDetail(id));
        }

        /// <summary>
        /// 書籍修改
        /// </summary>
        [HttpPost]
        public ActionResult BooksUpdate(Models.Books book,int id)
        {
            ////HtmlEncode
            //id = Server.HtmlEncode(id);
            ModelHtmlEncode(book);

            book.BookId =id;
            //驗證以借閱時借閱者不得為空
            if(book.BookStatusCode!="A"&& book.BookStatusCode !="U"&& book.KeeperId == null)
            {
                ViewBag.UpdateMessage = "已借閱 不得無借閱者";
                ViewBag.keeperAlarm = "";
                SetDropDownListItmes();
                return View(book);
            }

            if (ModelState.IsValid)
            {
                int UpdateResult = booksService.UpdateBooks(book);
                if (UpdateResult != 0)
                {
                    ViewBag.UpdateMessage = "修改完成";
                }
            }
            SetDropDownListItmes();
            return View(book);
        }

        /// <summary>
        /// 書籍新增畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BooksInsert()
        {
            //SelectListItem BookClass
            ViewBag.BookClass = this.booksService.GetBookClass();
            return View("BooksInsert");
        }

        /// <summary>
        /// 書籍新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BooksInsert(Models.Books book)
        {
            //HtmlEncode
            ModelHtmlEncode(book);

            ViewBag.BookClass = this.booksService.GetBookClass();
            if (ModelState.IsValid)
            {
                ViewBag.InsertMessage = "新增失敗";
                ViewBag.AlertClass = "alert alert-danger";
                int InsertResult = this.booksService.InsertBooks(book);
                if (InsertResult != 0)
                {
                    ViewBag.InsertMessage = "新增完成";
                    ViewBag.AlertClass = "alert alert-success";
                }
            }        
            return View();
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        [HttpPost()]
        public JsonResult BooksDelete(string bookId)
        {
            //HtmlEncode
            bookId = Server.HtmlEncode(bookId);

            try
            {
                JsonResult json = new JsonResult();
                if (booksService.BookIsNotLend(bookId))
                {                    
                    booksService.DeleteBook(bookId);
                    json.Data = new { message = "刪除完成",isDelete=true };
                }
                else
                {
                    json.Data = new { message = "請確認此書已歸還再刪除", isDelete = false };
                } 
                return json;
            }
            catch (Exception ex)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 以BookId搜尋此書
        /// </summary>
        public Models.Books GetBookDetail(int id)
        {
            //HtmlEncode
            //id = Server.HtmlEncode(id);

            //Models.BooksSearchArg arg = new Models.BooksSearchArg { BookId = Convert.ToInt32(id) };
            Models.BooksSearchArg arg = new Models.BooksSearchArg { BookId = id };
            Models.Books books = this.booksService.GetBooks(arg).FirstOrDefault();


            ModelHtmlDecode(books);
            return books;
        }

        /// <summary>
        /// 類別,借閱者,借閱狀態選單填入
        /// </summary>
        public void SetDropDownListItmes()
        {
            //SelectListItem BookClass
            ViewBag.BookClass = this.booksService.GetBookClass();
            //SelectListItem BookKeeper
            ViewBag.Member = this.booksService.GetMember();
            //SelectListItem BookStatus
            ViewBag.BookStatus = this.booksService.GetBookStatus();
        }

        /// <summary>
        /// HtmlDecode輸出網頁用
        /// </summary>
        public void ModelHtmlDecode(Models.Books book)
        {
            book.BookAuthor = Server.HtmlDecode(book.BookAuthor);
            book.BookBougthDate = Server.HtmlDecode(book.BookBougthDate);
            book.BookClassId = Server.HtmlDecode(book.BookClassId);
            book.BookName = Server.HtmlDecode(book.BookName);
            book.BookNote = Server.HtmlDecode(book.BookNote);
            book.BookPublisher = Server.HtmlDecode(book.BookPublisher);
            book.BookStatusCode = Server.HtmlDecode(book.BookStatusCode);
            book.KeeperId = Server.HtmlDecode(book.KeeperId);
        }

        /// <summary>
        /// HtmlEncode防止XSS
        /// </summary>
        public void ModelHtmlEncode(Models.Books book)
        {
            book.BookAuthor = Server.HtmlEncode(book.BookAuthor);
            book.BookBougthDate = Server.HtmlEncode(book.BookBougthDate);
            book.BookClassId = Server.HtmlEncode(book.BookClassId);
            book.BookName = Server.HtmlEncode(book.BookName);
            book.BookNote = Server.HtmlEncode(book.BookNote);
            book.BookPublisher = Server.HtmlEncode(book.BookPublisher);
            book.BookStatusCode = Server.HtmlEncode(book.BookStatusCode);
            book.KeeperId = Server.HtmlEncode(book.KeeperId);
        }

        /// <summary>
        /// HtmlEncode防止XSS
        /// </summary>
        public void ModelHtmlEncode(Models.BooksSearchArg arg)
        {
            
            arg.BookClassId = Server.HtmlEncode(arg.BookClassId);
            arg.BookName = Server.HtmlEncode(arg.BookName);
            arg.BookStatusCode = Server.HtmlEncode(arg.BookStatusCode);
            arg.KeeperId = Server.HtmlEncode(arg.KeeperId);
            
        }
    }
}