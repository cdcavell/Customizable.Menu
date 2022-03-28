namespace ClassLibrary.Common
{
    public static class AsciiCodes
    {
        /// <value>(null)</value>
        public static string NUL { get; } = ToString(00);
        /// <value>(start of heading)</value>
        public static string SOH { get; } = ToString(01);
        /// <value>(start of text)</value>
        public static string STX { get; } = ToString(02);
        /// <value>(end of text)</value>
        public static string ETX { get; } = ToString(03);
        /// <value>(end of transmission)</value>
        public static string EOT { get; } = ToString(04);
        /// <value>(enquiry)</value>
        public static string ENQ { get; } = ToString(05);
        /// <value>(acknowledge)</value>
        public static string ACK { get; } = ToString(06);
        /// <value>(bell)</value>
        public static string BEL { get; } = ToString(07);
        /// <value>(backspace)</value>
        public static string BS { get; } = ToString(08);
        /// <value>(horizontal tab)</value>
        public static string TAB { get; } = ToString(09);
        /// <value>(NL line feed, new line)</value>
        public static string LF { get; } = ToString(10);
        /// <value>(vertical tab)</value>
        public static string VT { get; } = ToString(11);
        /// <value>(NP form feed, new page)</value>
        public static string FF { get; } = ToString(12);
        /// <value>(carriage return)</value>
        public static string CR { get; } = ToString(13);
        /// <value>(shift out)</value>
        public static string SO { get; } = ToString(14);
        /// <value>(shift in)</value>
        public static string SI { get; } = ToString(15);
        /// <value>(data link escape)</value>
        public static string DLE { get; } = ToString(16);
        /// <value>(device control 1)</value>
        public static string DC1 { get; } = ToString(17);
        /// <value>(device control 2)</value>
        public static string DC2 { get; } = ToString(18);
        /// <value>(device control 3)</value>
        public static string DC3 { get; } = ToString(19);
        /// <value>(device control 4)</value>
        public static string DC4 { get; } = ToString(20);
        /// <value>(negative acknowledge)</value>
        public static string NAK { get; } = ToString(21);
        /// <value>(synchronous idle)</value>
        public static string SYN { get; } = ToString(22);
        /// <value>(end of trans. block)</value>
        public static string ETB { get; } = ToString(23);
        /// <value>(cancel)</value>
        public static string CAN { get; } = ToString(24);
        /// <value>(end of medium)</value>
        public static string EM { get; } = ToString(25);
        /// <value>(substitute)</value>
        public static string SUB { get; } = ToString(26);
        /// <value>(escape)</value>
        public static string ESC { get; } = ToString(27);
        /// <value>(file separator)</value>
        public static string FS { get; } = ToString(28);
        /// <value>(group separator)</value>
        public static string GS { get; } = ToString(29);
        /// <value>(record separator)</value>
        public static string RS { get; } = ToString(30);
        /// <value>(unit separator)</value>
        public static string US { get; } = ToString(31);
        /// <value>(carriage return) + (NL line feed, new line)</value>
        public static string CRLF { get; } = CR + LF;

        public static char ToChar(int dec)
        {
            string hexValue = dec.ToString("X");
            return (Convert.ToChar(Convert.ToUInt32(hexValue, 16)));
        }

        public static string ToString(int dec)
        {
            return ToChar(dec).ToString();
        }
    }
}
