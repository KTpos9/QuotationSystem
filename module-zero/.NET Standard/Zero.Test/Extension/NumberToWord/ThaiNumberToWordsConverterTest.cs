using Xunit;
using Zero.Extension.NumberToWord;

namespace Zero.Test.Extension.NumberToWord
{
    public class ThaiNumberToWordsConverterTest
    {
        [Fact]
        public void Convert_Unit()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("ศูนย์", converter.Convert(0));
            Assert.Equal("หนึ่ง", converter.Convert(1));
            Assert.Equal("สอง", converter.Convert(2));
            Assert.Equal("สาม", converter.Convert(3));
            Assert.Equal("สี่", converter.Convert(4));
            Assert.Equal("ห้า", converter.Convert(5));
            Assert.Equal("หก", converter.Convert(6));
            Assert.Equal("เจ็ด", converter.Convert(7));
            Assert.Equal("แปด", converter.Convert(8));
            Assert.Equal("เก้า", converter.Convert(9));
            Assert.Equal("สิบ", converter.Convert(10));
            Assert.Equal("สิบเอ็ด", converter.Convert(11));
            Assert.Equal("สิบสอง", converter.Convert(12));
            Assert.Equal("สิบเก้า", converter.Convert(19));
            Assert.Equal("ยี่สิบ", converter.Convert(20));
            Assert.Equal("ยี่สิบเอ็ด", converter.Convert(21));
            Assert.Equal("ยี่สิบสอง", converter.Convert(22));
            Assert.Equal("สามสิบ", converter.Convert(30));
            Assert.Equal("สามสิบเอ็ด", converter.Convert(31));
            Assert.Equal("สี่สิบ", converter.Convert(40));
            Assert.Equal("เก้าสิบ", converter.Convert(90));
            Assert.Equal("เก้าสิบเก้า", converter.Convert(99));
        }

