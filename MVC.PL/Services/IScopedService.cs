﻿using System;

namespace MVC.PL.Services
{
    public interface IScopedService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}