namespace VisualNovel {
    public class Character {
        public string Name = "";
        public string Picture = "";
        public int Side; // 0 = Left; 1 = Center; 2 = Right
        public bool Selected;

        public Character(string name, string picture, int side, bool selected = false) {
            Name = name;
            Picture = picture;
            Side = side;
            Selected = selected;
        }

        public bool HasPicture() {
            return Picture.Length > 0;
        }
    }
}