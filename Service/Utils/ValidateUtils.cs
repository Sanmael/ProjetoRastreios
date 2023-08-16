namespace Service.Utils
{
    public class ValidateUtils
    {
        private string _errorMessage;

        public ValidateUtils(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public void ValidateLength(string code, int size)
        {
            if (string.IsNullOrEmpty(code) || code.Length != size)
            {
                throw new PortalException(_errorMessage);
            }
        }
        public void ValidateSegment(string code, int index, int size, bool shouldBeNumber)
        {
            string segment = code.Substring(index, size);
            bool isNumber = int.TryParse(segment, out _);

            if (shouldBeNumber != isNumber)
            {
                throw new PortalException(_errorMessage);
            }
        }
    }
}
