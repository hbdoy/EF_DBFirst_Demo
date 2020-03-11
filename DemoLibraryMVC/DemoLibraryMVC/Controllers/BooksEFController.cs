using DemoLibraryMVC.Models;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BooksEFController : Controller
    {
        readonly BooksServiceEF booksService = new BooksServiceEF();

        // GET: BooksEF
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
        public ActionResult Index(BooksSearchArg arg)
        {
            //HtmlEncode
            ModelHtmlEncode(arg);

            //BookData
            List<BOOK_DATA> searchResult = this.booksService.GetBooks(arg);
            foreach (var book in searchResult)
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
            SetDropDownListItmes();
            return View("BooksDetail", GetBookDetail(id));
        }

        /// <summary>
        /// 書籍內容修改畫面
        /// </summary>
        [HttpGet]
        public ActionResult BooksUpdate(int id)
        {
            SetDropDownListItmes();
            return View("BooksUpdate", GetBookDetail(id));
        }

        /// <summary>
        /// 書籍修改
        /// </summary>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BooksUpdate(BOOK_DATA book, int id)
        {
            ModelHtmlEncode(book);
            book.BOOK_ID = id;
            //驗證以借閱時借閱者不得為空
            if (book.BOOK_STATUS != "A" && book.BOOK_STATUS != "U" && book.BOOK_KEEPER == null)
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
        [ValidateInput(false)]
        public ActionResult BooksInsert(BOOK_DATA book)
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
        [ValidateInput(false)]
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
                    json.Data = new { message = "刪除完成", isDelete = true };
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
        public BOOK_DATA GetBookDetail(int id)
        {
            BooksSearchArg arg = new BooksSearchArg { BookId = id };
            BOOK_DATA book = this.booksService.GetBooks(arg).FirstOrDefault();
            
            ModelHtmlDecode(book);
            return book;
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
        public void ModelHtmlDecode(BOOK_DATA book)
        {
            book.BOOK_AUTHOR = Server.HtmlDecode(book.BOOK_AUTHOR);
            book.BOOK_CLASS_ID = Server.HtmlDecode(book.BOOK_CLASS_ID);
            book.BOOK_NAME = Server.HtmlDecode(book.BOOK_NAME);
            book.BOOK_NOTE = Server.HtmlDecode(book.BOOK_NOTE);
            book.BOOK_PUBLISHER = Server.HtmlDecode(book.BOOK_PUBLISHER);
            book.BOOK_STATUS = Server.HtmlDecode(book.BOOK_STATUS);
            book.BOOK_KEEPER = Server.HtmlDecode(book.BOOK_KEEPER);
        }

        /// <summary>
        /// HtmlEncode防止XSS
        /// </summary>
        public void ModelHtmlEncode(BOOK_DATA book)
        {
            book.BOOK_AUTHOR = Server.HtmlEncode(book.BOOK_AUTHOR);
            book.BOOK_CLASS_ID = Server.HtmlEncode(book.BOOK_CLASS_ID);
            book.BOOK_NAME = Server.HtmlEncode(book.BOOK_NAME);
            book.BOOK_NOTE = Server.HtmlEncode(book.BOOK_NOTE);
            book.BOOK_PUBLISHER = Server.HtmlEncode(book.BOOK_PUBLISHER);
            book.BOOK_STATUS = Server.HtmlEncode(book.BOOK_STATUS);
            book.BOOK_KEEPER = Server.HtmlEncode(book.BOOK_KEEPER);
        }

        /// <summary>
        /// HtmlEncode防止XSS
        /// </summary>
        public void ModelHtmlEncode(BooksSearchArg arg)
        {
            arg.BookClassId = Server.HtmlEncode(arg.BookClassId);
            arg.BookName = Server.HtmlEncode(arg.BookName);
            arg.BookStatusCode = Server.HtmlEncode(arg.BookStatusCode);
            arg.KeeperId = Server.HtmlEncode(arg.KeeperId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                booksService.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}