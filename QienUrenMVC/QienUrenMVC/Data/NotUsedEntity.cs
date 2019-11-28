using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QienUrenMVC.Data
{
    public class NotUsedEntity
    {   
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
