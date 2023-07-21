using System.Reflection;

namespace EqualsTwoClass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(IsEqualTwoClass("v18_2.DataListsType", "NDCv18_2_OfferRS.DataListsType", 0));

        }

        static bool IsEqualTwoClass(string class1, string class2, int level)
        {
            var knowTypes = new string[] { "System.", "Int32", "Boolean", "Int64", "Long", "Decimal", "Float", "Double", "Byte[]" };
            var paragraph = "";
            for (int l = 0; l < level; l++)
                paragraph += "\t";
            Console.WriteLine($"{paragraph}We chech {class1} and {class2}");
            
            // If get Arrays, we need drop squad brakets
            var class1Arr = class1.Split('.');
            var class2Arr = class2.Split('.');
            if (class1Arr.Length != class2Arr.Length) {
                Console.WriteLine($"Diff in name of internal clasess: {class1} and {class2}");
                return false;
            }
            if (class1Arr[class1Arr.Length - 1] != class2Arr[class2Arr.Length - 1])
            {
                Console.WriteLine($"Diff in last part of name of internal clasess: {class1} and {class2}");
                return false;
            }
            if (class1Arr[class1Arr.Length - 1].Contains("[]"))
            {
                class1Arr[class1Arr.Length - 1] = class1Arr[class1Arr.Length - 1].Substring(0, class1Arr[class1Arr.Length - 1].Length - 2);
                class2Arr[class2Arr.Length - 1] = class2Arr[class2Arr.Length - 1].Substring(0, class2Arr[class2Arr.Length - 1].Length - 2);
            }
            class1 = String.Join(".", class1Arr);
            class2 = String.Join(".", class2Arr);

            Type? type1 = Type.GetType(class1, false, true);
            if (type1 == null) { return false; }
            var membersType1 = type1.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Type? type2 = Type.GetType(class2, false, true);
            if (type2 == null) { return false; }
            var membersType2 = type2.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (membersType1.Length != membersType2.Length)
            {
                Console.WriteLine($"Diff in amount of fields in class: {class1} and {class2}");
                Console.WriteLine($"\tIn class {class1} next fields:");
                foreach (var mem in membersType1)
                    Console.WriteLine("\t\t" + mem.ToString());
                Console.WriteLine($"\tIn class {class2} next fields:");
                foreach (var mem in membersType2)
                    Console.WriteLine("\t\t" + mem.ToString());

                return false;
            }

            for (int i = 0; i < membersType1.Length; i++)
            {
                string[] strArr = membersType1[i].ToString().Split(' ');
                string[] strArr2 = membersType2[i].ToString().Split(' ');
                //Console.WriteLine(i + " " + strArr[0] +  " " + strArr[1]);
                if (knowTypes.Where(x => strArr[0].Contains(x)).Count() > 0)
                {
                    // check name of field
                    if (strArr[1] != strArr2[1])
                    {
                        Console.WriteLine($"Diff in name of fields: {strArr[1]} and {strArr2[1]}");
                        return false;
                    }
                }
                else
                {
                    // this is side from class and repeat function
                    if (!IsEqualTwoClass(strArr[0], strArr2[0], level + 1))
                    {
                        Console.WriteLine($"Diff in classess: {strArr[0]} and {strArr2[0]}");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
