using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementBusinessLogic
{
    public class CommonValidations
    {
        public static bool isEmpty(params string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(input[i]) || input[i].Equals(""))
                    return true;
            }
            return false;
        }
    }
}
