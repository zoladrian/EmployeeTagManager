﻿using System;
namespace EmployeeTagManagerApp.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
