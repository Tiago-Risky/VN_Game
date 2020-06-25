using System.Xml.Linq;

namespace VisualNovel {
    public class Point {
        public string Name;
        public int Value;

        public Point(string name, int value) {
            Name = name;
            Value = value;
        }

        public Point(XElement xElement) {
            Name = xElement.Attribute("Name").Value;
            Value = int.Parse(xElement.Attribute("Value").Value);
        }

        public void AddPoints(int value) {
            Value += value;
        }

        public void RemovePoints(int value) {
            Value -= value;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            return new XElement("Point", new XAttribute("Name", Name), new XAttribute("Value", Value));
        }
    }
}
