namespace VisualNovel {
    public class Character {
        public string Name = "";
        public string Picture = "";
        public int Side; // 0 = Left; 1 = Center; 2 = Right
        public bool Selected;
        public bool Hidden;

        public Character(string name, string picture, int side, bool selected, bool hidden) {
            Name = name;
            Picture = picture;
            Side = side;
            Selected = selected;
            Hidden = hidden;
        }

        public bool HasPicture() {
            return Picture.Length > 0;
        }

        public int GetVisibility() {
            int Visibility = 0;

            /* Not Selected, Not Hidden: 0 
             * Selected, Not Hidden: 1
             * Not Selected, Hidden: 10
             * Selected, Hidden: 11             
             */
            Visibility += Selected ? 1 : 0;
            Visibility += Hidden ? 10 : 0;

            return Visibility;
        }
    }
}