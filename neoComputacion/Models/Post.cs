﻿using System;
using System.Collections.Generic;

namespace neoComputacion.Models;

public partial class Post
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
