﻿using MysteryLocation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MysteryLocation.ViewModel
{
    class CategoryViewModel
    {
        public IList<Category> CatList { get; set; }

        public CategoryViewModel()
        {
            try
            {
                CatList = new ObservableCollection<Category>();
                CatList.Add(new Category { CategoryId = 1, CategoryName = "Historisk" });
                CatList.Add(new Category { CategoryId = 2, CategoryName = "Natur" });
                CatList.Add(new Category { CategoryId = 3, CategoryName = "Arkitektur" });
                CatList.Add(new Category { CategoryId = 4, CategoryName = "Modern arkitektur" });
                CatList.Add(new Category { CategoryId = 5, CategoryName = "Historisk arkitektur" });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
