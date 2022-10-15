﻿using System;
using portfolio_backend.Data.Entities;
namespace portfolio_backend.Data.DTOs.Project
{
    public class ProjectUpdateDTO
    {
        public int ProjectId { get; set; }
        public string? Header { get; set; }
        public string? Message { get; set; }
        public string? Link { get; set; }
        public IEnumerable<Entities.Image>? Images { get; set; }
    }
}