        [Fact]
        public void Convert_100()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("หนึ่งร้อย", converter.Convert(100));
            Assert.Equal("หนึ่งร้อยหนึ่ง", converter.Convert(101));
            Assert.Equal("หนึ่งร้อยสิบ", converter.Convert(110));
            Assert.Equal("หนึ่งร้อยสิบเอ็ด", converter.Convert(111));
            Assert.Equal("สองร้อย", converter.Convert(200));
            Assert.Equal("สามร้อย", converter.Convert(300));
            Assert.Equal("เก้าร้อย", converter.Convert(900));
            Assert.Equal("เก้าร้อยเก้าสิบเก้า", converter.Convert(999));
        }

        [Fact]
        public void Convert_1000()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("หนึ่งพัน", converter.Convert(1000));
            Assert.Equal("หนึ่งพันหนึ่งร้อยหนึ่ง", converter.Convert(1101));
            Assert.Equal("สองพันหนึ่งร้อยสิบ", converter.Convert(2110));
            Assert.Equal("สามพันหนึ่งร้อยสิบเอ็ด", converter.Convert(3111));
            Assert.Equal("สี่พันสองร้อย", converter.Convert(4200));
            Assert.Equal("ห้าพันสามร้อย", converter.Convert(5300));
            Assert.Equal("เก้าพันเก้าร้อย", converter.Convert(9900));
            Assert.Equal("เก้าพันเก้าร้อยเก้าสิบเก้า", converter.Convert(9999));
        }

        [Fact]
        public void Convert_10000()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("หนึ่งหมื่น", converter.Convert(10000));
            Assert.Equal("หนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด", converter.Convert(11111));
            Assert.Equal("เก้าหมื่นเก้าพันเก้าร้อยเก้าสิบเก้า", converter.Convert(99999));
        }

        [Fact]
        public void Convert_100000()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("หนึ่งแสน", converter.Convert(100000));
            Assert.Equal("หนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด", converter.Convert(111111));
            Assert.Equal("เก้าแสนเก้าหมื่นเก้าพันเก้าร้อยเก้าสิบเก้า", converter.Convert(999999));
        }

        [Fact]
        public void Convert_1000000()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("หนึ่งล้าน", converter.Convert(1000000));
            Assert.Equal("หนึ่งล้านหนึ่งแสนหนึ่งหมื่นหนึ่งพันหนึ่งร้อยสิบเอ็ด", converter.Convert(1111111));
            Assert.Equal("เก้าล้านเก้าแสนเก้าหมื่นเก้าพันเก้าร้อยเก้าสิบเก้า", converter.Convert(9999999));
        }

        [Fact]
        public void Convert_Over1000000()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("สิบล้าน", converter.Convert(10000000));
            Assert.Equal("หนึ่งร้อยล้าน", converter.Convert(100000000));
            Assert.Equal("หนึ่งพันล้าน", converter.Convert(1000000000));
            Assert.Equal("หนึ่งหมื่นล้าน", converter.Convert(10000000000));
            Assert.Equal("หนึ่งแสนล้าน", converter.Convert(100000000000));
            Assert.Equal("หนึ่งล้านล้าน", converter.Convert(1000000000000));
            Assert.Equal("สิบล้านล้าน", converter.Convert(10000000000000));

            Assert.Equal("เก้าร้อยแปดสิบเจ็ดล้านหกแสนห้าหมื่นสี่พันสามร้อยยี่สิบเอ็ด", converter.Convert(987654321));
        }

        [Fact]
        public void Convert_Ordinal()
        {
            var converter = new ThaiNumberToWordsConverter();

            Assert.Equal("ที่ศูนย์", converter.ConvertToOrdinal(0));
            Assert.Equal("ที่หนึ่ง", converter.ConvertToOrdinal(1));
            Assert.Equal("ที่สอง", converter.ConvertToOrdinal(2));
            Assert.Equal("ที่สาม", converter.ConvertToOrdinal(3));
            Assert.Equal("ที่สี่", converter.ConvertToOrdinal(4));
            Assert.Equal("ที่ห้า", converter.ConvertToOrdinal(5));
            Assert.Equal("ที่หก", converter.ConvertToOrdinal(6));
            Assert.Equal("ที่เจ็ด", converter.ConvertToOrdinal(7));
            Assert.Equal("ที่แปด", converter.ConvertToOrdinal(8));
            Assert.Equal("ที่เก้า", converter.ConvertToOrdinal(9));
            Assert.Equal("ที่สิบ", converter.ConvertToOrdinal(10));
            Assert.Equal("ที่สิบเอ็ด", converter.ConvertToOrdinal(11));
            Assert.Equal("ที่สิบสอง", converter.ConvertToOrdinal(12));
            Assert.Equal("ที่สิบเก้า", converter.ConvertToOrdinal(19));
            Assert.Equal("ที่ยี่สิบ", converter.ConvertToOrdinal(20));
            Assert.Equal("ที่ยี่สิบเอ็ด", converter.ConvertToOrdinal(21));
            Assert.Equal("ที่ยี่สิบสอง", converter.ConvertToOrdinal(22));
            Assert.Equal("ที่สามสิบ", converter.ConvertToOrdinal(30));
            Assert.Equal("ที่สามสิบเอ็ด", converter.ConvertToOrdinal(31));
            Assert.Equal("ที่สี่สิบ", converter.ConvertToOrdinal(40));
            Assert.Equal("ที่เก้าสิบ", converter.ConvertToOrdinal(90));
            Assert.Equal("ที่เก้าสิบเก้า", converter.ConvertToOrdinal(99));
        }

        [Fact]
        public void Convert_ToThaiBaht()
        {
            Assert.Equal("เจ็ดหมื่นหกพันห้าร้อยสี่สิบสามบาท ยี่สิบเอ็ดสตางค์", ((decimal)76543.21).ToThaiBaht());
            Assert.Equal("ลบหกพันห้าร้อยสี่สิบสามบาท ยี่สิบเอ็ดสตางค์", ((decimal)-6543.21).ToThaiBaht());

            // auto round decimal place
            Assert.Equal("ลบหกพันห้าร้อยสี่สิบสามบาท ยี่สิบเอ็ดสตางค์", ((decimal)-6543.214).ToThaiBaht());
            Assert.Equal("ลบหกพันห้าร้อยสี่สิบสามบาท ยี่สิบสองสตางค์", ((decimal)-6543.215).ToThaiBaht());
        }
    }
}