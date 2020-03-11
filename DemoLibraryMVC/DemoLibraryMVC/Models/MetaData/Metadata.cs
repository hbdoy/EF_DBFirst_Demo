using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoLibraryMVC.Models
{
    public class BOOK_DATAMetadata
    {
        /// <summary>
        /// 書籍編號
        /// </summary>
        [DisplayName("書籍編號")]
        public int BOOK_ID { get; set; }

        /// <summary>
        /// 書名
        /// </summary>
        [DisplayName("書名")]
        [StringLength(400)]
        [Required(ErrorMessage = "請填入書名")]
        public string BOOK_NAME { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者")]
        [StringLength(60)]
        [Required(ErrorMessage = "請填入作者")]
        public string BOOK_AUTHOR { get; set; }

        /// <summary>
        /// 內容簡介
        /// </summary>
        [DisplayName("內容簡介")]
        [StringLength(2400)]
        [Required(ErrorMessage = "請填入內容")]
        public string BOOK_NOTE { get; set; }

        /// <summary>
        /// 出版商
        /// </summary>
        [DisplayName("出版商")]
        [StringLength(40)]
        [Required(ErrorMessage = "請填入出版商")]
        public string BOOK_PUBLISHER { get; set; }

        /// <summary>
        /// 購書日期
        /// </summary>
        [DisplayName("購書日期")]
        [Required(ErrorMessage = "此欄位必填")]
        public Nullable<System.DateTime> BOOK_BOUGHT_DATE { get; set; }
        
        /// <summary>
        /// 圖書類別編號
        /// </summary>
        [DisplayName("圖書類別")]
        [Required(ErrorMessage = "請選擇類別")]
        public string BOOK_CLASS_ID { get; set; }
    
        /// <summary>
        /// 借閱狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        public string BOOK_STATUS { get; set; }

        /// <summary>
        /// 借閱者
        /// </summary>
        [DisplayName("借閱者")]
        public string BOOK_KEEPER { get; set; }
    }
}