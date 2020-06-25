using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;

namespace VisualNovel {
    public class PointGate {
        public Redirect DefaultRedirect;
        public List<Condition> Conditions;

        public PointGate(Redirect defaultRedirect, List<Condition> conditions) {
            DefaultRedirect = defaultRedirect;
            Conditions = conditions;
        }

        public PointGate(XElement xElement) {
            DefaultRedirect = (xElement.Element("Redirect") != null) ? new Redirect(xElement.Element("Redirect")) : null;
            Conditions = new List<Condition>();
            foreach (XElement condition in xElement.Elements("Condition").ToList()) {
                Conditions.Add(new Condition(condition));
            }
        }

        public Redirect SolveGate(ref Dictionary<string, Point> PointsList) {
            foreach (Condition condition in Conditions) {
                if (condition.Pass(ref PointsList)) {
                    return condition.Redirect;
                }
            }
            if (DefaultRedirect != null) {
                return DefaultRedirect;
            }
            else {
                return null;
            }
        }

        public XElement ExportXML() {
            XElement xElement = new XElement("PointGate");

            if (DefaultRedirect != null) {
                xElement.Add(DefaultRedirect.ExportXML());
            }

            foreach (Condition condition in Conditions) {
                xElement.Add(condition.ExportXML());
            }

            return xElement;
        }
    }

    public class Condition {
        public string Type;
        public Redirect Redirect;
        public List<string> Expressions;

        public Condition(string type, Redirect redirect, List<string> expressions) {
            Type = type;
            Redirect = redirect;
            Expressions = expressions;
        }

        public Condition(XElement xElement) {
            Type = xElement.Attribute("Type").Value;
            Redirect = new Redirect(xElement.Element("Redirect"));
            Expressions = new List<string>();
            foreach (XElement expression in xElement.Elements("Expression").ToList()) {
                Expressions.Add(expression.Value);
            }
        }

        public bool Pass(ref Dictionary<string, Point> PointsList) {
            switch (Type.ToLower()) {
                case "and":
                    foreach (string Expression in Expressions) {
                        if (!TestExpression(Expression, ref PointsList)) {
                            return false;
                        }
                    }
                    return true;
                case "or":
                    foreach (string Expression in Expressions) {
                        if (TestExpression(Expression, ref PointsList)) {
                            return true;
                        }
                    }
                    return false;
                default:
                    Debug.Log("This condition has an invalid type:" + Expressions.ToString());
                    break;
            }
            return false;
        }

        public bool TestExpression(string expression, ref Dictionary<string, Point> PointsList) {
            string[] splitExpression = Regex.Split(expression, "(>=|<=|<>|!=|=|>|<)");
            List<int> Numbers = new List<int>();
            if (int.TryParse(splitExpression[0], out int result)) {
                Numbers.Add(result);
                Numbers.Add(PointsList[splitExpression[2]].Value);
            }
            else {
                Numbers.Add(PointsList[splitExpression[0]].Value);
                Numbers.Add(int.Parse(splitExpression[2]));
            }

            switch (splitExpression[1]) {
                case "=":
                    return Numbers[0] == Numbers[1];
                case "<":
                    return Numbers[0] < Numbers[1];
                case ">":
                    return Numbers[0] > Numbers[1];
                case ">=":
                    return Numbers[0] >= Numbers[1];
                case "<=":
                    return Numbers[0] <= Numbers[1];
                case "<>":
                case "!=":
                    return Numbers[0] != Numbers[1];
                default:
                    Debug.Log("This expression has an invalid operator:" + expression);
                    break;
            }

            return false;
        }

        // For the dialogue editor
        public XElement ExportXML() {
            XElement xElement = new XElement("Condition", new XAttribute("Type", Type), Redirect.ExportXML());

            foreach (string expression in Expressions) {
                xElement.Add(new XElement("Expression", expression));
            }

            return xElement;
        }
    }
}
