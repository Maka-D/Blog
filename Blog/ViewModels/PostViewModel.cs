﻿namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string Author { get; set; } = "";
        public string UserId { get; set; }
    }
}