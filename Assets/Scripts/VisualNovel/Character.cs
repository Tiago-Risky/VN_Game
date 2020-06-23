using System.Xml.Linq;

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

        // Defaults: Name = "" ; Picture = "" ; Side = 0 (Left); Selected = true; Hidden = false
        public Character(XElement xElement) {
            Name = (xElement.Attribute("Name") != null) ? xElement.Attribute("Name").Value : "";
            Picture = (xElement.Attribute("Picture") != null) ? xElement.Attribute("Picture").Value : "";
            Side = (xElement.Attribute("Side") != null) ? int.Parse(xElement.Attribute("Side").Value) : 0;
            Selected = (xElement.Attribute("Selected") != null) ? bool.Parse(xElement.Attribute("Selected").Value) : true;
            Hidden = (xElement.Attribute("Hidden") != null) ? bool.Parse(xElement.Attribute("Hidden").Value) : false;
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

        // For the dialogue editor
        public XElement ExportXML() {
            return new XElement("Character",
                            new XAttribute("Name", Name),
                            new XAttribute("Picture", Picture),
                            new XAttribute("Side", Side),
                            new XAttribute("Selected", Selected),
                            new XAttribute("Hidden", Hidden)
                            );
        }
    }
}