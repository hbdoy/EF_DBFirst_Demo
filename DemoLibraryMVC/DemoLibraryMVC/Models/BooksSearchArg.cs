using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class BooksSearchArg
    {
        [DisplayName("書籍編號")]
        public int BookId { get; set; }

        [DisplayName("書名")]
        public string BookName { get; set; }

        [DisplayName("圖書類別")]
        public string BookClassId { get; set; }

        [DisplayName("借閱人")]
        public string KeeperId { get; set; }

        [DisplayName("借閱狀態")]
        public string BookStatusCode { get; set; }
    }
}