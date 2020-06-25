using System.Collections.Generic;
using System.Xml.Linq;

namespace VisualNovel {
    public class PointOperations {
        public List<PointOperation> OperationList;

        public PointOperations(List<PointOperation> operationList) {
            OperationList = operationList;
        }

        public PointOperations(XElement xElement) {
            OperationList = new List<PointOperation>();
            foreach (XElement pointOperation in xElement.Elements()) {
                OperationList.Add(new PointOperation(pointOperation));
            }
        }

        public void RunAll(ref Dictionary<string, Point> PointsList) {
            foreach (PointOperation pointOperation in OperationList) {
                pointOperation.Run(ref PointsList);
            }
        }

        public XElement ExportXML() {
            XElement xElement = new XElement("PointOperations");
            foreach (PointOperation pointOperation in OperationList) {
                xElement.Add(pointOperation.ExportXML());
            }

            return xElement;
        }
    }

    public class PointOperation {
        public string PointName;
        public string Operation;
        public int Value;

        public PointOperation(string pointName, string operation, int value) {
            PointName = pointName;
            Operation = operation;
            Value = value;
        }

        public PointOperation(XElement xElement) {
            PointName = xElement.Attribute("Point").Value;
            Operation = xElement.Name.ToString();
            Value = int.Parse(xElement.Attribute("Value").Value);
        }

        public void Run(ref Dictionary<string, Point> PointsList) {
            switch (Operation) {
                case "Add":
                    PointsList[PointName].AddPoints(Value);
                    break;
                case "Remove":
                    PointsList[PointName].RemovePoints(Value);
                    break;
                case "Set":
                    PointsList[PointName].Value = Value;
                    break;
            }
        }

        public XElement ExportXML() {
            return new XElement(Operation, new XAttribute("Point",PointName), new XAttribute("Value", Value));
        }
    }
}
