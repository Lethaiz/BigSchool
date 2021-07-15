namespace BigSchool.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            Attendance = new HashSet<Attendance>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string LeturerIdId { get; set; }

        [StringLength(225)]
        public string Place { get; set; }

        public DateTime? Datetime { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string LectureName;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendance { get; set; }

        public List<Category> ListCategory = new List<Category>();
        public virtual Category Category { get; set; }
      
            public bool isLogin = false;
            public bool isShowGoing = false;
            public bool isShowFollow = false;
        

    }
}
