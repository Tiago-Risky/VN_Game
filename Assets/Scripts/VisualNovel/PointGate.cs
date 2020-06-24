using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VisualNovel {
    public class PointGate {
        //TODO
        public PointGate(List<(Condition, Redirect)> conditions, Redirect defaultRedirect) {
            //TODO
        }

        public Redirect SolveGate() {
            //TODO
            return new Redirect(-1, -1);
        }
    }

    public class Condition {
        private PersistentManagerScript persistent = PersistentManagerScript.Instance;
        List<string> Expressions;
        string Type;

        public Condition(List<string> expressions, string type) {
            Expressions = expressions;
            Type = type;
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
            string symbol = Regex.Match(expression, "(=|>|<|>=|<=|<>|!=)").ToString();
            string[] splitExpression = Regex.Split(expression, "(=|>|<|>=|<=|<>|!=)");
            List<int> Numbers = new List<int>();
            if (int.TryParse(splitExpression[0], out int result)) {
                Numbers.Add(result);
                Numbers.Add(persistent.PointsList[splitExpression[1]].Value);
            }
            else {
                Numbers.Add(persistent.PointsList[splitExpression[0]].Value);
                Numbers.Add(int.Parse(splitExpression[1]));
            }

            switch (symbol) {
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
