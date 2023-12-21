namespace UserControls
{
    class PlayerOrClubItem
    {
        public string ID { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }

        public PlayerOrClubItem(string id, string imageUrl, string name)
        {
            ID = id;
            ImageUrl = imageUrl;
            Name = name;
        }
    }
}
