using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Books
    {
        /// <summary>
        /// 書籍編號
        /// </summary>
        [DisplayName("書籍編號")]
        public int BookId { get; set; }

        /// <summary>
        /// 書名
        /// </summary>
        [DisplayName("書名")]
        [StringLength(400)]
        [Required(ErrorMessage = "請填入書名")]
        public string BookName { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者")]
        [StringLength(60)]
        [Required(ErrorMessage = "請填入作者")]
        public string BookAuthor { get; set; }

        /// <summary>
        /// 內容簡介
        /// </summary>
        [DisplayName("內容簡介")]
        [StringLength(2400)]
        [Required(ErrorMessage = "請填入內容")]
        public string BookNote { get; set; }

        /// <summary>
        /// 出版商
        /// </summary>
        [DisplayName("出版商")]
        [StringLength(40)]
        [Required(ErrorMessage = "請填入出版商")]
        public string BookPublisher { get; set; }

        /// <summary>
        /// 購書日期
        /// </summary>
        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public string BookBougthDate { get; set; }

        /// <summary>
        /// 圖書類別
        /// </summary>
        [DisplayName("圖書類別")]
        public string BookClass { get; set; }

        /// <summary>
        /// 圖書類別編號
        /// </summary>
        [DisplayName("圖書類別編號")]
        [Required(ErrorMessage = "請選擇類別")]
        public string BookClassId { get; set; }

        /// <summary>
        /// 借閱狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        public string BookStatus { get; set; }

        /// <summary>
        /// 借閱狀態代號
        /// </summary>
        [DisplayName("借閱狀態代號")]
        public string BookStatusCode { get; set; }

        /// <summary>
        /// 借閱者編號
        /// </summary>
        [DisplayName("借閱者編號")]
        public string KeeperId { get; set; }

        /// <summary>
        /// 借閱人中文名
        /// </summary>
        [DisplayName("借閱人")]
        public string KeeperCName { get; set; }

        /// <summary>
        /// 借閱人英文名
        /// </summary>
        [DisplayName("借閱人")]
        public string KeeperEName { get; set; }

        /// <summary>
        /// 借閱者
        /// </summary>
        [DisplayName("借閱人")]
        public string Keeper
        {
            get
            {
                return KeeperId.Trim() == "" ?
                    KeeperId + "-" + KeeperEName + "(" + KeeperCName + ")" : "";
            }
        }
    }

}