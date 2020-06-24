using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VisualNovel {
    public class PointGate {
        public List<Condition> Conditions;
        public Redirect DefaultRedirect;

        public PointGate(List<Condition> conditions, Redirect defaultRedirect) {
            Conditions = conditions;
            DefaultRedirect = defaultRedirect;
        }

        public Redirect SolveGate() {
            foreach (Condition condition in Conditions) {
                if (condition.Pass()) {
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
    }

    public class Condition {
        private PersistentManagerScript persistent = PersistentManagerScript.Instance;
        public List<string> Expressions;
        public string Type;
        public Redirect Redirect;

        public Condition(List<string> expressions, string type, Redirect redirect) {
            Expressions = expressions;
            Type = type;
            Redirect = redirect;
        }

        public bool Pass() {
            switch (Type.ToLower()) {
                case "and":
                    foreach (string Expression in Expressions) {
                        if (!TestExpression(Expression))
                            return false;
                    }
                    return true;
                case "or":
                    foreach (string Expression in Expressions) {
                        if (TestExpression(Expression))
                            return true;
                    }
                    return false;
                default:
                    Debug.Log("This condition has an invalid type:" + Expressions.ToString());
                    break;
            }
            return false;
        }

        public bool TestExpression(string expression) {
            string[] splitExpression = Regex.Split(expression, "(>=|<=|<>|!=|=|>|<)");
            List<int> Numbers = new List<int>();
            if (int.TryParse(splitExpression[0], out int result)) {
                Numbers.Add(result);
                Numbers.Add(persistent.PointsList[splitExpression[2]].Value);
            }
            else {
                Numbers.Add(persistent.PointsList[splitExpression[0]].Value);
                Numbers.Add(int.Parse(splitExpression[2]));
            }

            switch (splitExpression[1]) {
                case "=":
                    if (Numbers[0] == Numbers[1])
                        return true;
                    else
                        return false;
                case "<":
                    if (Numbers[0] < Numbers[1])
                        return true;
                    else
                        return false;
                case ">":
                    if (Numbers[0] > Numbers[1])
                        return true;
                    else
                        return false;
                case ">=":
                    if (Numbers[0] >= Numbers[1])
                        return true;
                    else
                        return false;
                case "<=":
                    if (Numbers[0] <= Numbers[1])
                        return true;
                    else
                        return false;
                case "<>":
                case "!=":
                    if (Numbers[0] != Numbers[1])
                        return true;
                    else
                        return false;
                default:
                    Debug.Log("This expression has an invalid operator:" + expression);
                    break;
            }

            return false;
        }
    }
}
