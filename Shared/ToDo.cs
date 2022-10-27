using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared;

public sealed class ToDo
{
    public int Id { get; set; }

    [Required]
    [MaxLength(70)]
    public string Content { get; set; } = string.Empty;

    public bool IsDone { get; set; }
}
