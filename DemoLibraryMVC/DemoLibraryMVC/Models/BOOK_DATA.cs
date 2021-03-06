//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoLibraryMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BOOK_DATA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BOOK_DATA()
        {
            this.BOOK_LEND_RECORD = new HashSet<BOOK_LEND_RECORD>();
        }
    
        public int BOOK_ID { get; set; }
        public string BOOK_NAME { get; set; }
        public string BOOK_CLASS_ID { get; set; }
        public string BOOK_AUTHOR { get; set; }
        public Nullable<System.DateTime> BOOK_BOUGHT_DATE { get; set; }
        public string BOOK_PUBLISHER { get; set; }
        public string BOOK_NOTE { get; set; }
        public string BOOK_STATUS { get; set; }
        public string BOOK_KEEPER { get; set; }
        public Nullable<int> BOOK_AMOUNT { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_USER { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string MODIFY_USER { get; set; }
    
        public virtual BOOK_CLASS BOOK_CLASS { get; set; }
        public virtual BOOK_CODE BOOK_CODE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BOOK_LEND_RECORD> BOOK_LEND_RECORD { get; set; }
        public virtual MEMBER_M MEMBER_M { get; set; }
    }
}
