namespace VisualNovel {
    public class Points {
        public string Name;
        public int Value;
        public Points(string name, int value) {
            Name = name;
            Value = value;
        }

        public void AddPoints(int value) {
            Value += value;
        }

        public void RemovePoints(int value) {
            Value -= value;
        }
    }
}
