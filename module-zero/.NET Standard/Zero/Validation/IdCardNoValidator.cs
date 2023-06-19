using System;

namespace Zero.Validation
{
    /// <summary>
    /// ใช้ตรวจสอบเลขบัตรประชาชน
    /// Ref: https://th.wikipedia.org/wiki/เลขประจำตัวประชาชนไทย
    /// </summary>
    public class IdCardNoValidator
    {
        /// <summary>
        /// ใช้ตรวจสอบเลขบัตรประชาชน
        /// </summary>
        /// <param name="idcardno">เลขบัตรประชาชน 13 หลัก</param>
        /// <returns></returns>
        public bool Validate(string idcardno)
        {
            if (idcardno.Length != 13)
            {
                throw new ArgumentException("IdCardNo should be 13 length.");
            }
            var first12Digits = idcardno.Substring(0, 12);
            var lastDigit = idcardno.Substring(12, 1);

            return Validate(first12Digits, lastDigit);
        }

        /// <summary>
        /// ใช้ตรวจสอบเลขบัตรประชาชน
        /// </summary>
        /// <param name="first12Digits">เลขบัตรประชาชน 12 ตัวหน้า</param>
        /// <param name="lastDigit">เลขบัตรประชาชนตัวสุดท้าย</param>
        /// <returns></returns>
        public bool Validate(string first12Digits, string lastDigit)
        {
            var computedLastDigit = ComputeLastDigit(first12Digits);
            return computedLastDigit == lastDigit;
        }

        /// <summary>
        /// ใช้คำนวณค่าตัวเลขตัวสุดท้ายของเลขบัตรประชาชน
        /// </summary>
        /// <param name="first12Digits">เลขบัตรประชาชน 12 ตัวหน้า</param>
        /// <returns>เลขบัตรประชาชนตัวสุดท้าย</returns>
        public string ComputeLastDigit(string first12Digits)
        {
            if (first12Digits.Length != 12)
            {
                throw new ArgumentException("Input should be 12 length.");
            }

            int sumPriority = SummarizePriority(first12Digits);
            return GetLastDigit(sumPriority);
        }

        private int SummarizePriority(string first12Digits)
        {
            var digitCalculator = new IdCardNoDigitCalculator();
            int sum = 0;
            for (int i = 0; i < first12Digits.Length; i++)
            {
                string digit = first12Digits.Substring(i, 1);
                sum += digitCalculator.Calculate(Convert.ToInt32(digit), i + 1);
            }
            return sum;
        }

        private string GetLastDigit(int sumPriority)
        {
            int result = sumPriority % 11;
            if (result <= 1)
            {
                return (1 - result).ToString();
            }
            return (11 - result).ToString();
        }
    }
}