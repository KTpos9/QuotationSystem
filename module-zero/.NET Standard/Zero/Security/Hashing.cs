namespace Zero.Security
{
    public class Hashing
    {
        public string ToMd5(string value)
        {
            return new Md5().ComputeHash(value);
        }

        public string ToSha256(string value)
        {
            return new Sha256().ComputeHash(value);
        }
    }
}