﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
	public class ManageContentViewModel
	{
		public List<ContentPage> Pages;

		public ManageContentViewModel()
		{
			using (DataContext context = new DataContext())
			{
				Pages = context.ContentPages.ToList();
			}
		}
	}
}