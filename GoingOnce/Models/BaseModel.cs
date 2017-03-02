using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GoingOnce.Models
{
    public interface IDataModel
    {
        Guid Id { get; }
    }

    public abstract class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id
        {
            get; set;
        }

        public DateTime? DateCreatedUtc { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public string UserModified { get; set; }
    }

    public abstract class BaseModel<T> : BaseModel
        where T : BaseModel<T>
    {
    }

}