﻿using MVC.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace MVC.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Required")]

        public string Name { get; set; }

        [DisplayName(" Date Of Creation  ")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; }


    }
}
