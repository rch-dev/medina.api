using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medina.Api
{
    public class MedinaSiteSectorId
    {
        public string Letter { get; set; }
        public string Number { get; set; }

        /// <summary>
        /// Generate id from three character string.
        /// </summary>
        /// <param name="id">Unformatted id. Ex: "G13"</param>
        public MedinaSiteSectorId(string id)
        {
            if (id.Length != 3)
            {
                throw new Exception("Invalid id format. String must be exactly three characters. Ex: 'M27'");
            };

            Letter = id.Substring(0, 1);
            Number = id.Substring(1);
        }

        /// <summary>
        /// Generate id from formatted parts.
        /// </summary>
        /// <param name="letter">Single character column identifier. If a longer string is provided, only the first letter will be used.</param>
        /// <param name="number">Integer row identifier. Will be represented with two digits. If a value greater than 99 is provided, it will be replaced with 99.</param>
        public MedinaSiteSectorId(string letter, int number)
        {
            Letter = letter.Substring(0, 1);

            if (number > 99) number = 99;

            Number = number.ToString().PadLeft(2, '0');
        }

        public override string ToString()
        {
            return $"{Letter}{Number}";
        }
    }
}