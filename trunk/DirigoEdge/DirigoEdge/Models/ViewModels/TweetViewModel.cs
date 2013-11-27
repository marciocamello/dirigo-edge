using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Utils;

namespace DirigoEdge.Models.ViewModels
{
	public class TweetViewModel
	{
		public List<TwitterUtils.Tweet> Tweets;


		public TweetViewModel(int count)
		{
		    Tweets = TwitterUtils.GetTweets(count);
		}
	}
}